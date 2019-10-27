using System;
using System.Collections.Generic;
using ScrumPokerBot.Models.Callbacks;
using ScrumPokerBot.Models.Commands;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace ScrumPokerBot.Models
{
    public class Bot
    {
        private static readonly DateTime RunTime = DateTime.Now;
        private static TelegramBotClient _botClient;
        private static List<Command> _commands;
        private static List<Callback> _collbacks;

        public static List<Voice> Votes { get; set; } = new List<Voice>();

        public static IReadOnlyList<Command> Commands => _commands.AsReadOnly();
        public static IReadOnlyList<Callback> Callbacks => _collbacks.AsReadOnly();

        public static TelegramBotClient GetBotClientAsync(string token)
        {
            if (_botClient != null)
            {
                return _botClient;
            }

            _commands = new List<Command> { new VoteCommand() };
            _collbacks = new List<Callback> { new VoteCallback() };

            _botClient = new TelegramBotClient(token);
            _botClient.OnMessage += BotOnMessage;
            _botClient.OnCallbackQuery += OnBotCallbackQuery;
            _botClient.StartReceiving();
            return _botClient;
        }

        private static void BotOnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Date.ToFileTimeUtc() < RunTime.ToFileTimeUtc())
                return;

            var message = e.Message;

            foreach (var command in Commands)
            {
                if (command.Contains(message))
                {
                    command.Execute(message, _botClient);
                    break;
                }
            }
        }

        private static void OnBotCallbackQuery(object sender, CallbackQueryEventArgs ev)
        {
            foreach (var callback in Callbacks)
            {
                if (callback.Contains(ev.CallbackQuery))
                {
                    callback.Execute(ev.CallbackQuery, _botClient);
                    break;
                }
            }
        }
    }
}
