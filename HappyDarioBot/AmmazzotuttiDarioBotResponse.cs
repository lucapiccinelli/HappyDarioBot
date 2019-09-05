using System;
using System.IO;
using System.Threading.Tasks;
using HappyDarioBot.Dto.Webhook.In;
using TelegramBotApi;

namespace HappyDarioBot
{
    public class AmmazzotuttiDarioBotResponse : IDarioBotReply
    {
        private readonly IDarioBotRepository _repository;
        private readonly TelegramBot _telegramApi;
        private readonly TelegramFrom _from;
        private readonly int _forwardId;
        private readonly int _idToKill;

        public AmmazzotuttiDarioBotResponse(IDarioBotRepository repository, TelegramBot telegramApi, TelegramFrom @from, int forwardId, string idToKill)
        {
            _repository = repository;
            _telegramApi = telegramApi;
            _from = @from;
            _forwardId = forwardId;
            _idToKill = int.Parse(idToKill);
        }

        public DarioBotReplyEnum Type => DarioBotReplyEnum.Ammazzotutti;
        public void ApplyTo(IDarioBotReplyConsumer consumer)
        {
            consumer.Use(this);
        }

        public async Task SendBackReplay()
        {
            await _repository.HasAnAudio("ammazzotutti", 
                async audioBytes =>
                {
                    using (MemoryStream ms = new MemoryStream(audioBytes))
                    {
                        await _telegramApi.SendAudioMessage(_idToKill, ms);
                        await _telegramApi.SendMessage(_forwardId, "Sfanculato!");
                    }
                }, 
                async () =>  await _telegramApi.SendMessage(_idToKill, "Pelato di merda!!!"));
        }
    }
}