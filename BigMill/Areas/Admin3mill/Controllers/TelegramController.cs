using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BigMill.Models;
using System.IO;
using System.Net.Http;
using System.Net;
using BigMill.Areas.Admin3mill.Models;
using BigMill.CustomFilters;

namespace BigMill.Areas.Admin3mill.Controllers
{
    [AuthLog(Roles = "Developer, Admin, Author")]
    public class TelegramController : Controller
    {

        /// <summary>
        /// in controller jahate generate kardane view e modiriyyate channele telegram morede estefade gharar migirad
        /// </summary>
        /// <returns>
        /// ActionResult : View(TelegramManagementPannel)
        /// </returns>
        public ActionResult TelegramManagementPannel()
        {

            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = @Resource.Resource.PageTittle_TelegramManagementPannel;
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<PageTitle> PathLog = new List<PageTitle>();
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *********************************
            return View();
        }

        /// <summary>
        /// in controller jahate generate kardane partialView e ersale message morede estefade gharar migirad
        /// </summary>
        /// <returns>
        /// PartialViewResult : PartialView(SendMessage)
        /// </returns>
        public PartialViewResult SendMessage()
        {
            return PartialView();
        }

        /// <summary>
        /// in controller jahate generate kardane partialView e ersale Tasvir morede estefade gharar migirad
        /// </summary>
        /// <returns>
        /// PartialViewResult : PartialView(SendPhoto)
        /// </returns>
        public PartialViewResult SendPhoto()
        {
            return PartialView();
        }

        /// <summary>
        /// in controller jahate generate kardane partialView e ersale Video morede estefade gharar migirad
        /// </summary>
        /// <returns>
        /// PartialViewResult : PartialView(SendVideo)
        /// </returns>
        public PartialViewResult SendVideo()
        {
            return PartialView();
        }

        /// <summary>
        /// in controller jahate generate kardane partialView e ersale file haye soti morede estefade gharar migirad
        /// </summary>
        /// <returns>
        /// PartialViewResult : PartialView(SendVoice)
        /// </returns>
        public PartialViewResult SendVoice()
        {
            return PartialView();
        }

        /// <summary>
        /// in controller jahate generate kardane partialView e ersale file morede estefade gharar migirad
        /// </summary>
        /// <returns>
        /// PartialViewResult : PartialView(SendDocument)
        /// </returns>
        public PartialViewResult SendDocument()
        {
            return PartialView();
        }




        /// <summary>
        /// in controller jahate ersale message be channele telegrame morede estefade gharar migirad
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// PartialViewResult 
        /// PartialView("TelegramSuccess") : ersale payam ba movaffaghiyat surat gerefte
        /// PartialView("TelegramFailed") : ersale payam ba moshkel ru be ru shode
        /// </returns>
        [HttpPost]
        public PartialViewResult SendMessage(TelegramModel model)
        {
            if (model.Caption == null)
            {
                ModelState.AddModelError("Caption", @Resource.Resource.View_ValidationError);
                return PartialView(model);
            }

            TelegramBot t = new TelegramBot();
            string IsOk = t.SendMessage(model.Caption);
            if (IsOk == "true")
            {
                return PartialView("TelegramSuccess");
            }
            else
            {
                return PartialView("TelegramFailed");
            }
        }

