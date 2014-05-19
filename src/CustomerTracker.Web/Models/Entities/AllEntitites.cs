using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CustomerTracker.Web.Models.Enums;

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

        public Customer Customer { get; set; }

        public int TitleId { get; set; }

        public Title Title { get; set; }

        public int GenderId { get; set; }

        public EnumGender Gender
        {
            get
            {
                return (EnumGender)GenderId;
            }
        }
    }

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


    public class Title : BaseEntity
    {
        [StringLength(100)]
        public string Name { get; set; }

        public virtual List<Communication> Communications { get; set; }
    }

    public class Product : BaseEntity
    {
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(4000)]
        public string Explanation { get; set; }
    }
}