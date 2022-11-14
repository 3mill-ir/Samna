using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BigMill.Models
{
    public class PostCommentModel
    {
        public PostCommentModel()
        {
            Replies=new List<PostCommentReplyModel>();
        }
        [Display(ResourceType = typeof(Resource.Resource), Name = "PostComment_ID")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public int ID { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "PostComment_Text")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public string Text { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "PostComment_CreateOnUtc")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public DateTime CreateOnUtc { get; set; }
        public string CreateDateUtcJalali { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "PostComment_Display")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public bool Display { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "PostComment_NumberOfReply")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public int NumberOfReply { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "PostComment_NumberOfLikes")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public int NumberOfLikes { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "PostComment_NumberOfDislikes")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public int NumberOfDislikes { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "PostComment_F_Posts_Id")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public int F_Posts_Id { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "PostComment_F_Posts_Id")]
        public List<PostCommentReplyModel> Replies { get; set; }
        
    }
}