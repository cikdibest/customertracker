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

        public virtual List<RemoteMachine> RemoteMachines { get; set; }

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

    public class RemoteMachine : BaseEntity
    {
        [StringLength(50)]
        public string MachineCode { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Username { get; set; }

        [StringLength(500)]
        public string Password { get; set; }

        [StringLength(100)]
        public string RemoteAddress { get; set; }

        [StringLength(4000)]
        public string Explanation { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public int RemoteMachineConnectionTypeId { get; set; }
        public virtual RemoteMachineConnectionType RemoteMachineConnectionType { get; set; }

        public virtual List<Product> Products { get; set; }

        public virtual ICollection<ApplicationService> ApplicationServices { get; set; }

        public virtual ICollection<MachineLog> MachineLogs { get; set; }

        [StringLength(100)]
        [NotMapped]
        public string DecryptedName
        {
            get
            {
                return Decrypt(this.Name);
            }
            set
            {
                this.Name = Encrypt(value);
            }
        }

        [StringLength(100)]
        [NotMapped]
        public string DecryptedUsername
        {
            get
            {
                return Decrypt(this.Username);
            }
            set
            {
                this.Username = Encrypt(value);
            }
        }

        [StringLength(500)]
        [NotMapped]
        public string DecryptedPassword
        {
            get
            {
                return Decrypt(this.Password);
            }
            set
            {
                this.Password = Encrypt(value);
            }
        }

        [StringLength(100)]
        [NotMapped]
        public string DecryptedRemoteAddress
        {
            get
            {
                return Decrypt(this.RemoteAddress);
            }
            set
            {
                this.RemoteAddress = Encrypt(value);
            }
        }

    }

    public class RemoteMachineConnectionType : BaseEntity
    {
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(250)]
        public string AvatarImageUrl { get; set; }

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

        public virtual ICollection<RemoteMachine> RemoteMachines { get; set; }


    }

    public class DataMaster : BaseEntity
    {
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string AvatarImageUrl { get; set; }

        public int? CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        public int? UserId { get; set; }
        public virtual User User { get; set; }

        public ICollection<DataDetail> DataDetails { get; set; }
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

    public class ApplicationService : BaseEntity
    {
        [StringLength(50)]
        public string InstanceName { get; set; }

        [StringLength(1000)]
        public string Description { get; set; }

        public int ApplicationServiceTypeId { get; set; }
        public ApplicationServiceType ApplicationServiceType
        {
            get
            {
                return (ApplicationServiceType)ApplicationServiceTypeId;
            }
        }
        public string ApplicationServiceTypeName
        {
            get
            {
                return ApplicationServiceType.GetDescription();
            }
        }

        public int RemoteMachineId { get; set; }
        public virtual RemoteMachine RemoteMachine { get; set; }
    }

    public class MachineLog : BaseEntity
    {
        public string MachineConditionJson { get; set; }

        public bool IsAlarm { get; set; }

        public int RemoteMachineId { get; set; }
        public virtual RemoteMachine RemoteMachine { get; set; }
    }

    public enum ApplicationServiceType
    {
        WindowsService = 1,

        SqlServer = 2,
    }
}