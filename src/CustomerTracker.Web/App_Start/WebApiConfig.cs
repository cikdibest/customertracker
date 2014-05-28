using System.Web.Http;
using System.Web.Mvc;

namespace CustomerTracker.Web.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                 name: "DefaultApi",
                 routeTemplate: "api/{controller}/{id}",
                 defaults: new { id = RouteParameter.Optional }
             );
         
            config.Routes.MapHttpRoute(
                name: "ApiByAction",
                routeTemplate: "api/{controller}/{action}",
                defaults: new { action = "Get" }
            );

            config.Routes.MapHttpRoute(
            name: "ApiByActionAndId",
            routeTemplate: "api/{controller}/{action}/{id}",
            defaults: new { id=RouteParameter.Optional}
            );

         
            //config.Routes.MapHttpRoute(
            //    null, 
            //    "api/{controller}/{pageNumber}/{sortBy}/{sortDir}",
            //    new { pageNumber = UrlParameter.Optional, sortBy = UrlParameter.Optional, Action = "Get" });


            var json = config.Formatters.JsonFormatter;
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore; 

        
        }
    }
}
