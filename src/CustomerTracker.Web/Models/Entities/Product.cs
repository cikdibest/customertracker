using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CustomerTracker.Web.Models.Entities
{
    public class Product : BaseEntity
    {
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(4000)]
        public string Explanation { get; set; }
    }
}