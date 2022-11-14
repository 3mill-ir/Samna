using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BigMill.Models
{
    public class TagModel
    {
        [Display(ResourceType = typeof(Resource.Resource), Name = "Tag_ID")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public int ID { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "Tag_Text")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public string Text { get; set; }
    }
}