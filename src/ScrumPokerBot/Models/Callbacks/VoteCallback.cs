using System;
using System.Linq;
using System.Threading.Tasks;
using BotsController.Models.Bots;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace BotsController.Models.Callbacks
{
    public class VoteCallback : Callback
    {
        public override string Name => @"callbackVoice";

        public override async Task Execute(CallbackQuery query, TelegramBotClient client)
        {
            try
            {
                int num = int.Parse(query.Data[^1].ToString());

                var currentVoice = ScrumPokerBot.Votes.First(gol => gol.MessageId == query.Message.MessageId);
                var variants = currentVoice.Answers;

                //Whan "Show results is clicked"
                if (num == 7)
                {
                    currentVoice.IsOpened = true;
                }
                else
                {
                    if (currentVoice.IsOpened)
                    {
                        var all = string.Format("{0}:\r\n {1}", currentVoice.Answers[num],
                            string.Join(",\r\n ",
                                currentVoice.Votes
                                    .Where(item => item.Value == num)
                                    .Select(item => item.Key)));
                        await client.AnswerCallbackQueryAsync(query.Id, all, true);
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
                    var nums = string.Empty;
                    if (currentVoice.IsOpened)
                    {
                        nums = " (" + currentVoice.Votes.Where(a => a.Value == i).ToArray().Length + ")";
                    }
                    buttons[0][i] = new InlineKeyboardButton
                    {
                        Text = variants[i] + nums,
                        CallbackData = "callbackVoice" + i
                    };
                }
                for (var i = 0; i < 3; i++)
                {
                    var nums = string.Empty;
                    if (currentVoice.IsOpened)
                    {
                        nums = " (" + currentVoice.Votes.Where(a => a.Value == i + 4).ToArray().Length + ")";
                    }
                    buttons[1][i] = new InlineKeyboardButton
                    {
                        Text = variants[i + 4] + nums,
                        CallbackData = "callbackVoice" + (i + 4)
                    };

                }
                if (buttons.Length > 2)
                {
                    var nums = currentVoice.Votes.Count;
                    buttons[2][0] = new InlineKeyboardButton
                    {
                        Text = "Show Votes " + " (" + nums + ")",
                        CallbackData = "callbackVoice" + (7)
                    };
                }

                var keyboard = new InlineKeyboardMarkup(buttons);
                await client.EditMessageTextAsync(query.Message.Chat.Id, currentVoice.MessageId, currentVoice.Question, ParseMode.Default, false, keyboard);
            }
            catch (Exception) { }
        }
    }
}