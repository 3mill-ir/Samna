using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BigMill.Models
{
    public class PollAnswerModel
    {
        [Display(ResourceType = typeof(Resource.Resource), Name = "PollAnswer_ID")]
   
        public int ID { get; set; }



        [Display(ResourceType = typeof(Resource.Resource), Name = "PollAnswer_Text")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public string AnswerText { get; set; }



        [Display(ResourceType = typeof(Resource.Resource), Name = "PollAnswer_AnswerKey")]
  
        public int AnswerKey { get; set; }



        [Display(ResourceType = typeof(Resource.Resource), Name = "PollAnswer_Score")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public int Score { get; set; }



        [Display(ResourceType = typeof(Resource.Resource), Name = "PollAnswer_F_PollQuestion_Id")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public int F_PollQusetion_Id { get; set; }

        public bool IsChecked { get; set; }
    }
}