using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace TelegramBotApi
{
    public class TelegramBot
    {
        private readonly string _botToken;

        public TelegramBot(string botToken)
        {
            _botToken = botToken;
        }

        public async Task SendMessage(int fromId, string message)
        {
            TelegramBotClient telegramClient = new TelegramBotClient(_botToken);
            await telegramClient.SendTextMessageAsync(fromId, message, ParseMode.Html);
        }
    }
}
