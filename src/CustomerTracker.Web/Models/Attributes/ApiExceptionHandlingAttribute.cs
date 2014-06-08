using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;
using CustomerTracker.Web.App_Start;
using Ninject;
using NLog;

namespace CustomerTracker.Web.Models.Attributes
{
    public class ApiExceptionHandlingAttribute : ExceptionFilterAttribute
    {
        private readonly Logger _logger;

        public ApiExceptionHandlingAttribute()
        {
            _logger = NinjectWebCommon.GetKernel.Get<Logger>();

        }
        public override void OnException(HttpActionExecutedContext context)
        {
            //if (context.Exception is BusinessException)
            //{
            //    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
            //    {
            //        Content = new StringContent(context.Exception.Message),
            //        ReasonPhrase = "Exception"
            //    });

            //}

            //Log Critical errors
            _logger.Error(context.Exception);

            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("An error occurred, please try again or contact the administrator."),
                ReasonPhrase = "Critical Exception"
            });
        }
    }
}