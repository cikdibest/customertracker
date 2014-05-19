using System;
using System.Web.DynamicData;
using System.Web.Mvc;
using CustomerTracker.Web.Business;
using CustomerTracker.Web.Business.SearchBusiness;
using CustomerTracker.Web.Utilities;
using MvcPaging;

namespace CustomerTracker.Web.Controllers
{
    [Authorize(Roles = "Personel,Admin")]
    public class LandingPageController : Controller
    {
        private ISearchEngine _searchEngine;

        public LandingPageController()
        {
            _searchEngine = new IndexSearchEngine(ConfigurationHelper.UnitOfWorkInstance);
        }

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Search(string searchCriteria, string searchType)
        {
            IPagedList<SearchResultModel> resultModels;
       
            switch (searchType)
            {
                case "customer":
                    resultModels = _searchEngine.SearchCustomers(searchCriteria, 0, "Id", false);
                    break;
                case "communication":
                    resultModels = _searchEngine.SearchRemoteComputers(searchCriteria, 0, "Id", false);
                    break;
                case "remotecomputer":
                    resultModels = _searchEngine.SearchRemoteComputers(searchCriteria, 0, "Id", false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
                    break;
                     
            }

            return View("Index");
        }
    }
}
