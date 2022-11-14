using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BigMill.Models;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Xml.Linq;
using System.Data;
using System.Xml;
using BigMill.Areas.Admin3mill.Models;

namespace BigMill.Controllers
{
    public class UtilityController : Controller
    {
        /// <summary>
        /// in controller jahate sakhte menu ye bala safahat (samte user) bekar miravad
        /// </summary>
        /// <returns>
        /// ActionResult : PartialView(Menu)
        /// </returns>
        [ChildActionOnly]
        public ActionResult Menu()
        {
            MenuGenerator m = new MenuGenerator();
            return PartialView(m.GenerateMenu());
        }

        /// <summary>
        /// in controller jahate nemayeshe textbox va buttone serach estefade mishavad
        /// </summary>
        /// <returns>
        /// ActionResult : PartialView(Search)
        /// </returns>
        [ChildActionOnly]
        public ActionResult Search()
        {
            return PartialView();
        }

        /// <summary>
        /// in controller jahate nemayeshe ghesmate search dar safahate khabar ha va majles morede estefade gharar migirad
        /// </summary>
        /// <returns>
        /// ActionResult : PartialView(NewsParliamentSearch)
        /// </returns>
        [ChildActionOnly]
        public ActionResult NewsParliamentSearch()
        {
            return PartialView();
        }