        /// <summary>
        /// in controller jahate ersale Tasvir ba caption be channele telegrame morede estefade gharar migirad
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Path">Tasvire morede nazar jahate ersal</param>
        /// <returns>
        /// ActionResult
        /// PartialView("TelegramSuccess") : ersale Tasvir ba movaffaghiyat surat gerefte
        /// View("TelegramFailed") : ersale Tasvir ba moshkel ru be ru shode
        /// </returns>
        [HttpPost]
        public ActionResult SendPhoto(TelegramModel model, HttpPostedFileBase Path)
        {
            TelegramBot Telegram = new TelegramBot();
            if (Path == null)
            {
                ModelState.AddModelError("Path", @Resource.Resource.View_ValidationError);
                return PartialView(model);
            }

            if (model.Caption != null)
            {
                //agar tule caption bishtar az 199 character bashad be surate payami joda az tasvir ersal mishavad
                // be dalile mahdudiyyate telegram
                if (model.Caption.Length > 199)
                {
                    bool Allresult = false; ;
                    bool result1 = false;
                    bool result2 = false;
                    string temp = model.Caption;
                    model.Caption = "ادامه خبر ... 👇👇👇👇👇👇👇";
                    result1 = Telegram.SendPhoto(model, Path).Equals("true") ? true : false;
                    result2 = Telegram.SendMessage(temp).Equals("true") ? true : false;
                    if (result1 && result2)
                        Allresult = true;
                    if (Allresult)
                    {
                        return PartialView("TelegramSuccess");
                    }
                    else
                    {
                        return View("TelegramFailed");
                    }
                }

            }

            string IsOk = Telegram.SendPhoto(model, Path);
            if (IsOk == "true")
            {
                return PartialView("TelegramSuccess");
            }
            else
            {
                return PartialView("TelegramFailed");
            }
        }


        /// <summary>
        /// in controller jahate ersale Video ba caption be channele telegrame morede estefade gharar migirad
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Path">Video ye morede nazar jahate ersal</param>
        /// <returns>
        /// ActionResult
        /// PartialView("TelegramSuccess") : ersale Video ba movaffaghiyat surat gerefte
        /// View("TelegramFailed") : ersale Video ba moshkel ru be ru shode
        /// </returns>
        [HttpPost]
        public ActionResult SendVideo(TelegramModel model, HttpPostedFileBase Path)
        {
            TelegramBot Telegram = new TelegramBot();
            if (Path == null)
            {
                ModelState.AddModelError("Path", @Resource.Resource.View_ValidationError);
                return PartialView(model);
            }

            if (model.Caption != null)
            {
                //agar tule caption bishtar az 199 character bashad be surate payami joda az tasvir ersal mishavad
                // be dalile mahdudiyyate telegram
                if (model.Caption.Length > 199)
                {
                    bool Allresult = false; ;
                    bool result1 = false;
                    bool result2 = false;
                    string temp = model.Caption;
                    model.Caption = "ادامه خبر ... 👇👇👇👇👇👇👇";
                    result1 = Telegram.SendVideo(model, Path).Equals("true") ? true : false;
                    result2 = Telegram.SendMessage(temp).Equals("true") ? true : false;
                    if (result1 && result2)
                        Allresult = true;
                    if (Allresult)
                    {
                        return PartialView("TelegramSuccess");
                    }
                    else
                    {
                        return View("TelegramFailed");
                    }
                }

            }

            string IsOk = Telegram.SendVideo(model, Path);
            if (IsOk == "true")
            {
                return PartialView("TelegramSuccess");
            }
            else
            {
                return PartialView("TelegramFailed");
            }

        }

        /// <summary>
        /// in controller jahate ersale File Soti ba caption be channele telegrame morede estefade gharar migirad
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Path">File Soti ye morede nazar jahate ersal</param>
        /// <returns>
        /// ActionResult
        /// PartialView("TelegramSuccess") : ersale File Soti ba movaffaghiyat surat gerefte
        /// View("TelegramFailed") : ersale File Soti ba moshkel ru be ru shode
        /// </returns>
        [HttpPost]
        public ActionResult SendVoice(TelegramModel model, HttpPostedFileBase Path)
        {
            TelegramBot Telegram = new TelegramBot();
            if (Path == null)
            {
                ModelState.AddModelError("Path", @Resource.Resource.View_ValidationError);
                return PartialView(model);
            }

            bool Allresult = false; ;
            bool result1 = false;
            bool result2 = false;
            string temp = model.Caption;
            model.Caption = "ادامه خبر ... 👇👇👇👇👇👇👇";
            result1 = Telegram.SendVoice(model, Path).Equals("true") ? true : false;
            result2 = Telegram.SendMessage(temp).Equals("true") ? true : false;
            if (result1 && result2)
                Allresult = true;
            if (Allresult)
            {
                return PartialView("TelegramSuccess");
            }
            else
            {
                return View("TelegramFailed");
            }



        }

