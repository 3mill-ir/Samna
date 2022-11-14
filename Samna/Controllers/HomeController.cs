using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Samna.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


      

        public string AgentList(string AgentName)
        {
            switch (AgentName)
            {
                case "سردشت": { return "{\"Root\": [{\"Name\": \"دکتر رسول خضری\",\"Image\": \"http://drkhezri.samna.ir/Content/UserContent/images/AgentPic/DrKhezri.jpg\",\"Link\": \"http://drkhezri.samna.ir/\"}]}"; }
                case "پیرانشهر": { return "{\"Root\": [{\"Name\": \"دکتر رسول خضری\",\"Image\": \"http://drkhezri.samna.ir/Content/UserContent/images/AgentPic/DrKhezri.jpg\",\"Link\": \"http://drkhezri.samna.ir/\"}]}"; }
                case "تهران": { return "{\"Root\": [{\"Name\": \"اکانت نمایشی\",\"Image\": \"http://demo.samna.ir/Content/UserContent/images/AgentPic/demo.jpg\",\"Link\": \"http://demo.samna.ir/\"}]}"; }
                case "آستانه اشرفیه": { return "{\"Root\": [{\"Name\": \"دکتر حسین قربانی\",\"Image\": \"http://drghorbani.samna.ir/Content/UserContent/images/AgentPic/drghorbani.jpg\",\"Link\": \"http://drghorbani.samna.ir/\"}]}"; }
                default: { return "null"; }
            }
        }

    }
}