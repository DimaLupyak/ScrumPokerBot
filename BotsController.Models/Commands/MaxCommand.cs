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
                    await client.SendTextMessageAsync(message.Chat.Id, "Аве Макс!!!");
                }
            }
            catch (Exception ex)
            {
                await client.SendTextMessageAsync(message.Chat.Id, ex.ToString());
            }
        }
    }
}