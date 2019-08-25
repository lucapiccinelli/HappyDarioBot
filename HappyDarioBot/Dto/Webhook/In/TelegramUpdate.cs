using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HappyDarioBot.Dto.Webhook.In
{
    public class TelegramUpdate
    {
        [JsonProperty("update_id")]
        public int UpdateId { get; set; }
        [JsonProperty("message")]
        public TelegramMessage Message { get; set; }
        [JsonProperty("callback_query")]
        public TelegramCallbackQuery CallbackQuery { get; set; }
    }
}
