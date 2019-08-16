using System;
using System.IO;
using System.Threading.Tasks;
using HappyDarioBot.Dto.Webhook.In;
using TelegramBotApi;

namespace HappyDarioBot
{
    public class DarioBot
    {
        private readonly TelegramBot _telegramApi;
        private readonly int _toId;
        private readonly IDarioBotRepository _repository;

        public DarioBot(string botToken, string toId, IDarioBotRepository darioBotRepository)
        {
            _repository = darioBotRepository;
            _toId = Int32.Parse(toId);
            _telegramApi = new TelegramBot(botToken);
        }

        public DarioBot(string botToken, string toId) : this(botToken, toId, new LocalFileRepository()) { }

        public IDarioBotReply ReplyBack(TelegramUpdate telegramMsg)
        {
            string messageText = telegramMsg.Message.Text;
            return ReplyFor(telegramMsg, messageText);
        }

        private IDarioBotReply ReplyFor(TelegramUpdate telegramMsg, string messageText)
        {
            return _repository.HasAnAudio(messageText,
                audioBytes => ReplyWithAudio(telegramMsg, audioBytes),
                () => ReplyWithForwarded(telegramMsg));
        }

        private IDarioBotReply ReplyWithForwarded(TelegramUpdate telegramMsg)
        {
            var fromReplyMessage = "Tiocanta!!! Non ti conosco zio! Aspetta che ti faccio l'audio";
            var fromId = telegramMsg.Message.From.Id;

            var name = telegramMsg.Message.Text;
            string toReplyMessage = $"Dario, fammi un audio per {name}";
            return new ForwardDarioBotReply(_telegramApi, fromReplyMessage, toReplyMessage, fromId, _toId, name);
        }

        private IDarioBotReply ReplyWithAudio(TelegramUpdate telegramMsg, byte[] fileBytes)
        {;
            return new AudioDarioBotReply(_telegramApi, telegramMsg.Message.From.Id, fileBytes);
        }
    }
}