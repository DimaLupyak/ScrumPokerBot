using BotsController.DAL;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace BotsController.Core.Commands
{
    public class TestCommand : Command
    {
        public override string Name => @"test";

        readonly Random random = new Random();
        public override async Task ExecuteAsync(Message message, TelegramBotClient client)
        {
            try
            {
                var repository = new Repository(
                    Environment.GetEnvironmentVariable("GRISHA_BOT_FIREBASE_AUTH"),
                    Environment.GetEnvironmentVariable("GRISHA_BOT_FIREBASE_URL"));

                await repository.AddPidarAsync("test");

                await client.SendTextMessageAsync(message.Chat.Id, repository.GetPidarAsync().Result);

            }
            catch (Exception ex)
            {
                await client.SendTextMessageAsync(message.Chat.Id, ex.ToString());
            }
        }
    }
}