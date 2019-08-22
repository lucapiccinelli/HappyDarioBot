using System;

namespace HappyDarioBot
{
    public interface IDarioBotRepository
    {
        T HasAnAudio<T>(string name, Func<byte[], T> eitherRight, Func<T> eitherLeft);
    }
}