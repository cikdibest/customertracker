using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CustomerTracker.Web.Utilities.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static string ToJson(this HtmlHelper htmlHelper,object obj)
        {
            var settings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
            //settings.Converters.Add(new JavaScriptDateTimeConverter());
            return JsonConvert.SerializeObject(obj,Formatting.None,settings);

        }
    }
}