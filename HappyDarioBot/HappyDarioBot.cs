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
    public static class HappyDarioBot
    {
        [FunctionName("HappyDarioBot")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(WebHookType = "genericJson")]HttpRequestMessage req, ILogger log)
        {
            log.LogInformation("DarioBot was triggered!");

            string jsonContent = await req.Content.ReadAsStringAsync();
            log.LogInformation(jsonContent);
            TelegramUpdate telegramMsg = JsonConvert.DeserializeObject<TelegramUpdate>(jsonContent);
            
            string botToken = DarioBotConfiguration.Get(DarioBotConfiguration.BotTokenKey);
            string toId = DarioBotConfiguration.Get(DarioBotConfiguration.ForwardToIdKey);
            string resourcesPath = DarioBotConfiguration.Get(DarioBotConfiguration.RemoteResourcesPathKey);
            string azureStorageConnectionString = DarioBotConfiguration.Get(DarioBotConfiguration.StorageConnectionStringKey);
            log.LogInformation($"resourcesPath: {resourcesPath}");

            if(telegramMsg.Message.Text == null)
            {
                return req.CreateResponse(HttpStatusCode.Accepted);
            }

            DarioBot darioBot = new DarioBot(botToken, toId, new AzureFileRepository(azureStorageConnectionString, resourcesPath));
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
