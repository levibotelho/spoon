using System.Collections.Generic;
using System.IO;
using System.Web.Routing;
using System.Web.Hosting;

namespace Spoon.Sample
{
    public class MvcApplication : System.Web.HttpApplication
    {
        // These urls can be loaded from a sitemap.
        readonly Dictionary<string, string> _escapedFragmentUrlPairs = new Dictionary<string, string>
        {
            // TODO: Replace the port numbers with your own.
            { "home", "http://localhost:53825/home" }, { "about", "http://localhost:53825/about" }, { "contact", "http://localhost:53825/contact" }
        };

        protected void Application_Start()
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
#if !DEBUG
            var snapshotsPath = HostingEnvironment.MapPath("~/Snapshots/");
            // ReSharper disable once AssignNullToNotNullAttribute
            foreach (var file in new DirectoryInfo(snapshotsPath).EnumerateFiles())
                file.Delete();
            SnapshotManager.InitializeAsync(_escapedFragmentUrlPairs, snapshotsPath).Wait();
#endif
        }
    }
}