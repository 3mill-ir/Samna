using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BigMill.Models
{
    public class PollAnswerManagement
    {
        /// <summary>
        /// In tabe jahate afzudane gozineye pasokh be Soale morede nazar mibashad
        /// </summary>
        /// <param name="model">type="PollQuestionModel"</param>
        /// <returns>
        /// int (Adade 1 ra bar migardanad)
        /// </returns>
        public int AddPollAnswer(PollQuestionModel model)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                int t = 1;
                foreach (var item in model.AnswerBoxes)
                {
                    PollAnswer pol = new PollAnswer();
                    pol.AnswerText = item.AnswerText;
                    pol.F_PollQuestion_Id = model.ID;
                    pol.AnswerKey = t++;
                    //Score harkodam ra besurate pishfarz 1 dar nazar migirim ta nemudare rasm shode baraye Nazarsanji khali nabashad.
                    pol.Score = 1;
                    db.PollAnswers.Add(pol);
                }
                db.SaveChanges();
                return 1;
            }
        }


        /// <summary>
        /// in tabe zamani bekar borde mishavad ke hengame virayeshe gozine haye yek soal bekhahim gozine i jadid ezafe konim
        /// </summary>
        /// <param name="AnswerText">type="string"</param>
        /// <param name="F_PollQuestion_Id">type="int"</param>
        /// <param name="MaxKeyValue">type="int"</param>
        private void AddPollAnswerEditHelper(string AnswerText, int F_PollQuestion_Id, int MaxKeyValue)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                PollAnswer pol = new PollAnswer();
                pol.AnswerText = AnswerText;
                pol.F_PollQuestion_Id = F_PollQuestion_Id;
                pol.AnswerKey = MaxKeyValue+1;
                pol.Score = 1;
                db.PollAnswers.Add(pol);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// In tabe jahate virayeshe gozine haye yek soal bekar borde mishavad
        /// </summary>
        /// <param name="model">type="PollQuestionModel"</param>
        /// <returns>
        /// int
        /// </returns>
        public int EditPollAnswer(PollQuestionModel model)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                // tamamiye pasokh haye marbut be soale morede nazar ra miyabim
                var EditObject = db.PollAnswers.Where(u => u.F_PollQuestion_Id == model.ID);
                int MaxKeyValue;
                if (EditObject != null)
                {
                    foreach (var item in model.AnswerBoxes)
                    {
                        // tataboghe pasokh haye mojud dare model va pasokh haye yafte shode dar Database
                        //in kar baraye in ast ke befahmim aya pasokh feli dar halghe virayeshe pasokhist ke ghablan vojud dashte ya taze ezafe shode
                        var found = EditObject.FirstOrDefault(t => t.AnswerKey == item.AnswerKey);
                        if (found != null)
                        {
                            found.AnswerText = item.AnswerText;
                        }
                        else
                        {
                            //bishtarin key valu ra dar beyne pasokh haye feli miyabim ta gozineye jadid ra ba keyvalue ye yeki bishtar az aan varede Database konim
                            MaxKeyValue = EditObject.Max(u => u.AnswerKey) ?? default(int);
                            //ba estefade az tabee farakhani shode pasokhe jadid ra ezafe mikonim
                            AddPollAnswerEditHelper(item.AnswerText, model.ID, MaxKeyValue);
                        }
                    }
                    db.SaveChanges();
                    return 1;
                }
                else
                    return 2;
            }
        }

        /// <summary>
        /// In tabe baraye pak kardane gozine ha estefade migardad
        /// </summary>
        /// <param name="PollAnswerId">type="int"</param>
        /// <param name="PollQuestionID">type="int"</param>
        /// <returns>
        /// String
        /// </returns>
        public string DeletePollAnswer(int PollAnswerId, int PollQuestionID)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var _Object = db.PollQuestions.FirstOrDefault(u => u.ID == PollQuestionID);
                if (_Object == null || _Object.Status==false) { return "NotFound"; }
                //baraye barresiye inke soale morede nazar hatman 2 gozine dashte bashad va ejazeye pak kardan do gozineye baghimande hich gah dade nemishavad
                if (_Object.PollAnswers.Count < 3) { return "MinCount"; }
                var DeleteObject = _Object.PollAnswers.FirstOrDefault(u => u.ID == PollAnswerId);
                if (DeleteObject == null) { return "NotFound"; }
                //in ghesmat baraye pak kardane PollLog haye marbut be answere morede nazar baraye pak kardan bekar miravad
                    for (int i=0;i<DeleteObject.PollLogs.Count;i++)
                    {
                        db.PollLogs.Remove(DeleteObject.PollLogs.ToList()[i]);
                    }
                    db.SaveChanges();
                    db.PollAnswers.Remove(DeleteObject);
                    db.SaveChanges();
                    return "OK";
            }
        }

        /// <summary>
        /// Makhsuse Web
        /// In tabe baraye bala bordane Score pasokhe morede nazar bekar miravad
        /// hengame nazar dehi tavassote karbar gozineye entekhabiyash be in method ersal mishavad
        /// ba har afzayeshe score bayad PollLog i marbut be pasokh ham sakhte shavad.
        /// </summary>
        /// <param name="PollAnswerId">type="int"</param>
        /// <returns>
        /// int
        /// </returns>
        public int IncreasePollAnswerScore(int PollAnswerId)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var IsFound = db.PollAnswers.FirstOrDefault(u => u.ID == PollAnswerId);
                if (IsFound != null)
                {
                    IsFound.Score = IsFound.Score+1;
                    db.SaveChanges();
                    PollLogManagement PLM = new PollLogManagement();
                    PLM.AddPollLog(PollAnswerId,"Web");
                    return 1;
                }
                else
                    return 2;
            }
        }
        /// <summary>
        /// Makhsuse Android
        /// In tabe baraye bala bordane Score pasokhe morede nazar bekar miravad
        /// hengame nazar dehi tavassote karbar gozineye entekhabiyash be in method ersal mishavad
        /// ba har afzayeshe score bayad PollLog i marbut be pasokh ham sakhte shavad.
        /// </summary>
        /// <param name="PollAnswerId">type="int"</param>
        /// <returns>
        /// int
        /// </returns>
        public int AndroidIncreasePollAnswerScore(int PollAnswerId)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var IsFound = db.PollAnswers.FirstOrDefault(u => u.ID == PollAnswerId);
                if (IsFound != null)
                {
                    IsFound.Score = IsFound.Score + 1;
                    db.SaveChanges();
                    PollLogManagement PLM = new PollLogManagement();
                    PLM.AddPollLog(PollAnswerId, "Android");
                    return 1;
                }
                else
                    return 2;
            }
        }



        //public List<PollAnswerModel> ListPollAnswer()
        //{
        //    using (BigMill.Models.Entities db = new Entities())
        //    {
        //        var ListObject = db.PollAnswers;
        //        List<PollAnswerModel> list = new List<PollAnswerModel>();
        //        foreach (var ListItem in ListObject)
        //        {
        //            PollAnswerModel t = new PollAnswerModel();
        //            t.AnswerKey = ListItem.AnswerKey ?? default(int);
        //            t.AnswerText = ListItem.AnswerText;
        //            t.F_PollQusetion_Id = ListItem.F_PollQuestion_Id ?? default(int);
        //            t.Score = ListItem.Score ?? default(int);
        //            t.ID = ListItem.ID;
        //            list.Add(t);
        //        }
        //        return list;
        //    }
        //}
        //public List<PollAnswerModel> ListQusetionsSpecificPollAnswer(int F_PollQuestion_Id)
        //{
        //    using (BigMill.Models.Entities db = new Entities())
        //    {
        //        var ListObject = db.PollAnswers.Where(u => u.F_PollQuestion_Id == F_PollQuestion_Id);
        //        List<PollAnswerModel> list = new List<PollAnswerModel>();
        //        foreach (var ListItem in ListObject)
        //        {
        //            PollAnswerModel t = new PollAnswerModel();
        //            t.AnswerKey = ListItem.AnswerKey ?? default(int);
        //            t.AnswerText = ListItem.AnswerText;
        //            t.F_PollQusetion_Id = ListItem.F_PollQuestion_Id ?? default(int);
        //            t.Score = ListItem.Score ?? default(int);
        //            t.ID = ListItem.ID;
        //            list.Add(t);
        //        }
        //        return list;
        //    }
        //}
    }
}