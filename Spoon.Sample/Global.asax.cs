using System.Web.Routing;

namespace Spoon.Sample
{
    public class MvcApplication : System.Web.HttpApplication
    {
        //readonly string[] _urls = {"http://localhost:", ""};

        protected void Application_Start()
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //SnapshotGenerator.GenerateSnapshotCollectionAsync()
        }
    }
}
