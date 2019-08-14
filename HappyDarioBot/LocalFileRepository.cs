using System;
using System.IO;

namespace HappyDarioBot
{
    public class LocalFileRepository
    {
        public T HasAnAudio<T>(string messageText, Func<byte[], T> eitherRight, Func<T> eitherLeft)
        {
            var filename = Path.Combine("Resources", $"{messageText}.aac");
            if (File.Exists(filename))
            {
                var bytes = File.ReadAllBytes(filename);
                return eitherRight(bytes);
            }

            return eitherLeft();
        }
    }
}