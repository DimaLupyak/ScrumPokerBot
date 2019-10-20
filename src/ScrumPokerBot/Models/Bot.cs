using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ScrumPokerBot.Models.Commands;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace ScrumPokerBot.Models
{
    public class Bot
    {
        private static readonly DateTime RunTime = DateTime.Now;
        private static TelegramBotClient _botClient;
        private static List<Command> _commandsList;
        public static Settings Settings;

        public static IReadOnlyList<Command> Commands => _commandsList.AsReadOnly();

        public static async Task<TelegramBotClient> GetBotClientAsync()
        {
            if (_botClient != null)
            {
                return _botClient;
            }

            try
            {
                Settings = new Settings(AppSettings.VotesFile);
            }
            catch (Exception ex)
            {
            }

            _commandsList = new List<Command> {new VoteCommand()};

            _botClient = new TelegramBotClient(AppSettings.Key);
            _botClient.OnMessage += BotOnMessage;
            _botClient.OnCallbackQuery += OnBotCallbackQuery;
            _botClient.StartReceiving();
            return _botClient;
        }

        private static void BotOnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Date.ToFileTimeUtc() < RunTime.ToFileTimeUtc())
                return;

            var message = e.Message;

            foreach (var command in Commands)
            {
                if (command.Contains(message))
                {
                    command.Execute(message, _botClient);
                    break;
                }
            }
        }

        private static void OnBotCallbackQuery(object sender, CallbackQueryEventArgs ev)
        {
            if (ev.CallbackQuery.Data.Contains("callbackVoice"))
            {
                try
                {
                    int num = int.Parse(ev.CallbackQuery.Data[ev.CallbackQuery.Data.Length - 1].ToString());

                    var currentVoice = Settings.VoiceList.First(gol => gol.MessageId == ev.CallbackQuery.Message.MessageId);
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
                            _botClient.AnswerCallbackQueryAsync(ev.CallbackQuery.Id, all, true);
                            return;
                        }
                        if (!currentVoice.Votes.ContainsKey(ev.CallbackQuery.From.FirstName + ev.CallbackQuery.From.LastName))
                        {
                            currentVoice.Votes.Add(ev.CallbackQuery.From.FirstName + ev.CallbackQuery.From.LastName, num);
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
                            Text = "Show VotesFile " + " (" + nums + ")",
                            CallbackData = "callbackVoice" + (7)
                        };
                    }

                    var keyboard = new InlineKeyboardMarkup(buttons);
                    try
                    {
                        _botClient.EditMessageTextAsync(ev.CallbackQuery.Message.Chat.Id, currentVoice.MessageId, currentVoice.Question, ParseMode.Default, false, keyboard);
                    }
                    catch (Exception) { }
                }
                catch (Exception) { }
                Settings.Save();
            }
        }
    }
}
