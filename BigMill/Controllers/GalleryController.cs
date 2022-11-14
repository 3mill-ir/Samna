using BigMill.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;




namespace BigMill.Controllers
{
    public class GalleryController : Controller
    {

        /// <summary>
        /// in tabe tasavire post haye mojud ra barmigardanad (Makhsuse Web)
        /// Ba eemale PagedList
        /// </summary>
        /// <param name="page">shomare page feeli</param>
        /// <returns>
        /// ActionResult_View(PhotosGallery)
        /// </returns>
        /// 
        /// Viewe (PhotosGallery) az partial viewe :
        /// 1- InformationBar
        //////////////////////////// estefade mikonad.
        public ActionResult PhotosGallery(int? page)
        {
            PostManagement post = new PostManagement();
            int pageSize = 12;
            int pageNumber = (page ?? 1);
            int total;
            var pagedList = new StaticPagedList<GalleryModel>(post.PhotosGallery(pageNumber, pageSize, out total), pageNumber, pageSize, total);
            return View(pagedList);
        }
        /// <summary>
        /// in tabe jsoni az linke tasavire post ha be device haye androidi baz migardanad.
        /// </summary>
        /// <param name="SubDomain">SubDomaine morede nazar</param>
        /// <returns>
        /// string
        /// Jsoni dar ghalebe string
        /// </returns>
        public string AndroidGallery(string SubDomain)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var Photos = db.Posts.Where(m=>m.Status==true && m.Display==true).Select(y => new { y.Content_Two,y.CreatedOnUTC }).OrderByDescending(u => u.CreatedOnUTC);
                string result = "{\"Root\": [";
                foreach (var item in Photos)
                {
                    result += "{\"Image\": \"" + SubDomain + "/Content/UplodedImages/PostImages/"+item.Content_Two+"\"},";
                }
                result = result.Remove(result.LastIndexOf(','), 1);
                result+="]}";
                return result;
            }
        }
    }
}