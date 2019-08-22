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

        public T HasAnAudio<T>(string name, Func<byte[], T> eitherRight, Func<T> eitherLeft)
        {
            NameMatcher nameMatcher = new NameMatcher(Directory.GetFiles(_resourcesPath));
            var filename = nameMatcher.Match(name);
            if (File.Exists(filename))
            {
                var bytes = File.ReadAllBytes(filename);
                return eitherRight(bytes);
            }

            return eitherLeft();
        }
    }
}