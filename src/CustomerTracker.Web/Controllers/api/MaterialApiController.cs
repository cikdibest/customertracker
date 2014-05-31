using System;
using System.Web.Http;
using CustomerTracker.Web.App_Start;
using CustomerTracker.Web.Business.SearchBusiness;
using CustomerTracker.Web.Models.Enums;
using MvcPaging;
using Ninject;

namespace CustomerTracker.Web.Controllers.api
{
    public class MaterialApiController : ApiController
    {
        public class SearchModel
        {
            public string searchCriteria { get; set; }

            public int searchTypeId { get; set; }
        }

        [HttpPost]
        public IPagedList<SearchResultModel> SearchMaterials(SearchModel searchModel)
        {
            ISearchEngine searchEngine = NinjectWebCommon.GetKernel.Get<ISearchEngine>();

            IPagedList<SearchResultModel> resultModels = null;

            switch ((SearchType)searchModel.searchTypeId)
            {
                case SearchType.Customer:
                    resultModels = searchEngine.SearchCustomers(searchModel.searchCriteria, 0, "Id", false);
                    break;
                case SearchType.Communication:
                    resultModels = searchEngine.SearchCommunications(searchModel.searchCriteria, 0, "Id", false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
                    break;

            }

            return resultModels;

        }


    }
}