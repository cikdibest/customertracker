using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CustomerTracker.Web.Models.Enums;

namespace CustomerTracker.Web.Models.Entities
{
    public class RemoteComputer : BaseEntity
    {
        public string Name { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Explanation { get; set; }

        public string RemoteAddress { get; set; }

        public int RemoteConnectionTypeId { get; set; }

        public string ProductTags { get; set; }

        public Customer Customer { get; set; }

        public int CustomerId { get; set; }

        public RemoteConnectionType RemoteConnectionType
        {
            get
            {
                return (RemoteConnectionType) RemoteConnectionTypeId;
            }
        }
    }
}