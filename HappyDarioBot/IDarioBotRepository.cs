using System;
using System.Threading.Tasks;

namespace HappyDarioBot
{
    public interface IDarioBotRepository
    {
        T HasAnAudio<T>(string name, Func<byte[], T> onSuccess, Func<T> onError);
        Task SetCurrentAudioName(string name, Action onSuccess, Action<RepositoryError> onError);
    }
}