using System;
using System.Web.Http;
using CustomerTracker.Web.App_Start;
using CustomerTracker.Web.Business.SearchBusiness;
using CustomerTracker.Web.Models.Attributes;
using CustomerTracker.Web.Models.Enums;
using MvcPaging;
using Ninject;

namespace CustomerTracker.Web.Controllers.api
{
    [CustomAuthorize(Roles = "Admin,Personel")]
    public class MaterialApiController : ApiController
    {
        public class SearchModel
        {
            public string searchCriteria { get; set; }

            public int searchTypeId { get; set; }
        }

        [HttpPost]
        public dynamic SearchMaterials(SearchModel searchModel)
        {
            var searchEngine = NinjectWebCommon.GetKernel.Get<ISearchEngine>();
             
            switch ((SearchType)searchModel.searchTypeId)
            {
                case SearchType.Customer:
                    return new { materials = searchEngine.SearchCustomers(searchModel.searchCriteria, 0, "Id", false), searchTypeId = SearchType.Customer.GetHashCode() };
                    break;
                case SearchType.Communication:
                   return new {materials=searchEngine.SearchCommunications(searchModel.searchCriteria, 0, "Id", false),searchTypeId = SearchType.Communication.GetHashCode()};
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
                    break;

            }

           

        }


    }
}