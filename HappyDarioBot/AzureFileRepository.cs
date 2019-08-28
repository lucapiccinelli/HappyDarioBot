using System;
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


        private readonly string _connectionString;
        private readonly string _resourcePath;
        private readonly string _repositoryReference;

        public AzureFileRepository(string connectionString, string resourcePath, string repositoryReference = "happydariobot")
        {
            _connectionString = connectionString;
            _resourcePath = resourcePath;
            _repositoryReference = repositoryReference;
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
                container.CreateIfNotExists(BlobContainerPublicAccessType.Off);
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

        public async Task Clean()
        {
            var container = GetContainer();
            await container.DeleteIfExistsAsync();
        }

        private CloudBlobContainer GetContainer()
        {
            var storageAccount = CloudStorageAccount.Parse(_connectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(_repositoryReference);
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