        /// <summary>
        /// !!!!!!!!!!!!!!!!!!! Momken ast hazf shavad !!!!!!!!!!!!!!!!!!!
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public ActionResult ExtraBox()
        {
            return PartialView();
        }
        /// <summary>
        /// in controller mahbub tarin khabar hara baz migardanad
        /// </summary>
        /// <returns>
        /// ActionResult : PartialView(PopularPosts)
        /// </returns>
        [ChildActionOnly]
        public ActionResult PopularPosts()
        {
            PostManagement post = new PostManagement();
            return PartialView(post.ListPopularPosts());
        }
        /// <summary>
        /// in controller jadid tarin khabar ha ra baz migardanad
        /// </summary>
        /// <returns>
        /// ActionResult : PartialView(RecentPosts)
        /// </returns>
        [ChildActionOnly]
        public ActionResult RecentPosts()
        {
            PostManagement post = new PostManagement();
            return PartialView(post.ListRecentPosts());
        }
        /// <summary>
        /// in controller jahate bazgardandane tag haye marbut be shakhe va zirshakhe morede nazar dar safahate (khabari va search va archive va tag) morede estefade gharar migirad
        /// </summary>
        /// <param name="Tags"></param>
        /// <returns>
        /// ActionResult : PartialView(SubCategoryTags)
        /// </returns>
        [ChildActionOnly]
        public ActionResult SubCategoryTags(List<BigMill.Models.TagSelectModel> Tags)
        {
            @ViewBag.ChildPipTags = Tags;
            return PartialView();
        }
        /// <summary>
        /// in controller jahate bazgardandane tag haye marbut be post va shakhe va zirshakhe ash dar safahate (khabari va search va archive va tag) morede estefade gharar migirad
        /// </summary>
        /// <param name="Tags"></param>
        /// <returns>
        /// ActionResult : PartialView(PostTags)
        /// </returns>
        [ChildActionOnly]
        public ActionResult PostTags(List<BigMill.Models.TagSelectModel> Tags)
        {
            @ViewBag.ChildPipTags = Tags;
            return PartialView();
        }
        /// <summary>
        /// in controller jahate khandane Feede khabari az site farsnews va nemayeshe aan dar safahate (khabari va search va archive va tag) morede estefade gharar migirad
        /// </summary>
        /// <returns>
        /// ActionResult : PartialView(RssFeed)
        /// </returns>
        [ChildActionOnly]
        public ActionResult RssFeed()
        {
            List<RssModel> model = new List<RssModel>();
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load("http://www.farsnews.com/RSS");
            XmlNodeList item = xmldoc.GetElementsByTagName("item");
            for (int i = 0; i < 15; i++)
            {
                RssModel m = new RssModel();
                m.Title = item.Item(i).SelectSingleNode("title").InnerText;
                m.Url = item.Item(i).SelectSingleNode("link").InnerText;
                model.Add(m);
            }
            return PartialView(model);
        }
        /// <summary>
        /// in controller jahate generate kardane parte nazarsanji va amare nazar sanji dar safahate (khabari va search va archive va tag) bekar miravad
        /// </summary>
        /// <returns>
        /// ActionResultc : PartialView(PollParticipation)
        /// </returns>
        [ChildActionOnly]
        public ActionResult PollParticipation()
        {
            PollQuestionManagement PQM = new PollQuestionManagement();
            return PartialView(PQM.UserPollHandler());
        }
        /// <summary>
        /// in controller jahate ersale nazare karbar dar safahate (khabari va search va archive va tag) bekar miravad
        /// dar in controller az cookie ha jahate mahdud kardane sabte nazar be tedade yek bar baraye har karbar estefade shode ast
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// ActionResultc : PartialView(PollParticipationPost)
        /// </returns>
        [HttpPost]
        public ActionResult PollParticipationPost(UserPollModel model)
        {
            PollAnswerManagement PAM = new PollAnswerManagement();
            int scale;
            if (HttpContext.Request.Cookies["P2016l"] != null)
            {
                string IdString = HttpContext.Request.Cookies["P2016l"].Value;
                List<string> LikeIds = new List<string>();
                LikeIds = IdString.Split('-').ToList();
                if (LikeIds.Exists(u => u == "" + model.ID) == false)
                {
                    scale = PAM.IncreasePollAnswerScore(model.SelectedPollAnswerID);
                    if (scale == 1)
                    {
                        Response.Cookies["P2016l"].Value = IdString + "-" + model.ID;
                    }
                }
                else
                {
                    ViewBag.PollWarning = "کاربر گرامی شما قبلا در این نظر سنجی شرکت کرده اید";
                    return PartialView(model);
                }
            }
            else
            {
                HttpCookie cookie = new HttpCookie("P2016l");
                cookie.Expires = DateTime.Now.AddDays(365);
                scale = PAM.IncreasePollAnswerScore(model.SelectedPollAnswerID);
                if (scale == 1)
                {
                    cookie.Value = "" + model.ID;
                }
                Response.Cookies.Add(cookie);
            }
            if (scale == 1)
            {
                ViewBag.PollSuccess = "نظر شما با موفقیت در سیستم ثبت شد";
                return PartialView(model);
            }
            else
            {
                ViewBag.PollError = "متاسفانه نظر شما در سیستم ثبت نشد. لطفا دوباره تلاش کنید";
                return PartialView(model);
            }
        }
        /// <summary>
        /// in controller jahate generate kardane parte nazarsanji va amare nazar sanji dar safheye nokhost bekar miravad
        /// </summary>
        /// <returns>
        /// ActionResultc : PartialView(PollParticipationIndex)
        /// </returns>
        [ChildActionOnly]
        public ActionResult PollParticipationIndex()
        {
            PollQuestionManagement PQM = new PollQuestionManagement();
            return PartialView(PQM.UserPollHandler());
        }

        /// <summary>
        /// in controller jahate ersale nazare karbar dar safheye nokhost bekar miravad
        /// dar in controller az cookie ha jahate mahdud kardane sabte nazar be tedade yek bar baraye har karbar estefade shode ast
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// ActionResultc : PartialView(PollParticipationIndexPost)
        /// </returns>
        [HttpPost]
        public ActionResult PollParticipationIndexPost(UserPollModel model)
        {
            PollAnswerManagement PAM = new PollAnswerManagement();
            int scale;
            if (HttpContext.Request.Cookies["P2016l"] != null)
            {
                string IdString = HttpContext.Request.Cookies["P2016l"].Value;
                List<string> LikeIds = new List<string>();
                LikeIds = IdString.Split('-').ToList();
                if (LikeIds.Exists(u => u == "" + model.ID) == false)
                {
                    scale = PAM.IncreasePollAnswerScore(model.SelectedPollAnswerID);
                    if (scale == 1)
                    {
                        Response.Cookies["P2016l"].Value = IdString + "-" + model.ID;
                    }
                }
                else
                {
                    ViewBag.PollWarning = "کاربر گرامی شما قبلا در این نظر سنجی شرکت کرده اید";
                    return PartialView(model);
                }
            }
            else
            {
                HttpCookie cookie = new HttpCookie("P2016l");
                cookie.Expires = DateTime.Now.AddDays(365);
                scale = PAM.IncreasePollAnswerScore(model.SelectedPollAnswerID);
                if (scale == 1)
                {
                    cookie.Value = "" + model.ID;
                }
                Response.Cookies.Add(cookie);
            }
            if (scale == 1)
            {
                ViewBag.PollSuccess = "نظر شما با موفقیت در سیستم ثبت شد";
                return PartialView(model);
            }
            else
            {
                ViewBag.PollError = "متاسفانه نظر شما در سیستم ثبت نشد. لطفا دوباره تلاش کنید";
                return PartialView(model);
            }
        }

