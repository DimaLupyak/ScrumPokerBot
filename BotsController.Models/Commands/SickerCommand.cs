using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace BotsController.Core.Commands
{
    public class SickerCommand : Command
    {
        private readonly Random random = new Random();
        public override string Name => @"sticker";

        private readonly string[] stickerPacks =
        {
            "BlueRobots",
            "VoldemarDenchik",
            "metairony",
            "sad_crying_cat",
            "BOMZHI_eeZee",
            "citati_prosto"
        };

        private const float MinPossibility = 1;
        private const float MaxPossibility = 10;
        private const float PossibilityStep = 0.1f;

        private float possibility = 5;
        
        public override bool ShouldExecute(Message message)
        {
            if (possibility < MaxPossibility)
            {
                possibility += PossibilityStep;
            }

            if (random.Next(100) < possibility)
            {
                possibility = MinPossibility;
                return true;
            }

            return false;
        }

        public override async Task ExecuteAsync(Message message, TelegramBotClient client)
        {
            var stickers = new List<Sticker>();
            foreach (var stickerPack in stickerPacks)
            {
                stickers.AddRange((await client.GetStickerSetAsync(stickerPack).ConfigureAwait(false)).Stickers);
            }
            await client.SendStickerAsync(message.Chat.Id,
                new InputOnlineFile(stickers[random.Next(stickers.Count)].FileId)).ConfigureAwait(true);
        }
    }
}
