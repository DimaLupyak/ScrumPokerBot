using System;
using System.Collections.Generic;
using BotsController.Core.Commands;
using Telegram.Bot;

namespace BotsController.Core.Bots
{
    public class ScrumPokerBot : ABot
    {
        public ScrumPokerBot()
        {
            _commands = new List<Command>
            {
                new PingCommand(),
                new VoteCommand(),
                new YesNoCommand()
            };

            _botClient = new TelegramBotClient(Environment.GetEnvironmentVariable("SCRUM_POKER_BOT_TOKEN"));
            _botClient.OnMessage += BotOnMessage;
            _botClient.StartReceiving();
        }
    }
}
