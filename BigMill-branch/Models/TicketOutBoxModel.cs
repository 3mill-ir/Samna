using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BigMill.Models
{
    public class TicketOutBoxModel
    {
        [Display(ResourceType = typeof(Resource.Resource), Name = "TicketOutBox_ID")]

        public int ID { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "TicketOutBox_Content_One")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public string Content_One { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "TicketOutBox_isRead")]

        public bool isRead { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "TicketOutBox_SMSText")]

        public string SMSText { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "TicketOutBox_SMSStatusOne")]

        public string SMSStatusOne { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "TicketOutBox_SMSStatusTwo")]

        public string SMSStatusTwo { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "TicketOutBox_CreatedOnUTC")]

        public DateTime CreatedOnUTC { get; set; }
        public string CreatedOnUTCJalali { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "TicketOutBox_F_TicketInbox_Id")]

        public int F_TicketInbox_Id { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "TicketOutBox_F_User_Id")]

        public int F_User_Id { get; set; }
    }
}