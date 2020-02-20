using System;
using System.Threading.Tasks;
using BotsController.Core.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Voice = BotsController.Models.Data.Voice;

namespace BotsController.Core.Commands
{
    public class VoteCommand : Command
    {
        private readonly IRepository<Voice> _voiceRepository;

        public VoteCommand(IRepository<Voice> voiceRepository)
        {
            _voiceRepository = voiceRepository;
        }

        public override string Name => @"/vote";

        public override async Task ExecuteAsync(Message message, TelegramBotClient botClient)
        {
            try
            {
                await botClient.SendPollAsync(
                    chatId: message.Chat,
                    question: "Make your choice",
                    options: new[] { "1", "2", "3", "5", "8", "13", "21" }
                );
            }
            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Це так не работає");
            }
        }
    }
}