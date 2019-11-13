using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BotsController.Core.Commands
{
    public abstract class Command
    {
        public abstract string Name { get; }

        public abstract Task ExecuteAsync(Message message, TelegramBotClient client);

        public virtual bool ShouldExecute(Message message)
        {
            if (message.Type != MessageType.Text)
                return false;

            return message.Text.ToLower().Contains(this.Name.ToLower());
        }
    }
}
