using BigMill.Areas.Admin3mill.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BigMill.Models
{
    public class TicketOutBoxManagement
    {
        public int AddTicketOutBox(TicketOutBoxModel model)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var ListObject = db.TicketInboxes.FirstOrDefault(m => m.ID == model.F_TicketInbox_Id);
                if (ListObject != null)
                {
                        ListObject.Ticket.Status = "پاسخ داده شده";
                        db.SaveChanges();
                }
                TicketOutbox InsertObject = new TicketOutbox();
                InsertObject.CreatedOnUTC = DateTime.Now;
                InsertObject.Content_One = model.Content_One;
                InsertObject.F_TicketInbox_Id = model.F_TicketInbox_Id;
                InsertObject.F_User_Id = Tools.F_UserId();
                InsertObject.isRead = false;

                if (!string.IsNullOrEmpty(model.SMSText))
                {
                    InsertObject.SMSText = model.SMSText;
                    SendingReciveingSMS SMS = new SendingReciveingSMS();
                    string[] SMS_Status = SMS.Send_single(model.SMSText, "09141863449");
                    InsertObject.SMSStatusOne = SMS_Status[0];
                    InsertObject.SMSStatusTwo = SMS_Status[1];
                }
                db.TicketOutboxes.Add(InsertObject);
                db.SaveChanges();
                return 1;
            }
        }

        /// <summary>
        /// in tabe jahate list kardane OutBox haye darkhaste morede nazar morede estefade gharar migirad
        /// </summary>
        /// <param name="F_TicketInbox_Id">idie darkhaste morede nazar</param>
        /// <returns>
        /// List<TicketOutBoxModel>
        /// </returns>
        public List<TicketOutBoxModel> ListTicketOutBox(int F_TicketInbox_Id)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var ListObject = db.TicketOutboxes.Where(m=>m.F_TicketInbox_Id==F_TicketInbox_Id);
                List<TicketOutBoxModel> list = new List<TicketOutBoxModel>();
                foreach (var ListItem in ListObject)
                {
                    TicketOutBoxModel t = new TicketOutBoxModel();
                    t.CreatedOnUTC = ListItem.CreatedOnUTC??default(DateTime);
                    t.CreatedOnUTCJalali =Tools.GetDateTimeReturnJalaliDate(ListItem.CreatedOnUTC ?? default(DateTime));
                    t.Content_One = ListItem.Content_One;
                    t.F_TicketInbox_Id = ListItem.F_TicketInbox_Id ?? default(int);
                    t.F_User_Id = ListItem.F_User_Id ?? default(int);
                    t.isRead = ListItem.isRead ?? default(bool);
                    t.SMSStatusOne = ListItem.SMSStatusOne;
                    t.SMSStatusTwo = ListItem.SMSStatusTwo;
                    t.SMSText = ListItem.SMSText;
                    t.ID = ListItem.ID;
                    list.Add(t);
                }
                return list;
            }
        }
    }
}