using System;
using System.IO;
using System.Threading.Tasks;
using BotsController.Core.Helpers;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace BotsController.Core.Commands
{
    public class NooCommand : Command
    {
        public override string Name => @"скажи ноу";

        public override Task ExecuteAsync(Message message, TelegramBotClient botClient)
        {
            try
            {
                return botClient.SendTextMessageAsync(message.Chat.Id, "https://www.youtube.com/watch?v=WWaLxFIVX1s");
            }
            catch (Exception ex)
            {
                return botClient.SendTextMessageAsync(message.Chat.Id, ex.ToString());
            }
        }
    }
}