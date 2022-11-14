
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace BigMill.Models
{
    public class TicketInboxMediaManagement
    {
        /// <summary>
        /// jahate ezafe kardane file haye multi mediaye hamrahe darkhaste morede nazar morede estefade gharar migirad
        /// mahdudiyyate haddeaksar zakhire 5 file dar har bar eemal shode ast
        /// </summary>
        /// <param name="model"></param>
        /// <param name="F_TicketInbox_Id">idie TicketInbox morede nazar</param>
        /// <returns>
        /// int : 1
        /// </returns>
        public int  AddTicketInboxMedia(UserTicketModel model,int F_TicketInbox_Id)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                int Count = 0;
                foreach (var item in model.MediaBox)
                {
                    TicketInboxMedia InsertObject = new TicketInboxMedia();
                    InsertObject.F_TicketInbox_Id = F_TicketInbox_Id;
                    InsertObject.MediaType = item.ContentType;
                    if (Count < 5)
                    {
                        if (item != null)
                        {
                            if (item.ContentLength > 0)
                            {
                                string curFile = "";
                                string RandomValueString;
                                Random rnd = new Random();
                                string extension = Path.GetExtension(item.FileName);
                                do
                                {

                                    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                                    RandomValueString = new string(Enumerable.Repeat(chars, 12)
                                     .Select(s => s[rnd.Next(s.Length)]).ToArray());
                                    curFile = HttpContext.Current.Server.MapPath("~/App_Data/TicketMediaUpload/") + RandomValueString + extension;  //Your path
                                } while (File.Exists(curFile));
                                InsertObject.MediaPath = RandomValueString + extension;
                                item.SaveAs(curFile);
                                Count++;
                            }
                        }
                    }
                    db.TicketInboxMedias.Add(InsertObject);
                }
                db.SaveChanges();
                return 1;
            }
        }

        /// <summary>
        /// in tabe jahate list kardane file haye multi media ye darkhaste morede nazar bekar miravad
        /// </summary>
        /// <param name="F_TicketInbox_Id">idie TicketInboxe morede nazar</param>
        /// <returns>
        /// List<TicketInboxMediaModel>
        /// </returns>
        public List<TicketInboxMediaModel> ListTicketInboxMedia(int F_TicketInbox_Id)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var ListObject = db.TicketInboxMedias.Where(m=>m.F_TicketInbox_Id==F_TicketInbox_Id);
                List<TicketInboxMediaModel> list = new List<TicketInboxMediaModel>();
                foreach (var ListItem in ListObject)
                {
                    TicketInboxMediaModel t = new TicketInboxMediaModel();
                    t.F_TicketInbox_Id = ListItem.F_TicketInbox_Id??default(int);
                    t.MediaPath = ListItem.MediaPath;
                    t.MediaType = ListItem.MediaType; 
                    list.Add(t);
                }
                return list;
            }
        }
    }
}