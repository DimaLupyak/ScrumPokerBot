using System;
using System.Collections.Generic;
using BotsController.Models.Callbacks;
using BotsController.Models.Commands;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace BotsController.Models.Bots
{
    public class GrishaBot : ABot
    {
        public GrishaBot()
        {
            _commands = new List<Command>
            {
                new PingCommand(),
                new SickerCommand(),
                new MaxCommand(),
                new JokeCommand()
            };
            _callbacks = new List<Callback>
            {

            };

            _botClient = new TelegramBotClient(Environment.GetEnvironmentVariable("GRISHA_BOT_TOKEN"));
            _botClient.OnMessage += BotOnMessage;
            _botClient.OnCallbackQuery += OnBotCallbackQuery;
            _botClient.StartReceiving();
        }
    }
}
