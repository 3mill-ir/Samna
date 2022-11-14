using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BigMill.Models
{
    public class RollModel
    {
        [Display(ResourceType = typeof(Resource.Resource), Name = "Roll_Description")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public string Description { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "Roll_Value")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public string Value { get; set; }
    }
}