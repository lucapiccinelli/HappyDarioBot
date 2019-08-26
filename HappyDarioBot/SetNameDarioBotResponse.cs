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
        private readonly TelegramCallbackQuery _callbackQuery;

        public SetNameDarioBotResponse(TelegramBot telegramApi, IDarioBotRepository repository, TelegramCallbackQuery callbackQuery, string name)
        {
            _telegramApi = telegramApi;
            _repository = repository;
            _callbackQuery = callbackQuery;
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
            await _repository.SetCurrentAudioName(Name, 
                async () => await _telegramApi.SendMessage(_callbackQuery.From.Id, "ok"),
                async error => await _telegramApi.SendMessage(_callbackQuery.From.Id, error.Message));
        }
    }
}