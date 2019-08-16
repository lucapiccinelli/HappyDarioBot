using System;
using System.IO;

namespace HappyDarioBot
{
    public class LocalFileRepository : IDarioBotRepository
    {
        private readonly string _resourcesPath;

        public LocalFileRepository(string resourcesPath = "Resources")
        {
            _resourcesPath = resourcesPath;
        }

        public T HasAnAudio<T>(string messageText, Func<byte[], T> eitherRight, Func<T> eitherLeft)
        {
            var filename = Path.Combine(_resourcesPath, $"{messageText}.aac");
            if (File.Exists(filename))
            {
                var bytes = File.ReadAllBytes(filename);
                return eitherRight(bytes);
            }

            return eitherLeft();
        }
    }
}