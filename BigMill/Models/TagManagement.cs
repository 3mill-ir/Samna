using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace BigMill.Models
{
    public class TagManagement
    {

        /// <summary>
        /// in tabe jahate ezafe kardane tag bekar miravad
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// int
        /// 1 : agar ba movaffaghiyyat afzude shavad
        /// 2 : agar in tag ghablan sabt shode bashad
        /// </returns>
        public int AddTag(TagModel model)
        {
            using(BigMill.Models.Entities db=new Entities())
            {
                var IsFound = db.Tags.FirstOrDefault(u=>u.Text==model.Text);
                if (IsFound == null)
                {
                    Tag InsertObject = new Tag();
                    InsertObject.Text = model.Text;
                    InsertObject.CreatedOnUTC = DateTime.UtcNow;
                    db.Tags.Add(InsertObject);
                    db.SaveChanges();
                    return 1;
                }
                else
                    return 2;
            }
        }

        /// <summary>
        /// in tabe jahate list kardane tamamiye tag haye mojud 
        /// </summary>
        /// <returns>
        /// List<TagModel>
        /// </returns>
        public List<TagModel> ListTag()
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                List<TagModel> list = new List<TagModel>();
                var ListObject = db.Tags;
                foreach (var item in ListObject)
                {
                    TagModel ListItem = new TagModel();
                    ListItem.Text = item.Text;
                    ListItem.ID = item.ID;
                    list.Add(ListItem);
                }
                return list;
            }
        }


        /// <summary>
        /// in tabe jahate list kardane tamamiye post hayi ke taghaye aan ha shamele tage morede nazar ast bekar borde mishavad
        /// tavabe (GetPostSubCategoryTagReturnListPost , GetPostCategoryTagReturnListPost) dar in tabe farakhani mishavand
        /// </summary>
        /// <param name="TagId">idie tage morede nazar</param>
        /// <returns>
        /// List<PostModel>
        /// </returns>
        public List<PostModel> GetPostTagReturnListPost(int TagId)
        {
            using (BigMill.Models.Entities db = new BigMill.Models.Entities())
            {
                List<PostModel> Result = new List<PostModel>();
                var Posts = db.Tags_Posts_Mapping.Include(y => y.Post).Where(u => u.F_Tags_Id == TagId);
                foreach (var item in Posts)
                {
                    PostModel ListItem = new PostModel();
                    ListItem.Content_One = System.Text.RegularExpressions.Regex.Replace(item.Post.Content_One, @"[^\w\040@-]", "",
                                System.Text.RegularExpressions.RegexOptions.None);;
                    ListItem.Content_Two = item.Post.Content_Two;
                    ListItem.Content_Three = item.Post.Content_Three;
                    ListItem.Content_Four = item.Post.Content_Four;
                    ListItem.ID = item.Post.ID;
                    ListItem.Display = item.Post.Display ?? default(bool);
                    ListItem.F_PostsSubCategory_ID = item.Post.F_PostsSubCategory_Id ?? default(int);
                    ListItem.F_Users_Id = item.Post.F_User_Id ?? default(int);
                    ListItem.NumberOfComments = item.Post.NumberOfComments ?? default(int);
                    ListItem.NumberOfDisLikes = item.Post.NumberOfDislikes ?? default(int);
                    ListItem.NumberOfLikes = item.Post.NumberOfLikes ?? default(int);
                    ListItem.NumberOfVisitors = item.Post.NumberOfVisitors ?? default(int);
                    ListItem.Status = item.Post.Status ?? default(bool);
                    ListItem.CreateDateUtc = item.Post.CreatedOnUTC ?? default(DateTime);
                    Result.Add(ListItem);
                }
                Result.AddRange(GetPostSubCategoryTagReturnListPost(TagId));
                Result.AddRange(GetPostCategoryTagReturnListPost(TagId));
                Result = Result.GroupBy(y => y.ID).Select(u=>u.First()).ToList();
                return Result;
            }
        }

        /// <summary>
        /// in tabe jahate list kardane post hayi ke jozve zirshakhe haye shamele tage morede nazar hastand bekar miravad
        /// </summary>
        /// <param name="TagId">idie tage morede nazar</param>
        /// <returns>
        /// List<PostModel>
        /// </returns>
        public List<PostModel> GetPostSubCategoryTagReturnListPost(int TagId)
        {
            using (BigMill.Models.Entities db = new BigMill.Models.Entities())
            {
                List<PostModel> Result = new List<PostModel>();
                var PostsSubCategory = db.Tags_PostsSubCategory_Mapping.Include(y => y.PostsSubCategory).Where(u => u.F_Tags_Id == TagId);
                foreach (var item in PostsSubCategory)
                {
                    var Posts = db.Posts.Where(u => u.F_PostsSubCategory_Id == item.PostsSubCategory.ID && u.Display == true && u.Status == true).OrderByDescending(t => t.CreatedOnUTC);
                    foreach (var PostItem in Posts)
                    {
                        PostModel ListItem = new PostModel();
                        ListItem.Content_One = PostItem.Content_One;
                        ListItem.Content_Two = PostItem.Content_Two;
                        ListItem.Content_Three = PostItem.Content_Three;
                        ListItem.Content_Four = PostItem.Content_Four;
                        ListItem.ID = PostItem.ID;
                        ListItem.Display = PostItem.Display ?? default(bool);
                        ListItem.F_PostsSubCategory_ID = PostItem.F_PostsSubCategory_Id ?? default(int);
                        ListItem.F_Users_Id = PostItem.F_User_Id ?? default(int);
                        ListItem.NumberOfComments = PostItem.NumberOfComments ?? default(int);
                        ListItem.NumberOfDisLikes = PostItem.NumberOfDislikes ?? default(int);
                        ListItem.NumberOfLikes = PostItem.NumberOfLikes ?? default(int);
                        ListItem.NumberOfVisitors = PostItem.NumberOfVisitors ?? default(int);
                        ListItem.Status = PostItem.Status ?? default(bool);
                        ListItem.CreateDateUtc = PostItem.CreatedOnUTC ?? default(DateTime);
                        Result.Add(ListItem);
                    }
                }
                return Result;
            }
        }

        /// <summary>
        /// in tabe jahate list kardane post hayi ke jozve shakhe haye shamele tage morede nazar hastand bekar miravad 
        /// </summary>
        /// <param name="TagId">idie tage morede nazar</param>
        /// <returns>
        /// List<PostModel>
        /// </returns>
        public List<PostModel> GetPostCategoryTagReturnListPost(int TagId)
        {
            using (BigMill.Models.Entities db = new BigMill.Models.Entities())
            {
                List<PostModel> Result = new List<PostModel>();
                var PostsCategory = db.Tags_PostsCategory_Mapping.Include(y => y.PostsCategory).Where(u => u.F_Tags_Id == TagId);
                foreach (var cat in PostsCategory)
                {
                    foreach (var subcat in cat.PostsCategory.PostsSubCategories)
                    {
                        var Posts = db.Posts.Where(u => u.F_PostsSubCategory_Id == subcat.ID && u.Display == true && u.Status == true).OrderByDescending(t => t.CreatedOnUTC);
                        foreach (var PostItem in Posts)
                        {
                            PostModel ListItem = new PostModel();
                            ListItem.Content_One = PostItem.Content_One;
                            ListItem.Content_Two = PostItem.Content_Two;
                            ListItem.Content_Three = PostItem.Content_Three;
                            ListItem.Content_Four = PostItem.Content_Four;
                            ListItem.ID = PostItem.ID;
                            ListItem.Display = PostItem.Display ?? default(bool);
                            ListItem.F_PostsSubCategory_ID = PostItem.F_PostsSubCategory_Id ?? default(int);
                            ListItem.F_Users_Id = PostItem.F_User_Id ?? default(int);
                            ListItem.NumberOfComments = PostItem.NumberOfComments ?? default(int);
                            ListItem.NumberOfDisLikes = PostItem.NumberOfDislikes ?? default(int);
                            ListItem.NumberOfLikes = PostItem.NumberOfLikes ?? default(int);
                            ListItem.NumberOfVisitors = PostItem.NumberOfVisitors ?? default(int);
                            ListItem.Status = PostItem.Status ?? default(bool);
                            ListItem.CreateDateUtc = PostItem.CreatedOnUTC ?? default(DateTime);
                            Result.Add(ListItem);
                        }
                    }
                }
                return Result;
            }
        }
    }
}