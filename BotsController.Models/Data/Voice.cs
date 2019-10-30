using System.Collections.Generic;

namespace BotsController.Models.Data
{
    public class Voice : IIdentifiable
    {
        public List<string> Users { get; set; }
        public string Question { get; set; }
        public string[] Answers { get; set; }
        public Dictionary<string, int> Votes { get; set; }
        public string Id { get; set; }
        public int MessageId { get; set; }
        public bool IsOpened { get; set; }

        public Voice()
        {

        }

        public Voice(int mId, string question, string[] answers)
        {
            Id = mId.ToString();
            MessageId = mId;
            Question = question;
            Answers = answers;
            Users = new List<string>();
            Votes = new Dictionary<string, int>();
        }
    }

}
