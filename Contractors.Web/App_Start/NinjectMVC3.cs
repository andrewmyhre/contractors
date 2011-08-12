using CommonServiceLocator.NinjectAdapter;
using Contractors.Core;
using Contractors.Web.Controllers;
using Microsoft.Practices.ServiceLocation;
using OAuth.Net.Common;
using OAuth.Net.Consumer;
using OAuth.Net.Consumer.Components;
using Raven.Client;

[assembly: WebActivator.PreApplicationStartMethod(typeof(Contractors.Web.App_Start.NinjectMVC3), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(Contractors.Web.App_Start.NinjectMVC3), "Stop")]

namespace Contractors.Web.App_Start
{
    using System.Reflection;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Mvc;

    public static class NinjectMVC3 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestModule));
            DynamicModuleUtility.RegisterModule(typeof(HttpApplicationInitializationModule));
            bootstrapper.Initialize(CreateKernel);
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
            RegisterServices(kernel);

            var locator = new NinjectServiceLocator(kernel);

            ServiceLocator.SetLocatorProvider(() => locator);

            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<Raven.Client.IDocumentSession>().ToMethod(r => MvcApplication.RavenDocumentStore.OpenSession()).InRequestScope().OnDeactivation(
                delegate(IDocumentSession session) { session.SaveChanges();session.Dispose(); });
            kernel.Bind<IDbContext>().ToMethod(x=>new RavenDbContext(MvcApplication.RavenDocumentStore)).InSingletonScope();
            kernel.Bind<IDbSession>().ToMethod(x => new RavenDbSession(kernel.Get<IDocumentSession>())).InRequestScope();

            kernel.Bind<IAccountCredentialsService>().To<AccountCredentialsService>();
            kernel.Bind<IUserAccountService>().To<UserAccountService>();
            kernel.Bind<INonceProvider>().To<OAuth.Net.Components.GuidNonceProvider>();
            kernel.Bind<ISigningProvider>().To<OAuth.Net.Components.HmacSha1SigningProvider>().Named("signing.provider:HMAC-SHA1");
            kernel.Bind<IRequestStateStore>().To<SessionRequestStateStore>();
            /*kernel.Bind<System.Web.HttpContextBase>().ToMethod(
                r => new System.Web.HttpContextWrapper(System.Web.HttpContext.Current));*/
        }        
    }
}
