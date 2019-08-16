namespace TelegramBotApi.Dto
{
    public class TelegramApiResponse
    {
        public bool Ok { get; set; }
        public int ErrorCode { get; set; }
        public string Description { get; set; }
    }
}