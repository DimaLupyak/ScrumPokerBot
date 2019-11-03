using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.Polly;
using Amazon.Polly.Model;
using BotsController.Core.Helpers;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace BotsController.Models.Commands
{
    public class SpeechCommand : Command
    {
        public override string Name => @"скажи";

        public override Task ExecuteAsync(Message message, TelegramBotClient botClient)
        {
            try
            {
                var text = message.Text.ToLower().Replace(Name, string.Empty);
                var speechGenerator = new SpeechGenerator();
                botClient.SendTextMessageAsync(message.Chat.Id, text);
                return botClient.SendAudioAsync(message.Chat.Id, new InputOnlineFile(speechGenerator.SynthesizeSpeech(text), "speech.mp3"), text);
            }
            catch (Exception ex)
            {
                return botClient.SendTextMessageAsync(message.Chat.Id, ex.ToString());
            }
        }
    }
}