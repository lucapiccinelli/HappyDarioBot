using RestSharp;
using TelegramBotApi.Dto;

namespace TelegramBotApi.Exception
{
    public class TelegramApiException : System.Exception
    {
        public TelegramApiException(IRestResponse<TelegramApiResponse> response) 
            : base($"Status: {response.StatusCode}, message: {response.Data?.Description ?? "No message"}")
        {
            
        }
    }
}