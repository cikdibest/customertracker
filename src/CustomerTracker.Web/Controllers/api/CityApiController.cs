using System.Collections.Generic;
using System.Web.Http;
using CustomerTracker.Web.Models.Entities;
using CustomerTracker.Web.Utilities;

namespace CustomerTracker.Web.Controllers.api
{
    public class CityApiController : ApiController
    {
        public IEnumerable<City> GetCities()
        {
            return ConfigurationHelper.UnitOfWorkInstance.GetRepository<City>().SelectAll();
        }
    }
}