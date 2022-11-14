using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using CKSource.FileSystem;
using BigMill.Areas.Admin3mill.Models;
using System.Globalization;
using PagedList;

namespace BigMill.Models
{
    public class PostManagement
    {
        /// <summary>
        /// in tabe baraye ezafe kardane poste jadid morede estefade gharar migirad
        /// tabe (AddMidTable) dar in tabe farakhani mishavad
        /// </summary>
        /// <param name="model"></param>
        /// <param name="F_PostsSubCategory_Id">idie zirshakheyi ke in post mortabet be aan ast</param>
        /// <returns>
        /// int : idie initiate shode baraye in post dar database
        /// </returns>
        public int AddPost(PostModel model, int F_PostsSubCategory_Id)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                Post InsertObject = new Post();
                InsertObject.Content_Three = model.Content_Three;
                InsertObject.Content_Two = model.Content_Two;
                InsertObject.Content_One = model.Content_One;
                InsertObject.CreatedOnUTC = DateTime.Now;
                InsertObject.Display = model.Display;
                InsertObject.F_PostsSubCategory_Id = F_PostsSubCategory_Id;
                InsertObject.F_User_Id = model.F_Users_Id;
                InsertObject.NumberOfComments = 0;
                InsertObject.NumberOfDislikes = 0;
                InsertObject.NumberOfLikes = 0;
                InsertObject.NumberOfVisitors = 0;
                InsertObject.Status = true;
                db.Posts.Add(InsertObject);
                db.SaveChanges();
                if (model.Video != null)
                {
                    var FM = new FileManagement();
                    FM.AddFile(InsertObject.ID, model.Video);
                }
                InsertObject.Content_Four = Tools.ContentFour_Save(model.Content_Four, InsertObject.ID);
                db.SaveChanges();
                //in tabe baraye zakhireye tag haye marbut be in post morede estefade gharar migirad
                AddMidTable(InsertObject.ID, model.TagsText.TrimStart('#').Split('#').ToList());
                return InsertObject.ID;
            }
        }


        /// <summary>
        /// in tabe jahate zakhire saziye tag haye yek post morede estefade gharar migirad.
        /// </summary>
        /// <param name="F_Posts_Id">idie posti ke mikhahid tag haye aan ra zakhire sazi konid</param>
        /// <param name="list">liste tag haye post</param>
        private void AddMidTable(int F_Posts_Id, List<string> list)
        {
            using (BigMill.Models.Entities db = new BigMill.Models.Entities())
            {
                var Tags = db.Tags;
                foreach (var ListItem in list)
                {
                    if (!string.IsNullOrEmpty(ListItem))
                    {
                        var IsFound = Tags.FirstOrDefault(u => u.Text == ListItem);
                        // agar tag dar table tag ha mojud nabashad ebteda tag ra dar table tag zakhire karde sepas table miyani ra por mikonim
                        if (IsFound == null)
                        {
                            Tag InsertTag = new Tag();
                            InsertTag.Text = ListItem;
                            InsertTag.CreatedOnUTC = DateTime.Now;
                            db.Tags.Add(InsertTag);
                            db.SaveChanges();
                            Tags_Posts_Mapping InsertTagsPosts = new Tags_Posts_Mapping();
                            InsertTagsPosts.F_Posts_Id = F_Posts_Id;
                            InsertTagsPosts.F_Tags_Id = InsertTag.ID;
                            db.Tags_Posts_Mapping.Add(InsertTagsPosts);
                            db.SaveChanges();
                        }
                        // agar tag dar table tag ha mojud bashad mostaghiman table miyani ra por mikonim
                        else
                        {
                            Tags_Posts_Mapping InsertTagsPosts = new Tags_Posts_Mapping();
                            InsertTagsPosts.F_Posts_Id = F_Posts_Id;
                            InsertTagsPosts.F_Tags_Id = IsFound.ID;
                            db.Tags_Posts_Mapping.Add(InsertTagsPosts);
                            db.SaveChanges();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// in tabe jahate virayeshe poste morede nazar estefade migardad
        /// Tavabe (DeleteMidTable , AddMidTable) dar in tabe farakhani mishavand
        /// </summary>
        /// <param name="model"></param>
        /// <param name="_newPic">agar axe marbut be post taghir yafte bashad bayad true shavad dar gheyre in surat false</param>
        /// <returns>
        /// string 
        /// NotFound : poste morede nazar vojud nadarad ya (statuse post == false) ast
        /// OK : agar post ba movaffaghiyyat virayesh shavad
        /// </returns>
        public string EditPost(PostModel model, bool _newPic)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var EditObject = db.Posts.FirstOrDefault(u => u.ID == model.ID);
                if (EditObject == null || EditObject.Status == false) { return "NotFound"; }
                Tools.ContentFour_Edit(model.Content_Four, EditObject.Content_Four);
                EditObject.Content_Three = model.Content_Three;
                EditObject.Content_One = model.Content_One;
                EditObject.F_User_Id = model.F_Users_Id;
                EditObject.Status = model.Status;
                if (_newPic)
                {
                    EditObject.Content_Two = model.Content_Two;
                }
                if (model.Video != null)
                {
                    var FM = new FileManagement();
                    FM.EditFile(EditObject.ID, model.Video);
                }
                db.SaveChanges();
                DeleteMidTable(model.ID);
                AddMidTable(model.ID, model.TagsText.TrimStart('#').Split('#').ToList());
                return "OK";
            }
        }

        /// <summary>
        /// in tabe jahate pak kardane tag haye marbut be poste morede nazar dar table miyani estefade migardad
        /// </summary>
        /// <param name="F_Post_Id">idie poste morede nazar</param>
        private void DeleteMidTable(int F_Post_Id)
        {
            using (BigMill.Models.Entities db = new BigMill.Models.Entities())
            {
                var MidTableMembers = db.Tags_Posts_Mapping.Where(u => u.F_Posts_Id == F_Post_Id);
                foreach (var item in MidTableMembers)
                {
                    db.Tags_Posts_Mapping.Remove(item);
                }
                db.SaveChanges();
            }
        }


        public int DeletePost(PostModel model)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var DeleteObject = db.Posts.FirstOrDefault(u => u.ID == model.ID);
                if (DeleteObject != null)
                {
                    DeleteMidTable(model.ID);
                    PostCommentManagement p = new PostCommentManagement();
                    p.DeleteCasacadePostComment(model.ID);
                    db.Posts.Remove(DeleteObject);
                    db.SaveChanges();
                    return 1;
                }
                else
                    return 2;
            }
        }


        public void DeleteCasacadePost(int F_PostsSubCategory_Id)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var DeleteObject = db.Posts.Where(u => u.F_PostsSubCategory_Id == F_PostsSubCategory_Id);
                foreach (var item in DeleteObject)
                {
                    PostCommentManagement temp = new PostCommentManagement();
                    temp.DeleteCasacadePostComment(item.ID);
                    db.Posts.Remove(item);
                }
                db.SaveChanges();
            }
        }

        /// <summary>
        /// in tabe baraye afzayeshe tedade Like haye poste morede nazar estefade migardad
        /// tabe staticiye (GetDateTimeReturnJalaliDate) az classe Tools dar in tabe farakhani shode ast
        /// </summary>
        /// <param name="PostId">idie poste morede nazar</param>
        /// <param name="flag">agar ghablan like shode false dar gheyre in surat true</param>
        /// <returns>
        /// PostModel
        /// model : agar ba movaffaghiyat anjam shavad 
        /// null : agar poste morede nazar yaft nashavad
        /// </returns>
        public PostModel LikePost(int PostId, bool flag)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var IsFound = db.Posts.FirstOrDefault(u => u.ID == PostId);
                if (IsFound != null)
                {
                    PostModel model = new PostModel();
                    if (flag == true)
                    {
                        IsFound.NumberOfLikes++;
                        db.SaveChanges();
                    }
                    model.CreateDateUtcJalali = Tools.GetDateTimeReturnJalaliDate(IsFound.CreatedOnUTC ?? default(DateTime));
                    model.ID = IsFound.ID;
                    model.NumberOfComments = IsFound.NumberOfComments ?? default(int);
                    model.NumberOfDisLikes = IsFound.NumberOfDislikes ?? default(int);
                    model.NumberOfLikes = IsFound.NumberOfLikes ?? default(int);
                    return model;
                }
                else
                    return null;
            }
        }


        /// <summary>
        /// in tabe baraye afzayeshe tedade DisLike haye poste morede nazar estefade migardad
        /// tabe staticiye (GetDateTimeReturnJalaliDate) az classe Tools dar in tabe farakhani shode ast
        /// </summary>
        /// <param name="PostId">idie poste morede nazar</param>
        /// <param name="flag">agar ghablan Dislike shode false dar gheyre in surat true</param>
        /// <returns>
        /// PostModel
        /// model : agar ba movaffaghiyat anjam shavad 
        /// null : agar poste morede nazar yaft nashavad
        /// </returns>
        public PostModel DisLikePost(int PostId, bool flag)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var IsFound = db.Posts.FirstOrDefault(u => u.ID == PostId);
                if (IsFound != null)
                {
                    PostModel model = new PostModel();
                    if (flag == true)
                    {
                        IsFound.NumberOfDislikes++;
                        db.SaveChanges();
                    }
                    model.CreateDateUtcJalali = Tools.GetDateTimeReturnJalaliDate(IsFound.CreatedOnUTC ?? default(DateTime));
                    model.ID = IsFound.ID;
                    model.NumberOfComments = IsFound.NumberOfComments ?? default(int);
                    model.NumberOfDisLikes = IsFound.NumberOfDislikes ?? default(int);
                    model.NumberOfLikes = IsFound.NumberOfLikes ?? default(int);
                    return model;
                }
                else
                    return null;
            }
        }

        /// <summary>
        /// in tabe jahate taghire vaziyate nemayeshe yek post morede estefade gharar migirad.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// string
        /// NotFound : agar poste morede nazar yaft nashavad
        /// OK : agar taghir vaziyate post ba movaffaghiyat anjam girad
        /// </returns>
        public string ChangeDisplayPost(PostModel model)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var ChangeDisplayObject = db.Posts.FirstOrDefault(u => u.ID == model.ID);
                if (ChangeDisplayObject == null || ChangeDisplayObject.Status == false) { return "NotFound"; }
                ChangeDisplayObject.Display = !ChangeDisplayObject.Display;
                db.SaveChanges();
                return "OK";
            }
        }

        /// <summary>
        /// in tabe baraye bazgardani *Jadid tarin* ax haye post ha jahate estefade dar gallery bekar gerefte mishavad
        /// dar in tabe az (paging)  estefade shode ast
        /// tabe staticiye (GetDateTimeReturnJalaliDate) az classe Tools dar in tabe farakhani shode ast
        /// </summary>
        /// <param name="pageNumber">shomare page feli</param>
        /// <param name="pageSize">tedade record haye har page</param>
        /// <param name="total">tedade kolle page ha</param>
        /// <returns>
        /// List<GalleryModel>
        /// </returns>
        public List<GalleryModel> PhotosGallery(int pageNumber, int pageSize, out int total)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                //az F_PostsSubCategory_Id dar ghesmate view baraye daste bandiye ax ha estefade mikonim.
                var ListObject = db.Posts.Where(u => u.Display == true && u.Status == true).OrderByDescending(t => t.CreatedOnUTC).Select(y => new { y.ID, y.Content_One, y.CreatedOnUTC, y.Content_Two, y.F_PostsSubCategory_Id }).ToPagedList(pageNumber, pageSize);
                // ba bedast avardane tedade kolle record ha mikhahim tedade safahat ra bedast avaraim
                total = db.Posts.Where(u => u.Display == true && u.Status == true).Count();
                List<GalleryModel> list = new List<GalleryModel>();
                foreach (var item in ListObject)
                {
                    GalleryModel ListItem = new GalleryModel();
                    ListItem.ImgPath = item.Content_Two;
                    ListItem.Title = item.Content_One;
                    ListItem.F_SubCategroy_Id = item.F_PostsSubCategory_Id ?? default(int);
                    ListItem.ID = item.ID;
                    ListItem.CreateDateUtcJalali = Tools.GetDateTimeReturnJalaliDate(item.CreatedOnUTC ?? default(DateTime));
                    list.Add(ListItem);
                }
                return list;
            }
        }

        /// <summary>
        /// in tabe baraye bazgardandane 4 ta az mahbub tarin post ha ( bar asase tedade like ha ) morede estefade gharar migirad.
        /// tabe staticiye (GetDateTimeReturnJalaliDate) az classe Tools dar in tabe farakhani shode ast
        /// Eeemale (...) bar ruye Titre khabar baraye jomalati ba bish az 60 character
        /// </summary>
        /// <returns>
        /// List<PostModel>
        /// </returns>
        public List<PostModel> ListPopularPosts()
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var ListObject = db.Posts.Where(u => u.Status == true && u.Display == true).OrderByDescending(t => (t.NumberOfLikes)).Select(y => new { y.ID, y.Content_One, y.CreatedOnUTC, y.Content_Two, y.F_PostsSubCategory_Id }).Take(4);
                List<PostModel> list = new List<PostModel>();
                foreach (var item in ListObject)
                {
                    PostModel ListItem = new PostModel();
                    ListItem.ID = item.ID;
                    ListItem.Content_Two = item.Content_Two;
                    // be dalile mahdudiyyat dar namayeshe tedade character ha dar view edameye jomalate bish az 60 character ra ba ... nemayesh midahim
                    if (item.Content_One.Count() < 60)
                    {
                        ListItem.Content_One = System.Text.RegularExpressions.Regex.Replace(item.Content_One, @"[^\w\040@-]", "",
                                    System.Text.RegularExpressions.RegexOptions.None); ;
                    }
                    else
                    {
                        ListItem.Content_One = System.Text.RegularExpressions.Regex.Replace(item.Content_One, @"[^\w\040@-]", "",
                                System.Text.RegularExpressions.RegexOptions.None);
                        ListItem.Content_One = item.Content_One.Substring(0, 60);
                        ListItem.Content_One = ListItem.Content_One.Remove(ListItem.Content_One.LastIndexOf(ListItem.Content_One.Split(' ').Last())) + " ...";
                    }
                    ListItem.F_PostsSubCategory_ID = item.F_PostsSubCategory_Id ?? default(int);
                    ListItem.CreateDateUtcJalali = Tools.GetDateTimeReturnJalaliDate(item.CreatedOnUTC ?? default(DateTime));
                    list.Add(ListItem);
                }
                return list;
            }
        }


        /// <summary>
        /// in tabe baraye bazgardandane 4 ta az akharin post haye vared shode morede estefade gharar migirad.
        /// tabe staticiye (GetDateTimeReturnJalaliDate) az classe Tools dar in tabe farakhani shode ast
        /// Eeemale (...) bar ruye Titre khabar baraye jomalati ba bish az 60 character
        /// </summary>
        /// <returns>
        /// List<PostModel>
        /// </returns>
        public List<PostModel> ListRecentPosts()
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var ListObject = db.Posts.Where(u => u.Status == true && u.Display == true).OrderByDescending(t => t.CreatedOnUTC).Select(y => new { y.ID, y.Content_One, y.CreatedOnUTC, y.Content_Two, y.F_PostsSubCategory_Id }).Take(4);
                List<PostModel> list = new List<PostModel>();
                foreach (var item in ListObject)
                {
                    PostModel ListItem = new PostModel();
                    ListItem.ID = item.ID;

                    ListItem.Content_Two = item.Content_Two;
                    // be dalile mahdudiyyat dar namayeshe tedade character ha dar view edameye jomalate bish az 60 character ra ba ... nemayesh midahim
                    if (item.Content_One.Count() < 60)
                    {
                        ListItem.Content_One = System.Text.RegularExpressions.Regex.Replace(item.Content_One, @"[^\w\040@-]", "",
                                    System.Text.RegularExpressions.RegexOptions.None);
                    }
                    else
                    {
                        ListItem.Content_One = System.Text.RegularExpressions.Regex.Replace(item.Content_One, @"[^\w\040@-]", "",
                              System.Text.RegularExpressions.RegexOptions.None);
                        ListItem.Content_One = item.Content_One.Substring(0, 60);
                        ListItem.Content_One = ListItem.Content_One.Remove(ListItem.Content_One.LastIndexOf(ListItem.Content_One.Split(' ').Last())) + " ...";
                    }
                    ListItem.F_PostsSubCategory_ID = item.F_PostsSubCategory_Id ?? default(int);
                    ListItem.CreateDateUtcJalali = Tools.GetDateTimeReturnJalaliDate(item.CreatedOnUTC ?? default(DateTime));
                    list.Add(ListItem);
                }
                return list;
            }
        }

        /// <summary>
        /// in tabe baraye list kardane tamame post haye zirshakheye morede nazar (ham ghabele nemayesh ham gheyre ghabele nemayesh) morede estefade gharar migirad (makhsuse admin)
        /// dar in tabe az (paging) estefade shode ast
        /// tabe staticiye (GetDateTimeReturnJalaliDate) az classe Tools dar in tabe farakhani shode ast
        /// </summary>
        /// <param name="F_PostsSubCategory_ID">idie zirshakheye morede nazar</param>
        /// <param name="pageNumber">shomare page feli</param>
        /// <param name="pageSize">tedade record haye har page</param>
        /// <param name="total">tedade kolle page ha</param>
        /// <returns>
        /// List<PostModel>
        /// </returns>
        public List<PostModel> ListSpecificPostAdmin(int F_PostsSubCategory_ID, int pageNumber, int pageSize, out int total)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var ListObject = db.Posts.Where(u => u.F_PostsSubCategory_Id == F_PostsSubCategory_ID && u.Status == true).OrderByDescending(w => w.CreatedOnUTC).Select(y => new { y.ID, y.NumberOfComments, y.NumberOfDislikes, y.NumberOfLikes, y.NumberOfVisitors, y.Status, y.Content_One, y.CreatedOnUTC, y.Display }).ToPagedList(pageNumber, pageSize);
                List<PostModel> list = new List<PostModel>();
                // tedade tamame post haye ghabele namayesh ra baraye fahmidane tedade kolle safahat bedast miavarim
                total = db.Posts.Where(m => m.F_PostsSubCategory_Id == F_PostsSubCategory_ID).Count();
                foreach (var item in ListObject)
                {
                    PostModel ListItem = new PostModel();
                    ListItem.Content_One = item.Content_One;
                    ListItem.ID = item.ID;
                    ListItem.Display = item.Display ?? default(bool);
                    ListItem.NumberOfComments = item.NumberOfComments ?? default(int);
                    ListItem.NumberOfDisLikes = item.NumberOfDislikes ?? default(int);
                    ListItem.NumberOfLikes = item.NumberOfLikes ?? default(int);
                    ListItem.NumberOfVisitors = item.NumberOfVisitors ?? default(int);
                    ListItem.Status = item.Status ?? default(bool);
                    ListItem.CreateDateUtcJalali = Tools.GetDateTimeReturnJalaliDate(item.CreatedOnUTC ?? default(DateTime));
                    list.Add(ListItem);
                }
                return list;
            }
        }


        /// <summary>
        /// in tabe baraye bazgardandane tamame post haye ghabele namayeshe zirshakheye morede nazar baraye (Android) morede estefade gharar migirad.
        /// tabe staticiye (GetDateTimeReturnJalaliDate) az classe Tools dar in tabe farakhani shode ast
        /// tabe (ContentFour_Get) az classe Tools dar in tabe farakhani shode ast
        /// </summary>
        /// <param name="F_PostsSubCategory_ID">idie zirshakheye morede nazar</param>
        /// <returns>
        /// List<PostModel>
        /// </returns>
        public List<PostModel> AndroidListSpecificPost(int F_PostsSubCategory_ID)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var ListObject = db.Posts.Where(u => u.Status == true && u.Display == true && u.F_PostsSubCategory_Id == F_PostsSubCategory_ID).OrderByDescending(r => r.CreatedOnUTC);
                List<PostModel> list = new List<PostModel>();
                foreach (var item in ListObject)
                {
                    PostModel ListItem = new PostModel();
                    ListItem.Content_One = item.Content_One;
                    ListItem.Content_Two = item.Content_Two;
                    ListItem.Content_Three = item.Content_Three;
                    ListItem.Content_Four = Tools.ContentFour_Get(item.Content_Four);
                    ListItem.ID = item.ID;
                    ListItem.F_PostsSubCategory_ID = item.F_PostsSubCategory_Id ?? default(int);
                    ListItem.CreateDateUtcJalali = Tools.GetDateTimeReturnJalaliDate(item.CreatedOnUTC ?? default(DateTime));
                    list.Add(ListItem);
                }
                return list;
            }
        }

        /// <summary>
        /// in tabe baraye list kardane 10 poste akhar marbut be akhbar ya majles morede estefade gharar migirad.
        /// tabe staticiye (GetDateTimeReturnJalaliDate) az classe Tools dar in tabe farakhani shode ast
        /// Eeemale (...) bar ruye kholase khabar baraye jomalati ba bish az 200 character
        /// </summary>
        /// <param name="Type">agar akhbar ast (News) agar majles ast (Parliament)</param>
        /// <returns>
        /// List<PostModel>
        /// </returns>
        public List<PostModel> LatestNewsAndParliamentPost(string Type)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                List<int> SubCats;
                if (Type == "News")
                {
                    SubCats = new List<int>() { int.Parse(System.Configuration.ConfigurationManager.AppSettings["AkhbarPublicID"]), int.Parse(System.Configuration.ConfigurationManager.AppSettings["AkhbarHozeID"]) };
                }
                else
                {
                    SubCats = new List<int>() { int.Parse(System.Configuration.ConfigurationManager.AppSettings["SpeechID"]), int.Parse(System.Configuration.ConfigurationManager.AppSettings["QuestionID"]), int.Parse(System.Configuration.ConfigurationManager.AppSettings["InterviewID"]), int.Parse(System.Configuration.ConfigurationManager.AppSettings["CorrespondenceID"]) };
                }
                var ListObject = db.Posts.Where(u => SubCats.Contains(u.F_PostsSubCategory_Id ?? default(int)) && u.Status == true && u.Display == true).OrderByDescending(r => r.CreatedOnUTC).Select(y => new { y.ID, y.Content_One, y.CreatedOnUTC, y.Content_Two, y.Content_Three, y.F_PostsSubCategory_Id }).Take(10);
                List<PostModel> list = new List<PostModel>();
                foreach (var item in ListObject)
                {
                    PostModel ListItem = new PostModel();
                    ListItem.Content_One = System.Text.RegularExpressions.Regex.Replace(item.Content_One, @"[^\w\040@-]", "",
                               System.Text.RegularExpressions.RegexOptions.None);
                    ListItem.Content_Two = item.Content_Two;
                    if (item.Content_Three.Count() < 200)
                    {
                        ListItem.Content_Three = item.Content_Three;
                    }
                    else
                    {
                        ListItem.Content_Three = item.Content_Three.Substring(0, 200);
                        ListItem.Content_Three = ListItem.Content_Three.Remove(ListItem.Content_Three.LastIndexOf(ListItem.Content_Three.Split(' ').Last())) + " ...";
                    }

                    ListItem.ID = item.ID;
                    ListItem.F_PostsSubCategory_ID = item.F_PostsSubCategory_Id ?? default(int);
                    ListItem.CreateDateUtcJalali = Tools.GetDateTimeReturnJalaliDate(item.CreatedOnUTC ?? default(DateTime));
                    list.Add(ListItem);
                }
                return list;
            }
        }

        /// <summary>
        /// in tabe baraye list kardane post haye zirshakheye morede nazar morede estefade gharar migirad (makhsuse user)
        /// dar in tabe az (paging) estefade shode ast
        /// tabe staticiye (GetDateTimeReturnJalaliDate) az classe Tools dar in tabe farakhani shode ast
        /// </summary>
        /// <param name="F_PostsSubCategory_ID">idie zirshakheye morede nazar</param>
        /// <param name="pageNumber">shomare page feli</param>
        /// <param name="pageSize">tedade record haye har page</param>
        /// <param name="total">tedade kolle page ha</param>
        /// <returns>
        /// List<PostModel>
        /// </returns>
        public List<PostModel> ListSpecificPost(int F_PostsSubCategory_ID, int pageNumber, int pageSize, out int total)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var ListObject = db.Posts.Where(u => u.F_PostsSubCategory_Id == F_PostsSubCategory_ID && u.Status == true && u.Display == true).OrderByDescending(r => r.CreatedOnUTC).Select(y => new { y.ID, y.NumberOfComments, y.NumberOfDislikes, y.NumberOfLikes, y.NumberOfVisitors, y.Content_One, y.CreatedOnUTC, y.Content_Two, y.Content_Three, y.F_PostsSubCategory_Id }).ToPagedList(pageNumber, pageSize);
                List<PostModel> list = new List<PostModel>();
                total = ListObject.TotalItemCount;
                foreach (var item in ListObject)
                {
                    PostModel ListItem = new PostModel();
                    ListItem.Content_One = System.Text.RegularExpressions.Regex.Replace(item.Content_One, @"[^\w\040@-]", "",
                                System.Text.RegularExpressions.RegexOptions.None);
                    ListItem.Content_Two = item.Content_Two;
                    ListItem.Content_Three = item.Content_Three;
                    ListItem.ID = item.ID;
                    ListItem.F_PostsSubCategory_ID = item.F_PostsSubCategory_Id ?? default(int);
                    ListItem.NumberOfComments = item.NumberOfComments ?? default(int);
                    ListItem.NumberOfDisLikes = item.NumberOfDislikes ?? default(int);
                    ListItem.NumberOfLikes = item.NumberOfLikes ?? default(int);
                    ListItem.NumberOfVisitors = item.NumberOfVisitors ?? default(int);
                    ListItem.CreateDateUtcJalali = Tools.GetDateTimeReturnJalaliDate(item.CreatedOnUTC ?? default(DateTime));
                    list.Add(ListItem);
                }
                return list;
            }
        }


        /// <summary>
        /// in tabe baraye list kardane post hayi ke reshteye morede nazare josteju ra dar titr ya kholase ya matne asli poste khod darad morede estefade gharar migirad
        /// dar in tabe az (paging) estefade shode ast
        /// tabe staticiye (GetDateTimeReturnJalaliDate) az classe Tools dar in tabe farakhani shode ast
        /// </summary>
        /// <param name="SearchBox">reshteye morede nazar jahate josteju</param>
        /// <param name="pageNumber">shomare page feli</param>
        /// <param name="pageSize">tedade record haye har page</param>
        /// <param name="total">tedade kolle page ha</param>
        /// <returns>
        /// List<PostModel>
        /// </returns>
        public List<PostModel> SearchResult(string SearchBox, int pageNumber, int pageSize, out int total)
        {
            List<PostModel> list = new List<PostModel>();
            using (BigMill.Models.Entities db = new Entities())
            {
                var ListObject = db.Posts.Include(o => o.PostsSubCategory).Where(t => t.Display == true && t.Status == true && (t.Content_One.Contains(SearchBox) || t.Content_Three.Contains(SearchBox) || t.Content_Four.Contains(SearchBox))).OrderByDescending(u => u.CreatedOnUTC).Select(y => new { y.ID, y.NumberOfComments, y.NumberOfDislikes, y.NumberOfLikes, y.NumberOfVisitors, y.Content_One, y.CreatedOnUTC, y.Content_Two, y.Content_Three, y.F_PostsSubCategory_Id }).ToPagedList(pageNumber, pageSize);
                total = db.Posts.Include(o => o.PostsSubCategory).Where(t => t.Display == true && t.Status == true && (t.Content_One.Contains(SearchBox) || t.Content_Three.Contains(SearchBox) || t.Content_Four.Contains(SearchBox))).Count();
                foreach (var item in ListObject)
                {
                    PostModel ListItem = new PostModel();
                    ListItem.Content_One = System.Text.RegularExpressions.Regex.Replace(item.Content_One, @"[^\w\040@-]", "",
                                System.Text.RegularExpressions.RegexOptions.None);
                    ListItem.Content_Two = item.Content_Two;
                    ListItem.Content_Three = item.Content_Three;
                    ListItem.ID = item.ID;
                    ListItem.F_PostsSubCategory_ID = item.F_PostsSubCategory_Id ?? default(int);
                    ListItem.NumberOfComments = item.NumberOfComments ?? default(int);
                    ListItem.NumberOfDisLikes = item.NumberOfDislikes ?? default(int);
                    ListItem.NumberOfLikes = item.NumberOfLikes ?? default(int);
                    ListItem.CreateDateUtcJalali = Tools.GetDateTimeReturnJalaliDate(item.CreatedOnUTC ?? default(DateTime));
                    list.Add(ListItem);
                }
            }
            return list;
        }


        /// <summary>
        /// in tabe baraye bazgardandane ettelaate poste morede nazar bekar gerefte mishavad
        /// tavabe staticiye (GetDateTimeReturnJalaliDate , ContentFour_Get) az classe Tools dar in tabe farakhani shode and 
        /// tavabe (SpecificListPostComment , GetTags) dar in tabe farakhani mishavand
        /// </summary>
        /// <param name="PostId">idie poste morede nazar</param>
        /// <param name="F_PostSubCategory_Id">idie zirshakheye morede nazar</param>
        /// <returns>
        /// PostModel
        /// </returns>
        public PostModel PostDetails(int PostId, int F_PostSubCategory_Id)
        {
            using (BigMill.Models.Entities db = new BigMill.Models.Entities())
            {
                var IsFound = db.Posts.FirstOrDefault(u => u.ID == PostId && u.F_PostsSubCategory_Id == F_PostSubCategory_Id);
                PostModel model = new PostModel();
                model.Content_One = IsFound.Content_One;
                model.Content_Two = IsFound.Content_Two;
                model.Content_Three = IsFound.Content_Three;
                model.Content_Four = Tools.ContentFour_Get(IsFound.Content_Four);
                model.ID = IsFound.ID;
                model.Display = IsFound.Display ?? default(bool);
                model.F_PostsSubCategory_ID = IsFound.F_PostsSubCategory_Id ?? default(int);
                model.F_Users_Id = IsFound.F_User_Id ?? default(int);
                model.NumberOfComments = IsFound.NumberOfComments ?? default(int);
                model.NumberOfDisLikes = IsFound.NumberOfDislikes ?? default(int);
                model.NumberOfLikes = IsFound.NumberOfLikes ?? default(int);
                model.NumberOfVisitors = IsFound.NumberOfVisitors ?? default(int);
                model.Status = IsFound.Status ?? default(bool);
                model.CreateDateUtcJalali = Tools.GetDateTimeReturnJalaliDate(IsFound.CreatedOnUTC ?? default(DateTime));
                model.TagsText = GetTags(IsFound.ID);
                PostCommentManagement comment = new PostCommentManagement();
                model.Comments = comment.SpecificListPostComment(PostId);
                return model;
            }
        }


        /// <summary>
        /// in tabe jahate gereftane tag haye marbut be poste morede nazar estefade mishavad
        /// dar in tabe az (EagerLoading) estefade shode ast
        /// </summary>
        /// <param name="PostId">idie poste morede nazar</param>
        /// <returns>
        /// string
        /// reshteyi az tag ha ke ba charactere '#' az ham digar joda shode and
        /// </returns>
        public string GetTags(int PostId)
        {
            using (BigMill.Models.Entities db = new BigMill.Models.Entities())
            {
                string Result = "";
                var TagsObject = db.Tags_Posts_Mapping.Include(y => y.Tag);
                var Tags = TagsObject.Where(u => u.F_Posts_Id == PostId);
                if (Tags != null)
                {
                    foreach (var item in Tags)
                    {
                        Result += item.Tag.Text + "#";
                    }
                    if (Result != "")
                        Result = Result.Remove(Result.LastIndexOf("#"), 1);
                    return Result;
                }
                return Result;
            }
        }


        /// <summary>
        /// in tabe baraye yaftane tamamiye tag haye poste morede nazar va tag haye zirshakhe haye aan va tag haye shakheye marbut be aan morede estefade gharar migirad
        /// dar in tabe az (EagerLoading) estefade mishavad
        /// </summary>
        /// <param name="PostId">idie poste morede nazar</param>
        /// <param name="PostSubCategoryId">idie zirshakheye poste morede nazar</param>
        /// <param name="PostCategoryId">idie shakheye poste morede nazar</param>
        /// <returns>
        /// List<TagSelectModel>
        /// </returns>
        public List<TagSelectModel> GetPostTags(int PostId, int PostSubCategoryId, int PostCategoryId)
        {
            using (BigMill.Models.Entities db = new BigMill.Models.Entities())
            {
                List<TagSelectModel> Result = new List<TagSelectModel>();
                var TagsObject = db.Tags_Posts_Mapping.Include(y => y.Tag).Where(u => u.F_Posts_Id == PostId);
                foreach (var item in TagsObject)
                {
                    TagSelectModel m = new TagSelectModel();
                    m.TagText = item.Tag.Text;
                    m.ID = item.Tag.ID;
                    m.Type = "PostTag";
                    Result.Add(m);
                }
                Result.AddRange(GetSubCategoryTags(PostSubCategoryId, PostCategoryId));
                return Result;
            }
        }

        /// <summary>
        /// in tabe jahate gereftane tag haye zirshakhe morede nazar va tag haye shakheye aan bekar borde mishavad
        /// dar in tabe az (EagerLoading) estefade mishavad
        /// </summary>
        /// <param name="PostSubCategoryId">idiye zirshakheye morede nazar</param>
        /// <param name="PostCategoryId">idiye shakhe i ke idie</param>
        /// <returns>
        /// List<TagSelectModel>
        /// </returns>
        public List<TagSelectModel> GetSubCategoryTags(int PostSubCategoryId, int PostCategoryId)
        {
            using (BigMill.Models.Entities db = new BigMill.Models.Entities())
            {
                List<TagSelectModel> Result = new List<TagSelectModel>();
                var TagsObject = db.Tags_PostsSubCategory_Mapping.Include(y => y.Tag).Where(u => u.F_PostsSubCategory_Id == PostSubCategoryId);
                foreach (var item in TagsObject)
                {
                    TagSelectModel m = new TagSelectModel();
                    m.TagText = item.Tag.Text;
                    m.ID = item.Tag.ID;
                    m.Type = "SubTag";
                    Result.Add(m);
                }
                Result.AddRange(GetCategoryTags(PostCategoryId));
                return Result;
            }
        }

        /// <summary>
        /// in tabe baraye bargardandane tag haye shakheye morede nazar estefade migardad
        /// dar in tabe az (EagerLoading) estefade mishavad
        /// </summary>
        /// <param name="PostCategoryId">idie shakheye morede nazar</param>
        /// <returns>
        /// List<TagSelectModel>
        /// </returns>
        public List<TagSelectModel> GetCategoryTags(int PostCategoryId)
        {
            using (BigMill.Models.Entities db = new BigMill.Models.Entities())
            {
                List<TagSelectModel> Result = new List<TagSelectModel>();
                var TagsObject = db.Tags_PostsCategory_Mapping.Include(y => y.Tag).Where(u => u.F_PostsCategory_Id == PostCategoryId);
                foreach (var item in TagsObject)
                {
                    TagSelectModel m = new TagSelectModel();
                    m.TagText = item.Tag.Text;
                    m.ID = item.Tag.ID;
                    m.Type = "CatTag";
                    Result.Add(m);
                }
                return Result;
            }
        }

        /// <summary>
        /// in tabe jahate list kardane mahhaye sal az 1391/1/1 ta tarikhe emruz estefade migardad
        /// tabe staticiye (ToArabic) az classe Tools dar in tabe farakhani mishavad
        /// ba forme (فروردین ماه 1391)
        /// </summary>
        /// <param name="PostSubCategoryId">idie zirshakheye morede nazar</param>
        /// <returns>
        /// Archive
        /// </returns>
        public Archive ArchiveDates(int PostSubCategoryId)
        {
            using (BigMill.Models.Entities db = new BigMill.Models.Entities())
            {
                string Year = "";
                Archive model = new Archive();
                PersianCalendar pers = new PersianCalendar();
                DateTime SystemStart = new DateTime(1391, 1, 1, pers);
                DateTime DateNow = DateTime.Now;
                int diffrenceYear = DateNow.Year - SystemStart.Year;
                int diffrence = (DateNow.Month - SystemStart.Month) + (diffrenceYear * 12);
                for (int i = 0; i < diffrence; i++)
                {
                    ArchiveModel AM = new ArchiveModel();
                    string month = "";
                    switch (pers.GetMonth(SystemStart))
                    {
                        case 1: month = "فروردین"; break;
                        case 2: month = "اردیبهشت"; break;
                        case 3: month = "خرداد"; break;
                        case 4: month = "تیر"; break;
                        case 5: month = "مرداد"; break;
                        case 6: month = "شهریور"; break;
                        case 7: month = "مهر"; break;
                        case 8: month = "آبان"; break;
                        case 9: month = "آذر"; break;
                        case 10: month = "دی"; break;
                        case 11: month = "بهمن"; break;
                        case 12: month = "اسفند"; break;
                    }
                    Year = Tools.ToArabic(pers.GetYear(SystemStart).ToString());
                    AM.Year = Year;
                    AM.ShowingDate = month + " ماه " + Year;
                    AM.Start = SystemStart;
                    SystemStart = pers.AddMonths(SystemStart, 1);
                    AM.End = SystemStart;
                    model.Archives.Add(AM);
                }
                model.Archives = model.Archives.OrderByDescending(u => u.End).ToList();
                model.PostSubCategoryId = PostSubCategoryId;
                return model;
            }
        }

        /// <summary>
        /// in tabe jahate list kardane post haye beyne tarikhe shuru va payan e zirshakheye morede nazar estefade migardad
        /// dar in tabe az (paging) estefade shode ast
        /// tabe staticiye (GetDateTimeReturnJalaliDate) az classe Tools dar in tabe farakhani shode ast 
        /// </summary>
        /// <param name="start">tarikhe shoru</param>
        /// <param name="end">tarikhe payan</param>
        /// <param name="F_PostSubCategory_Id">idie zirshakheye morede nazar</param>
        /// <param name="pageNumber">shomare page feli</param>
        /// <param name="pageSize">tedade record haye har page</param>
        /// <param name="total">tedade kolle page ha</param>
        /// <returns>
        /// List<PostModel>
        /// </returns>
        public List<PostModel> ListArchiveNews(DateTime start, DateTime end, int F_PostSubCategory_Id, int pageNumber, int pageSize, out int total)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var ListObject = db.Posts.Where(u => u.F_PostsSubCategory_Id == F_PostSubCategory_Id && u.Status == true && u.Display == true && !(u.CreatedOnUTC < start) && !(u.CreatedOnUTC > end)).OrderByDescending(r => r.CreatedOnUTC).Select(y => new { y.ID, y.NumberOfComments, y.NumberOfDislikes, y.NumberOfLikes, y.NumberOfVisitors, y.Content_One, y.CreatedOnUTC, y.Content_Two, y.Content_Three, y.F_PostsSubCategory_Id }).ToPagedList(pageNumber, pageSize);
                total = db.Posts.Where(u => u.F_PostsSubCategory_Id == F_PostSubCategory_Id && u.Status == true && u.Display == true && !(u.CreatedOnUTC < start) && !(u.CreatedOnUTC > end)).Count();
                List<PostModel> list = new List<PostModel>();
                foreach (var item in ListObject)
                {
                    PostModel ListItem = new PostModel();
                    ListItem.Content_One = System.Text.RegularExpressions.Regex.Replace(item.Content_One, @"[^\w\040@-]", "",
                                System.Text.RegularExpressions.RegexOptions.None);
                    ListItem.Content_Two = item.Content_Two;
                    ListItem.Content_Three = item.Content_Three;

                    ListItem.ID = item.ID;
                    ListItem.F_PostsSubCategory_ID = item.F_PostsSubCategory_Id ?? default(int);
                    ListItem.NumberOfComments = item.NumberOfComments ?? default(int);
                    ListItem.NumberOfDisLikes = item.NumberOfDislikes ?? default(int);
                    ListItem.NumberOfLikes = item.NumberOfLikes ?? default(int);
                    ListItem.NumberOfVisitors = item.NumberOfVisitors ?? default(int);
                    ListItem.CreateDateUtcJalali = Tools.GetDateTimeReturnJalaliDate(item.CreatedOnUTC ?? default(DateTime));
                    list.Add(ListItem);
                }
                return list;
            }
        }

    }
}