using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using HappyDarioBot.Dto;
using HappyDarioBot.Dto.Webhook.In;
using TelegramBotApi;

namespace HappyDarioBot
{
    public class AudioUploadDarioBotReply : IDarioBotReply
    {
        private readonly TelegramBot _telegramApi;
        private readonly IDarioBotRepository _repository;
        private readonly TelegramFrom _from;
        private readonly TelegramVoice _messageVoice;
        public DarioBotReplyEnum Type => DarioBotReplyEnum.Ok;

        public AudioUploadDarioBotReply(
            TelegramBot telegramApi, 
            IDarioBotRepository repository, 
            TelegramFrom @from, 
            TelegramVoice messageVoice)
        {
            _telegramApi = telegramApi;
            _repository = repository;
            _from = @from;
            _messageVoice = messageVoice;
        }

        public void ApplyTo(IDarioBotReplyConsumer consumer)
        {
            consumer.Use(this);
        }

        public async Task SendBackReplay()
        {
            byte[] uploadedFile = await _telegramApi.Download(_messageVoice.fileId);
             _repository.Save(
                 uploadedFile, 
                 async savedTo => await _telegramApi.SendMessage(_from.Id, $"ok: saved to {savedTo}"),
                 async error => await _telegramApi.SendMessage(_from.Id, $"error: {error.Message}"));
            List<int> waitingList = _repository.GetWaitingList();

            using (MemoryStream ms = new MemoryStream(uploadedFile))
            {
                foreach (var telegramId in waitingList)
                {
                    await _telegramApi.SendAudioMessage(telegramId, ms);
                }
            }
        }
    }
}