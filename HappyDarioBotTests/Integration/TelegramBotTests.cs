using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HappyDarioBot;
using TelegramBotApi;
using TelegramBotApi.Exception;
using Xunit;

namespace HappyDarioBotTests.Integration
{
    public class TelegramBotTests
    {
        private readonly TelegramBot _telegramBot;
        private string _botToken;
        private readonly string _resourcesPath;
        private const int MyId = 494523457;

        public TelegramBotTests()
        {
            _botToken = DarioBotConfiguration.Get(DarioBotConfiguration.BotTokenKey);
            _resourcesPath = DarioBotConfiguration.Get(DarioBotConfiguration.ResourcesPathKey);
            _telegramBot = new TelegramBot(_botToken);
        }

        [Fact]
        public async void CanSend_Text()
        {
            await _telegramBot.SendMessage(MyId, "ciao!");
        }

        [Fact]
        public async void CanSend_Audio()
        {
            var filename = "Gesù.aac";
            var filepath = Path.Combine(_resourcesPath, filename);
            var audiobytes = File.ReadAllBytes(filepath);
            using (MemoryStream ms = new MemoryStream(audiobytes))
            {
                await _telegramBot.SendAudioMessage(MyId, ms);
            }
        }

        [Fact]
        public async void SendAudio_WithAnError_ItThrows_AnException()
        {
            var filename = "Gesù.aac";
            var filepath = Path.Combine(_resourcesPath, filename);
            var audiobytes = File.ReadAllBytes(filepath);
            using (MemoryStream ms = new MemoryStream(audiobytes))
            {
                await Assert.ThrowsAsync<TelegramApiException>(() => _telegramBot.SendAudioMessage(0, ms));
            }
        }

        [Fact]
        public async void WithAnError_ItThrows_AnException()
        {
            await Assert.ThrowsAsync<TelegramApiException>(() => _telegramBot.SendMessage(0, "ciao!"));
        }
    }
}
