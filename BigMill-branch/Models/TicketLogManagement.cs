using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BigMill.Models
{
    public class TicketLogManagement
    {
        /// <summary>
        /// in tabe jahate sabte moshakhasate systemiye darkhast dahande morede estefade gharar migirad ( Makhsuse Web )
        /// </summary>
        /// <param name="F_TicketInbox_Id">idie TicketInboxe morede nazar</param>
        /// <returns>
        /// int : 1
        /// </returns>
        public int AddTicketLog(int F_TicketInbox_Id)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                TicketLog InsertObject = new TicketLog();
                InsertObject.CreatedOnUTC = DateTime.Now;
                // jahate gereftane IP
                string IP = HttpContext.Current.Request.UserHostAddress;
                if (IP == "::1")
                    IP = "127.0.0.1";
                InsertObject.Address = IP;
                InsertObject.Device = "Web";
                InsertObject.F_TicketInbox_Id =F_TicketInbox_Id;
                db.TicketLogs.Add(InsertObject);
                db.SaveChanges();
                return 1;
            }
        }

        /// <summary>
        /// in tabe jahate sabte moshakhasate systemiye darkhast dahande morede estefade gharar migirad ( Makhsuse Android )
        /// </summary>
        /// <param name="F_TicketInbox_Id">idie TicketInboxe morede nazar</param>
        /// <returns>
        /// int : 1
        /// </returns>
        public int AndroidAddTicketLog(int F_TicketInbox_Id)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                TicketLog InsertObject = new TicketLog();
                InsertObject.CreatedOnUTC = DateTime.Now;
                // jahate gereftane IP
                string IP = HttpContext.Current.Request.UserHostAddress;
                if (IP == "::1")
                    IP = "127.0.0.1";
                InsertObject.Address = IP;
                InsertObject.Device = "Android";
                InsertObject.F_TicketInbox_Id = F_TicketInbox_Id;
                db.TicketLogs.Add(InsertObject);
                db.SaveChanges();
                return 1;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="F_TicketInbox_Id">idie TicketInboxe morede nazar</param>
        /// <param name="Tell">shomare telefone morede nazar jahate ersale sms be aan</param>
        /// <returns>
        /// int : 1
        /// </returns>
        public int SMSAddTicketLog(int F_TicketInbox_Id,string Tell)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                TicketLog InsertObject = new TicketLog();
                InsertObject.CreatedOnUTC = DateTime.Now;
                InsertObject.Address = Tell;
                InsertObject.Device = "SMS";
                InsertObject.F_TicketInbox_Id = F_TicketInbox_Id;
                db.TicketLogs.Add(InsertObject);
                db.SaveChanges();
                return 1;
            }
        }

    }
}