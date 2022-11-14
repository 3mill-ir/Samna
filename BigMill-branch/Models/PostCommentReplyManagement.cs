using BigMill.Areas.Admin3mill.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BigMill.Models
{
    public class PostCommentReplyManagement
    {
        /// <summary>
        /// in tabe reply vared shode ra be commente morede nazar ezafe mikonad
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// int (1)
        /// </returns>
        public int AddPostCommentReply(PostCommentReplyModel model)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                PostsCommentsReply InsertObject = new PostsCommentsReply();
                InsertObject.CreatedOnUTC = DateTime.Now;
                InsertObject.Display = false;
                InsertObject.F_PostsComments_Id = model.F_PostsComments_Id;
                InsertObject.NumberOfDislikes = 0;
                InsertObject.NumberOfLikes = 0;
                InsertObject.Text = model.Text;
                db.PostsCommentsReplies.Add(InsertObject);
                db.SaveChanges();
                return 1;
            }
        }

        /// <summary>
        /// in tabe tedade reply haye yek comment ra afzayesh midahad
        /// </summary>
        /// <param name="CommentId">idie commente morede nazar</param>
        public void IncreaseCommentReplyNumber(int CommentId)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var parent = db.PostsComments.FirstOrDefault(u => u.ID == CommentId);
                parent.NumberOfReply++;
                db.SaveChanges();
            }
        }
        /// <summary>
        /// in tabe tedade reply haye yek comment ra Kahesh midahad
        /// </summary>
        /// <param name="CommentId">idie commente morede nazar</param>
        public void DecreaseCommentReplyNumber(int CommentId)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var parent = db.PostsComments.FirstOrDefault(u => u.ID == CommentId);
                parent.NumberOfReply--;
                db.SaveChanges();
            }
        }


        /// <summary>
        /// in tabe replye morede nazar ra hazf mikonad
        /// </summary>
        /// <param name="CommentReplyId">idie replye morede nazar</param>
        /// <returns>
        /// string
        /// OK : dar surati ke ba movaffaghiyat anjam shavad
        /// NotFound : dar surati ke reply morede nazar mojud nabashad
        /// </returns>
        public string PermanentlyDeletePostCommentReply(int CommentReplyId)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var DeleteObject = db.PostsCommentsReplies.FirstOrDefault(u => u.ID == CommentReplyId);
                if (DeleteObject == null || DeleteObject.Display == true)
                {
                    return "NotFound";
                }
                    db.PostsCommentsReplies.Remove(DeleteObject);
                    db.SaveChanges();
                    return "OK";
            }
        }
        /// <summary>
        /// in tabe tamamiye reply haye marbut be commente morede nazar ra pak mikonad
        /// </summary>
        /// <param name="F_PostsComments_Id">idie commente morede nazar</param>
        public void PermanentlyDeleteCasacadePostCommentReply(int F_PostsComments_Id)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var DeleteObject = db.PostsCommentsReplies.Where(u => u.F_PostsComments_Id == F_PostsComments_Id);
                foreach (var item in DeleteObject)
                {
                    db.PostsCommentsReplies.Remove(item);
                }
                db.SaveChanges();
            }
        }

        /// <summary>
        /// in tabe tedade like haye replye morede nazar ra afzayesh midahad
        /// </summary>
        /// <param name="commentReplyId">idie commente morede nazar</param>
        /// <param name="flag">flag darsurati ke ghablan like shode bashad false dar gheyre in surat true khahad bud</param>
        /// <returns>
        /// PostsCommentsReply
        /// IsFound : darsurati ke replye morede nazar yaft shavad 
        /// null : darsurati ke replye morede nazar yaft nashavad
        /// </returns>
        public PostsCommentsReply LikePostCommentReply(int commentReplyId, bool flag)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var IsFound = db.PostsCommentsReplies.FirstOrDefault(u => u.ID == commentReplyId);
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
        /// in tabe tedade DisLike haye replye morede nazar ra afzayesh midahad
        /// </summary>
        /// <param name="commentReplyId">idie commente morede nazar</param>
        /// <param name="flag">flag darsurati ke ghablan DisLike shode bashad false dar gheyre in surat true khahad bud</param>
        /// <returns>
        /// PostsCommentsReply
        /// IsFound : darsurati ke replye morede nazar yaft shavad 
        /// null : darsurati ke replye morede nazar yaft nashavad
        /// </returns>
        public PostsCommentsReply DisLikePostCommentReply(int commentReplyId, bool flag)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var IsFound = db.PostsCommentsReplies.FirstOrDefault(u => u.ID == commentReplyId);
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
        /// in tabe baraye taghir vaziyate reply morede estefade gharar migirad
        /// </summary>
        /// <param name="CommentReplyID">idie replye morede nazar</param>
        /// <returns>
        /// string
        /// OK : darsurati ke taghir vaziyat ba movaffaghiyat surat girad 
        /// NotFound : darsurati ke replye morede nazar yaft nashavad
        /// </returns>
        public string ChangeStatusPostCommentReply(int CommentReplyID)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var ChangeStatusObject = db.PostsCommentsReplies.FirstOrDefault(u => u.ID == CommentReplyID);
                if (ChangeStatusObject == null)
                {
                    return "NotFound";
                }

                ChangeStatusObject.Display = !ChangeStatusObject.Display;
                if (ChangeStatusObject.Display == true)
                    IncreaseCommentReplyNumber(ChangeStatusObject.F_PostsComments_Id ?? default(int));
                else
                    DecreaseCommentReplyNumber(ChangeStatusObject.F_PostsComments_Id ?? default(int));
                db.SaveChanges();
                return "OK";

            }
        }


        /// <summary>
        /// in tabe baraye list kardane tamame reply haye mojud morede estefade gharar migirad
        /// </summary>
        /// <returns>
        /// List<PostCommentReplyModel>
        /// </returns>
        public List<PostCommentReplyModel> ListPostCommentReply()
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var ListObject = db.PostsCommentsReplies;
                List<PostCommentReplyModel> list = new List<PostCommentReplyModel>();
                foreach (var item in ListObject)
                {
                    PostCommentReplyModel ListItem = new PostCommentReplyModel();
                    ListItem.ID = item.ID;
                    ListItem.CreateOnUTC = item.CreatedOnUTC ?? default(DateTime);
                    ListItem.Display = item.Display ?? default(bool);
                    ListItem.F_PostsComments_Id = item.F_PostsComments_Id ?? default(int);
                    ListItem.NumberOfDislikes = item.NumberOfDislikes ?? default(int);
                    ListItem.NumberOfLikes = item.NumberOfLikes ?? default(int);
                    ListItem.Text = item.Text;
                    list.Add(ListItem);
                }
                return list;
            }
        }


        /// <summary>
        /// in tabe baraye list kardane reply haye mortabet ba 
        /// </summary>
        /// <param name="PostCommentId">idie commente morede nazar</param>
        /// <returns>
        /// List<PostCommentReplyModel>
        /// </returns>
        public List<PostCommentReplyModel> SpecificListPostCommentReply(int PostCommentId)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var ListObject = db.PostsCommentsReplies.Where(u => u.F_PostsComments_Id == PostCommentId && u.Display == true);
                List<PostCommentReplyModel> list = new List<PostCommentReplyModel>();
                foreach (var item in ListObject)
                {
                    PostCommentReplyModel ListItem = new PostCommentReplyModel();
                    ListItem.ID = item.ID;
                    ListItem.CreateOnUTC = item.CreatedOnUTC ?? default(DateTime);
                    ListItem.CreateDateUtcJalali = Tools.GetDateTimeReturnJalaliDate(item.CreatedOnUTC ?? default(DateTime));
                    ListItem.Display = item.Display ?? default(bool);
                    ListItem.F_PostsComments_Id = item.F_PostsComments_Id ?? default(int);
                    ListItem.NumberOfDislikes = item.NumberOfDislikes ?? default(int);
                    ListItem.NumberOfLikes = item.NumberOfLikes ?? default(int);
                    ListItem.Text = item.Text;
                    list.Add(ListItem);
                }
                return list;
            }
        }
    }
}