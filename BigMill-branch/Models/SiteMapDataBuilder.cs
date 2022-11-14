using System;
using System.Collections.Generic;
using SimpleMvcSitemap.Images;
using SimpleMvcSitemap.Mobile;
using SimpleMvcSitemap.News;
using SimpleMvcSitemap.StyleSheets;
using SimpleMvcSitemap.Translations;
using SimpleMvcSitemap.Videos;
using SimpleMvcSitemap;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using System.Data.Entity;
using BigMill.Areas.Admin3mill.Models;
using HtmlAgilityPack;

namespace BigMill.Models
{
    public class SiteMapDataBuilder
    {
        UrlHelper url;
        public SiteMapDataBuilder()
        {
            url = new UrlHelper(HttpContext.Current.Request.RequestContext);
        }

        public string Truncate(string str)
        {
            int Maxlenght = 150;
            return str.Length <= Maxlenght ? str : str.Substring(0, Maxlenght).TrimEnd('-');
        }

        public List<SitemapNode> CreateSitemapNodeWithAllProperties()
        {
            List<SitemapNode> List = new List<SitemapNode>
        {
            new SitemapNode(url.Action("Index","Home")){
                LastModificationDate = new DateTime(2015, 11, 01, 16, 05, 00, DateTimeKind.Utc),
                ChangeFrequency = ChangeFrequency.Daily,
                Priority = 1M
            },
            new SitemapNode(url.Action("StaticView","Static",new {FileName="AcademicRecords"})){
                            LastModificationDate = new DateTime(2015, 11, 01, 16, 05, 00, DateTimeKind.Utc),
                ChangeFrequency = ChangeFrequency.Never,
                Priority = 0.8M
            },
               new SitemapNode(url.Action("StaticView","Static",new {FileName="Biography"})){
                            LastModificationDate = new DateTime(2015, 11, 01, 16, 05, 00, DateTimeKind.Utc),
                ChangeFrequency = ChangeFrequency.Never,
                Priority = 0.8M
            },
              new SitemapNode(url.Action("StaticView","Static",new {FileName="BrifeFunctional"})){
                             LastModificationDate = new DateTime(2015, 11, 01, 16, 05, 00, DateTimeKind.Utc),
                ChangeFrequency = ChangeFrequency.Never,
                Priority = 0.8M
            },
         new SitemapNode(url.Action("StaticView","Static",new {FileName="PeopleContactOffice"})){
                      LastModificationDate = new DateTime(2015, 11, 01, 16, 05, 00, DateTimeKind.Utc),
                ChangeFrequency = ChangeFrequency.Never,
                Priority = 0.8M
            },   
            new SitemapNode(url.Action("StaticView","Static",new {FileName="Resume"})){
                              LastModificationDate = new DateTime(2015, 11, 01, 16, 05, 00, DateTimeKind.Utc),
                ChangeFrequency = ChangeFrequency.Never,
                Priority = 0.8M
            },
                new SitemapNode(url.Action("StaticView","Static",new {FileName="TicketTutorial"})){
                     LastModificationDate = new DateTime(2015, 11, 01, 16, 05, 00, DateTimeKind.Utc),
                ChangeFrequency = ChangeFrequency.Never,
                Priority = 1M
            },
            //other nodes
        };

            using (BigMill.Models.Entities db = new BigMill.Models.Entities())
            {
                var query = db.Posts.Where(u => u.Display == true && u.Status == true).OrderByDescending(r => r.CreatedOnUTC).Select(u => new { u.Content_One, u.F_PostsSubCategory_Id, u.ID, u.CreatedOnUTC });
                int pagesize = int.Parse(System.Configuration.ConfigurationManager.AppSettings["PagedlistSize"]);

                int AkhbarOmumiId = int.Parse(System.Configuration.ConfigurationManager.AppSettings["AkhbarOmumiId"]);
                int AkhbarHozeID = int.Parse(System.Configuration.ConfigurationManager.AppSettings["AkhbarHozeID"]);
                int SpeechID = int.Parse(System.Configuration.ConfigurationManager.AppSettings["SpeechID"]);
                int InterviewID = int.Parse(System.Configuration.ConfigurationManager.AppSettings["InterviewID"]);
                int QuestionID = int.Parse(System.Configuration.ConfigurationManager.AppSettings["QuestionID"]);
                int CorrespondenceID = int.Parse(System.Configuration.ConfigurationManager.AppSettings["CorrespondenceID"]);

                int i = 0;
                int CurrentPage = 0;
                var AkhbarOmoomi = query.Where(u => u.F_PostsSubCategory_Id == AkhbarOmumiId);
                foreach (var n in AkhbarOmoomi)
                {
                    int tempCurrentPage = (i / pagesize) + 1;
                    if (CurrentPage != tempCurrentPage)
                    {
                        CurrentPage = tempCurrentPage;
                        string parrentPath = url.Action("AkhbarOmumi", "News", new { page = CurrentPage });
                        List.Add(new SitemapNode(parrentPath)
                        {
                            LastModificationDate = (n.CreatedOnUTC.HasValue==true)? n.CreatedOnUTC.Value.ToUniversalTime():DateTime.Now.ToUniversalTime(),
                            ChangeFrequency = ChangeFrequency.Weekly,
                            Priority = 0.8M
                        });
                    }
                    string tempExtra = (System.Text.RegularExpressions.Regex.Replace(n.Content_One, @"[^\w\040@-]", "",
                              System.Text.RegularExpressions.RegexOptions.None)).Replace(" ", "-");
                    string path = System.Configuration.ConfigurationManager.AppSettings["WebSiteURL"] + "News/AkhbarOmumi_Detail/" + n.ID + "/" + tempExtra;
                    List.Add(new SitemapNode(Truncate(path))
                    {
                        LastModificationDate = (n.CreatedOnUTC.HasValue == true) ? n.CreatedOnUTC.Value.ToUniversalTime() : DateTime.Now.ToUniversalTime(),
                        ChangeFrequency = ChangeFrequency.Weekly,
                        Priority = 0.5M
                    });
                    i++;
                }

                 i = 0;
                 CurrentPage = 0;
                 var AkhbarHoze = query.Where(u => u.F_PostsSubCategory_Id == AkhbarHozeID);
                foreach (var n in AkhbarHoze)
                {
                    int tempCurrentPage = (i / pagesize) + 1;
                    if (CurrentPage != tempCurrentPage)
                    {
                        CurrentPage = tempCurrentPage;
                        string parrentPath = url.Action("AkhbarHozeEntekhabiye", "News", new { page = CurrentPage });
                        List.Add(new SitemapNode(parrentPath)
                        {
                            LastModificationDate = (n.CreatedOnUTC.HasValue == true) ? n.CreatedOnUTC.Value.ToUniversalTime() : DateTime.Now.ToUniversalTime(),
                            ChangeFrequency = ChangeFrequency.Weekly,
                            Priority = 0.8M
                        });
                    }
                    string tempExtra = (System.Text.RegularExpressions.Regex.Replace(n.Content_One, @"[^\w\040@-]", "",
                              System.Text.RegularExpressions.RegexOptions.None)).Replace(" ", "-");
                    string path = System.Configuration.ConfigurationManager.AppSettings["WebSiteURL"] + "News/AkhbarHozeEntekhabiye_Detail/" + n.ID + "/" + tempExtra;
                    List.Add(new SitemapNode(Truncate(path))
                    {
                        LastModificationDate = (n.CreatedOnUTC.HasValue == true) ? n.CreatedOnUTC.Value.ToUniversalTime() : DateTime.Now.ToUniversalTime(),
                        ChangeFrequency = ChangeFrequency.Weekly,
                        Priority = 0.5M
                    });
                    i++;
                }



                i = 0;
                CurrentPage = 0;
                var Speech = query.Where(u => u.F_PostsSubCategory_Id == SpeechID);
                foreach (var n in Speech)
                {
                    int tempCurrentPage = (i / pagesize) + 1;
                    if (CurrentPage != tempCurrentPage)
                    {
                        CurrentPage = tempCurrentPage;
                        string parrentPath = url.Action("ViewSpeech", "Parliament", new { page = CurrentPage });
                        List.Add(new SitemapNode(parrentPath)
                        {
                            LastModificationDate = (n.CreatedOnUTC.HasValue == true) ? n.CreatedOnUTC.Value.ToUniversalTime() : DateTime.Now.ToUniversalTime(),
                            ChangeFrequency = ChangeFrequency.Weekly,
                            Priority = 0.8M
                        });
                    }
                    string tempExtra = (System.Text.RegularExpressions.Regex.Replace(n.Content_One, @"[^\w\040@-]", "",
                              System.Text.RegularExpressions.RegexOptions.None)).Replace(" ", "-");
                    string path = System.Configuration.ConfigurationManager.AppSettings["WebSiteURL"] + "Parliament/ViewSpeech_Detail/" + n.ID + "/" + tempExtra;
                    List.Add(new SitemapNode(Truncate(path))
                    {
                        LastModificationDate = (n.CreatedOnUTC.HasValue == true) ? n.CreatedOnUTC.Value.ToUniversalTime() : DateTime.Now.ToUniversalTime(),
                        ChangeFrequency = ChangeFrequency.Weekly,
                        Priority = 0.5M
                    });
                    i++;
                }



                i = 0;
                CurrentPage = 0;
                var Interview = query.Where(u => u.F_PostsSubCategory_Id == InterviewID);
                foreach (var n in Interview)
                {
                    int tempCurrentPage = (i / pagesize) + 1;
                    if (CurrentPage != tempCurrentPage)
                    {
                        CurrentPage = tempCurrentPage;
                        string parrentPath = url.Action("ViewInterview", "Parliament", new { page = CurrentPage });
                        List.Add(new SitemapNode(parrentPath)
                        {
                            LastModificationDate = (n.CreatedOnUTC.HasValue == true) ? n.CreatedOnUTC.Value.ToUniversalTime() : DateTime.Now.ToUniversalTime(),
                            ChangeFrequency = ChangeFrequency.Weekly,
                            Priority = 0.8M
                        });
                    }
                    string tempExtra = (System.Text.RegularExpressions.Regex.Replace(n.Content_One, @"[^\w\040@-]", "",
                              System.Text.RegularExpressions.RegexOptions.None)).Replace(" ", "-");
                    string path = System.Configuration.ConfigurationManager.AppSettings["WebSiteURL"] + "Parliament/ViewInterview_Detail/" + n.ID + "/" + tempExtra;
                    List.Add(new SitemapNode(Truncate(path))
                    {
                        LastModificationDate = (n.CreatedOnUTC.HasValue == true) ? n.CreatedOnUTC.Value.ToUniversalTime() : DateTime.Now.ToUniversalTime(),
                        ChangeFrequency = ChangeFrequency.Weekly,
                        Priority = 0.5M
                    });
                    i++;
                }


                i = 0;
                CurrentPage = 0;
                var Question = query.Where(u => u.F_PostsSubCategory_Id == QuestionID);
                foreach (var n in Question)
                {
                    int tempCurrentPage = (i / pagesize) + 1;
                    if (CurrentPage != tempCurrentPage)
                    {
                        CurrentPage = tempCurrentPage;
                        string parrentPath = url.Action("ViewQuestion", "Parliament", new { page = CurrentPage });
                        List.Add(new SitemapNode(parrentPath)
                        {
                            LastModificationDate = (n.CreatedOnUTC.HasValue == true) ? n.CreatedOnUTC.Value.ToUniversalTime() : DateTime.Now.ToUniversalTime(),
                            ChangeFrequency = ChangeFrequency.Weekly,
                            Priority = 0.8M
                        });
                    }
                    string tempExtra = (System.Text.RegularExpressions.Regex.Replace(n.Content_One, @"[^\w\040@-]", "",
                              System.Text.RegularExpressions.RegexOptions.None)).Replace(" ", "-");
                    string path = System.Configuration.ConfigurationManager.AppSettings["WebSiteURL"] + "Parliament/ViewQuestion_Detail/" + n.ID + "/" + tempExtra;
                    List.Add(new SitemapNode(Truncate(path))
                    {
                        LastModificationDate = (n.CreatedOnUTC.HasValue == true) ? n.CreatedOnUTC.Value.ToUniversalTime() : DateTime.Now.ToUniversalTime(),
                        ChangeFrequency = ChangeFrequency.Weekly,
                        Priority = 0.5M
                    });
                    i++;
                }


                i = 0;
                CurrentPage = 0;
                var Correspondence = query.Where(u => u.F_PostsSubCategory_Id == CorrespondenceID);
                foreach (var n in Correspondence)
                {
                    int tempCurrentPage = (i / pagesize) + 1;
                    if (CurrentPage != tempCurrentPage)
                    {
                        CurrentPage = tempCurrentPage;
                        string parrentPath = url.Action("ViewCorrespondence", "Parliament", new { page = CurrentPage });
                        List.Add(new SitemapNode(parrentPath)
                        {
                            LastModificationDate = (n.CreatedOnUTC.HasValue == true) ? n.CreatedOnUTC.Value.ToUniversalTime() : DateTime.Now.ToUniversalTime(),
                            ChangeFrequency = ChangeFrequency.Weekly,
                            Priority = 0.8M
                        });
                    }
                    string tempExtra = (System.Text.RegularExpressions.Regex.Replace(n.Content_One, @"[^\w\040@-]", "",
                              System.Text.RegularExpressions.RegexOptions.None)).Replace(" ", "-");
                    string path = System.Configuration.ConfigurationManager.AppSettings["WebSiteURL"] + "Parliament/ViewCorrespondence_Detail/" + n.ID + "/" + tempExtra;
                    List.Add(new SitemapNode(Truncate(path))
                    {
                        LastModificationDate = (n.CreatedOnUTC.HasValue == true) ? n.CreatedOnUTC.Value.ToUniversalTime() : DateTime.Now.ToUniversalTime(),
                        ChangeFrequency = ChangeFrequency.Weekly,
                        Priority = 0.5M
                    });
                    i++;
                }
            }

            return List;
        }

