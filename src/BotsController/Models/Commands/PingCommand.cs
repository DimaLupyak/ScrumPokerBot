using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotsController.Models.Commands
{
    public class PingCommand : Command
    {
        public override string Name => @"/ping";

        public override async Task Execute(Message message, TelegramBotClient botClient)
        {
            await botClient.SendTextMessageAsync(message.Chat.Id, "pong");
        }
    }
}