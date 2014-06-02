using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using CustomerTracker.Web.Models.Entities;
using CustomerTracker.Web.Models.Enums;
using CustomerTracker.Web.Utilities;
using CustomerTracker.Web.Utilities.Helpers;

namespace CustomerTracker.Web.Controllers.api
{
#warning ilgili apiler oluşturulunca bu methotlar taşınacak
    [System.Web.Mvc.Authorize(Roles = "Admin,Personel")]
    public class SharedDataApiController : ApiController
    {
        public List<KeyValuePair<int, string>> GetSelectorCities()
        {
            var cities = ConfigurationHelper.UnitOfWorkInstance.GetRepository<City>()
                 .SelectAll()
                 .AsEnumerable()
                 .Select(q => new KeyValuePair<int, string>(q.Id, q.Name))
                 .ToList();

            return cities;
        }

        public List<KeyValuePair<int, string>> GetSelectorGenders()
        {
            return EnumHelper.ToDictionary<EnumGender>()
                .Select(q=>new KeyValuePair<int,string>(q.Key,q.Value))
                .ToList();
             
        }
    }
}