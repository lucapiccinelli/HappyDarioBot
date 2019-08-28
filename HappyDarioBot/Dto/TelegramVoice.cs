using Newtonsoft.Json;

namespace HappyDarioBot.Dto
{
    public class TelegramVoice
    {
        public int duration { get; set; }
        public string mime_type { get; set; }
        [JsonProperty("file_id")]
        public string fileId { get; set; }
        public int file_size { get; set; }
    }
}