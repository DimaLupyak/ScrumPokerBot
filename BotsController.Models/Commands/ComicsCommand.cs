using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BotsController.Core.Helpers;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace BotsController.Core.Commands
{
    public class ComicsCommand : Command
    {
        public override string Name => @"комікс";
        public override Task ExecuteAsync(Message message, TelegramBotClient botClient)
        {
            try
            {
                return botClient.SendTextMessageAsync(message.Chat.Id, GetComics().Result);
            }
            catch (Exception ex)
            {
                return botClient.SendTextMessageAsync(message.Chat.Id, ex.ToString());
            }
        }

        static async Task<string> GetComics()
        {
            using var client = new HttpClient();
            using var response = await client.GetAsync("http://paintraincomic.com/?random&nocache=1").ConfigureAwait(true);
            using var content = response.Content;
            var myContent = await content.ReadAsStringAsync().ConfigureAwait(true);
            return FindComics(myContent);
        }

        private static string FindComics(string s)
        {
            if (!s.Contains("<div id=\"comic\">"))
                return null;
            var start = s.IndexOf("<div id=\"comic\">", StringComparison.Ordinal);
            var end = s.IndexOf("</div", start, StringComparison.Ordinal);
            var comicDiv = s[start..end];

            start = comicDiv.IndexOf("<img src", StringComparison.Ordinal);
            start = comicDiv.IndexOf("http", start, StringComparison.Ordinal);
            end = comicDiv.IndexOf("\" ", start, StringComparison.Ordinal);
            
            return comicDiv[start..end];
        }


    }
}