using System;
using System.Transactions;
using System.Web.Mvc;
using System.Web.Security;
using CustomerTracker.Web.Angular.Business.AccountBusiness;
using CustomerTracker.Web.Angular.Business.UserBusiness;
using CustomerTracker.Web.Angular.Infrastructure.Membership;
using CustomerTracker.Web.Angular.Models.ViewModels;
using CustomerTracker.Web.Angular.Utilities;
using Microsoft.Web.WebPages.OAuth;
using Ninject;

namespace CustomerTracker.Web.Angular.Controllers
{
    [Authorize]
    //[InitializeSimpleMembership]
    public class AccountController : Controller
    {
        private ISecurityEncoder _securityEncoder;

        private IMailBuilder _mailBuilder;

        private IMailSenderUtility _mailSenderUtility;

        public AccountController()
        { 
            _securityEncoder = new SecurityEncoder();
             
            _mailBuilder = new MailBuilder();

            _mailSenderUtility = new MailSenderUtility();
             
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            //DummyDataGenerate.Generate();

            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
                return RedirectToLocal(returnUrl);

            ModelState.AddModelError("", "The user name or password provided is incorrect.");

            return View(model);
        }

        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToLocal(null);
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    WebSecurity.CreateUser(registerModel, ConfigurationHelper.RolePersonel);

                    SendMail(registerModel);

                    WebSecurity.Login(registerModel.UserName, registerModel.Password);

                    return RedirectToLocal(null);
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }

            return View(registerModel);
        }
         
        private void SendMail(RegisterModel registerModel)
        {
            var sendToUserAfterRegistration = new SendToUserAfterRegistrationMailViewModel()
            {
                FullName = registerModel.FirstName + " " + registerModel.LastName,
                UserMailAdress = registerModel.Email
            };

            var mailMessageForUser = _mailBuilder.BuildMailMessageForSendToUserAfterRegistration(sendToUserAfterRegistration);

            _mailSenderUtility.SendEmailAsync(mailMessageForUser);

        }
        
        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

       

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion


        
    }
}


