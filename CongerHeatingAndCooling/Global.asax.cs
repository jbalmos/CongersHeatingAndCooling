using Autofac;
using Autofac.Integration.Mvc;
using System;
using System.Configuration;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace CongerHeatingAndCooling
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private static IContainer autofacContainer;

        protected void Application_Start()
        {
            autofacContainer = InitializeAutofac();

            InitializeMvc(autofacContainer);
        }

        protected void Application_End()
        {
            autofacContainer.Dispose();
        }


        private void InitializeMvc(IContainer container)
        {
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            //AreaRegistration.RegisterAllAreas();
            //RegisterGlobalFilters(GlobalFilters.Filters);
            //EslRepsRoutes.RegisterRoutes(RouteTable.Routes);
            //EslRepsBundler.RegisterBundles(BundleTable.Bundles, eslRepsConfig.EnableContentMinification);
            //ModelBinders.Binders.Add(typeof(DataTablePagingRequest), new DataTablePagingRequestBinder());
            //DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            // default client validation message override
            // only way this seems to stick (HttpContext.GetGlobalResourceObject) is by sticking it in App_GlobalResources
            //ClientDataTypeModelValidatorProvider.ResourceClassKey = typeof(Resources.ClientDataTypeModelValidatorProviderMessages).Name;
           // DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(RequiredIfAttribute), typeof(RequiredAttributeAdapter));
        }

        private IContainer InitializeAutofac()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterModule(new AutofacWebTypesModule());
            builder.RegisterModule(new ChcAutofacModule
            {
                ChcDbConnectionString = ReadConnectionString("DefaultConnection")
            });

            return builder.Build();
        }

        public static string ReadConnectionString(string connectionStringName)
        {
            var settings = ConfigurationManager.ConnectionStrings[connectionStringName];
            if (settings == null)
            {
                throw new ConfigurationErrorsException(String.Format(
                    "Unable to locate connection string named '{0}' in the configuration file.",
                    connectionStringName));
            }

            return settings.ConnectionString;
        }
    }
}