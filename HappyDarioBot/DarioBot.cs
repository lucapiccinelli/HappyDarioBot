using System;
using System.IO;
using System.Linq;
using HappyDarioBot.Dto;
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

            if (callbackQuery != null)
            {
                return ReplyFor(callbackQuery);
            }
            else
            {
                if (message.Voice != null)
                {
                    return ReplyFor(message.From, message.Voice);
                }

                return ReplyFor(message);
            }
        }

        private IDarioBotReply ReplyFor(TelegramFrom @from, TelegramVoice messageVoice)
        {
            if (@from.Id != _forwardId)
            {
                return new PrivateCommandDarioBotResponse(@from, _telegramApi);
            }
            return new AudioUploadDarioBotReply(_telegramApi, _repository, @from, messageVoice);
        }

        private IDarioBotReply ReplyFor(TelegramCallbackQuery callbackQuery)
        {
            return HandleCommand(callbackQuery.Data, callbackQuery.From);
        }

        private IDarioBotReply ReplyFor(TelegramMessage telegramMsg)
        {
            if (IsACommand(telegramMsg.Text))
            {
                return HandleCommand(telegramMsg.Text, telegramMsg.From);
            }

            return _repository.HasAnAudio(telegramMsg.Text,
                audioBytes => ReplyWithAudio(telegramMsg, audioBytes),
                () => ReplyWithForwarded(telegramMsg));
        }

        private IDarioBotReply HandleCommand(string commandData, TelegramFrom @from)
        {
            string[] commandTokens = GetCommandTokens(commandData);
            if (commandTokens.Length != 2)
            {
                return new BadCommandFormatResponse(@from, _telegramApi, $"Expected at least 1 command and 1 argument. Received: {commandData}");
            }

            string theCommand = commandTokens.First();
            if (!IsACommand(theCommand))
            {
                return new BadCommandFormatResponse(@from, _telegramApi, $"Not a command. A command should start with: {commandData}");
            }

            string[] args = commandTokens.Skip(1).ToArray();
            return GetTheCommand(@from, theCommand, args);
        }

        private static string[] GetCommandTokens(string commandData)
        {
            return commandData.Split(' ');
        }

        private IDarioBotReply GetTheCommand(TelegramFrom from, string command, string[] args)
        {
            if (command == TelegramBotConstants.SetNameCommand)
            {
                if (from.Id != _forwardId)
                {
                    return new PrivateCommandDarioBotResponse(from, _telegramApi);
                }

                return new SetNameDarioBotResponse(_telegramApi, _repository, from, args.First());
            }

            return new UnknownCommand($"Not a known command: {command}, args: {args}");
        }

        private bool IsACommand(string commandText)
        {
            return commandText.StartsWith("/");
        }

        private IDarioBotReply ReplyWithForwarded(TelegramMessage telegramMsg)
        {
            var fromReplyMessage = "Tiocanta!!! Non ti conosco zio! Aspetta che ti faccio l'audio";
            var fromId = telegramMsg.From.Id;

            var name = telegramMsg.Text;
            string toReplyMessage = $"Dario, fammi un audio per {name}";
            return new ForwardDarioBotReply(_telegramApi, _repository, fromReplyMessage, toReplyMessage, fromId, _forwardId, name);
        }

        private IDarioBotReply ReplyWithAudio(TelegramMessage telegramMsg, byte[] fileBytes)
        {;
            return new AudioDarioBotReply(_telegramApi, telegramMsg.From.Id, fileBytes);
        }
    }
}