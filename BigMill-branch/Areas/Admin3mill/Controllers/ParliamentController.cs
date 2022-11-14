using BigMill.Models;
using CKSource.FileSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using PagedList;
using BigMill.Areas.Admin3mill.Models;
using BigMill.CustomFilters;

namespace BigMill.Areas.Admin3mill.Controllers
{
      [AuthLog(Roles = "Developer, Admin, Author")]
    public class ParliamentController : Controller
    {
        /////// Speech ///////////

        /// <summary>
        /// in controller jahate list kardane khabar haye Majles(sokhanrani ha va notgh ha) morede estefade gharar migirad
        /// khabar hayi ke vaziyate namayeshe aan ha false ast ham nemayesh dade mishavand vali status==false ha be manzeleye pak shode hastand va nemayesh dade nemishavand
        /// </summary>
        /// <param name="page">shomare safheye feeli dar paging</param>
        /// <returns>
        /// ActionResult : View(ListSpeech)
        /// </returns>
        public ActionResult ListSpeech(int? page)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = @Resource.Resource.PageTittle_ListSpeech;
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_AddSpeech, "AddSpeech", "Parliament", "btn-green ti-plus", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************
            PostManagement post = new PostManagement();
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            @ViewBag.PaginationCount = pageNumber;
            int total;
            var pagedList = new StaticPagedList<PostModel>(post.ListSpecificPostAdmin(int.Parse(System.Configuration.ConfigurationManager.AppSettings["SpeechID"]), pageNumber, pageSize, out total), pageNumber, pageSize, total);
            return View(pagedList);
        }
        /// <summary>
        /// in controller jahate generate kardane viewe afzudane khabare Majles(sokhanraniha va notgh ha)e jadid morede estefade gharar migirad
        /// tabe (ListTag) az classe TagManagement dar in controller farakhani shode ast
        /// </summary>
        /// <returns>
        /// ActionResult : View(AddSpeech)
        /// </returns>
        public ActionResult AddSpeech()
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = @Resource.Resource.PageTittle_AddSpeech;
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_ListSpeech, "ListSpeech", "Parliament", "btn-purple ti-view-list-alt", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************
            TagManagement tag = new TagManagement();
            ViewBag.Tags = tag.ListTag();
            return View();
        }
        /// <summary>
        /// in controller jahate afzudane khabare Majles(sokhanrani ha va notgh ha)e jadid morede estefade gharar migirad
        /// tavabe (ImageSave_MaintainAspect , F_UserId) az classe Tools dar in controller farakhani shode ast
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Content_Two">tasvire marbute be khabar</param>
        /// <returns>
        /// ActionResult : RedirectToAction("SpeechSuccess", "Parliament")
        /// </returns>
        [HttpPost]
        public ActionResult AddSpeech(PostModel model, HttpPostedFileBase Content_Two)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = @Resource.Resource.PageTittle_AddSpeech;
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_ListSpeech, "ListSpeech", "Parliament", "btn-purple ti-view-list-alt", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************
            if (Content_Two == null)
            {
                ModelState.AddModelError("Content_Two", @Resource.Resource.View_ValidationError);
            }
            if (string.IsNullOrEmpty(model.TagsText))
            {
                ModelState.AddModelError("TagsText", @Resource.Resource.View_ValidationError);
            }
            PostManagement post = new PostManagement();
            if (ModelState.IsValid)
            {
                model.Content_Two = Tools.ImageSave_MaintainAspect(Content_Two);
                model.F_Users_Id = Tools.F_UserId();
                int Scale = post.AddPost(model, int.Parse(System.Configuration.ConfigurationManager.AppSettings["SpeechID"]));
                TempData["Content_Two"] = model.Content_Two;
                TempData["Content_Three"] = model.Content_Three;
                TempData["pipoURL"] = "Parliament/ViewSpeech_Detail?PostId=" + Scale;
                return RedirectToAction("SpeechSuccess", "Parliament");
            }
            else
            {
                TagManagement tag = new TagManagement();
                ViewBag.Tags = tag.ListTag();
                if (model.TagsText != null)
                {
                    model.Tags = model.TagsText.Split('#').ToList();
                }
                return View(model);
            }
        }
        /// <summary>
        /// in controller view e ersale poste sabt shode be telegram ra generate mikonad
        /// </summary>
        /// <returns>
        /// ActionResult : View(SpeechSuccess)
        /// </returns>
        public ActionResult SpeechSuccess()
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = "موفقیت در ثبت";
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_ListAkhbarPublic, "ListSpeech", "Parliament", "btn-purple ti-view-list-alt", null));
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_AddSpeech, "AddSpeech", "Parliament", "btn-green ti-plus", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************
            PostModel model = new PostModel();
            model.Content_Two = TempData["Content_Two"].ToString();
            model.Content_Three = TempData["Content_Three"].ToString();
            string tempURL = TempData["pipoURL"].ToString();
            TempData["ImgPath"] = model.Content_Two;
            TempData["Caption"] = model.Content_Three;
            TempData["URL4Chanel"] = tempURL;
            return View(model);
        }
        /// <summary>
        /// in controller view e virayeshe khabare Majles(sokhanrani ha va notgh ha)e morede nazar ra generate mikonad
        /// </summary>
        /// <param name="PostId">idie poste morede nazar</param>
        /// <returns>
        /// ActionResult 
        /// View(EditSpeech) : dar surati ke khabar yaft shavad ettelaate khabar ra dar ghalebe view baz migardanad
        /// View("~/Views/Shared/NotFoundFailed.cshtml") : dar surati ke khabare morede nazar yaft nashavad ya statuse aan false bashad
        /// </returns>
        public ActionResult EditSpeech(int PostId)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = @Resource.Resource.PageTittle_EditSpeech;
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_ListSpeech, "ListSpeech", "Parliament", "btn-purple ti-view-list-alt", null));
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_AddSpeech, "AddSpeech", "Parliament", "btn-green ti-plus", null));
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_ChangeDisplaySpeech, "ChangeDisplaySpeech", "Parliament", "btn-green ti-plus", PostId.ToString()));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************
            PostModel model = new PostModel();
            using (BigMill.Models.Entities db = new Entities())
            {
                var item = db.Posts.FirstOrDefault(u => u.ID == PostId);
                if (item == null || item.Status == false) { return View("~/Views/Shared/NotFoundFailed.cshtml"); }
                model.Content_Four = Tools.ContentFour_Get(item.Content_Four);
                model.Content_One = item.Content_One;
                model.Content_Three = item.Content_Three;
                model.Content_Two = item.Content_Two;
                model.ID = PostId;
                model.Status = item.Status ?? default(bool);
                PostManagement p = new PostManagement();
                model.TagsText = p.GetTags(PostId);
                model.Tags = model.TagsText.Split('#').ToList();
            }
            FileManagement FM = new FileManagement();
            var temp = FM.LoadFiles().FirstOrDefault(u => u.ID == model.ID);
            ViewBag.VideoSrc = temp != null ? temp.File : "";
            TagManagement tag = new TagManagement();
            ViewBag.Tags = tag.ListTag();
            return View(model);
        }
        /// <summary>
        /// in controller jahate virayeshe khabare Majles(Sokhanraniha va notgh ha)e bekar miravad
        /// </summary>
        /// <param name="model"></param>
        /// <param name="newUploadImage">tasvire jadidi ke baaraye post dar nazar gerefte shode</param>
        /// <returns>
        /// ActionResult
        /// RedirectToAction("ListSpeech", "Parliament") : agar post be dorosti virayesh shavad
        /// View("~/Views/Shared/NotFoundFailed.cshtml") : dar surati ke khabare morede nazar yaft nashavad
        /// </returns>
        [HttpPost]
        public ActionResult EditSpeech(PostModel model, HttpPostedFileBase newUploadImage)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = @Resource.Resource.PageTittle_EditSpeech;
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_ListSpeech, "ListSpeech", "Parliament", "btn-purple ti-view-list-alt", null));
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_AddSpeech, "AddSpeech", "Parliament", "btn-green ti-plus", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************

            if (string.IsNullOrEmpty(model.TagsText))
            {
                ModelState.AddModelError("TagsText", @Resource.Resource.View_ValidationError);
            }
            PostManagement post = new PostManagement();
            if (ModelState.IsValid)
            {
                bool newPic = false;
                if (newUploadImage != null)
                {
                    // jahate monaseb saziye size ax ha
                    model.Content_Two = Tools.ImageSave_MaintainAspect(newUploadImage);
                    newPic = true;
                }

                model.F_Users_Id = Tools.F_UserId();
                if (post.EditPost(model, newPic) == "NotFound") { return View("~/Views/Shared/NotFoundFailed.cshtml"); }
                return RedirectToAction("ListSpeech", "Parliament");

            }
            FileManagement FM = new FileManagement();
            var temp = FM.LoadFiles().FirstOrDefault(u => u.ID == model.ID);
            ViewBag.VideoSrc = temp != null ? temp.File : "";
            model.Content_Two = model.Content_Two;
            TagManagement tag = new TagManagement();
            ViewBag.Tags = tag.ListTag();
            return View(model);
        }
        /// <summary>
        /// in controller jahate taghire vaziyate nemayeshe khabare Majles(Sokhanrani ha va notgh ha)e morede nazar bekar miravad
        /// </summary>
        /// <param name="PostId">idie poste morede nazar</param>
        /// <param name="Page">shomare safheye feeliye paging</param>
        /// <returns>
        /// ActionResult : RedirectToAction("ListSpeech", "Parliament", new { page = Page })
        /// </returns>
        [HttpPost]
        public ActionResult ChangeDisplaySpeech(int PostId, int Page)
        {
            PostModel model = new PostModel();
            model.ID = PostId;
            PostManagement post = new PostManagement();
            if (post.ChangeDisplayPost(model) == "NotFound") { return View("~/Views/Shared/NotFoundFailed.cshtml"); }
            // new { page = Page } baraye in ke vaghti be liste khabar ha baz migardim haman page i ke ghablan dar aan budim neshan dade shavad va be avval baz nagardad
            return RedirectToAction("ListSpeech", "Parliament", new { page = Page });
        }





        /////// Question ///////////
        /// <summary>
        /// in controller jahate list kardane khabar haye Majles(tazakkorat va soalat) morede estefade gharar migirad
        /// khabar hayi ke vaziyate namayeshe aan ha false ast ham nemayesh dade mishavand vali status==false ha be manzeleye pak shode hastand va nemayesh dade nemishavand
        /// </summary>
        /// <param name="page">shomare safheye feeli dar paging</param>
        /// <returns>
        /// ActionResult : View(ListQuestion)
        /// </returns>
        public ActionResult ListQuestion(int? page)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = @Resource.Resource.PageTittle_ListQuestion;
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_AddQuestion, "AddQuestion", "Parliament", "btn-green ti-plus", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************
            PostManagement post = new PostManagement();
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            @ViewBag.PaginationCount = pageNumber;
            int total;
            var pagedList = new StaticPagedList<PostModel>(post.ListSpecificPostAdmin(int.Parse(System.Configuration.ConfigurationManager.AppSettings["QuestionID"]), pageNumber, pageSize, out total), pageNumber, pageSize, total);
            return View(pagedList);
        }
        /// <summary>
        /// in controller jahate generate kardane viewe afzudane khabare Majles(tazakkorat va soalat)e jadid morede estefade gharar migirad
        /// tabe (ListTag) az classe TagManagement dar in controller farakhani shode ast
        /// </summary>
        /// <returns>
        /// ActionResult : View(AddQuestion)
        /// </returns>
        public ActionResult AddQuestion()
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = @Resource.Resource.PageTittle_AddQuestion;
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_ListQuestion, "ListQuestion", "Parliament", "btn-purple ti-view-list-alt", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************
            TagManagement tag = new TagManagement();
            ViewBag.Tags = tag.ListTag();
            return View();
        }
        /// <summary>
        /// in controller jahate afzudane khabare Majles(tazakkorat va soalat)e jadid morede estefade gharar migirad
        /// tavabe (ImageSave_MaintainAspect , F_UserId) az classe Tools dar in controller farakhani shode ast
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Content_Two">tasvire marbute be khabar</param>
        /// <returns>
        /// ActionResult : RedirectToAction("QuestionSuccess", "Parliament")
        /// </returns>
        [HttpPost]
        public ActionResult AddQuestion(PostModel model, HttpPostedFileBase Content_Two)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = @Resource.Resource.PageTittle_AddQuestion;
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_ListQuestion, "ListQuestion", "Parliament", "btn-purple ti-view-list-alt", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************

            if (Content_Two == null)
            {
                ModelState.AddModelError("Content_Two", @Resource.Resource.View_ValidationError);
            }
            if (string.IsNullOrEmpty(model.TagsText))
            {
                ModelState.AddModelError("TagsText", @Resource.Resource.View_ValidationError);
            }
            PostManagement post = new PostManagement();
            if (ModelState.IsValid)
            {
                model.Content_Two = Tools.ImageSave_MaintainAspect(Content_Two);
                model.F_Users_Id = Tools.F_UserId();
                int Scale = post.AddPost(model, int.Parse(System.Configuration.ConfigurationManager.AppSettings["QuestionID"]));
                TempData["Content_Two"] = model.Content_Two;
                TempData["Content_Three"] = model.Content_Three;
                TempData["pipoURL"] = "Parliament/ViewQuestion_Detail?PostId=" + Scale;
                return RedirectToAction("QuestionSuccess", "Parliament");
            }
            else
            {
                TagManagement tag = new TagManagement();
                ViewBag.Tags = tag.ListTag();
                if (model.TagsText != null)
                {
                    model.Tags = model.TagsText.Split('#').ToList();
                }
                return View(model);
            }
        }
        /// <summary>
        /// in controller view e ersale poste sabt shode be telegram ra generate mikonad
        /// </summary>
        /// <returns>
        /// ActionResult : View(QuestionSuccess)
        /// </returns>
        public ActionResult QuestionSuccess()
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = "موفقیت در ثبت";
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_ListQuestion, "ListQuestion", "Parliament", "btn-purple ti-view-list-alt", null));
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_AddQuestion, "AddQuestion", "Parliament", "btn-green ti-plus", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************
            PostModel model = new PostModel();
            model.Content_Two = TempData["Content_Two"].ToString();
            model.Content_Three = TempData["Content_Three"].ToString();
            string tempURL = TempData["pipoURL"].ToString();
            TempData["ImgPath"] = model.Content_Two;
            TempData["Caption"] = model.Content_Three;
            TempData["URL4Chanel"] = tempURL;
            return View(model);
        }
        /// <summary>
        /// in controller view e virayeshe khabare Majles(tazakkorat va soalat)e morede nazar ra generate mikonad
        /// </summary>
        /// <param name="PostId">idie poste morede nazar</param>
        /// <returns>
        /// ActionResult 
        /// View(EditQuestion) : dar surati ke khabar yaft shavad ettelaate khabar ra dar ghalebe view baz migardanad
        /// View("~/Views/Shared/NotFoundFailed.cshtml") : dar surati ke khabare morede nazar yaft nashavad ya statuse aan false bashad
        /// </returns>
        public ActionResult EditQuestion(int PostId)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = @Resource.Resource.PageTittle_EditQuestion;
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_ListQuestion, "ListQuestion", "Parliament", "btn-purple ti-view-list-alt", null));
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_AddQuestion, "AddQuestion", "Parliament", "btn-green ti-plus", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************
            PostModel model = new PostModel();
            using (BigMill.Models.Entities db = new Entities())
            {
                var item = db.Posts.FirstOrDefault(u => u.ID == PostId);
                if (item == null || item.Status == false) { return View("~/Views/Shared/NotFoundFailed.cshtml"); }
                model.Content_Four = Tools.ContentFour_Get(item.Content_Four);
                model.Content_One = item.Content_One;
                model.Content_Three = item.Content_Three;
                model.Content_Two = item.Content_Two;
                model.ID = PostId;
                model.Status = item.Status ?? default(bool);
                PostManagement p = new PostManagement();
                model.TagsText = p.GetTags(PostId);
                model.Tags = model.TagsText.Split('#').ToList();
            }
            FileManagement FM = new FileManagement();
            var temp = FM.LoadFiles().FirstOrDefault(u => u.ID == model.ID);
            ViewBag.VideoSrc = temp != null ? temp.File : "";
            TagManagement tag = new TagManagement();
            ViewBag.Tags = tag.ListTag();
            return View(model);
        }
        /// <summary>
        /// in controller jahate virayeshe khabare Majles(tazakkorat va soalat)e bekar miravad
        /// </summary>
        /// <param name="model"></param>
        /// <param name="newUploadImage">tasvire jadidi ke baaraye post dar nazar gerefte shode</param>
        /// <returns>
        /// ActionResult
        /// RedirectToAction("ListQuestion", "Parliament") : agar post be dorosti virayesh shavad
        /// View("~/Views/Shared/NotFoundFailed.cshtml") : dar surati ke khabare morede nazar yaft nashavad
        /// </returns>
        [HttpPost]
        public ActionResult EditQuestion(PostModel model, HttpPostedFileBase newUploadImage)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = @Resource.Resource.PageTittle_EditQuestion;
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_ListQuestion, "ListQuestion", "Parliament", "btn-purple ti-view-list-alt", null));
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_AddQuestion, "AddQuestion", "Parliament", "btn-green ti-plus", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************

            if (string.IsNullOrEmpty(model.TagsText))
            {
                ModelState.AddModelError("TagsText", @Resource.Resource.View_ValidationError);
            }
            PostManagement post = new PostManagement();
            if (ModelState.IsValid)
            {
                bool newPic = false;
                if (newUploadImage != null)
                {
                    // jahate monaseb saziye size ax ha
                    model.Content_Two = Tools.ImageSave_MaintainAspect(newUploadImage);
                    newPic = true;
                }

                model.F_Users_Id = Tools.F_UserId();
                if (post.EditPost(model, newPic) == "NotFound") { return View("~/Views/Shared/NotFoundFailed.cshtml"); }
                    return RedirectToAction("ListQuestion", "Parliament");
            
            }
            FileManagement FM = new FileManagement();
            var temp = FM.LoadFiles().FirstOrDefault(u => u.ID == model.ID);
            ViewBag.VideoSrc = temp != null ? temp.File : "";
            model.Content_Two = model.Content_Two;
            TagManagement tag = new TagManagement();
            ViewBag.Tags = tag.ListTag();
            return View(model);
        }
        /// <summary>
        /// in controller jahate taghire vaziyate nemayeshe khabare Majles(Tazakkorat va soalat)e morede nazar bekar miravad
        /// </summary>
        /// <param name="PostId">idie poste morede nazar</param>
        /// <param name="Page">shomare safheye feeliye paging</param>
        /// <returns>
        /// ActionResult : RedirectToAction("ListQuestion", "Parliament", new { page = Page })
        /// </returns>
        [HttpPost]
        public ActionResult ChangeDisplayQuestion(int PostId, int Page)
        {
            PostModel model = new PostModel();
            model.ID = PostId;
            PostManagement post = new PostManagement();
            if (post.ChangeDisplayPost(model) == "NotFound") { return View("~/Views/Shared/NotFoundFailed.cshtml"); }
            // new { page = Page } baraye in ke vaghti be liste khabar ha baz migardim haman page i ke ghablan dar aan budim neshan dade shavad va be avval baz nagardad
            return RedirectToAction("ListQuestion", "Parliament", new { page = Page });
        }



        //////////// Interview /////////
        /// <summary>
        /// in controller jahate list kardane khabar haye Majles(Mosahebe ha) morede estefade gharar migirad
        /// khabar hayi ke vaziyate namayeshe aan ha false ast ham nemayesh dade mishavand vali status==false ha be manzeleye pak shode hastand va nemayesh dade nemishavand
        /// </summary>
        /// <param name="page">shomare safheye feeli dar paging</param>
        /// <returns>
        /// ActionResult : View(ListInterview)
        /// </returns>
        public ActionResult ListInterview(int? page)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = @Resource.Resource.PageTittle_ListInterview;
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_AddInterview, "AddInterview", "Parliament", "btn-green ti-plus", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************
            PostManagement post = new PostManagement();
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            @ViewBag.PaginationCount = pageNumber;
            int total;
            var pagedList = new StaticPagedList<PostModel>(post.ListSpecificPostAdmin(int.Parse(System.Configuration.ConfigurationManager.AppSettings["InterviewID"]), pageNumber, pageSize, out total), pageNumber, pageSize, total);
            return View(pagedList);
        }
        /// <summary>
        /// in controller jahate generate kardane viewe afzudane khabare Majles(Mosahebe ha)e jadid morede estefade gharar migirad
        /// tabe (ListTag) az classe TagManagement dar in controller farakhani shode ast
        /// </summary>
        /// <returns>
        /// ActionResult : View(AddInterview)
        /// </returns>
        public ActionResult AddInterview()
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = @Resource.Resource.PageTittle_AddInterview;
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_ListInterview, "ListInterview", "Parliament", "btn-purple ti-view-list-alt", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************
            TagManagement tag = new TagManagement();
            ViewBag.Tags = tag.ListTag();
            return View();
        }
        /// <summary>
        /// in controller jahate afzudane khabare Majles(Mosahebe ha)e jadid morede estefade gharar migirad
        /// tavabe (ImageSave_MaintainAspect , F_UserId) az classe Tools dar in controller farakhani shode ast
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Content_Two">tasvire marbute be khabar</param>
        /// <returns>
        /// ActionResult : RedirectToAction("InterviewSuccess", "Parliament")
        /// </returns>
        [HttpPost]
        public ActionResult AddInterview(PostModel model, HttpPostedFileBase Content_Two)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = @Resource.Resource.PageTittle_AddInterview;
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_ListInterview, "ListInterview", "Parliament", "btn-purple ti-view-list-alt", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************

            if (Content_Two == null)
            {
                ModelState.AddModelError("Content_Two", @Resource.Resource.View_ValidationError);
            }
            if (string.IsNullOrEmpty(model.TagsText))
            {
                ModelState.AddModelError("TagsText", @Resource.Resource.View_ValidationError);
            }
            PostManagement post = new PostManagement();
            if (ModelState.IsValid)
            {
                model.Content_Two = Tools.ImageSave_MaintainAspect(Content_Two);
                model.F_Users_Id = Tools.F_UserId();
                int Scale = post.AddPost(model, int.Parse(System.Configuration.ConfigurationManager.AppSettings["InterviewID"]));
                TempData["Content_Two"] = model.Content_Two;
                TempData["Content_Three"] = model.Content_Three;
                TempData["pipoURL"] = "Parliament/ViewInterview_Detail?PostId=" + Scale;
                return RedirectToAction("InterviewSuccess", "Parliament");
            }
            else
            {
                TagManagement tag = new TagManagement();
                ViewBag.Tags = tag.ListTag();
                if (model.TagsText != null)
                {
                    model.Tags = model.TagsText.Split('#').ToList();
                }
                return View(model);
            }
        }
        /// <summary>
        /// in controller view e ersale poste sabt shode be telegram ra generate mikonad
        /// </summary>
        /// <returns>
        /// ActionResult : View(InterviewSuccess)
        /// </returns>
        public ActionResult InterviewSuccess()
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = "ثبت با موفقیت";
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_ListInterview, "ListInterview", "Parliament", "btn-purple ti-view-list-alt", null));
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_AddInterview, "AddInterview", "Parliament", "btn-green ti-plus", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************
            PostModel model = new PostModel();
            model.Content_Two = TempData["Content_Two"].ToString();
            model.Content_Three = TempData["Content_Three"].ToString();
            string tempURL = TempData["pipoURL"].ToString();
            TempData["ImgPath"] = model.Content_Two;
            TempData["Caption"] = model.Content_Three;
            TempData["URL4Chanel"] = tempURL;
            return View(model);
        }
        /// <summary>
        /// in controller view e virayeshe khabare Majles(Mosahebe ha)e morede nazar ra generate mikonad
        /// </summary>
        /// <param name="PostId">idie poste morede nazar</param>
        /// <returns>
        /// ActionResult 
        /// View(EditInterview) : dar surati ke khabar yaft shavad ettelaate khabar ra dar ghalebe view baz migardanad
        /// View("~/Views/Shared/NotFoundFailed.cshtml") : dar surati ke khabare morede nazar yaft nashavad ya statuse aan false bashad
        /// </returns>
        public ActionResult EditInterview(int PostId)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = @Resource.Resource.PageTittle_EditInterview;
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_ListInterview, "ListInterview", "Parliament", "btn-purple ti-view-list-alt", null));
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_AddInterview, "AddInterview", "Parliament", "btn-green ti-plus", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************
            PostModel model = new PostModel();
            using (BigMill.Models.Entities db = new Entities())
            {
                var item = db.Posts.FirstOrDefault(u => u.ID == PostId);
                if (item == null || item.Status == false) { return View("~/Views/Shared/NotFoundFailed.cshtml"); }
                model.Content_Four = Tools.ContentFour_Get(item.Content_Four);
                model.Content_One = item.Content_One;
                model.Content_Three = item.Content_Three;
                model.Content_Two = item.Content_Two;
                model.ID = PostId;
                model.Status = item.Status ?? default(bool);
                PostManagement p = new PostManagement();
                model.TagsText = p.GetTags(PostId);
                model.Tags = model.TagsText.Split('#').ToList();
            }
            FileManagement FM = new FileManagement();
            var temp = FM.LoadFiles().FirstOrDefault(u => u.ID == model.ID);
            ViewBag.VideoSrc = temp != null ? temp.File : "";
            TagManagement tag = new TagManagement();
            ViewBag.Tags = tag.ListTag();
            return View(model);
        }
        /// <summary>
        /// in controller jahate virayeshe khabare Majles(Mosahebe ha)e bekar miravad
        /// </summary>
        /// <param name="model"></param>
        /// <param name="newUploadImage">tasvire jadidi ke baaraye post dar nazar gerefte shode</param>
        /// <returns>
        /// ActionResult
        /// RedirectToAction("ListInterview", "Parliament") : agar post be dorosti virayesh shavad
        /// View("~/Views/Shared/NotFoundFailed.cshtml") : dar surati ke khabare morede nazar yaft nashavad
        /// </returns>
        [HttpPost]
        public ActionResult EditInterview(PostModel model, HttpPostedFileBase newUploadImage)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = @Resource.Resource.PageTittle_EditInterview;
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_ListInterview, "ListInterview", "Parliament", "btn-purple ti-view-list-alt", null));
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_AddInterview, "AddInterview", "Parliament", "btn-green ti-plus", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************
            if (string.IsNullOrEmpty(model.TagsText))
            {
                ModelState.AddModelError("TagsText", @Resource.Resource.View_ValidationError);
            }
            PostManagement post = new PostManagement();
            if (ModelState.IsValid)
            {
                bool newPic = false;
                if (newUploadImage != null)
                {
                    // jahate monaseb saziye size ax ha
                    model.Content_Two = Tools.ImageSave_MaintainAspect(newUploadImage);
                    newPic = true;
                }

                model.F_Users_Id = Tools.F_UserId();
                if (post.EditPost(model, newPic) == "NotFound") { return View("~/Views/Shared/NotFoundFailed.cshtml"); }
                    return RedirectToAction("ListInterview", "Parliament");
            }
            FileManagement FM = new FileManagement();
            var temp = FM.LoadFiles().FirstOrDefault(u => u.ID == model.ID);
            ViewBag.VideoSrc = temp != null ? temp.File : "";
            model.Content_Two = model.Content_Two;
            TagManagement tag = new TagManagement();
            ViewBag.Tags = tag.ListTag();
            return View(model);
        }
        /// <summary>
        /// in controller jahate taghire vaziyate nemayeshe khabare Majles(Mosahebe ha)e morede nazar bekar miravad
        /// </summary>
        /// <param name="PostId">idie poste morede nazar</param>
        /// <param name="Page">shomare safheye feeliye paging</param>
        /// <returns>
        /// ActionResult : RedirectToAction("ListInterview", "Parliament", new { page = Page })
        /// </returns>
        [HttpPost]
        public ActionResult ChangeDisplayInterview(int PostId, int Page)
        {
            PostModel model = new PostModel();
            model.ID = PostId;
            PostManagement post = new PostManagement();
            if (post.ChangeDisplayPost(model) == "NotFound") { return View("~/Views/Shared/NotFoundFailed.cshtml"); }
            // new { page = Page } baraye in ke vaghti be liste khabar ha baz migardim haman page i ke ghablan dar aan budim neshan dade shavad va be avval baz nagardad
            return RedirectToAction("ListInterview", "Parliament", new { page = Page });
        }



        //////////// Correspondence /////////

        /// <summary>
        /// in controller jahate list kardane khabar haye Majles(Ahamme mokatebat) morede estefade gharar migirad
        /// khabar hayi ke vaziyate namayeshe aan ha false ast ham nemayesh dade mishavand vali status==false ha be manzeleye pak shode hastand va nemayesh dade nemishavand
        /// </summary>
        /// <param name="page">shomare safheye feeli dar paging</param>
        /// <returns>
        /// ActionResult : View(ListCorrespondence)
        /// </returns>
        public ActionResult ListCorrespondence(int? page)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = @Resource.Resource.PageTittle_ListCorrespondence;
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_AddCorrespondence, "AddCorrespondence", "Parliament", "btn-green ti-plus", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************
            PostManagement post = new PostManagement();
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            @ViewBag.PaginationCount = pageNumber;
            int total;
            var pagedList = new StaticPagedList<PostModel>(post.ListSpecificPostAdmin(int.Parse(System.Configuration.ConfigurationManager.AppSettings["CorrespondenceID"]), pageNumber, pageSize, out total), pageNumber, pageSize, total);
            return View(pagedList);
        }
        /// <summary>
        /// in controller jahate generate kardane viewe afzudane khabare Majles(Ahamme mokatebat)e jadid morede estefade gharar migirad
        /// tabe (ListTag) az classe TagManagement dar in controller farakhani shode ast
        /// </summary>
        /// <returns>
        /// ActionResult : View(AddCorrespondence)
        /// </returns>
        public ActionResult AddCorrespondence()
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = @Resource.Resource.PageTittle_AddCorrespondence;
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_ListCorrespondence, "ListCorrespondence", "Parliament", "btn-purple ti-view-list-alt", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************
            TagManagement tag = new TagManagement();
            ViewBag.Tags = tag.ListTag();
            return View();
        }
        /// <summary>
        /// in controller jahate afzudane khabare Majles(Ahamme mokatebat)e jadid morede estefade gharar migirad
        /// tavabe (ImageSave_MaintainAspect , F_UserId) az classe Tools dar in controller farakhani shode ast
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Content_Two">tasvire marbute be khabar</param>
        /// <returns>
        /// ActionResult : RedirectToAction("CorrespondenceSuccess", "Parliament")
        /// </returns>
        [HttpPost]
        public ActionResult AddCorrespondence(PostModel model, HttpPostedFileBase Content_Two)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = @Resource.Resource.PageTittle_AddCorrespondence;
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_ListCorrespondence, "ListCorrespondence", "Parliament", "btn-purple ti-view-list-alt", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************

            if (Content_Two == null)
            {
                ModelState.AddModelError("Content_Two", @Resource.Resource.View_ValidationError);
            }
            if (string.IsNullOrEmpty(model.TagsText))
            {
                ModelState.AddModelError("TagsText", @Resource.Resource.View_ValidationError);
            }
            PostManagement post = new PostManagement();
            if (ModelState.IsValid)
            {
                model.Content_Two = Tools.ImageSave_MaintainAspect(Content_Two);
                model.F_Users_Id = Tools.F_UserId();
                int Scale = post.AddPost(model, int.Parse(System.Configuration.ConfigurationManager.AppSettings["CorrespondenceID"]));
                TempData["Content_Two"] = model.Content_Two;
                TempData["Content_Three"] = model.Content_Three;
                TempData["pipoURL"] = "Parliament/ViewCorrespondence_Detail?PostId=" + Scale;
                return RedirectToAction("CorrespondenceSuccess", "Parliament");
            }
            else
            {
                TagManagement tag = new TagManagement();
                ViewBag.Tags = tag.ListTag();
                if (model.TagsText != null)
                {
                    model.Tags = model.TagsText.Split('#').ToList();
                }
                return View(model);
            }
        }
        /// <summary>
        /// in controller view e ersale poste sabt shode be telegram ra generate mikonad
        /// </summary>
        /// <returns>
        /// ActionResult : View(CorrespondenceSuccess)
        /// </returns>
        public ActionResult CorrespondenceSuccess()
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = "ثبت با موفقیت";
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_ListCorrespondence, "ListCorrespondence", "Parliament", "btn-purple ti-view-list-alt", null));
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_AddCorrespondence, "AddCorrespondence", "Parliament", "btn-green ti-plus", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************
            PostModel model = new PostModel();
            model.Content_Two = TempData["Content_Two"].ToString();
            model.Content_Three = TempData["Content_Three"].ToString();
            string tempURL = TempData["pipoURL"].ToString();
            TempData["ImgPath"] = model.Content_Two;
            TempData["Caption"] = model.Content_Three;
            TempData["URL4Chanel"] = tempURL;
            return View(model);
        }
        /// <summary>
        /// in controller view e virayeshe khabare Majles(Ahamme mokatebat)e morede nazar ra generate mikonad
        /// </summary>
        /// <param name="PostId">idie poste morede nazar</param>
        /// <returns>
        /// ActionResult 
        /// View(EditCorrespondence) : dar surati ke khabar yaft shavad ettelaate khabar ra dar ghalebe view baz migardanad
        /// View("~/Views/Shared/NotFoundFailed.cshtml") : dar surati ke khabare morede nazar yaft nashavad ya statuse aan false bashad
        /// </returns>
        public ActionResult EditCorrespondence(int PostId)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = @Resource.Resource.PageTittle_EditCorrespondence;
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_ListCorrespondence, "ListCorrespondence", "Parliament", "btn-purple ti-view-list-alt", null));
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_AddCorrespondence, "AddCorrespondence", "Parliament", "btn-green ti-plus", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************
            PostModel model = new PostModel();
            using (BigMill.Models.Entities db = new Entities())
            {
                var item = db.Posts.FirstOrDefault(u => u.ID == PostId);
                if (item == null || item.Status == false) { return View("~/Views/Shared/NotFoundFailed.cshtml"); }
                model.Content_Four = Tools.ContentFour_Get(item.Content_Four);
                model.Content_One = item.Content_One;
                model.Content_Three = item.Content_Three;
                model.Content_Two = item.Content_Two;
                model.ID = PostId;
                model.Status = item.Status ?? default(bool);
                PostManagement p = new PostManagement();
                model.TagsText = p.GetTags(PostId);
                model.Tags = model.TagsText.Split('#').ToList();
            }
            FileManagement FM = new FileManagement();
            var temp = FM.LoadFiles().FirstOrDefault(u => u.ID == model.ID);
            ViewBag.VideoSrc = temp != null ? temp.File : "";
            TagManagement tag = new TagManagement();
            ViewBag.Tags = tag.ListTag();
            return View(model);
        }
        /// <summary>
        /// in controller jahate virayeshe khabare Majles(Ahamme mokatebat)e bekar miravad
        /// </summary>
        /// <param name="model"></param>
        /// <param name="newUploadImage">tasvire jadidi ke baaraye post dar nazar gerefte shode</param>
        /// <returns>
        /// ActionResult
        /// RedirectToAction("ListCorrespondence", "Parliament") : agar post be dorosti virayesh shavad
        /// View("~/Views/Shared/NotFoundFailed.cshtml") : dar surati ke khabare morede nazar yaft nashavad
        /// </returns>
        [HttpPost]
        public ActionResult EditCorrespondence(PostModel model, HttpPostedFileBase newUploadImage)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = @Resource.Resource.PageTittle_EditCorrespondence;
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_ListCorrespondence, "ListCorrespondence", "Parliament", "btn-purple ti-view-list-alt", null));
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_AddCorrespondence, "AddCorrespondence", "Parliament", "btn-green ti-plus", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************
            if (string.IsNullOrEmpty(model.TagsText))
            {
                ModelState.AddModelError("TagsText", @Resource.Resource.View_ValidationError);
            }
            PostManagement post = new PostManagement();
            if (ModelState.IsValid)
            {
                bool newPic = false;
                if (newUploadImage != null)
                {
                    // jahate monaseb saziye size ax ha
                    model.Content_Two = Tools.ImageSave_MaintainAspect(newUploadImage);
                    newPic = true;
                }

                model.F_Users_Id = Tools.F_UserId();
                if (post.EditPost(model, newPic) == "NotFound") { return View("~/Views/Shared/NotFoundFailed.cshtml"); }
                    return RedirectToAction("ListCorrespondence", "Parliament");
            }
            FileManagement FM = new FileManagement();
            var temp = FM.LoadFiles().FirstOrDefault(u => u.ID == model.ID);
            ViewBag.VideoSrc = temp != null ? temp.File : "";
            model.Content_Two = model.Content_Two;
            TagManagement tag = new TagManagement();
            ViewBag.Tags = tag.ListTag();
            return View(model);
        }
        /// <summary>
        /// in controller jahate taghire vaziyate nemayeshe khabare Majles(Ahamme mokatebat)e morede nazar bekar miravad
        /// </summary>
        /// <param name="PostId">idie poste morede nazar</param>
        /// <param name="Page">shomare safheye feeliye paging</param>
        /// <returns>
        /// ActionResult : RedirectToAction("ListCorrespondence", "Parliament", new { page = Page })
        /// </returns>
        [HttpPost]
        public ActionResult ChangeDisplayCorrespondence(int PostId, int Page)
        {
            PostModel model = new PostModel();
            model.ID = PostId;
            PostManagement post = new PostManagement();
            if (post.ChangeDisplayPost(model) == "NotFound") { return View("~/Views/Shared/NotFoundFailed.cshtml"); }
            // new { page = Page } baraye in ke vaghti be liste khabar ha baz migardim haman page i ke ghablan dar aan budim neshan dade shavad va be avval baz nagardad
            return RedirectToAction("ListCorrespondence", "Parliament", new { page = Page });
        }


    }
}