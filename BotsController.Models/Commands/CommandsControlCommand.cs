using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BotsController.Core.Commands
{
    public class CommandsControlCommand : Command
    {
        public override string Name => @"CommandsControlCommand";

        private List<Command> _commands;
        private IReadOnlyList<Command> _dependedCommnds;

        public CommandsControlCommand(List<Command> commands, IReadOnlyList<Command> dependedCommnds)
        {
            _commands = commands;
            _dependedCommnds = dependedCommnds;
        }


        public override Task ExecuteAsync(Message message, TelegramBotClient botClient)
        {
            var selectedCommand = _dependedCommnds.FirstOrDefault(x => message.Text.Contains(x.Name, StringComparison.InvariantCultureIgnoreCase));
            string answer = "please enter 'CommandsControlCommand (start/stop) (command name)";

            if (selectedCommand == null)
                return botClient.SendTextMessageAsync(message.Chat.Id, "command name does not exists");


            if (message.Text.ToLower().Contains("start", StringComparison.InvariantCultureIgnoreCase) && _commands.All(x => x.Name != selectedCommand.Name))
            {
                _commands.Add(selectedCommand);
                answer = $"{selectedCommand.Name} started";
            }

            if (message.Text.ToLower().Contains("stop", StringComparison.InvariantCultureIgnoreCase) && _commands.Any(x => x.Name == selectedCommand.Name))
            {
                _commands.RemoveAll(x=> x.Name == selectedCommand.Name);
                answer = $"{selectedCommand.Name} stoped";
            }

            return botClient.SendTextMessageAsync(message.Chat.Id, answer);
        }

        public override bool ShouldExecute(Message message)
        {
            if (message.Type != MessageType.Text)
                return false;

            return message.Text.Contains(Name, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}