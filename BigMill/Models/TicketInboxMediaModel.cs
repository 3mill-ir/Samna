using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BigMill.Models
{
    public class TicketInboxMediaModel
    {
        [Display(ResourceType = typeof(Resource.Resource), Name = "TicketInboxMedia_ID")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public int ID { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "TicketInboxMedia_MediaPath")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public string MediaPath { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "TicketInboxMedia_MediaType")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public string MediaType { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "TicketInboxMedia_F_TicketInbox_Id")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public int F_TicketInbox_Id { get; set; }
    }
}