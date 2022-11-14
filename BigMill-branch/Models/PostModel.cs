using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BigMill.Models
{
    public class PostModel
    {
       public PostModel()
        {
            Tags = new List<string>();
            Comments = new List<PostCommentModel>();
        }
       [Display(ResourceType = typeof(Resource.Resource), Name = "Post_ID")]
        public int ID { get; set; }


       [Display(ResourceType = typeof(Resource.Resource), Name = "Post_Content_One")]
       [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public string Content_One { get; set; }


       [Display(ResourceType = typeof(Resource.Resource), Name = "Post_Content_Two")]
       public string Content_Two { get; set; }


       [Display(ResourceType = typeof(Resource.Resource), Name = "Post_Content_Three")]
       [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public string Content_Three { get; set; }

        [AllowHtml]
       [Display(ResourceType = typeof(Resource.Resource), Name = "Post_Content_Four")]
       [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public string Content_Four { get; set; }


       [Display(ResourceType = typeof(Resource.Resource), Name = "Post_DisplayNumberOfVisitors")]

        public int NumberOfVisitors { get; set; }


       [Display(ResourceType = typeof(Resource.Resource), Name = "Post_DisplayNumberOfComments")]

        public int NumberOfComments { get; set; }


       [Display(ResourceType = typeof(Resource.Resource), Name = "Post_NumberOfLikes")]

        public int NumberOfLikes { get; set; }


       [Display(ResourceType = typeof(Resource.Resource), Name = "Post_NumberOfDisLikes")]

        public int NumberOfDisLikes { get; set; }


       [Display(ResourceType = typeof(Resource.Resource), Name = "Post_Display")]
       [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public bool Display { get; set; }


       [Display(ResourceType = typeof(Resource.Resource), Name = "Post_Status")]
        public bool Status { get; set; }


       [Display(ResourceType = typeof(Resource.Resource), Name = "Post_F_PostsSubCategory_ID")]
        public int F_PostsSubCategory_ID { get; set; }


       [Display(ResourceType = typeof(Resource.Resource), Name = "Post_F_Users_Id")]
        public int F_Users_Id { get; set; }


       [Display(ResourceType = typeof(Resource.Resource), Name = "Post_CreateDateUtc")]
        public DateTime CreateDateUtc { get; set; }

       public string CreateDateUtcJalali { get; set; }


       [Display(ResourceType = typeof(Resource.Resource), Name = "Post_Tags")]
        public List<string> Tags { get; set; }


       [Display(ResourceType = typeof(Resource.Resource), Name = "Post_TagsText")]
        public string TagsText { get; set; }


       [Display(ResourceType = typeof(Resource.Resource), Name = "Post_Comments")]
       public List<PostCommentModel> Comments { get; set; }
       public string Comment { get; set; }
       public string Reply { get; set; }

       public HttpPostedFileBase Video { get; set; }

    }
}