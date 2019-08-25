using System;
using System.Threading.Tasks;
using TelegramBotApi;

namespace HappyDarioBot
{
    public class BadCommandFormat : IDarioBotReply
    {
        public string ErrorMessage { get; }

        public BadCommandFormat(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public DarioBotReplyEnum Type => DarioBotReplyEnum.BadCommand;
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