using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BigMill.Models
{
    public class PostCommentReplyModel
    {
        [Display(ResourceType = typeof(Resource.Resource), Name = "PostCommentReply_ID")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public int ID { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "PostCommentReply_Text")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public string Text { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "PostCommentReply_CreateOnUTC")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public DateTime CreateOnUTC{ get; set; }
        public string CreateDateUtcJalali { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "PostCommentReply_NumberOfLikes")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public int NumberOfLikes { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "PostCommentReply_NumberOfDislikes")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public int NumberOfDislikes { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "PostCommentReply_F_PostsComments_Id")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public int F_PostsComments_Id { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "PostCommentReply_Display")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public bool Display { get; set; }
    }
}