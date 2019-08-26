using System;
using System.Threading.Tasks;
using HappyDarioBot.Dto.Webhook.In;
using TelegramBotApi;

namespace HappyDarioBot
{
    public class PrivateCommandDarioBotResponse : IDarioBotReply
    {
        private readonly TelegramCallbackQuery _callbackQuery;
        private readonly TelegramBot _telegramApi;

        public PrivateCommandDarioBotResponse(TelegramCallbackQuery callbackQuery, TelegramBot telegramApi)
        {
            _callbackQuery = callbackQuery;
            _telegramApi = telegramApi;
        }

        public DarioBotReplyEnum Type => DarioBotReplyEnum.Ko;
        public void ApplyTo(IDarioBotReplyConsumer consumer)
        {
            consumer.Use(this);
        }

        public async Task SendBackReplay()
        {
            await _telegramApi.SendMessage(_callbackQuery.From.Id, "No!");
        }
    }
}