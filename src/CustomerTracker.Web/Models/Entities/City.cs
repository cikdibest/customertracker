using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerTracker.Web.Models.Entities
{
    public class City : BaseEntity
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public virtual List<Customer> Customers { get; set; }
    }
}