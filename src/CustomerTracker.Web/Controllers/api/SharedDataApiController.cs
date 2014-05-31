using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using CustomerTracker.Web.Models.Entities;
using CustomerTracker.Web.Models.Enums;
using CustomerTracker.Web.Utilities;
using CustomerTracker.Web.Utilities.Helpers;

namespace CustomerTracker.Web.Controllers.api
{
#warning ilgili apiler oluştuurlunca bu methotlar taşınacak
    [System.Web.Mvc.Authorize(Roles = "Admin,Personel")]
    public class SharedDataApiController : ApiController
    {
        public IEnumerable<KeyValuePair<int, string>> GetSelectorCities()
        {
            var cities = ConfigurationHelper.UnitOfWorkInstance.GetRepository<City>()
                .SelectAll()
                 .Select(q => new KeyValuePair<int, string>(q.Id, q.Name));

            return cities;
        }

        public IEnumerable<KeyValuePair<int, string>> GetSelectorRemoteMachineConnectionTypes()
        {
            var remoteMachineConnectionTypes = ConfigurationHelper.UnitOfWorkInstance.GetRepository<RemoteMachineConnectionType>()
                .SelectAll()
                .Select(q => new KeyValuePair<int, string>(q.Id, q.Name));

            return remoteMachineConnectionTypes;
        }
    }
}