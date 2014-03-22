using System.Web.Routing;
using System.Web.Hosting;

namespace Spoon.Sample
{
    public class MvcApplication : System.Web.HttpApplication
    {
        // TODO: Replace the port number with your own.
        readonly string[] _urls = { "http://localhost:53825/" };

        protected void Application_Start()
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            SnapshotGenerator.InitializeAsync(_urls, HostingEnvironment.MapPath("~/Snapshots/")).Wait();
        }
    }
}