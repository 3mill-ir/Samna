using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BigMill.Models
{
    public class PostSubCategoryModel
    {
        public PostSubCategoryModel()
        {
            Tags = new List<string>();
        }
        [Display(ResourceType = typeof(Resource.Resource), Name = "PostSubCategory_ID")]

        public int ID { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "PostSubCategory_Text")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public string Text { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "PostSubCategory_Status")]

        public bool Status { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "PostSubCategory_Weight")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public double Weight { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "PostSubCategory_F_PostCategory_ID")]
        public int F_PostCategory_ID { get; set; }



        [Display(ResourceType = typeof(Resource.Resource), Name = "PostSubCategory_Tags")]

        public List<string> Tags { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "PostSubCategory_TagsText")]

        public string TagsText { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "PostSubCategory_SeoName")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public string SeoName { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "PostSubCategory_IsView")]
        public string IsView { get; set; }
    }
}