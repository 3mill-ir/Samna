using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BigMill.Models
{
    public class UserPollModel
    {
        public UserPollModel()
        {
            AnswerBox = new List<PollAnswerModel>();
        }
        public int ID { get; set;}
        public int SelectedPollAnswerID { get; set; }
        public string QuestionText { get; set; }
        public List<PollAnswerModel> AnswerBox { get; set; }
    }
}