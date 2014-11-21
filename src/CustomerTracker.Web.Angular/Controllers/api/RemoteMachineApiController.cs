using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CustomerTracker.Data.Model.Entities;
using CustomerTracker.Web.Angular.Models.Attributes;
using CustomerTracker.Web.Angular.Utilities;
using Newtonsoft.Json;

namespace CustomerTracker.Web.Angular.Controllers.api
{
    [CustomAuthorize(Roles = "Admin,Personel")]
    public class RemoteMachineApiController : ApiController
    {
        public dynamic GetRemoteMachines(int pageNumber, int pageSize, string sortBy, string sortDir)
        {
            var skippedRow = (pageNumber - 1) * pageSize;

            var remoteMachineTrackerDataContext = ConfigurationHelper.UnitOfWorkInstance.GetCurrentDataContext();

            var remoteMachines = remoteMachineTrackerDataContext.RemoteMachines;

            var pageRemoteMachines = remoteMachines
                .Include(q => q.Customer)
                .Include(q => q.RemoteMachineConnectionType)
                .Include(q => q.ApplicationServices)
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

        [CustomAuthorize(Roles = "Admin")]
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
                remoteMachine.MachineCode = Guid.NewGuid().ToString();

                ConfigurationHelper.UnitOfWorkInstance.GetRepository<RemoteMachine>().Create(remoteMachine);

                ConfigurationHelper.UnitOfWorkInstance.Save();

                var machineCode = string.Format("C{0}R{1}", remoteMachine.CustomerId, remoteMachine.Id);

                remoteMachine.MachineCode = machineCode;

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

        [CustomAuthorize(Roles = "Admin")]
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
                var save = ConfigurationHelper.UnitOfWorkInstance.Save();
                if (save == -547)
                    return Request.CreateResponse(HttpStatusCode.MultipleChoices, new Exception("Silmek istediğiniz kaydın bağlantılı verileri var.Lütfen önce bu verileri siliniz"));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, remoteMachine);
        }

        public dynamic GetRemoteMachineStates()
        {

            var remoteMachines = ConfigurationHelper.UnitOfWorkInstance.GetRepository<RemoteMachine>()
                .SelectAll()
                .Where(q => q.ApplicationServices.Any())
                .Include(q => q.Customer)
                .Include(q => q.RemoteMachineConnectionType)
                .Include(q => q.ApplicationServices)
                .Include(q => q.MachineLogs);

            var pageRemoteMachines = remoteMachines
                .OrderBy(q => q.Id)
                //.Skip(skippedRow)
                //.Take(pageSize)
                .ToList();

            var enumerable = pageRemoteMachines.Select(t => new
            { 
                MachineCode = t.MachineCode,
                DecryptedName = t.DecryptedName,
                DecryptedRemoteAddress = t.DecryptedRemoteAddress,
                ApplicationServices = t.ApplicationServices.Select(aps => new { Id = aps.Id, InstanceName = aps.InstanceName }),
                Customer = new { Id = t.CustomerId, Name = t.Customer.Name },
                RemoteMachineConnectionType = new { Id = t.RemoteMachineConnectionTypeId, Name = t.RemoteMachineConnectionType.Name },
                MachineCondition = ParseMachineStatus(t),
            }).ToList();

            return enumerable;
        }

        public HttpResponseMessage PostApplicationService(ApplicationService applicationService)
        {
            if (ModelState.IsValid)
            {
                ConfigurationHelper.UnitOfWorkInstance.GetRepository<ApplicationService>().Create(applicationService);

                ConfigurationHelper.UnitOfWorkInstance.Save();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, applicationService);

                response.Headers.Location = new Uri(Url.Link("DefaultApiWithId", new { id = applicationService.Id }));

                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [CustomAuthorize(Roles = "Admin")]
        public HttpResponseMessage DeleteApplicationService(int id)
        {
            var applicationService = ConfigurationHelper.UnitOfWorkInstance.GetRepository<ApplicationService>().Find(id);

            if (applicationService == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            ConfigurationHelper.UnitOfWorkInstance.GetRepository<ApplicationService>().Delete(applicationService);

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

            return Request.CreateResponse(HttpStatusCode.OK, applicationService);
        }

        [CustomAuthorize(Roles = "Admin")]
        public HttpResponseMessage PutApplicationService(int id, ApplicationService applicationService)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            if (id != applicationService.Id)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            ConfigurationHelper.UnitOfWorkInstance.GetRepository<ApplicationService>().Update(applicationService);

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

        private dynamic ParseMachineStatus(RemoteMachine remnoteMachine)
        {
            var lastMachineLog = remnoteMachine.MachineLogs.OrderByDescending(q => q.Id).FirstOrDefault();

            if (lastMachineLog == null)
                return new { };

            var machineConditionJson = JsonConvert.DeserializeObject<dynamic>(lastMachineLog.MachineConditionJson);

            return new
            {
                LogDate = lastMachineLog.CreationDate.HasValue ? lastMachineLog.CreationDate.Value.ToString("dd.MM.yyyy HH:mm") :null, 
                MachineStatusList = machineConditionJson.HardwareControlMessages,
                ServiceStatusList = machineConditionJson.ServiceControlMessages,
            };

        }
    }
}
