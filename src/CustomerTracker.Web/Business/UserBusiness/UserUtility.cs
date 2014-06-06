using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Security;
using CustomerTracker.Web.Infrastructure.Membership;
using CustomerTracker.Web.Infrastructure.Repository;
using CustomerTracker.Web.Models.Entities;
using CustomerTracker.Web.Models.ViewModels;
using CustomerTracker.Web.Utilities;

namespace CustomerTracker.Web.Business.UserBusiness
{
    public class UserUtility : IUserUtility
    {  
        public void AddSocialAccountToUser(string provider, string providerUserId, string userName)
        {
            var repositoryUser = ConfigurationHelper.UnitOfWorkInstance.GetRepository<User>();

            var user = repositoryUser.Filter(q => q.Username == userName).SingleOrDefault();

            if (user.SocialAccounts == null)
                user.SocialAccounts = new List<SocialAccount>();

            user.AddSocialAccount(provider, providerUserId);
        }

        public User CreateUserWithSocialAccount(SocialUserRegisterModel socialUserRegisterModel)
        { 
            var user = WebSecurity.CreateUser(new RegisterModel()
                  {
                      Password = socialUserRegisterModel.Password,
                      ConfirmPassword = socialUserRegisterModel.Password,
                      Email = socialUserRegisterModel.Email,
                      UserName = socialUserRegisterModel.UserName,
                      FirstName = socialUserRegisterModel.FirstName,
                      LastName = socialUserRegisterModel.LastName,
                  },ConfigurationHelper.RolePersonel);
             
            user.AddSocialAccount(socialUserRegisterModel.Provider, socialUserRegisterModel.ProviderUserId);

            ConfigurationHelper.UnitOfWorkInstance.Save();

            return user;
        }

        public User FindUserBySocialAccount(string provider, string providerUserId)
        {
            var repositoryUser = ConfigurationHelper.UnitOfWorkInstance.GetRepository<User>();

            var user = repositoryUser.Filter(q => q.SocialAccounts.Any(t => t.Provider == provider && t.ProviderUserId == providerUserId)).SingleOrDefault();

            return user;
        }

        public User FindUserByUserName(string username)
        {
            var repositoryUser = ConfigurationHelper.UnitOfWorkInstance.GetRepository<User>();

            User user = repositoryUser.SelectAll().Include("Roles").FirstOrDefault(usr => usr.Username == username);

            return user;
        }

        public List<User> FindUsersByUserName(string[] usernames)
        {
            var repositoryUser = ConfigurationHelper.UnitOfWorkInstance.GetRepository<User>();

            var users = repositoryUser.SelectAll().Where(usr => usernames.Contains(usr.Username)).ToList();

            return users;
        }

        public bool IsExitUserByUserName(string username)
        {
            var repositoryUser = ConfigurationHelper.UnitOfWorkInstance.GetRepository<User>();

            return repositoryUser.SelectAll().Any(usr => usr.Username == username);

        }

        public bool IsExitUserByEmail(string email)
        {
            var repositoryUser = ConfigurationHelper.UnitOfWorkInstance.GetRepository<User>();

            return repositoryUser.SelectAll().Any(usr => usr.Email == email);

        }

        public User CreateUser(User user)
        {
            IRepositoryGeneric<User> repository = ConfigurationHelper.UnitOfWorkInstance.GetRepository<User>();

            repository.Create(user);

            ConfigurationHelper.UnitOfWorkInstance.Save();

            return user;
        }

        public User FindUserByEmail(string email)
        {
            var repositoryUser = ConfigurationHelper.UnitOfWorkInstance.GetRepository<User>();

            User user = repositoryUser.SelectAll().FirstOrDefault(usr => usr.Email == email);

            return user;
        }

        public User MapUserFromRegisterModel(RegisterModel registerModel)
        {
            if (string.IsNullOrEmpty(registerModel.UserName))
                throw new MembershipCreateUserException(MembershipCreateStatus.InvalidUserName);
             
            if (string.IsNullOrEmpty(registerModel.Password))
                throw new MembershipCreateUserException(MembershipCreateStatus.InvalidPassword);


            string hashedPassword = Crypto.HashPassword(registerModel.Password);

            if (hashedPassword.Length > 128)
                throw new MembershipCreateUserException(MembershipCreateStatus.InvalidPassword);

            if (IsExitUserByUserName(registerModel.UserName))
                throw new MembershipCreateUserException(MembershipCreateStatus.DuplicateUserName);

            if (IsExitUserByEmail(registerModel.Email))
                throw new MembershipCreateUserException(MembershipCreateStatus.DuplicateEmail);

            var newUser = new User
            {
                Username = registerModel.UserName,
                Password = hashedPassword,
                IsApproved = true,
                Email = registerModel.Email,
                LastPasswordChangedDate = DateTime.UtcNow,
                PasswordFailuresSinceLastSuccess = 0,
                LastLoginDate = DateTime.UtcNow,
                LastActivityDate = DateTime.UtcNow,
                LastLockoutDate = DateTime.UtcNow,
                IsLockedOut = false,
                LastPasswordFailureDate = DateTime.UtcNow,
                ConfirmationToken = string.Empty,
                FirstName = registerModel.FirstName,
                LastName = registerModel.LastName
            };

            return newUser;
        }
    }


    public interface IUserUtility
    {
        void AddSocialAccountToUser(string provider, string providerUserId, string userName);

        User CreateUserWithSocialAccount(SocialUserRegisterModel socialUserRegisterModel);

        User FindUserBySocialAccount(string provider, string providerUserId);

        User FindUserByUserName(string username);

        List<User> FindUsersByUserName(string[] usernames);

        bool IsExitUserByUserName(string username);

        bool IsExitUserByEmail(string email);

        User CreateUser(User user);

        User FindUserByEmail(string email);

        User MapUserFromRegisterModel(RegisterModel registerModel);
    }
}