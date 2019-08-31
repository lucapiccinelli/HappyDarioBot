using System;
using System.Collections.Generic;
using System.Linq;
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
    public static class HappyDarioBot
    {
        private static ILogger _log = null;
        private static TelegramBot _telegramClient;
        private static TelegramUpdate _telegramUpdate;
        private static readonly List<string> MonitorReportList;
        private static int _logToId;

        static HappyDarioBot()
        {
            MonitorReportList = new List<string>();
            _logToId = 0;
        }

        [FunctionName("HappyDarioBot")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(WebHookType = "genericJson")]HttpRequestMessage req, ILogger log)
        {
            _log = log;
            log.LogInformation("DarioBot was triggered!");

            MonitorReportList.Clear();
            try
            {
                string botToken = DarioBotConfiguration.Get(DarioBotConfiguration.BotTokenKey);
                _logToId = int.Parse(DarioBotConfiguration.Get(DarioBotConfiguration.LogToIdKey));
                _telegramClient = new TelegramBot(botToken);

                string jsonContent = await req.Content.ReadAsStringAsync();
                log.LogInformation(jsonContent);
                _telegramUpdate = JsonConvert.DeserializeObject<TelegramUpdate>(jsonContent);
                TelegramFrom telegramFrom = GetFrom(_telegramUpdate);
                string message = GetMessage(_telegramUpdate);

                LogMessage($"received message from {telegramFrom?.FirstName} {telegramFrom?.LastName}. {message}");

                string toId = DarioBotConfiguration.Get(DarioBotConfiguration.ForwardToIdKey);
                string resourcesPath = DarioBotConfiguration.Get(DarioBotConfiguration.RemoteResourcesPathKey);
                string azureStorageConnectionString =
                    DarioBotConfiguration.Get(DarioBotConfiguration.StorageConnectionStringKey);
                string azureStorage = DarioBotConfiguration.Get(DarioBotConfiguration.AzureStorageNameKey);
                log.LogInformation($"forwardId: {toId}");

                DarioBot darioBot = new DarioBot(
                    botToken,
                    toId,
                    new AzureFileRepository(azureStorageConnectionString, resourcesPath, azureStorage));

                IDarioBotReply reply = darioBot.ReplyBack(_telegramUpdate);
                LogMessage(
                    $"DarioBot reply to {telegramFrom?.FirstName} {telegramFrom?.LastName}. {message} --> {reply.Type}");

                await reply.SendBackReplay();
                LogMessage($"DarioBot replied with success");
            }
            catch (JsonSerializationException e)
            {
                LogError(e.Message);
                LogError(e.StackTrace);
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (Exception e)
            {
                LogError(e.Message);
                LogError(e.StackTrace);
                return req.CreateResponse(HttpStatusCode.InternalServerError);
            }
            finally
            {
                SendReport();
            }

            return req.CreateResponse(HttpStatusCode.OK);
        }

        private static async void SendReport()
        {
            if (_telegramClient != null && _logToId != 0)
            {
                string report = string.Join(Environment.NewLine, MonitorReportList);
                await _telegramClient.SendMessage(_logToId, report);
            }
        }

        private static void LogMessage(string message)
        {
            _log.LogInformation(message);
            MonitorReportList.Add($"{_telegramUpdate?.UpdateId.ToString() ?? "No ID"} --> {message}");
        }
        private static void LogError(string message)
        {
            _log.LogError(message);
            MonitorReportList.Add($"{_telegramUpdate?.UpdateId.ToString() ?? "No ID"} --> ERROR!!! --> {message}");
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
