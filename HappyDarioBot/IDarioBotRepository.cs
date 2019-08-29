using System;
using System.Threading.Tasks;

namespace HappyDarioBot
{
    public interface IDarioBotRepository
    {
        T HasAnAudio<T>(string name, Func<byte[], T> onExists, Func<T> onNotExists);
        void SetCurrentAudioName(string name, Action onSuccess, Action<RepositoryError> onError);
        void Save(byte[] uploadedFile, Action<string> onSuccess, Action<RepositoryError> onError);
        void PushInWaitingList(int fromId, string name);
    }
}