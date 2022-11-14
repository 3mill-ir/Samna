using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BigMill.Models;

namespace BigMill.Controllers
{
    public class PostCommentController : Controller
    {

        /// <summary>
        /// in controller jahate ezafe kardane commente jadid be poste morede nazar morede estefade gharar migirad
        /// </summary>
        /// <param name="Tempmodel"></param>
        /// <returns>
        /// ActionResult : PartialView(AddPostComment)
        /// </returns>
        [HttpPost]
        public ActionResult AddPostComment(PostModel Tempmodel)
        {
            PostCommentManagement post = new PostCommentManagement();
            {
                PostCommentModel model = new PostCommentModel();
                model.Text = Tempmodel.Comment;
                model.F_Posts_Id = Tempmodel.ID;
                int scale = post.AddPostComment(model);
                if (scale == 1)
                    @ViewBag.AddCommentSuccess = "نظر شما با موفقیت ثبت شد";
                else
                    @ViewBag.AddCommentFailed = "متاسفانه ثبت نظر شما با خطا رو به رو شد";
                return PartialView();
            }
        }

        /// <summary>
        /// in controller jahate like kardane commente morede nazar morede estefade gharar migirad
        /// dar in controller az cookie ha jahate mahdud kardane like be tedade yek adad baraye har nafar estefade shode ast
        /// </summary>
        /// <param name="CommentId">idie commente morede nazar</param>
        /// <returns>
        /// ActionResult : PartialView(LikePostComment)
        /// </returns>
        [HttpPost]
        public ActionResult LikePostComment(int CommentId)
        {
            //PCL mokhaffafe (Post Comment Like) mibashad
            PostCommentManagement post = new PostCommentManagement();
            PostsComment model = new PostsComment();
            // check kardane inke aya cookie az ghabl dar systeme user bude ya na
            if (HttpContext.Request.Cookies["P2016O"] != null)
            {
                string LikeIdString = HttpContext.Request.Cookies["P2016O"].Value;
                List<string> LikeIds = new List<string>();
                LikeIds = LikeIdString.Split('-').ToList();
                if (LikeIds.Exists(u => u == "PCL" + CommentId) == false)
                {
                    Response.Cookies["P2016O"].Value = LikeIdString + "-PCL" + CommentId;
                    model = post.LikePostComment(CommentId, true);
                }
                else
                {
                    model = post.LikePostComment(CommentId, false);
                }
            }
                // agar cookie az ghabl vojud nadashte bashad inja sakhte mishavad
            else
            {
                HttpCookie cookie = new HttpCookie("P2016O");
                cookie.Expires = DateTime.Now.AddDays(365);
                cookie.Value = "PCL" + CommentId;
                Response.Cookies.Add(cookie);
                model = post.LikePostComment(CommentId, true);
            }
            return PartialView(model);
        }


        /// <summary>
        /// in controller jahate Dislike kardane commente morede nazar morede estefade gharar migirad
        /// dar in controller az cookie ha jahate mahdud kardane Dislike be tedade yek adad baraye har nafar estefade shode ast
        /// </summary>
        /// <param name="CommentId">idie commente morede nazar</param>
        /// <returns>
        /// ActionResult : PartialView(DisLikePostComment)
        /// </returns>
        [HttpPost]
        public ActionResult DisLikePostComment(int CommentId)
        {
            //PCD mokhaffafe (Post Comment DisLike) mibashad
            PostCommentManagement post = new PostCommentManagement();
            PostsComment model = new PostsComment();
            // check kardane inke aya cookie az ghabl dar systeme user bude ya na
            if (HttpContext.Request.Cookies["P2016O"] != null)
            {
                string LikeIdString = HttpContext.Request.Cookies["P2016O"].Value;
                List<string> LikeIds = new List<string>();
                LikeIds = LikeIdString.Split('-').ToList();
                if (LikeIds.Exists(u => u == "PCD" + CommentId) == false)
                {
                    Response.Cookies["P2016O"].Value = LikeIdString + "-PCD" + CommentId;
                    model = post.DisLikePostComment(CommentId, true);
                }
                else
                {
                    model = post.DisLikePostComment(CommentId, false);
                }
            }
            // agar cookie az ghabl vojud nadashte bashad inja sakhte mishavad
            else
            {
                HttpCookie cookie = new HttpCookie("P2016O");
                cookie.Expires = DateTime.Now.AddDays(365);
                cookie.Value = "PCD" + CommentId;
                Response.Cookies.Add(cookie);
                model = post.DisLikePostComment(CommentId, true);
            }
            return PartialView(model);
        }

    }
}