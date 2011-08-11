using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Contractors.Core;
using Contractors.Web.Code;
using Raven.Client;

namespace Contractors.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        //private static readonly string _ravenDbStoreKey="ravenDbStore";
        public static IDocumentStore RavenDocumentStore;

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Candidates", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            var ravenDbConfiguration = ConfigurationManager.GetSection("ravenDb") as IRavenDbConfigurationSection;
            RavenDocumentStore = ConfigureRavenDb(ravenDbConfiguration);
            RavenDocumentStore.Initialize();

            Indexes.InitialiseIndexes(RavenDocumentStore);
        }

        protected void Application_OnAuthenticateRequest(Object sender, EventArgs e)
        {
            if (Context.User != null)
            {
                if (Context.User.Identity.IsAuthenticated)
                {
                    var accountService =
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(
                            typeof (IUserAccountService)) as IUserAccountService;

                    var user = new WebUser(accountService.Retrieve(Context.User.Identity.Name));

                    if (user == null)
                    {
                        throw new ApplicationException("Context.User.Identity.Name is not a recognised user.");
                    }

                    System.Threading.Thread.CurrentPrincipal = Context.User = user;
                    return;
                }
            }
            System.Threading.Thread.CurrentPrincipal = Context.User = null;
        }

        private IDocumentStore ConfigureRavenDb(IRavenDbConfigurationSection ravenDbConfiguration)
        {
            if (ravenDbConfiguration.StorageType == StorageTypeEnum.Embedded)
                return new Raven.Client.Embedded.EmbeddableDocumentStore() { DataDirectory = ravenDbConfiguration.Location };
            else if (ravenDbConfiguration.StorageType == StorageTypeEnum.Http)
                return new Raven.Client.Document.DocumentStore() {Url = ravenDbConfiguration.Location};

            throw new ConfigurationErrorsException(string.Format("{0} is not a valid raven db storage type", ravenDbConfiguration.StorageType));
        }
    }
}