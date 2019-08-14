using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using HappyDarioBot;
using HappyDarioBot.Dto.Webhook.In;
using Newtonsoft.Json;
using Xunit;

namespace HappyDarioBotTests.Acceptance
{
    public class HappyDarioBotTests
    {
        private const String DarioRequest =
            "{\"update_id\":26554013,\r\n\"message\":{\"message_id\":2,\"from\":{\"id\":494523457,\"is_bot\":false,\"first_name\":\"Luca\",\"last_name\":\"Piccinelli\",\"language_code\":\"en\"},\"chat\":{\"id\":494523457,\"first_name\":\"Luca\",\"last_name\":\"Piccinelli\",\"type\":\"private\"},\"date\":1564981439,\"text\":\"banana DarioBot\"}}";
       
        public HappyDarioBotTests()
        {
            
        }


        [Fact]
        public async void DarioBotCanReceiveMessagesAndAnswer()
        {
            var request = TestFactory.CreateHttpRequest(HttpMethod.Post, "");
            var logger = TestFactory.CreateLogger();

            var requestPayload = JsonConvert.DeserializeObject<TelegramUpdate>(DarioRequest);
            request.Content = new ObjectContent<TelegramUpdate>(requestPayload, new JsonMediaTypeFormatter());

            var response = await HappyDarioBotFunction.Run(request, logger);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
