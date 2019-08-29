using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.File;

namespace HappyDarioBot
{
    public class AzureFileRepository : IDarioBotRepository
    {
        private const string CurrentnameFile = "currentname";
        private const char WaitingListSeparator = ',';


        private readonly string _connectionString;
        private readonly string _resourcePath;
        private readonly string _repositoryReference;
        private readonly StringNormalizer _stringNormalizer;

        public AzureFileRepository(string connectionString, string resourcePath, string repositoryReference = "happydariobot")
        {
            _connectionString = connectionString;
            _resourcePath = resourcePath;
            _repositoryReference = repositoryReference;
            _stringNormalizer = new StringNormalizer();
        }

        public T HasAnAudio<T>(string name, Func<byte[], T> onExists, Func<T> onNotExists)
        {
            var audioFilesDir = GetAudioFileDirectory();

            if (audioFilesDir.Exists())
            {
                var allTheFiles = ListAllTheFiles(audioFilesDir);

                NameMatcher nameMatcher = new NameMatcher(allTheFiles);
                var filename = nameMatcher.Match(name);
                if(!string.IsNullOrWhiteSpace(filename))
                {
                    var fileToDownload = audioFilesDir.GetFileReference(filename);
                    if (fileToDownload.Exists())
                    {
                        return DownloadTheAudio(onExists, fileToDownload);
                    }
                }
            }
            return onNotExists();
        }

        public void SetCurrentAudioName(string name, Action onSuccess, Action<RepositoryError> onError)
        {
            Try(() =>
            {
                CloudBlobContainer container = GetContainer();
                CloudBlockBlob blob = container.GetBlockBlobReference(CurrentnameFile);

                blob.UploadText(name);
                onSuccess();
            }, onError);
        }

        public void Save(byte[] uploadedFile, Action<string> onSuccess, Action<RepositoryError> onError)
        {
            Try(() =>
            {
                string saveFilename = $"{GetFilename()}.mp3";

                var audioDirectory = GetAudioFileDirectory();
                var fileReference = audioDirectory.GetFileReference(saveFilename);
                fileReference.UploadFromByteArray(uploadedFile, 0, uploadedFile.Length);

                onSuccess(saveFilename);
            }, onError);
        }

        public void PushInWaitingList(int fromId, string name)
        {
            CloudBlockBlob blob = GetBlobFor(name);

            string waitingList = "";
            if (blob.Exists())
            {
                waitingList = $"{blob.DownloadText()}{WaitingListSeparator}";
            }
            waitingList = $"{waitingList}{fromId}";

            blob.UploadText(waitingList);
        }

        public void ClearWaitingList(string name)
        {
            CloudBlockBlob blob = GetBlobFor(name);
            blob.DeleteIfExists();
        }

        public List<int> GetWaitingList()
        {
            string currentnameFile = GetFilename();
            CloudBlockBlob waitingListBlob = GetBlobFor(currentnameFile);

            if (!waitingListBlob.Exists())
            {
                return new List<int>();
            }

            string waitingListString = waitingListBlob.DownloadText();
            List<int> waitingList =  waitingListString.Split(WaitingListSeparator)
                .Select(int.Parse)
                .ToList();


            waitingListBlob.DeleteIfExists();
            return waitingList;
        }

        private CloudBlockBlob GetBlobFor(string name)
        {
            string normalizedName = _stringNormalizer.Normalize(name);
            CloudBlobContainer container = GetContainer();
            CloudBlockBlob blob = container.GetBlockBlobReference(normalizedName);
            return blob;
        }

        public void Try(Action action, Action<RepositoryError> onError)
        {
            try
            {
                action();
            }
            catch (Exception e)
            {
                onError(new RepositoryError()
                {
                    Message = e.Message,
                    Exception = e
                });
            }
        }

        private string GetFilename()
        {
            var container = GetContainer();
            var currentname = container.GetBlockBlobReference(CurrentnameFile);
            return currentname.DownloadText();
        }

        public void Clean()
        {
            var container = GetContainer();
            container.DeleteIfExists();
        }

        private CloudBlobContainer GetContainer()
        {
            var storageAccount = CloudStorageAccount.Parse(_connectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(_repositoryReference);
            container.CreateIfNotExists(BlobContainerPublicAccessType.Off);
            return container;
        }

        private static T DownloadTheAudio<T>(Func<byte[], T> eitherRight, CloudFile fileToDownload)
        {
            byte[] fileBytes = null;
            using (MemoryStream ms = new MemoryStream())
            {
                fileToDownload.DownloadToStream(ms);
                fileBytes = ms.GetBuffer();
            }

            return eitherRight(fileBytes);
        }

        private static string[] ListAllTheFiles(CloudFileDirectory audioFilesDir)
        {
            return audioFilesDir
                .ListFilesAndDirectories()
                .Select(item => Uri.UnescapeDataString(item.Uri.Segments.Last()))
                .ToArray();
        }

        private CloudFileDirectory GetAudioFileDirectory()
        {
            var storageAccount = CloudStorageAccount.Parse(_connectionString);
            var fileClient = storageAccount.CreateCloudFileClient();
            var share = fileClient.GetShareReference(_repositoryReference);
            var rootDir = share.GetRootDirectoryReference();
            var sampleDir = rootDir.GetDirectoryReference(_resourcePath);
            return sampleDir;
        }
    }
}