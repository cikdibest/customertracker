using System;
using System.Web.DynamicData;
using System.Web.Mvc;
using System.Web.Routing;
using CustomerTracker.Web.App_Start;
using CustomerTracker.Web.Business;
using CustomerTracker.Web.Business.SearchBusiness;
using CustomerTracker.Web.Infrastructure.Repository;
using CustomerTracker.Web.Utilities;
using MvcPaging;
using Ninject;

namespace CustomerTracker.Web.Controllers
{
    [Authorize(Roles = "Personel,Admin")]
    public class LandingPageController : Controller
    {
        private ISearchEngine _searchEngine;

        public LandingPageController()
        {
            _searchEngine = NinjectWebCommon.GetKernel.Get<ISearchEngine>();

        }

        public ActionResult Index(object searchResultModels = null)
        {
            return View(searchResultModels as IPagedList<SearchResultModel>);

        }

        public JsonResult Search(string searchCriteria, string searchType)
        {
            IPagedList<SearchResultModel> resultModels;

            switch (searchType)
            {
                case "customer":
                    resultModels = _searchEngine.SearchCustomers(searchCriteria, 0, "Id", false);
                    break;
                case "communication":
                    resultModels = _searchEngine.SearchCommunications(searchCriteria, 0, "Id", false);
                    break;
                case "remotecomputer":
                    resultModels = _searchEngine.SearchRemoteComputers(searchCriteria, 0, "Id", false);
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
