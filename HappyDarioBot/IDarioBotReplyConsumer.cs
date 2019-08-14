namespace HappyDarioBot
{
    public interface IDarioBotReplyConsumer
    {
        void Use(ForwardDarioBotReply reply);
        void Use(AudioDarioBotReply reply);
    }
}