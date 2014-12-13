using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;
using CustomerTracker.Web.Angular.Utilities;
using Ninject;
using NLog;

namespace CustomerTracker.Web.Angular.Models.Attributes
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

            var userName = ConfigurationHelper.CurrentUser != null ? ConfigurationHelper.CurrentUser.UserName : string.Empty;

            string errorMessage = string.Format("Username={0}.ExceptionMessage={1}", userName, context.Exception.Message);

            _logger.Error(errorMessage, context.Exception);
              
            throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(errorMessage),
                ReasonPhrase = "Critical Exception"
            });
        }
    }
}