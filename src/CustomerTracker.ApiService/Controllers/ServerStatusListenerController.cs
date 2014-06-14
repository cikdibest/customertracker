using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TestServerApiApp.Models;

namespace TestServerApiApp.Controllers
{
    public class ServerStatusListenerController : ApiController
    {
        public List<TargetService> GetTargetServices()
        {
            return new List<TargetService>()
            {
                new TargetService(){Id = 1,Name = "LibREF Database Service"},
                new TargetService(){Id = 2,Name = "LibREF Database Web Service"},
                new TargetService(){Id = 3,Name = "LibREF Reader Service Web Proxy"}

            };
        }

        public HttpResponseMessage PostServerCondition(ServerCondition serverCondition)
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}