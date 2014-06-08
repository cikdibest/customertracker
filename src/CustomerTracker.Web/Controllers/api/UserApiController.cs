using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CustomerTracker.Web.Models.Attributes;
using CustomerTracker.Web.Models.Entities;
using CustomerTracker.Web.Utilities;

namespace CustomerTracker.Web.Controllers.api
{
    [CustomAuthorize(Roles = "Admin")]
    public class UserApiController : ApiController
    { 
        public dynamic GetUsers(int pageNumber, int pageSize, string sortBy, string sortDir)
        {
            var skippedRow = (pageNumber - 1) * pageSize;

            var dataContext = ConfigurationHelper.UnitOfWorkInstance.GetCurrentDataContext();

            var users = dataContext.Users;

            var pageUsers = users
                .Include(q => q.Roles)
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

            ConfigurationHelper.UnitOfWorkInstance.GetRepository<User>().Update(user);

            try
            {
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
         
        public HttpResponseMessage DeleteUser(int id)
        {
            var user = ConfigurationHelper.UnitOfWorkInstance.GetRepository<User>().Find(id);

            if (user == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            ConfigurationHelper.UnitOfWorkInstance.GetRepository<User>().Delete(user);

            try
            {
                ConfigurationHelper.UnitOfWorkInstance.Save();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, user);
        }

        public List<KeyValuePair<int, string>> GetSelectorUsers()
        {
            var users = ConfigurationHelper.UnitOfWorkInstance.GetRepository<User>()
                .SelectAll()
                .AsEnumerable()
                .Select(q => new KeyValuePair<int, string>(q.Id, q.FullName))
                .ToList();

            return users;
        }


    }


}
