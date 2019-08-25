using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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

        public TelegramBot(string botToken)
        {
            _botToken = botToken;
            _telegramBotClient = new RestClient($"https://api.telegram.org/bot{_botToken}");
        }

        public async Task SendMessage(int fromId, string message)
        {
            RestRequest request = new RestRequest("sendMessage");
            request.AddJsonBody(new 
            {
                chat_id = fromId,
                text = message,
                parse_mode = "HTML"
            });

            await SendRequestOrThrow(request);
        }
        public async Task SendInlineKeyboard(int fromId, string message)
        {
            RestRequest request = new RestRequest("sendMessage");
            request.AddJsonBody(new 
            {
                chat_id = fromId,
                text = message,
                parse_mode = "HTML",
                reply_markup = new 
                {
                    inline_keyboard = new[]
                    {
                        new []
                        {
                            new
                            {
                                text = message,
                                callback_data = $"{TelegramBotConstants.SetNameCommand} {message}"
                            }

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

        private async Task SendRequestOrThrow(RestRequest request)
        {
            IRestResponse<TelegramApiResponse> response = await _telegramBotClient.ExecutePostTaskAsync<TelegramApiResponse>(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new TelegramApiException(response);
            }
        }
    }
}
