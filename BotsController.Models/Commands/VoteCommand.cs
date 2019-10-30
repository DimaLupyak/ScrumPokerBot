using System;
using System.Threading.Tasks;
using BotsController.Models.Bots;
using BotsController.Models.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Voice = BotsController.Models.Data.Voice;

namespace BotsController.Models.Commands
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
                var title = "Make your choice";
                var variants = new[] { "1", "2", "3", "5", "8", "13", "21" };
                var buttons = new InlineKeyboardButton[2][];
                buttons[0] = new InlineKeyboardButton[4];
                buttons[1] = new InlineKeyboardButton[3];
                for (var i = 0; i < 4; i++)
                {
                    buttons[0][i] = new InlineKeyboardButton
                    {
                        Text = variants[i],
                        CallbackData = "callbackVoice" + i
                    };

                }
                for (var i = 0; i < 3; i++)
                {
                    buttons[1][i] = new InlineKeyboardButton
                    {
                        Text = variants[i + 4],
                        CallbackData = "callbackVoice" + (i + 4)
                    };

                }

                var keyboard = new InlineKeyboardMarkup(buttons);
                var mess = await botClient.SendTextMessageAsync(message.Chat.Id, title, ParseMode.Default, false, false, 0, keyboard);
                _voiceRepository.Insert(new Voice(mess.MessageId, title, variants));

            }
            catch (Exception ex)
            {
                await botClient.SendTextMessageAsync(message.Chat.Id, "Це так не работає");
            }
        }
    }
}