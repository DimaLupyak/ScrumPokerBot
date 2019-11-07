using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BotsController.Core.Helpers;
using BotsController.Models.Commands;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace BotsController.Core.Commands
{
    public class DamnCommand : Command
    {
        public override string Name => @"обізви";
        private readonly string[] myNames = { "гріша", "гриша", "грішу", "гришу", "григорій", "grisha", "жора" };
        public override async Task ExecuteAsync(Message message, TelegramBotClient botClient)
        {
            try
            {
                var name = message.Text.ToLower().Replace(Name, string.Empty);
                string text;
                if (myNames.Any(grishaName => message.Text.ToLower().Contains(grishaName)) )
                {
                    text = "Пошел нахуй, не буду я себя обзывать. " + await GetDamn("ты").ConfigureAwait(false);
                }
                else
                {
                    text = await GetDamn(name).ConfigureAwait(false);
                }
                var speechGenerator = new SpeechGenerator();
                await using var stream = new MemoryStream(speechGenerator.SynthesizeSpeech(text));
                stream.Seek(0, SeekOrigin.Begin);
                await botClient.SendAudioAsync(message.Chat.Id, new InputOnlineFile(stream, text), text).ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Не буду обзиватись, сьогодні я хороший.").ConfigureAwait(true);
            }
        }

        static async Task<string> GetDamn(string name)
        {
            using var client = new HttpClient();
            using var response = await client.GetAsync("https://damn.ru/?name=" + name + "&sex=m").ConfigureAwait(true);
            using var content = response.Content;
            var myContent = await content.ReadAsStringAsync().ConfigureAwait(true);
            return GetDamnFromResponse(myContent);
        }

        private static string GetDamnFromResponse(string s)
        {
            if (!s.Contains("<div class=\"damn\""))
                return null;
            var start = s.IndexOf("<div class=\"damn\"", StringComparison.Ordinal);
            start = s.IndexOf(">", start + 1, StringComparison.Ordinal);
            start++;
            var end = s.IndexOf("</div", start, StringComparison.Ordinal);
            var withSpan = s.Substring(start, end - start);
            withSpan = withSpan.Replace("<span class=\"name\">", "");
            withSpan = withSpan.Replace("&mdash; ", "");
            return withSpan.Replace("</span>", "");
        }


    }
}