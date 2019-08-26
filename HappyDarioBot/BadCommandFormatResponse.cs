using System;
using System.Threading.Tasks;
using HappyDarioBot.Dto.Webhook.In;
using TelegramBotApi;

namespace HappyDarioBot
{
    public class BadCommandFormatResponse : IDarioBotReply
    {
        private readonly TelegramCallbackQuery _callbackQuery;
        private readonly TelegramBot _telegramApi;
        public string ErrorMessage { get; }

        public BadCommandFormatResponse(TelegramCallbackQuery callbackQuery, TelegramBot telegramApi, string errorMessage)
        {
            _callbackQuery = callbackQuery;
            _telegramApi = telegramApi;
            ErrorMessage = errorMessage;
        }

        public DarioBotReplyEnum Type => DarioBotReplyEnum.BadCommand;
        public void ApplyTo(IDarioBotReplyConsumer consumer)
        {
            consumer.Use(this);
        }

        public async Task SendBackReplay()
        {
            await _telegramApi.SendMessage(_callbackQuery.From.Id, ErrorMessage);
        }
    }
}