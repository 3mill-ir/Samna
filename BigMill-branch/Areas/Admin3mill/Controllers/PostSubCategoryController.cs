using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BigMill.Models;
using BigMill.Areas.Admin3mill.Models;
using PagedList;
using BigMill.CustomFilters;

namespace BigMill.Areas.Admin3mill.Controllers
{
    [AuthLog(Roles = "Developer")]
    public class PostSubCategoryController : Controller
    {
        /// <summary>
        /// in controller jahate list kardane zirshakhe haye shakheye morede nazar morede estefade gharar migirad
        /// </summary>
        /// <param name="PostCategory_Id">idie shakheye morede nazar</param>
        /// <param name="page">shomare safheye feeli dar paging</param>
        /// <returns>
        /// ActionResult : View(ListPostSubCategory)
        /// </returns>
        public ActionResult ListPostSubCategory(int PostCategory_Id, int? page)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = @Resource.Resource.PageTittle_ListPostSubCategory;
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_AddPostSubCategory, "AddPostSubCategory", "PostSubCategory", "btn-green ti-plus", "?PostCategoryId=" + PostCategory_Id));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *********************************
            @ViewBag.PostCategoryId = PostCategory_Id;
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            @ViewBag.PaginationCount = pageNumber;
            
            PostSubCategoryManagement post = new PostSubCategoryManagement();
            if (!post.isPostCategoryValid(PostCategory_Id)) { return View("~/Views/Shared/NotFoundFailed.cshtml"); }
            return View(post.ListPostSubCategory(PostCategory_Id).ToPagedList(pageNumber, pageSize));
        }

        /// <summary>
        /// in controller jahate generate kardane viewe afzudane zirshakheye jadid be shakheye morede nazar morede estefade gharar migirad
        /// tabe (ListTag) az classe TagManagement dar in controller farakhani shode ast
        /// </summary>
        /// <param name="PostCategoryId">idie shakheye morede nazar</param>
        /// <returns>
        /// ActionResult
        /// View(AddPostSubCategory) : dar surati ke shakheye morede nazar jahate afzudane zir shakhe be aan yaft shavad
        /// View("~/Views/Shared/NotFoundFailed.cshtml") : dar surati ke shakheye morede nazar jahate afzudane zirshakheye jadid yaft nashavad
        /// </returns>
        public ActionResult AddPostSubCategory(int PostCategoryId)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = Resource.Resource.PageTittle_AddPostSubCategory;
            ViewBag.PageTittle_Description = Resource.Resource.PageTittle_Description_AddPostSubCategory;
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_ListPostSubCategory, "ListPostSubCategory", "PostSubCategory", "btn-green ti-list", "?PostCategory_Id=" + PostCategoryId));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************
            TagManagement tag = new TagManagement();
            ViewBag.Tags = tag.ListTag();
            PostSubCategoryModel model = new PostSubCategoryModel();
            model.F_PostCategory_ID=PostCategoryId;
            PostSubCategoryManagement post = new PostSubCategoryManagement();
            if (!post.isPostCategoryValid(PostCategoryId)) { return View("~/Views/Shared/NotFoundFailed.cshtml"); }
            return View(model);
        }

        /// <summary>
        /// in controller jahate afzudane zirshakheye jadid be shakheye morede nazar morede estefade gharar migirad
        /// tabe (ListTag) az classe TagManagement dar in controller farakhani shode ast
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// ActionResult
        /// RedirectToAction("ListPostSubCategory", "PostSubCategory", new { PostCategory_Id = model.F_PostCategory_ID }) : dar surati ke afzudane shakheye jadid ba movaffaghiyyat anjam shavad
        /// View(AddPostSubCategory) : dar surati ke fiele khassi por nashode bashad ya eshkali dar sabte zirshakheye jad rokh dahad
        /// View("~/Views/Shared/NotFoundFailed.cshtml") :  dar surati ke shakheye morede nazar jahate afzudane zirshakhe be aan yaft nashavad
        /// </returns>
        [HttpPost]
        public ActionResult AddPostSubCategory(PostSubCategoryModel model)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = Resource.Resource.PageTittle_AddPostSubCategory;
            ViewBag.PageTittle_Description = Resource.Resource.PageTittle_Description_AddPostSubCategory;
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_ListPostSubCategory, "ListPostSubCategory", "PostSubCategory", "btn-green ti-list", "?PostCategory_Id=" + model.F_PostCategory_ID));
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
                PostSubCategoryManagement post = new PostSubCategoryManagement();
                if (post.AddPostSubCategory(model) == "NotFound") { return View("~/Views/Shared/NotFoundFailed.cshtml"); }

                return RedirectToAction("ListPostSubCategory", "PostSubCategory", new { PostCategory_Id = model.F_PostCategory_ID });
            }
            else
                @ViewBag.PostCatId = model.F_PostCategory_ID;
            return View(model);
        }
        /// <summary>
        /// in controller jahate generate kardane view e virayeshe zirshakhe ye morede nazar morede estefade gharar migirad
        /// tabe (ListTag) az classe TagManagement dar in controller farakhani shode ast
        /// tabe (GetTags) az classe PostSubCategoryManagement dar in controller farakhani shode ast
        /// </summary>
        /// <param name="PostCategory_Id">idie shakheye morede nazar</param>
        /// <param name="PostSubCategoryId">idie zirshakheye morede nazar</param>
        /// <returns>
        /// ActionResult
        /// View(EditPostSubCategory) : dar surati ke zirshakheye morede nazar baraye bazyabi ettelaatash yaft shavad
        /// View("~/Views/Shared/NotFoundFailed.cshtml") : dar surati ke zirshakheye morede nazar yaft nashavad
        /// </returns>
        public ActionResult EditPostSubCategory(int PostCategory_Id,int PostSubCategoryId)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = Resource.Resource.PageTittle_Tittle_EditPostSubCategory;
            ViewBag.PageTittle_Description = Resource.Resource.PageTittle_Description_EditPostSubCategory;
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_ListPostSubCategory, "ListPostSubCategory", "PostSubCategory", "btn-green ti-list", "?PostCategory_Id=" + PostCategory_Id));
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_AddPostSubCategory, "AddPostSubCategory", "PostSubCategory", "btn-green ti-plus", "?PostCategoryId=" + PostCategory_Id));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************
            @ViewBag.PostSubCatId = PostSubCategoryId;
            @ViewBag.PostCatId = PostCategory_Id;
            TagManagement tag = new TagManagement();
            ViewBag.Tags = tag.ListTag();
            using (BigMill.Models.Entities db = new Entities())
            {
                var temp = db.PostsSubCategories.FirstOrDefault(u => u.ID == PostSubCategoryId && u.F_PostsCategory_Id==PostCategory_Id);
                if (temp == null || temp.Status == false) { return View("~/Views/Shared/NotFoundFailed.cshtml"); }
                PostSubCategoryModel model = new PostSubCategoryModel();
                model.ID = temp.ID;
                model.Text = temp.Text;
                model.SeoName = temp.SeoName;
                model.IsView = temp.isView;
                model.F_PostCategory_ID = temp.F_PostsCategory_Id ?? default(int);
                model.Status = temp.Status ?? default(bool);
                model.Weight = temp.Weight ?? default(double);
                PostSubCategoryManagement pm = new PostSubCategoryManagement();
                model.TagsText = pm.GetTags(PostSubCategoryId);
                model.Tags = model.TagsText.Split('#').ToList();
                return View(model);
            }
        }

        /// <summary>
        /// in controller jahate virayeshe zirshakheye morede nazar morede estefade gharar migirad
        /// tabe (ListTag) az classe TagManagement dar in controller farakhani shode ast
        /// </summary>
        /// <param name="model"></param>
        /// <param name="PostCategory_Id">idie shakheye morede nazar</param>
        /// <param name="PostSubCategoryId">idie zir shakheye morede nazar</param>
        /// <returns>
        /// ActionResult
        /// RedirectToAction("ListPostSubCategory", "PostSubCategory", new { PostCategory_Id = model.F_PostCategory_ID }) : dar surati ke virayesh ba movaffaghiyyat anjam girad
        /// View(EditPostSubCategory) : dar surati ke fielde khassi por nashode bashad 
        /// View("~/Views/Shared/NotFoundFailed.cshtml") : dar surati ke zirshakheye morede nazar yaft nashavad ya Statuse aan False bashad
        /// </returns>
        [HttpPost]
        public ActionResult EditPostSubCategory(PostSubCategoryModel model,int PostCategory_Id, int PostSubCategoryId)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = Resource.Resource.PageTittle_Tittle_EditPostSubCategory;
            ViewBag.PageTittle_Description = Resource.Resource.PageTittle_Description_EditPostSubCategory;
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_ListPostSubCategory, "ListPostSubCategory", "PostSubCategory", "btn-green ti-list", "?PostCategory_Id=" + PostCategory_Id));
            PathLog.Add(new PageTitle(@Resource.Resource.PageTittle_AddPostSubCategory, "AddPostSubCategory", "PostSubCategory", "btn-green ti-plus", "?PostCategoryId=" + PostCategory_Id));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************
            @ViewBag.PostCatId = PostSubCategoryId;
            TagManagement tag = new TagManagement();
            ViewBag.Tags = tag.ListTag();
            if (string.IsNullOrEmpty(model.TagsText))
            {
                ModelState.AddModelError("TagsText", @Resource.Resource.View_ValidationError);
            }
            if (ModelState.IsValid)
            {
                PostSubCategoryManagement post = new PostSubCategoryManagement();

                if (post.EditPostSubCategory(model) == "NotFound") { return View("~/Views/Shared/NotFoundFailed.cshtml"); }
                return RedirectToAction("ListPostSubCategory", "PostSubCategory", new { PostCategory_Id = model.F_PostCategory_ID });

            }
            else
                return View(model);
        }


        /// <summary>
        /// in controller jahate taghir vaziyate zirshakheye morede nazar morede estefade gharar migirad
        /// </summary>
        /// <param name="PostSubCategoryId">idie zirshakheye morede nazar</param>
        /// <param name="Page">shomare safheye feeli dar paging</param>
        /// <returns>
        /// ActionResult
        /// RedirectToAction("ListPostSubCategory", "PostSubCategory", new { PostCategory_Id = temp.F_PostsCategory_Id, page = Page }) : dar surati ke taghire vaziyate zirshakheye morede nazar ba movaffaghiyyat anjam girad
        /// View("~/Views/Shared/NotFoundFailed.cshtml") : dar surati ke zirshakheye morede nazar yaft nashavad
        /// </returns>
        public ActionResult ChangeStatusPostSubCategory(int PostSubCategoryId, int Page)
        {
   
            using (BigMill.Models.Entities db = new Entities())
            {
                var temp = db.PostsSubCategories.FirstOrDefault(u => u.ID == PostSubCategoryId);
                if (temp == null) { return View("~/Views/Shared/NotFoundFailed.cshtml"); }
                PostSubCategoryModel model = new PostSubCategoryModel();
                model.ID = temp.ID;
                PostSubCategoryManagement post = new PostSubCategoryManagement();
                int scale = post.ChangeStatusPostSubCategory(model);
                return RedirectToAction("ListPostSubCategory", "PostSubCategory", new { PostCategory_Id = temp.F_PostsCategory_Id, page = Page });
            }
        }
    }
}