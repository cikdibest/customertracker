using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerTracker.Web.Models.Entities
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmailAddress { get; set; }

        public string Username { get; set; }
    }
}