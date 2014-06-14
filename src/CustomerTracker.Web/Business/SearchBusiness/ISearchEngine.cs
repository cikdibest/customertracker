using System.Collections.Generic;
using CustomerTracker.Data.Model.Entities;
using MvcPaging;

namespace CustomerTracker.Web.Business.SearchBusiness
{
    public interface ISearchEngine
    {
        IPagedList<Customer> SearchCustomers(string criteria, int currentPageIndex, string sorting, bool isAscending);

        IPagedList<Communication> SearchCommunications(string criteria, int currentPageIndex, string sorting, bool isAscending);

       
    }

     
}