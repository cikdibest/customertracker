using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Security;
using CustomerTracker.Common;
using CustomerTracker.Data;
using CustomerTracker.Data.Model.Entities;
using CustomerTracker.Web.Angular.Infrastructure.Membership;
using CustomerTracker.Web.Angular.Infrastructure.Repository;
using CustomerTracker.Web.Angular.Models.ViewModels;
using CustomerTracker.Web.Angular.Utilities;

namespace CustomerTracker.Web.Angular.Business.UserBusiness
{
    public class UserUtility : IUserUtility
    {  
        

        

       
        public User FindUserByUserName(string username)
        {
            var repositoryUser = ConfigurationHelper.UnitOfWorkInstance.GetRepository<User>();

            User user = repositoryUser.SelectAll().FirstOrDefault(usr => usr.Username == username);

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
       
        User FindUserByUserName(string username);

        List<User> FindUsersByUserName(string[] usernames);

        bool IsExitUserByUserName(string username);

        bool IsExitUserByEmail(string email);

        User CreateUser(User user);

        User FindUserByEmail(string email);

        User MapUserFromRegisterModel(RegisterModel registerModel);
    }
}