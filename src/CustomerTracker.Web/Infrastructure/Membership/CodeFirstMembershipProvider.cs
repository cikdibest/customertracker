using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Security;
using CustomerTracker.Web.App_Start;
using CustomerTracker.Web.Business.UserBusiness;
using CustomerTracker.Web.Infrastructure.Repository;
using CustomerTracker.Web.Models.Entities;
using CustomerTracker.Web.Models.ViewModels;
using CustomerTracker.Web.Utilities;
using Ninject;

namespace CustomerTracker.Web.Infrastructure.Membership
{
    public class CodeFirstMembershipProvider : MembershipProvider
    {
        private IUserUtility _userUtility;
         
        public CodeFirstMembershipProvider()
        {
            _userUtility = NinjectWebCommon.GetKernel.Get<IUserUtility>();

            
        }

        #region Properties

        private const int TokenSizeInBytes = 16;

        public override string ApplicationName
        {
            get
            {
                return this.GetType().Assembly.GetName().Name.ToString();
            }
            set
            {
                this.ApplicationName = this.GetType().Assembly.GetName().Name.ToString();
            }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { return 5; }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return 0; }
        }

        public override int MinRequiredPasswordLength
        {
            get { return 6; }
        }

        public override int PasswordAttemptWindow
        {
            get { return 0; }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return MembershipPasswordFormat.Hashed; }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { return String.Empty; }
        }

        public override bool RequiresUniqueEmail
        {
            get { return true; }
        }

        #endregion

        #region Functions

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new Exception("CreateUser not implemented");

            if (string.IsNullOrEmpty(username))
            {
                status = MembershipCreateStatus.InvalidUserName;
                return null;
            }
            if (string.IsNullOrEmpty(password))
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }
            if (string.IsNullOrEmpty(email))
            {
                status = MembershipCreateStatus.InvalidEmail;
                return null;
            }

            var hashedPassword = Crypto.HashPassword(password);

            if (hashedPassword.Length > 128)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            if (_userUtility.IsExitUserByUserName(username))
            {
                status = MembershipCreateStatus.DuplicateUserName;

                return null;
            }

            if (_userUtility.IsExitUserByEmail(email))
            {
                status = MembershipCreateStatus.DuplicateEmail;

                return null;
            }

            var newUser = new User
                              {
                                  Username = username,
                                  Password = hashedPassword,
                                  IsApproved = isApproved,
                                  Email = email,
                                  //CreationDate = DateTime.UtcNow,
                                  LastPasswordChangedDate = DateTime.UtcNow,
                                  PasswordFailuresSinceLastSuccess = 0,
                                  LastLoginDate = DateTime.UtcNow,
                                  LastActivityDate = DateTime.UtcNow,
                                  LastLockoutDate = DateTime.UtcNow,
                                  IsLockedOut = false,
                                  LastPasswordFailureDate = DateTime.UtcNow
                              };

            _userUtility.CreateUser(newUser);

            status = MembershipCreateStatus.Success;

            return new MembershipUser(System.Web.Security.Membership.Provider.Name, newUser.Username, newUser.Id, newUser.Email, null, null, newUser.IsApproved, newUser.IsLockedOut, newUser.CreationDate.HasValue ? newUser.CreationDate.Value : DateTime.Now, newUser.LastLoginDate.Value, newUser.LastActivityDate.Value, newUser.LastPasswordChangedDate.Value, newUser.LastLockoutDate.Value);

        }
         
        public override bool ValidateUser(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
            {
                return false;
            }
            if (string.IsNullOrEmpty(password))
            {
                return false;
            }
             
            var user = _userUtility.FindUserByUserName(username);

            if (user == null)
                return false;

            if (!user.IsApproved)
                return false;

            if (user.IsLockedOut)
                return false;

            var hashedPassword = user.Password;

            var verificationSucceeded = (hashedPassword != null && Crypto.VerifyHashedPassword(hashedPassword, password));

            if (verificationSucceeded)
            {
                user.PasswordFailuresSinceLastSuccess = 0;
                user.LastLoginDate = DateTime.UtcNow;
                user.LastActivityDate = DateTime.UtcNow;
            }
            else
            {
                int failures = user.PasswordFailuresSinceLastSuccess;

                if (failures < MaxInvalidPasswordAttempts)
                {
                    user.PasswordFailuresSinceLastSuccess += 1;
                    user.LastPasswordFailureDate = DateTime.UtcNow;
                }
                else if (failures >= MaxInvalidPasswordAttempts)
                {
                    user.LastPasswordFailureDate = DateTime.UtcNow;
                    user.LastLockoutDate = DateTime.UtcNow;
                    user.IsLockedOut = true;
                }
            }

            ConfigurationHelper.UnitOfWorkInstance.Save();

            return verificationSucceeded;

        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            if (string.IsNullOrEmpty(username))
            {
                return null;
            }
             
            var user =_userUtility.FindUserByUserName(username);

            if (user == null)
                return null;

            if (userIsOnline)
            {
                user.LastActivityDate = DateTime.UtcNow;

                ConfigurationHelper.UnitOfWorkInstance.Save();
            }

            return new MembershipUser(System.Web.Security.Membership.Provider.Name, user.Username, user.Id, user.Email, null, null,
                                      user.IsApproved, user.IsLockedOut, user.CreationDate.Value,
                                      user.LastLoginDate.Value, user.LastActivityDate.Value,
                                      user.LastPasswordChangedDate.Value, user.LastLockoutDate.Value);

        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            if (providerUserKey is int) { }
            else
            {
                return null;
            }

            var repositoryUser = ConfigurationHelper.UnitOfWorkInstance.GetRepository<User>();

            var userId = int.Parse(providerUserKey.ToString());

            var user = repositoryUser.Find(userId);

            if (user == null)
                return null;

            if (userIsOnline)
            {
                user.LastActivityDate = DateTime.UtcNow;

                ConfigurationHelper.UnitOfWorkInstance.Save();
            }

            return new MembershipUser(System.Web.Security.Membership.Provider.Name, user.Username, user.Id, user.Email, null, null,
                                      user.IsApproved, user.IsLockedOut, user.CreationDate.Value,
                                      user.LastLoginDate.Value, user.LastActivityDate.Value,
                                      user.LastPasswordChangedDate.Value, user.LastLockoutDate.Value);

        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            if (string.IsNullOrEmpty(username))
            {
                return false;
            }
            if (string.IsNullOrEmpty(oldPassword))
            {
                return false;
            }
            if (string.IsNullOrEmpty(newPassword))
            {
                return false;
            }
             
            var user =_userUtility.FindUserByUserName(username);

            if (user == null)
            {
                return false;
            }

            var hashedPassword = user.Password;

            var verificationSucceeded = (hashedPassword != null && Crypto.VerifyHashedPassword(hashedPassword, oldPassword));

            if (verificationSucceeded)
            {
                user.PasswordFailuresSinceLastSuccess = 0;
            }
            else
            {
                int failures = user.PasswordFailuresSinceLastSuccess;

                if (failures < MaxInvalidPasswordAttempts)
                {
                    user.PasswordFailuresSinceLastSuccess += 1;
                    user.LastPasswordFailureDate = DateTime.UtcNow;
                }
                else if (failures >= MaxInvalidPasswordAttempts)
                {
                    user.LastPasswordFailureDate = DateTime.UtcNow;
                    user.LastLockoutDate = DateTime.UtcNow;
                    user.IsLockedOut = true;
                }
                ConfigurationHelper.UnitOfWorkInstance.Save();

                return false;
            }

            var newHashedPassword = Crypto.HashPassword(newPassword);

            if (newHashedPassword.Length > 128)
            {
                return false;
            }

            user.Password = newHashedPassword;

            user.LastPasswordChangedDate = DateTime.UtcNow;

            ConfigurationHelper.UnitOfWorkInstance.Save();

            return true;

        }

        public override bool UnlockUser(string userName)
        { 
            var user =_userUtility.FindUserByUserName(userName);

            if (user != null)
            {
                user.IsLockedOut = false;

                user.PasswordFailuresSinceLastSuccess = 0;

                ConfigurationHelper.UnitOfWorkInstance.Save();

                return true;
            }

            return false;

        }

        public override int GetNumberOfUsersOnline()
        {
            var dateActive = DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(Convert.ToDouble(System.Web.Security.Membership.UserIsOnlineTimeWindow)));

            var repositoryUser = ConfigurationHelper.UnitOfWorkInstance.GetRepository<User>();

            return repositoryUser.SelectAll().Count(usr => usr.LastActivityDate > dateActive);
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            if (string.IsNullOrEmpty(username))
            {
                return false;
            }

            var repositoryUser = ConfigurationHelper.UnitOfWorkInstance.GetRepository<User>();

            var user =_userUtility.FindUserByUserName(username);

            if (user != null)
            {
                repositoryUser.Delete(user);

                ConfigurationHelper.UnitOfWorkInstance.Save();

                return true;
            }

            return false;

        }

        public override string GetUserNameByEmail(string email)
        {
            var user = _userUtility.FindUserByEmail(email);

            return user != null ? user.Username : string.Empty;

        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            var membershipUsers = new MembershipUserCollection();

            var repositoryUser = ConfigurationHelper.UnitOfWorkInstance.GetRepository<User>();
             
            totalRecords = repositoryUser.SelectAll().Count(Usr => Usr.Email == emailToMatch);

            var users = repositoryUser.SelectAll().Where(usr => usr.Email == emailToMatch).OrderBy(Usrn => Usrn.Username).Skip(pageIndex * pageSize).Take(pageSize);

            foreach (var user in users)
            {
                membershipUsers.Add(new MembershipUser(System.Web.Security.Membership.Provider.Name, user.Username, user.Id, user.Email, null, null, user.IsApproved, user.IsLockedOut, user.CreationDate.Value, user.LastLoginDate.Value, user.LastActivityDate.Value, user.LastPasswordChangedDate.Value, user.LastLockoutDate.Value));
            }

            return membershipUsers;
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            var membershipUsers = new MembershipUserCollection();

            var repositoryUser = ConfigurationHelper.UnitOfWorkInstance.GetRepository<User>();

            totalRecords = repositoryUser.SelectAll().Count(usr => usr.Username == usernameToMatch);

            var users = repositoryUser.SelectAll().Where(usr => usr.Username == usernameToMatch).OrderBy(usrn => usrn.Username).Skip(pageIndex * pageSize).Take(pageSize);

            foreach (var user in users)
            {
                membershipUsers.Add(new MembershipUser(System.Web.Security.Membership.Provider.Name, user.Username, user.Id, user.Email, null, null, user.IsApproved, user.IsLockedOut, user.CreationDate.Value, user.LastLoginDate.Value, user.LastActivityDate.Value, user.LastPasswordChangedDate.Value, user.LastLockoutDate.Value));
            }

            return membershipUsers;
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            var membershipUsers = new MembershipUserCollection();

            var repositoryUser = ConfigurationHelper.UnitOfWorkInstance.GetRepository<User>();

            totalRecords = repositoryUser.SelectAll().Count();

            var users = repositoryUser.SelectAll().OrderBy(usrn => usrn.Username).Skip(pageIndex * pageSize).Take(pageSize);

            foreach (var user in users)
            {
                membershipUsers.Add(new MembershipUser(System.Web.Security.Membership.Provider.Name, user.Username, user.Id, user.Email, null, null, user.IsApproved, user.IsLockedOut, user.CreationDate.Value, user.LastLoginDate.Value, user.LastActivityDate.Value, user.LastPasswordChangedDate.Value, user.LastLockoutDate.Value));
            }

            return membershipUsers;
        }

        //private static string GenerateToken()
        //{
        //    using (var prng = new RNGCryptoServiceProvider())
        //    {
        //        return GenerateToken(prng);
        //    }
        //}

        //private static string GenerateToken(RandomNumberGenerator generator)
        //{
        //    byte[] tokenBytes = new byte[TokenSizeInBytes];
  
        //    generator.GetBytes(tokenBytes);
            
        //    return HttpServerUtility.UrlTokenEncode(tokenBytes);
        //}

        #endregion

        #region Not Supported

        //CodeFirstMembershipProvider does not support password retrieval scenarios.
        public override bool EnablePasswordRetrieval
        {
            get { return false; }
        }
        public override string GetPassword(string username, string answer)
        {
            throw new NotSupportedException("Consider using methods from WebSecurity module.");
        }

        //CodeFirstMembershipProvider does not support password reset scenarios.
        public override bool EnablePasswordReset
        {
            get { return false; }
        }
        public override string ResetPassword(string username, string answer)
        {
            throw new NotSupportedException("Consider using methods from WebSecurity module.");
        }

        //CodeFirstMembershipProvider does not support question and answer scenarios.
        public override bool RequiresQuestionAndAnswer
        {
            get { return false; }
        }
        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotSupportedException("Consider using methods from WebSecurity module.");
        }

        //CodeFirstMembershipProvider does not support UpdateUser because this method is useless.
        public override void UpdateUser(MembershipUser user)
        {
            throw new NotSupportedException();
        }

        #endregion
    }

}