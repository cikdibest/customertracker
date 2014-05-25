using System;
using System.Collections.Generic;
using System.Web.DynamicData;
using System.Web.Mvc;
using System.Web.Routing;
using CustomerTracker.Web.App_Start;
using CustomerTracker.Web.Business;
using CustomerTracker.Web.Business.SearchBusiness;
using CustomerTracker.Web.Infrastructure.Repository;
using CustomerTracker.Web.Models.Enums;
using CustomerTracker.Web.Utilities;
using CustomerTracker.Web.Utilities.Helpers;
using MvcPaging;
using Ninject;

namespace CustomerTracker.Web.Controllers
{
    [Authorize(Roles = "Personel,Admin")]
    public class SearchController : Controller
    {
        private ISearchEngine _searchEngine;

        public SearchController()
        {
            _searchEngine = NinjectWebCommon.GetKernel.Get<ISearchEngine>();

        }

        public ActionResult Index()
        {
            Dictionary<int, string> searchTypes = EnumHelper.ToDictionary<SearchType>();
            ViewBag.SearchTypes = searchTypes;

            return View();
        }

        public JsonResult Search(string searchCriteria, int searchTypeId)
        { 
            IPagedList<SearchResultModel> resultModels;

            switch ((SearchType)searchTypeId)
            {
                case SearchType.Customer:
                    resultModels = _searchEngine.SearchCustomers(searchCriteria, 0, "Id", false);
                    break;
                case SearchType.Communication:
                    resultModels = _searchEngine.SearchCommunications(searchCriteria, 0, "Id", false);
                    break; 
                default:
                    throw new ArgumentOutOfRangeException();
                    break;

            }

            return Json(resultModels, JsonRequestBehavior.AllowGet);
        }
    }

    //public class NinjectControllerFactory : DefaultControllerFactory
    //{
    //    protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
    //    { 
    //        return controllerType == null
    //               ? null
    //               : (IController)NinjectWebCommon.GetKernel.Get(controllerType);

    //        //return base.GetControllerInstance(requestContext, controllerType);
    //    }
    //}
}
