using System;
using System.Threading.Tasks;
using TelegramBotApi;

namespace HappyDarioBot
{
    public class SetNameDarioBotResponse : IDarioBotReply
    {
        public SetNameDarioBotResponse(string name)
        {
            Name = name;
        }

        public DarioBotReplyEnum Type => DarioBotReplyEnum.Ok;

        public string Name { get; set; }

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