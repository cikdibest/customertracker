using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Security;
using CustomerTracker.Web.App_Start;
using CustomerTracker.Web.Business.UserBusiness;
using CustomerTracker.Web.Infrastructure.Repository;
using CustomerTracker.Web.Models.Entities;
using CustomerTracker.Web.Utilities;
using Ninject;

namespace CustomerTracker.Web.Infrastructure.Membership
{
    public class CodeFirstRoleProvider : RoleProvider
    {
        private IUserUtility _userUtility;
         
        public CodeFirstRoleProvider()
        {
            _userUtility = NinjectWebCommon.GetKernel.Get<IUserUtility>();
     }

        public override string ApplicationName
        {
            get
            {
                return this.GetType().Assembly.GetName().Name;
            }
            set
            {
                this.ApplicationName = this.GetType().Assembly.GetName().Name;
            }
        }

        public override bool RoleExists(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return false;
            }

            var repository = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Role>();

            Role role = null;

            role = repository.SelectAll().FirstOrDefault(rl => rl.RoleName == roleName);

            return role != null;

        }

        public override bool IsUserInRole(string username, string roleName)
        {
            if (string.IsNullOrEmpty(username))
            {
                return false;
            }
            if (string.IsNullOrEmpty(roleName))
            {
                return false;
            }

            var repositoryRole = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Role>();

            User user = _userUtility.FindUserByUserName(username);

            if (user != null)
            {
                var role = repositoryRole.SelectAll().FirstOrDefault(Rl => Rl.RoleName == roleName);

                return role != null && user.Roles.Contains(role);
            }

            return false;

        }

        public override string[] GetAllRoles()
        {
            var repositoryRole = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Role>();

            return repositoryRole.SelectAll().Select(rl => rl.RoleName).ToArray();

        }

        public override string[] GetUsersInRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return null;
            }

            var repositoryRole = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Role>();

            Role role = repositoryRole.SelectAll().FirstOrDefault(rl => rl.RoleName == roleName);

            if (role != null)
            {
                return role.Users.Select(usr => usr.Username).ToArray();
            }

            return null;

        }

        public override string[] GetRolesForUser(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return null;
            }

            User user = _userUtility.FindUserByUserName(username);

            return user != null ? user.Roles.Select(rl => rl.RoleName).ToArray() : null;

        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return null;
            }

            if (string.IsNullOrEmpty(usernameToMatch))
            {
                return null;
            }

            var repositoryRole = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Role>();

            return (from rl in repositoryRole.SelectAll() from usr in rl.Users where rl.RoleName == roleName && usr.Username.Contains(usernameToMatch) select usr.Username).ToArray();

        }

        public override void CreateRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName)) return;

            var repositoryRole = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Role>();

            var role = repositoryRole.SelectAll().FirstOrDefault(rl => rl.RoleName == roleName);

            if (role != null) return;

            var newRole = new Role
                              {
                                  RoleName = roleName
                              };

            repositoryRole.Create(newRole);

            ConfigurationHelper.UnitOfWorkInstance.Save();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return false;
            }

            var repositoryRole = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Role>();

            var role = repositoryRole.SelectAll().FirstOrDefault(rl => rl.RoleName == roleName);

            if (role == null)
            {
                return false;
            }

            if (throwOnPopulatedRole)
            {
                if (role.Users.Any())
                {
                    return false;
                }
            }
            else
            {
                role.Users.Clear();
            }

            repositoryRole.Delete(role);

            ConfigurationHelper.UnitOfWorkInstance.Save();

            return true;

        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            var repositoryRole = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Role>();

            var users = _userUtility.FindUsersByUserName(usernames);

            var roles = repositoryRole.SelectAll().Where(rl => roleNames.Contains(rl.RoleName)).ToList();

            foreach (var user in users)
            {
                foreach (Role role in roles)
                {
                    if (user.Roles == null)
                        user.Roles = new List<Role>();

                    if (user.Roles.Contains(role)) continue;

                    user.Roles.Add(role);
                }
            }

        }
         
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            var repositoryUser = ConfigurationHelper.UnitOfWorkInstance.GetRepository<User>();

            foreach (var username in usernames)
            {
                var us = username;

                var user = repositoryUser.SelectAll().FirstOrDefault(U => U.Username == us);

                if (user == null) continue;

                foreach (String roleName in roleNames)
                {
                    String rl = roleName;
                    Role role = user.Roles.FirstOrDefault(R => R.RoleName == rl);
                    if (role != null)
                    {
                        user.Roles.Remove(role);
                    }
                }
            }

            ConfigurationHelper.UnitOfWorkInstance.Save();

        }
    }
}