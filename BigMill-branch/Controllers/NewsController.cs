using BigMill.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;


namespace BigMill.Controllers
{
    public class NewsController : Controller
    {
        #region For Android
        /// <summary>
        /// in tabe liste khabar haye oomumi ra baraye device haye androdi baz migardanad (Android)
        /// </summary>
        /// <returns>
        /// string
        /// Jsoni dar ghalebe string
        /// </returns>
        public string AndroidAkhbarOmumi()
        {
            PostManagement post = new PostManagement();
            int ID = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["AkhbarPublicID"]);
            var query = post.AndroidListSpecificPost(ID);
           string result = "{\"Root\": [";
           string Posts = "";
           foreach (var item in query)
           {
               string Description = item.Content_Three.Replace("\"", "-");
               string Title = item.Content_One.Replace("\"", "-");
               Posts += "{\"id\": \"" + item.ID + "\", \"title\": \"" + Title + "\", \"description\": \"" + Description + "\", \"picture\": \"" + System.Configuration.ConfigurationManager.AppSettings["WebSiteURL"] + "Content/UplodedImages/PostImages/" + item.Content_Two + "\"},";
           }
           result += Posts;
           if (result.LastIndexOf(',') > 0)
           {
               result = result.Remove(result.LastIndexOf(","), 1);
           }
           result += "]}";
            return result;
        }

