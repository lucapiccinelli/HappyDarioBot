using System;

namespace HappyDarioBot
{
    public interface IDarioBotRepository
    {
        T HasAnAudio<T>(string messageText, Func<byte[], T> eitherRight, Func<T> eitherLeft);
    }
}