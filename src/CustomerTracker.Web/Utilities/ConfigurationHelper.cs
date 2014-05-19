using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CustomerTracker.Web.Infrastructure.Membership;
using CustomerTracker.Web.Infrastructure.Repository;
using CustomerTracker.Web.Models;
using CustomerTracker.Web.Models.Entities;

namespace CustomerTracker.Web.Utilities
{
    public class ConfigurationHelper
    {
       
        public static IUnitOfWork UnitOfWorkInstance
        {
            get
            {

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
                    throw new ArgumentNullException("user is null");
                }

                return user as UserPrincipal;
            }

        }

        public static string FromMailAddressForContactRequest { get { return ConfigurationManager.AppSettings["FromMailAddressForContactRequest"]; } }

        public static string ToMailAddressForContactRequest { get { return ConfigurationManager.AppSettings["ToMailAddressForContactRequest"]; } }

        public static string FromMailAddressForUserRegistration { get { return ConfigurationManager.AppSettings["FromMailAddressForUserRegistration"]; } }

        public static string RoleAdmin = "Admin";

        public static string RolePersonel = "Personel";

        public static string RoleCustomer = "Customer";
         
       
    }
}