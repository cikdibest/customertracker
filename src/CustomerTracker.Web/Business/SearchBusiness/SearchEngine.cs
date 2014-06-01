using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CustomerTracker.Web.Infrastructure.Repository;
using CustomerTracker.Web.Models.Entities;
using CustomerTracker.Web.Models.Enums;
using CustomerTracker.Web.Utilities;
using MvcPaging;

namespace CustomerTracker.Web.Business.SearchBusiness
{
    public class SearchEngine : ISearchEngine
    {

        public IPagedList<Customer> SearchCustomers(string criteria, int currentPageIndex, string sorting, bool isAscending)
        {
            var customers = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Customer>()
                .SelectAll()
                .Include(cu => cu.City) 
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
                .SelectAll().Include(co => co.Customer)
                .Include(cu => cu.Department) 
                .Where(c => c.FirstName.Contains(criteria) || c.LastName.Contains(criteria) || (c.FirstName + " " + c.LastName).Contains(criteria))
                .OrderBy(q=>q.FirstName)
                .AsEnumerable();

            var list = communications.ToPagedList(currentPageIndex, 1000);
            return list;
        }

        //private IPagedList<dynamic> ToPagedSearchResultModel<T>(IQueryable<T> results, int pageIndex, int pageSize) where T : BaseEntity
        //{
        //    var type = typeof(T).Name;
        //    var searchResultList = new List<SearchResultModel>();
        //    switch (type.ToLower())
        //    {
        //        case "customer":
        //            foreach (var customer in results)
        //            {
        //                var castedCustomer = customer as Customer;
        //                searchResultList.Add(new SearchResultModel()
        //                {
        //                    SearchTypeId = SearchType.Customer.GetHashCode(), 
        //                    Url = string.Format("{0}?customerId={1}", "api/customerapi/getcustomeradvanceddetail", customer.Id), 
        //                    Summary = castedCustomer.Explanation,
        //                    Title = castedCustomer.Name + "-" + castedCustomer.City.Name
        //                });
        //            }
        //            return searchResultList.ToPagedList(pageIndex, pageSize);

        //        case "communication":
        //            foreach (var communication in results)
        //            {
        //                var castedCommunication = communication as Communication;
        //                searchResultList.Add(new SearchResultModel()
        //                {
        //                    SearchTypeId = SearchType.Communication.GetHashCode(),
        //                    Url = string.Format("{0}?customerId={1}", "api/customerapi/getcustomeradvanceddetail", castedCommunication.Customer.Id),
        //                    Summary =
        //                        castedCommunication.Customer.Name + "-" + castedCommunication.Customer.City.Name +
        //                        "Tel : " + castedCommunication.MobilePhoneNumber + "Email :" + castedCommunication.Email,
        //                    Title = castedCommunication.FirstName + castedCommunication.LastName
        //                });
        //            }
        //            return searchResultList.ToPagedList(pageIndex, pageSize);

        //        default:
        //            return null;
        //    }
        //}
    }
}