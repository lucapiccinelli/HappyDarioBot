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

        public T HasAnAudio<T>(string name, Func<byte[], T> onSuccess, Func<T> onError)
        {
            NameMatcher nameMatcher = new NameMatcher(Directory.GetFiles(_resourcesPath));
            var filename = nameMatcher.Match(name);
            if (File.Exists(filename))
            {
                var bytes = File.ReadAllBytes(filename);
                return onSuccess(bytes);
            }

            return onError();
        }

        public Task SetCurrentAudioName(string name, Action onSuccess, Action<RepositoryError> onError)
        {
            throw new NotImplementedException();
        }
    }
}