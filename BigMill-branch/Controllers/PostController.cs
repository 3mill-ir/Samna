using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BigMill.Models;

namespace BigMill.Controllers
{
    public class PostController : Controller
    {

        /// <summary>
        /// in controller jahate Like kardane poste morede nazar morede estefade gharar migirad
        /// dar in controller az cookie ha jahate mahdud kardane like be tedade yek adad baraye har nafar estefade shode ast
        /// </summary>
        /// <param name="PostId">idie poste morede nazar</param>
        /// <returns>
        /// ActionResult : PartialView(LikePost)
        /// </returns>
        [HttpPost]
        public ActionResult LikePost(int PostId)
        {
            // PL mokhaffafe (Post Like) mibashad
            PostManagement post = new PostManagement();
            PostModel model = new PostModel();
            // jahate check kardane inke cookie az ghabl ruye system bude ya na
            if (HttpContext.Request.Cookies["P2016O"] != null)
            {
                string LikeIdString = HttpContext.Request.Cookies["P2016O"].Value;
                List<string> LikeIds = new List<string>();
                LikeIds = LikeIdString.Split('-').ToList();
                if (LikeIds.Exists(u => u == "PL" + PostId) == false)
                {
                    Response.Cookies["P2016O"].Value = LikeIdString + "-PL" + PostId;
                    model = post.LikePost(PostId, true);
                }
                else
                {
                    model = post.LikePost(PostId, false);
                }
            }
            // dar in ghesmat cookie baraye avvalin bar sakhte mishavad (chon check shode ke az ghabl vojud nadashte)
            else
            {
                HttpCookie cookie = new HttpCookie("P2016O");
                cookie.Expires = DateTime.Now.AddDays(365);
                cookie.Value = "PL" + PostId;
                Response.Cookies.Add(cookie);
                model = post.LikePost(PostId, true);
            }
            return PartialView(model);
        }


        /// <summary>
        /// in controller jahate DisLike kardane poste morede nazar morede estefade gharar migirad
        /// dar in controller az cookie ha jahate mahdud kardane Dislike be tedade yek adad baraye har nafar estefade shode ast
        /// </summary>
        /// <param name="PostId">idie poste morede nazar</param>
        /// <returns>
        /// ActionResult : PartialView(DisLikePost)
        /// </returns>
        [HttpPost]
        public ActionResult DisLikePost(int PostId)
        {
            // PD mokhaffafe (Post DisLike) mibashad
            PostManagement post = new PostManagement();
            PostModel model = new PostModel();
            // jahate check kardane inke cookie az ghabl ruye system bude ya na
            if (HttpContext.Request.Cookies["P2016O"] != null)
            {
                string LikeIdString = HttpContext.Request.Cookies["P2016O"].Value;
                List<string> LikeIds = new List<string>();
                LikeIds = LikeIdString.Split('-').ToList();
                if (LikeIds.Exists(u => u == "PD" + PostId) == false)
                {
                    Response.Cookies["P2016O"].Value = LikeIdString + "-PD" + PostId;
                    model = post.DisLikePost(PostId, true);
                }
                else
                {
                    model = post.DisLikePost(PostId, false);
                }
            }
            // dar in ghesmat cookie baraye avvalin bar sakhte mishavad (chon check shode ke az ghabl vojud nadashte)
            else
            {
                HttpCookie cookie = new HttpCookie("P2016O");
                cookie.Expires = DateTime.Now.AddDays(365);
                cookie.Value = "PD" + PostId;
                Response.Cookies.Add(cookie);
                model = post.DisLikePost(PostId, true);
            }
            return PartialView(model);
        }

    }
}