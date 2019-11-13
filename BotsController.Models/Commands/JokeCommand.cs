using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotsController.Core.Commands
{
    public class JokeCommand : Command
    {
        public override string Name => @"анекдот";

        private readonly Random random = new Random();

        public override Task ExecuteAsync(Message message, TelegramBotClient botClient)
        {
            try
            {
                var joke = GetJoke().Result;
                return botClient.SendTextMessageAsync(message.Chat.Id, joke);
            }
            catch (Exception ex)
            {
                return botClient.SendTextMessageAsync(message.Chat.Id, "У мене нема настрою для анекдоту, розважай себе сам.");
            }
        }

        private async Task<string> GetJoke()
        {
            using var client = new HttpClient();
            using var response = await client.GetAsync(@"http://umorili.herokuapp.com/api/get?num=50&name=bash");
            using var content = response.Content;
            var myContent = await content.ReadAsStringAsync();
            var data = (JArray)JsonConvert.DeserializeObject(myContent);
            var token = data.SelectToken($"$.[{random.Next(data.Count)}].elementPureHtml");
            var value = token as JValue;
            return value.Value<string>().Replace(@"<br />", "").Replace(@"&quot;", "");
        }
    }
}