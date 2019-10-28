using System;
using System.Collections.Generic;
using BotsController.Models.Callbacks;
using BotsController.Models.Commands;
using BotsController.Models.Data;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace BotsController.Models.Bots
{
    public class ScrumPokerBot : ABot
    {

        public static List<Voice> Votes { get; set; } = new List<Voice>();

        public ScrumPokerBot()
        {
            _commands = new List<Command> { new VoteCommand(), new PingCommand() };
            _callbacks = new List<Callback> { new VoteCallback() };

            _botClient = new TelegramBotClient(Environment.GetEnvironmentVariable("SCRUM_POKER_BOT_TOKEN"));
            _botClient.OnMessage += BotOnMessage;
            _botClient.OnCallbackQuery += OnBotCallbackQuery;
            _botClient.StartReceiving();
        }
    }
}
