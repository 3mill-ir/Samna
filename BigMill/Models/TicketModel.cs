using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BigMill.Models
{
    public class TicketModel
    {
        [Display(ResourceType = typeof(Resource.Resource), Name = "Ticket_ID")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public int ID { get; set; }

        public string TrackingCode { get; set; }

        public string ID_STR { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "Ticket_Status")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public string Status { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "Ticket_LastUpdateOnUtc")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public DateTime LastUpdateOnUtc { get; set; }

        public string LastUpdateOnUtcJalali { get; set; }

        public string TicketInbox_brief { get; set; }
        public int CountInbox { get; set; }
        public int CountOutbox { get; set; }
        public int CountInboxMedia { get; set; }

     
  
      public  Collection<TicketInboxModel> TicketInbox { get; set; }
    }
}