using BigMill.Areas.Admin3mill.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace BigMill.Models
{
    public class TicketManagement
    {
        public string AddTicket(UserTicketModel model)
        {
            string result = model.TrackingCode;
            using (BigMill.Models.Entities db = new Entities())
            {
                if (model.ID==0)
                {
                    Ticket InsertObject = new Ticket();
                    InsertObject.LatsUpdatedOnUTC = DateTime.Now;
                    InsertObject.Status = "در وضعیت انتظار";
                    InsertObject.TrackingCode = UniqTrackingCodeGenerator();
                    db.Tickets.Add(InsertObject);
                    db.SaveChanges();
                    model.ID = InsertObject.ID;
                    result = InsertObject.TrackingCode;
                }
                TicketInboxManagement TicketInbox = new TicketInboxManagement();
                TicketInbox.AddTicketInbox(model);
                return result;
            }
        }
        public string AndroidAddTicket(UserTicketModel model)
        {
            string result = "";
            using (BigMill.Models.Entities db = new Entities())
            {
                if (model.ID == 0)
                {
                    Ticket InsertObject = new Ticket();
                    InsertObject.LatsUpdatedOnUTC = DateTime.Now;
                    InsertObject.Status = "در وضعیت انتظار";
                    InsertObject.TrackingCode = UniqTrackingCodeGenerator();
                    db.Tickets.Add(InsertObject);
                    db.SaveChanges();
                    model.ID = InsertObject.ID;
                    result = InsertObject.TrackingCode;
                }
                
                TicketInboxManagement TicketInbox = new TicketInboxManagement();
                TicketInbox.AndroidAddTicketInbox(model);
                return result;
            }
        }
        public string SMSAddTicket(UserTicketModel model)
        {
            string result = "";
            using (BigMill.Models.Entities db = new Entities())
            {
                if (model.ID == 0)
                {
                    Ticket InsertObject = new Ticket();
                    InsertObject.LatsUpdatedOnUTC = DateTime.Now;
                    InsertObject.Status = "در وضعیت انتظار";
                    InsertObject.TrackingCode = UniqTrackingCodeGenerator();
                    db.Tickets.Add(InsertObject);
                    db.SaveChanges();
                    model.ID = InsertObject.ID;
                    result = InsertObject.TrackingCode;
                }

                TicketInboxManagement TicketInbox = new TicketInboxManagement();
                TicketInbox.SMSAddTicketInbox(model);
                return result;
            }
        }
        public int ChangeTicketStatus(TicketModel model)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var EditObject = db.Tickets.FirstOrDefault(u => u.ID == model.ID);
                if (EditObject != null)
                {
                    EditObject.Status = model.Status;
                    db.SaveChanges();
                    return 1;
                }
                else
                    return 2;
            }
        }

        public int DeleteTicket(TicketModel model)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var DeleteObject = db.Tickets.FirstOrDefault(u => u.ID == model.ID);
                if (DeleteObject != null)
                {
                    db.Tickets.Remove(DeleteObject);
                    db.SaveChanges();
                    return 1;
                }
                else
                    return 2;
            }
        }

        public List<TicketModel> ListTicket(string SearchString, string TicketStatus, int pageNumber, int pageSize, out int total)
        {
            using (BigMill.Models.Entities db = new Entities())
            {

                IPagedList<Ticket> ListObject;

                if (string.IsNullOrEmpty(TicketStatus))
                {
                    if (string.IsNullOrEmpty(SearchString))
                    {
                        ListObject = db.Tickets.OrderByDescending(u => u.LatsUpdatedOnUTC).ToPagedList(pageNumber, pageSize);
                        //  total = db.Tickets.Count();
                        total = ListObject.TotalItemCount;
                    }
                    else
                    {
                        ListObject = db.Tickets.Where(m => m.TrackingCode.ToLower().Contains(SearchString.ToLower())).OrderByDescending(u => u.LatsUpdatedOnUTC).ToPagedList(pageNumber, pageSize);
                        // db.Tickets.Where(m => m.ID.ToString().Contains(SearchString)).Count();
                        total = ListObject.TotalItemCount;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(SearchString))
                    {
                        ListObject = db.Tickets.Where(m => m.Status == TicketStatus).OrderByDescending(u => u.LatsUpdatedOnUTC).ToPagedList(pageNumber, pageSize);
                        //  total = db.Tickets.Where(m => m.Status == TicketStatus).Count();
                        total = ListObject.TotalItemCount;
                    }
                    else
                    {

                        ListObject = db.Tickets.Where(m => m.Status == TicketStatus).Where(m => m.TrackingCode.ToLower().Contains(SearchString.ToLower())).OrderByDescending(u => u.LatsUpdatedOnUTC).ToPagedList(pageNumber, pageSize);
                        //  total = db.Tickets.Where(m => m.ID.ToString().Contains(SearchString)).Where(m => m.ID.ToString().Contains(SearchString)).Count();
                        total = ListObject.TotalItemCount;
                    }
                }

                List<TicketModel> list = new List<TicketModel>();
                foreach (var ListItem in ListObject)
                {
                    TicketModel t = new TicketModel();
                    t.Status = ListItem.Status;
                    t.LastUpdateOnUtcJalali = Tools.GetDateTimeReturnJalaliDate(ListItem.LatsUpdatedOnUTC ?? default(DateTime));
                    t.LastUpdateOnUtc = ListItem.LatsUpdatedOnUTC ?? default(DateTime);
                    t.ID = ListItem.ID;
                    t.ID_STR = ListItem.ID.ToString();
                    t.TrackingCode = ListItem.TrackingCode;
                    var inbox = ListItem.TicketInboxes.OrderBy(m => m.CreatedOnUTC).FirstOrDefault();
                    if (inbox != null)
                    {
                        switch (inbox.TicketType)
                        {
                            case "Text":
                                if (!string.IsNullOrEmpty(inbox.TicketContent))
                                    t.TicketInbox_brief = inbox.TicketContent;
                                break;
                            case "Voice":
                                t.TicketInbox_brief = "صوت";
                                break;
                            case "Video":
                                t.TicketInbox_brief = "ویدیو";
                                break;
                            case "Image":
                                t.TicketInbox_brief = "عکس";
                                break;
                            case "Document":
                                t.TicketInbox_brief = "سند";
                                break;
                            default:
                                if (!string.IsNullOrEmpty(inbox.TicketType) && inbox.TicketContent != null)
                                    t.TicketInbox_brief = inbox.TicketContent;
                                break;
                        }
                        t.CountInbox = ListItem.TicketInboxes.Count();
                        t.CountInboxMedia = ListItem.TicketInboxes.Sum(m => m.TicketInboxMedias.Count());
                        t.CountOutbox = ListItem.TicketInboxes.Sum(m => m.TicketOutboxes.Count());
                    }
                    list.Add(t);
                }
                return list;
            }
        }



        public TicketModel TicketBrief(int Id)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var Object = db.Tickets.FirstOrDefault(m => m.ID == Id);


                TicketModel t = new TicketModel();
                t.Status = Object.Status;
                t.LastUpdateOnUtc = Object.LatsUpdatedOnUTC ?? default(DateTime);
                t.ID = Object.ID;
                t.TrackingCode = Object.TrackingCode;


                return t;
            }
        }


        public string UniqTrackingCodeGenerator()
        {
            string result = "";
            int length = 8;
            bool loop = true;
            Random random = new Random(DateTime.Now.Millisecond * DateTime.Now.Second);
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            using (BigMill.Models.Entities db = new Entities())
            {
                do
                {
                    string temp = new string(Enumerable.Repeat(chars, length)
                        .Select(s => s[random.Next(s.Length)]).ToArray());
                    var _object = db.Tickets.FirstOrDefault(u => u.TrackingCode == temp);
                    if (_object == null)
                    {
                        result = temp;
                        loop = false;
                    }
                    else
                    {
                        loop = true;
                    }
                } while (loop);
            }
            return result;
        }



    }
}