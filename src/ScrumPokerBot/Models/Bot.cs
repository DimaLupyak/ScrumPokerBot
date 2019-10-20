using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScrumPokerBot.Models.Commands;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace ScrumPokerBot.Models
{
    public class Bot
    {
        private static TelegramBotClient _botClient;
        private static List<Command> _commandsList;

        public static IReadOnlyList<Command> Commands => _commandsList.AsReadOnly();

        public static async Task<TelegramBotClient> GetBotClientAsync()
        {
            if (_botClient != null)
            {
                return _botClient;
            }

            _commandsList = new List<Command> {new VoteCommand()};

            _botClient = new TelegramBotClient(AppSettings.Key);
            _botClient.OnMessage += BotOnMessage;
            _botClient.StartReceiving();
            return _botClient;
        }

        private static void BotOnMessage(object sender, MessageEventArgs e)
        {
            var message = e.Message;

            foreach (var command in Commands)
            {
                //if (command.Contains(message))
                {
                    command.Execute(message, _botClient);
                    break;
                }
            }
        }
    }
}
