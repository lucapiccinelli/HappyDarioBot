using System;
using System.Threading.Tasks;
using HappyDarioBot.Dto.Webhook.In;
using TelegramBotApi;

namespace HappyDarioBot
{
    public class PrivateCommandDarioBotResponse : IDarioBotReply
    {
        private readonly TelegramFrom _from;
        private readonly TelegramBot _telegramApi;

        public PrivateCommandDarioBotResponse(TelegramFrom @from, TelegramBot telegramApi)
        {
            _from = @from;
            _telegramApi = telegramApi;
        }

        public DarioBotReplyEnum Type => DarioBotReplyEnum.Ko;
        public void ApplyTo(IDarioBotReplyConsumer consumer)
        {
            consumer.Use(this);
        }

        public async Task SendBackReplay()
        {
            await _telegramApi.SendMessage(_from.Id, "No!");
        }
    }
}