        public SitemapIndexNode CreateSitemapIndexNodeWithRequiredProperties()
        {
            return new SitemapIndexNode("abc");
        }

        public SitemapIndexNode CreateSitemapIndexNodeWithAllProperties()
        {
            return new SitemapIndexNode("abc")
            {
                LastModificationDate = new DateTime(2013, 12, 11, 16, 05, 00, DateTimeKind.Utc)
            };
        }


        public List<SitemapNode> CreateSitemapNodeWithImageAllProperties()
        {


            List<SitemapNode> list = new List<SitemapNode>();

            using (BigMill.Models.Entities db = new BigMill.Models.Entities())
            {
                var img = db.Posts.Where(u => u.Display == true && u.Status == true).Select(u => new { u.Content_Two, u.Content_One, u.Content_Three, u.F_PostsSubCategory_Id, u.ID, u.Content_Four });
                int pagesize = int.Parse(System.Configuration.ConfigurationManager.AppSettings["PagedlistSize"]);


                foreach (var n in img)
                {

                    string tempExtra = (System.Text.RegularExpressions.Regex.Replace(n.Content_One, @"[^\w\040@-]", "",
                              System.Text.RegularExpressions.RegexOptions.None)).Replace(" ", "-");
                    string path = System.Configuration.ConfigurationManager.AppSettings["WebSiteURL"];

                    if (n.F_PostsSubCategory_Id == int.Parse(System.Configuration.ConfigurationManager.AppSettings["AkhbarPublicID"]))
                    {
                        path = path + "News/AkhbarOmumi_Detail/" + n.ID + "/" + tempExtra;
                    }
                    else if (n.F_PostsSubCategory_Id == int.Parse(System.Configuration.ConfigurationManager.AppSettings["AkhbarHozeID"]))
                    {
                        path = path + "News/AkhbarHozeEntekhabiye_Detail/" + n.ID + "/" + tempExtra;
                    }
                    else if (n.F_PostsSubCategory_Id == int.Parse(System.Configuration.ConfigurationManager.AppSettings["SpeechID"]))
                    {
                        path = path + "Parliament/ViewSpeech_Detail/" + n.ID + "/" + tempExtra;
                    }
                    else if (n.F_PostsSubCategory_Id == int.Parse(System.Configuration.ConfigurationManager.AppSettings["InterviewID"]))
                    {
                        path = path + "Parliament/ViewInterview_Detail/" + n.ID + "/" + tempExtra;
                    }
                    else if (n.F_PostsSubCategory_Id == int.Parse(System.Configuration.ConfigurationManager.AppSettings["QuestionID"]))
                    {
                        path = path + "Parliament/ViewQuestion_Detail/" + n.ID + "/" + tempExtra;
                    }
                    else if (n.F_PostsSubCategory_Id == int.Parse(System.Configuration.ConfigurationManager.AppSettings["CorrespondenceID"]))
                    {
                        path = path + "Parliament/ViewCorrespondence_Detail/" + n.ID + "/" + tempExtra;
                    }
                    SitemapNode node = new SitemapNode(Truncate(path));
                    List<SitemapImage> listImage = new List<SitemapImage>();
                    if (n.Content_Two != "NoImageTittle.jpg")
                    {
                        string imgPath = System.Configuration.ConfigurationManager.AppSettings["WebSiteURL"] + "Content/UplodedImages/PostImages/" + n.Content_Two;
                        listImage.Add(new SitemapImage(imgPath)
                        {
                            Caption = n.Content_Three,
                            Location = "Iran",
                            License = "http://hezareyesevom.ir",
                            Title = n.Content_One
                        });
                    }
                    string tempContent = Tools.ContentFour_Get(n.Content_Four);
                    HtmlAgilityPack.HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
                    document.LoadHtml(tempContent);

                    if (document.DocumentNode.SelectNodes("//img[@src]") != null)
                    {
                        HtmlNode[] detail_description_imgLink = document.DocumentNode.SelectNodes("//img[@src]").ToArray();
                        foreach (var q in detail_description_imgLink)
                        {
                            string temp = q.GetAttributeValue("src", string.Empty);
                            if (!temp.StartsWith("data", StringComparison.OrdinalIgnoreCase) && !temp.Contains("JiroCMS"))
                            {
                                SitemapImage im = new SitemapImage(q.GetAttributeValue("src", string.Empty))
                                {
                                    Caption = n.Content_Three,
                                    Location = "Iran",
                                    License = "http://hezareyesevom.ir",
                                    Title = n.Content_One
                                };
                                listImage.Add(im);
                            }
                        }
                    }
                    if (listImage.Count() > 0)
                    {
                        node.Images = listImage;
                        list.Add(node);
                    }
                }

            }
            return list;
        }

