using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CommandPattern.Core;
using CommandPattern.Helpers;
using CommandPattern.Runners;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Mvc;

namespace CommandPattern
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            GlobalFilters.Filters.Add(new HandleErrorAttribute());

            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            RouteTable.Routes.MapRoute(
                name: "Empty",
                url: "",
                defaults: new { controller = "Home", action = "Index" });

            RouteTable.Routes.MapRoute(
                name: "Command",
                url: "{commandModelName}",
                defaults: new { controller = "Command", action = "Execute" });

            InitUnity();
        }

        private static void InitUnity()
        {
            var container = new UnityContainer();

            RegisterCommands(container);

            FilterProviders.Providers.Remove(FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().First());
            FilterProviders.Providers.Add(new UnityFilterAttributeFilterProvider(container));

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }

        private static void RegisterCommands(IUnityContainer container)
        {
            LifetimeManager manager;
            var map = CommandReflectionHelper.GetCommandTypes();

            foreach (var pair in map)
            {
                manager = new ContainerControlledLifetimeManager();
                container.RegisterType(pair.Key, pair.Value, manager);
            }

            manager = new ContainerControlledLifetimeManager();
            container.RegisterType<ICommandRunner, UnityCommandRunner>(manager);
        }
    }
}