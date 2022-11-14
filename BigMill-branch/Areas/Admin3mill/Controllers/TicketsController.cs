using BigMill.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Web.Hosting;
using System.IO;
using System.Net;
using PagedList;
using BigMill.CustomFilters;

namespace BigMill.Areas.Admin3mill.Controllers
{
        [AuthLog(Roles = "Developer, Admin, Developer")]
    public class TicketsController : Controller
    {
        /// <summary>
        /// in controller jahate generate kardane view e safheye modiriyyate darkhast ha morede estefade gharar migirad
        /// </summary>
        /// <returns>
        /// ActionResult : View(Index)
        /// </returns>
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult TicketAccessory()
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                int UnResponseTicket = db.Tickets.Where(u => u.Status == "در وضعیت انتظار").Count();
                int ToResponseTicket = db.Tickets.Where(u => u.Status == "در حال بررسی").Count();
                int ResponseTicket = db.Tickets.Where(u => u.Status == "پاسخ داده شده").Count();
                @ViewBag.UnResponseTicketCount = UnResponseTicket.ToString();
                @ViewBag.ToResponseTicketCount = ToResponseTicket.ToString();
                @ViewBag.ResponseTicketCount = ResponseTicket.ToString();
                @ViewBag.AllTicketCount = (ResponseTicket + ToResponseTicket + UnResponseTicket).ToString();
            }

            return PartialView();
        }

        [HttpPost]
        public ActionResult TicketList(int? page, string currentFilter, string searchString, string TicketStatus)
        {
            TicketManagement Ticket = new TicketManagement();


            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewBag.CurrentFilter = searchString;
            ViewBag.CurrentStatus = TicketStatus;




            int pageSize = 10;
            int pageNumber = (page ?? 1);
            ViewBag.mypage = pageNumber;
            int total;
            var pagedList = new StaticPagedList<TicketModel>(Ticket.ListTicket(searchString, TicketStatus, pageNumber, pageSize, out total).OrderByDescending(u => u.LastUpdateOnUtc), pageNumber, pageSize, total);
            return PartialView(pagedList);
        }

        public ActionResult TicketDetail(int F_TicketId)
        {
            TicketInboxManagement Ticket = new TicketInboxManagement();
            return PartialView(Ticket.ListTicketInbox(F_TicketId).OrderByDescending(u => u.CreatedOnUTC));
        }

        /// <summary>
        /// in controller jahate generate kardane view e pasokh be darkhast ha morede estefade gharar migirad
        /// tabe (AddTicketOutBox) az classe TicketOutBoxManagement dar in controller farakhani shode ast
        /// </summary>
        /// <param name="F_LastTicketInboxId">idie LastTicketInbox e morede nazar</param>
        /// <returns>
        /// ActionResult : PartialView(TicketResponse)
        /// </returns>
        public ActionResult TicketResponse(int F_LastTicketInboxId)
        {

            TicketOutBoxModel model = new TicketOutBoxModel();
            model.F_TicketInbox_Id = F_LastTicketInboxId;
            return PartialView(model);
        }
        /// <summary>
        /// in controller jahate pasokh be darkhast ha morede estefade gharar migirad
        /// tabe (AddTicketOutBox) az classe TicketOutBoxManagement dar in controller farakhani shode ast
        /// </summary>
        /// <param name="model"></param>
        /// <param name="F_LastTicketInboxId">idie LastTicketInbox e morede nazar</param>
        /// <returns>
        /// ActionResult 
        /// PartialView(TicketResponse) : dar surate boruze moshkel dar sabte pasokhe darkhaste morede nazar
        /// PartialView("OKTicketResponse") : dar surati ke ba movaffaghiyat be darkhaste morede nazar pasokh dade shavad
        /// </returns>
        [HttpPost]
        public ActionResult TicketResponse(TicketOutBoxModel model, int F_LastTicketInboxId)
        {

            if (string.IsNullOrEmpty(model.Content_One))
                ModelState.AddModelError("Content_One", "لطفاً فیلد های خالی را پر نمایید");
            if (ModelState.IsValid)
            {
                TicketOutBoxManagement Outbox = new TicketOutBoxManagement();
                Outbox.AddTicketOutBox(model);
                ViewBag.F_LastTicketId = F_LastTicketInboxId;
                return PartialView("OKTicketResponse");
            }
            return PartialView(model);
        }




        /// <summary>
        /// in controller jahate generate kardane view e taghire vaziyate darkhast ke 4 halat darad morede estefade gharar migirad
        /// tabe (TicketBrief) az classe TicketManagement dar in controller farakhani shode ast
        /// </summary>
        /// <param name="F_TicketId">idie darkhaste morede nazar</param>
        /// <returns>
        /// ActionResult : PartialView(TicketChangeStatus)
        /// </returns>
        public ActionResult TicketChangeStatus(int F_TicketId)
        {
            List<SelectListItem> li = new List<SelectListItem>();
            li.Add(new SelectListItem { Text = "انتخاب", Value = "" });
            li.Add(new SelectListItem { Text = "در وضعیت انتظار", Value = "در وضعیت انتظار" });
            li.Add(new SelectListItem { Text = "در حال بررسی", Value = "در حال بررسی" });
            li.Add(new SelectListItem { Text = "پاسخ داده شده", Value = "پاسخ داده شده" });
            ViewData["TicketStatus"] = li;
            TicketManagement ticket = new TicketManagement();
            TicketModel model = new TicketModel();
            model.ID = F_TicketId;
            model.Status = ticket.TicketBrief(F_TicketId).Status;
            return PartialView(model);
        }

        /// <summary>
        /// in controller jahate taghir vaziyate darkhaste morede nazar ke 4 halat darad morede estefade gharar migirad
        /// </summary>
        /// <param name="model"></param>
        /// <param name="F_TicketId">idie darkhaste morede nazar</param>
        /// <returns>
        /// ActionResult : PartialView("OKTicketStatus")
        /// </returns>
        [HttpPost]
        public ActionResult TicketChangeStatus(TicketModel model, int F_TicketId)
        {

            TicketManagement ticket = new TicketManagement();
            ticket.ChangeTicketStatus(model);
            return PartialView("OKTicketStatus");
        }



        /// <summary>
        /// !!!!!!!!!!!!!!!!!!!!! Ehtemalan hazf beshe !!!!!!!!!!!!!!!!!!!!!!!!!!
        /// </summary>
        /// <param name="Media"></param>
        /// <returns>
        /// ActionResult : No View
        /// </returns>
        public ActionResult TicketShowAttachment(string Media)
        {
            return View();
        }



    }
}