        public SitemapNode CreateSitemapNodeWithVideoRequiredProperties()
        {
            return new SitemapNode("http://www.example.com/videos/some_video_landing_page.html")
            {
                Video = new SitemapVideo("Grilling steaks for summer", "Alkis shows you how to get perfectly done steaks every time",
                                         "http://www.example.com/thumbs/123.jpg", "http://www.example.com/video123.flv")
            };
        }


        public SitemapNode CreateSitemapNodeWithVideoAllProperties()
        {
            return new SitemapNode("http://www.example.com/videos/some_video_landing_page.html")
            {
                Video = new SitemapVideo("Grilling steaks for summer", "Alkis shows you how to get perfectly done steaks every time",
                                         "http://www.example.com/thumbs/123.jpg", "http://www.example.com/video123.flv")
                {
                    Player = new VideoPlayer("http://www.example.com/videoplayer.swf?video=123")
                    {
                        AllowEmbed = YesNo.Yes,
                        Autoplay = "ap=1"
                    },
                    Duration = 600,
                    ExpirationDate = new DateTime(2014, 12, 16, 16, 56, 0, DateTimeKind.Utc),
                    Rating = 4.2M,
                    ViewCount = 12345,
                    PublicationDate = new DateTime(2014, 12, 16, 17, 51, 0, DateTimeKind.Utc),
                    FamilyFriendly = YesNo.No,
                    Tags = new[] { "steak", "summer", "outdoor" },
                    Category = "Grilling",
                    Restriction = new VideoRestriction("IE GB US CA", VideoRestrictionRelationship.Allow),
                    Gallery = new VideoGallery("http://cooking.example.com")
                    {
                        Title = "Cooking Videos"
                    },
                    Prices = new List<VideoPrice>
                    {
                        new VideoPrice("EUR",1.99M),
                        new VideoPrice("TRY",5.99M){Type = VideoPurchaseOption.Rent},
                        new VideoPrice("USD",2.99M){Resolution = VideoPurchaseResolution.Hd}
                    },
                    RequiresSubscription = YesNo.No,
                    Uploader = new VideoUploader("GrillyMcGrillerson")
                    {
                        Info = "http://www.example.com/users/grillymcgrillerson"
                    },
                    Platform = "web mobile",
                    Live = YesNo.Yes
                }
            };
        }

    


