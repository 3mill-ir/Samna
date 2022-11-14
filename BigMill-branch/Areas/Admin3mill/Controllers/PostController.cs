using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BigMill.Models;

namespace BigMill.Areas.Admin3mill.Controllers
{
    public class PostController : Controller
    {
        // GET: Post
        public ActionResult ListPost()
        {
            PostManagement post = new PostManagement();
            return View(post.ListPost());
        }
        [ValidateInput(false)] 
        public ActionResult AddPost(int F_PostSubCategoryID)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = "اضافه کردن پست جدید";
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<string[]> PathLog = new List<string[]>();
            PathLog.Add(new string[] { "لیست پست ها", "ListPostCategory", "PostCategory" });
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************

            return View();
        }
        [ValidateInput(false)] 
        [HttpPost]
        public ActionResult AddPost(PostModel model, int F_PostSubCategoryID)
        {

            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = "اضافه کردن پست جدید";
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<string[]> PathLog = new List<string[]>();
            PathLog.Add(new string[] { "لیست پست ها", "ListPostCategory", "PostCategory" });
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************
            PostManagement post = new PostManagement();
            //if (ModelState.IsValid)
            {
                if (model.TagsText != null && model.TagsText != "")
                model.Tags = model.TagsText.Split(',').ToList();
                model.F_PostsSubCategory_ID = F_PostSubCategoryID;
                model.F_Users_Id = 3;
                int Scale = post.AddPost(model,3);
                if (Scale == 1)
                {
                    return View("ListPost");
                }
                else
                {
                    return View("Repetetious");
                }
            }
            //else
            //    return View("TryAgain");
        }

        public ActionResult EditPost(int PostId)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var temp = db.Posts.FirstOrDefault(u => u.ID == PostId);
                PostModel model = new PostModel();
                model.ID = temp.ID;
                model.Content_Four = temp.Content_Four;
                model.Content_Three = temp.Content_Three;
                model.Content_Two = temp.Content_Two;
                model.Content_One = temp.Content_One;
                model.Display = temp.Display ?? default(bool);
                model.F_PostsSubCategory_ID = temp.F_PostsSubCategory_Id ?? default(int);
                model.F_Users_Id = temp.F_User_Id ??default(int);
                model.NumberOfComments = temp.NumberOfComments ?? default(int);
                model.NumberOfDisLikes = temp.NumberOfDislikes ?? default(int);
                model.NumberOfLikes = temp.NumberOfLikes ?? default(int);
                model.NumberOfVisitors = temp.NumberOfVisitors ?? default(int);
                model.Status = temp.Status ?? default(bool);
                foreach (var item in model.Tags)
                {
                    model.TagsText += item + ",";
                }
                return View(model);
            }
        }
        [HttpPost]
        public ActionResult EditPost(PostModel model)
        {
            PostManagement post = new PostManagement();
            if (ModelState.IsValid)
            {
                model.Tags.Clear();
                model.Tags = model.TagsText.Split(',').ToList();
                int Scale = post.EditPost(model);
                if (Scale == 1)
                {
                    return View("ListPost");
                }
                else
                {
                    return View("Repetetious");
                }
            }
            else
                return View("TryAgain");
        }




        public ActionResult DeletePost(int PostId)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var temp = db.Posts.FirstOrDefault(u => u.ID == PostId);
                PostModel model = new PostModel();
                model.ID = temp.ID;
                model.Content_Four = temp.Content_Four;
                model.Content_Three = temp.Content_Three;
                model.Content_Two = temp.Content_Two;
                model.Content_One = temp.Content_One;
                model.Display = temp.Display ?? default(bool);
                model.F_PostsSubCategory_ID = temp.F_PostsSubCategory_Id ?? default(int);
                model.F_Users_Id = temp.F_User_Id ?? default(int);
                model.NumberOfComments = temp.NumberOfComments ?? default(int);
                model.NumberOfDisLikes = temp.NumberOfDislikes ?? default(int);
                model.NumberOfLikes = temp.NumberOfLikes ?? default(int);
                model.NumberOfVisitors = temp.NumberOfVisitors ?? default(int);
                model.Status = temp.Status ?? default(bool);
                return View(model);
            }
        }
        [HttpPost]
        public ActionResult DeletePost(PostModel model)
        {
            PostManagement post = new PostManagement();
            if (ModelState.IsValid)
            {
                int Scale = post.DeletePost(model);
                if (Scale == 1)
                {
                    return View("ListPost");
                }
                else
                {
                    return View("TryAgain");
                }
            }
            else
                return View("TryAgain");
        }

        public ActionResult ChangeStatusPost(int PostId)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var temp = db.Posts.FirstOrDefault(u => u.ID == PostId);
                PostModel model = new PostModel();
                model.ID = temp.ID;
                model.Content_Four = temp.Content_Four;
                model.Content_Three = temp.Content_Three;
                model.Content_Two = temp.Content_Two;
                model.Content_One = temp.Content_One;
                model.Display = temp.Display ?? default(bool);
                model.F_PostsSubCategory_ID = temp.F_PostsSubCategory_Id ?? default(int);
                model.F_Users_Id = temp.F_User_Id ?? default(int);
                model.NumberOfComments = temp.NumberOfComments ?? default(int);
                model.NumberOfDisLikes = temp.NumberOfDislikes ?? default(int);
                model.NumberOfLikes = temp.NumberOfLikes ?? default(int);
                model.NumberOfVisitors = temp.NumberOfVisitors ?? default(int);
                model.Status = temp.Status ?? default(bool);
                return View(model);
            }
        }
        [HttpPost]
        public ActionResult ChangeStatusPost(PostModel model)
        {
            PostManagement post = new PostManagement();
            if (ModelState.IsValid)
            {
                int Scale = post.ChangeDisplayPost(model);
                if (Scale == 1)
                {
                    return View("ListPost");
                }
                else
                {
                    return View("TryAgain");
                }
            }
            else
                return View("TryAgain");
        }
    }
}