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
            _searchEngine = default(ISearchEngine);
        }

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Search(string criteria, string searchType)
        {
            IPagedList<SearchResultModel> resultModels;
       
            switch (searchType)
            {
                case "customer":
                  resultModels=  _searchEngine.SearchCustomers(criteria, 0, "Id", false);
                    break;
                case "communication":
                    resultModels = _searchEngine.SearchRemoteComputers(criteria, 0, "Id", false);
                    break;
                case "remotecomputer":
                    resultModels = _searchEngine.SearchRemoteComputers(criteria, 0, "Id", false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
                    break;
                     
            }

            return View("Index");
        }
    }
}
