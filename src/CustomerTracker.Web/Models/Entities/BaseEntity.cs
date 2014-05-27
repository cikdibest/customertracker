using System;
using System.Runtime.Serialization;

namespace CustomerTracker.Web.Models.Entities
{ 
    public class BaseEntity
    { 
        public int Id { get; set; }
         
        public DateTime ? CreationDate { get; set; }
         
        public int ? CreationPersonelId { get; set; }
         
        public DateTime? UpdatedDate { get; set; }
         
        public int? UpdatedPersonelId { get; set; }
         
        public bool IsActive { get; set; }
         
        public bool IsDeleted { get; set; }
         
    }
}