        public List<SitemapNode> CreateSitemapNodeWithNewsAllProperties()
        {

            List<SitemapNode> list = new List<SitemapNode>();

            using (BigMill.Models.Entities db = new BigMill.Models.Entities())
            {
                var news = from a in (from b in db.Posts join c in db.Tags_Posts_Mapping on b.ID equals c.F_Posts_Id join d in db.Tags on c.F_Tags_Id equals d.ID where b.Status == true && b.Display == true select new { b.Content_One, b.F_PostsSubCategory_Id, b.ID, b.CreatedOnUTC, d.Text }) group a by a.ID into grp select new { CI=grp.Key,
                pID=grp.Select(u=>u.F_PostsSubCategory_Id).FirstOrDefault(),
                mID=grp.Select(u=>u.ID).FirstOrDefault(),CD=grp.Select(u=>u.CreatedOnUTC).FirstOrDefault(),CO=grp.Select(u=>u.Content_One).FirstOrDefault(),TG=grp.Select(u=>u.Text)};
                    //db.Posts.Include(u => u.Tags_Posts_Mapping.FirstOrDefault().Tag).Where(u => u.Display == true && u.Status == true);

                foreach (var n in news)
                {
                    string tempExtra = (System.Text.RegularExpressions.Regex.Replace(n.CO, @"[^\w\040@-]", "",
                              System.Text.RegularExpressions.RegexOptions.None)).Replace(" ", "-");

                    string path = System.Configuration.ConfigurationManager.AppSettings["WebSiteURL"];

                    if (n.pID == int.Parse(System.Configuration.ConfigurationManager.AppSettings["AkhbarPublicID"]))
                    {
                        path = path + "News/AkhbarOmumi_Detail/" + n.mID + "/" + tempExtra;
                    }
                    else if (n.pID == int.Parse(System.Configuration.ConfigurationManager.AppSettings["AkhbarHozeID"]))
                    {
                        path = path + "News/AkhbarHozeEntekhabiye_Detail/" + n.mID + "/" + tempExtra;
                    }
                    else if (n.pID == int.Parse(System.Configuration.ConfigurationManager.AppSettings["SpeechID"]))
                    {
                        path = path + "Parliament/ViewSpeech_Detail/" + n.mID + "/" + tempExtra;
                    }
                    else if (n.pID == int.Parse(System.Configuration.ConfigurationManager.AppSettings["InterviewID"]))
                    {
                        path = path + "Parliament/ViewInterview_Detail/" + n.mID + "/" + tempExtra;
                    }
                    else if (n.pID == int.Parse(System.Configuration.ConfigurationManager.AppSettings["QuestionID"]))
                    {
                        path = path + "Parliament/ViewQuestion_Detail/" + n.mID + "/" + tempExtra;
                    }
                    else if (n.pID == int.Parse(System.Configuration.ConfigurationManager.AppSettings["CorrespondenceID"]))
                    {
                        path = path + "Parliament/ViewCorrespondence_Detail/" + n.mID + "/" + tempExtra;
                    }
                    SitemapNode node = new SitemapNode(Truncate(path));
                    //var TagsObject = n.Tags_Posts_Mapping.Where(u => u.F_Posts_Id == n.ID);
                    //var TagsObject = n.Where(u => u.F_Posts_Id == n.ID);
                   string tempTag = "";
                   foreach (var item in n.TG)
                   {
                       tempTag = tempTag + item + ",";

                   }
                   DateTime pipoDate = (n.CD.HasValue == true) ? n.CD.Value.ToUniversalTime() : DateTime.Now.ToUniversalTime();
                   tempTag = tempTag.Trim(',');
                   node.News = new SitemapNews(new NewsPublication("سامنا", "fa"), pipoDate, n.CO)
                {
                    Keywords = tempTag,
                };
                    list.Add(node);
                }

            }
            return list;
            //return new List<SitemapNode>{
            // new SitemapNode("http://www.example.org/business/article55.html")
            //{
            //    News = new SitemapNews(new NewsPublication("The Example Times", "en"), new DateTime(2014, 11, 5, 0, 0, 0, DateTimeKind.Utc), "Companies A, B in Merger Talks")
            //    {
            //        Access = NewsAccess.Subscription,
            //        Genres = "PressRelease, Blog",
            //        Keywords = "business, merger, acquisition, A, B",
            //        StockTickers = "NASDAQ:A, NASDAQ:B"
            //    }
            //}
            //};
        }



