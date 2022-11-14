using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BigMill.Models
{
    public class PollQuestionModel
    {
        public PollQuestionModel()
        {
            AnswerBoxes = new List<PollAnswerModel>();
        }
        [Display(ResourceType = typeof(Resource.Resource), Name = "PollQuestion_ID")]

        public int ID { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "PollQuestion_Question")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public string Question { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "PollQuestion_CreatedOnUTC")]
        public DateTime CreatedOnUTC { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "PollQuestion_CreatedOnUTC")]
        public string CreatedOnUTCJalali { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "PollQuestion_StartedOnUTC")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public string StartDateOnUTC { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "PollQuestion_EndDateUTC")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public string EndDateOnUTC { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "PollQuestion_F_User_Id")]
        public int F_User_Id { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "PollQuestion_AnswerBoxes")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public List<PollAnswerModel> AnswerBoxes{get;set;}


        [Display(ResourceType = typeof(Resource.Resource), Name = "PollQuestion_Status")]
        public bool Status { get; set; }

        [Display(ResourceType = typeof(Resource.Resource), Name = "PollQuestion_Active")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public bool Active { get; set; }


    }
}