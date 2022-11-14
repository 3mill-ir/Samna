using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BigMill.Models;
using CKSource.FileSystem;
using System.Web.Helpers;
using PagedList;
using System.Threading.Tasks;
using BigMill.Areas.Admin3mill.Models;
using BigMill.CustomFilters;

namespace BigMill.Areas.Admin3mill.Controllers
{
    [AuthLog(Roles = "Developer, Admin, Author")]
    public class AkhbarController : Controller
    {
        /// <summary>
        /// in controller jahate list kardane khabar haye omumi morede estefade gharar migirad
        /// khabar hayi ke vaziyate namayeshe aan ha false ast ham nemayesh dade mishavand vali status==false ha be manzeleye pak shode hastand va nemayesh dade nemishavand
        /// </summary>
        /// <param name="page">shomare safheye feeli dar paging</param>
        /// <returns>
        /// ActionResult : View(ListAkhbarPublic)
        /// </returns>
        public ActionResult ListAkhbarPublic(int? page)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = @Resource.Resource.PageTittle_ListAkhbarPublic;
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_AddPublicKhabar, "AddPublicKhabar", "Akhbar", "btn-green ti-plus", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *********************************
            PostManagement post = new PostManagement();
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            @ViewBag.PaginationCount = pageNumber;
            int total;
            var pagedList = new StaticPagedList<PostModel>(post.ListSpecificPostAdmin(int.Parse(System.Configuration.ConfigurationManager.AppSettings["AkhbarPublicID"]), pageNumber, pageSize, out total), pageNumber, pageSize, total);
            return View(pagedList);
        }

