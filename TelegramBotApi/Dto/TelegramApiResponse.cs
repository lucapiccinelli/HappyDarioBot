using System.IO;
using Newtonsoft.Json;
using RestSharp;

namespace TelegramBotApi.Dto
{
    public class TelegramApiResponse
    {
        public bool Ok { get; set; }
        public int ErrorCode { get; set; }
        public string Description { get; set; }
        public Result Result { get; set; }
        public byte[] FileBytes { get; set; }

    }

    public class Result
    {
        [JsonProperty("file_id")]
        public string FileId { get; set; }
        [JsonProperty("file_size")]
        public int FileSize { get; set; }
        [JsonProperty("file_path")]
        public string FilePath { get; set; }
    }

}