        /// <summary>
        /// in controller jahate generate kardane nemudare peygiriye darkhast ha bekar miravad
        /// </summary>
        /// <returns>
        /// ActionResultc : PartialView(TicketTrackFacts)
        /// </returns>
        [ChildActionOnly]
        public ActionResult TicketTrackFacts()
        {
            TicketInboxManagement TIM = new TicketInboxManagement();
            using (BigMill.Models.Entities db = new Entities())
            {
                var Statuses = (from c in db.Tickets group c by c.Status into g select new { FieldCount = g.Count(), FieldName = g.Key }).ToList();
                List<TicketTrackFactModel> list = new List<TicketTrackFactModel>();
                foreach (var item in Statuses)
                {
                    TicketTrackFactModel ListItem = new TicketTrackFactModel();
                    ListItem.Count = item.FieldCount;
                    ListItem.Name = item.FieldName;
                    list.Add(ListItem);
                }
                return PartialView(list.ToList());
            }
        }
        /// <summary>
        /// in controller jahate generate kardane tarikh haye arshive akhbar bekar miravad
        /// </summary>
        /// <returns>
        /// ActionResultc : PartialView(NewsArchive)
        /// </returns>
        [ChildActionOnly]
        public ActionResult NewsArchive(int PostSubCategoryId, int PostCategoryId)
        {
            @ViewBag.NewsArchivePostCategoryId = PostCategoryId;
            PostManagement post = new PostManagement();
            return PartialView(post.ArchiveDates(PostSubCategoryId));
        }
        /// <summary>
        /// in controller jahate generate kardane Link ha bekar miravad
        /// </summary>
        /// <returns>
        /// ActionResultc : PartialView(Links)
        /// </returns>
        [ChildActionOnly]
        public ActionResult Links()
        {
            return PartialView();
        }
        [ChildActionOnly]
        /// <summary>
        /// in controller jahate generate kardane sokhane ruz bekar miravad
        /// </summary>
        /// <returns>
        /// ActionResultc : PartialView(RuzWord)
        /// </returns>
        public ActionResult RuzWord()
        {
            return PartialView();
        }
        /// <summary>
        /// in controller jahate generate kardane tasvire ruz bekar miravad
        /// </summary>
        /// <returns>
        /// ActionResultc : PartialView(SpeechVideo)
        /// </returns>
        [ChildActionOnly]
        public ActionResult RuzPic()
        {
            return PartialView();
        }

        /// <summary>
        /// in controller jahate generate kardane video ye ruz bekar miravad
        /// </summary>
        /// <returns>
        /// ActionResultc : PartialView(SpeechVideo)
        /// </returns>
        [ChildActionOnly]
        public ActionResult SpeechVideo()
        {
            return PartialView();
        }
        /// <summary>
        /// in controller jahate generate kardane AyineKhedmat shakhs bekar miravad
        /// </summary>
        /// <returns>
        /// ActionResultc : PartialView(AyineKhedmat)
        /// </returns>
        [ChildActionOnly]
        public ActionResult AyineKhedmat()
        {
            return PartialView();
        }

