using System;
using System.Threading.Tasks;
using HappyDarioBot.Dto.Webhook.In;
using TelegramBotApi;

namespace HappyDarioBot
{
    public class UnknownCommand : IDarioBotReply
    {
        private readonly TelegramFrom _from;
        private readonly TelegramBot _telegramApi;
        private readonly string _errorMessage;

        public UnknownCommand(TelegramFrom @from, TelegramBot telegramApi, string errorMessage)
        {
            _from = @from;
            _telegramApi = telegramApi;
            _errorMessage = errorMessage;
        }

        public DarioBotReplyEnum Type => DarioBotReplyEnum.UnknownCommand;
        public void ApplyTo(IDarioBotReplyConsumer consumer)
        {
            consumer.Use(this);
        }

        public async Task SendBackReplay()
        {
            await _telegramApi.SendMessage(_from.Id, _errorMessage);
        }
    }
}