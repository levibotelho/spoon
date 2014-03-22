using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Spoon.Sample.Controllers
{
    public class HomeController : Controller
    {
        // ReSharper disable once InconsistentNaming
        public async Task<ActionResult> Index(string _escaped_fragment_)
        {
            if (_escaped_fragment_ != null)
            {
                var path = await SnapshotManager.GetSnapshotUrlAsync(_escaped_fragment_);
                if (path == null)
                    throw new ArgumentException("Invalid _escaped_fragment_");
                return File(path, "text/html");
            }

            return View();
        }
    }
}