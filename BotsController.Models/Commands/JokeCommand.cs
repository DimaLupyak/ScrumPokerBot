using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Telegram.Bot;
using Telegram.Bot.Types;
using File = System.IO.File;

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
            var allJokes = File.ReadAllText(@"Files\jokes.txt").Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            return allJokes[random.Next(allJokes.Length)];
        }
    }
}