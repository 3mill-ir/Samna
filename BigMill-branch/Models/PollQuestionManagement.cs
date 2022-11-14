using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BigMill.Models;
using BigMill.Areas.Admin3mill.Models;
using System.Data.Entity;

namespace BigMill.Models
{
    public class PollQuestionManagement
    {
        /// <summary>
        /// in tabe baraye ezafe kardane soale nazar sanjiye jadid be hamrahe gozine haye pasokhe aan bekar miravad
        /// tabe (CanBeInserted) dar in tabe farakhani mishavad
        /// </summary>
        /// <param name="model">type="PollQuestionModel"</param>
        /// <param name="start">type="DateTime"</param>
        /// <param name="end">type="DateTime"</param>
        /// <returns>
        /// int
        /// </returns>
        public int AddPollQuestion(PollQuestionModel model, DateTime start, DateTime end)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                // check kardane in ke tarikh e shuru va payane vurudi ba shuru va payan haye feeli tadakhol nadashte bashad.
                if (CanBeInserted(start, end,model.ID))
                {
                    PollQuestion InsertObject = new PollQuestion();
                    InsertObject.CreatedOnUTC = DateTime.Now;
                    InsertObject.EndDateOnUTC = end;
                    InsertObject.Question = model.Question;
                    InsertObject.StartDateOnUTC = start;
                    InsertObject.Status = true;
                    InsertObject.F_User_Id = Tools.F_UserId();
                    db.PollQuestions.Add(InsertObject);
                    db.SaveChanges();
                    model.ID = InsertObject.ID;
                    PollAnswerManagement pol = new PollAnswerManagement();
                    pol.AddPollAnswer(model);
                    return 1;
                }
                else
                    return 2;
            }
        }

        /// <summary>
        ///baraye check kardane tadakhole zamani
        ///tabe (WithoutContact) dar in tabe farakhani mishavad
        /// </summary>
        /// <param name="start">type="DateTime"</param>
        /// <param name="end">type="DateTime"</param>
        /// <param name="ID">type="int"</param>
        /// <returns>
        /// bool
        /// </returns>
        private bool CanBeInserted(DateTime start, DateTime end,int? ID)
        {
            using(BigMill.Models.Entities db = new Entities())
            {
                if (start < end)
                {
                    var Times = db.PollQuestions.Where(u => u.StartDateOnUTC > DateTime.Now && u.Status == true);
                    if (Times != null)
                    {
                        foreach (var tople in Times)
                        {
                            // baraye barrasiye inke yek nazarsanji az nazare tadakhole zamani ba khodash moghayese nashavad
                            if (tople.ID != ID)
                            {
                                DateTime teststart = new DateTime();
                                teststart = tople.StartDateOnUTC ?? default(DateTime);
                                DateTime testend = new DateTime();
                                testend = tople.EndDateOnUTC ?? default(DateTime);
                                if (WithoutContact(start, end, teststart, testend) != true)
                                    return false;
                            }
                        }
                        return true;
                    }
                    return true;
                }
                else
                    return false;
            }
        }
        /// <summary>
        /// tavassote tabee CanBeInserted estefade migardad baraye moghayese tarikh ha
        /// </summary>
        /// <param name="start">type="DateTime"</param>
        /// <param name="end">type="DateTime"</param>
        /// <param name="teststart">type="DateTime"</param>
        /// <param name="testend">type="DateTime"</param>
        /// <returns>
        /// bool
        /// </returns>
        private bool WithoutContact(DateTime start, DateTime end, DateTime teststart, DateTime testend)
        {
            if (  ((start < teststart||start == teststart) && (end < teststart||end == teststart))     ||     ((start > testend||start == testend) && (end > testend||end == testend))  )
                return true;
            else
                return false;
        }

        /// <summary>
        /// In Tabe baraye virayeshe soale morede nazar be hamrahe pasokh hayash estefade migardad
        /// Tavabe (EditPollAnswer va CanBeInserted) dar in tabe farakhani mishavand
        /// </summary>
        /// <param name="model">type"PollQuestionModel"</param>
        /// <param name="start">type"DateTime"</param>
        /// <param name="end">type"DateTime"</param>
        /// <returns>
        /// string
        /// </returns>
        public string EditPollQuestion(PollQuestionModel model, DateTime start, DateTime end)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                // check kardane inke aya tarikh haye vorudi ba tarikh haye mojud tadakhol darand ya kheyr
                if (CanBeInserted(start, end,model.ID))
                {
                    var EditObject = db.PollQuestions.FirstOrDefault(u => u.ID == model.ID);
                    if (EditObject == null || EditObject.Status==false) { return "NotFound"; }
                        EditObject.EndDateOnUTC = end;
                        EditObject.F_User_Id = model.F_User_Id;
                        EditObject.StartDateOnUTC = start;
                        EditObject.Question = model.Question;
                        PollAnswerManagement pollans = new PollAnswerManagement();
                        pollans.EditPollAnswer(model);
                        db.SaveChanges();
                        return "OK";
                }
                else
                    return "Conflict";
            }
        }

        //public int DeletePollQuestion(int PollQuestionId)
        //{
        //    using (BigMill.Models.Entities db = new Entities())
        //    {
        //        var DeleteObject = db.PollQuestions.FirstOrDefault(u => u.ID == PollQuestionId);
        //        if (DeleteObject != null)
        //        {
        //            db.PollQuestions.Remove(DeleteObject);
        //            db.SaveChanges();
        //            return 1;
        //        }
        //        else
        //            return 2;
        //    }
        //}

        /// <summary>
        /// In tabe baraye taghir vaziyate soale nazar sanji bekar miravad
        /// </summary>
        /// <param name="PollQuestionId">type="int"</param>
        /// <returns>
        /// string
        /// </returns>
        public string ChangeStatusPollQuestion(int PollQuestionId)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var ChangeStatusObject = db.PollQuestions.FirstOrDefault(u => u.ID == PollQuestionId);

                if (ChangeStatusObject == null || ChangeStatusObject.Status==false)        {    return "NotFound";}
                    ChangeStatusObject.Status = !ChangeStatusObject.Status;
                    db.SaveChanges();
                    return "OK";
            }
        }

        /// <summary>
        /// In tabe baraye bazgardandane soale nazarsanjiye feli be hamrahe gozine haye pasokhe aan estefade migardad
        /// </summary>
        /// <returns>
        /// UserPollModel : dar surate vujude nazarsanjiye faal
        /// null: dar surate nabude nazarsanjiye faal
        /// </returns>
        public UserPollModel UserPollHandler()
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                // estefade az LazyLoading dar query
                db.Configuration.LazyLoadingEnabled = false;
                var FindedActiveObject = db.PollQuestions.Include(t=>t.PollAnswers).FirstOrDefault(u =>u.Status==true && (u.StartDateOnUTC < DateTime.Now || u.StartDateOnUTC == DateTime.Now) && (u.EndDateOnUTC > DateTime.Now || u.EndDateOnUTC == DateTime.Now));

                if (FindedActiveObject != null)
                {
                    UserPollModel model = new UserPollModel();
                    model.QuestionText = FindedActiveObject.Question;
                    model.ID = FindedActiveObject.ID;
                    var Answers = FindedActiveObject.PollAnswers.ToList();
                    foreach (var item in Answers)
                    {
                        PollAnswerModel PAL = new PollAnswerModel();
                        PAL.AnswerText = item.AnswerText;
                        PAL.ID = item.ID;
                        PAL.Score = item.Score ?? default(int);
                        PAL.AnswerKey = item.AnswerKey ?? default(int);
                        model.AnswerBox.Add(PAL);
                    }
                    return model;
                }
                else
                    return null;
            }
        }

        //Liste tamamiye nazarsanjihaye ta behal sabt shode (Makhsuse admin)
        public List<PollQuestionModel> ListPollQuestion()
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                // status==false yani admin aan nazarsanji ra paak karde va niazi be nemayeshe aan nist
                var ListObject = db.PollQuestions.Where(m=>m.Status==true).OrderByDescending(m=>m.StartDateOnUTC);
                List<PollQuestionModel> list = new List<PollQuestionModel>();
                foreach (var ListItem in ListObject)
                {
                    PollQuestionModel t = new PollQuestionModel();
                    DateTime creat = new DateTime();
                    creat = ListItem.CreatedOnUTC ?? default(DateTime);
                    t.CreatedOnUTCJalali = Tools.GetDateTimeReturnJalaliDate(creat);
                    DateTime end = new DateTime();
                    end = ListItem.EndDateOnUTC ?? default(DateTime);
                    t.EndDateOnUTC = Tools.GetDateTimeReturnJalaliDate(end);
                    DateTime start = new DateTime();
                    start = ListItem.StartDateOnUTC ?? default(DateTime);
                    t.StartDateOnUTC = Tools.GetDateTimeReturnJalaliDate(start);
                    t.F_User_Id = ListItem.F_User_Id ?? default(int);
                    t.Question = ListItem.Question;
                    t.Status = ListItem.Status ?? default(bool);
                    t.ID = ListItem.ID;
                    // baraye check kardane Faal budan ya nabudan
                    if ((ListItem.StartDateOnUTC < DateTime.Now || ListItem.StartDateOnUTC == DateTime.Now) && (ListItem.EndDateOnUTC > DateTime.Now || ListItem.EndDateOnUTC == DateTime.Now))
                        t.Active = true;
                    else
                        t.Active = false;
                    list.Add(t);
                }
                return list;
            }
        }



        /// <summary>
        /// In tabe baraye bazgardandane Joziyate soale nazarsanjiye feli be hamrahe gozine haye pasokhe aan estefade migardad (Makhsuse admin)
        /// </summary>
        /// <returns>
        /// UserPollModel : dar surate vujude nazarsanjiye faal
        /// null: dar surate nabude nazarsanjiye faal
        /// </returns>
        public PollQuestionModel PollQuestionDetail(int PollQuestionId)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var Founded = db.PollQuestions.FirstOrDefault(u => u.ID == PollQuestionId);
                if (Founded == null || Founded.Status==false) { return null; }
                PollQuestionModel t = new PollQuestionModel();
                DateTime creat = new DateTime();
                creat = Founded.CreatedOnUTC ?? default(DateTime);
                t.CreatedOnUTCJalali = Tools.GetDateTimeReturnJalaliDate(creat);
                t.CreatedOnUTC = Founded.CreatedOnUTC ?? default(DateTime);
                DateTime end = new DateTime();
                end = Founded.EndDateOnUTC ?? default(DateTime);
                t.EndDateOnUTC = Tools.GetDateTimeReturnJalaliDate(end);
                DateTime start = new DateTime();
                start = Founded.StartDateOnUTC ?? default(DateTime);
                t.StartDateOnUTC = Tools.GetDateTimeReturnJalaliDate(start);
                t.F_User_Id = Founded.F_User_Id ?? default(int);
                t.Question = Founded.Question;
                t.Status = Founded.Status ?? default(bool);
                t.ID = Founded.ID;
                var answerbox = db.PollAnswers.Where(u => u.F_PollQuestion_Id == PollQuestionId);
                foreach (var item in answerbox)
                {
                    PollAnswerModel model = new PollAnswerModel();
                    model.AnswerText = item.AnswerText;
                    model.AnswerKey = item.AnswerKey??default(int);
                    model.ID = item.ID;
                    model.Score = item.Score ?? default(int);
                    model.F_PollQusetion_Id = item.F_PollQuestion_Id ?? default(int);
                    model.Score = item.Score ?? default(int);
                    t.AnswerBoxes.Add(model);
                }
                //Gozine hara betartibe vared shode morattab mikonad
                t.AnswerBoxes=t.AnswerBoxes.OrderBy(r => r.AnswerKey).ToList();
                // baraye check kardane Faal budan ya nabudan
                if ((Founded.StartDateOnUTC < DateTime.Now || Founded.StartDateOnUTC == DateTime.Now) && (Founded.EndDateOnUTC > DateTime.Now || Founded.EndDateOnUTC == DateTime.Now))
                    t.Active = true;
                else
                    t.Active = false;
                return t;
            }
        }
    }
}