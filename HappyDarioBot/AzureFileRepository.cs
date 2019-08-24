using System;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.File;

namespace HappyDarioBot
{
    public class AzureFileRepository : IDarioBotRepository
    {
        private readonly CloudStorageAccount _storageAccount;
        private readonly string _resourcePath;

        public AzureFileRepository(string connectionString, string resourcePath)
        {
            _storageAccount = CloudStorageAccount.Parse(connectionString);
            _resourcePath = resourcePath;
        }

        public T HasAnAudio<T>(string name, Func<byte[], T> eitherRight, Func<T> eitherLeft)
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
                        return DownloadTheAudio(eitherRight, fileToDownload);
                    }
                }
            }
            return eitherLeft();
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
            var fileClient = _storageAccount.CreateCloudFileClient();
            var share = fileClient.GetShareReference("happydariobot");
            var rootDir = share.GetRootDirectoryReference();
            var sampleDir = rootDir.GetDirectoryReference(_resourcePath);
            return sampleDir;
        }
    }
}