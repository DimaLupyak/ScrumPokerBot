using System;
using System.Net.Http;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotsController.Models.Commands
{
    public class DamnCommand : Command
    {
        public override string Name => @"обізви";

        public override Task ExecuteAsync(Message message, TelegramBotClient botClient)
        {
            try
            {
                string name = message.Text.Replace(Name, string.Empty);
                var joke = GetDamn(name).Result;
                return botClient.SendTextMessageAsync(message.Chat.Id, joke);
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