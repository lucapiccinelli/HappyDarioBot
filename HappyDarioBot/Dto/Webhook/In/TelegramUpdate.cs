using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HappyDarioBot.Dto.Webhook.In
{
    public class TelegramUpdate
    {
        public int UpdateId { get; set; }
        public TelegramMessage Message { get; set; }
    }
}
