using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BigMill.Models
{
    public class UserInformationManagement
    {
        public int AddUserInformation(UserInformationModel model)
        {
            using(BigMill.Models.Entities db=new Entities())
            {
                UserInformation InsertObject = new UserInformation();
                InsertObject.CreatedOnUTC = DateTime.UtcNow;
                InsertObject.Email = model.Email;
                InsertObject.F_ID = model.F_ID;
                InsertObject.FirstName = model.FirstName;
                InsertObject.LastName = model.LastName;
                InsertObject.MobileNumber = model.MobileNumber;
                InsertObject.Status = true;
                db.UserInformations.Add(InsertObject);
                db.SaveChanges();
                return 1;
            }
        }

        public int EditUserInformation(UserInformationModel model)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var EditObject = db.UserInformations.FirstOrDefault(u => u.ID == model.ID);
                if (EditObject != null)
                {
                    EditObject.UpdatedOnUTC = DateTime.UtcNow;
                    EditObject.Email = model.Email;
                    EditObject.F_ID = model.F_ID;
                    EditObject.FirstName = model.FirstName;
                    EditObject.LastName = model.LastName;
                    EditObject.MobileNumber = model.MobileNumber;
                    EditObject.Status = model.Status;
                    db.SaveChanges();
                    return 1;
                }
                else
                    return 2;
            }
        }

        public int DeleteUserInformation(UserInformationModel model)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var DeleteObject = db.UserInformations.FirstOrDefault(u => u.ID == model.ID);
                if (DeleteObject != null)
                {
                    db.UserInformations.Remove(DeleteObject);
                    db.SaveChanges();
                    return 1;
                }
                else
                    return 2;
            }
        }

        public int ChangeStatusUserInformation(UserInformationModel model)
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var ChangeStatusObject = db.UserInformations.FirstOrDefault(u => u.ID == model.ID);
                if (ChangeStatusObject != null)
                {
                    ChangeStatusObject.Status = !model.Status;
                    db.SaveChanges();
                    return 1;
                }
                else
                    return 2;
            }
        }

        public List<UserInformationModel> ListUserInformation()
        {
            using (BigMill.Models.Entities db = new Entities())
            {
                var ListObject = db.UserInformations;
                List<UserInformationModel> list = new List<UserInformationModel>();
                foreach (var ListItem in ListObject)
                {
                    UserInformationModel t = new UserInformationModel();
                    t.UpdatedOnUTC = ListItem.UpdatedOnUTC??default(DateTime);
                    t.CreatedOnUTC = ListItem.CreatedOnUTC ?? default(DateTime);
                    t.Email = ListItem.Email;
                    t.F_ID = ListItem.F_ID;
                    t.FirstName = ListItem.FirstName;
                    t.LastName = ListItem.LastName;
                    t.MobileNumber = ListItem.MobileNumber;
                    t.Status = ListItem.Status ?? default(bool);
                    list.Add(t);
                }
                return list;
            }
        }


    }
}