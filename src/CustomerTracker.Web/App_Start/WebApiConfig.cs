using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Routing;
using CustomerTracker.Web.Models.Attributes;

namespace CustomerTracker.Web.App_Start
{
#warning bu geliştirelebilir...api lerin busines exceptionlar ı ypaıbilr ve burdan response lar yönetilebilir
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //config.Routes.MapHttpRoute(
            //     name: "DefaultApi",
            //     routeTemplate: "api/{controller}/{id}",
            //     defaults: new { id = RouteParameter.Optional }
            // );

            //config.Routes.MapHttpRoute(
            //    name: "ApiByAction",
            //    routeTemplate: "api/{controller}/{action}",
            //    defaults: new { action = "Get" }
            //);

            //config.Routes.MapHttpRoute(
            //name: "ApiByActionAndId",
            //routeTemplate: "api/{controller}/{action}/{id}",
            //defaults: new { id=RouteParameter.Optional}
            //);

            config.Routes.MapHttpRoute("DefaultApiWithId", "api/{controller}/{action}/{id}", new { id = RouteParameter.Optional }, new { id = @"\d+" });
            config.Routes.MapHttpRoute("DefaultApiWithAction", "api/{controller}/{action}");
            config.Routes.MapHttpRoute("DefaultApiGet", "api/{controller}/{action}", new { action = "Get" }, new { httpMethod = new HttpMethodConstraint(HttpMethod.Get) });
            config.Routes.MapHttpRoute("DefaultApiPost", "api/{controller}/{action}", new { action = "Post" }, new { httpMethod = new HttpMethodConstraint(HttpMethod.Post) });
            config.Routes.MapHttpRoute("DefaultApiPut", "api/{controller}/{action}", new { action = "Put" }, new { httpMethod = new HttpMethodConstraint(HttpMethod.Put) });
            config.Routes.MapHttpRoute("DefaultApiDelete", "api/{controller}/{action}", new { action = "Delete" }, new { httpMethod = new HttpMethodConstraint(HttpMethod.Delete) });

            config.Filters.Add(new ApiExceptionHandlingAttribute());

            var json = config.Formatters.JsonFormatter;
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;


        }
    }
}
