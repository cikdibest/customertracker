using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Security;
using CustomerTracker.Web.Infrastructure.Membership;
using CustomerTracker.Web.Utilities;

namespace CustomerTracker.Web.Models.Attributes
{
     
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private const string BasicAuthResponseHeader = "WWW-Authenticate";
        private const string BasicAuthResponseHeaderValue = "Basic";

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {
                if (ConfigurationHelper.CurrentUser == null)
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                    actionContext.Response.Headers.Add(BasicAuthResponseHeader, BasicAuthResponseHeaderValue);
                    return;
                }
                 
                var currentUser = ConfigurationHelper.CurrentUser;

                if (!String.IsNullOrEmpty(Roles))
                {
                    if (!currentUser.IsInRole(Roles.Split(',')))
                    {
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
                        actionContext.Response.Headers.Add(BasicAuthResponseHeader, BasicAuthResponseHeaderValue);
                        return;
                    }
                }

                if (!String.IsNullOrEmpty(Users))
                {
                    if (!Users.Contains(currentUser.UserName))
                    {
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden);
                        actionContext.Response.Headers.Add(BasicAuthResponseHeader, BasicAuthResponseHeaderValue);
                        return;
                    }
                }
            }
            catch (Exception)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                actionContext.Response.Headers.Add(BasicAuthResponseHeader, BasicAuthResponseHeaderValue);
                return;

            }
        }

        private Credentials ParseAuthorizationHeader(string authHeader)
        {
            string[] credentials = Encoding.ASCII.GetString(Convert.FromBase64String(authHeader)).Split(new[] { ':' });

            if (credentials.Length != 2 || string.IsNullOrEmpty(credentials[0]) || string.IsNullOrEmpty(credentials[1]))
                return null;

            return new Credentials() { Username = credentials[0], Password = credentials[1], };
        }
    }
    //Client credential
    public class Credentials
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}