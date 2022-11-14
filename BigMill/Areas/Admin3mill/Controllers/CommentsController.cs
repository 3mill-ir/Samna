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
    [AuthLog(Roles = "Developer, Admin, Author")]
    public class CommentsController : Controller
    {

        /// <summary>
        /// in controller jahate list kardane comment ha va reply ha jahate taeed ya radd ya pak kardan morede estefade gharar migirad
        /// </summary>
        /// <param name="page">shomare safheye feeli dar paging</param>
        /// <returns>
        /// ActionResult : View(ListComment)
        /// </returns>
        public ActionResult ListComment(int? page)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = Resource.Resource.PageTittle_Tittle_ListComment;
            ViewBag.PageTittle_Description = Resource.Resource.PageTittle_Description_ListComment;
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *****************************
            PostCommentManagement comment = new PostCommentManagement();
            PostCommentReplyManagement commentReply = new PostCommentReplyManagement();
            List<PostCommentModel> list = new List<PostCommentModel>();
            list = comment.ListPostComment();
            foreach (var item in commentReply.ListPostCommentReply())
            {
                PostCommentModel ListItem = new PostCommentModel();
                ListItem.ID = item.ID;
                ListItem.CreateOnUtc = item.CreateOnUTC;
                ListItem.CreateDateUtcJalali = Tools.GetDateTimeReturnJalaliDate(item.CreateOnUTC);
                ListItem.Display = item.Display;
                ListItem.F_Posts_Id = item.F_PostsComments_Id;
                ListItem.NumberOfDislikes = item.NumberOfDislikes;
                ListItem.NumberOfLikes = item.NumberOfLikes;
                ListItem.NumberOfReply = -1;
                ListItem.Text = item.Text;
                list.Add(ListItem);
            }
            list = list.OrderByDescending(u => u.CreateOnUtc).ToList();

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            @ViewBag.PaginationCount = pageNumber;
            return View(list.ToPagedList(pageNumber, pageSize));
        }


        /// <summary>
        /// in controller jahate taghir vaziyate commente morede nazar bekar miravad
        /// </summary>
        /// <param name="CommentsID">idie commente morede nazar</param>
        /// <param name="Page">shomare safheye feeli dar paging</param>
        /// <returns>
        /// ActionResult : RedirectToAction("ListComment", "Comments", new { page = Page })
        /// </returns>
        [HttpPost]
        public ActionResult ChangeStatusComments(int CommentsID, int Page)
        {
            PostCommentManagement comment = new PostCommentManagement();
            if (comment.ChangeStatusPostComments(CommentsID) == "NotFound") { return View("~/Views/Shared/NotFoundFailed.cshtml"); }
            return RedirectToAction("ListComment", "Comments", new { page = Page });

        }

        /// <summary>
        /// in controller jahate taghir vaziyate Replye morede nazar bekar miravad
        /// </summary>
        /// <param name="CommentsReplyID">idie Replye morede nazar</param>
        /// <returns>
        /// ActionResult : RedirectToAction("ListComment", "Comments")
        /// </returns>
        [HttpPost]
        public ActionResult ChangeStatusCommentsReply(int CommentsReplyID)
        {
            PostCommentReplyManagement comment = new PostCommentReplyManagement();
            if (comment.ChangeStatusPostCommentReply(CommentsReplyID) == "NotFound") { return View("~/Views/Shared/NotFoundFailed.cshtml"); }
            return RedirectToAction("ListComment", "Comments");

        }

        /// <summary>
        /// in controller jahate pak kardane commente morede nazar bekar miravad
        /// </summary>
        /// <param name="CommentsID">idie commente morede nazar</param>
        /// <returns>
        /// ActionResult : RedirectToAction("ListComment", "Comments")
        /// </returns>
        [HttpPost]
        public ActionResult DeleteComments(int CommentsID)
        {
            PostCommentManagement comment = new PostCommentManagement();
            if (comment.PermanentlyDeletePostComment(CommentsID) == "NotFound") { return View("~/Views/Shared/NotFoundFailed.cshtml"); }
            return RedirectToAction("ListComment", "Comments");
        }

        /// <summary>
        /// in controller jahate pak kardane Replye morede nazar bekar miravad
        /// </summary>
        /// <param name="CommentsReplyID">idie Replye morede nazar</param>
        /// <returns>
        /// ActionResult : RedirectToAction("ListComment", "Comments")
        /// </returns>
        [HttpPost]
        public ActionResult DeleteCommentsReply(int CommentsReplyID)
        {
            PostCommentReplyManagement comment = new PostCommentReplyManagement();
            if (comment.PermanentlyDeletePostCommentReply(CommentsReplyID) == "NotFound") { return View("~/Views/Shared/NotFoundFailed.cshtml"); }
            return RedirectToAction("ListComment", "Comments");
        }
    }
}