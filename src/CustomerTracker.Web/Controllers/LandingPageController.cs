using System.Collections.Generic;
using System.Web.Mvc;
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

      
    }

    public interface ISearchEngine
    {
        IPagedList<SearchResultModel> SearchCustomers(string criteria,int currentPageIndex,  string sorting, bool isAscending);

        IPagedList<SearchResultModel> SearchCommunications(string criteria, int currentPageIndex, string sorting, bool isAscending);

        IPagedList<SearchResultModel> SearchRemoteComputers(string criteria, int currentPageIndex, string sorting, bool isAscending);

    }


    public class SearchResultModel
    {
        public string Title { get; set; }

        public int SearchResultId { get; set; }

        public string Summary { get; set; }
    }
}
