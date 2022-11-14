using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BigMill.Models;

namespace BigMill.Controllers
{
    public class PollQuestionController : Controller
    {
        #region Android
        /// <summary>
        /// in tabe soale nazar sanji zamane feeli ra be hamrahe pasokh haye aan baz migardanad.(Makhsuse android)
        /// </summary>
        /// <returns>
        /// string
        /// Jsoni tahte onvane string ke soal ha dar fielde (Question) va pasokh ha dar fielde (Answer) mibashad
        /// </returns>
        public string AndroidPoll()
        {
            UserPollModel model = new UserPollModel();
            PollQuestionManagement PQM = new PollQuestionManagement();
            model = PQM.UserPollHandler();
            if (model != null)
            {
                // baraye soal matne answer ra khali migozarim
                string Result = "{\"Root\": [{\"Question\": \"" + model.QuestionText + "\"," + "\"Answer\": \"-\",\"ID\":\"" + model.ID + "\"}";
                foreach (var item in model.AnswerBox)
                {
                    // baraye pasokh ha matne question  ra khali migozarim
                    Result += ",{\"Question\": \"\",\"Answer\": \"" + item.AnswerText + "\",\"ID\":\"" + item.ID + "\"}";
                }
                Result += "]}";
                return Result;
            }
            else
                return "{\"Root\": [{\"Error\": \"!در حال حاضر هیچ نظر سنجی فعالی وجود ندارد\"}]}";
        }
        /// <summary>
        /// in controller makhsuste daryafte pasokhe soale nazar sanji az tarafe device haye androidi ast
        /// </summary>
        /// <param name="PollAnswerId">idie pasokhe morede nazar</param>
        /// <returns>
        /// string
        /// OK : dar surati ke nazar ba movaffaghiyat sabt shavad
        /// NOK : dar surati ke heyne sabte nazar moshkeli rokh dahad
        /// </returns>
        [HttpPost]       
        public string AndroidPoll(string PollAnswerId)
        {
            PollAnswerManagement PAM = new PollAnswerManagement();
            int scale = PAM.AndroidIncreasePollAnswerScore(int.Parse( PollAnswerId));
            if (scale == 1)
            {
                return "OK";
            }
            else
            {
                return "NOK";
            }
        }

        /// <summary>
        /// in controller baraye faraham kardane dade haye lazem jahate rasme charte nazarsanjiye device haye androidi bekar miravad
        /// </summary>
        /// <returns>
        /// string
        /// Jsoni tahte onvane string
        /// fielde gozine haye pasokh ra ba (AnswerName) va fielde emtiyaze har pasokh ra ba (Score)
        /// </returns>
        public string AndroidPollChart()
        {
            UserPollModel model = new UserPollModel();
            PollQuestionManagement PQM = new PollQuestionManagement();
            model = PQM.UserPollHandler();
            if (model != null)
            {
                string Result = "{\"Root\": [";
                foreach (var item in model.AnswerBox)
                {
                    Result += "{\"AnswerName\": \"" + item.AnswerText + "\",\"Score\": \"" + item.Score + "\"},";
                }
                Result = Result.Remove(Result.LastIndexOf(','), 1);
                Result += "]}";
                return Result;
            }
            else
                //agar nazar sanji dar hale hazer mojud nabashad
                return "{\"Root\": [{\"Error\": \"!در حال حاضر هیچ نظر سنجی فعالی وجود ندارد\"}]}";
        }
        #endregion
    }
}