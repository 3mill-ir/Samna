using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace BigMill.Models
{
    public class MenuNames
    {
        public MenuNames()
        {
            Menu = new Collection<MenuModel>();
        }
        public Collection<MenuModel> Menu { get; set; }
    }

    public class MenuModel
    {


        public MenuModel()
        {
            SubMenu = new Collection<SubMenuModel>();
        }

        public string MenuName { get; set; }
        public string IsView { get; set; }
        public Collection<SubMenuModel> SubMenu { get; set; }
    }

    public class SubMenuModel
    {
        public string SubMenuName { get; set; }
        public string IsView { get; set; }
    }
    public class MenuGenerator
    {
        // dar koll chand Shekle IsView baraye category ha va sub category ha darim:
        // 1- ControllerName/MethodName
        // 2- HtmlFileName



        /// <summary>
        /// In tabe bar asase shakhe ha va zir shakhe hayi ke sharte (Status==true) darand listi az (Menu Models) ha ra tolid mikonad
        /// </summary>
        /// <returns>
        /// MenuNames
        /// </returns>
        public MenuNames GenerateMenu()
        {
            MenuNames Result = new MenuNames();
            using (BigMill.Models.Entities db = new Entities())
            {
                var DbMenus = db.PostsCategories.Include(t => t.PostsSubCategories).Where(u=>u.Status==true).OrderBy(r => r.Weight);
                foreach (var CategoryItem in DbMenus)
                {
                    MenuModel MenMod = new MenuModel();
                    MenMod.MenuName = CategoryItem.Text;
                    if (CategoryItem.isView != null)
                    {
                        MenMod.IsView = CategoryItem.isView;
                    }
                    foreach (var SubMenuItem in CategoryItem.PostsSubCategories)
                    {
                        SubMenuModel SubMenMod = new SubMenuModel();
                        SubMenMod.SubMenuName = SubMenuItem.Text;
                        if (SubMenuItem.isView != null)
                        {
                            SubMenMod.IsView = SubMenuItem.isView;
                        }
                        MenMod.SubMenu.Add(SubMenMod);
                    }
                    Result.Menu.Add(MenMod);
                }
            }
            return Result;
        }


        /// <summary>
        /// In Tabe bar asase shakhe ha va zir shakhe ha yek menu baraye dastgah haye androidi misazad ke be surate (Json) tahte onvane string baz migardad
        /// Jsone tarrahi shode shamele majmue i Object hayi mibashad ke har kodam field haye { MenuName , SubMenu(*) , IsView }
        /// (*) Khode SubMenu shamele field haye { SubMenuName , IsViewDetail , IsViewDetail }
        /// </summary>
        /// <InsideMethods>
        /// AcceptableSubCategory
        /// </InsideMethods>
        /// <returns>
        /// String(json)
        /// </returns>
        public string AndroidMenuGenerator()
        {
            string Helper = "";
            string Result = "{\"Menu\":[";
            using (BigMill.Models.Entities db = new Entities())
            {

                var DbMenus = db.PostsCategories.Include(t => t.PostsSubCategories).OrderBy(r => r.Weight);
                foreach (var CategoryItem in DbMenus)
                {
                    if (String.IsNullOrEmpty(CategoryItem.isView))
                    {
                        if (CategoryItem.PostsSubCategories.Count > 0)
                        {
                            // Check mikone bebine aya Subcategory haye Category haye feeli Motabar hastand ya na.
                            bool flag = AcceptableSubCategory(CategoryItem.PostsSubCategories.ToList());
                            if (flag == true)
                            {
                                Result += "{\"MenuName\":\"" + CategoryItem.Text + "\"";
                                Result += ",\"SubMenu\":[";
                                foreach (var SubMenuItem in CategoryItem.PostsSubCategories)
                                {
                                    if (!String.IsNullOrEmpty(SubMenuItem.isView))
                                    {
                                        if (SubMenuItem.isView.IndexOf("/") > 0)
                                        {
                                            //( System.Configuration.ConfigurationManager.AppSettings["WebSiteURL"] ) in tekke code url e Site ro barmigardune
                                            Helper = SubMenuItem.isView.Insert(SubMenuItem.isView.IndexOf('/')+1, "Android");
                                            Result += "{\"SubMenuName\":\"" + SubMenuItem.Text + "\",\"IsView\":\"" + System.Configuration.ConfigurationManager.AppSettings["WebSiteURL"] + Helper + "\",\"IsViewDetail\":\"" + System.Configuration.ConfigurationManager.AppSettings["WebSiteURL"] + Helper + "_Detail?PostId=" + "\"},";
                                        }
                                        else
                                            Result += ",";
                                    }
                                }
                                Result = Result.Remove(Result.LastIndexOf(','), 1);
                                Result += "],\"IsView\":\"\"},";
                            }
                        }
                    }
                    else
                    {
                        if (CategoryItem.isView.IndexOf("/") > 0)
                        {
                            if (CategoryItem.PostsSubCategories.Count > 0)
                            {
                                // Check mikone bebine aya Subcategory haye Category haye feeli Motabar hastand ya na.
                                bool flag = AcceptableSubCategory(CategoryItem.PostsSubCategories.ToList());
                                if (flag == true)
                                {
                                Result += "{\"MenuName\":\"" + CategoryItem.Text + "\"";
                                    Result += ",\"SubMenu\":[";
                                    foreach (var SubMenuItem in CategoryItem.PostsSubCategories)
                                    {
                                        if (!String.IsNullOrEmpty(SubMenuItem.isView))
                                        {
                                            if (SubMenuItem.isView.IndexOf("/") > 0)
                                            {
                                                //( System.Configuration.ConfigurationManager.AppSettings["WebSiteURL"] ) in tekke code url e Site ro barmigardune
                                                Helper = SubMenuItem.isView.Insert(SubMenuItem.isView.IndexOf('/')+1,"Android");
                                                Result += "{\"SubMenuName\":\"" + SubMenuItem.Text + "\",\"IsView\":\"" + System.Configuration.ConfigurationManager.AppSettings["WebSiteURL"] + Helper + "\",\"IsViewDetail\":\"" + System.Configuration.ConfigurationManager.AppSettings["WebSiteURL"] + Helper + "_Detail?PostId=" + "\"},";
                                            }
                                            else
                                                Result += ",";
                                        }
                                    }
                                    Result = Result.Remove(Result.LastIndexOf(','), 1);
                                    Helper = CategoryItem.isView.Insert(CategoryItem.isView.IndexOf('/')+1, "Android");
                                    Result += "],\"IsView\":\"" + System.Configuration.ConfigurationManager.AppSettings["WebSiteURL"] + Helper + "\"},";
                                }
                            }
                        }
                    }
                }
                Result = Result.Remove(Result.LastIndexOf(','), 1);
                Result += "]}";
            }
            return Result;
        }


        /// <summary>
        /// In Tabe check mikonad ke liste zirshakhe hayi ke be onvane argomane vurudi gerefte aya hamegi motabar hastand ya na (deghat shavad hamegi)
        /// </summary>
        /// <param name="list"></param>
        /// <returns>
        /// bool
        /// </returns>
        private bool AcceptableSubCategory(List<PostsSubCategory> list)
        {
            foreach (var item in list)
            {
                //baraye hameye zirshakhe ha check mikonad ke Isview (Shebhe link) gheyre null va hatman tahte forme (ControllerName/MethodName) bashad.
                // in kar ra ba barresi kardan in ke Isview harkodam bayad charactere '/' ra dashte bashand
                if (!String.IsNullOrEmpty(item.isView))
                {
                    if (item.isView.IndexOf("/") < 0)
                        return false;
                }
                else
                    return false;
            }
            return true;
        }
       
    }

}