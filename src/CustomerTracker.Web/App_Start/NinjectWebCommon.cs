using System.Web.Mvc;
using CustomerTracker.Common;
using CustomerTracker.Web.Business;
using CustomerTracker.Web.Business.SearchBusiness;
using CustomerTracker.Web.Business.UserBusiness;
using CustomerTracker.Web.Controllers;
using CustomerTracker.Web.Infrastructure.Repository;
using CustomerTracker.Web.Utilities;
using Ninject.Activation;
using Ninject.Modules;
using NLog;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(CustomerTracker.Web.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(CustomerTracker.Web.App_Start.NinjectWebCommon), "Stop")]

namespace CustomerTracker.Web.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();
      
        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            var kernel = CreateKernel();
            kernel.Bind<IUnitOfWork>().ToMethod(context => { return ConfigurationHelper.UnitOfWorkInstance; });
           // kernel.Bind<IUnitOfWork>().To<UnitOfWork>().InRequestScope();
            kernel.Bind<ISearchEngine>().To<SearchEngine>().InSingletonScope();
            kernel.Bind<IUserUtility>().To<UserUtility>().InSingletonScope();
            //kernel.Bind<IEncrypt>().To<AesEncryption>().InSingletonScope();
            kernel.Bind<IMailBuilder>().To<MailBuilder>().InSingletonScope();
            kernel.Bind<IMailSenderUtility>().To<MailSenderUtility>().InSingletonScope();
            kernel.Load(new LoggingModule());

            bootstrapper.Initialize(() => { return kernel; });
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
        }

        public static IKernel GetKernel
        {
            get
            {
                return bootstrapper.Kernel;
            }
        }


    }

    public class LoggingModule : NinjectModule
    {
        public override void Load()
        {
            Bind<Logger>().ToMethod(x => NLog.LogManager.GetLogger(GetParentTypeName(x))).InTransientScope();
        }

        private string GetParentTypeName(IContext context)
        {
            if (context.Request.ParentContext == null)
                return "CustomerTracker.Web.App_Start.NinjectWebCommon";

            if (context.Request.ParentContext.Request.ParentContext == null)
                return context.Request.ParentContext.Request.Service.FullName;

            return context.Request.ParentContext.Request.ParentContext.Request.Service.FullName;
        }
    }
}
