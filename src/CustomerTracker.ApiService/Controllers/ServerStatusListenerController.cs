using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;
using CustomerTracker.Data;
using CustomerTracker.Data.Model.Entities;
using Newtonsoft.Json;
using TestServerApiApp.Models;

namespace TestServerApiApp.Controllers
{
    public class ServerStatusListenerController : ApiController
    {
        public HttpResponseMessage GetTargetServices(string machineCode)
        {

            try
            {
                var trimmmedLowerMachineCode = machineCode.ToLower().Trim();

                var httpResponseMessage = ValidateMachineCode(trimmmedLowerMachineCode);

                if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
                    return httpResponseMessage;

                using (var customerTrackerDataContext = new CustomerTrackerDataContext())
                {
                    var repositoryRemoteMachine = new RepositoryGeneric<RemoteMachine>(customerTrackerDataContext);

                    var remoteMachine = repositoryRemoteMachine.SelectAll()
                        .Include(q => q.ApplicationServices)
                        .SingleOrDefault(q => q.MachineCode.ToLower() == trimmmedLowerMachineCode);

                    if (remoteMachine == null)
                        return Request.CreateErrorResponse(HttpStatusCode.NoContent, "Target machine is not found");

                    var targetServices = remoteMachine.ApplicationServices.Select(q => new TargetService() { ApplicationServiceId = q.Id, InstanceName = q.InstanceName, ApplicationServiceTypeId = q.ApplicationServiceTypeId }).ToList();

                    return Request.CreateResponse(HttpStatusCode.OK, targetServices);
                }

            }
            catch (Exception exc)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exc);
            }

        }

        public HttpResponseMessage PostServerCondition(ServerCondition serverCondition)
        {
            var trimmmedLowerMachineCode = serverCondition.MachineCode.ToLower().Trim();

            var httpResponseMessage = ValidateMachineCode(trimmmedLowerMachineCode);

            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
                return httpResponseMessage;

            var serializeObject = JsonConvert.SerializeObject(serverCondition);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private HttpResponseMessage ValidateMachineCode(string trimmmedLowerMachineCode)
        {
            if (string.IsNullOrWhiteSpace(trimmmedLowerMachineCode))
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "machineCode can not be null");

            if (!trimmmedLowerMachineCode.Contains("c") || !trimmmedLowerMachineCode.Contains("r") || !trimmmedLowerMachineCode.Contains("t"))
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "machineCode has wrong format");

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}