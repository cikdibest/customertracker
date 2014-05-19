using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerTracker.Web.Models.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }

        public string Explanation { get; set; }
    }
}