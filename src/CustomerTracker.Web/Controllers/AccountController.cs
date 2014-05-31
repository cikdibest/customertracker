using System;
using System.Transactions;
using System.Web.Mvc;
using System.Web.Security;
using CustomerTracker.Web.App_Start;
using CustomerTracker.Web.Business.AccountBusiness;
using CustomerTracker.Web.Business.UserBusiness;
using CustomerTracker.Web.Infrastructure.Membership;
using CustomerTracker.Web.Models.ViewModels;
using CustomerTracker.Web.Utilities;
using Microsoft.Web.WebPages.OAuth;
using Ninject;

namespace CustomerTracker.Web.Controllers
{
    [Authorize]
    //[InitializeSimpleMembership]
    public class AccountController : Controller
    {
        private ICustomOAuthProvider _customOAuthProvider;

        private ISecurityEncoder _securityEncoder;

        private IUserUtility _userUtility;

        private IAccountMapper _accountMapper;

        private IMailBuilder _mailBuilder;

        private IMailSenderUtility _mailSenderUtility;

        private IPasswordCreater _passwordCreater;

        public AccountController()
        {
            _customOAuthProvider = new CustomOAuthProvider();

            _securityEncoder = new SecurityEncoder();

            _userUtility = NinjectWebCommon.GetKernel.Get<IUserUtility>();

            _accountMapper = new AccountMapper(_securityEncoder);

            _mailBuilder = new MailBuilder();

            _mailSenderUtility = new MailSenderUtility();

            _passwordCreater = new PasswordCreater();
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            throw new Exception("Disassociate not implemented");

            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == User.Identity.Name)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("Manage", new { Message = message });
        }

        public ActionResult Manage(ManageMessageId? message)
        {
            return View();
            throw new Exception("Disassociate not implemented");

            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ExternalLoginResult(provider, Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));
        }

        [AllowAnonymous]
        public ActionResult ExternalLoginCallback(string returnUrl)
        {
            var authenticationResult = _customOAuthProvider.VerifyOAuthAuthentication(Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl }));

            if (!authenticationResult.IsSuccessful)
                return RedirectToAction("ExternalLoginFailure");

            if (_customOAuthProvider.OAuthLogin(authenticationResult.Provider, authenticationResult.ProviderUserId, persistCookie: false))
                return RedirectToLocal(returnUrl);

            if (User.Identity.IsAuthenticated)
            {
                _userUtility.AddSocialAccountToUser(authenticationResult.Provider, authenticationResult.ProviderUserId, User.Identity.Name);

                return RedirectToLocal(returnUrl);
            }

            ViewBag.ProviderDisplayName = _customOAuthProvider.GetOAuthClientData(authenticationResult.Provider).DisplayName;

            ViewBag.ReturnUrl = returnUrl;

            return View("ExternalLoginConfirmation", _accountMapper.MapRegisterExternalLoginModelFromAuthenticationResult(authenticationResult.Provider, authenticationResult.ProviderUserId, authenticationResult.UserName));
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLoginConfirmation(RegisterExternalLoginModel model, string returnUrl)
        {
            string provider;

            string providerUserId;

            if (User.Identity.IsAuthenticated || !_securityEncoder.TryDeserializeOAuthProviderUserId(model.ExternalLoginData, out provider, out providerUserId))
                return RedirectToAction("Manage");

            if (ModelState.IsValid)
            {
                var socialUserRegisterModel = new SocialUserRegisterModel()
                    {
                        Email = model.Email,
                        UserName = model.UserName,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Provider = provider,
                        ProviderUserId = providerUserId,
                        Password = _passwordCreater.Create()
                    };

                _userUtility.CreateUserWithSocialAccount(socialUserRegisterModel);

                bool isSuccessLogin = _customOAuthProvider.OAuthLogin(provider, providerUserId, false);

                if (isSuccessLogin)
                {
                    SendMail(socialUserRegisterModel);

                    return RedirectToLocal(returnUrl);
                }

                return View("Login");
            }

            ViewBag.ProviderDisplayName = _customOAuthProvider.GetOAuthClientData(provider).DisplayName;

            ViewBag.ReturnUrl = returnUrl;

            return View(model);
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

        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        [AllowAnonymous]
        [ChildActionOnly]
        public ActionResult ExternalLoginsList(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return PartialView("_ExternalLoginsListPartial", _customOAuthProvider.RegisteredClientData);
        }

        [ChildActionOnly]
        public ActionResult RemoveExternalLogins()
        {
            throw new Exception("RemoveExternalLogins not implemented");

            //ICollection<OAuthAccount> accounts = OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name);
            //List<ExternalLogin> externalLogins = new List<ExternalLogin>();
            //foreach (OAuthAccount account in accounts)
            //{
            //    AuthenticationClientData clientData = OAuthWebSecurity.GetOAuthClientData(account.Provider);

            //    externalLogins.Add(new ExternalLogin
            //    {
            //        Provider = account.Provider,
            //        ProviderDisplayName = clientData.DisplayName,
            //        ProviderUserId = account.ProviderUserId,
            //    });
            //}

            //ViewBag.ShowRemoveButton = externalLogins.Count > 1 || OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            //return PartialView("_RemoveExternalLoginsPartial", externalLogins);
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Material");
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        internal class ExternalLoginResult : ActionResult
        {
            private ICustomOAuthProvider _customOAuthProvider;

            public ExternalLoginResult(string provider, string returnUrl)
            {
                _customOAuthProvider = new CustomOAuthProvider();

                Provider = provider;

                ReturnUrl = returnUrl;
            }

            public string Provider { get; private set; }

            public string ReturnUrl { get; private set; }

            public override void ExecuteResult(ControllerContext context)
            {
                _customOAuthProvider.RequestOAuthAuthentication(Provider, ReturnUrl);
            }
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


