using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Samna.Controllers
{
    public class AppController : Controller
    {
        //
        // GET: /App/
        public FileResult Index()
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes(Server.MapPath("~/App_Data/Android/samna.apk"));
            string fileName = "samna.apk";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
	}
}