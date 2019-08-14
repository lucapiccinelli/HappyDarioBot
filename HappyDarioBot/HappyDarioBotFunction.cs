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

            string botToken = DarioBotConfiguration.Get(DarioBotConfiguration.BotTokenKey);
            string toId = DarioBotConfiguration.Get(DarioBotConfiguration.ForwardToIdKey);

            DarioBot darioBot = new DarioBot(botToken, toId);
            IDarioBotReply reply = darioBot.ReplyBack(telegramMsg);
            await reply.SendBackReplay();

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
