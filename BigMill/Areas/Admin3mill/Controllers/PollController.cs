using BigMill.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.Text;
using PagedList;
using BigMill.Areas.Admin3mill.Models;
using System.Data.Entity;
using BigMill.CustomFilters;

namespace BigMill.Areas.Admin3mill.Controllers
{
    [AuthLog(Roles = "Developer, Admin, Author")]
    public class PollController : Controller
    {
        // GET: Admin3mill/Poll

        /// <summary>
        /// in controller jahate list kardane soalate nazar sanji mibashad va dar surate faal budan nazarsanji dar zamane hal aan ra moshakhass khahad kard
        /// </summary>
        /// <param name="page">shomare safheye feeli paging</param>
        /// <returns>
        /// ActionResult : View(ListPollQuestion)
        /// </returns>
        public ActionResult ListPollQuestion(int? page)
        {

            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = @Resource.Resource.PageTittle_ListPollQuestion;
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_AddPollQuestion, "AddPollQuestion", "Poll", "btn-green ti-plus", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *********************************
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            @ViewBag.PaginationCount = pageNumber;
            PollQuestionManagement poll = new PollQuestionManagement();
            return View(poll.ListPollQuestion().ToPagedList(pageNumber, pageSize));
        }
        /// <summary>
        /// in controller jahate faraham kardane viewe ezafe kardane soale nazar sanjiye jadid be hamrahe gozine haye pasokhe aan morede estefade gharar migirad
        /// </summary>
        /// <returns>
        /// ActionResult : View(AddPollQuestion)
        /// </returns>
        public ActionResult AddPollQuestion()
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = @Resource.Resource.PageTittle_AddPollQuestion;
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_ListPollQuestion, "ListPollQuestion", "Poll", "btn-purple ti-view-list-alt", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************
            PollQuestionModel model = new PollQuestionModel();
            model.AnswerBoxes.Add(new PollAnswerModel() { ID = 0 });
            model.AnswerBoxes.Add(new PollAnswerModel() { ID = 1 });
            return View(model);
        }

        /// <summary>
        /// in controller jahte ezafe kardane soale nazar sanjiye jadid be hamrahe gozine haye pasokhe aan morede estefade gharar migirad
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// ActionResult
        /// RedirectToAction("ListPollQuestion", "Poll") : dar surati ke soale nazar sanji va gozine haye pasokhe hamrahe aan ba movaffaghiyyat sabt shavand
        /// View(AddPollQuestion) : dar surati ke tadakhole zamani vojud dashte bashad ya dar sabte nazar sanjiye jadid moshkeli rokh dahad ya field ha por nashode bashand
        /// </returns>
        [HttpPost]
        public ActionResult AddPollQuestion(PollQuestionModel model)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = @Resource.Resource.PageTittle_AddPollQuestion;
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_ListPollQuestion, "ListPollQuestion", "Poll", "btn-purple ti-view-list-alt", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************

            DateTime start = new DateTime();
            DateTime end = new DateTime();
            if (!Tools.GetJalaliDateReturnDateTime(model.StartDateOnUTC, out start))
            {
                ModelState.AddModelError("StartDateOnUTC", @Resource.Resource.View_DateFormatError);
            }
            if (!Tools.GetJalaliDateReturnDateTime(model.EndDateOnUTC, out end))
            {
                ModelState.AddModelError("EndDateOnUTC", @Resource.Resource.View_DateFormatError);
            }


