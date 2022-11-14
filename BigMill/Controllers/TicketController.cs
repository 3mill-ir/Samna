using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BigMill.Models;
using System.IO;

namespace BigMill.Controllers
{
    public class TicketController : Controller
    {

        /// <summary>
        /// in controller jahate nemayeshe viewe ersale darkhast morede estefade gharar migirad
        /// </summary>
        /// <returns>
        /// ActionResult : View(AddTicket)
        /// </returns>
        public ActionResult AddTicket()
        {
            return View();
        }
        public ActionResult AddTicketPost()
        {
            return PartialView();
        }
        /// <summary>
        /// in controller jahate ersale darkhast tavassote karbar morede estefade gharar migirad(post)
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// ActionResult : PartialView(AddTicketPost)
        /// </returns>
        [HttpPost]
        public ActionResult AddTicketPost(UserTicketModel model)
        {
            for (int i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];
            }
            TicketManagement Tickets = new TicketManagement();
            var allowedExtensions = new[] { ".doc", ".xlsx", ".txt", ".pdf", ".ppt", ".gif", ".jpg", ".jpeg", ".bmp", ".png", ".amr", ".mp3", ".amr", ".mp4", ".3gp", ".mov", ".mpeg" };
            bool flag = false;
            for (int i = 0; i < model.MediaBox.Count; i++)
            {
                var extension = Path.GetExtension(model.MediaBox[i].FileName).ToLower();
                if (!allowedExtensions.Contains(extension))
                {
                    flag = true;
                    ViewBag.TicketError += " - " + " قالب فایل" + " " + (i + 1) + " " + " نامعتبر می باشد";
                }
            }
            if (flag == true)
            {
                ViewBag.TicketError = "فرمت فایل انتخابی نادرست می باشد.";
                return PartialView(model);
            }
            if (String.IsNullOrEmpty(model.Content))
            {
                ViewBag.TicketWarning = "لطفا برای ارسال درخواست فیلد متن درخواست را کامل کنید";
                return PartialView(model);
            }
            if (!string.IsNullOrEmpty(model.TrackingCode))
            {
                using (BigMill.Models.Entities db = new Entities())
                {
           
                    var IsTicketExists = db.Tickets.FirstOrDefault(u => u.TrackingCode == model.TrackingCode);
                    if (IsTicketExists == null)
                    {
                        ViewBag.TicketError = "کد پیگیری وارد شده صحیح نمی باشد";
                        return PartialView(model);
                    }
                    else
                    {
                        IsTicketExists.LatsUpdatedOnUTC = DateTime.Now;
                        model.ID = IsTicketExists.ID;
                        db.SaveChanges();
                    }
                }
            }
            else
            {
                model.ID = 0;
            }

            string Scale = Tickets.AddTicket(model);

            ViewBag.TicketSuccess = " درخواست با موفقیت ثبت شد. کد پیگری شما : " + Scale;
            return PartialView(model);


        }


        /// <summary>
        /// in controller jahate peygiriye darkhast tavassote code peygiri morede estefade gharar migirad
        /// </summary>
        /// <param name="TicketId">idie darkhaste morede nazar</param>
        /// <returns>
        /// ActionResult : PartialView(TicketTracking)
        /// </returns>
        [HttpPost]
        public ActionResult TicketTracking(string TicketId)
        {
            int Id = 0;
            using (BigMill.Models.Entities db = new Entities())
            {
                var ListObject = db.Tickets.FirstOrDefault(m => m.TrackingCode == TicketId);
                if (ListObject == null)
                {
                    ViewBag.TicketError = "کد پیگیری وارد شده صحیح نمی باشد";
                    return PartialView("AddTicketPost");
                }
                else
                {
                    Id = ListObject.ID;
                }
            }
            TicketInboxManagement TIM = new TicketInboxManagement();
            List<TicketInboxModel> model = TIM.ListTicketInbox(Id).ToList();
            if (model.Count < 1)
            {
                ViewBag.TicketError = "کد پیگیری وارد شده صحیح نمی باشد";
                return PartialView("AddTicketPost");
            }
            return PartialView(model);
        }


