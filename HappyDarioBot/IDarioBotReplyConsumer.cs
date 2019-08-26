namespace HappyDarioBot
{
    public interface IDarioBotReplyConsumer
    {
        void Use(ForwardDarioBotReply reply);
        void Use(AudioDarioBotReply reply);
        void Use(SetNameDarioBotResponse reply);
        void Use(PrivateCommandDarioBotResponse reply);
        void Use(BadCommandFormatResponse reply);
        void Use(UnknownCommand reply);
    }
}