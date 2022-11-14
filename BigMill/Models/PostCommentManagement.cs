using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using BigMill.Areas.Admin3mill.Models;

namespace BigMill.Models
{
    public class PostCommentManagement
    {
        /// <summary>
        /// in tabe baraye ezafe kardane comment beyek post estefade migardad
        /// </summary>
        /// <param name="model">type="PostCommentModel"</param>
        /// <returns>
        /// int
        /// 1 : agar ba movaffaghiyyat anjam shavad
        /// 2 : dar surate khata dar sabte comment
        /// </returns>
        public int AddPostComment(PostCommentModel model)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                PostsComment InsertObject = new PostsComment();
                InsertObject.CreatedOnUTC = DateTime.Now;
                InsertObject.Display = false;
                InsertObject.F_Posts_Id = model.F_Posts_Id;
                InsertObject.NumberOfDislikes = 0;
                InsertObject.NumberOfLikes = 0;
                InsertObject.NumberOfReply = 0;
                InsertObject.Text = model.Text;
                db.PostsComments.Add(InsertObject);
                db.SaveChanges();
                if (InsertObject.ID > 0)
                    return 1;
                else
                    return 2;
            }
        }
        /// <summary>
        /// in tabe jahate afzayeshe tedade comment haye poste morede nazar estefade migardad
        /// </summary>
        /// <param name="PostId">Idie post</param>
        public void IncreasePostCommentNumber(int PostId)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var parent = db.Posts.FirstOrDefault(u => u.ID == PostId);
                parent.NumberOfComments = (parent.NumberOfComments == null) ? 1 : (parent.NumberOfComments + 1);
                db.SaveChanges();
            }
        }
        /// <summary>
        /// in tabe jahate Kaheshe tedade comment haye poste morede nazar estefade migardad
        /// </summary>
        /// <param name="PostId">Idie post</param>
        public void DecreasePostCommentNumber(int PostId)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var parent = db.Posts.FirstOrDefault(u => u.ID == PostId);
                parent.NumberOfComments--;
                db.SaveChanges();
            }
        }


        /// <summary>
        /// in tabe commente morede nazar ra be hamrahe tamame reply haye mortabet ba aan ra hazf mikonad
        /// tabe PermanentlyDeleteCasacadePostCommentReply dar in tabe farakhani shode ast
        /// </summary>
        /// <param name="Id">idie Commente morede nazar</param>
        /// <returns>
        /// string
        /// OK : darsurati ke ba movaffaghiyat anjam shavad
        /// NotFound : dar surati ke commente morede nazar mojud nabashad
        /// </returns>
        public string PermanentlyDeletePostComment(int Id)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var DeleteObject = db.PostsComments.FirstOrDefault(u => u.ID == Id);
                if (DeleteObject == null || DeleteObject.Display == true)
                {
                    return "NotFound";
                }
                PostCommentReplyManagement post = new PostCommentReplyManagement();
                //dar in ghesmat reply haye comment be tore kamel hazf migardand
                post.PermanentlyDeleteCasacadePostCommentReply(Id);
                db.PostsComments.Remove(DeleteObject);
                db.SaveChanges();
                return "OK";
            }
        }

        /// <summary>
        /// in tabe baraye pak kardane tamamiye comment ha va reply haye mortabet be poste morede nazar morede estefade gharar migirad
        /// Tabe PermanentlyDeleteCasacadePostCommentReply dar in tabe farakhani mishavad
        /// </summary>
        /// <param name="F_Posts_Id">idie poste morede nazar</param>
        public void DeleteCasacadePostComment(int F_Posts_Id)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var DeleteObject = db.PostsComments.Where(u => u.F_Posts_Id == F_Posts_Id);
                foreach (var item in DeleteObject)
                {
                    PostCommentReplyManagement temp = new PostCommentReplyManagement();
                    //dar in ghesmat reply haye comment be tore kamel hazf migardand
                    temp.PermanentlyDeleteCasacadePostCommentReply(item.ID);
                    db.PostsComments.Remove(item);
                    db.SaveChanges();
                }

            }
        }

        /// <summary>
        /// in tabe jahate afzayeshe tedade like haye commente morede nazar estefade migardad
        /// </summary>
        /// <param name="commentId">idie commente morede nazar</param>
        /// <param name="flag">flag agar ghablan in comment like shode bashad (false) dar gheyre in surat (true)</param>
        /// <returns>
        /// PostsComment
        /// IsFound : agar ba movaffaghiyyat sabt shavad
        /// null : agar commente morede nazar mojud nabashad
        /// </returns>
        public PostsComment LikePostComment(int commentId, bool flag)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var IsFound = db.PostsComments.FirstOrDefault(u => u.ID == commentId);
                if (IsFound != null)
                {
                    if (flag == true)
                    {
                        IsFound.NumberOfLikes++;
                        db.SaveChanges();
                    }
                    return IsFound;
                }
                else
                    return null;
            }
        }

        /// <summary>
        /// in tabe jahate afzayeshe tedade Dislike haye commente morede nazar estefade migardad
        /// </summary>
        /// <param name="commentId">idie commente morede nazar</param>
        /// <param name="flag">flag agar ghablan in comment DisLike shode bashad (false) dar gheyre in surat (true)</param>
        /// <returns>
        /// PostsComment
        /// IsFound : agar ba movaffaghiyyat sabt shavad
        /// null : agar commente morede nazar mojud nabashad
        public PostsComment DisLikePostComment(int commentId, bool flag)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var IsFound = db.PostsComments.FirstOrDefault(u => u.ID == commentId);
                if (IsFound != null)
                {
                    if (flag == true)
                    {
                        IsFound.NumberOfDislikes++;
                        db.SaveChanges();
                    }
                    return IsFound;
                }
                else
                    return null;
            }
        }

        /// <summary>
        /// in tabe baraye taghir vaziyate commente morede nazar estefade migardad
        /// Tavabe (IncreasePostCommentNumber , DecreasePostCommentNumber) tahte sharayeti farakhani mishavand va besurate fardi
        /// </summary>
        /// <param name="CommentID">idie commente morede nazar</param>
        /// <returns>
        /// string
        /// NotFound : agar commente morede nazar vojud nadashte bashad
        /// OK : agar ba movaffaghiyat anjam girad
        /// </returns>
        public string ChangeStatusPostComments(int CommentID)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var ChangeStatusObject = db.PostsComments.FirstOrDefault(u => u.ID == CommentID);
                if (ChangeStatusObject == null)
                {
                    return "NotFound";
                }

                ChangeStatusObject.Display = !ChangeStatusObject.Display;
                if (ChangeStatusObject.Display == true)
                    IncreasePostCommentNumber(ChangeStatusObject.F_Posts_Id ?? default(int));
                else
                    DecreasePostCommentNumber(ChangeStatusObject.F_Posts_Id ?? default(int));
                db.SaveChanges();
                return "OK";

            }
        }

        /// <summary>
        /// in tabe baraye list kardane tamame comment ha estefade mishavad
        /// Tabe Staticiye GetDateTimeReturnJalaliDate az kelase Tools dar in tabe farakhani shode ast
        /// </summary>
        /// <returns>
        /// List<PostCommentModel>
        /// </returns>
        public List<PostCommentModel> ListPostComment()
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var ListObject = db.PostsComments;
                List<PostCommentModel> list = new List<PostCommentModel>();
                foreach (var item in ListObject)
                {
                    PostCommentModel ListItem = new PostCommentModel();
                    ListItem.ID = item.ID;
                    ListItem.CreateOnUtc = item.CreatedOnUTC ?? default(DateTime);
                    ListItem.Display = item.Display ?? default(bool);
                    ListItem.F_Posts_Id = item.F_Posts_Id ?? default(int);
                    ListItem.NumberOfDislikes = item.NumberOfDislikes ?? default(int);
                    ListItem.NumberOfLikes = item.NumberOfLikes ?? default(int);
                    ListItem.NumberOfReply = item.NumberOfReply ?? default(int);
                    ListItem.CreateDateUtcJalali = Tools.GetDateTimeReturnJalaliDate(item.CreatedOnUTC ?? default(DateTime));
                    ListItem.Text = item.Text;
                    list.Add(ListItem);
                }
                return list;
            }
        }


        /// <summary>
        /// in tabe baraye list kardane tamame comment ha va reply haye mortabet ba poste morede nazar bekar miravad ()
        /// tabe SpecificListPostCommentReply dar in tabe farakhani mishavad
        /// </summary>
        /// <param name="PostId">idie poste morede nazar</param>
        /// <returns>
        /// List<PostCommentModel>
        /// </returns>
        public List<PostCommentModel> SpecificListPostComment(int PostId)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var ListObject = db.PostsComments.Where(u => u.F_Posts_Id == PostId && u.Display == true);
                List<PostCommentModel> list = new List<PostCommentModel>();
                PostCommentReplyManagement po = new PostCommentReplyManagement();
                foreach (var item in ListObject)
                {
                    PostCommentModel ListItem = new PostCommentModel();
                    ListItem.ID = item.ID;
                    ListItem.CreateOnUtc = item.CreatedOnUTC ?? default(DateTime);
                    ListItem.CreateDateUtcJalali = Tools.GetDateTimeReturnJalaliDate(item.CreatedOnUTC ?? default(DateTime));
                    ListItem.Display = item.Display ?? default(bool);
                    ListItem.F_Posts_Id = item.F_Posts_Id ?? default(int);
                    ListItem.NumberOfDislikes = item.NumberOfDislikes ?? default(int);
                    ListItem.NumberOfLikes = item.NumberOfLikes ?? default(int);
                    ListItem.NumberOfReply = item.NumberOfReply ?? default(int);
                    ListItem.Text = item.Text;
                    // baraye gereftane tamame reply haye marbut be commenthaye mojud dar halghe
                    ListItem.Replies = po.SpecificListPostCommentReply(item.ID);
                    list.Add(ListItem);
                }
                return list;
            }
        }
    }
}