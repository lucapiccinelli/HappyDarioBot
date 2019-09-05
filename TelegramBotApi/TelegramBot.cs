using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using RestSharp;
using RestSharp.Extensions;
using TelegramBotApi.Dto;
using TelegramBotApi.Exception;

namespace TelegramBotApi
{
    public class TelegramBot
    {
        private readonly string _botToken;
        private readonly RestClient _telegramBotClient;
        private readonly RestClient _telegramBotFileClient;

        public TelegramBot(string botToken)
        {
            _botToken = botToken;
            _telegramBotClient = new RestClient($"https://api.telegram.org/bot{_botToken}");
            _telegramBotFileClient = new RestClient($"https://api.telegram.org/file/bot{_botToken}");
        }

        public async Task SendMessage(int fromId, string message)
        {
            RestRequest request = new RestRequest("sendMessage");
            request.AddJsonBody(new 
            {
                chat_id = fromId,
                text = ParseMessage(message),
                parse_mode = "HTML"
            });

            await SendRequestOrThrow(request);
        }

        public async Task SendInlineKeyboard(int fromId, int forwardId, string message, string buttonText)
        {
            RestRequest request = new RestRequest("sendMessage");
            request.AddJsonBody(new 
            {
                chat_id = forwardId,
                text = ParseMessage(message),
                parse_mode = "HTML",
                reply_markup = new 
                {
                    inline_keyboard = new[]
                    {
                        new []
                        {
                            new
                            {
                                text = ParseMessage(buttonText),
                                callback_data = $"{TelegramBotConstants.SetNameCommand} {buttonText}"
                            },
                            new
                            {
                                text = ParseMessage("Sfancula"),
                                callback_data = $"{TelegramBotConstants.AmmazzoTuttiCommand} {fromId}"
                            },

                        }
                    }
                }
            });

            await SendRequestOrThrow(request);
        }

        public async Task SendAudioMessage(int fromId, Stream filestream){
            RestRequest request = new RestRequest("sendAudio");
            request.Files.Add(new FileParameter()
            {
                Name = "audio",
                FileName = "HappyDario",
                ContentLength = filestream.Length,
                ContentType = "multipart/form-data",
                Writer = stream => stream.Write(filestream.ReadAsBytes(), 0, (int) filestream.Length)
            });
            request.AddParameter("chat_id", fromId);
            request.AddHeader("ContentType", "application/x-www-form-urlencoded");

            await SendRequestOrThrow(request);
        }

        public async Task<byte[]> Download(string fileId)
        {
            RestRequest request = new RestRequest("getFile");
            request.AddParameter("file_id", fileId);
            request.AddHeader("ContentType", "application/x-www-form-urlencoded");

            IRestResponse<TelegramApiResponse> response = await SendRequestOrThrow(request);

            request = new RestRequest(response.Data.Result.FilePath, Method.GET);
            return (await _telegramBotFileClient.ExecuteTaskAsync(request)).RawBytes;
        }

        private async Task<IRestResponse<TelegramApiResponse>> SendRequestOrThrow(RestRequest request, Method httpMethod = Method.POST)
        {
            return await SendRequestOrThrow(request, _telegramBotClient, httpMethod);
        }

        private static string ParseMessage(string message)
        {
            return HttpUtility.HtmlEncode(message);
        }

        private async Task<IRestResponse<TelegramApiResponse>> SendRequestOrThrow(RestRequest request, RestClient client, Method httpMethod = Method.POST)
        {
            IRestResponse<TelegramApiResponse> response = await _telegramBotClient.ExecuteTaskAsync<TelegramApiResponse>(request, httpMethod);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new TelegramApiException(response);
            }

            return response;
        }
    }
}
