using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BigMill.Models
{
    public class PollLogModel
    {
        [Display(ResourceType = typeof(Resource.Resource), Name = "PollLog_ID")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public int ID { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "PollLog_Address")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public string Address { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "PollLog_Device")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public string Device { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "PollAnswer_CreateOnUTC")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public DateTime CreateOnUTC { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "PollLog_F_PollAnswer_Id")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public int F_PollAnswer_Id { get; set; }
    }
}