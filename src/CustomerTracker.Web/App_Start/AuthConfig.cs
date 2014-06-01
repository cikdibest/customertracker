using System.Collections.Generic;
using DotNetOpenAuth.AspNet.Clients;
using CustomerTracker.Web.Infrastructure.Membership;
using Microsoft.Web.WebPages.OAuth;

namespace CustomerTracker.Web.App_Start
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            //CustomOAuthProvider.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            //CustomOAuthProvider.RegisterTwitterClient(
            //    consumerKey: "",
            //    consumerSecret: "");

            CustomOAuthProvider.RegisterClient(
                new FacebookClient("326919827459603", "e527fd3bf55d721745fe81dd4c592ff6"), "Facebook",
                new Dictionary<string, object>());
                
            //CustomOAuthProvider.RegisterGoogleClient();

            CustomOAuthProvider.RegisterClient(
              new GoogleOpenIdClient(),
              "Google", new Dictionary<string, object>());
        }
    }
}