            if (ModelState.IsValid)
            {
                PollQuestionManagement poll = new PollQuestionManagement();


                int scale1 = poll.AddPollQuestion(model, start, end);
                if (scale1 == 1)
                    return RedirectToAction("ListPollQuestion", "Poll");
                else
                {
                    @ViewBag.DateConflict = @Resource.Resource.View_DateConflictError;
                    return View(model);
                }
            }
            else
                return View(model);
        }

        /// <summary>
        /// in controller jahate generate kardan va bazgardandane ettelaate soale nazar sanjiye morede nazar jahate virayesh morede estefade gharar migirad
        /// tabe staticiye (GetDateTimeReturnJalaliDate) az classe Tools dar in tabe farakhani shode ast
        /// </summary>
        /// <param name="PollQuestionId">idie soale nazarsanjiye morede nazar</param>
        /// <returns>
        /// ActionResult 
        /// View(EditPollQuestion) : dar surati ke amale virayeshe nazar sanjiye morede nazar ba movaffaghiyyat anjam girad
        /// View("~/Views/Shared/NotFoundFailed.cshtml") : dar surati ke nazarsanjiye morede nazar yaft nashode bashad ya statuse aan False Bude bashad
        /// </returns>
        public ActionResult EditPollQuestion(int PollQuestionId)
        {

            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = @Resource.Resource.PageTittle_EditPollQuestion;
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_ListPollQuestion, "ListPollQuestion", "Poll", "btn-purple ti-view-list-alt", null));
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_AddPollQuestion, "AddPollQuestion", "Poll", "btn-green ti-plus", null));
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_DetailPollQuestion, "DetailPollQuestion", "Poll", "btn-purple ti-bar-chart", "?PollQuestionId=" + PollQuestionId));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *********************************

            using (BigMill.Models.Entities db = new Entities())
            {
                PollQuestionModel model = new PollQuestionModel();
                var temp = db.PollQuestions.Include(z => z.PollAnswers).FirstOrDefault(u => u.ID == PollQuestionId);
                if (temp == null || temp.Status == false) { return View("~/Views/Shared/NotFoundFailed.cshtml"); }
                model.CreatedOnUTC = temp.CreatedOnUTC ?? default(DateTime);
                model.StartDateOnUTC = temp.StartDateOnUTC.ToString() ?? default(string);
                PersianCalendar p = new PersianCalendar();
                DateTime DateStarttoConvert = new DateTime();
                DateTime DateEndtoConvert = new DateTime();
                DateStarttoConvert = temp.StartDateOnUTC ?? default(DateTime);
                DateEndtoConvert = temp.EndDateOnUTC ?? default(DateTime);
                model.EndDateOnUTC = Tools.GetDateTimeReturnJalaliDate(DateEndtoConvert);
                model.StartDateOnUTC = Tools.GetDateTimeReturnJalaliDate(DateStarttoConvert);
                model.F_User_Id = temp.F_User_Id ?? default(int);
                model.ID = temp.ID;
                model.Question = temp.Question;
                foreach (var item in temp.PollAnswers)
                {
                    PollAnswerModel ans = new PollAnswerModel();
                    ans.AnswerText = item.AnswerText;
                    ans.ID = item.ID;
                    ans.AnswerKey = item.AnswerKey ?? default(int);
                    ans.F_PollQusetion_Id = item.F_PollQuestion_Id ?? default(int);
                    model.AnswerBoxes.Add(ans);
                }
                return View(model);
            }

        }
        /// <summary>
        /// in controller jahate virayeshe soale nazar sanji morede nazar va gozine haye pasokhe hamrahe aan bekar miravad
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// ActionResult
        /// RedirectToAction("ListPollQuestion", "Poll") : dar surati ke virayeshe nazar sanji ba movaffaghiyat surat girad
        /// View("~/Views/Shared/NotFoundFailed.cshtml") : dar surati ke nazar sanjiye morede nazar yaft nashode bashad ya statuse aan False bashad
        /// </returns>
        [HttpPost]
        public ActionResult EditPollQuestion(PollQuestionModel model)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = @Resource.Resource.PageTittle_EditPollQuestion;
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_ListPollQuestion, "ListPollQuestion", "Poll", "btn-purple ti-view-list-alt", null));
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_AddPollQuestion, "AddPollQuestion", "Poll", "btn-green ti-plus", null));
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_DetailPollQuestion, "DetailPollQuestion", "Poll", "btn-purple ti-bar-chart", "?PollQuestionId=" + model.ID));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *********************************
            DateTime start = new DateTime();
            DateTime end = new DateTime();
            if (!Tools.GetJalaliDateReturnDateTime(model.StartDateOnUTC, out start))
            {
                ModelState.AddModelError("StartDateOnUTC", @Resource.Resource.View_DateFormatError);
            }
            if (!Tools.GetJalaliDateReturnDateTime(model.EndDateOnUTC, out end))
            {
                ModelState.AddModelError("EndDateOnUTC", @Resource.Resource.View_DateFormatError);
            }

            if (ModelState.IsValid)
            {
                PollQuestionManagement poll = new PollQuestionManagement();
                model.F_User_Id = Tools.F_UserId();
                string result = poll.EditPollQuestion(model, start, end);
                if (result == "NotFound")
                {
                    return View("~/Views/Shared/NotFoundFailed.cshtml");
                }
                else if (result == "Conflict")
                {
                    @ViewBag.DateConflict = @Resource.Resource.View_DateConflictError;
                    return View(model);
                }
                else
                {
                    return RedirectToAction("ListPollQuestion", "Poll");
                }
            }
            return View(model);
        }

        /// <summary>
        /// in controller jahate taghire vaziyate soale nazar sanjiye morede nazar morede estefade gharar migirad
        /// </summary>
        /// <param name="PollQuestionId">idie soale nazar sanjiye morede nazar</param>
        /// <param name="Page">shomare safheye feeli dar paging</param>
        /// <returns>
        /// ActionResult : RedirectToAction("ListPollQuestion", "Poll", new { page = Page })
        /// </returns>
        [HttpPost]
        public ActionResult ChangeStatusPollQuestion(int PollQuestionId, int Page)
        {
            PollQuestionManagement post = new PollQuestionManagement();
            if (post.ChangeStatusPollQuestion(PollQuestionId) == "NotFound") { return View("~/Views/Shared/NotFoundFailed.cshtml"); }
            return RedirectToAction("ListPollQuestion", "Poll", new { page = Page });
        }
        /// <summary>
        /// in controller jahate namayeshe joziyate soale nazar sanji be hamrahe gozine haye pasokhe aan morede estefade gharar migirad
        /// </summary>
        /// <param name="PollQuestionId">idie soale nazar sanjiye morede nazar</param>
        /// <returns>
        /// ActionResult 
        /// View(DetailPollQuestion) : dar surati ke joziyat ba movaffaghiyyat bazgardande shavad
        /// View("~/Views/Shared/NotFoundFailed.cshtml") : dar surati ke soale nazar sanjiye morede nazar yaft nashavad
        /// </returns>
        public ActionResult DetailPollQuestion(int PollQuestionId)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = @Resource.Resource.PageTittle_DetailPollQuestion;
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_ListPollQuestion, "ListPollQuestion", "Poll", "btn-purple ti-view-list-alt", null));
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_AddPollQuestion, "AddPollQuestion", "Poll", "btn-green ti-plus", null));
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_EditPollQuestion, "EditPollQuestion", "Poll", "btn-purple ti-pencil", "?PollQuestionId=" + PollQuestionId));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *********************************
            string temp = (TempData["DeleteError"]==null) ? null :TempData["DeleteError"].ToString();
            @ViewBag.ErrorinDelete = temp;
            PollQuestionManagement post = new PollQuestionManagement();
            PollQuestionModel model = new PollQuestionModel();
            model = post.PollQuestionDetail(PollQuestionId);
            if (model == null) { return View("~/Views/Shared/NotFoundFailed.cshtml"); }
            return View(model);
        }

        /// <summary>
        /// in controller jahate pak kardane gozineye pasokhe morede nazar az yek soale nazar sanji bekar miravad
        /// </summary>
        /// <param name="PollAnswerId">idie pasokhe nazar sanjiye morede nazar</param>
        /// <param name="PollQuestionID">idie soale nazar sanjiye morede nazar</param>
        /// <returns>
        /// ActionResult
        /// RedirectToAction("DetailPollQuestion", "Poll", new { PollQuestionId = PollQuestionID }) : dar surati ke amale pak kardan ba movaffaghiyat anjam shavad
        /// View("~/Views/Shared/NotFoundFailed.cshtml") :  dar surati ke pasokhe nazar sanjiye morede nazar yaft nashode bashad
        /// </returns>
        [HttpPost]
        public ActionResult DeletePollAnswer(int PollAnswerId, int PollQuestionID)
        {
            PollAnswerManagement post = new PollAnswerManagement();
            string result=post.DeletePollAnswer(PollAnswerId, PollQuestionID);
            if (result == "NotFound") { return View("~/Views/Shared/NotFoundFailed.cshtml"); }
            else if(result=="MinCount"){
                TempData["DeleteError"] = "حداقل بایستی دو گزینه برای نظر سنجی وجود داشته باشد.";
            }
                return RedirectToAction("DetailPollQuestion", "Poll", new { PollQuestionId = PollQuestionID });
        }
    }
}