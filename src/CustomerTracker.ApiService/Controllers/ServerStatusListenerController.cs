using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CustomerTracker.ApiService.Models;
using CustomerTracker.Data;
using CustomerTracker.Data.Model.Entities;
using Newtonsoft.Json;

namespace CustomerTracker.ApiService.Controllers
{

    public class ServerStatusListenerController : ApiController
    {
        public HttpResponseMessage GetApplicationServices(string machineCode)
        {
            try
            {

                var trimmmedLowerMachineCode = machineCode.ToLower().Trim();

                var httpResponseMessage = ValidateMachineCode(trimmmedLowerMachineCode);

                if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
                {
                    AddRequest(Request, httpResponseMessage);

                    return httpResponseMessage;
                }


                using (var customerTrackerDataContext = new CustomerTrackerDataContext())
                {
                    var repositoryRemoteMachine = new RepositoryGeneric<RemoteMachine>(customerTrackerDataContext);

                    var remoteMachine = repositoryRemoteMachine.SelectAll()
                        .Include(q => q.ApplicationServices)
                        .SingleOrDefault(q => q.MachineCode.ToLower() == trimmmedLowerMachineCode);

                    if (remoteMachine == null)
                    {
                        var responseMessage = Request.CreateErrorResponse(HttpStatusCode.NoContent, "Remote machine is not found");
                        AddRequest(Request, responseMessage);
                        return responseMessage;
                    }


                    var targetServices = remoteMachine.ApplicationServices.Select(q => new TargetService() { ApplicationServiceId = q.Id, InstanceName = q.InstanceName, ApplicationServiceTypeId = q.ApplicationServiceTypeId }).ToList();

                    var response = Request.CreateResponse(HttpStatusCode.OK, targetServices);
                    AddRequest(Request, response);
                    return response;
                }

            }
            catch (Exception exc)
            {
                var httpResponseMessage = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exc);
                AddRequest(Request, httpResponseMessage);
                return httpResponseMessage;
            }

        }

        public HttpResponseMessage PostServerCondition(dynamic serverCondition)
        {
            try
            {
                var machineCode = serverCondition.MachineCode.Value as string;

                var isAlarm = serverCondition.IsAlarm.Value == true;

                var trimmmedLowerMachineCode = machineCode.ToLower().Trim();

                var httpResponseMessage = ValidateMachineCode(trimmmedLowerMachineCode);

                if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
                {
                    AddRequest(Request, httpResponseMessage);

                    return httpResponseMessage;
                }

                using (var customerTrackerDataContext = new CustomerTrackerDataContext())
                {
                    var repositoryRemoteMachine = new RepositoryGeneric<RemoteMachine>(customerTrackerDataContext);

                    var remoteMachine = repositoryRemoteMachine.Find(q => q.MachineCode.ToLower() == trimmmedLowerMachineCode);

                    if (remoteMachine == null)
                    {
                        var responseMessage1 = Request.CreateErrorResponse(HttpStatusCode.NoContent, "Remote machine is not found");
                        AddRequest(Request, responseMessage1);
                        return responseMessage1;
                    }

                    var serializeObject = JsonConvert.SerializeObject(serverCondition);

                    if (remoteMachine.MachineLogs == null)
                        remoteMachine.MachineLogs = new Collection<MachineLog>();

                    remoteMachine.MachineLogs.Add(new MachineLog() { MachineConditionJson = serializeObject, IsAlarm = isAlarm,CreationDate = DateTime.Now,IsActive = true});

                    customerTrackerDataContext.SaveChanges();
                }


                var responseMessage = Request.CreateResponse(HttpStatusCode.OK);

                AddRequest(Request, responseMessage);

                return responseMessage;
            }

            catch (Exception exc)
            {
                var httpResponseMessage = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exc);
                AddRequest(Request, httpResponseMessage);
                return httpResponseMessage;
            }



        }

        public HttpResponseMessage GetActiveRequests()
        {
            return Request.CreateResponse(HttpStatusCode.OK, CacheData.Instance.HttpRequestMessages);
        }

        #region private methods
        private HttpResponseMessage ValidateMachineCode(string trimmmedLowerMachineCode)
        {
            if (string.IsNullOrWhiteSpace(trimmmedLowerMachineCode))
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "machineCode can not be null");

            if (!trimmmedLowerMachineCode.Contains("c") || !trimmmedLowerMachineCode.Contains("r") || !trimmmedLowerMachineCode.Contains("t"))
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "machineCode has wrong format");

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        private void AddRequest(HttpRequestMessage httpRequestMessage, HttpResponseMessage responseMessage)
        {
            if (CacheData.Instance.HttpRequestMessages.Count > 50)
                CacheData.Instance.HttpRequestMessages.RemoveAt(0);

            CacheData.Instance.HttpRequestMessages.Add(new RequestModel()
            {
                RequestUrl = httpRequestMessage.RequestUri.AbsoluteUri,
                RequestDate = DateTime.Now.ToString("dd.MM.yyyy hh:mm"),
                ResponseStatusCode = responseMessage.StatusCode.ToString(),

            });
        }

        #endregion

    }
}