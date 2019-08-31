using System;
using System.Threading.Tasks;
using HappyDarioBot.Dto.Webhook.In;
using TelegramBotApi;

namespace HappyDarioBot
{
    public class UnhandledInput : IDarioBotReply
    {
        private readonly TelegramBot _telegramApi;
        private readonly IDarioBotRepository _repository;
        private readonly TelegramFrom _messageFrom;

        public UnhandledInput(TelegramBot telegramApi, IDarioBotRepository repository, TelegramFrom messageFrom)
        {
            _telegramApi = telegramApi;
            _repository = repository;
            _messageFrom = messageFrom;
        }

        public DarioBotReplyEnum Type => DarioBotReplyEnum.UnhandledInput;
        public void ApplyTo(IDarioBotReplyConsumer consumer)
        {
            consumer.Use(this);
        }

        public async Task SendBackReplay()
        {
            await _telegramApi.SendMessage(_messageFrom.Id, "Cos'è sta roba?!?");
            await _repository.HasAnAudio("ammazzotutti",
                audiobytes => new AudioDarioBotReply(_telegramApi, _messageFrom.Id, audiobytes).SendBackReplay(),
                () => Task.Run(() => { }));
        }
    }
}