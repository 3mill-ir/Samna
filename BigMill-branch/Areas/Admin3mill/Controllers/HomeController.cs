using BigMill.Areas.Admin3mill.Models;
using BigMill.CustomFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BigMill.Areas.Admin3mill.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// in controller jahate generate kardane view safheye dashboard(safheye asli)e admin morede estefade gharar migirad
        /// </summary>
        /// <returns>
        /// ActionResult : View(Index)
        /// </returns>
        [AuthLog]
        public ActionResult Index()
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = @Resource.Resource.PageTittle_Dashboard;
            //************ End Page Tittle *********************************
            return View();
        }
    }
}