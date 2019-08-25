using Newtonsoft.Json;

namespace HappyDarioBot.Dto.Webhook.In
{
    public class TelegramCallbackQuery
    {
        public string Id { get; set; }
        public TelegramFrom From { get; set; }
        public TelegramMessage Message { get; set; }
        [JsonProperty("chat_instance")]
        public string ChatInstance { get; set; }
        public string Data { get; set; }
    }
}