        /// <summary>
        /// in controller jahate ersale File ba caption be channele telegrame morede estefade gharar migirad
        /// </summary>
        /// <param name="model"></param>
        /// <param name="Path">File e morede nazar jahate ersal</param>
        /// <returns>
        /// ActionResult
        /// PartialView("TelegramSuccess") : ersale File ba movaffaghiyat surat gerefte
        /// View("TelegramFailed") : ersale File ba moshkel ru be ru shode
        /// </returns>
        [HttpPost]
        public ActionResult SendDocument(TelegramModel model, HttpPostedFileBase Path)
        {
            TelegramBot Telegram = new TelegramBot();
            if (Path == null)
            {
                ModelState.AddModelError("Path", @Resource.Resource.View_ValidationError);
                return PartialView(model);
            }


            bool Allresult = false; ;
            bool result1 = false;
            bool result2 = false;
            string temp = model.Caption;
            model.Caption = "ادامه خبر ... 👇👇👇👇👇👇👇";
            result1 = Telegram.SendDocument(model, Path).Equals("true") ? true : false;
            result2 = Telegram.SendMessage(temp).Equals("true") ? true : false;
            if (result1 && result2)
                Allresult = true;
            if (Allresult)
            {
                return PartialView("TelegramSuccess");
            }
            else
            {
                return View("TelegramFailed");
            }

        }
        /// <summary>
        /// in controller jahate ersale poste jadide sabt shode dar web site be channele telegrame morede nazar morede estefade gharar migirad
        /// </summary>
        /// <param name="type">idie zirshakheye poste morede nazar</param>
        /// <returns>
        /// ActionResult
        /// RedirectToAction("List_SubCategoryName_", "_CategoryName_Controller") : agar type vurudi ozve zirshakheye morede nazar be hamrahe shakhe ash bashad
        /// View(SentContentToTelegram) : agar type vorudi ozve zirshakheye morede nazar nabashad
        /// </returns>
        public ActionResult SentContentToTelegram(string type)
        {
            TelegramModel model = new TelegramModel();
            model.Caption = TempData["Caption"].ToString();
            model.Path = TempData["ImgPath"].ToString();
            string tempURL = System.Configuration.ConfigurationManager.AppSettings["WebSiteURLForTelegram"] + TempData["URL4Chanel"].ToString();
            TelegramBot Telegram = new TelegramBot();

            //agar tule caption bishtar az 199 character bashad be surate payami joda az tasvir ersal mishavad
            // be dalile mahdudiyyate telegram
            if ((model.Caption.Length + tempURL.Length) > 199)
            {

                string temp = model.Caption;
                model.Caption = "ادامه خبر ... 👇👇👇👇👇👇👇" + "\n" + tempURL;
                Telegram.SendContent(model);
                Telegram.SendMessage(temp);

            }
            else
            {
                model.Caption = model.Caption + "\n" + "ادامه خبر ... 👇👇👇👇👇👇👇" + "\n" + tempURL;
                Telegram.SendContent(model);
            }




            if (type == System.Configuration.ConfigurationManager.AppSettings["AkhbarPublicID"])
            {
                return RedirectToAction("ListAkhbarPublic", "Akhbar");
            }
            else if (type == System.Configuration.ConfigurationManager.AppSettings["AkhbarHozeID"])
            {
                return RedirectToAction("ListAkhbarHoze", "Akhbar");
            }
            else if (type == System.Configuration.ConfigurationManager.AppSettings["SpeechID"])
            {
                return RedirectToAction("ListSpeech", "Parliament");
            }
            else if (type == System.Configuration.ConfigurationManager.AppSettings["InterviewID"])
            {
                return RedirectToAction("ListInterview", "Parliament");
            }
            else if (type == System.Configuration.ConfigurationManager.AppSettings["QuestionID"])
            {
                return RedirectToAction("ListQuestion", "Parliament");
            }
            else if (type == System.Configuration.ConfigurationManager.AppSettings["CorrespondenceID"])
            {
                return RedirectToAction("ListCorrespondence", "Parliament");
            }



            return View();
        }
    }
}