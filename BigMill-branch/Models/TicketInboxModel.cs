using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BigMill.Models
{
    public class TicketInboxModel
    {
        public TicketInboxModel() {
            TicketOutbox = new List<TicketOutBoxModel>();
            TicketInboxMedia = new List<TicketInboxMediaModel>();
        }
        [Display(ResourceType = typeof(Resource.Resource), Name = "TicketInbox_ID")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public int ID { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "TicketInbox_TicketContent")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public string TicketContent { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "TicketInbox_CreatedOnUTC")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public DateTime CreatedOnUTC { get; set; }
        public string CreatedOnUTCJalali { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "TicketInbox_TicketType")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public string TicketType { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "TicketInbox_TicketForm")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public string TicketFrom { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "TicketInbox_F_Ticket_Id")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public int F_Ticket_Id { get; set; }


        public List<TicketOutBoxModel> TicketOutbox { get; set; }

        public List<TicketInboxMediaModel> TicketInboxMedia { get; set; }

    }
}