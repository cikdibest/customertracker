using System.Collections.Generic;
using System.Linq;
using System.Web.DynamicData;
using System.Web.Mvc;
using CustomerTracker.Web.Infrastructure.Repository;
using CustomerTracker.Web.Models.Entities;
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

    public class IndexSearchEngine : ISearchEngine
    {
        private readonly IUnitOfWork _unitOfWork;

        public IndexSearchEngine(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IPagedList<SearchResultModel> SearchCustomers(string criteria, int currentPageIndex, string sorting, bool isAscending)
        {
            var results = _unitOfWork.GetRepository<Customer>()
                .Filter(c => c.Name.Contains("/" + criteria + "/"));
            return ToPagedSearchResultModel<Customer>(results,currentPageIndex,20);
        }

        public IPagedList<SearchResultModel> SearchCommunications(string criteria, int currentPageIndex, string sorting, bool isAscending)
        {
            var results = _unitOfWork.GetRepository<Communication>()
               .Filter(c => c.FirstName.Contains("/" + criteria + "/") ||c.LastName.Contains("/"+criteria+"/") || (c.FirstName+" "+c.LastName).Contains("/"+criteria+"/"));
            return ToPagedSearchResultModel<Communication>(results, currentPageIndex, 20);
        }

        public IPagedList<SearchResultModel> SearchRemoteComputers(string criteria, int currentPageIndex, string sorting, bool isAscending)
        {
            var results = _unitOfWork.GetRepository<RemoteComputer>()
                .Filter(c => c.Name.Contains("/" + criteria + "/"));
            return ToPagedSearchResultModel<RemoteComputer>(results, currentPageIndex, 20);
        }

        private IPagedList<SearchResultModel> ToPagedSearchResultModel<T>(IQueryable<T> results, int pageIndex, int pageSize) where T : BaseEntity
        {
            var type = typeof(T).ToString();
            var searchResultList = new List<SearchResultModel>();
            switch (type)
            {
                case "Customer":

                    foreach (var customer in results)
                    {
                        var castedCustomer = customer as Customer;
                        searchResultList.Add(new SearchResultModel() { SearchResultId = customer.Id, Summary = castedCustomer.Explanation, Title = castedCustomer.Name + "-" + castedCustomer.City.Name });
                    }
                    return searchResultList.ToPagedList(pageIndex, pageSize);

                case "Communication":
                    foreach (var communication in results)
                    {
                        var castedCommunication = communication as Communication;
                        searchResultList.Add(new SearchResultModel()
                        {
                            SearchResultId = communication.Id,
                            Summary =
                                castedCommunication.Customer.Name + "-" + castedCommunication.Customer.City.Name +
                                "Tel : " + castedCommunication.PhoneNumber + "Email :" + castedCommunication.Email,
                            Title = castedCommunication.FirstName + castedCommunication.LastName
                        });
                    }
                    return searchResultList.ToPagedList(pageIndex, pageSize);
                case "RemoteComputer":
                    foreach (var communication in results)
                    {
                        var castedRemoteComputer = communication as RemoteComputer;
                        searchResultList.Add(new SearchResultModel()
                        {
                            SearchResultId = communication.Id,
                            Summary =
                                castedRemoteComputer.Customer.Name + "-" + castedRemoteComputer.Customer.City.Name + "-" +
                                castedRemoteComputer.RemoteConnectionType.ToString() + "Kullanıcı adı : " + castedRemoteComputer.Username + " Şifre :" + castedRemoteComputer.Password
                                + " " + castedRemoteComputer.Explanation,
                            Title = castedRemoteComputer.Name
                        });
                    }
                    return searchResultList.ToPagedList(pageIndex, pageSize);

                default:
                    return null;
            }
        }
    }
}
