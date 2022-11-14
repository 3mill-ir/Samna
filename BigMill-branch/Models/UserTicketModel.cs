using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BigMill.Models
{
    public class UserTicketModel
    {
        public UserTicketModel()
        {
            MediaBox = new List<HttpPostedFileBase>();
        }

        [Display(ResourceType = typeof(Resource.Resource), Name = "UserTicketModel_Content")]
        public string Content { get; set; }
        public List<HttpPostedFileBase> MediaBox { get; set; }

        public int ID { get; set; }
        public int TrackingTicketID { get; set; }

        public string TrackingCode { get; set; }

        public string Tell { get; set; }

    }
}