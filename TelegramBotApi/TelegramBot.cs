using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace TelegramBotApi
{
    public class TelegramBot
    {
        private readonly string _botToken;
        private readonly TelegramBotClient _telegramBotClient;

        public TelegramBot(string botToken)
        {
            _botToken = botToken;
            _telegramBotClient = new TelegramBotClient(_botToken);
        }

        public async Task SendMessage(int fromId, string message)
        {
            await _telegramBotClient.SendTextMessageAsync(fromId, message, ParseMode.Html);
        }

        public async Task SendAudioMessage(int fromId, Stream filestream){
            await _telegramBotClient.SendAudioAsync(fromId, new InputOnlineFile(filestream));
        }
    }
}
