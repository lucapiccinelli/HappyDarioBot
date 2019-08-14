using System.IO;
using System.Threading.Tasks;
using TelegramBotApi;

namespace HappyDarioBot
{
    public class AudioDarioBotReply : IDarioBotReply
    {
        private readonly TelegramBot _telegramApi;
        private readonly int _fromId;
        private readonly byte[] _audioBytes;

        public AudioDarioBotReply(TelegramBot telegramApi, int fromId, byte[] audioBytes)
        {
            _telegramApi = telegramApi;
            _fromId = fromId;
            _audioBytes = audioBytes;
        }

        public DarioBotReplyEnum Type => DarioBotReplyEnum.Audio;
        public void ApplyTo(IDarioBotReplyConsumer consumer)
        {
            consumer.Use(this);
        }

        public async Task SendBackReplay()
        {
            using (MemoryStream ms = new MemoryStream(_audioBytes))
            {
                await _telegramApi.SendAudioMessage(_fromId, ms);
            }
        }
    }
}