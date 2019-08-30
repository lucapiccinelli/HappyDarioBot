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

            try
            {
                string jsonContent = await req.Content.ReadAsStringAsync();
                log.LogInformation(jsonContent);
                TelegramUpdate telegramMsg = JsonConvert.DeserializeObject<TelegramUpdate>(jsonContent);
                TelegramFrom telegramFrom = GetFrom(telegramMsg);
                string message = GetMessage(telegramMsg);

                log.LogInformation($"received message from {telegramFrom.FirstName} {telegramFrom.LastName}. {message}");

                string botToken = DarioBotConfiguration.Get(DarioBotConfiguration.BotTokenKey);
                string toId = DarioBotConfiguration.Get(DarioBotConfiguration.ForwardToIdKey);
                string resourcesPath = DarioBotConfiguration.Get(DarioBotConfiguration.RemoteResourcesPathKey);
                string azureStorageConnectionString = DarioBotConfiguration.Get(DarioBotConfiguration.StorageConnectionStringKey);
                string azureStorage = DarioBotConfiguration.Get(DarioBotConfiguration.AzureStorageNameKey);
                log.LogInformation($"forwardId: {toId}");

                DarioBot darioBot = new DarioBot(
                    botToken, 
                    toId, 
                    new AzureFileRepository(azureStorageConnectionString, resourcesPath, azureStorage));

                IDarioBotReply reply = darioBot.ReplyBack(telegramMsg);
                log.LogInformation($"DarioBot reply to {telegramFrom.FirstName} {telegramFrom.LastName}. {message} --> {reply.Type}");

                await reply.SendBackReplay();
                log.LogInformation($"DarioBot replied with success");
            }
            catch (JsonSerializationException e)
            {
                log.LogError(e.Message);
                log.LogError(e.StackTrace);
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (Exception e)
            {
                log.LogError(e.Message);
                log.LogError(e.StackTrace);
                return req.CreateResponse(HttpStatusCode.InternalServerError);
            }

            return req.CreateResponse(HttpStatusCode.OK);
        }

        private static TelegramFrom GetFrom(TelegramUpdate telegramMsg) =>
            telegramMsg
                ?.Message?.From
                ?? telegramMsg?.CallbackQuery?.From;

        private static string GetMessage(TelegramUpdate telegramMsg) =>
            telegramMsg
                ?.Message?.Text
                ?? telegramMsg?.CallbackQuery?.Data
                ?? "";
    }
}
