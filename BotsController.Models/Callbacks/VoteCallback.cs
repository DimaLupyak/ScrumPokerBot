using System;
using System.Linq;
using System.Threading.Tasks;
using BotsController.Models.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Voice = BotsController.Models.Data.Voice;

namespace BotsController.Models.Callbacks
{
    public class VoteCallback : Callback
    {
        private readonly IRepository<Voice> _voiceRepository;

        public VoteCallback(IRepository<Voice> voiceRepository)
        {
            _voiceRepository = voiceRepository;
        }

        public override string Name => @"callbackVoice";

        public override async Task ExecuteAsync(CallbackQuery query, TelegramBotClient client)
        {
            try
            {
                int num = int.Parse(query.Data[^1].ToString());

                var currentVoice = _voiceRepository.Get(query.Message.MessageId.ToString());
                var variants = currentVoice.Answers;

                //When "Show results is clicked"
                if (num == 7)
                {
                    currentVoice.IsOpened = true;
                }
                else
                {
                    if (currentVoice.IsOpened)
                    {
                        var all =
                            $"{currentVoice.Answers[num]}:\r\n {string.Join(",\r\n ", currentVoice.Votes.Where(item => item.Value == num).Select(item => item.Key))}";
                        await client.AnswerCallbackQueryAsync(query.Id, all, true).ConfigureAwait(false);
                        return;
                    }

                    if (!currentVoice.Votes.ContainsKey(query.From.FirstName + query.From.LastName))
                    {
                        currentVoice.Votes.Add(query.From.FirstName + query.From.LastName, num);
                    }
                }

                var buttonLinesCount = currentVoice.IsOpened ? 2 : 3;

                var buttons = new InlineKeyboardButton[buttonLinesCount][];
                buttons[0] = new InlineKeyboardButton[4];
                buttons[1] = new InlineKeyboardButton[3];
                if (buttons.Length > 2)
                {
                    buttons[2] = new InlineKeyboardButton[1];
                }

                for (var i = 0; i < 4; i++)
                {
                    var choicesCount = string.Empty;
                    if (currentVoice.IsOpened)
                    {
                        choicesCount = $" ({currentVoice.Votes.Count(a => a.Value == i)})";
                    }

                    buttons[0][i] = new InlineKeyboardButton
                    {
                        Text = variants[i] + choicesCount,
                        CallbackData = "callbackVoice" + i
                    };
                }

                for (var i = 0; i < 3; i++)
                {
                    var choicesCount = string.Empty;
                    if (currentVoice.IsOpened)
                    {
                        choicesCount = $" ({currentVoice.Votes.Count(a => a.Value == i + 4)})";
                    }

                    buttons[1][i] = new InlineKeyboardButton
                    {
                        Text = variants[i + 4] + choicesCount,
                        CallbackData = "callbackVoice" + (i + 4)
                    };

                }

                if (buttons.Length > 2)
                {
                    buttons[2][0] = new InlineKeyboardButton
                    {
                        Text = "Show Votes " + " (" + currentVoice.Votes.Count + ")",
                        CallbackData = "callbackVoice7" + 7
                    };
                }

                _voiceRepository.Update(currentVoice);
                var keyboard = new InlineKeyboardMarkup(buttons);
                await client.EditMessageTextAsync(query.Message.Chat.Id, currentVoice.MessageId, currentVoice.Question,
                    ParseMode.Default, false, keyboard).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                client.SendTextMessageAsync(query.Message.Chat.Id, "Я забув про це голосування =(");

            }
        }
    }
}