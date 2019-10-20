using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumPokerBot.Models
{
    public static class AppSettings
    {
        public static string Url { get; set; } = "https://scrum-poker-bot.herokuapp.com/{0}";
        public static string Name { get; set; } = "StoryPointer";
        public static string Key { get; set; } = "661853198:AAGqpmG4XnM2Q8xRpxhsLfQU_pUsoC_85ow";
    }
}
