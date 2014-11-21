using System;
using CustomerTracker.Web.Angular.Infrastructure.Membership;
using CustomerTracker.Web.Angular.Models.ViewModels;

namespace CustomerTracker.Web.Angular.Business.AccountBusiness
{
    public class AccountMapper : IAccountMapper
    {
        private ISecurityEncoder _securityEncoder;

        public AccountMapper(ISecurityEncoder securityEncoder)
        {
            _securityEncoder = securityEncoder;
        }

        public RegisterExternalLoginModel MapRegisterExternalLoginModelFromAuthenticationResult(string provider, string providerUserId, string userName)
        {
            var loginData = _securityEncoder.SerializeOAuthProviderUserId(provider, providerUserId);

            var registerExternalLoginModel = new RegisterExternalLoginModel
                {
                    ExternalLoginData = loginData
                };

            switch (provider.ToLower())
            {
                case "google":
                
                    registerExternalLoginModel.UserName = userName.Substring(0, userName.IndexOf("@"));

                    registerExternalLoginModel.Email = userName;

                    break;

                default:
                    throw new Exception("no found provider");

            }

            return registerExternalLoginModel;
        }
    }

    public interface IAccountMapper
    {
        RegisterExternalLoginModel MapRegisterExternalLoginModelFromAuthenticationResult(string provider, string providerUserId, string userName);
    }
}