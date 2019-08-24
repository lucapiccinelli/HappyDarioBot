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

        private const String DarioAudioRequest =
            "{\"update_id\":26554033,\r\n\"message\":{\"message_id\":188,\"from\":{\"id\":494523457,\"is_bot\":false,\"first_name\":\"Luca\",\"last_name\":\"Piccinelli\",\"language_code\":\"en\"},\"chat\":{\"id\":494523457,\"first_name\":\"Luca\",\"last_name\":\"Piccinelli\",\"type\":\"private\"},\"date\":1566535682,\"voice\":{\"duration\":3,\"mime_type\":\"audio/ogg\",\"file_id\":\"AwADBAADbQYAAtQHAVMH6C2nlJBQnRYE\",\"file_size\":27277}}}";
       
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

            var response = await HappyDarioBot.HappyDarioBot.Run(request, logger);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
