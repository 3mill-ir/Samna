using BigMill.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Data.Entity;

namespace BigMill.Controllers
{
    public class SearchController : Controller
    {
        /// <summary>
        /// in controller jahate search kardane post hayi ke ya titr ya kholase ya mohtavaye aan ha shamele reshteye morede nazar mibashad
        /// dar in controller az (pagedList) estefade shode ast
        /// </summary>
        /// <param name="SearchBox">reshteye morede nazar</param>
        /// <param name="page">shomareye safheye feeliye paging</param>
        /// <returns>
        /// ActionResult : View(SearchResult)
        /// </returns>
        /// 
        /// Viewe (AkhbarOmumi) az partial view haye :
        /// 1- InformationBar
        /// ///////////////////////// estefade mikonad.
        public ActionResult SearchResult(string SearchBox, int? page)
        {
            @ViewBag.SearchBox = SearchBox;
            PostManagement post = new PostManagement();
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            int total;
            var pagedList = new StaticPagedList<PostModel>(post.SearchResult(SearchBox,pageNumber, pageSize, out total), pageNumber, pageSize, total);
            return View(pagedList);
        }
    }
}