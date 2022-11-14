using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BigMill.Models
{
    public class UserInformationModel
    {
        [Display(ResourceType = typeof(Resource.Resource), Name = "UserInformation_ID")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public int ID { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "UserInformation_FirstName")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public string FirstName { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "UserInformation_LastName")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public string LastName { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "UserInformation_MobileNumber")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public string MobileNumber { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "UserInformation_Email")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public string Email { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "UserInformation_Status")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public bool Status { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "UserInformation_CreatedOnUTC")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public DateTime CreatedOnUTC { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "UserInformation_UpdatedOnUTC")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public DateTime UpdatedOnUTC { get; set; }


        [Display(ResourceType = typeof(Resource.Resource), Name = "UserInformation_F_ID")]
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        public string F_ID { get; set; }
    }
}