using System;
using System.Web.Mvc;

namespace CustomerTracker.Web.Angular.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class AjaxOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var request = filterContext.HttpContext.Request;
            if (!request.IsAjaxRequest())
                filterContext.Result = new HttpNotFoundResult("Only Ajax calls are permitted.");
        }
    }
}