using MvcPaging;

namespace CustomerTracker.Web.Business.SearchBusiness
{
    public interface ISearchEngine
    {
        IPagedList<SearchResultModel> SearchCustomers(string criteria, int currentPageIndex, string sorting, bool isAscending);

        IPagedList<SearchResultModel> SearchCommunications(string criteria, int currentPageIndex, string sorting, bool isAscending);

        IPagedList<SearchResultModel> SearchRemoteComputers(string criteria, int currentPageIndex, string sorting, bool isAscending);

    }
}