using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace BotsController.Models.Commands
{
    public class MaxCommand : Command
    {
        public override string Name => @"/макс";

        public override async Task ExecuteAsync(Message message, TelegramBotClient client)
        {
            var stickerSet = await client.GetStickerSetAsync("More Squad");
            foreach (var sticker in stickerSet.Stickers)
            {
                await client.SendStickerAsync(message.Chat.Id,
                    new InputOnlineFile(sticker.FileId)).ConfigureAwait(true);
                await client.SendTextMessageAsync(message.Chat.Id, sticker.FileId);
            }

            await client.SendTextMessageAsync(message.Chat.Id, "Аве Макс!!!");
        }
    }
}