namespace HappyDarioBot.Dto.Webhook.In
{
    public class TelegramMessage
    {
        public int MessageId { get; set; }
        public TelegramFrom From { get; set; }
        public TelegramChat Chat { get; set; }
        public int Date { get; set; }
        public string Text { get; set; }
    }
}