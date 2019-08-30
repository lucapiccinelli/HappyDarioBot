using System;
using System.Threading.Tasks;
using HappyDarioBot.Dto.Webhook.In;
using TelegramBotApi;

namespace HappyDarioBot
{
    public class WelcomeDarioBotResponse : IDarioBotReply
    {
        private readonly TelegramBot _telegramApi;
        private readonly TelegramFrom _from;

        public WelcomeDarioBotResponse(TelegramBot telegramApi, TelegramFrom @from)
        {
            _telegramApi = telegramApi;
            _from = @from;
        }

        public DarioBotReplyEnum Type => DarioBotReplyEnum.Welcome;
        public void ApplyTo(IDarioBotReplyConsumer consumer)
        {
            consumer.Use(this);
        }

        public async Task SendBackReplay()
        {
            await _telegramApi.SendMessage(_from.Id, "Buongiornoooo!!! Scrivimi come ti chiami che ti faccio gli auguri!");
        }
    }
}