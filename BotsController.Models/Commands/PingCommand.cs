using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotsController.Models.Commands
{
    public class PingCommand : Command
    {
        public override string Name => @"/ping";

        public override Task ExecuteAsync(Message message, TelegramBotClient botClient)
        {
            return botClient.SendTextMessageAsync(message.Chat.Id, "pong");
        }
    }
}