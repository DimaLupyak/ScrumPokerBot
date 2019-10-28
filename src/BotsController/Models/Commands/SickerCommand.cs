using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace BotsController.Models.Commands
{
    public class SickerCommand : Command
    {
        Random random = new Random();
        public override string Name => @"sticker";

        public override bool ShouldExecute(Message message)
        {
            return random.Next(100) < 1;
        }

        public override async Task Execute(Message message, TelegramBotClient client)
        {
            var stickers = new List<Sticker>();
                stickers.AddRange(client.GetStickerSetAsync("BlueRobots").Result.Stickers);
            await client.SendStickerAsync(message.Chat.Id, new InputOnlineFile(stickers[random.Next(stickers.Count)].FileId));
        }
    }
}
