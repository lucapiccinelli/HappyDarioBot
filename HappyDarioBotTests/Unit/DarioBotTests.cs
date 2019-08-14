using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HappyDarioBot;
using HappyDarioBot.Dto.Webhook.In;
using TelegramBotApi;
using Xunit;

namespace HappyDarioBotTests.Unit
{
    public class DarioBotTests : IDarioBotReplyConsumer
    {
        private readonly DarioBot _darioBot;
        private readonly string _forwardedId;
        private const int MyId = 494523457;

        public DarioBotTests()
        {
            _darioBot = new DarioBot(
                DarioBotConfiguration.Get(DarioBotConfiguration.BotTokenKey),
                DarioBotConfiguration.Get(DarioBotConfiguration.ForwardToIdKey));
            _forwardedId = DarioBotConfiguration.Get(DarioBotConfiguration.ForwardToIdKey);
        }

        [Fact]
        public void DarioBot_AnswerBack_AnAudio_IfIt_Has_TheFile()
        {
            IDarioBotReply reply = _darioBot.ReplyBack(new TelegramUpdate
            {
                Message = new TelegramMessage
                {
                    From = new TelegramFrom
                    {
                        Id = MyId
                    },
                    Text = "Gesù"
                }
            });

            Assert.Equal(DarioBotReplyEnum.Audio, reply.Type);
        }

        [Fact]
        public void DarioBot_AnswerBack_ForwardedReply_IfIt_DoesNotHas_TheFile()
        {
            IDarioBotReply reply = _darioBot.ReplyBack(new TelegramUpdate
            {
                Message = new TelegramMessage
                {
                    From = new TelegramFrom
                    {
                        Id = MyId
                    },
                    Text = "Baaaaa"
                }
            });


            Assert.Equal(DarioBotReplyEnum.Forwarded, reply.Type);
            reply.ApplyTo(this);
        }

        public void Use(ForwardDarioBotReply reply)
        {
            Assert.Equal(MyId, reply.FromId);
            Assert.Equal(_forwardedId, reply.ForwardedToId.ToString());
        }


        public void Use(AudioDarioBotReply reply)
        {
            throw new NotImplementedException();
        }
    }
}