        /// <summary>
        /// in controller jahate generate kardane kholase amalarde shakhs bekar miravad
        /// </summary>
        /// <returns>
        /// ActionResultc : PartialView(BrifeFunctional)
        /// </returns>
        [ChildActionOnly]
        public ActionResult BrifeFunctional()
        {
            return PartialView();
        }
        /// <summary>
        /// in controller jahate generate kardane slide khabariye makhsuse khabar haye omumi va hoze safheye nokhost bekar miravad
        /// </summary>
        /// <param name="CategoryId">idie shakheye morede nazar</param>
        /// <returns>
        /// ActionResultc : PartialView(LatestNews)
        /// </returns>
        [ChildActionOnly]
        public ActionResult LatestNews(int CategoryId)
        {
            PostManagement post = new PostManagement();
            List<PostModel> model = new List<PostModel>();
            model.AddRange(post.LatestNewsAndParliamentPost("News"));
            return PartialView(model);
        }

        /// <summary>
        /// in controller jahate generate kardane slide khabariye makhsuse khabar haye majlese safheye nokhost bekar miravad
        /// </summary>
        /// <param name="CategoryId">idie shakheye morede nazar</param>
        /// <returns>
        /// ActionResultc : PartialView(LatestParliament)
        /// </returns>
        [ChildActionOnly]
        public ActionResult LatestParliament(int CategoryId)
        {
            PostManagement post = new PostManagement();
            List<PostModel> model = new List<PostModel>();
            model.AddRange(post.LatestNewsAndParliamentPost("Parliament"));
            return PartialView(model);
        }
        /// <summary>
        /// in controller jahate generate kardane slidere safheye nokhost bekar miravad
        /// bar asase 5 poste akhar
        /// </summary>
        /// <returns>
        /// ActionResultc : PartialView(SliderGenerator)
        /// </returns>
        [ChildActionOnly]
        public ActionResult SliderGenerator()
        {
            PostManagement post = new PostManagement();
            List<Post> model = new List<Post>();
            using (BigMill.Models.Entities db = new Entities())
            {
                var Slides = db.Posts.Where(t => t.Display == true && t.Status == true).OrderByDescending(u => u.CreatedOnUTC).Take(5);
                foreach (var item in Slides)
                {
                    model.Add(item);
                }
            }
            return PartialView(model);
        }
        /// <summary>
        /// in controller jahate generate kardane navar bare ettelaiyye ha bekar miravad 
        /// bar asase 5 poste akhar
        /// </summary>
        /// <returns>
        /// ActionResult : PartialView(InformationBar)
        /// </returns>
        [ChildActionOnly]
        public ActionResult InformationBar()
        {
            PostManagement post = new PostManagement();
            List<PostModel> list = new List<PostModel>();
            using (BigMill.Models.Entities db = new Entities())
            {
                var ListObject = db.Posts.Where(t => t.Display == true && t.Status == true).OrderByDescending(u => u.CreatedOnUTC).Select(y => new {y.Content_One, y.CreatedOnUTC}).Take(5);
                foreach (var item in ListObject)
                {
                    PostModel ListItem = new PostModel();
                    ListItem.Content_One = item.Content_One;
                    ListItem.CreateDateUtcJalali = Tools.GetDateTimeReturnJalaliDate(item.CreatedOnUTC ?? default(DateTime));
                    list.Add(ListItem);
                }
            }
            return PartialView(list);
        }
        /// <summary>
        /// in controller jahate generate kardane albome axe footere safheye nokhost bekar miravad
        /// bar asase 6 poste akhar
        /// </summary>
        /// <returns>
        /// ActionResult : PartialView(FooterImageGallery)
        /// </returns>
        [ChildActionOnly]
        public ActionResult FooterImageGallery()
        {
            PostManagement post = new PostManagement();
            List<Post> model = new List<Post>();
            using (BigMill.Models.Entities db = new Entities())
            {
                var Slides = db.Posts.Where(t => t.Display == true && t.Status == true).OrderBy(u => u.CreatedOnUTC).Take(6);
                foreach (var item in Slides)
                {
                    model.Add(item);
                }
            }
            return PartialView(model);
        }
    }
}