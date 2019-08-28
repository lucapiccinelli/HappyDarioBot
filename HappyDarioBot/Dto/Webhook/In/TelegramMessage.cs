using Newtonsoft.Json;

namespace HappyDarioBot.Dto.Webhook.In
{
    public class TelegramMessage
    {
        [JsonProperty("message_id")]
        public int MessageId { get; set; }
        public TelegramFrom From { get; set; }
        public TelegramChat Chat { get; set; }
        public int Date { get; set; }
        public string Text { get; set; }
        public TelegramVoice Voice { get; set; }
    }
}