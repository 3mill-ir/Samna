using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BigMill.Models;

namespace BigMill.Controllers
{
    public class StaticController : Controller
    {

        /// <summary>
        /// in controller jahate nemayeshe file haye staticiye html bekar miravad
        /// bedin surat ke name file html i ke dar masire tayin shode vojud darad ra be onvane argomane vurudi be aan midahim va aan file ra khande va mohtavaye aan ra be viewe khaliye feeli append mikonad
        /// </summary>
        /// <param name="FileName">name file html</param>
        /// <returns>
        /// ActionResult : View(StaticView)
        /// </returns>
        public ActionResult StaticView(string FileName)
        {
            StaticModel model=new StaticModel();
            string filename = "";
            if (FileName.IndexOf(".html")>-1)
                filename += FileName;
            else
                filename += FileName + ".html";
            model.HtmlContent = System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/Views/Static/" + filename));
            return View(model);
        }
    }
}