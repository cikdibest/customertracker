using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CustomerTracker.Web.Infrastructure.Repository;
using CustomerTracker.Web.Models.Entities;
using CustomerTracker.Web.Utilities;
using MvcPaging;

namespace CustomerTracker.Web.Business.SearchBusiness
{
    public class SearchEngine : ISearchEngine
    {
       
        public IPagedList<SearchResultModel> SearchCustomers(string criteria, int currentPageIndex, string sorting, bool isAscending)
        {
            var results = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Customer>().SelectAll().Include(cu=>cu.City)
                .Where(c => c.Title.Contains(criteria));
            return ToPagedSearchResultModel<Customer>(results,currentPageIndex,20);
        }

        public IPagedList<SearchResultModel> SearchCommunications(string criteria, int currentPageIndex, string sorting, bool isAscending)
        {
            var results = ConfigurationHelper.UnitOfWorkInstance.GetRepository<Communication>().SelectAll().Include(co=>co.Customer).Include(co=>co.Customer.City)
                .Where(c => c.FirstName.Contains(criteria) ||c.LastName.Contains(criteria) || (c.FirstName+" "+c.LastName).Contains(criteria));
            return ToPagedSearchResultModel<Communication>(results, currentPageIndex, 20);
        }

        public IPagedList<SearchResultModel> SearchRemoteMachines(string criteria, int currentPageIndex, string sorting, bool isAscending)
        {
            var results = ConfigurationHelper.UnitOfWorkInstance.GetRepository<RemoteMachine>().SelectAll().Include(re=>re.Customer).Include(re=>re.Customer.City)
                .Where(c => c.Name.Contains(criteria));
            return ToPagedSearchResultModel<RemoteMachine>(results, currentPageIndex, 20);
        }

        private IPagedList<SearchResultModel> ToPagedSearchResultModel<T>(IQueryable<T> results, int pageIndex, int pageSize) where T : BaseEntity
        {
            var type = typeof(T).Name;
            var searchResultList = new List<SearchResultModel>();
            switch (type.ToLower())
            {
                case "customer":
                    foreach (var customer in results)
                    {
                        var castedCustomer = customer as Customer;
                        searchResultList.Add(new SearchResultModel() { Url = "/customer/detail?customerId=" + customer.Id, Summary = castedCustomer.Explanation, Title = castedCustomer.Title + "-" + castedCustomer.City.Name });
                    }
                    return searchResultList.ToPagedList(pageIndex, pageSize);

                case "communication":
                    foreach (var communication in results)
                    {
                        var castedCommunication = communication as Communication;
                        searchResultList.Add(new SearchResultModel()
                        {
                            Url ="/communication/"+ communication.Id,
                            Summary =
                                castedCommunication.Customer.Title + "-" + castedCommunication.Customer.City.Name +
                                "Tel : " + castedCommunication.PhoneNumber + "Email :" + castedCommunication.Email,
                            Title = castedCommunication.FirstName + castedCommunication.LastName
                        });
                    }
                    return searchResultList.ToPagedList(pageIndex, pageSize);
                case "remotemachine":
                    foreach (var communication in results)
                    {
                        var castedRemoteMachine = communication as RemoteMachine;
                        searchResultList.Add(new SearchResultModel()
                        {
                            Url = "/remotemachine/"+castedRemoteMachine.Id,
                            Summary =
                                castedRemoteMachine.Customer.Title + "-" + castedRemoteMachine.Customer.City.Name + "-" +
                                castedRemoteMachine.RemoteConnectionType + "Kullanıcı adı : " + castedRemoteMachine.Username + " Şifre :" + castedRemoteMachine.Password
                                + " " + castedRemoteMachine.Explanation,
                            Title = castedRemoteMachine.Name
                        });
                    }
                    return searchResultList.ToPagedList(pageIndex, pageSize);

                default:
                    return null;
            }
        }
    }
}