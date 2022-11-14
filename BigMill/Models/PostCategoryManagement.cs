using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace BigMill.Models
{
    public class PostCategoryManagement
    {
        /// <summary>
        /// in tabe baraye ezafe kardane shakheye jadid estefade migardad
        /// tabe AddMidTable dar in tabe farakhani mishavad
        /// </summary>
        /// <param name="model">type="PostCategoryModel"</param>
        /// <returns>
        /// int 
        /// 1 : dar surati ke ba movaffaghiyat anjam gardad
        /// 2 : dar surati ke shakheye vared shode tekrari bashad
        /// </returns>
        public int AddPostCategory(PostCategoryModel model)
        {
            using (BigMill.Models.Entities db = new BigMill.Models.Entities())
            {
                var IsFound = db.PostsCategories.FirstOrDefault(u => u.Text == model.Text);
                if (IsFound == null)
                {
                    PostsCategory InsertObject = new PostsCategory();
                    InsertObject.Text = model.Text;
                    InsertObject.isView = model.IsView;
                    InsertObject.SeoName = model.SeoName;
                    InsertObject.Status = true;
                    InsertObject.Weight = model.Weight;
                    db.PostsCategories.Add(InsertObject);
                    db.SaveChanges();
                    //ezafe kardane tag haye vared shode baraye shakhe
                    AddMidTable(InsertObject.ID, model.TagsText.TrimStart('#').Split('#').ToList());
                    return 1;
                }
                return 2;
            }
        }

        /// <summary>
        /// in tabe baraye ezafe kardane tag haye marbut be shakhe ha estefade migardad
        /// </summary>
        /// <param name="F_PostsCategory_Id">type="int"</param>
        /// <param name="list">type="List<string>"</param>
        private void AddMidTable(int F_PostsCategory_Id, List<string> list)
        {
            using (BigMill.Models.Entities db = new BigMill.Models.Entities())
            {
                var Tags = db.Tags;
                foreach (var ListItem in list)
                {
                    var IsFound = Tags.FirstOrDefault(u => u.Text == ListItem);
                    if (IsFound == null)
                    {
                        Tag InsertTag = new Tag();
                        InsertTag.Text = ListItem.Trim();
                        InsertTag.CreatedOnUTC = DateTime.Now;
                        db.Tags.Add(InsertTag);
                        db.SaveChanges();
                        Tags_PostsCategory_Mapping InsertTagsPosts = new Tags_PostsCategory_Mapping();
                        InsertTagsPosts.F_PostsCategory_Id = F_PostsCategory_Id;
                        InsertTagsPosts.F_Tags_Id = InsertTag.ID;
                        db.Tags_PostsCategory_Mapping.Add(InsertTagsPosts);
                        db.SaveChanges();
                    }
                    else
                    {
                        Tags_PostsCategory_Mapping InsertTagsPosts = new Tags_PostsCategory_Mapping();
                        InsertTagsPosts.F_PostsCategory_Id = F_PostsCategory_Id;
                        InsertTagsPosts.F_Tags_Id = IsFound.ID;
                        db.Tags_PostsCategory_Mapping.Add(InsertTagsPosts);
                        db.SaveChanges();
                    }
                }
            }
        }

        /// <summary>
        /// in tabe baraye virayeshe shakheye morede nazar estefade migardad
        /// tavabe (DeleteMidTable , AddMidTable) dar in tabe farakhani mishavand
        /// </summary>
        /// <param name="model">type="PostCategoryModel"</param>
        /// <returns>
        /// string
        /// OK : agar ba movffaghiyyat anjam shavad
        /// NotFound : dar surati ke shakheye morede nazar vujud nadashte bashad
        /// </returns>
        public string EditPostCategory(PostCategoryModel model)
        {
            using (BigMill.Models.Entities db = new BigMill.Models.Entities())
            {
                var EditObject = db.PostsCategories.FirstOrDefault(u => u.ID == model.ID && u.Text == model.Text);
                if (EditObject == null || EditObject.Status == false)
                {
                    return "NotFound";
                }
                EditObject.Text = model.Text;
                EditObject.isView = model.IsView;
                EditObject.SeoName = model.SeoName;
                EditObject.Weight = model.Weight;
                db.SaveChanges();
                // kolle tag haye marbut be in shakhe ra pak mikonad (albatte dar table miyani)
                DeleteMidTable(model.ID);   
                // Tag haye marbut be shakhe ra az no vared mikonad
                AddMidTable(model.ID, model.TagsText.TrimStart('#').Split('#').ToList());
                return "OK";


            }
        }

        /// <summary>
        /// in tabe baraye pak kardane tag haye marbut be shakheye morede nazar dar table miyani estefade mishavad
        /// </summary>
        /// <param name="PostCategoryId">type="int"</param>
        private void DeleteMidTable(int PostCategoryId)
        {
            using (BigMill.Models.Entities db = new BigMill.Models.Entities())
            {
                var MidTableMembers = db.Tags_PostsCategory_Mapping.Where(u => u.F_PostsCategory_Id == PostCategoryId);
                foreach (var item in MidTableMembers)
                {
                    db.Tags_PostsCategory_Mapping.Remove(item);
                }
                db.SaveChanges();
            }
        }

        /// <summary>
        /// in tabe baraye taghire vaziyate shakheye morede nazar estefade migardad
        /// </summary>
        /// <param name="model">type="PostCategoryModel"</param>
        /// <returns>
        /// int 
        /// 1 : agar ba movaffaghiyat anjam shavad
        /// 2 : agar shakheye morede nazar mojud nabashad
        /// </returns>
        public int ChangeStatusPostCategory(PostCategoryModel model)
        {
            using (BigMill.Models.Entities db = new BigMill.Models.Entities())
            {
                var ChangeStatusObject = db.PostsCategories.FirstOrDefault(u => u.ID == model.ID);
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
        /// in tabe tamamiye shakhehaye mojud ke bar asase vazn morattab shodeand ra be hamrahe tag hayash barmigardanad (makhsuse admin)
        /// tabe GetTags dar in tabe farakhani mishavad
        /// </summary>
        /// <returns>
        /// List<PostCategoryModel>
        /// </returns>
        public List<PostCategoryModel> ListPostCategory()
        {
            using (BigMill.Models.Entities db = new BigMill.Models.Entities())
            {
                var ListObject = db.PostsCategories.Where(m => m.Status == true).OrderBy(t => t.Weight);
                List<PostCategoryModel> list = new List<PostCategoryModel>();
                foreach (var item in ListObject)
                {

                    PostCategoryModel ListItem = new PostCategoryModel();
                    ListItem.Text = item.Text;
                    ListItem.ID = item.ID;
                    ListItem.IsView = item.isView;
                    ListItem.SeoName = item.SeoName;
                    ListItem.Status = item.Status ?? default(bool);
                    ListItem.Weight = item.Weight ?? default(double);
                    // baraye gereftane tag haye marbut be shakhe
                    ListItem.TagsText = GetTags(item.ID);
                    list.Add(ListItem);
                }
                return list;
            }
        }

        /// <summary>
        /// in tabe baraye gereftane tag haye mortabet ba shakheye morede nazar estefade migardad
        /// </summary>
        /// <param name="PostCategoryId">type="int"</param>
        /// <returns>
        /// string (majmueye tag ha ke ba charactere '#' az hamdigar joda shodeand)
        /// </returns>
        public string GetTags(int PostCategoryId)
        {
            using (BigMill.Models.Entities db = new BigMill.Models.Entities())
            {
                string Result = "";
                //dar in query az EagerLoading estefade shode ast
                //baraye load kardane tamame tag haye shakhe ha
                var TagsObject = db.Tags_PostsCategory_Mapping.Include(y => y.Tag);
                //yaftane tag haye mortabet be shakheye morede nazar az beyne TagsObject ha
                var Tags = TagsObject.Where(u => u.F_PostsCategory_Id == PostCategoryId);
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