        /// <summary>
        /// in controller jahate list kardane khabar haye hoze morede estefade gharar migirad
        /// khabar hayi ke vaziyate namayeshe aan ha false ast ham nemayesh dade mishavand vali status==false ha be manzeleye pak shode hastand va nemayesh dade nemishavand
        /// </summary>
        /// <param name="page">shomare safheye feeli dar paging</param>
        /// <returns>
        /// ActionResult : View(ListAkhbarHoze)
        /// </returns>
        public ActionResult ListAkhbarHoze(int? page)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = @Resource.Resource.PageTittle_ListAkhbarHoze;
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_AddHozeKhabar, "AddHozeKhabar", "Akhbar", "btn-green ti-plus", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************
            PostManagement post = new PostManagement();
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            @ViewBag.PaginationCount = pageNumber;
            int total;
            var pagedList = new StaticPagedList<PostModel>(post.ListSpecificPostAdmin(int.Parse(System.Configuration.ConfigurationManager.AppSettings["AkhbarHozeID"]), pageNumber, pageSize, out total), pageNumber, pageSize, total);
            return View(pagedList);
        }

        /// <summary>
        /// in controller jahate generate kardane viewe afzudane khabare omumie jadid morede estefade gharar migirad
        /// tabe (ListTag) az classe TagManagement dar in controller farakhani shode ast
        /// </summary>
        /// <returns>
        /// ActionResult : View(AddPublicKhabar)
        /// </returns>
        public ActionResult AddPublicKhabar()
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = @Resource.Resource.PageTittle_AddPublicKhabar;
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_ListAkhbarHoze, "ListAkhbarPublic", "Akhbar", "btn-purple ti-list", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************
            TagManagement tag = new TagManagement();
            ViewBag.Tags = tag.ListTag();
            return View();
        }
        /// <summary>
        /// in controller jahate afzudane khabare omumie jadid morede estefade gharar migirad
        /// tavabe (ImageSave_MaintainAspect , F_UserId) az classe Tools dar in controller farakhani shode ast
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Content_Two">tasvire marbute be khabar</param>
        /// <returns>
        /// ActionResult : RedirectToAction("PublicKhabarSucces","Akhbar")
        /// </returns>
        [HttpPost]
        public ActionResult AddPublicKhabar(PostModel model, HttpPostedFileBase Content_Two)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = @Resource.Resource.PageTittle_AddPublicKhabar;
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_ListAkhbarHoze, "ListAkhbarPublic", "Akhbar", "btn-purple ti-list", null));
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
                int Scale = post.AddPost(model, int.Parse(System.Configuration.ConfigurationManager.AppSettings["AkhbarPublicID"]));
                TempData["Content_Two"] = model.Content_Two;
                TempData["Content_Three"] = model.Content_Three;
                TempData["pipoURL"] = "News/AkhbarOmumi_Detail?PostId=" + Scale;
                return RedirectToAction("PublicKhabarSucces", "Akhbar");
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
        /// in controller jahate generate kardane viewe afzudane khabare hoze jadid morede estefade gharar migirad
        /// tabe (ListTag) az classe TagManagement dar in controller farakhani shode ast
        /// </summary>
        /// <returns>
        /// ActionResult : View(AddHozeKhabar)
        /// </returns>
        public ActionResult AddHozeKhabar()
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = @Resource.Resource.PageTittle_AddHozeKhabar;
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_ListAkhbarHoze, "ListAkhbarHoze", "Akhbar", "btn-purple ti-view-list-alt", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************
            TagManagement tag = new TagManagement();
            ViewBag.Tags = tag.ListTag();
            return View();
        }
        /// <summary>
        /// in controller jahate afzudane khabare hoze jadid morede estefade gharar migirad
        /// tavabe (ImageSave_MaintainAspect , F_UserId) az classe Tools dar in controller farakhani shode ast
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Content_Two">tasvire marbute be khabar</param>
        /// <returns>
        /// ActionResult : RedirectToAction("HozeKhabarSucces", "Akhbar")
        /// </returns>
        [HttpPost]
        public ActionResult AddHozeKhabar(PostModel model, HttpPostedFileBase Content_Two)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = @Resource.Resource.PageTittle_AddHozeKhabar;
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_ListAkhbarHoze, "ListAkhbarHoze", "Akhbar", "btn-purple ti-view-list-alt", null));
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
                // jahate monaseb saziye size tasvir
                model.Content_Two = Tools.ImageSave_MaintainAspect(Content_Two);
                model.F_Users_Id = Tools.F_UserId();
                int Scale = post.AddPost(model, int.Parse(System.Configuration.ConfigurationManager.AppSettings["AkhbarHozeID"]));
                TempData["Content_Two"] = model.Content_Two;
                TempData["Content_Three"] = model.Content_Three;
                TempData["pipoURL"] = "News/AkhbarHozeEntekhabiye_Detail?PostId=" + Scale;
                return RedirectToAction("HozeKhabarSucces", "Akhbar");
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
        /// ActionResult : View(PublicKhabarSucces)
        /// </returns>

        public ActionResult PublicKhabarSucces()
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = @Resource.Resource.PageTittle_PublicKhabarSucces;
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_ListAkhbarPublic, "ListAkhbarPublic", "Akhbar", "btn-purple ti-view-list-alt", null));
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_AddPublicKhabar, "AddPublicKhabar", "Akhbar", "btn-green ti-plus", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************
            PostModel model = new PostModel();
            model.Content_Two = TempData["Content_Two"].ToString();
            model.Content_Three = TempData["Content_Three"].ToString();
            string tempURL = TempData["pipoURL"].ToString();
            TempData["ImgPath"] = TempData["Content_Two"].ToString();
            TempData["Caption"] = model.Content_Three;
            TempData["URL4Chanel"] = tempURL;
            return View(model);
        }
        /// <summary>
        /// in controller view e ersale poste sabt shode be telegram ra generate mikonad
        /// </summary>
        /// <returns>
        /// ActionResult : View(HozeKhabarSucces)
        /// </returns>
        public ActionResult HozeKhabarSucces()
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = "موفقیت در ثبت خبر حوزه انتخابیه";
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_ListAkhbarHoze, "ListAkhbarHoze", "Akhbar", "btn-purple ti-view-list-alt", null));
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_AddHozeKhabar, "AddHozeKhabar", "Akhbar", "btn-green ti-plus", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************
            PostModel model = new PostModel();
            model.Content_Two = TempData["Content_Two"].ToString();
            model.Content_Three = TempData["Content_Three"].ToString();
            string tempURL = TempData["pipoURL"].ToString();
            TempData["Caption"] = model.Content_Three;
            TempData["ImgPath"] = model.Content_Two;
            TempData["URL4Chanel"] = tempURL;
            return View(model);
        }

        /// <summary>
        /// in controller view e virayeshe khabare omumiye morede nazar ra generate mikonad
        /// </summary>
        /// <param name="PostId">idie poste morede nazar</param>
        /// <returns>
        /// ActionResult 
        /// View(EditPublicKhabar) : dar surati ke khabar yaft shavad ettelaate khabar ra dar ghalebe view baz migardanad
        /// View("~/Views/Shared/NotFoundFailed.cshtml") : dar surati ke khabare morede nazar yaft nashavad ya statuse aan false bashad
        /// </returns>
        public ActionResult EditPublicKhabar(int PostId)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = @Resource.Resource.PageTittle_EditPublicKhabar;
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_ListAkhbarPublic, "ListAkhbarPublic", "Akhbar", "btn-purple ti-view-list-alt", null));
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_AddPublicKhabar, "AddPublicKhabar", "Akhbar", "btn-green ti-plus", null));
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
        /// in controller jahate virayeshe khabare omumi bekar miravad
        /// </summary>
        /// <param name="model"></param>
        /// <param name="newUploadImage">tasvire jadidi ke baaraye post dar nazar gerefte shode</param>
        /// <returns>
        /// ActionResult
        /// RedirectToAction("ListAkhbarPublic", "Akhbar") : agar post be dorosti virayesh shavad
        /// View("~/Views/Shared/NotFoundFailed.cshtml") : dar surati ke khabare morede nazar yaft nashavad
        /// </returns>
        [HttpPost]
        public ActionResult EditPublicKhabar(PostModel model, HttpPostedFileBase newUploadImage)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = @Resource.Resource.PageTittle_EditPublicKhabar;
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_ListAkhbarPublic, "ListAkhbarPublic", "Akhbar", "btn-purple ti-view-list-alt", null));
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_AddPublicKhabar, "AddPublicKhabar", "Akhbar", "btn-green ti-plus", null));
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
                return RedirectToAction("ListAkhbarPublic", "Akhbar");

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
        /// in controller view e virayeshe khabare morede nazar ra generate mikonad
        /// </summary>
        /// <param name="PostId">idie poste morede nazar</param>
        /// <returns>
        /// ActionResult 
        /// View(EditHozeKhabar) : dar surati ke khabar yaft shavad ettelaate khabar ra dar ghalebe view baz migardanad
        /// View("~/Views/Shared/NotFoundFailed.cshtml") : dar surati ke khabare morede nazar yaft nashavad ya statuse aan false bashad
        /// </returns>
        public ActionResult EditHozeKhabar(int PostId)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = @Resource.Resource.PageTittle_EditPublicKhabar;
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_ListAkhbarHoze, "ListAkhbarHoze", "Akhbar", "btn-purple ti-view-list-alt", null));
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_AddHozeKhabar, "AddHozeKhabar", "Akhbar", "btn-green ti-plus", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************
            PostModel model = new PostModel();
            using (BigMill.Models.Entities db = new Entities())
            {
                var item = db.Posts.FirstOrDefault(u => u.ID == PostId);
                if (item == null || item.Status == false) { return View("~/Views/Shared/NotFoundFailed.cshtml"); }
                model.Content_Four = System.IO.File.ReadAllText(Server.MapPath("~/App_Data/ContentFour/" + item.Content_Four));
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
        /// in controller jahate virayeshe khabare hozeye bekar miravad
        /// </summary>
        /// <param name="model"></param>
        /// <param name="newUploadImage">tasvire jadidi ke baaraye post dar nazar gerefte shode</param>
        /// <returns>
        /// ActionResult
        /// RedirectToAction("ListAkhbarHoze", "Akhbar") : agar be dorosti post virayesh shavad
        /// View("~/Views/Shared/NotFoundFailed.cshtml") : dar surati ke khabare morede nazar yaft nashavad
        /// </returns>
        [HttpPost]
        public ActionResult EditHozeKhabar(PostModel model, HttpPostedFileBase newUploadImage)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = @Resource.Resource.PageTittle_EditPublicKhabar;
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_ListAkhbarHoze, "ListAkhbarHoze", "Akhbar", "btn-purple ti-view-list-alt", null));
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_AddHozeKhabar, "AddHozeKhabar", "Akhbar", "btn-green ti-plus", null));
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
                    model.Content_Two = Tools.ImageSave_MaintainAspect(newUploadImage);
                    newPic = true;
                }

                model.F_Users_Id = Tools.F_UserId();
                if (post.EditPost(model, newPic) == "NotFound") { return View("~/Views/Shared/NotFoundFailed.cshtml"); }
                return RedirectToAction("ListAkhbarHoze", "Akhbar");
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
        /// in controller jahate taghire vaziyate nemayeshe khabare omumi morede nazar bekar miravad
        /// </summary>
        /// <param name="PostId">idie poste morede nazar</param>
        /// <param name="Page">shomare safheye feeliye paging</param>
        /// <returns>
        /// ActionResult : RedirectToAction("ListAkhbarPublic", "Akhbar", new { page = Page })
        /// </returns>
        [HttpPost]
        public ActionResult ChangeDisplayPublicKhabar(int PostId, int Page)
        {
            PostModel model = new PostModel();
            model.ID = PostId;
            PostManagement post = new PostManagement();
            if (post.ChangeDisplayPost(model) == "NotFound") { return View("~/Views/Shared/NotFoundFailed.cshtml"); }
            // new { page = Page } baraye in ke vaghti be liste khabar ha baz migardim haman page i ke ghablan dar aan budim neshan dade shavad va be avval baz nagardad
            return RedirectToAction("ListAkhbarPublic", "Akhbar", new { page = Page });
        }

        /// <summary>
        /// in controller jahate taghire vaziyate nemayeshe khabare hoze morede nazar bekar miravad
        /// </summary>
        /// <param name="PostId">idie poste morede nazar</param>
        /// <param name="Page">shomare safheye feeliye paging</param>
        /// <returns>
        /// ActionResult : RedirectToAction("ListAkhbarHoze", "Akhbar", new { page = Page })
        /// </returns>
        [HttpPost]
        public ActionResult ChangeDisplayHozeKhabar(int PostId, int Page)
        {
            PostModel model = new PostModel();
            model.ID = PostId;
            PostManagement post = new PostManagement();
            if (post.ChangeDisplayPost(model) == "NotFound") { return View("~/Views/Shared/NotFoundFailed.cshtml"); }
            // new { page = Page } baraye in ke vaghti be liste khabar ha baz migardim haman page i ke ghablan dar aan budim neshan dade shavad va be avval baz nagardad
            return RedirectToAction("ListAkhbarHoze", "Akhbar", new { page = Page });
        }


    }
}