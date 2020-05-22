using BotsController.DAL;
using System;
using System.Linq;
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
                var repository = new Repository<Models.User>(
                    Environment.GetEnvironmentVariable("GRISHA_BOT_FIREBASE_AUTH"),
                    Environment.GetEnvironmentVariable("GRISHA_BOT_FIREBASE_URL"));

                var user = new Models.User()
                {
                    ChatId = message.Chat.Id.ToString(),
                    UserId = message.From.Id.ToString(),
                    DateTime = message.Date,
                    UserName = message.From.Username ?? message.From.FirstName + message.From.LastName
                };

                await repository.AddAsync(user);

                var allUsers = await repository.GetAllAsync().ConfigureAwait(false);

                await client.SendTextMessageAsync(
                    message.Chat.Id,
                    string.Join(", ", allUsers.Select(x=>x.UserName)));

            }
            catch (Exception ex)
            {
                await client.SendTextMessageAsync(message.Chat.Id, ex.ToString());
            }
        }
    }
}