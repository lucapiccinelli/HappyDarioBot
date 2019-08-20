using System;
using System.IO;
using Microsoft.WindowsAzure.Storage;

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

        public T HasAnAudio<T>(string messageText, Func<byte[], T> eitherRight, Func<T> eitherLeft)
        {
            var fileClient = _storageAccount.CreateCloudFileClient();
            var share = fileClient.GetShareReference("happydariobot");
            var rootDir = share.GetRootDirectoryReference();
            var sampleDir = rootDir.GetDirectoryReference(_resourcePath);

            if (sampleDir.Exists())
            {
                var fileToDownload = sampleDir.GetFileReference($"{messageText}.aac");
                if (fileToDownload.Exists())
                {
                    byte[] fileBytes = null;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        fileToDownload.DownloadToStream(ms);
                        fileBytes = ms.GetBuffer();
                    }
                    return eitherRight(fileBytes);
                }
            }
            return eitherLeft();
        }
    }
}