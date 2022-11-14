using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace BigMill.Models
{
    public class PostSubCategoryManagement
    {
        /// <summary>
        /// in tabe jahate ezafe kardane zirshakheyi jadid morede estefade gharar migirad
        /// tabe (AddMidTable) dar in tabe farakhani mikonad
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// string
        /// NotFound : dar surati ke shakheye asliye entekhabi baraye in zirshakhe mojud nabashad
        /// Repeated : dar surati ke in zirshakhe ghablan sabt shode bashad
        /// OK : dar surati ke sabt ba movaffaghiyat surat girad
        /// </returns>
        public string AddPostSubCategory(PostSubCategoryModel model)
        {
            using(BigMill.Models.Entities db=new BigMill.Models.Entities())
            {
                var IsFound=db.PostsSubCategories.FirstOrDefault(u=>u.Text==model.Text);
                if (db.PostsCategories.FirstOrDefault(u => u.ID == model.F_PostCategory_ID) == null)
                {
                    return "NotFound";
                }
                if(IsFound==null)
                {
                    PostsSubCategory InsertObject = new PostsSubCategory();
                    InsertObject.Text = model.Text;
                    InsertObject.F_PostsCategory_Id = model.F_PostCategory_ID;
                    InsertObject.isView = model.IsView;
                    InsertObject.SeoName = model.SeoName;
                    InsertObject.Status = true;
                    InsertObject.Weight = model.Weight;
                    db.PostsSubCategories.Add(InsertObject);
                    db.SaveChanges();
                    // jahate afzudane tag haye zirshakheye morede nazar bekar miravad
                    AddMidTable(InsertObject.ID, model.TagsText.TrimStart('#').Split('#').ToList());
                    return "OK";
                }
                return "Repeated";
            }
        }

        /// <summary>
        /// in tabe jahate afzudane tag haye mortabet ba zirshakheye morede nazar bekar miravad
        /// table e miyani ra niz takmil mikonad
        /// </summary>
        /// <param name="F_PostsSubCategory_Id">idie zirshakheye morede nazar</param>
        /// <param name="list"></param>
        private void AddMidTable(int F_PostsSubCategory_Id, List<string> list)
        {
            using (BigMill.Models.Entities db = new BigMill.Models.Entities())
            {
                var Tags = db.Tags;
                foreach (var ListItem in list)
                {
                    var IsFound = Tags.FirstOrDefault(u => u.Text == ListItem);
                    // jahate check kardane inke tag ghablan sabt shode ya na
                    if (IsFound == null)
                    {
                        Tag InsertTag = new Tag();
                        InsertTag.Text = ListItem;
                        InsertTag.CreatedOnUTC = DateTime.Now;
                        db.Tags.Add(InsertTag);
                        db.SaveChanges();
                        Tags_PostsSubCategory_Mapping InsertTagsPostsSub = new Tags_PostsSubCategory_Mapping();
                        InsertTagsPostsSub.F_PostsSubCategory_Id = F_PostsSubCategory_Id;
                        InsertTagsPostsSub.F_Tags_Id = InsertTag.ID;
                        db.Tags_PostsSubCategory_Mapping.Add(InsertTagsPostsSub);
                        db.SaveChanges();
                    }
                    else
                    {
                        Tags_PostsSubCategory_Mapping InsertTagsPostsSub = new Tags_PostsSubCategory_Mapping();
                        InsertTagsPostsSub.F_PostsSubCategory_Id = F_PostsSubCategory_Id;
                        InsertTagsPostsSub.F_Tags_Id = IsFound.ID;
                        db.Tags_PostsSubCategory_Mapping.Add(InsertTagsPostsSub);
                        db.SaveChanges();
                    }
                }
            }
        }


        /// <summary>
        /// in tabe jahate virayeshe zirshakheye morede nazar morede estefade gharar migirad
        /// Tavabe (DeleteMidTable , AddMidTable) dar in tabe farakhani migardad
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// string
        /// NotFound : dar surati ke zirshakheye morede nazar vojud nadashte bashad
        /// OK : dar surati ke virayesh ba movaffaghiyat surat girad
        /// </returns>
        public string EditPostSubCategory(PostSubCategoryModel model)
        {
            using (BigMill.Models.Entities db = new BigMill.Models.Entities())
            {
                var EditObject = db.PostsSubCategories.FirstOrDefault(u => u.ID == model.ID);
                if (EditObject == null || EditObject.Status == false) { return "NotFound"; }

                    EditObject.Text = model.Text;
                    EditObject.isView = model.IsView;
                    EditObject.SeoName = model.SeoName;
                    EditObject.Weight = model.Weight;
                    db.SaveChanges();
                    DeleteMidTable(model.ID);
                    AddMidTable(model.ID, model.TagsText.TrimStart('#').Split('#').ToList());
                    return "OK";
            }
        }

        /// <summary>
        /// in tabe jahate pak kardane tag haye zirshakheye moreden nazar morede estefade gharar migirad
        /// </summary>
        /// <param name="PostSubCategoryId">idie zirshakheye morede nazar</param>
        private void DeleteMidTable(int PostSubCategoryId)
        {
            using (BigMill.Models.Entities db = new BigMill.Models.Entities())
            {
                var MidTableMembers = db.Tags_PostsSubCategory_Mapping.Where(u => u.F_PostsSubCategory_Id == PostSubCategoryId);
                foreach (var item in MidTableMembers)
                {
                    db.Tags_PostsSubCategory_Mapping.Remove(item);
                }
                db.SaveChanges();
            }
        }


        /// <summary>
        /// in tabe jahate taghir vaziyate zirshakheye morede nazar morede estefade gharar migirad
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// int
        /// 1 : dar surate movaffaghiyyat dar taghirvaziyat
        /// 2 : dar surati ke zirshakheye morede nazar vojud nadashte bashad
        /// </returns>
        public int ChangeStatusPostSubCategory(PostSubCategoryModel model)
        {
            using (BigMill.Models.Entities db = new BigMill.Models.Entities())
            {
                var ChangeStatusObject = db.PostsSubCategories.FirstOrDefault(u => u.ID == model.ID);
                if (ChangeStatusObject != null)
                {
                    ChangeStatusObject.Status = !ChangeStatusObject.Status;
                    db.SaveChanges();
                    return 1;
                }
                return 2;
            }
        }

        /// <summary>
        /// in tabe jahate ettela az vojude shakheye morede nazar morede estefade gharar migirad
        /// </summary>
        /// <param name="PostCategoryId">idie shakheye morede nazar</param>
        /// <returns>
        /// bool
        /// true : agar shakheye morede nazar yaft shavad
        /// false : agar shakheye morede nazar yaft nashavad
        /// </returns>
        public bool isPostCategoryValid(int PostCategoryId){
            using (BigMill.Models.Entities db = new BigMill.Models.Entities())
            {
                var temp = db.PostsCategories.FirstOrDefault(u => u.ID == PostCategoryId);
                if (temp == null)
                {
                    return false;
                }
                return true;
            }
        }


        /// <summary>
        /// in tabe jahate list kardane zirshakhe haye 
        /// tabe (GetTags) dar in tabe farakhani shode ast
        /// </summary>
        /// <param name="F_PostCategory_Id">idie shakheye morede nazar</param>
        /// <returns>
        /// List<PostSubCategoryModel>
        /// </returns>
        public List<PostSubCategoryModel> ListPostSubCategory(int F_PostCategory_Id)
        {
            using (BigMill.Models.Entities db = new BigMill.Models.Entities())
            {
                var ListObject = db.PostsSubCategories.Where(u => u.F_PostsCategory_Id == F_PostCategory_Id && u.Status == true).OrderBy(u => u.Weight);
                List<PostSubCategoryModel> list = new List<PostSubCategoryModel>();
                foreach (var item in ListObject)
                {
                    PostSubCategoryModel ListItem = new PostSubCategoryModel();
                    ListItem.Text = item.Text;
                    ListItem.ID = item.ID;
                    ListItem.SeoName = item.SeoName;
                    ListItem.IsView = item.isView;
                    ListItem.TagsText = GetTags(item.ID);
                    ListItem.F_PostCategory_ID = item.F_PostsCategory_Id ?? default(int);
                    ListItem.Status = item.Status ?? default(bool);
                    ListItem.Weight = item.Weight ?? default(double);
                    list.Add(ListItem);
                }
                return list;
            }
        }


        /// <summary>
        /// in tabe jahate bazgardandane tamame tag haye marbut be in zirshakhe morede estefade gharar migirad
        /// </summary>
        /// <param name="PostSubCategoryId">idie zirshakheye morede nazar</param>
        /// <returns>
        /// string
        /// liste tag haye marbut be zirshakheye morede nazar ke ba charactere '#' az ham digar joda shode and
        /// </returns>
        public string GetTags(int PostSubCategoryId)
        {
            using (BigMill.Models.Entities db = new BigMill.Models.Entities())
            {
                string Result = "";
                var TagsObject = db.Tags_PostsSubCategory_Mapping.Include(y => y.Tag);
                var Tags = TagsObject.Where(u => u.F_PostsSubCategory_Id == PostSubCategoryId);
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
    }
}