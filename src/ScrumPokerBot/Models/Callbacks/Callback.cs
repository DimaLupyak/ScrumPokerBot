using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotsController.Models.Callbacks
{
    public abstract class Callback
    {
        public abstract string Name { get; }

        public abstract Task Execute(CallbackQuery query, TelegramBotClient client);

        public virtual bool Contains(CallbackQuery query)
        {
            return query.Data.Contains(Name);
        }
    }
}
