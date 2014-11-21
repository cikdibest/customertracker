using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CustomerTracker.Web.Angular.Infrastructure.Membership;
using CustomerTracker.Web.Angular.Infrastructure.Repository;
using CustomerTracker.Web.Angular.Models;

namespace CustomerTracker.Web.Angular.Utilities
{
    public class ConfigurationHelper
    {

        public static IUnitOfWork UnitOfWorkInstance
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    return null;
                }

                return (IUnitOfWork)HttpContext.Current.Items["UnitOfWorkInstance"];
            }
            set
            {
                HttpContext.Current.Items["UnitOfWorkInstance"] = value;
            }
        }

        public static UserPrincipal CurrentUser
        {
            get
            {
                var user = HttpContext.Current.User;

                if (user == null)
                {
                    return null;
                }

                return user as UserPrincipal;
            }

        }

        public static string FromMailAddressForContactRequest { get { return ConfigurationManager.AppSettings["FromMailAddressForContactRequest"]; } }

        public static string ToMailAddressForContactRequest { get { return ConfigurationManager.AppSettings["ToMailAddressForContactRequest"]; } }

        public static string FromMailAddressForUserRegistration { get { return ConfigurationManager.AppSettings["FromMailAddressForUserRegistration"]; } }

        public static string SmtpHost { get { return ConfigurationManager.AppSettings["SmtpHost"]; } }
        public static int SmtpPort { get { return int.Parse(ConfigurationManager.AppSettings["SmtpPort"]); } }
        public static string SmtpUserName { get { return ConfigurationManager.AppSettings["SmtpUserName"]; } }
        public static string SmtpPassword { get { return ConfigurationManager.AppSettings["SmtpPassword"]; } }


        public static string RoleAdmin = "Admin";

        public static string RolePersonel = "Personel";

        public static string RoleCustomer = "Customer";

        public static string RoleAnonymous = "Anonymous";


    }
}