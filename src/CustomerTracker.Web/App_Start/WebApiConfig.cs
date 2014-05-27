using System.Web.Http;

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


            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
            config.Formatters.Remove(config.Formatters.XmlFormatter);


        }
    }
}
