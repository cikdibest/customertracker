using System.Collections.Generic;
using MvcPaging;

namespace CustomerTracker.Web.Business.SearchBusiness
{
    public interface ISearchEngine
    {
        IPagedList<SearchResultModel> SearchCustomers(string criteria, int currentPageIndex, string sorting, bool isAscending);

        IPagedList<SearchResultModel> SearchCommunications(string criteria, int currentPageIndex, string sorting, bool isAscending);

        IPagedList<SearchResultModel> SearchRemoteComputers(string criteria, int currentPageIndex, string sorting, bool isAscending);

    }

    public class FakeSearchEngine : ISearchEngine
    {
        public IPagedList<SearchResultModel> SearchCustomers(string criteria, int currentPageIndex, string sorting, bool isAscending)
        {
            return new PagedList<SearchResultModel>(new List<SearchResultModel>()
            {
                new SearchResultModel() { Summary = "sumary1", Title = "title1", Url = "url1" },
                new SearchResultModel() { Summary = "sumary2", Title = "title2", Url = "url2" },
                new SearchResultModel() { Summary = "sumary3", Title = "title3", Url = "url3" },
                new SearchResultModel() { Summary = "sumary4", Title = "title4", Url = "url4" },

            }, currentPageIndex, 20);
        }

        public IPagedList<SearchResultModel> SearchCommunications(string criteria, int currentPageIndex, string sorting, bool isAscending)
        {
            return new PagedList<SearchResultModel>(new List<SearchResultModel>()
            {
                new SearchResultModel() { Summary = "sumary1", Title = "title1", Url = "url1" },
                new SearchResultModel() { Summary = "sumary2", Title = "title2", Url = "url2" },
                new SearchResultModel() { Summary = "sumary3", Title = "title3", Url = "url3" },
                new SearchResultModel() { Summary = "sumary4", Title = "title4", Url = "url4" },

            }, currentPageIndex, 20);
        }

        public IPagedList<SearchResultModel> SearchRemoteComputers(string criteria, int currentPageIndex, string sorting, bool isAscending)
        {
            return new PagedList<SearchResultModel>(new List<SearchResultModel>()
            {
                new SearchResultModel() { Summary = "sumary1", Title = "title1", Url = "url1" },
                new SearchResultModel() { Summary = "sumary2", Title = "title2", Url = "url2" },
                new SearchResultModel() { Summary = "sumary3", Title = "title3", Url = "url3" },
                new SearchResultModel() { Summary = "sumary4", Title = "title4", Url = "url4" },

            }, currentPageIndex, 20);
        }
    }
}