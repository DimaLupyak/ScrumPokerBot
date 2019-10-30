using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace BotsController.Models.Commands
{
    public class MaxCommand : Command
    {
        public override string Name => @"/max";

        public override async Task ExecuteAsync(Message message, TelegramBotClient client)
        {
            try
            {
                var stickerSet = await client.GetStickerSetAsync("More_Faces");
                foreach (var sticker in stickerSet.Stickers)
                {
                    await client.SendTextMessageAsync(message.Chat.Id, sticker.FileId);
                    await client.SendStickerAsync(message.Chat.Id,
                        new InputOnlineFile(sticker.FileId)).ConfigureAwait(true);
                    await client.SendTextMessageAsync(message.Chat.Id, sticker.FileId);
                }

                await client.SendTextMessageAsync(message.Chat.Id, "Аве Макс!!!");
            }
            catch (Exception ex)
            {
                await client.SendTextMessageAsync(message.Chat.Id, ex.ToString());
            }
        }
    }
}