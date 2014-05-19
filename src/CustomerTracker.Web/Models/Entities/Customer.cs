using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerTracker.Web.Models.Entities
{
    public class Customer : BaseEntity
    {
        public string Name { get; set; }

        public int CityId { get; set; }

        public City City { get; set; }

        public string Explanation { get; set; }

        public virtual List<Communication> Communications { get; set; }
    }
}