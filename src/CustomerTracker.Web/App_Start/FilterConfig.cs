using System.Web.Mvc;
using CustomerTracker.Web.Models.Attributes;

namespace CustomerTracker.Web.App_Start
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