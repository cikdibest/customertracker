using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using CustomerTracker.Web.Models.Entities;
using CustomerTracker.Web.Models.Enums;
using CustomerTracker.Web.Utilities;
using CustomerTracker.Web.Utilities.Helpers;

namespace CustomerTracker.Web.Controllers.api
{
    public class SharedDataApiController : ApiController
    { 
        public IEnumerable<City> GetCities()
        {
            var cities = ConfigurationHelper.UnitOfWorkInstance.GetRepository<City>().SelectAll();

            return cities;
        }
         
        public IEnumerable<KeyValuePair<int,string>> GetRemoteConnectionTypes()
        {
            var remoteConnectionTypes = EnumHelper.ToDictionary<RemoteConnectionType>();

            return remoteConnectionTypes.ToList();
        }
    }
}