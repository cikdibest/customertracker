using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using CustomerTracker.Web.Models.Enums;

namespace CustomerTracker.Web.Models.Entities
{ 
    public class Customer : BaseEntity
    {
        [StringLength(100)]
        public string Title { get; set; }

        public int CityId { get; set; }

        public City City { get; set; }

        [StringLength(4000)]
        public string Explanation { get; set; }

        public virtual List<Communication> Communications { get; set; }

        public virtual List<RemoteMachine> RemoteMachines { get; set; }

        public virtual List<Product> Products { get; set; }
    }
     
    public class Communication : BaseEntity
    {
        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(100)]
        public string PhoneNumber { get; set; }

        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }

        public int DepartmentId { get; set; }

        public Department Department { get; set; }

        public int GenderId { get; set; }

        public EnumGender Gender
        {
            get
            {
                return (EnumGender)GenderId;
            }
        }

        public string FullName
        {
            get
            {
                return this.FirstName + " " + this.LastName;
            }
        }
    }
   
    public class RemoteMachine : BaseEntity
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

        public Customer Customer { get; set; }

        public int CustomerId { get; set; }

        public virtual List<Product> Products { get; set; }

        public RemoteConnectionType RemoteConnectionType
        {
            get
            {
                return (RemoteConnectionType)RemoteConnectionTypeId;
            }
        }
    }
 
    public class City : BaseEntity
    {
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(10)]
        public string Code { get; set; }

        public virtual List<Customer> Customers { get; set; }
    }
     
    public class Department : BaseEntity
    {
        [StringLength(100)]
        public string Name { get; set; }

        public virtual List<Communication> Communications { get; set; }
    }
     
    public class Product : BaseEntity
    {
        [StringLength(100)]
        public string Name { get; set; }

        public int? ParentProductId { get; set; }

        public virtual Product ParentProduct { get; set; }

        [StringLength(4000)]
        public string Explanation { get; set; }

        public virtual ICollection<Product> SubProducts { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }

        public virtual ICollection<RemoteMachine> RemoteMachines { get; set; }

    }
}