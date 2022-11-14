using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BigMill.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        [DataType(DataType.Password)]
        [Display(Name = "کلمه عبور فعلی")]
        public string OldPassword { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        [StringLength(100, ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError_Password", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "کلمه عبور جدید")]
        public string NewPassword { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "تکرار کلمه عبور جدید")]
        [System.Web.Mvc.Compare("NewPassword", ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError_ConfirmPassword")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        [Display(Name = "User name")]
        public string UserName { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        [Display(Name = "Roll")]
        public string Name { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        [StringLength(100, ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError_Password", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.Web.Mvc.Compare("Password", ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError_ConfirmPassword")]
        public string ConfirmPassword { get; set; }


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
    }

    public class GoogleCaptcha
    {

        public bool isValid(string Payload)
        {
            var response = Payload;// 
            //secret that was generated in key value pair
            const string secret = "6LdY8AcUAAAAABJ0lfjKHh_a4b7M-zouo2w4qSOl";

            var client = new System.Net.WebClient();
            var reply =
                client.DownloadString(
                    string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));

            var captchaResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<CaptchaResponse>(reply);

            //when response is false check for the error message
            if (captchaResponse.Success)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        class CaptchaResponse
        {
            [Newtonsoft.Json.JsonProperty("success")]
            public bool Success { get; set; }

            [Newtonsoft.Json.JsonProperty("error-codes")]
            public List<string> ErrorCodes { get; set; }
        }
    }

    public class ResetPasswordViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        [Display(Name = "نام کاربری")]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError")]
        [StringLength(100, ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError_Password", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "کلمه عبور جدید")]
        public string NewPassword { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "تکرار کلمه عبور جدید")]
        [System.Web.Mvc.Compare("NewPassword", ErrorMessageResourceType = typeof(Resource.Resource), ErrorMessageResourceName = "View_ValidationError_ConfirmPassword")]
        public string ConfirmPassword { get; set; }
    }
}
