using System;
using System.Collections.Generic;
using BotsController.Core.Commands;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace BotsController.Core.Bots
{
    public class GrishaBot : ABot
    {
        private IReadOnlyList<Command> _allCommands;
        public GrishaBot()
        {
            _allCommands = new List<Command>
            {
                new PingCommand(),
                new SickerCommand(),
                new MaxCommand(),
                new JokeCommand(),
                new DamnCommand(),
                new NooCommand(),
                new SpeechCommand(),
                new YesNoCommand(),
                new ComicsCommand()
            }.AsReadOnly();


            _commands.Add(new CommandsControlCommand(_commands, _allCommands));
            _commands.AddRange(_allCommands);


            _botClient = new TelegramBotClient(Environment.GetEnvironmentVariable("GRISHA_BOT_TOKEN"));
            _botClient.OnMessage += BotOnMessage;
            _botClient.StartReceiving();
        }

    }
}
