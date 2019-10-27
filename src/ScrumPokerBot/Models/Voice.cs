using System.Collections.Generic;

namespace BotsController.Models
{
    public class Voice
    {
        public List<string> Users { get; set; }
        public string Question { get; set; }
        public string[] Answers { get; set; }
        public Dictionary<string, int> Votes { get; set; }
        public int MessageId { get; set; }
        public bool IsOpened { get; set; }

        public Voice(int mId, string question, string[] answers)
        {
            MessageId = mId;
            Question = question;
            Answers = answers;
            Users = new List<string>();
            Votes = new Dictionary<string, int>();
        }
    }

}
