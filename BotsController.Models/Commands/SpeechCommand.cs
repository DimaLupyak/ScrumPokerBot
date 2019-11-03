using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Amazon;
using Amazon.Polly;
using Amazon.Polly.Model;
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
                using var stream = new MemoryStream(GetSpeech(text));
                stream.Seek(0, SeekOrigin.Begin);
                return botClient.SendAudioAsync(message.Chat.Id, new InputOnlineFile(stream, "speech.mp3"), text);
            }
            catch (Exception ex)
            {
                return botClient.SendTextMessageAsync(message.Chat.Id, ex.ToString());
            }
        }

        private static byte[] GetSpeech(string text)
        {
            var pollyClient = new AmazonPollyClient(
                Environment.GetEnvironmentVariable("POLLY_KEY_ID"),
                Environment.GetEnvironmentVariable("POLLY_SECRET_KEY"),
                RegionEndpoint.EUWest2);

            var request = new SynthesizeSpeechRequest
            {
                Text = text,
                OutputFormat = OutputFormat.Mp3,
                VoiceId = VoiceId.Maxim
            };
            var result = pollyClient.SynthesizeSpeechAsync(request).Result;

            return StreamToBytes(result.AudioStream);
        }

        public static byte[] StreamToBytes(Stream input)
        {
            var buffer = new byte[16 * 1024];
            using var ms = new MemoryStream();
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                ms.Write(buffer, 0, read);
            }
            return ms.ToArray();
        }
    }
}