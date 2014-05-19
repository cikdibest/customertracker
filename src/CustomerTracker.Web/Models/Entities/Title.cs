using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerTracker.Web.Models.Entities
{
    public class Title : BaseEntity
    {
        public string Name { get; set; }

        public virtual List<Communication> Communications { get; set; }
    }
}