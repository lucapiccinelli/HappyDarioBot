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
            string[] commandTokens = GetCommandTokens(callbackQuery);
            if (commandTokens.Length != 2)
            {
                return new BadCommandFormatResponse(callbackQuery, _telegramApi, $"Expected at least 1 command and 1 argument. Received: {callbackQuery.Data}");
            }

            string theCommand = commandTokens.First();
            if (!theCommand.StartsWith("/"))
            {
                return new BadCommandFormatResponse(callbackQuery, _telegramApi, $"Not a command. A command should start with: {callbackQuery.Data}");
            }

            string[] args = commandTokens.Skip(1).ToArray();
            return GetTheCommand(callbackQuery, theCommand, args);
        }

        private static string[] GetCommandTokens(TelegramCallbackQuery callbackQuery)
        {
            return callbackQuery.Data.Split(' ');
        }

        private IDarioBotReply GetTheCommand(TelegramCallbackQuery callbackQuery, string command, string[] args)
        {
            if (command == TelegramBotConstants.SetNameCommand)
            {
                if (callbackQuery.From.Id != _forwardId)
                {
                    return new PrivateCommandDarioBotResponse(callbackQuery, _telegramApi);
                }

                return new SetNameDarioBotResponse(_telegramApi, _repository, callbackQuery, args.First());
            }

            return new UnknownCommand($"Not a known command: {callbackQuery.Data}");
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