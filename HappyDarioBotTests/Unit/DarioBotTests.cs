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
        private const string MyName = "Luca";


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

        [Fact]
        public void DarioBot_AnswersOk_IfForwardId_Asks_ToSet_AName_FromACallback()
        {
            IDarioBotReply reply = _darioBot.ReplyBack(new TelegramUpdate
            {
                CallbackQuery = new TelegramCallbackQuery()
                {
                    From = new TelegramFrom()
                    {
                        Id = MyId
                    },
                    Data = $"{TelegramBotConstants.SetNameCommand} {MyName}"
                }
            });


            Assert.Equal(DarioBotReplyEnum.Ok, reply.Type);
            reply.ApplyTo(this);
        }

        [Fact]
        public void DarioBot_AnswersKo_AnUnknowId_Asks_ToSet_AName_FromACallback()
        {
            IDarioBotReply reply = _darioBot.ReplyBack(new TelegramUpdate
            {
                CallbackQuery = new TelegramCallbackQuery()
                {
                    From = new TelegramFrom()
                    {
                        Id = 123456789
                    },
                    Data = $"{TelegramBotConstants.SetNameCommand} {MyName}"
                }
            });


            Assert.Equal(DarioBotReplyEnum.Ko, reply.Type);
        }

        public void Use(ForwardDarioBotReply reply)
        {
            Assert.Equal(MyId, reply.FromId);
            Assert.Equal(_forwardedId, reply.ForwardedToId.ToString());
        }


        public void Use(SetNameDarioBotResponse reply)
        {
            Assert.Equal(MyName, reply.Name);
        }

        public void Use(PrivateCommandDarioBotResponse reply)
        {
            throw new NotImplementedException();
        }

        public void Use(AudioDarioBotReply reply)
        {
            throw new NotImplementedException();
        }
    }
}
