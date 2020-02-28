using BotsController.DAL;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace BotsController.Core.Commands
{
    public class MaxCommand : Command
    {
        public override string Name => @"макс";

        private string maxStickerFileId = "CAADAgADXAADXnqpFgYOGj8qLFTeFgQ";
        readonly Random random = new Random();
        public override async Task ExecuteAsync(Message message, TelegramBotClient client)
        {
            try
            {
                if (random.Next(100) < 20)
                {
                    await client.SendStickerAsync(message.Chat.Id,
                        new InputOnlineFile(maxStickerFileId));
                }
                else
                {
                    var repository = new Repository(
                        Environment.GetEnvironmentVariable("GRISHA_BOT_FIREBASE_AUTH"),
                        Environment.GetEnvironmentVariable("GRISHA_BOT_FIREBASE_URL"));

                    repository.AddAsync("АВЕ МАКС");
                                        
                    await client.SendTextMessageAsync(message.Chat.Id, repository.GetDataAsync().Result);
                }
            }
            catch (Exception ex)
            {
                await client.SendTextMessageAsync(message.Chat.Id, ex.ToString());
            }
        }
    }
}