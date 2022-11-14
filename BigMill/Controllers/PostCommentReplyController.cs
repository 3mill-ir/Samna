using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BigMill.Models;

namespace BigMill.Controllers
{
    public class PostCommentReplyController : Controller
    {

        /// <summary>
        /// in controller jahate ezafe kardane reply be commente morede nazar bekar miravad
        /// </summary>
        /// <param name="Tempmodel"></param>
        /// <param name="PostCommentId">idie commente morede nazar</param>
        /// <returns>
        /// ActionResult : PartialView(AddPostCommentReply)
        /// </returns>
        [HttpPost]
        public ActionResult AddPostCommentReply(PostModel Tempmodel, int PostCommentId)
        {
            PostCommentReplyModel model = new PostCommentReplyModel();
            model.Text = Tempmodel.Reply;
            model.F_PostsComments_Id = PostCommentId;
            PostCommentReplyManagement post = new PostCommentReplyManagement();
            //     if (ModelState.IsValid)
            {
                int Scale = post.AddPostCommentReply(model);
                if (Scale == 1)
                    @ViewBag.AddCommentReplySuccess = "پاسخ نظر شما با موفقیت ثبت شد";
                else
                    @ViewBag.AddCommentReplyFailed = "متاسفانه ثبت پاسخ نظر شما با خطا رو به رو شد";
                return PartialView();
            }
            //else
            //    return View("TryAgain");
        }

        /// <summary>
        /// in controller jahate like kardane replye morede nazar bekar miravad
        /// dar in controller az cookie ha jahate mahdud kardane like be tedade yek adad baraye har nafar estefade shode ast
        /// </summary>
        /// <param name="CommentReplyId">idie replye morede nazar</param>
        /// <returns>
        /// ActionResult : PartialView(LikePostCommentReply)
        /// </returns>
        [HttpPost]
        public ActionResult LikePostCommentReply(int CommentReplyId)
        {
            // PCRL mokhaffafe (Post Comment Reply Like) mibashad
            PostCommentReplyManagement post = new PostCommentReplyManagement();
            PostsCommentsReply model = new PostsCommentsReply();
            // jahate check kardane inke cookie az ghabl ruye system bude ya na
            if (HttpContext.Request.Cookies["P2016O"] != null)
            {
                string LikeIdString = HttpContext.Request.Cookies["P2016O"].Value;
                List<string> LikeIds = new List<string>();
                LikeIds = LikeIdString.Split('-').ToList();
                if (LikeIds.Exists(u => u == "PCRL" + CommentReplyId) == false)
                {
                    Response.Cookies["P2016O"].Value = LikeIdString + "-PCRL" + CommentReplyId;
                    model = post.LikePostCommentReply(CommentReplyId, true);
                }
                else
                {
                    model = post.LikePostCommentReply(CommentReplyId, false);
                }
            }
                // dar in ghesmat cookie baraye avvalin bar sakhte mishavad (chon check shode ke az ghabl vojud nadashte)
            else
            {
                HttpCookie cookie = new HttpCookie("P2016O");
                cookie.Expires = DateTime.Now.AddDays(365);
                cookie.Value = "PCRL" + CommentReplyId;
                Response.Cookies.Add(cookie);
                model = post.LikePostCommentReply(CommentReplyId, true);
            }
            return PartialView(model);
        }


        /// <summary>
        /// in controller jahate Dislike kardane replye morede nazar bekar miravad
        /// dar in controller az cookie ha jahate mahdud kardane Dislike be tedade yek adad baraye har nafar estefade shode ast
        /// </summary>
        /// <param name="CommentReplyId">idie replye morede nazar</param>
        /// <returns>
        /// ActionResult : PartialView(DisLikePostCommentReply)
        /// </returns>
        [HttpPost]
        public ActionResult DisLikePostCommentReply(int CommentReplyId)
        {
            // PCRD mokhaffafe (Post Comment Reply DisLike) mibashad
            PostCommentReplyManagement post = new PostCommentReplyManagement();
            PostsCommentsReply model = new PostsCommentsReply();
            if (HttpContext.Request.Cookies["P2016O"] != null)
            {
                string LikeIdString = HttpContext.Request.Cookies["P2016O"].Value;
                List<string> LikeIds = new List<string>();
                LikeIds = LikeIdString.Split('-').ToList();
                // jahate check kardane inke cookie az ghabl ruye system bude ya na
                if (LikeIds.Exists(u => u == "PCRD" + CommentReplyId) == false)
                {
                    Response.Cookies["P2016O"].Value = LikeIdString + "-PCRD" + CommentReplyId;
                    model = post.DisLikePostCommentReply(CommentReplyId, true);
                }
                else
                {
                    model = post.DisLikePostCommentReply(CommentReplyId, false);
                }
            }
            // dar in ghesmat cookie baraye avvalin bar sakhte mishavad (chon check shode ke az ghabl vojud nadashte)
            else
            {
                HttpCookie cookie = new HttpCookie("P2016O");
                cookie.Expires = DateTime.Now.AddDays(365);
                cookie.Value = "PCRD" + CommentReplyId;
                Response.Cookies.Add(cookie);
                model = post.DisLikePostCommentReply(CommentReplyId, true);
            }
            return PartialView(model);
        }

    }
}