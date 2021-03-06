using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotsController.Core.Commands
{
    public class VoteCommand : Command
    {

        public override string Name => @"/vote";

        public override async Task ExecuteAsync(Message message, TelegramBotClient botClient)
        {
            try
            {
                await botClient.SendPollAsync(
                    chatId: message.Chat.Id,
                    question: "Make your choice",
                    options: new[] { "1", "2", "3", "5", "8", "13", "21", "я тут не рішаю" },
                    isAnonymous: false
                );
            }
            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Це так не работає");
            }
        }
    }
}