using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CustomerTracker.Common;
using CustomerTracker.Data.Model.Entities;
using CustomerTracker.Web.Angular.Infrastructure.Membership;
using CustomerTracker.Web.Angular.Models.Attributes;
using CustomerTracker.Web.Angular.Models.ViewModels;
using CustomerTracker.Web.Angular.Utilities;
using Ninject;

namespace CustomerTracker.Web.Angular.Controllers.api
{
    [CustomAuthorize(Roles = "Admin,Personel")]
    public class UserApiController : ApiController
    {
        [CustomAuthorize(Roles = "Admin")]
        public dynamic GetUsers(int pageNumber, int pageSize, string sortBy, string sortDir)
        {
            var skippedRow = (pageNumber - 1) * pageSize;

            var dataContext = ConfigurationHelper.UnitOfWorkInstance.GetCurrentDataContext();

            var users = dataContext.Users;

            var pageUsers = users
             
                .OrderBy(q => q.Id)
                .Skip(skippedRow)
                .Take(pageSize)
                .ToList();

            return new { users = pageUsers, totalCount = users.Count() };
        }

        //public User GetUser(int id)
        //{
        //    var userTrackerDataContext = ConfigurationHelper.UnitOfWorkInstance.GetCurrentDataContext();

        //    var user = userTrackerDataContext.Users.Include(q => q.Roles).SingleOrDefault(q => q.Id == id);

        //    if (user == null)
        //    {
        //        throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
        //    }

        //    return user;
        //}

        [CustomAuthorize(Roles = "Admin")]
        public HttpResponseMessage PutUser(int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != user.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            //var dbUser=ConfigurationHelper.UnitOfWorkInstance.GetRepository<User>().SelectAll().Include(q=>q.Roles).SingleOrDefault(q=>q.Id==user.Id);

            //if (user.RoleId==0)
            //    return Request.CreateResponse(HttpStatusCode.BadRequest);

            //if (dbUser.RoleId!=user.RoleId)//kullanıcının rolü değişmiştir
            //{
            //    user.Roles.Clear();

            //    var role = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Role>().Find(user.RoleId);

            //    user.Roles.Add(role);
            //}

            ConfigurationHelper.UnitOfWorkInstance.GetRepository<User>().Update(user);

            try
            {
                ConfigurationHelper.UnitOfWorkInstance.Save();

                var selectedRoles = user.SelectedRoles;

                var user1 = ConfigurationHelper.UnitOfWorkInstance.GetRepository<User>().SelectAll() .SingleOrDefault(q => q.Id == user.Id);
                user1.SelectedRoles = selectedRoles;
                WebSecurity.ReConfigureRoles(user1,user1.SelectedRoles.Select(q=>q.Id).ToList());

                ConfigurationHelper.UnitOfWorkInstance.Save();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        //public HttpResponseMessage PostUser(User user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        ConfigurationHelper.UnitOfWorkInstance.GetRepository<User>().Create(user);

        //        ConfigurationHelper.UnitOfWorkInstance.Save();

        //        HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, user);

        //        response.Headers.Location = new Uri(Url.Link("DefaultApiWithId", new { id = user.Id }));

        //        return response;
        //    }
        //    else
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        //    }
        //}

        [CustomAuthorize(Roles = "Admin")]
        public HttpResponseMessage DeleteUser(int id)
        {
            var user = ConfigurationHelper.UnitOfWorkInstance.GetRepository<User>().Find(id);

            if (user == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            ConfigurationHelper.UnitOfWorkInstance.GetRepository<User>().Delete(user);

            try
            {
                var save = ConfigurationHelper.UnitOfWorkInstance.Save();
                if (save == -547)
                    return Request.CreateResponse(HttpStatusCode.MultipleChoices, new Exception("Silmek istediğiniz kaydın bağlantılı verileri var.Lütfen önce bu verileri siliniz"));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, user);
        }

        [CustomAuthorize(Roles = "Admin,Personel")]
        public List<KeyValuePair<int, string>> GetSelectorUsers()
        {
            var users = ConfigurationHelper.UnitOfWorkInstance.GetRepository<User>()
                .SelectAll()
                .AsEnumerable()
                .Select(q => new KeyValuePair<int, string>(q.Id, q.FullName))
                .ToList();

            return users;
        }

        [CustomAuthorize(Roles = "Admin")]
        public HttpResponseMessage SendPasswordToUser(PasswordChangeModel passwordChangeModel)
        {
            
            var user = ConfigurationHelper.UnitOfWorkInstance.GetRepository<User>().SelectAll() .SingleOrDefault(q=>q.Id==passwordChangeModel.userId);

            var password = passwordChangeModel.password;

            var registerModel = new RegisterModel()
            {
                Password = user.Username + "1",
                ConfirmPassword = user.Username + "1",
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.Username
            };

            string hashedPassword = Crypto.HashPassword(registerModel.Password);

            user.Password = hashedPassword;

            user.LastPasswordChangedDate = DateTime.Now;

            var sendToUserAfterRegistration = new SendToUserAfterRegistrationMailViewModel()
            {
                FullName = registerModel.FirstName + " " + registerModel.LastName,
                UserMailAdress = registerModel.Email,
                Password = registerModel.Password,
                Username = registerModel.UserName,

            };

            var mailMessageForUser = NinjectWebCommon.GetKernel.Get<IMailBuilder>().BuildMailMessageForSendToUserAfterRegistration(sendToUserAfterRegistration);

            NinjectWebCommon.GetKernel.Get<IMailSenderUtility>().SendEmailAsync(mailMessageForUser);

            ConfigurationHelper.UnitOfWorkInstance.Save();

            return Request.CreateResponse(HttpStatusCode.OK, user);
        }
    }

    public class PasswordChangeModel
    {
        public int userId;
        public string password { get; set; }
    }
}
