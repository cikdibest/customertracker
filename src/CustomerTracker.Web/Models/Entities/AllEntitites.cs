﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using CustomerTracker.Web.App_Start;
using CustomerTracker.Web.Models.Enums;
using CustomerTracker.Web.Utilities;
using CustomerTracker.Web.Utilities.Helpers;
using Ninject;

namespace CustomerTracker.Web.Models.Entities
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

        [StringLength(100)]
        public string RemoteAddress { get; set; }

        [StringLength(4000)]
        public string Explanation { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public int RemoteMachineConnectionTypeId { get; set; }
        public virtual RemoteMachineConnectionType RemoteMachineConnectionType { get; set; }

        public virtual List<Product> Products { get; set; }

        [StringLength(100)]
        [NotMapped]
        public string DecryptedName
        {
            get
            {
                return this.Name.Decrypt();
            }
            set
            {
                this.Name = value.Encrypt();
            }
        }

        [StringLength(100)]
        [NotMapped]
        public string DecryptedUsername
        {
            get
            {
                return this.Username.Decrypt();
            }
            set
            {
                this.Username = value.Encrypt();
            }
        }

        [StringLength(500)]
        [NotMapped]
        public string DecryptedPassword
        {
            get
            {
                return this.Password.Decrypt();
            }
            set
            {
                this.Password = value.Encrypt();
            }
        }

        [StringLength(100)]
        [NotMapped]
        public string DecryptedRemoteAddress
        {
            get
            {
                return this.RemoteAddress.Decrypt();
            }
            set
            {
                this.RemoteAddress = value.Encrypt();
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

        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

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
                return this.Value.Decrypt();
            }
            set
            {
                this.Value = value.Encrypt();
            }
        }

    }


}