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
        public List<TargetService> GetTargetServices(string machineCode)
        {
            try
            {
                var trimmmedLowerMachineCode = machineCode.ToLower().Trim();

                ValidateMachineCode(trimmmedLowerMachineCode);

                using (var customerTrackerDataContext = new CustomerTrackerDataContext())
                {
                    var repositoryRemoteMachine = new RepositoryGeneric<RemoteMachine>(customerTrackerDataContext);

                    var remoteMachine = repositoryRemoteMachine.SelectAll()
                        .Include(q => q.ApplicationServices)
                        .SingleOrDefault(q => q.MachineCode.ToLower() == trimmmedLowerMachineCode);

                    if (remoteMachine == null)
                        throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.NoContent, "Target machine is not found"));

                    return remoteMachine.ApplicationServices.Select(q => new TargetService() { ApplicationServiceId = q.Id, InstanceName = q.InstanceName, ApplicationServiceTypeId = q.ApplicationServiceTypeId }).ToList();
                }

            }
            catch (Exception exc)
            {
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exc));
            }


        }
         
        public HttpResponseMessage PostServerCondition(ServerCondition serverCondition)
        { 
            var trimmmedLowerMachineCode = serverCondition.MachineCode.ToLower().Trim();

            ValidateMachineCode(trimmmedLowerMachineCode);

            var serializeObject = JsonConvert.SerializeObject(serverCondition);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        private void ValidateMachineCode(string trimmmedLowerMachineCode)
        {
            if (string.IsNullOrWhiteSpace(trimmmedLowerMachineCode))
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "machineCode can not be null"));

            if (trimmmedLowerMachineCode.Trim().Length != 15 || !trimmmedLowerMachineCode.Contains("c") || !trimmmedLowerMachineCode.Contains("r") || !trimmmedLowerMachineCode.Contains("t"))
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "machineCode has wrong format"));

        }
    }
}