        /// <summary>
        /// in tabe joziyate khabare omumiye morede nazar ra dar ghalebe yek view be device androidi baz migardanad (Android)
        /// </summary>
        /// <param name="PostId">idie poste morede nazar</param>
        /// <returns>
        /// ActionResult : View(AndroidAkhbarOmumi_Detail)
        /// </returns>
        public ActionResult AndroidAkhbarOmumi_Detail(int PostId)
        {
            PostManagement post = new PostManagement();
            PostModel model = new PostModel();
            int SubId = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["AkhbarPublicID"]);
            model = post.PostDetails(PostId, SubId);
            FileManagement FM = new FileManagement();
            var temp = FM.LoadFiles().FirstOrDefault(u => u.ID == PostId);
            ViewBag.VideoSrc = temp != null ? temp.File : "";
            return View(model);
        }



        /// <summary>
        /// in tabe liste khabar haye Hoze ra baraye device haye androdi baz migardanad (Android)
        /// </summary>
        /// <returns>
        /// string
        /// Jsoni dar ghalebe string
        /// </returns>
        public string AndroidAkhbarHozeEntekhabiye()
        {
            PostManagement post = new PostManagement();
            int ID = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["AkhbarHozeID"]);
            var query = post.AndroidListSpecificPost(ID);
            string result = "{\"Root\": [";
            string Posts = "";
            foreach (var item in query)
            {
                string Description = item.Content_Three.Replace("\"", "-");
                string Title = item.Content_One.Replace("\"", "-");
                Posts += "{\"id\": \"" + item.ID + "\", \"title\": \"" + Title + "\", \"description\": \"" + Description + "\", \"picture\": \"" + System.Configuration.ConfigurationManager.AppSettings["WebSiteURL"] + "Content/UplodedImages/PostImages/" + item.Content_Two + "\"},";
            }
            result += Posts;
            if (result.LastIndexOf(',') > 0)
            {
                result = result.Remove(result.LastIndexOf(","), 1);
            }
            result += "]}";
            return result;
        }

        /// <summary>
        /// in tabe joziyate khabare Hoze morede nazar ra dar ghalebe yek view be device androidi baz migardanad (Android)
        /// </summary>
        /// <param name="PostId">idie poste morede nazar</param>
        /// <returns>
        /// ActionResult : View(AndroidAkhbarHozeEntekhabiye_Detail)
        /// </returns>
        public ActionResult AndroidAkhbarHozeEntekhabiye_Detail(int PostId)
        {
            PostManagement post = new PostManagement();
            PostModel model = new PostModel();
            int SubId = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["AkhbarHozeID"]);
            model = post.PostDetails(PostId, SubId);
            FileManagement FM = new FileManagement();
            var temp = FM.LoadFiles().FirstOrDefault(u => u.ID == PostId);
            ViewBag.VideoSrc = temp != null ? temp.File : "";
            return View(model);
        }
        #endregion


        #region For Web
        /// <summary>
        /// in tabe liste khabar haye oomumi ra baz migardanad (Web)
        /// </summary>
        /// <returns>
        /// ActionResult
        /// ActionResult : View(AkhbarOmumi)
        /// </returns>
        /// 
        /// Viewe (AkhbarOmumi) az partial view haye :
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
        /// ///////////////////////// estefade mikonad.
        public ActionResult AkhbarOmumi(int? page)
        {
            PostManagement post = new PostManagement();
            int pageSize = int.Parse(System.Configuration.ConfigurationManager.AppSettings["PagedlistSize"]);
            int pageNumber = (page ?? 1);
            int total;
            var pagedList = new StaticPagedList<PostModel>(post.ListSpecificPost(int.Parse(System.Configuration.ConfigurationManager.AppSettings["AkhbarPublicID"]), pageNumber, pageSize, out total), pageNumber, pageSize, total);
       @ViewBag.pipTags=post.GetSubCategoryTags(int.Parse(System.Configuration.ConfigurationManager.AppSettings["AkhbarPublicID"]), int.Parse(System.Configuration.ConfigurationManager.AppSettings["AkhbarId"]));

            return View(pagedList);
        }

        /// <summary>
        /// in tabe joziyate khabare omumiye morede nazar ra baz migardanad (Web)
        /// </summary>
        /// <param name="PostId">idie poste morede nazar</param>
        /// <returns>
        /// ActionResult : View(AkhbarOmumi_Detail)
        /// </returns>
        /// 
        /// Viewe (AkhbarOmumi_Detail) az partial view haye :
        /// 1- InformationBar
        /// 2- Links
        /// 3- NewsParliamentSearch
        /// 4- PopularPosts
        /// 5- RecentPosts
        /// 6- PostTags
        /// 7- RuzWord
        /// 8- RuzPic
        /// 9- AyineKhedmat
        /// 10- NewsArchive
        /// ///////////////////////// estefade mikonad.
        public ActionResult AkhbarOmumi_Detail(int PostId)
        {
            PostManagement post = new PostManagement();
            int SubId = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["AkhbarPublicID"]);
            @ViewBag.pipTags = post.GetPostTags(PostId,int.Parse(System.Configuration.ConfigurationManager.AppSettings["AkhbarPublicID"]), int.Parse(System.Configuration.ConfigurationManager.AppSettings["AkhbarId"]));
            FileManagement FM = new FileManagement();
            var temp = FM.LoadFiles().FirstOrDefault(u => u.ID == PostId);
            ViewBag.VideoSrc = temp != null ? temp.File : "";
            return View(post.PostDetails(PostId,SubId));
        }

        /// <summary>
        /// in tabe liste khabar haye HozeEntekhabiyye ra baz migardanad (Web)
        /// </summary>
        /// <returns>
        /// ActionResult : View(AkhbarHozeEntekhabiye)
        /// </returns>
        /// 
        /// Viewe (AkhbarHozeEntekhabiye) az partial view haye :
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
        /// ///////////////////////// estefade mikonad.
        public ActionResult AkhbarHozeEntekhabiye(int? page)
        {
            PostManagement post = new PostManagement();
            int pageSize = int.Parse(System.Configuration.ConfigurationManager.AppSettings["PagedlistSize"]);
            int pageNumber = (page ?? 1);
            int total;
            var pagedList = new StaticPagedList<PostModel>(post.ListSpecificPost(int.Parse(System.Configuration.ConfigurationManager.AppSettings["AkhbarHozeID"]), pageNumber, pageSize, out total), pageNumber, pageSize, total);
            @ViewBag.pipTags = post.GetSubCategoryTags(int.Parse(System.Configuration.ConfigurationManager.AppSettings["AkhbarHozeID"]), int.Parse(System.Configuration.ConfigurationManager.AppSettings["AkhbarId"]));
            return View(pagedList);
        }

        /// <summary>
        /// in tabe joziyate khabare Hoze morede nazar ra baz migardanad (Web)
        /// </summary>
        /// <param name="PostId">idie poste morede nazar</param>
        /// <returns>
        /// ActionResult : View(AkhbarHozeEntekhabiye_Detail)
        /// </returns>
        /// 
        /// Viewe (AkhbarHozeEntekhabiye_Detail) az partial view haye :
        /// 1- InformationBar
        /// 2- Links
        /// 3- NewsParliamentSearch
        /// 4- PopularPosts
        /// 5- RecentPosts
        /// 6- PostTags
        /// 7- RuzWord
        /// 8- RuzPic
        /// 9- AyineKhedmat
        /// 10- NewsArchive
        /// ///////////////////////// estefade mikonad.
        public ActionResult AkhbarHozeEntekhabiye_Detail(int PostId)
        {
            PostManagement post = new PostManagement();
            int SubId = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["AkhbarHozeID"]);
            @ViewBag.pipTags = post.GetPostTags(PostId, int.Parse(System.Configuration.ConfigurationManager.AppSettings["AkhbarHozeID"]), int.Parse(System.Configuration.ConfigurationManager.AppSettings["AkhbarId"]));
            FileManagement FM = new FileManagement();
            var temp = FM.LoadFiles().FirstOrDefault(u => u.ID == PostId);
            ViewBag.VideoSrc = temp != null ? temp.File : "";
            return View(post.PostDetails(PostId, SubId));
        }
        #endregion
    }
}