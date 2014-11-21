using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;
using CustomerTracker.Data.Model.Entities;
using CustomerTracker.Web.Angular.Business.UserBusiness;
using CustomerTracker.Web.Angular.Infrastructure.Repository;
using CustomerTracker.Web.Angular.Models.ViewModels;
using CustomerTracker.Web.Angular.Utilities;
using Ninject;

namespace CustomerTracker.Web.Angular.Infrastructure.Membership
{
    public sealed class WebSecurity
    {
        public static HttpContextBase Context
        {
            get { return new HttpContextWrapper(HttpContext.Current); }
        }

        public static HttpRequestBase Request
        {
            get { return Context.Request; }
        }

        public static HttpResponseBase Response
        {
            get { return Context.Response; }
        }

        public static System.Security.Principal.IPrincipal User
        {
            get
            {
                return Context.User;
            }
        }

        public static bool IsAuthenticated
        {
            get { return User.Identity.IsAuthenticated; }
        }

        //public static MembershipCreateStatus Register(string username, string password, string email, bool isApproved, string firstName, string lastName)
        //{
        //    MembershipCreateStatus createStatus;

        //    Membership.CreateUser(username, password, email, null, null, isApproved, Guid.NewGuid(), out createStatus);

        //    if (createStatus == MembershipCreateStatus.Success)
        //    {
        //        var ConfigurationHelper.UnitOfWorkInstance = new UnitOfWork<JewelryDataContext>();

        //        var repositoryUser = ConfigurationHelper.UnitOfWorkInstance.GetRepository<User>();

        //        using (ConfigurationHelper.UnitOfWorkInstance)
        //        {
        //            var user = repositoryUser.SelectAll().FirstOrDefault(usr => usr.Username == username);

        //            user.FirstName = firstName;

        //            user.LastName = lastName;

        //            ConfigurationHelper.UnitOfWorkInstance.Save();
        //        }

        //        if (isApproved)
        //        {
        //            FormsAuthentication.SetAuthCookie(username, false);
        //        }
        //    }

        //    return createStatus;
        //}

        public static Boolean Login(string username, string password, bool persistCookie = false)
        {
            var success = System.Web.Security.Membership.ValidateUser(username, password);

            if (success)
            {
                //FormsAuthentication.SetAuthCookie(username, persistCookie);

                var repositoryUser = ConfigurationHelper.UnitOfWorkInstance.GetRepository<User>();

                var user = repositoryUser.SelectAll().FirstOrDefault(usr => usr.Username == username);

                CreateCookieWithUser(user);
            }

            return success;

        }

        public static void CreateCookieWithUser(User user)
        {
            var serializeModel = new UserPrincipalSerializeModel
                {
                    UserId = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    UserName = user.Username,
                    Roles = user.Roles.Select(q => q.RoleName).ToArray()
                };

            var serializer = new JavaScriptSerializer();

            var userData = serializer.Serialize(serializeModel);

            var authTicket = new FormsAuthenticationTicket(
                     1,
                     user.Username,
                     DateTime.Now,
                     DateTime.Now.AddMinutes(60),
                     false,
                     userData);

            var encTicket = FormsAuthentication.Encrypt(authTicket);

            var faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);

            Response.Cookies.Add(faCookie);
        }

        public static Boolean Login(User user, bool persistCookie = false)
        {
            return Login(user.Username, user.Password, persistCookie);
        }

        public static void Logout()
        {
            FormsAuthentication.SignOut();
        }

        public static MembershipUser GetUser(string username)
        {
            return System.Web.Security.Membership.GetUser(username);
        }

        public static bool ChangePassword(string userName, string currentPassword, string newPassword)
        {
            bool success = false;
            try
            {
                MembershipUser currentUser = System.Web.Security.Membership.GetUser(userName, true);

                success = currentUser.ChangePassword(currentPassword, newPassword);
            }
            catch (ArgumentException)
            {

            }

            return success;
        }

        public static bool DeleteUser(string username)
        {
            return System.Web.Security.Membership.DeleteUser(username);
        }

        public static int GetUserId(string userName)
        {
            MembershipUser user = System.Web.Security.Membership.GetUser(userName);

            return (int)user.ProviderUserKey;
        }

        public static User CreateUser(RegisterModel registerModel, string roleName)
        {
            var userUtility = NinjectWebCommon.GetKernel.Get<IUserUtility>();

            var user = userUtility.MapUserFromRegisterModel(registerModel);

            var repository = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Role>();

            var role = repository.Filter(q => q.RoleName == roleName).SingleOrDefault();

            user.AddRole(role);

            return userUtility.CreateUser(user);
        }

        public static List<MembershipUser> FindUsersByEmail(string email, int pageIndex, int pageSize)
        {
            int totalRecords;
            return System.Web.Security.Membership.FindUsersByEmail(email, pageIndex, pageSize, out totalRecords).Cast<MembershipUser>().ToList();
        }

        public static List<MembershipUser> FindUsersByName(string username, int pageIndex, int pageSize)
        {
            int totalRecords;

            return System.Web.Security.Membership.FindUsersByName(username, pageIndex, pageSize, out totalRecords).Cast<MembershipUser>().ToList();
        }

        public static List<MembershipUser> GetAllUsers(int pageIndex, int pageSize)
        {
            int totalRecords;
            return System.Web.Security.Membership.GetAllUsers(pageIndex, pageSize, out totalRecords).Cast<MembershipUser>().ToList();
        }

        public static void InitializeDatabaseConnection(string connectionStringName, string userTableName, string userIdColumn, string userNameColumn, bool autoCreateTables)
        {

        }

        public static void InitializeDatabaseConnection(string connectionString, string providerName, string userTableName, string userIdColumn, string userNameColumn, bool autoCreateTables)
        {

        }

        public static void ReConfigureRoles(User user, List<int> roleIdList)
        {
            //if (this.Roles == null)
            //    this.Roles = new List<Role>();

            var deletedRoles = user.Roles.Where(q => !roleIdList.Contains(q.Id)).ToList();

            foreach (var deletedRole in deletedRoles)
            {
                user.Roles.Remove(deletedRole);
            }

            foreach (var roleId in roleIdList)
            {
                if (user.Roles.Any(q => q.Id == roleId)) continue;

                var repositoryRole = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Role>();

                var role = repositoryRole.Find(q => q.Id == roleId);

                user.Roles.Add(role);
            }


        }
    }
}