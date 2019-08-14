using System;
using System.Net;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using HappyDarioBot.Dto.Webhook.In;
using HappyDarioBot.Dto.Webhook.Out;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using TelegramBotApi;

namespace HappyDarioBot
{
    public static class HappyDarioBotFunction
    {
        [FunctionName("HappyDarioBot")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(WebHookType = "genericJson")]HttpRequestMessage req, ILogger log)
        {
            log.LogInformation("DarioBot was triggered!");

            string jsonContent = await req.Content.ReadAsStringAsync();
            log.LogDebug(jsonContent);
            TelegramUpdate telegramMsg = JsonConvert.DeserializeObject<TelegramUpdate>(jsonContent);

            var botToken = DarioBotConfiguration.Get("BotToken");
            TelegramBot telegramApi = new TelegramBot(botToken);
            await telegramApi.SendMessage(telegramMsg.Message.From.Id, "ciao");

            return req.CreateResponse(HttpStatusCode.OK);


            //if (data.first == null || data.last == null)
            //{
            //    return req.CreateResponse(HttpStatusCode.BadRequest, new
            //    {
            //        error = "Please pass first/last properties in the input object"
            //    });
            //}

            //return req.CreateResponse(HttpStatusCode.OK, new
            //{
            //    greeting = $"Hello {data.first} {data.last}!"
            //});
        }
    }
}
