using System;
using System.Threading.Tasks;
using HappyDarioBot.Dto.Webhook.In;
using TelegramBotApi;

namespace HappyDarioBot
{
    public class BadCommandFormatResponse : IDarioBotReply
    {
        private readonly TelegramFrom _from;
        private readonly TelegramBot _telegramApi;
        public string ErrorMessage { get; }

        public BadCommandFormatResponse(TelegramFrom @from, TelegramBot telegramApi, string errorMessage)
        {
            _from = @from;
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
            await _telegramApi.SendMessage(_from.Id, ErrorMessage);
        }
    }
}