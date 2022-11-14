using BigMill.Areas.Admin3mill.Models;
using BigMill.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using BigMill.CustomFilters;

namespace BigMill.Areas.Admin3mill.Controllers
{
    [AuthLog(Roles = "Developer")]
    public class PostCategoryController : Controller
    {
        // GET: PostCategory
        /// <summary>
        /// in controller jahate list kardane shakhe ha morede estefade gharar migirad
        /// </summary>
        /// <param name="page">shomare safheye feeli dar paging</param>
        /// <returns>
        /// ActionResult : View(ListPostCategory)
        /// </returns>
        public ActionResult ListPostCategory(int? page)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = @Resource.Resource.PageTittle_Tittle_ListPostCategory;
            ViewBag.PageTittle_Description = Resource.Resource.PageTittle_Description_ListPostCategory;
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_AddPostCategory, "AddPostCategory", "PostCategory", "btn-green ti-plus", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *********************************
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            @ViewBag.PaginationCount = pageNumber;
            PostCategoryManagement post = new PostCategoryManagement();
            return View(post.ListPostCategory().ToPagedList(pageNumber, pageSize));
        }

        /// <summary>
        /// in controller jahte generate kardane view e afzudane shakheye jadid morede estefade gharar migirad
        /// </summary>
        /// <returns>
        /// ActionResult : View(AddPostCategory)
        /// </returns>
        public ActionResult AddPostCategory()
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = Resource.Resource.PageTittle_Tittle_AddPostCategory;
            ViewBag.PageTittle_Description = Resource.Resource.PageTittle_Description_AddPostCategory;
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_Tittle_ListPostCategory, "ListPostCategory", "PostCategory", "btn-green ti-list", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************
            TagManagement tag = new TagManagement();
            ViewBag.Tags = tag.ListTag();
            return View();
        }
        /// <summary>
        /// in controller jahate afzudane shakheye jadid morede estefade gharar migirad
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// ActionResult 
        /// RedirectToAction("ListPostCategory", "PostCategory") : dar surati ke afzudane shakheye jadid ba movaffaghiyat anjam shavad
        /// View(model) : dar surati ke fieldi por nashode bashad
        /// </returns>
        [HttpPost]
        public ActionResult AddPostCategory(PostCategoryModel model)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = Resource.Resource.PageTittle_Tittle_AddPostCategory;
            ViewBag.PageTittle_Description = Resource.Resource.PageTittle_Description_AddPostCategory;
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_Tittle_ListPostCategory, "ListPostCategory", "PostCategory", "btn-green ti-list", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************
            TagManagement tag = new TagManagement();
            ViewBag.Tags = tag.ListTag();
            if (string.IsNullOrEmpty(model.TagsText))
            {
                ModelState.AddModelError("TagsText", @Resource.Resource.View_ValidationError);
            }

            if (ModelState.IsValid)
            {
                PostCategoryManagement post = new PostCategoryManagement();
                int Scale = post.AddPostCategory(model);
                return RedirectToAction("ListPostCategory", "PostCategory");
            }
            else
                return View(model);
        }

        /// <summary>
        /// in controller jahate generate kardane view va baz gardani ettelaate lazeme zirshakheye morede nazar jahate virayesh morede estefade gharar migirad
        /// </summary>
        /// <param name="PostCategoryId">idie shakheye morede nazar</param>
        /// <returns>
        /// ActionResult
        /// View("~/Views/Shared/NotFoundFailed.cshtml") : dar surati ke shakheye morede nazar yaft nashavad ya statuse aan False bashad
        /// View(EditPostCategory) : dar surati ke ettelaat ba movaffaghiyat bazgardande shavad
        /// </returns>
        public ActionResult EditPostCategory(int PostCategoryId)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = Resource.Resource.PageTittle_Tittle_EditPostCategory;
            ViewBag.PageTittle_Description = Resource.Resource.PageTittle_Description_EditPostCategory;
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_Tittle_ListPostCategory, "ListPostCategory", "PostCategory", "btn-green ti-list", null));
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_AddPostCategory, "AddPostCategory", "PostCategory", "btn-green ti-plus", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************
            TagManagement tag = new TagManagement();
            ViewBag.Tags = tag.ListTag();
            using (BigMill.Models.Entities db = new Entities())
            {
                var temp = db.PostsCategories.FirstOrDefault(u => u.ID == PostCategoryId);
                if (temp == null || temp.Status == false) { return View("~/Views/Shared/NotFoundFailed.cshtml"); }

                PostCategoryModel model = new PostCategoryModel();
                model.ID = temp.ID;
                model.Text = temp.Text;
                model.IsView = temp.isView;
                model.SeoName = temp.SeoName;
                model.Status = temp.Status ?? default(bool);
                model.Weight = temp.Weight ?? default(double);
                PostCategoryManagement post = new PostCategoryManagement();
                model.TagsText = post.GetTags(PostCategoryId);
                model.Tags = model.TagsText.Split('#').ToList();
                return View(model);

            }
        }

        /// <summary>
        /// in controller jahate virayeshe shakheye morede nazar morede estefade gharar migirad
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// ActionResult
        /// View("~/Views/Shared/NotFoundFailed.cshtml") : dar surati ke shakheye morede nazar yaft nashavad ya statuse aan False bashad
        /// View(EditPostCategory) : dar surati ke virayesh ba movaffaghiyat anjam shavad
        /// </returns>
        [HttpPost]
        public ActionResult EditPostCategory(PostCategoryModel model)
        {

            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = Resource.Resource.PageTittle_Tittle_EditPostCategory;
            ViewBag.PageTittle_Description = Resource.Resource.PageTittle_Description_EditPostCategory;
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_Tittle_ListPostCategory, "ListPostCategory", "PostCategory", "btn-green ti-list", null));
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_AddPostCategory, "AddPostCategory", "PostCategory", "btn-green ti-plus", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************
            TagManagement tag = new TagManagement();
            ViewBag.Tags = tag.ListTag();
            if (string.IsNullOrEmpty(model.TagsText))
            {
                ModelState.AddModelError("TagsText", @Resource.Resource.View_ValidationError);
            }

            if (ModelState.IsValid)
            {
                PostCategoryManagement post = new PostCategoryManagement();
                if (post.EditPostCategory(model) == "NotFound") { return View("~/Views/Shared/NotFoundFailed.cshtml"); }
                return RedirectToAction("ListPostCategory", "PostCategory");

            }
            else
            {
                return View(model);
            }


        }

        /// <summary>
        /// in controller jahate taghir vaziyate shakheye morede nazar bekar miravad
        /// </summary>
        /// <param name="PostCategoryId">idie shakheye morede nazar</param>
        /// <param name="Page">shomare safheye feeli dar paging</param>
        /// <returns>
        /// ActionResult
        /// View("~/Views/Shared/NotFoundFailed.cshtml") : dar surati ke shakheye morede nazar yaft nashavad
        /// RedirectToAction("ListPostCategory", "PostCategory", new { page = Page }) :  dar surati ke taghir vaziyat ba movaffaghiyat anjam girad
        /// </returns>
        [HttpPost]
        public ActionResult ChangeStatusPostCategory(int PostCategoryId, int Page)
        {

            using (BigMill.Models.Entities db = new Entities())
            {
                var temp = db.PostsCategories.FirstOrDefault(u => u.ID == PostCategoryId);
                if (temp == null ) { return View("~/Views/Shared/NotFoundFailed.cshtml"); }
                PostCategoryModel model = new PostCategoryModel();
                model.ID = temp.ID;
                PostCategoryManagement post = new PostCategoryManagement();
                int scale = post.ChangeStatusPostCategory(model);
                return RedirectToAction("ListPostCategory", "PostCategory", new { page = Page });
            }

        }

    }
}