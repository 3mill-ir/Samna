using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BigMill.Models
{
    public class TelegramModel
    {
        [Display(ResourceType = typeof(Resource.Resource), Name = "TelegramModel_Caption")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public string Caption { get; set; }
        [Display(ResourceType = typeof(Resource.Resource), Name = "TelegramModel_Path")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public string Path { get; set; }

    }
}