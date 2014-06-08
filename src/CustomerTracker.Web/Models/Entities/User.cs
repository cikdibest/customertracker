using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using CustomerTracker.Web.Infrastructure.Membership;
using CustomerTracker.Web.Utilities;
using CustomerTracker.Web.Utilities.Helpers;

namespace CustomerTracker.Web.Models.Entities
{
    public class SocialAccount : BaseEntity
    {
        public int UserId { get; set; }

        [StringLength(100)]
        public string Provider { get; set; }

        [StringLength(500)]
        public string ProviderUserId { get; set; }

        public override bool Equals(object objcToCompare)
        {
            var toCompare = objcToCompare as SocialAccount;

            if (toCompare != null)
            {
                return toCompare.Provider == Provider && toCompare.ProviderUserId == ProviderUserId;
            }

            return false;
        }
    }

    public class Role : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public virtual string RoleName { get; set; }

        [StringLength(50)]
        public virtual string Description { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }

    public class User : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public virtual String Username { get; set; }

        [StringLength(100)]
        public virtual String Email { get; set; }

        [Required, DataType(DataType.Password)]
        [StringLength(100)]
        public virtual String Password { get; set; }

        [StringLength(100)]
        public virtual String FirstName { get; set; }

        [StringLength(100)]
        public virtual String LastName { get; set; }

        public virtual Boolean IsApproved { get; set; }

        public virtual int PasswordFailuresSinceLastSuccess { get; set; }

        public virtual DateTime? LastPasswordFailureDate { get; set; }

        public virtual DateTime? LastActivityDate { get; set; }

        public virtual DateTime? LastLockoutDate { get; set; }

        public virtual DateTime? LastLoginDate { get; set; }

        [StringLength(100)]
        public virtual String ConfirmationToken { get; set; }

        public virtual Boolean IsLockedOut { get; set; }

        public virtual DateTime? LastPasswordChangedDate { get; set; }

        [StringLength(100)]
        public virtual String PasswordVerificationToken { get; set; }

        public virtual DateTime? PasswordVerificationTokenExpirationDate { get; set; }

        public virtual List<Role> Roles { get; set; }

        public virtual List<SocialAccount> SocialAccounts { get; set; }

        [NotMapped]
        public string FullName
        {
            get { return this.FirstName + " " + this.LastName; }
        }

        [NotMapped]
        public List<RoleId> SelectedRoles { get; set; }//client side da eitleme esnasında seçilen roller

        [NotMapped]
        public string RoleNames
        {
            get
            {
#warning bu kısmı kullanan userview.onun bunu kullanmaması sağlanırsa , bu property silenebilir.
                if (this.Roles==null)
                {
                    return "LazyLoading;)";
                }
                return string.Join(",", this.Roles.Select(q => q.RoleName));
            }
        }

        public void ReConfigureRoles()
        {
            //if (this.Roles == null)
            //    this.Roles = new List<Role>();

            var roleIds = this.SelectedRoles.Select(q => q.Id);

            var deletedRoles = this.Roles.Where(q => !roleIds.Contains(q.Id)).ToList();

            foreach (var deletedRole in deletedRoles)
            {
                this.Roles.Remove(deletedRole);
            }

            foreach (var roleId in roleIds)
            {
                if (this.Roles.Any(q => q.Id == roleId)) continue;

                var repositoryRole = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Role>();

                var role = repositoryRole.Find(q => q.Id == roleId);

                this.Roles.Add(role);
            }


        }

        public void AddSocialAccount(string provider, string providerUserId)
        {
            var socialAccount = new SocialAccount() { ProviderUserId = providerUserId, Provider = provider };

            if (SocialAccounts == null)
                SocialAccounts = new List<SocialAccount>();

            if (!SocialAccounts.Contains(socialAccount))
            {
                SocialAccounts.Add(socialAccount);
            }
            else
            {
                var itemIndex = SocialAccounts.IndexOf(socialAccount);

                SocialAccounts[itemIndex] = socialAccount;
            }

        }

        public void AddRole(Role role)
        {
            if (this.Roles == null)
                this.Roles = new List<Role>();

            if (this.Roles.Any(q => q.RoleName == role.RoleName)) return;

            this.Roles.Add(role);
        }
    }

    public class RoleId
    {
        public int Id { get; set; }
    }
}