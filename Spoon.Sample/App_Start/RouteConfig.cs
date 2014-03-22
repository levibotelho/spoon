using System.Web.Mvc;
using System.Web.Routing;

namespace Spoon.Sample
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("Default", "{*value}", new { controller = "Home", action = "Index" });
        }
    }
}
