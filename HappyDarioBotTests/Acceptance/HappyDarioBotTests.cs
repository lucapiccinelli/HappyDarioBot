using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using HappyDarioBot;
using HappyDarioBot.Dto.Webhook.In;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Xunit;

namespace HappyDarioBotTests.Acceptance
{
    public class HappyDarioBotTests
    {
        private const String DarioTextRequest =
            "{\"update_id\":26554013,\r\n\"message\":{\"message_id\":2,\"from\":{\"id\":494523457,\"is_bot\":false,\"first_name\":\"Luca\",\"last_name\":\"Piccinelli\",\"language_code\":\"en\"},\"chat\":{\"id\":494523457,\"first_name\":\"Luca\",\"last_name\":\"Piccinelli\",\"type\":\"private\"},\"date\":1564981439,\"text\":\"banana DarioBot\"}}";

        private const String DarioAudioUploadRequest =
            "{\"update_id\":26554033,\r\n\"message\":{\"message_id\":188,\"from\":{\"id\":494523457,\"is_bot\":false,\"first_name\":\"Luca\",\"last_name\":\"Piccinelli\",\"language_code\":\"en\"},\"chat\":{\"id\":494523457,\"first_name\":\"Luca\",\"last_name\":\"Piccinelli\",\"type\":\"private\"},\"date\":1566535682,\"voice\":{\"duration\":3,\"mime_type\":\"audio/ogg\",\"file_id\":\"AwADBAADbQYAAtQHAVMH6C2nlJBQnRYE\",\"file_size\":27277}}}";

        private const String DarioSetNameCallbackRequest =
            "{\"update_id\":26554040,\r\n\"callback_query\":{\"id\":\"2123962075205558993\",\"from\":{\"id\":494523457,\"is_bot\":false,\"first_name\":\"Luca\",\"last_name\":\"Piccinelli\",\"language_code\":\"en\"},\"message\":{\"message_id\":191,\"from\":{\"id\":959082501,\"is_bot\":true,\"first_name\":\"DarioBot\",\"username\":\"HappyDarioBot\"},\"chat\":{\"id\":494523457,\"first_name\":\"Luca\",\"last_name\":\"Piccinelli\",\"type\":\"private\"},\"date\":1566536656,\"text\":\"Luca\",\"reply_markup\":{\"inline_keyboard\":[[{\"text\":\"Luca\",\"callback_data\":\"/setname Luca\"}]]}},\"chat_instance\":\"4166611704352923468\",\"data\":\"/setname Luca\"}}";

        private const String DarioSetNameCommandRequest =
            "{\"update_id\":26554043,\r\n\"message\":{\"message_id\":202,\"from\":{\"id\":494523457,\"is_bot\":false,\"first_name\":\"Luca\",\"last_name\":\"Piccinelli\",\"language_code\":\"it\"},\"chat\":{\"id\":494523457,\"first_name\":\"Luca\",\"last_name\":\"Piccinelli\",\"type\":\"private\"},\"date\":1566711316,\"text\":\"/setname\",\"entities\":[{\"offset\":0,\"length\":8,\"type\":\"bot_command\"}]}}";

        private const String BadFormatRequest =
            "{\"message\":\"ciao\"}";


        [Theory]
        [InlineData(DarioTextRequest, HttpStatusCode.OK)]
        [InlineData(DarioSetNameCallbackRequest, HttpStatusCode.OK)]
        [InlineData(DarioSetNameCommandRequest, HttpStatusCode.OK)]
        [InlineData(DarioAudioUploadRequest, HttpStatusCode.OK)]
        public async void DarioBotCanHandleRequests(string darioRequest, HttpStatusCode expectedStatus)
        {
            var request = CreateRequest(out var logger, JsonConvert.DeserializeObject<TelegramUpdate>(darioRequest));

            var response = await HappyDarioBot.HappyDarioBot.Run(request, logger);
            Assert.Equal(expectedStatus, response.StatusCode);
        }

        [Fact]
        public async void DarioBotCanBadFormatRequest()
        {
            var request = TestFactory.CreateHttpRequest(HttpMethod.Post, "");
            var logger = TestFactory.CreateLogger();

            request.Content = new StringContent(BadFormatRequest);

            var response = await HappyDarioBot.HappyDarioBot.Run(request, logger);
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        private static HttpRequestMessage CreateRequest(out ILogger logger, TelegramUpdate requestPayload)
        {
            var request = TestFactory.CreateHttpRequest(HttpMethod.Post, "");
            logger = TestFactory.CreateLogger();
            ;
            request.Content = new ObjectContent<TelegramUpdate>(requestPayload, new JsonMediaTypeFormatter());
            return request;
        }
    }
}
