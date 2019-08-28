using System;
using System.Threading.Tasks;
using HappyDarioBot.Dto.Webhook.In;
using TelegramBotApi;

namespace HappyDarioBot
{
    public class SetNameDarioBotResponse : IDarioBotReply
    {
        private readonly TelegramBot _telegramApi;
        private readonly IDarioBotRepository _repository;
        private readonly TelegramFrom _from;

        public SetNameDarioBotResponse(TelegramBot telegramApi, IDarioBotRepository repository, TelegramFrom @from, string name)
        {
            _telegramApi = telegramApi;
            _repository = repository;
            _from = @from;
            Name = name;
        }

        public DarioBotReplyEnum Type => DarioBotReplyEnum.Ok;

        public string Name { get; set; }

        public void ApplyTo(IDarioBotReplyConsumer consumer)
        {
            consumer.Use(this);
        }

        public async Task SendBackReplay()
        {
            _repository.SetCurrentAudioName(Name, 
                async () => await _telegramApi.SendMessage(_from.Id, "ok"),
                async error => await _telegramApi.SendMessage(_from.Id, error.Message));
        }
    }
}