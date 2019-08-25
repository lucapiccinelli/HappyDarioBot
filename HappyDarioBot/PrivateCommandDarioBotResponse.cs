using System;
using System.Threading.Tasks;
using TelegramBotApi;

namespace HappyDarioBot
{
    public class PrivateCommandDarioBotResponse : IDarioBotReply
    {
        public DarioBotReplyEnum Type => DarioBotReplyEnum.Ko;
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