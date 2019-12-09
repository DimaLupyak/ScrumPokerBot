using System;
using System.Collections.Generic;
using BotsController.Core.Callbacks;
using BotsController.Core.Commands;
using Telegram.Bot;

namespace BotsController.Core.Bots
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
                new JokeCommand(),
                new DamnCommand(),
                new NooCommand(),
                new SpeechCommand(),
                new YesNoCommand()
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
