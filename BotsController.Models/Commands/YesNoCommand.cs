using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotsController.Core.Commands
{
    public class YesNoCommand : Command
    {
        public override string Name => @"так чи ні";

        private readonly Random random = new Random();

        public override Task ExecuteAsync(Message message, TelegramBotClient botClient)
        {
            try
            {
                var yerNoResponse = GetResponse().Result;
                botClient.SendTextMessageAsync(message.Chat.Id, yerNoResponse.Answer);
                return botClient.SendAnimationAsync(message.Chat.Id, yerNoResponse.Image);
            }
            catch (Exception ex)
            {
                return botClient.SendTextMessageAsync(message.Chat.Id, ex.ToString());
            }
        }

        private async Task<YerNoResponse> GetResponse()
        {
            using var client = new HttpClient();
            using var response = await client.GetAsync(@"https://yesno.wtf/api");
            using var content = response.Content;
            var myContent = await content.ReadAsStringAsync();
            var data = (JArray)JsonConvert.DeserializeObject(myContent);
            return new YerNoResponse
            {
                Answer = data.SelectToken(".answer").ToString(),
                Image = data.SelectToken(".image").ToString()
            };
        }

        private struct YerNoResponse
        {
            public string Answer;
            public string Image;
        }
    }
}