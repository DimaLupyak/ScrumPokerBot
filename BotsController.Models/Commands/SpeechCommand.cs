using System;
using System.IO;
using System.Threading.Tasks;
using BotsController.Core.Helpers;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace BotsController.Core.Commands
{
    public class SpeechCommand : Command
    {
        public override string Name => @"скажи";

        public override Task ExecuteAsync(Message message, TelegramBotClient botClient)
        {
            try
            {
                var text = message.Text
                    .ToLower()
                    .Replace(Name, string.Empty)
                    .Replace("\"", " ");
                var speechGenerator = new SpeechGenerator();
                using var stream = new MemoryStream(speechGenerator.SynthesizeSpeech(text));
                stream.Seek(0, SeekOrigin.Begin);
                return botClient.SendAudioAsync(message.Chat.Id, new InputOnlineFile(stream, text), text);
            }
            catch (Exception ex)
            {
                return botClient.SendTextMessageAsync(message.Chat.Id, ex.ToString());
            }
        }
    }
}