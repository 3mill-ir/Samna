using BigMill.Areas.Admin3mill.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BigMill.Models
{
    public class TicketInboxManagement
    {
        /// <summary>
        /// in tabe jahate sabte darkhaste jadid morede estefade gharar migirad ( Makhsuse Web )
        /// Tavabe (AddTicketInboxMedia , AddTicketLog) dar in tabe farakhani migardand
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// int : 1
        /// </returns>
        public int AddTicketInbox(UserTicketModel model)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                TicketInbox InsertObject = new TicketInbox();
                InsertObject.CreatedOnUTC = DateTime.Now;
                InsertObject.F_Ticket_Id = model.ID;
                // tafavote asli ba tabe makhsuse android
                InsertObject.TicketFrom = "Web";
                InsertObject.TicketContent = model.Content;
                string TiketType = "";
                TiketType += "Text";
                if (model.MediaBox.Count != 0)
                    TiketType = "Multimedia";
                InsertObject.TicketType = TiketType;
                db.TicketInboxes.Add(InsertObject);
                db.SaveChanges();
                TicketInboxMediaManagement TIMM = new TicketInboxMediaManagement();
                // in tabe jahate afzudane file haye multimediaye hamrahe darkhast  farakhani shode ast
                TIMM.AddTicketInboxMedia(model, InsertObject.ID);
                TicketLogManagement TLM = new TicketLogManagement();
                // in tabe jahate zakhire moshakhasate systeme shakhse darkhast dahande morede estefade gharar migirad
                TLM.AddTicketLog(InsertObject.ID);
                return 1;
            }
        }


        /// <summary>
        /// in tabe jahate sabte darkhaste jadid morede estefade gharar migirad ( Makhsuse Android )
        /// Tavabe (AndroidAddTicketLog , AddTicketLog) dar in tabe farakhani migardand
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// int : 1
        /// </returns>
        public int AndroidAddTicketInbox(UserTicketModel model)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                TicketInbox InsertObject = new TicketInbox();
                InsertObject.CreatedOnUTC = DateTime.Now;
                InsertObject.F_Ticket_Id = model.ID;
                // tafavote asli ba tabe makhsuse Web
                InsertObject.TicketFrom = "Android";
                InsertObject.TicketContent = model.Content;
                string TiketType = "";
                if (String.IsNullOrEmpty(model.Content))
                {
                    if (model.MediaBox[0].ContentType.Contains("image"))
                        TiketType = "Image";
                    else if (model.MediaBox[0].ContentType.Contains("application") || model.MediaBox[0].ContentType.Contains("text"))
                        TiketType = "Document";
                    else if (model.MediaBox[0].ContentType.Contains("video"))
                        TiketType = "Video";
                    else if (model.MediaBox[0].ContentType.Contains("audio"))
                        TiketType = "Voice";
                }
                else
                {
                    if(model.MediaBox.Count>0)
                        TiketType = "Multimedia";
                    else
                        TiketType += "Text";
                }
                InsertObject.TicketType = TiketType;
                db.TicketInboxes.Add(InsertObject);
                db.SaveChanges();
                TicketInboxMediaManagement TIMM = new TicketInboxMediaManagement();
                // in tabe jahate afzudane file haye multimediaye hamrahe darkhast  farakhani shode ast
                TIMM.AddTicketInboxMedia(model, InsertObject.ID);
                TicketLogManagement TLM = new TicketLogManagement();
                // in tabe jahate zakhire moshakhasate systeme shakhse darkhast dahande morede estefade gharar migirad
                TLM.AndroidAddTicketLog(InsertObject.ID);
                return 1;
            }
        }

        /// <summary>
        /// in tabe jahate 
        /// tabe (SMSAddTicketLog) dar in tabe farakhani mishavad
        /// </summary>
        /// <param name="model"></param>
        /// <returns>
        /// int : 1
        /// </returns>
        public int SMSAddTicketInbox(UserTicketModel model)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                TicketInbox InsertObject = new TicketInbox();
                InsertObject.CreatedOnUTC = DateTime.Now;
                InsertObject.F_Ticket_Id = model.ID;
                InsertObject.TicketFrom = "SMS";
                InsertObject.TicketContent = model.Content;
                string TiketType = "Text";
                InsertObject.TicketType = TiketType;
                db.TicketInboxes.Add(InsertObject);
                db.SaveChanges();
                TicketLogManagement TLM = new TicketLogManagement();
                TLM.SMSAddTicketLog(InsertObject.ID,model.Tell);
                return 1;
            }
        }

        /// <summary>
        /// jahate list kardane mokalemate code peygiriye morede nazar ba system morede estefade gharar migirad
        /// tabe staticiye (GetDateTimeReturnJalaliDate) az classe Tools dar in tabe farakhani shode ast
        /// tavabe (ListTicketOutBox , ListTicketInboxMedia) dar in tabe farakhani mishavand
        /// </summary>
        /// <param name="F_Ticket_Id">idie darkhaste morede nazar</param>
        /// <returns>
        /// List<TicketInboxModel>
        /// </returns>
        public List<TicketInboxModel> ListTicketInbox(int F_Ticket_Id)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var ListObject = db.TicketInboxes.Where(m => m.F_Ticket_Id == F_Ticket_Id);
                if (ListObject.FirstOrDefault() != null)
                {
                    if (ListObject.FirstOrDefault().Ticket.Status == "در وضعیت انتظار")
                    {
                        ListObject.FirstOrDefault().Ticket.Status = "در حال بررسی";
                        db.SaveChanges();
                    }
                }
                List<TicketInboxModel> list = new List<TicketInboxModel>();
                foreach (var ListItem in ListObject)
                {
                    TicketInboxModel t = new TicketInboxModel();
                    t.CreatedOnUTC = ListItem.CreatedOnUTC ?? default(DateTime);
                    t.CreatedOnUTCJalali = Tools.GetDateTimeReturnJalaliDate(ListItem.CreatedOnUTC ?? default(DateTime));
                    t.F_Ticket_Id = ListItem.F_Ticket_Id ?? default(int);
                    t.TicketFrom = ListItem.TicketFrom ?? default(string);
                    t.TicketContent = ListItem.TicketContent;
                    t.TicketType = ListItem.TicketType;
                    t.ID = ListItem.ID;
                    TicketOutBoxManagement Outbox = new TicketOutBoxManagement();
                    t.TicketOutbox = Outbox.ListTicketOutBox(ListItem.ID).OrderByDescending(u => u.CreatedOnUTC).ToList();
                    TicketInboxMediaManagement Media = new TicketInboxMediaManagement();
                    t.TicketInboxMedia = Media.ListTicketInboxMedia(ListItem.ID).ToList();
                    list.Add(t);
                }
                return list;
            }
        }


        /// <summary>
        /// in tabe jahate list kardane tamamiye darkhastha morede estefade gharar migirad.
        /// tabe staticiye (GetDateTimeReturnJalaliDate) az classe Tools dar in tabe farakhani shode ast
        /// </summary>
        /// <returns>
        /// List<TicketInboxModel>
        /// </returns>
        public List<TicketInboxModel> ListAllTicketInboxes()
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var ListObject = db.TicketInboxes.OrderByDescending(u => u.CreatedOnUTC).Take(10);
                List<TicketInboxModel> list = new List<TicketInboxModel>();
                foreach (var ListItem in ListObject)
                {
                    TicketInboxModel t = new TicketInboxModel();
                    t.CreatedOnUTCJalali = Tools.GetDateTimeReturnJalaliDate(ListItem.CreatedOnUTC ?? default(DateTime));
                    t.TicketContent = ListItem.TicketContent;
                    t.TicketFrom = ListItem.TicketFrom;
                    t.ID = ListItem.ID;
                    list.Add(t);
                }
                return list;
            }
        }
    }
}