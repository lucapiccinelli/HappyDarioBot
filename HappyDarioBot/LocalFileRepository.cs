using System;
using System.IO;
using System.Threading.Tasks;

namespace HappyDarioBot
{
    public class LocalFileRepository : IDarioBotRepository
    {
        private readonly string _resourcesPath;

        public LocalFileRepository(string resourcesPath = "Resources")
        {
            _resourcesPath = resourcesPath;
        }

        public T HasAnAudio<T>(string name, Func<byte[], T> onExists, Func<T> onNotExists)
        {
            NameMatcher nameMatcher = new NameMatcher(Directory.GetFiles(_resourcesPath));
            var filename = nameMatcher.Match(name);
            if (File.Exists(filename))
            {
                var bytes = File.ReadAllBytes(filename);
                return onExists(bytes);
            }

            return onNotExists();
        }

        public void SetCurrentAudioName(string name, Action onSuccess, Action<RepositoryError> onError)
        {
            throw new NotImplementedException();
        }

        public void Save(byte[] uploadedFile, Action<string> onSuccess, Action<RepositoryError> onError)
        {
            throw new NotImplementedException();
        }

        public void PushInWaitingList(int fromId, string name)
        {
            throw new NotImplementedException();
        }
    }
}