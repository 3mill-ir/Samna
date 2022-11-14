using BigMill.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace BigMill.Controllers
{

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    

        /// <summary>
        /// in tabe liste shakhe ha va zir shakhe ha + linkhayeshan ra baraye sakhte menu dar (Android) be device baz migardanad
        /// </summary>
        /// <returns>
        /// string
        /// jsoni dar ghalebe string
        /// </returns>
        public string AndroidMenu()
        {
            BigMill.Models.MenuGenerator m = new BigMill.Models.MenuGenerator();
            return m.AndroidMenuGenerator();
        }


        /// <summary>
        /// in tabe name shahr ra gerefte sepas liste nemayande haye aan shahr ra ba tasviri az aanha va linke subdomaine aan ha ra be device (Android) baz migardanad
        /// </summary>
        /// <param name="AgentName">name shahre morede nazar</param>
        /// <returns>
        /// string
        /// Jsoni dar ghalebe string : agar nemayande haye aan shahr sabt shode bashand
        /// null : agar hich nemayandeye sabt shodeyi dar aan shahr vojud nadashte bashad
        /// </returns>
        public string AgentList(string AgentName)
        {
            switch(AgentName)
            {
                case "سردشت": { return "{\"Root\": [{\"Name\": \"دکتر رسول خضری\",\"Image\": \"http://drkhezri.samna.ir/Content/UserContent/images/AgentPic/DrKhezri.jpg\",\"Link\": \"http://drkhezri.samna.ir/\"}]}";}
                case "پیرانشهر": { return "{\"Root\": [{\"Name\": \"دکتر رسول خضری\",\"Image\": \"http://drkhezri.samna.ir/Content/UserContent/images/AgentPic/DrKhezri.jpg\",\"Link\": \"http://drkhezri.samna.ir/\"}]}"; }
                default:{return "null";}
        }
        }

        public string AndroidContactUsList(string AgentName)
        {
            //Flag==false yaani rahe ertebatiye mazkur link darad va daftare fizikiye nemayande nist
            //Flag==false yaani rahe ertebatiye mazkur darftare fizikiye nemayande mibashad
            //Text== noe rahe ertebati
            //Image == tasvir ya iconi az rahe ertebatiye morede nazar
            //Link== linke rahe ertebatiye morede nazar dar surate vojud
            //return "{\"Root\": [{\"Text\": \"کانال تلگرام دکتر خضری\",\"Image\": \"http://drkhezri.samna.ir/Content/UserContent/images/AgentPic/Telegram-icon.png\",\"Link\": \"https://telegram.me/dr_rasoul_khezri\",\"Flag\": \"False\"},{\"Text\": \"وب سایت رسمی دکتر خضری\",\"Image\": \"http://drkhezri.samna.ir/Content/UserContent/images/AgentPic/Web.png\",\"Link\": \"http://drkhezri.ir/DrRasoolKhezri.aspx\",\"Flag\": \"False\"},{\"Text\": \" تهران:مجلس شورای اسلامی، طبقه دوم، دفتر ارتباطات مردمی دکتر رسول خضری، تلفکس: 02133121937 \",\"Image\": \"http://drkhezri.samna.ir/Content/UserContent/images/AgentPic/Department.png\",\"Link\": \"\",\"Flag\": \"True\"},{\"Text\": \"پیرانشهر:خیابان پزشک، روبروی ساختمان کمیته امداد امام خمینی(ره)، تلفکس: 04444229140 \",\"Image\": \"http://drkhezri.samna.ir/Content/UserContent/images/AgentPic/Department.png\",\"Link\": \"\",\"Flag\": \"True\"},{\"Text\": \"سردشت:ساختمان جهاد کشاورزی- طبقه همکف- تلفکس: 04444323103\",\"Image\": \"http://drkhezri.samna.ir/Content/UserContent/images/AgentPic/Department.png\",\"Link\": \"\",\"Flag\": \"True\"},{\"Text\": \"ربط:آدرس:محل اداری شورای اسلامی شهر ربط واقع در خیابان امام (ره)\",\"Image\": \"http://drkhezri.samna.ir/Content/UserContent/images/AgentPic/Department.png\",\"Link\": \"\",\"Flag\": \"True\"}]}";

            return "{\"Root\": [{\"Text\": \"کانال تلگرام نسخه نمایشی\",\"Image\": \"http://demo.samna.ir/Content/UserContent/images/AgentPic/Telegram-icon.png\",\"Link\": \"https://telegram.me/demosamna\",\"Flag\": \"False\"},{\"Text\": \"نسخه نمایشی وب سایت\",\"Image\": \"http://demo.samna.ir/Content/UserContent/images/AgentPic/Web.png\",\"Link\": \"http://demo.samna.ir/\",\"Flag\": \"False\"},{\"Text\": \" آدرس دفتر ارتباطات مردمی \",\"Image\": \"http://demo.samna.ir/Content/UserContent/images/AgentPic/Department.png\",\"Link\": \"\",\"Flag\": \"True\"},{\"Text\": \"آدرس دفتر ارتباطات مردمی 2 \",\"Image\": \"http://demo.samna.ir/Content/UserContent/images/AgentPic/Department.png\",\"Link\": \"\",\"Flag\": \"True\"},{\"Text\": \"آدرس دفتر ارتباطات مردمی 3\",\"Image\": \"http://demo.samna.ir/Content/UserContent/images/AgentPic/Department.png\",\"Link\": \"\",\"Flag\": \"True\"},{\"Text\": \"آدرس دفتر ارتباطات مردمی 4)\",\"Image\": \"http://demo.samna.ir/Content/UserContent/images/AgentPic/Department.png\",\"Link\": \"\",\"Flag\": \"True\"}]}";
        }


    

    }
}