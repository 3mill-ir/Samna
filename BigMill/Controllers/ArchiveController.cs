using BigMill.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace BigMill.Controllers
{
    public class ArchiveController : Controller
    {
        // GET: Archive
        /// <summary>
        /// in controller archive post haye zirshakheye morede nazar ra be view baz migardanad
        /// Ba eemale PagedList
        /// </summary>
        /// <param name="page"></param>
        /// <param name="Start"></param>
        /// <param name="End"></param>
        /// <param name="PostSubCategoryId">idie zirshakheye morede nazar</param>
        /// <param name="PostCategoryId">idie shakheye morede nazar</param>
        /// <returns>
        /// ActionResult_View(ListArchiveNews)
        /// </returns>
        /// 
        /// Viewe (ListArchiveNews) az partial view haye :
        /// 1- InformationBar
        /// 2- Links
        /// 3- NewsParliamentSearch
        /// 4- PopularPosts
        /// 5- RecentPosts
        /// 6- SubCategoryTags
        /// 7- RuzWord
        /// 8- RuzPic
        /// 9- SpeechVideo
        /// 10- PollParticipation
        /// 11- AyineKhedmat
        /// 12- BrifeFunctional
        /// 13- RssFeed
        /// 14- NewsArchive
        //////////////////////////// estefade mikonad.
        public ActionResult ListArchiveNews(int? page, DateTime Start, DateTime End, int PostSubCategoryId,int PostCategoryId)
        {
                PostManagement post = new PostManagement();
                @ViewBag.ArchiveCategoryId = PostCategoryId;
                @ViewBag.ArchiveDateStart = Start;
                @ViewBag.ArchiveDateEnd = End;
                @ViewBag.F_PostSubCategory_Id = PostSubCategoryId;
                int pageSize = 5;
                int pageNumber = (page ?? 1);
                int total;
                var pagedList = new StaticPagedList<PostModel>(post.ListArchiveNews(Start, End, PostSubCategoryId,pageNumber, pageSize, out total), pageNumber, pageSize, total);
                return View(pagedList);
        }
    }
}