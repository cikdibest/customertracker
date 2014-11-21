using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using CustomerTracker.Common.Helpers;
using CustomerTracker.Data.Model.Entities;
using CustomerTracker.Data.Model.Enums;
using CustomerTracker.Web.Angular.Models.Attributes;
using CustomerTracker.Web.Angular.Models.Enums;
using CustomerTracker.Web.Angular.Utilities;

namespace CustomerTracker.Web.Angular.Controllers.api
{
#warning ilgili apiler oluşturulunca bu methotlar taşınacak
    [CustomAuthorize(Roles = "Admin,Personel")]
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
                .Select(q => new KeyValuePair<int, string>(q.Key, q.Value))
                .ToList();

        }

        public List<KeyValuePair<int, string>> GetSelectorTroubles()
        {
            var troubles = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Trouble>()
               .SelectAll()
               .AsEnumerable()
               .Select(q => new KeyValuePair<int, string>(q.Id, q.Name))
               .ToList();

            return troubles;

        }

        public List<KeyValuePair<int, string>> GetSelectorRoles()
        {
            var roles = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Role>()
               .SelectAll()
               .AsEnumerable()
               .Select(q => new KeyValuePair<int, string>(q.Id, q.RoleName))
               .ToList();

            return roles;

        }
    }
}