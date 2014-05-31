using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using CustomerTracker.Web.App_Start;
using CustomerTracker.Web.Business.SearchBusiness;
using CustomerTracker.Web.Models.Entities;
using CustomerTracker.Web.Models.Enums;
using CustomerTracker.Web.Utilities;
using MvcPaging;
using Ninject;

namespace CustomerTracker.Web.Controllers.api
{
    public class MaterialApiController : ApiController
    {
        public IPagedList<SearchResultModel> SearchMaterials(string searchCriteria, int searchTypeId)
        {
            ISearchEngine searchEngine = NinjectWebCommon.GetKernel.Get<ISearchEngine>();

            IPagedList<SearchResultModel> resultModels;

            switch ((SearchType)searchTypeId)
            {
                case SearchType.Customer:
                    resultModels = searchEngine.SearchCustomers(searchCriteria, 0, "Id", false);
                    break;
                case SearchType.Communication:
                    resultModels = searchEngine.SearchCommunications(searchCriteria, 0, "Id", false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
                    break;

            }

            return resultModels;

        }

      
    }
}