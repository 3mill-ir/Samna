using BigMill.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;


namespace BigMill.Controllers
{
    public class ParliamentController : Controller
    {
        #region Android

        /// <summary>
        /// in tabe liste khabar haye majles(Sokhanraniha va notgh ha) ra baraye device haye androdi baz migardanad (Android)
        /// </summary>
        /// <returns>
        /// string
        /// Jsoni dar ghalebe string
        /// </returns>
        public string AndroidViewSpeech()
        {
            PostManagement post = new PostManagement();
            int ID = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SpeechID"]);
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
        /// in tabe joziyate khabare majles(sokhanraniha va notgh ha)e morede nazar ra dar ghalebe yek view be device androidi baz migardanad (Android)
        /// </summary>
        /// <param name="PostId">idie poste morede nazar</param>
        /// <returns>
        /// ActionResult : View(AndroidViewSpeech_Detail)
        /// </returns>
        public ActionResult AndroidViewSpeech_Detail(int PostId)
        {
            PostManagement post = new PostManagement();
            PostModel model = new PostModel();
            int SubId = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SpeechID"]);
            model = post.PostDetails(PostId, SubId);
            FileManagement FM = new FileManagement();
            var temp = FM.LoadFiles().FirstOrDefault(u => u.ID == PostId);
            ViewBag.VideoSrc = temp != null ? temp.File : "";
            return View(model);
        }
        /// <summary>
        /// in tabe liste khabar haye majles(Tazakkorat va soal ha) ra baraye device haye androdi baz migardanad (Android)
        /// </summary>
        /// <returns>
        /// string
        /// Jsoni dar ghalebe string
        /// </returns>
        public string AndroidViewQuestion()
        {
            PostManagement post = new PostManagement();
            int ID = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["QuestionID"]);
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
        /// in tabe joziyate khabare majles(tazakkorat va soalat)e morede nazar ra dar ghalebe yek view be device androidi baz migardanad (Android)
        /// </summary>
        /// <param name="PostId">idie poste morede nazar</param>
        /// <returns>
        /// ActionResult : View(AndroidViewQuestion_Detail)
        /// </returns>
        public ActionResult AndroidViewQuestion_Detail(int PostId)
        {
            PostManagement post = new PostManagement();
            PostModel model = new PostModel();
            int SubId = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["QuestionID"]);
            model = post.PostDetails(PostId, SubId);
            FileManagement FM = new FileManagement();
            var temp = FM.LoadFiles().FirstOrDefault(u => u.ID == PostId);
            ViewBag.VideoSrc = temp != null ? temp.File : "";
            return View(model);
        }
        /// <summary>
        /// in tabe liste khabar haye majles(Mosahebeha) ra baraye device haye androdi baz migardanad (Android)
        /// </summary>
        /// <returns>
        /// string
        /// Jsoni dar ghalebe string
        /// </returns>
        public string AndroidViewInterview()
        {
            PostManagement post = new PostManagement();
            int ID = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["InterviewID"]);
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
        /// in tabe joziyate khabare majles(mosahebe ha)e morede nazar ra dar ghalebe yek view be device androidi baz migardanad (Android)
        /// </summary>
        /// <param name="PostId">idie poste morede nazar</param>
        /// <returns>
        /// ActionResult : View(AndroidViewInterview_Detail)
        /// </returns>
        public ActionResult AndroidViewInterview_Detail(int PostId)
        {
            PostManagement post = new PostManagement();
            PostModel model = new PostModel();
            int SubId = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["InterviewID"]);
            model = post.PostDetails(PostId, SubId);
            FileManagement FM = new FileManagement();
            var temp = FM.LoadFiles().FirstOrDefault(u => u.ID == PostId);
            ViewBag.VideoSrc = temp != null ? temp.File : "";
            return View(model);
        }
        /// <summary>
        /// in tabe liste khabar haye majles(Ahamme mokatebat) ra baraye device haye androdi baz migardanad (Android)
        /// </summary>
        /// <returns>
        /// string
        /// Jsoni dar ghalebe string
        /// </returns>
        public string AndroidViewCorrespondence()
        {
            PostManagement post = new PostManagement();
            int ID = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["CorrespondenceID"]);
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
        /// in tabe joziyate khabare majles(ahamme mokatebat)e morede nazar ra dar ghalebe yek view be device androidi baz migardanad (Android)
        /// </summary>
        /// <param name="PostId">idie poste morede nazar</param>
        /// <returns>
        /// ActionResult : View(AndroidViewCorrespondence_Detail)
        /// </returns>
        public ActionResult AndroidViewCorrespondence_Detail(int PostId)
        {
            PostManagement post = new PostManagement();
            PostModel model = new PostModel();
            int SubId = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["CorrespondenceID"]);
            model = post.PostDetails(PostId, SubId);
            FileManagement FM = new FileManagement();
            var temp = FM.LoadFiles().FirstOrDefault(u => u.ID == PostId);
            ViewBag.VideoSrc = temp != null ? temp.File : "";
            return View(model);
        }
        #endregion


        #region Web
        /// <summary>
        /// in tabe liste khabar haye majles(sokhanraniha va notgh ha) ra baz migardanad (Web)
        /// </summary>
        /// <returns>
        /// ActionResult : View(ViewSpeech)
        /// </returns>
        /// 
        /// Viewe (ViewSpeech) az partial view haye :
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
        public ActionResult ViewSpeech(int? page)
        {
            PostManagement post = new PostManagement();
            int pageSize = int.Parse(System.Configuration.ConfigurationManager.AppSettings["PagedlistSize"]);
            int pageNumber = (page ?? 1);
            int total;
            var pagedList = new StaticPagedList<PostModel>(post.ListSpecificPost(int.Parse(System.Configuration.ConfigurationManager.AppSettings["SpeechID"]), pageNumber, pageSize, out total), pageNumber, pageSize, total);
            @ViewBag.pipTags = post.GetSubCategoryTags(int.Parse(System.Configuration.ConfigurationManager.AppSettings["SpeechID"]), int.Parse(System.Configuration.ConfigurationManager.AppSettings["ParliamentId"]));
            return View(pagedList);
        }
        /// <summary>
        /// in tabe joziyate khabare majles(sokhanraniha va notgh ha)e morede nazar ra baz migardanad (Web)
        /// </summary>
        /// <param name="PostId">idie poste morede nazar</param>
        /// <returns>
        /// ActionResult : View(ViewSpeech_Detail)
        /// </returns>
        /// 
        /// Viewe (ViewSpeech_Detail) az partial view haye :
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
        public ActionResult ViewSpeech_Detail(int PostId)
        {
            PostManagement post = new PostManagement();
            int SubId = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["SpeechID"]);
            @ViewBag.pipTags = post.GetPostTags(PostId, int.Parse(System.Configuration.ConfigurationManager.AppSettings["SpeechID"]), int.Parse(System.Configuration.ConfigurationManager.AppSettings["ParliamentId"]));
            FileManagement FM = new FileManagement();
            var temp = FM.LoadFiles().FirstOrDefault(u => u.ID == PostId);
            ViewBag.VideoSrc = temp != null ? temp.File : "";
            return View(post.PostDetails(PostId, SubId));
        }
        /// <summary>
        /// in tabe liste khabar haye majles(tazakkorat va soal ha) ra baz migardanad (Web)
        /// </summary>
        /// <returns>
        /// ActionResult : View(ViewQuestion)
        /// </returns>
        /// 
        /// Viewe (ViewQuestion) az partial view haye :
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
        public ActionResult ViewQuestion(int? page)
        {
            PostManagement post = new PostManagement();
            int pageSize = int.Parse(System.Configuration.ConfigurationManager.AppSettings["PagedlistSize"]);
            int pageNumber = (page ?? 1);
            int total;
            var pagedList = new StaticPagedList<PostModel>(post.ListSpecificPost(int.Parse(System.Configuration.ConfigurationManager.AppSettings["QuestionID"]), pageNumber, pageSize, out total), pageNumber, pageSize, total);
            @ViewBag.pipTags = post.GetSubCategoryTags(int.Parse(System.Configuration.ConfigurationManager.AppSettings["QuestionID"]), int.Parse(System.Configuration.ConfigurationManager.AppSettings["ParliamentId"]));
            return View(pagedList);
        }
        /// <summary>
        /// in tabe joziyate khabare majles(tazakkorat va soal ha)e morede nazar ra baz migardanad (Web)
        /// </summary>
        /// <param name="PostId">idie poste morede nazar</param>
        /// <returns>
        /// ActionResult : View(ViewSpeech_Detail)
        /// </returns>
        /// 
        /// Viewe (ViewSpeech_Detail) az partial view haye :
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
        public ActionResult ViewQuestion_Detail(int PostId)
        {
            PostManagement post = new PostManagement();
            int SubId = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["QuestionID"]);
            @ViewBag.pipTags = post.GetPostTags(PostId, int.Parse(System.Configuration.ConfigurationManager.AppSettings["QuestionID"]), int.Parse(System.Configuration.ConfigurationManager.AppSettings["ParliamentId"]));
            FileManagement FM = new FileManagement();
            var temp = FM.LoadFiles().FirstOrDefault(u => u.ID == PostId);
            ViewBag.VideoSrc = temp != null ? temp.File : "";
            return View(post.PostDetails(PostId,SubId));
        }
        /// <summary>
        /// in tabe liste khabar haye majles(mosahebe ha) ra baz migardanad (Web)
        /// </summary>
        /// <returns>
        /// ActionResult : View(ViewInterview)
        /// </returns>
        /// 
        /// Viewe (ViewInterview) az partial view haye :
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
        public ActionResult ViewInterview(int? page)
        {
            PostManagement post = new PostManagement();
            int pageSize = int.Parse(System.Configuration.ConfigurationManager.AppSettings["PagedlistSize"]);
            int pageNumber = (page ?? 1);
            int total;
            var pagedList = new StaticPagedList<PostModel>(post.ListSpecificPost(int.Parse(System.Configuration.ConfigurationManager.AppSettings["InterviewID"]), pageNumber, pageSize, out total), pageNumber, pageSize, total);
            @ViewBag.pipTags = post.GetSubCategoryTags(int.Parse(System.Configuration.ConfigurationManager.AppSettings["InterviewID"]), int.Parse(System.Configuration.ConfigurationManager.AppSettings["ParliamentId"]));
            return View(pagedList);
        }
        /// <summary>
        /// in tabe joziyate khabare majles(mosahebe ha)e morede nazar ra baz migardanad (Web)
        /// </summary>
        /// <param name="PostId">idie poste morede nazar</param>
        /// <returns>
        /// ActionResult : View(ViewInterview_Detail)
        /// </returns>
        /// 
        /// Viewe (ViewInterview_Detail) az partial view haye :
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
        public ActionResult ViewInterview_Detail(int PostId)
        {
            PostManagement post = new PostManagement();
            int SubId = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["InterviewID"]);
            @ViewBag.pipTags = post.GetPostTags(PostId, int.Parse(System.Configuration.ConfigurationManager.AppSettings["InterviewID"]), int.Parse(System.Configuration.ConfigurationManager.AppSettings["ParliamentId"]));
            FileManagement FM = new FileManagement();
            var temp = FM.LoadFiles().FirstOrDefault(u => u.ID == PostId);
            ViewBag.VideoSrc = temp != null ? temp.File : "";
            return View(post.PostDetails(PostId,SubId));
        }
        /// <summary>
        /// in tabe liste khabar haye majles(ahamme mokatebat) ra baz migardanad (Web)
        /// </summary>
        /// <returns>
        /// ActionResult : View(ViewCorrespondence)
        /// </returns>
        /// 
        /// Viewe (ViewCorrespondence) az partial view haye :
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
        public ActionResult ViewCorrespondence(int? page)
        {
            PostManagement post = new PostManagement();
            int pageSize = int.Parse(System.Configuration.ConfigurationManager.AppSettings["PagedlistSize"]);
            int pageNumber = (page ?? 1);
            int total;
            var pagedList = new StaticPagedList<PostModel>(post.ListSpecificPost(int.Parse(System.Configuration.ConfigurationManager.AppSettings["CorrespondenceID"]), pageNumber, pageSize, out total), pageNumber, pageSize, total);
            @ViewBag.pipTags = post.GetSubCategoryTags(int.Parse(System.Configuration.ConfigurationManager.AppSettings["CorrespondenceID"]), int.Parse(System.Configuration.ConfigurationManager.AppSettings["ParliamentId"]));
            return View(pagedList);
        }
        /// <summary>
        /// in tabe joziyate khabare majles(ahamme mokatebat)e morede nazar ra baz migardanad (Web)
        /// </summary>
        /// <param name="PostId">idie poste morede nazar</param>
        /// <returns>
        /// ActionResult : View(ViewCorrespondence_Detail)
        /// </returns>
        /// 
        /// Viewe (ViewCorrespondence_Detail) az partial view haye :
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
        public ActionResult ViewCorrespondence_Detail(int PostId)
        {
            PostManagement post = new PostManagement();
            int SubId = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["CorrespondenceID"]);
            @ViewBag.pipTags = post.GetPostTags(PostId, int.Parse(System.Configuration.ConfigurationManager.AppSettings["CorrespondenceID"]), int.Parse(System.Configuration.ConfigurationManager.AppSettings["ParliamentId"]));
            FileManagement FM = new FileManagement();
            var temp = FM.LoadFiles().FirstOrDefault(u => u.ID == PostId);
            ViewBag.VideoSrc = temp != null ? temp.File : "";
            return View(post.PostDetails(PostId,SubId));
        }
        #endregion
    }
}