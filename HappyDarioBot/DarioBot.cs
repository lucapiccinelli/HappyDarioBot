using System;
using System.IO;
using System.Linq;
using HappyDarioBot.Dto.Webhook.In;
using TelegramBotApi;

namespace HappyDarioBot
{
    public class DarioBot
    {
        private readonly TelegramBot _telegramApi;
        private readonly int _forwardId;
        private readonly IDarioBotRepository _repository;

        public DarioBot(string botToken, string toId, IDarioBotRepository darioBotRepository)
        {
            _repository = darioBotRepository;
            _forwardId = Int32.Parse(toId);
            _telegramApi = new TelegramBot(botToken);
        }

        public DarioBot(string botToken, string toId) : this(botToken, toId, new LocalFileRepository()) { }

        public IDarioBotReply ReplyBack(TelegramUpdate telegramMsg)
        {
            TelegramMessage message = telegramMsg.Message;
            TelegramCallbackQuery callbackQuery = telegramMsg.CallbackQuery;

            if (message != null)
            {
                string messageText = message.Text;
                return ReplyFor(telegramMsg, messageText);
            }
            else
            {
                return ReplyFor(callbackQuery);
            }
        }

        private IDarioBotReply ReplyFor(TelegramCallbackQuery callbackQuery)
        {
            if (callbackQuery.From.Id != _forwardId)
            {
                return new PrivateCommandDarioBotResponse();
            }

            var commandTokens = callbackQuery.Data.Split(' ');
            if (commandTokens.Length != 2)
            {
                return new BadCommandFormat($"Expected at least 1 command and 1 argument. Received: {callbackQuery.Data}");
            }
            if (!commandTokens.First().StartsWith("/"))
            {
                return new BadCommandFormat($"Not a command. A command should start with: {callbackQuery.Data}");
            }

            string command = commandTokens.First();
            string[] args = commandTokens.Skip(1).ToArray();
            if (command != TelegramBotConstants.SetNameCommand)
            {
                return new UnknownCommand($"Not a known command: {callbackQuery.Data}");
            }

            return new SetNameDarioBotResponse(args.First());
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
            return new ForwardDarioBotReply(_telegramApi, fromReplyMessage, toReplyMessage, fromId, _forwardId, name);
        }

        private IDarioBotReply ReplyWithAudio(TelegramUpdate telegramMsg, byte[] fileBytes)
        {;
            return new AudioDarioBotReply(_telegramApi, telegramMsg.Message.From.Id, fileBytes);
        }
    }
}