using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CustomerTracker.Web.Models.Entities
{
    public class Customer : BaseEntity
    {
        [StringLength(100)]
        public string Name { get; set; }

        public int CityId { get; set; }

        public City City { get; set; }

        [StringLength(4000)]
        public string Explanation { get; set; }

        public virtual List<Communication> Communications { get; set; }
    }
}