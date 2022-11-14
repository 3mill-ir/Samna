using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BigMill.Models
{
    public class PollLogManagement
    {
        /// <summary>
        /// In tabe jahate afzudane PollLog bar asase pasokhe morede nazar va ip ye device sherkat konande dar nazarsanji bekar gerefte mishavad
        /// </summary>
        /// <param name="F_PollAnswer_Id">type="int"</param>
        /// <param name="Device">type="string"</param>
        /// <returns>
        /// int (1)
        /// </returns>
        public int AddPollLog(int F_PollAnswer_Id,string Device)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                PollLog InsertObject = new PollLog();
                //Jahate gereftan IP ye marbut be dastgahe sherkat konande dar nazarsanji
                string IP = HttpContext.Current.Request.UserHostAddress;
                //Marbut be IP haye local
                if (IP == "::1")
                    IP = "127.0.0.1";
                InsertObject.Address = IP;
                InsertObject.CreatedOnUTC =DateTime.Now;
                InsertObject.Device = Device;
                InsertObject.F_PollAnswer_Id = F_PollAnswer_Id;
                db.PollLogs.Add(InsertObject);
                db.SaveChanges();
                return 1;
            }
        }



        //public int EditPollLog(PollLogModel model)
        //{
        //    using (BigMill.Models.Entities db = new Entities())
        //    {
        //        var EditObject = db.PollLogs.FirstOrDefault(u => u.ID == model.ID);
        //        if (EditObject != null)
        //        {
        //            EditObject.Address = model.Address;
        //            EditObject.CreatedOnUTC = DateTime.UtcNow;
        //            EditObject.Device = model.Device;
        //            EditObject.F_PollAnswer_Id = model.F_PollAnswer_Id;
        //            db.SaveChanges();
        //            return 1;
        //        }
        //        else
        //            return 2;
        //    }
        //}

        //public int DeletePollLog(PollLogModel model)
        //{
        //    using (BigMill.Models.Entities db = new Entities())
        //    {
        //        var DeleteObject = db.PollLogs.FirstOrDefault(u => u.ID == model.ID);
        //        if (DeleteObject != null)
        //        {
        //            db.PollLogs.Remove(DeleteObject);
        //            db.SaveChanges();
        //            return 1;
        //        }
        //        else
        //            return 2;
        //    }
        //}

        //public List<PollLogModel> ListPollLog()
        //{
        //    using (BigMill.Models.Entities db = new Entities())
        //    {
        //        var ListObject = db.PollLogs;
        //        List<PollLogModel> list = new List<PollLogModel>();
        //        foreach (var ListItem in ListObject)
        //        {
        //            PollLogModel t = new PollLogModel();
        //            t.Address = ListItem.Address;
        //            t.CreateOnUTC = ListItem.CreatedOnUTC??default(DateTime);
        //            t.Device = ListItem.Device;
        //            t.F_PollAnswer_Id = ListItem.F_PollAnswer_Id ?? default(int);
        //            t.ID = ListItem.ID;
        //            list.Add(t);
        //        }
        //        return list;
        //    }
        //}
    }
}