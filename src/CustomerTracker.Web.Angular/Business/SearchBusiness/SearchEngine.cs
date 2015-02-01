using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CustomerTracker.Data.Model.Entities;
using CustomerTracker.Web.Angular.Infrastructure.Repository;
using CustomerTracker.Web.Angular.Models.Enums;
using CustomerTracker.Web.Angular.Utilities;
using MvcPaging;

namespace CustomerTracker.Web.Angular.Business.SearchBusiness
{
    public class SearchEngine : ISearchEngine
    {

        public IPagedList<Customer> SearchCustomers(string criteria, int currentPageIndex, string sorting, bool isAscending)
        {
            var customers = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Customer>()
                .SelectAll() 
                .Where(c => c.Name.Contains(criteria))
                .OrderBy(q=>q.Name)
                .AsEnumerable();

            var list = customers.ToPagedList(currentPageIndex, 1000);
            return list;

            //return ToPagedSearchResultModel<Customer>(results,currentPageIndex,20);
        }

        public IPagedList<Communication> SearchCommunications(string criteria, int currentPageIndex, string sorting, bool isAscending)
        {
            var communications = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Communication>()
                .SelectAll() 
                .Where(c => c.FirstName.Contains(criteria) || c.LastName.Contains(criteria) || (c.FirstName + " " + c.LastName).Contains(criteria))
                .OrderBy(q=>q.FirstName)
                .AsEnumerable();

            var list = communications.ToPagedList(currentPageIndex, 1000);
            return list;
        }

        
    }
}