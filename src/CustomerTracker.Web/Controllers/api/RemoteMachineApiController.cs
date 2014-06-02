using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CustomerTracker.Web.Models.Entities;
using CustomerTracker.Web.Utilities;

namespace CustomerTracker.Web.Controllers.api
{
    [System.Web.Mvc.Authorize(Roles = "Admin,Personel")]
    public class RemoteMachineApiController : ApiController
    {
        public dynamic GetRemoteMachines(int pageNumber, int pageSize, string sortBy, string sortDir)
        {
            var skippedRow = (pageNumber - 1) * pageSize;

            var remoteMachineTrackerDataContext = ConfigurationHelper.UnitOfWorkInstance.GetCurrentDataContext();

            var remoteMachines = remoteMachineTrackerDataContext.RemoteMachines;

            var pageRemoteMachines = remoteMachines.Include("Customer").Include("RemoteMachineConnectionType")
                .OrderBy(q => q.Id)
                .Skip(skippedRow)
                .Take(pageSize)
                .ToList();

            return new { remoteMachines = pageRemoteMachines, totalCount = remoteMachines.Count() };
        }

        public RemoteMachine GetRemoteMachine(int id)
        {
            var remoteMachineTrackerDataContext = ConfigurationHelper.UnitOfWorkInstance.GetCurrentDataContext();

            var remoteMachine = remoteMachineTrackerDataContext.RemoteMachines.Include("Customer").SingleOrDefault(q => q.Id == id);

            if (remoteMachine == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return remoteMachine;
        }

        [System.Web.Mvc.Authorize(Roles = "Admin")]
        public HttpResponseMessage PutRemoteMachine(int id, RemoteMachine remoteMachine)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != remoteMachine.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            ConfigurationHelper.UnitOfWorkInstance.GetRepository<RemoteMachine>().Update(remoteMachine);

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

        public HttpResponseMessage PostRemoteMachine(RemoteMachine remoteMachine)
        {
            if (ModelState.IsValid)
            {
                ConfigurationHelper.UnitOfWorkInstance.GetRepository<RemoteMachine>().Create(remoteMachine);

                ConfigurationHelper.UnitOfWorkInstance.Save();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, remoteMachine);

                response.Headers.Location = new Uri(Url.Link("DefaultApiWithId", new { id = remoteMachine.Id }));

                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [System.Web.Mvc.Authorize(Roles = "Admin")]
        public HttpResponseMessage DeleteRemoteMachine(int id)
        {
            var remoteMachine = ConfigurationHelper.UnitOfWorkInstance.GetRepository<RemoteMachine>().Find(id);

            if (remoteMachine == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            ConfigurationHelper.UnitOfWorkInstance.GetRepository<RemoteMachine>().Delete(remoteMachine);

            try
            {
                ConfigurationHelper.UnitOfWorkInstance.Save();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, remoteMachine);
        }
    }
}
