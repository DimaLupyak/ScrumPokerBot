using System;
using System.Collections.Generic;
using BotsController.Core.Callbacks;
using BotsController.Core.Commands;
using BotsController.Core.Interfaces;
using BotsController.Models.Data;
using Telegram.Bot;

namespace BotsController.Core.Bots
{
    public class ScrumPokerBot : ABot
    {
        public ScrumPokerBot(IRepository<Voice> voiceRepository)
        {
            _commands = new List<Command>
            {
                new PingCommand(),
                new VoteCommand(voiceRepository),
                new YesNoCommand()
            };
            _callbacks = new List<Callback>
            {
                new VoteCallback(voiceRepository)
            };

            _botClient = new TelegramBotClient(Environment.GetEnvironmentVariable("SCRUM_POKER_BOT_TOKEN"));
            _botClient.OnMessage += BotOnMessage;
            _botClient.OnCallbackQuery += OnBotCallbackQuery;
            _botClient.StartReceiving();
        }
    }
}
