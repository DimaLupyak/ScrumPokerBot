using System;
using System.Collections.Generic;
using BotsController.Models.Callbacks;
using BotsController.Models.Commands;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace BotsController.Models.Bots
{
    public abstract class ABot
    {
        protected readonly DateTime RunTime = DateTime.Now;
        protected TelegramBotClient _botClient;
        protected List<Command> _commands;
        protected List<Callback> _callbacks;
        
        protected IReadOnlyList<Command> Commands => _commands.AsReadOnly();
        protected IReadOnlyList<Callback> Callbacks => _callbacks.AsReadOnly();

        public abstract TelegramBotClient GetBotClient(string token);

        protected virtual void BotOnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Date.ToFileTimeUtc() < RunTime.ToFileTimeUtc())
                return;

            var message = e.Message;

            foreach (var command in Commands)
            {
                if (command.ShouldExecute(message))
                {
                    command.ExecuteAsync(message, _botClient);
                    break;
                }
            }
        }

        protected virtual void OnBotCallbackQuery(object sender, CallbackQueryEventArgs ev)
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
