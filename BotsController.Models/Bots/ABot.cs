using System;
using System.Collections.Generic;
using BotsController.Core.Commands;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace BotsController.Core.Bots
{
    public abstract class ABot
    {
        protected readonly DateTime RunTime = DateTime.Now;
        protected TelegramBotClient _botClient;
        protected List<Command> _commands;
           
        protected virtual void BotOnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Date.ToFileTimeUtc() < RunTime.ToFileTimeUtc())
                return;

            var message = e.Message;

            foreach (var command in _commands)
            {
                if (command.ShouldExecute(message))
                {
                    command.ExecuteAsync(message, _botClient);
                    break;
                }
            }
        }
    }
}
