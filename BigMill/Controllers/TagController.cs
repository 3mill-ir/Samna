using BigMill.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace BigMill.Controllers
{
    public class TagController : Controller
    {

        /// <summary>
        /// in controller tamame post hayi ke tage morede nazar ra darand ra barmigardanad
        /// </summary>
        /// <param name="page">shomare safheye feeliye paging</param>
        /// <param name="TagId">idie tage morede nazar</param>
        /// <returns>
        /// ActionResult : View(Tag)
        /// </returns>
        public ActionResult Tag(int? page, int TagId)
        {
            TagManagement post = new TagManagement();
            var query = post.GetPostTagReturnListPost(TagId);
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            ViewBag.PipoTagId = TagId;
            return View(query.ToPagedList(pageNumber, pageSize));
        }

    }
}