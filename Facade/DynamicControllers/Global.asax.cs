using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DynamicControllers
{
	// http://www.strathweb.com/2012/06/using-controllers-from-an-external-assembly-in-asp-net-web-api/
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            GlobalConfiguration.Configuration.Services.Replace(typeof(IAssembliesResolver), new CustomAssemblyResolver());
        }

        // TODO : Figure out DI, AutoFac shit.
    }

    public class CustomAssemblyResolver : IAssembliesResolver
    {
        public ICollection<Assembly> GetAssemblies()
        {
            var files = System.IO.Directory.GetFiles(@"V:\Dropbox\Code\DynamicControllers\Modules");
            List<Assembly> baseAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();

            foreach (var assemblyFile in files)
            {
                var controllersAssembly = Assembly.LoadFrom(assemblyFile);
                baseAssemblies.Add(controllersAssembly);
            }

            return baseAssemblies;
        }
    }
}