namespace HappyDarioBot.Dto.Webhook.In
{
    public class TelegramFrom
    {
        public int Id { get; set; }
        public bool IsBot { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LanguageCode { get; set; }
    }
}