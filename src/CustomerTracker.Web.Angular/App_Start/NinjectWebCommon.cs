using System;
using System.Web;
using CustomerTracker.Web.Angular;
using CustomerTracker.Web.Angular.Business.SearchBusiness;
using CustomerTracker.Web.Angular.Business.UserBusiness;
using CustomerTracker.Web.Angular.Infrastructure.Repository;
using CustomerTracker.Web.Angular.Utilities;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Activation;
using Ninject.Modules;
using Ninject.Web.Common;
using NLog;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(NinjectWebCommon), "Stop")]

namespace CustomerTracker.Web.Angular
{
    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper Bootstrapper = new Bootstrapper();
      
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

            Bootstrapper.Initialize(() => { return kernel; });
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            Bootstrapper.ShutDown();
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
                return Bootstrapper.Kernel;
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
                return "CustomerTracker.Web.Angular.NinjectWebCommon";

            if (context.Request.ParentContext.Request.ParentContext == null)
                return context.Request.ParentContext.Request.Service.FullName;

            return context.Request.ParentContext.Request.ParentContext.Request.Service.FullName;
        }
    }
}
