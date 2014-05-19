using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CustomerTracker.Web.Models.Enums;

namespace CustomerTracker.Web.Models.Entities
{
    public class Communication : BaseEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public int CustomerId { get; set; }

        public Customer Customer { get; set; }

        public int TitleId { get; set; }

        public Title Title { get; set; }

        public int GenderId { get; set; }

        public Gender Gender
        {
            get
            {
                return (Gender) GenderId;
            }
        }
    }
}