using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BigMill.Models
{
    public class PostCategoryModel
    {
        public PostCategoryModel()
        {
            Tags = new List<string>();
        }

        [Display(ResourceType = typeof(Resource.Resource), Name = "PostCategory_ID")]
        public int ID { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "PostCategory_Text")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public string Text { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "PostCategory_Status")]

        public bool Status { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "PostCategory_Weight")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public double Weight { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "PostCategory_isView")]
        public string IsView { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "PostCategory_SeoName")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public string SeoName { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "PostCategory_Tags")]
        public List<string> Tags { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "PostCategory_Tags")]
        public string TagsText { get; set; }

    }
}