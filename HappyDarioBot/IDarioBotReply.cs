using System.Threading.Tasks;
using TelegramBotApi;

namespace HappyDarioBot
{
    public interface IDarioBotReply
    {
        DarioBotReplyEnum Type { get; }
        void ApplyTo(IDarioBotReplyConsumer consumer);
        Task SendBackReplay();
    }
}