using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CustomerTracker.Web.Models.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsActive { get; set; }

        public int? CreatorUserId { get; set; }

        public int? ModifierUseId { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}