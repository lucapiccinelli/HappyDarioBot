﻿using System;
using System.IO;
using System.Linq;
using System.Text;
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

        public T HasAnAudio<T>(string name, Func<byte[], T> eitherRight, Func<T> eitherLeft)
        {
            var fileClient = _storageAccount.CreateCloudFileClient();
            var share = fileClient.GetShareReference("happydariobot");
            var rootDir = share.GetRootDirectoryReference();
            var sampleDir = rootDir.GetDirectoryReference(_resourcePath);

            if (sampleDir.Exists())
            {
                var listFileItems = sampleDir
                    .ListFilesAndDirectories()
                    .Select(item => Uri.UnescapeDataString(item.Uri.Segments.Last()))
                    .ToArray();

                NameMatcher nameMatcher = new NameMatcher(listFileItems);
                var filename = nameMatcher.Match(name);
                if(!string.IsNullOrWhiteSpace(filename))
                {
                    var fileToDownload = sampleDir.GetFileReference(filename);
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
            }
            return eitherLeft();
        }
    }
}