using System.Threading.Tasks;
using TelegramBotApi;

namespace HappyDarioBot
{
    public class ForwardDarioBotReply : IDarioBotReply
    {
        private readonly TelegramBot _telegramApi;
        public string ToReplyMessage { get; }
        public string FromReplyMessage { get; }
        public int FromId { get; }
        public int ForwardedToId { get; }
        public string Name { get; }

        public ForwardDarioBotReply(
            TelegramBot telegramApi, 
            string fromReplyMessage, 
            string toReplyMessage, 
            int fromId, 
            int forwardedToId,
            string name)
        {
            _telegramApi = telegramApi;
            ToReplyMessage = toReplyMessage;
            FromReplyMessage = fromReplyMessage;
            FromId = fromId;
            ForwardedToId = forwardedToId;
            Name = name;
        }

        public DarioBotReplyEnum Type => DarioBotReplyEnum.Forwarded;
        public void ApplyTo(IDarioBotReplyConsumer consumer)
        {
            consumer.Use(this);
        }

        public async Task SendBackReplay()
        {
            await _telegramApi.SendMessage(FromId, FromReplyMessage);
            await _telegramApi.SendMessage(ForwardedToId, ToReplyMessage);
        }
    }
}