        public SitemapNode CreateSitemapNodeWithMobile()
        {
            return new SitemapNode("http://mobile.example.com/article100.html") { Mobile = new SitemapMobile() };
        }

        public SitemapModel CreateSitemapWithTranslations()
        {
            var sitemapNodes = new List<SitemapNode>
            {
                new SitemapNode("abc")
                {
                    Translations = new List<SitemapPageTranslation>
                    {
                        new SitemapPageTranslation("cba", "de")
                    }
                },
                new SitemapNode("def")
                {
                    Translations = new List<SitemapPageTranslation>
                    {
                        new SitemapPageTranslation("fed", "de")
                    }
                }
            };

            return new SitemapModel(sitemapNodes);
        }

        public SitemapModel CreateSitemapWithSingleStyleSheet()
        {
            return new SitemapModel(new List<SitemapNode> { new SitemapNode("abc") })
            {
                StyleSheets = new List<XmlStyleSheet>
                {
                    new XmlStyleSheet("/sitemap.xsl")
                }
            };
        }

        public SitemapModel CreateSitemapWithMultipleStyleSheets()
        {
            return new SitemapModel(new List<SitemapNode> { new SitemapNode("abc") })
            {
                StyleSheets = new List<XmlStyleSheet>
                {
                    new XmlStyleSheet("/regular.css") {Type = "text/css",Title = "Regular fonts",Media = "screen"},
                    new XmlStyleSheet("/bigfonts.css") {Type = "text/css",Title = "Extra large fonts",Media = "projection",Alternate = YesNo.Yes},
                    new XmlStyleSheet("/smallfonts.css") {Type = "text/css",Title = "Smaller fonts",Media = "print",Alternate = YesNo.Yes,Charset = "UTF-8"}
                }
            };
        }

    }
}