﻿using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Script.Serialization;
using System.Web.Security;
using CustomerTracker.Web.Angular.Controllers;
using CustomerTracker.Web.Angular.Infrastructure.Membership;
using CustomerTracker.Web.Angular.Infrastructure.Repository;
using CustomerTracker.Web.Angular.Infrastructure.Services.DistributedCacheService;
using CustomerTracker.Web.Angular.Infrastructure.Tasks;
using CustomerTracker.Web.Angular.Utilities;
using Ninject;

namespace CustomerTracker.Web.Angular
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        protected MvcApplication()
        {
            BeginRequest += (sender, args) =>
            {
                OnBeginRequest();
            };

            EndRequest += (sender, args) =>
            {
                OnEndRequest(Server);

                TaskExecutor.StartExecuting();

            };
        }

        private void OnBeginRequest()
        {
            ConfigurationHelper.UnitOfWorkInstance = new UnitOfWork();


        }

        private void OnEndRequest(HttpServerUtility server)
        {
            if (ConfigurationHelper.UnitOfWorkInstance != null)
            {
                ConfigurationHelper.UnitOfWorkInstance.Dispose();
                return;
            }
            //using (var unitOfWork = ConfigurationHelper.UnitOfWorkInstance)
            //{
            //    if (unitOfWork == null)
            //        return;

            //    if (server.GetLastError() != null)
            //        return;

            //    //unitOfWork.Save();
            //}
        }

        //protected override IKernel CreateKernel()
        //{
        //    //var modules = new INinjectModule[]
        //    //    {
        //    //        new BusinessModule(),
        //    //        new UtilitiesModule(), 
        //    //        new InfrastructureModule(), 
        //    //    };

        //    //var kernel = new StandardKernel(modules);
        //    var kernel = new StandardKernel();

        //    return kernel;
        //}

        protected void Application_Start()
        {

            // HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();

            //ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory());

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            RouteConfig.RegisterRoutes(RouteTable.Routes);

            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AuthConfig.RegisterAuth();

            AutoMapperConfiguration.Map();

            DatabaseConfiguration.StartMigration();

            CachingService.StartCaching();

        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie == null) return;

            var authTicket = FormsAuthentication.Decrypt(authCookie.Value);

            var serializer = new JavaScriptSerializer();

            var serializeModel = serializer.Deserialize<UserPrincipalSerializeModel>(authTicket.UserData);

            var newUser = new UserPrincipal(authTicket);

            newUser.UserId = serializeModel.UserId;

            newUser.UserName = serializeModel.UserName;

            newUser.FirstName = serializeModel.FirstName;

            newUser.LastName = serializeModel.LastName;

            newUser.Roles = serializeModel.Roles;

            HttpContext.Current.User = newUser;

            //var principal = HttpContext.Current.User;
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            // your code here, it will be executed upon session start
        }
        protected void Session_End(object sender, EventArgs e)
        {
            // your code here, it will be executed upon session start
        }
    }
}