using System;
using System.Net.Http;
using System.Threading.Tasks;
using BotsController.Core.Helpers;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace BotsController.Models.Commands
{
    public class DamnCommand : Command
    {
        public override string Name => @"обізви";

        public override Task ExecuteAsync(Message message, TelegramBotClient botClient)
        {
            try
            {
                string name = message.Text.ToLower().Replace(Name, string.Empty);
                var text = GetDamn(name).Result;
                var speechGenerator = new SpeechGenerator();
                return botClient.SendAudioAsync(message.Chat.Id, new InputOnlineFile(speechGenerator.SynthesizeSpeech(text), text), text);
            }
            catch (Exception ex)
            {
                return botClient.SendTextMessageAsync(message.Chat.Id, "Не буду обзиватись, сьогодні я хороший.");
            }
        }

        static async Task<string> GetDamn(string name)
        {
            using var client = new HttpClient();
            using var response = await client.GetAsync("https://damn.ru/?name=" + name + "&sex=m");
            using var content = response.Content;
            var myContent = await content.ReadAsStringAsync();
            return GetDamnFromResponse(myContent);
        }

        private static String GetDamnFromResponse(String s)
        {
            if (!s.Contains("<div class=\"damn\""))
                return null;
            var start = s.IndexOf("<div class=\"damn\"");
            start = s.IndexOf(">", start + 1);
            start++;
            var end = s.IndexOf("</div", start);
            var withSpan = s.Substring(start, end - start);
            withSpan = withSpan.Replace("<span class=\"name\">", "");
            withSpan = withSpan.Replace("&mdash; ", "");
            return withSpan.Replace("</span>", "");
        }
    }
}