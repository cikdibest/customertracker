using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using CustomerTracker.Web.Models.Entities;
using CustomerTracker.Web.Utilities;

namespace CustomerTracker.Web.Controllers.api
{
    [System.Web.Mvc.Authorize(Roles = "Admin,Personel")]
    public class RemoteMachineConnectionTypeApiController : ApiController
    {
        public IEnumerable<RemoteMachineConnectionType> GetRemoteMachineConnectionTypes()
        {
            var remoteMachineConnectionTypes = ConfigurationHelper.UnitOfWorkInstance.GetRepository<RemoteMachineConnectionType>().SelectAll().ToList();

            return remoteMachineConnectionTypes;
        }

        public RemoteMachineConnectionType GetRemoteMachineConnectionType(int id)
        {
            var remoteMachineConnectionType = ConfigurationHelper.UnitOfWorkInstance.GetRepository<RemoteMachineConnectionType>().Find(id);

            if (remoteMachineConnectionType == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return remoteMachineConnectionType;
        }

        [System.Web.Mvc.Authorize(Roles = "Admin")]
        public HttpResponseMessage PutRemoteMachineConnectionType(int id, RemoteMachineConnectionType remoteMachineConnectionType)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != remoteMachineConnectionType.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            ConfigurationHelper.UnitOfWorkInstance.GetRepository<RemoteMachineConnectionType>().Update(remoteMachineConnectionType);

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

        public HttpResponseMessage PostRemoteMachineConnectionType(RemoteMachineConnectionType remoteMachineConnectionType)
        {
            if (ModelState.IsValid)
            {
                ConfigurationHelper.UnitOfWorkInstance.GetRepository<RemoteMachineConnectionType>().Create(remoteMachineConnectionType);

                ConfigurationHelper.UnitOfWorkInstance.Save();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, remoteMachineConnectionType);

                response.Headers.Location = new Uri(Url.Link("DefaultApiWithId", new { id = remoteMachineConnectionType.Id }));

                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [System.Web.Mvc.Authorize(Roles = "Admin")]
        public HttpResponseMessage DeleteRemoteMachineConnectionType(int id)
        {
            var remoteMachineConnectionType = ConfigurationHelper.UnitOfWorkInstance.GetRepository<RemoteMachineConnectionType>().Find(id);

            if (remoteMachineConnectionType == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            ConfigurationHelper.UnitOfWorkInstance.GetRepository<RemoteMachineConnectionType>().Delete(remoteMachineConnectionType);

            try
            {
                ConfigurationHelper.UnitOfWorkInstance.Save();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, remoteMachineConnectionType);
        }

        public List<KeyValuePair<int, string>> GetSelectorRemoteMachineConnectionTypes()
        {
            var remoteMachineConnectionType = ConfigurationHelper.UnitOfWorkInstance.GetRepository<RemoteMachineConnectionType>()
                .SelectAll()
                .AsEnumerable()
                .Select(q => new KeyValuePair<int, string>(q.Id, q.Name))
                .ToList();

            return remoteMachineConnectionType;
        }
    }
}
