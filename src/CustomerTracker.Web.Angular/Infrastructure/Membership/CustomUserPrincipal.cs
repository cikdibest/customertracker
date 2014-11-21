using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using CustomerTracker.Web.Angular.Infrastructure.Repository;
using CustomerTracker.Web.Angular.Utilities;
using Ninject;

namespace CustomerTracker.Web.Angular.Infrastructure.Membership
{

    public interface IUserPrincipal : IPrincipal
    {
        int UserId { get; set; }

        string UserName { get; set; }

        string FirstName { get; set; }

        string LastName { get; set; }

    }

    public class UserPrincipal : IUserPrincipal
    {
        public bool IsInRole(string roleName)
        {
            return  this.Roles.Contains(roleName);
        }

        public bool IsInRole(string[] roleNames)
        {
            return roleNames.Any(roleName => this.Roles.Contains(roleName));
        }

        public IIdentity Identity { get; private set; }

        public UserPrincipal(FormsAuthenticationTicket authTicket)
        {
            this.Identity = new FormsIdentity(authTicket);
        }

        public UserPrincipal(string name)
        {
            this.Identity = new GenericIdentity(name);
        }

        public int UserId { get; set; }

        public string UserName
        { get; set; }

        public string FirstName
        { get; set; }

        public string LastName
        { get; set; }

        public string[] Roles { get; set; }
    }

    public class UserPrincipalSerializeModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string[] Roles { get; set; }
    }

    //not used(optional)
    public class UserIdentity : IIdentity
    {
        public string Name { get; private set; }

        public string AuthenticationType { get; private set; }

        public bool IsAuthenticated { get; private set; }
    }


}