        /// <summary>
        /// in controller jahate peygiriye darkhaste morede nazare device haye androidi mibashad (Makhsuse Android)
        /// </summary>
        /// <param name="TicketId">idie darkhaste morede nazar</param>
        /// <returns>
        /// string
        /// Jsoni dar ghalebe string
        /// fielde marbut be matn ha ba (Content) va inke payam az tarafe server bude ya az tarafe device ba (type) moshakhas mishavad
        /// </returns>
        public string AndroidTicketTracking(string TicketId)
        {

            int Id = 0;
            using (BigMill.Models.Entities db = new Entities())
            {
                var ListObject = db.Tickets.FirstOrDefault(m => m.TrackingCode == TicketId);
                if (ListObject == null)
                {
                    return "{\"Root\": []}";
                }
                else
                {
                    Id = ListObject.ID;
                }
            }
            TicketInboxManagement TIM = new TicketInboxManagement();
            var Tickets = TIM.ListTicketInbox(Id);
            string result = "{\"Root\": [";
            foreach (var InBoxItem in Tickets)
            {
                result += "{\"Content\": \"" + InBoxItem.TicketContent + "\", \"Time\": \"" + InBoxItem.CreatedOnUTCJalali + "\" , \"type\":\"Inbox\"},";
                foreach (var OutBoxItem in InBoxItem.TicketOutbox)
                {
                    result += "{\"Content\": \"" + OutBoxItem.Content_One + "\", \"Time\": \"" + OutBoxItem.CreatedOnUTCJalali + "\" , \"type\":\"OutBox\"},";
                }
            }
            result = result.Remove(result.LastIndexOf(','), 1);
            result += "]}";
            return result;
        }

        /// <summary>
        /// in controller jahate ersale darkhast tavassote device haye androidi morede estefade gharar migirad
        /// </summary>
        /// <param name="UploadFile"></param>
        /// <param name="Content">matne darkhast</param>
        /// <param name="ID">dar surati ke darkhast baraye avvalin bar bashad (ID == -1) dar gheyre in surat (ID == codePeygiri) ke darkhaste jadid bar ruye haman code zakhire shavad</param>
        /// <returns>
        /// string
        /// Jsoni tahte onvane string
        /// Code peygiri : zamani ke darkhast be dorosti sabt shode bashad
        /// NOK : zamani ke heyne sabte darkhast moshkeli be vojud amade bashad
        /// </returns>
        [HttpPost]
        public string AndroidSendTicket(HttpPostedFileBase UploadFile, string Content, string ID)
        {
            TicketManagement Ticket = new TicketManagement();
            UserTicketModel model = new UserTicketModel();
            model.Content = Content;
            if (UploadFile != null)
            {
                var allowedExtensions = new[] { ".doc", ".xlsx", ".txt", ".pdf", ".ppt", ".gif", ".jpg", ".jpeg", ".bmp", ".png", ".amr", ".mp3", ".amr", ".mp4", ".3gp", ".mov", ".mpeg" };

                var extension = Path.GetExtension(UploadFile.FileName).ToLower();
                if (!allowedExtensions.Contains(extension))
                {
                    return "NOK";
                }
                model.MediaBox.Add(UploadFile);
            }
            try
            {
                using (BigMill.Models.Entities db = new Entities())
                {
                    if (ID != "-1")
                    {

                        var IsTicketExists = db.Tickets.FirstOrDefault(u => u.TrackingCode == ID);
                        if (IsTicketExists == null)
                            return "کد پیگیری وارد شده صحیح نمی باشد";
                        else
                            IsTicketExists.LatsUpdatedOnUTC = DateTime.Now;
                        model.ID = IsTicketExists.ID;
                    }
                    string Scale = Ticket.AndroidAddTicket(model);
                    return "" + Scale;
                }
            }
            catch
            {
                return "NOK";
            }
        }

        public string SMSTicketAdd(string SecureString, string from, string msg)
        {
            if (SecureString == "PARSADORSA")
            {
                UserTicketModel model = new UserTicketModel();
                model.Content = msg;
                model.Tell = from;
                TicketManagement Ticket = new TicketManagement();
                string Scale = Ticket.SMSAddTicket(model);
                BigMill.Areas.Admin3mill.Models.SendingReciveingSMS SMS = new BigMill.Areas.Admin3mill.Models.SendingReciveingSMS();
                SMS.Send_single(" درخواست با موفقیت ثبت شد. کد پیگری شما : " + Scale, from);
            }
            return "OK";
        }

        public string SMSTicketTracking(string SecureString, string from, string msg)
        {
            if (SecureString == "PARSADORSA")
            {
                BigMill.Areas.Admin3mill.Models.SendingReciveingSMS SMS = new BigMill.Areas.Admin3mill.Models.SendingReciveingSMS();
                using (BigMill.Models.Entities db = new Entities())
                {
                    var _object = db.Tickets.FirstOrDefault(u => u.TrackingCode.ToLower() == msg.ToLower());
                    if (_object != null)
                    {
                        var _obj = _object.TicketInboxes.OrderByDescending(u => u.CreatedOnUTC).FirstOrDefault().TicketOutboxes.FirstOrDefault();
                        if (_obj != null)
                        {

                            SMS.Send_single(_obj.Content_One, from);
                        }
                        else
                        {
                            SMS.Send_single("درخواست شما در حال بررسی می باشد.", from);
                        }

                    }
                    else
                    {
                        SMS.Send_single("کد پیگیری صحیح نمی باشد.", from);
                    }
                }
            }
            return "OK";
        }
    }
}