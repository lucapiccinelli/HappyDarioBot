using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HappyDarioBot;
using HappyDarioBot.Dto;
using HappyDarioBot.Dto.Webhook.In;
using TelegramBotApi;
using Xunit;

namespace HappyDarioBotTests.Unit
{
    public class DarioBotTests : IDarioBotReplyConsumer
    {
        private readonly DarioBot _darioBot;
        private readonly string _forwardedId;
        private readonly TelegramVoice _telegramVoiceExample;
        private const int MyId = 494523457;
        private const string MyName = "Luca";
        private const int UnknownId = 1235456789;
        private const int AnyoneId = 987654321;



        public DarioBotTests()
        {
            _darioBot = new DarioBot(
                DarioBotConfiguration.Get(DarioBotConfiguration.BotTokenKey),
                DarioBotConfiguration.Get(DarioBotConfiguration.ForwardToIdKey));
            _forwardedId = DarioBotConfiguration.Get(DarioBotConfiguration.ForwardToIdKey);

            _telegramVoiceExample = new TelegramVoice()
            {
                fileId = "AwADBAADbQYAAtQHAVMH6C2nlJBQnRYE",
                file_size = 27277,
                duration = 3
            };
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
        public void DarioBot_AnswersOk_IfForwardId_Asks_ToSet_ANameWithSpaces_FromACallback()
        {
            IDarioBotReply reply = _darioBot.ReplyBack(new TelegramUpdate
            {
                CallbackQuery = new TelegramCallbackQuery()
                {
                    From = new TelegramFrom()
                    {
                        Id = MyId
                    },
                    Data = $"{TelegramBotConstants.SetNameCommand} {MyName} Piccinelli"
                }
            });


            Assert.Equal(DarioBotReplyEnum.Ok, reply.Type);
            reply.ApplyTo(this);
        }

        [Fact]
        public void DarioBot_AnswersOk_IfForwardId_Asks_Ammazzotutti_FromACallback()
        {
            IDarioBotReply reply = _darioBot.ReplyBack(new TelegramUpdate
            {
                CallbackQuery = new TelegramCallbackQuery()
                {
                    From = new TelegramFrom()
                    {
                        Id = MyId
                    },
                    Data = $"{TelegramBotConstants.AmmazzoTuttiCommand} {MyId}"
                }
            });


            Assert.Equal(DarioBotReplyEnum.Ammazzotutti, reply.Type);
        }

        [Fact]
        public void DarioBot_AnswersOk_IfForwardId_Asks_ToSet_AName_FromAMessage()
        {
            IDarioBotReply reply = _darioBot.ReplyBack(new TelegramUpdate
            {
                Message = new TelegramMessage()
                {
                    From = new TelegramFrom()
                    {
                        Id = MyId
                    },
                    Text = $"{TelegramBotConstants.SetNameCommand} {MyName}"
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
                        Id = UnknownId
                    },
                    Data = $"{TelegramBotConstants.SetNameCommand} {MyName}"
                }
            });


            Assert.Equal(DarioBotReplyEnum.Ko, reply.Type);
        }

        [Theory]
        [InlineData("/unknown command", DarioBotReplyEnum.UnknownCommand)]
        [InlineData("/setname", DarioBotReplyEnum.BadCommand)]
        [InlineData("/ammazzotutti", DarioBotReplyEnum.BadCommand)]
        [InlineData("unknown command", DarioBotReplyEnum.BadCommand)]
        [InlineData("unknown", DarioBotReplyEnum.BadCommand)]
        public void DarioBot_AnswersKo_IfCallBackCommand_IsUnknown(string command, DarioBotReplyEnum expectedReply)
        {
            IDarioBotReply reply = _darioBot.ReplyBack(new TelegramUpdate
            {
                CallbackQuery = new TelegramCallbackQuery()
                {
                    From = new TelegramFrom()
                    {
                        Id = MyId
                    },
                    Data = command
                }
            });

            Assert.Equal(expectedReply, reply.Type);
        }

        [Fact]
        public void DarioBot_AnswersWelcome_ToStartCommand()
        {
            IDarioBotReply reply = _darioBot.ReplyBack(new TelegramUpdate
            {
                Message = new TelegramMessage()
                {
                    From = new TelegramFrom()
                    {
                        Id = AnyoneId
                    },
                    Text = $"{TelegramBotConstants.StartCommand}"
                }
            });


            Assert.Equal(DarioBotReplyEnum.Welcome, reply.Type);
        }

        [Fact]
        public void DarioBot_AnswersOk_WhenAudio_IsUploaded()
        {
            IDarioBotReply reply = _darioBot.ReplyBack(new TelegramUpdate
            {
                Message = new TelegramMessage
                {
                    From = new TelegramFrom()
                    {
                        Id = MyId
                    },
                    Voice = _telegramVoiceExample
                }
            });

            Assert.Equal(DarioBotReplyEnum.Ok, reply.Type);
        }

        [Fact]
        public void DarioBot_AnswersKo_WhenAudio_IsUploaded_ByAnUnknownId()
        {
            IDarioBotReply reply = _darioBot.ReplyBack(new TelegramUpdate
            {
                Message = new TelegramMessage
                {
                    From = new TelegramFrom()
                    {
                        Id = UnknownId
                    },
                    Voice = _telegramVoiceExample
                }
            });

            Assert.Equal(DarioBotReplyEnum.Ko, reply.Type);
        }

        [Fact]
        public void DarioBot_AnswersBadFormat_WhenTextIsNull()
        {
            IDarioBotReply reply = _darioBot.ReplyBack(new TelegramUpdate
            {
                Message = new TelegramMessage
                {
                    From = new TelegramFrom()
                    {
                        Id = UnknownId
                    }
                }
            });

            Assert.Equal(DarioBotReplyEnum.UnhandledInput, reply.Type);
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

        public void Use(BadCommandFormatResponse reply)
        {
            throw new NotImplementedException();
        }

        public void Use(UnknownCommand reply)
        {
            throw new NotImplementedException();
        }

        public void Use(AudioUploadDarioBotReply reply)
        {
            throw new NotImplementedException();
        }

        public void Use(WelcomeDarioBotResponse reply)
        {
            throw new NotImplementedException();
        }

        public void Use(UnhandledInput reply)
        {
            throw new NotImplementedException();
        }

        public void Use(AmmazzotuttiDarioBotResponse reply)
        {
            throw new NotImplementedException();
        }

        public void Use(AudioDarioBotReply reply)
        {
            throw new NotImplementedException();
        }
    }
}
