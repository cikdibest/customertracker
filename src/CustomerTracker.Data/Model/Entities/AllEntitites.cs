using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CustomerTracker.Common.Helpers;
using CustomerTracker.Data.Model.Enums;

namespace CustomerTracker.Data.Model.Entities
{
    public class Customer : BaseEntity
    {
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(20)]
        public string Abbreviation { get; set; }

        [StringLength(100)]
        public string Keywords { get; set; }

        [StringLength(4000)]
        public string Explanation { get; set; }

        [StringLength(250)]
        public string AvatarImageUrl { get; set; }

        public int CityId { get; set; }
        public City City { get; set; }

        public virtual List<Communication> Communications { get; set; }
         
        public virtual List<Product> Products { get; set; }

        public virtual List<DataMaster> DataMasters { get; set; }


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
        public string HomePhoneNumber { get; set; }

        [StringLength(100)]
        public string MobilePhoneNumber { get; set; }

        [StringLength(4000)]
        public string Explanation { get; set; }

        [StringLength(250)]
        public string AvatarImageUrl { get; set; }

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
        public string GenderName
        {
            get
            {
                return Gender.GetDescription();
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
     
    public class City : BaseEntity
    {
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(10)]
        public string Code { get; set; }


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

        [StringLength(4000)]
        public string Explanation { get; set; }

        [StringLength(250)]
        public string AvatarImageUrl { get; set; }

        [StringLength(100)]
        public string Keywords { get; set; }

        public int? ParentProductId { get; set; }
        public virtual Product ParentProduct { get; set; }

        public virtual ICollection<Product> SubProducts { get; set; }

        public virtual ICollection<Customer> Customers { get; set; }
         


    }

    public class DataMaster : BaseEntity
    {
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string AvatarImageUrl { get; set; }

        //public int? CustomerId { get; set; }
        //public virtual Customer Customer { get; set; }

        public ICollection<DataDetail> DataDetails { get; set; }

        public ICollection<User> Users { get; set; }

        public ICollection<Customer> Customers { get; set; }

        [NotMapped]
        public virtual int? TempCustomerId { get; set; }

        [NotMapped]
        public virtual int? TempUserId { get; set; }
    }

    public class DataDetail : BaseEntity
    {
        [StringLength(50)]
        public string Key { get; set; }

        [StringLength(4000)]
        public string Value { get; set; }

        public int DataMasterId { get; set; }
        public virtual DataMaster DataMaster { get; set; }

        [StringLength(4000)]
        [NotMapped]
        public string DecryptedValue
        {
            get
            {
                return Decrypt(this.Value);
            }
            set
            {
                this.Value = Encrypt(value);
            }
        }

    }

    public class Solution : BaseEntity
    {
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public int TroubleId { get; set; }
        public virtual Trouble Trouble { get; set; }

        public int SolutionUserId { get; set; }
        public virtual User SolutionUser { get; set; }

        [StringLength(250)]
        public string Title { get; set; }

        public string Description { get; set; }

    }

    public class Trouble : BaseEntity
    {
        public string Name { get; set; }
    }
     
}