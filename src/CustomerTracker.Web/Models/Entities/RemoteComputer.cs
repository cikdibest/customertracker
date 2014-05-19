using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CustomerTracker.Web.Models.Enums;

namespace CustomerTracker.Web.Models.Entities
{
    public class RemoteComputer : BaseEntity
    {
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Username { get; set; }

        [StringLength(500)]
        public string Password { get; set; }

        [StringLength(4000)]
        public string Explanation { get; set; }

        [StringLength(100)]
        public string RemoteAddress { get; set; }

        public int RemoteConnectionTypeId { get; set; }

        [StringLength(200)]
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