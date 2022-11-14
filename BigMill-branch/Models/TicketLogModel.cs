using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BigMill.Models
{
    public class TicketLogModel
    {
        [Display(ResourceType = typeof(Resource.Resource), Name = "TicketLog_ID")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public int ID { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "TicketLog_Address")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public string Address { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "TicketLog_Device")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public string Device { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "TicketLog_CreatedOnUTC")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public DateTime CreatedOnUTC { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "TicketLog_F_TicketInbox_Id")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public int F_TicketInbox_Id { get; set; }
    }
}