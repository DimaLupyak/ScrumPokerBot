using System;
using System.Collections.Generic;
using BotsController.Models.Callbacks;
using BotsController.Models.Commands;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace BotsController.Models.Bots
{
    public class ScrumPokerBot : ABot
    {

        public static List<Voice> Votes { get; set; } = new List<Voice>();

        public override TelegramBotClient GetBotClient(string token)
        {
            if (_botClient != null)
            {
                return _botClient;
            }

            _commands = new List<Command> { new VoteCommand(), new PingCommand() };
            _callbacks = new List<Callback> { new VoteCallback() };

            _botClient = new TelegramBotClient(token);
            _botClient.OnMessage += BotOnMessage;
            _botClient.OnCallbackQuery += OnBotCallbackQuery;
            _botClient.StartReceiving();
            return _botClient;
        }
    }
}
