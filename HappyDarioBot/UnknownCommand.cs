using System;
using System.Threading.Tasks;
using TelegramBotApi;

namespace HappyDarioBot
{
    public class UnknownCommand : IDarioBotReply
    {
        private readonly string _errorMessage;

        public UnknownCommand(string errorMessage)
        {
            _errorMessage = errorMessage;
        }

        public DarioBotReplyEnum Type => DarioBotReplyEnum.UnknownCommand;
        public void ApplyTo(IDarioBotReplyConsumer consumer)
        {
            consumer.Use(this);
        }

        public Task SendBackReplay()
        {
            throw new NotImplementedException();
        }
    }
}