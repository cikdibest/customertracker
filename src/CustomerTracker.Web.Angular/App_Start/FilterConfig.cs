using System.Web.Mvc;
using CustomerTracker.Web.Angular.Models.Attributes;

namespace CustomerTracker.Web.Angular
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CustomHandleErrorAttribute());   
        }
    }
}