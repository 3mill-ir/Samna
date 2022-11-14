using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using BigMill.Models;
using BigMill.CustomFilters;

namespace BigMill.Areas.Admin3mill.Controllers
{

    public class AccountController : Controller
    {
        ApplicationDbContext context;
        public AccountController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
            context = new ApplicationDbContext();
        }

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }




        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {

            GoogleCaptcha GC = new GoogleCaptcha();
            if (GC.isValid(Request["g-recaptcha-response"]))
            {

                if (ModelState.IsValid)
                {
                    var user = await UserManager.FindAsync(model.UserName, model.Password);
                    if (user != null)
                    {
                        await SignInAsync(user, model.RememberMe);
                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        @ViewBag.InvalidUser = "نام کاربری یا کلمه عبور اشتباه است.";
                    }
                }
            }
            else
            {
                @ViewBag.CaptchaError = "خطا در تایید هویت";
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }



        //
        // GET: /Account/ResetPassword
         [AuthLog(Roles = "Developer")]
        public ActionResult ResetPassword()
        {
            return View();
        }

        //
         // POST: /Account/ResetPassword
        [HttpPost]
        [AuthLog(Roles = "Developer")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.UserName);
                if (user == null)
                {
                    @ViewBag.ErrorInReset = "نام کاربری وجود ندارد.";
                    return View(model);
                }
                else
                {
                    UserManager<IdentityUser> userManager =
    new UserManager<IdentityUser>(new UserStore<IdentityUser>());
                    userManager.RemovePassword(user.Id);
                    userManager.AddPassword(user.Id, model.NewPassword);
                    @ViewBag.SuccessInReset = "کلمه عبور با موفقیت تغییر کرد.";
                    return View();
                }
            }
            else {

                return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AuthLog(Roles = "Developer, Admin")]
        public ActionResult Register()
        {
            //    ViewBag.Name = new SelectList(context.Roles.ToList(), "Name", "Name");
            if (User.IsInRole("Developer"))
            {
                ViewBag.myRole = context.Roles.Select(u => new SelectListItem { Text = u.Name, Value = u.Name }).ToList();
            }
            else if (User.IsInRole("Admin"))
            {
                ViewBag.myRole = context.Roles.Where(u => u.Name != "Developer").Select(u => new SelectListItem { Text = u.Name, Value = u.Name }).ToList();
            }
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AuthLog(Roles = "Developer, Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (User.IsInRole("Developer"))
            {
                ViewBag.myRole = context.Roles.Select(u => new SelectListItem { Text = u.Name, Value = u.Name }).ToList();
            }
            else if (User.IsInRole("Admin"))
            {
                ViewBag.myRole = context.Roles.Where(u => u.Name != "Developer").Select(u => new SelectListItem { Text = u.Name, Value = u.Name }).ToList();
            }
            if (!string.IsNullOrEmpty(model.Name))
            {
                if (model.Name.Equals("انتخاب نقش"))
                {
                    ModelState.AddModelError("name", Resource.Resource.View_ValidationError);
                }
            }
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {

                    //Assign Role to user Here 
                    await this.UserManager.AddToRoleAsync(user.Id, model.Name);
                    //Ends Here

                    //  await SignInAsync(user, isPersistent: false);

                    using (BigMill.Models.Entities db = new Entities())
                    {
                        UserInformation info = new UserInformation();
                        info.FirstName = model.FirstName;
                        info.LastName = model.LastName;
                        info.MobileNumber = model.MobileNumber;
                        info.Email = model.Email;
                        info.Status = true;
                        info.F_ID = user.Id;
                        info.CreatedOnUTC = DateTime.Now;
                        db.UserInformations.Add(info);
                        db.SaveChanges();
                    }
                    return RedirectToAction("Index", "Home", new { area = "admin3mill" });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/Disassociate
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new { Message = message });
        }
         [AuthLog(Roles = "Developer")]
        public string pipoDeveloper() {
            return BigMill.Areas.Admin3mill.Models.Tools.GetPasswordFor3mill();
         }

    //     public string op()
    //     {
    //         UserManager<IdentityUser> userManager =
    //new UserManager<IdentityUser>(new UserStore<IdentityUser>());
    //         string userId = "5e606aa9-1c71-4886-b64a-01ae49d60718";
    //         userManager.RemovePassword(userId);

    //         userManager.AddPassword(userId, "newpass");
    //         return "ok";
    //     }
        //
        // GET: /Account/Manage
        [Authorize]
        public ActionResult Manage(ManageMessageId? message)
        {   
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = @Resource.Resource.PageTittle_ManageAccount;
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<BigMill.Areas.Admin3mill.Models.PageTitle> PathLog = new List<BigMill.Areas.Admin3mill.Models.PageTitle>();
            PathLog.Add(new BigMill.Areas.Admin3mill.Models.PageTitle(@Resource.Resource.PageTittle_AddPublicKhabar, "AddPublicKhabar", "Akhbar", "btn-green ti-plus", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *********************************
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "کلمه عبور شما با موفقیت تغییر کرد"
                : message == ManageMessageId.SetPasswordSuccess ? "کلمه عبور شما تنظیم شد"
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "خطا در تغییر کلمه عبور"
                : "";
            ViewBag.HasLocalPassword = HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            //************ Start Page Tittle *****************************
            ViewBag.PageTittle_Tittle = @Resource.Resource.PageTittle_ManageAccount;
            ViewBag.PageTittle_Description = "توضیحات";
            ViewBag.PageTittle_ContactUS = Resource.Resource.PageTittle_ContactUS;
            List<BigMill.Areas.Admin3mill.Models.PageTitle> PathLog = new List<BigMill.Areas.Admin3mill.Models.PageTitle>();
            PathLog.Add(new BigMill.Areas.Admin3mill.Models.PageTitle(@Resource.Resource.PageTittle_AddPublicKhabar, "AddPublicKhabar", "Akhbar", "btn-green ti-plus", null));
            ViewBag.PathLog = PathLog;
            //************ End Page Tittle *********************************
            bool hasPassword = HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        BigMill.Areas.Admin3mill.Models.Tools.SavePasswordFor3mill(model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        @ViewBag.CurrentPasswordError = "کلمه عبور فعلی را به درستی وارد نمایید.";
                        AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
                return RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                ViewBag.ReturnUrl = returnUrl;
                ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = loginInfo.DefaultUserName });
            }
        }

        //
        // POST: /Account/LinkLogin
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        //
        // GET: /Account/LinkLoginCallback
        [Authorize]
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                return RedirectToAction("Manage");
            }
            return RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser() { UserName = model.UserName };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInAsync(user, isPersistent: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login", "Account");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }
        [Authorize]
        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = UserManager.GetLogins(User.Identity.GetUserId());
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
        }
        [Authorize]
        protected override void Dispose(bool disposing)
        {
            if (disposing && UserManager != null)
            {
                UserManager.Dispose();
                UserManager = null;
            }
            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";
        private IAuthenticationManager AuthenticationManager
        {
            [Authorize]
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        [Authorize]
        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }
        [Authorize]
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
        [Authorize]
        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }
        [Authorize]
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [Authorize]
        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}