using System;
using System.Collections.Generic;
using BotsController.Models.Callbacks;
using BotsController.Models.Commands;
using BotsController.Models.Data;
using BotsController.Models.Interfaces;
using Telegram.Bot;

namespace BotsController.Models.Bots
{
    public class ScrumPokerBot : ABot
    {
        public ScrumPokerBot(IRepository<Voice> voiceRepository)
        {
            _commands = new List<Command> { new VoteCommand(voiceRepository), new PingCommand() };
            _callbacks = new List<Callback> { new VoteCallback(voiceRepository) };

            _botClient = new TelegramBotClient(Environment.GetEnvironmentVariable("SCRUM_POKER_BOT_TOKEN"));
            _botClient.OnMessage += BotOnMessage;
            _botClient.OnCallbackQuery += OnBotCallbackQuery;
            _botClient.StartReceiving();
        }
    }
}
