Spoon
---------------------------------------------------------------------------------------------------

To use Spoon, insert the following code into the Application_Start() method in your global.asax.

---

// Dictionary mapping escaped fragments to page URLs. This may be generated from a Sitemap.
var escapedFragmentUrlPairs = new Dictionary<string, string>
{
	{ "home", "http://www.example.com/home" },
	{ "about", "http://www.example.com/about" },
	{ "contact", "http://www.example.com/contact" },
};

var snapshotsPath = HostingEnvironment.MapPath("[Path to snapshot folder]");
foreach (var file in new DirectoryInfo(snapshotsPath).EnumerateFiles())
	file.Delete();
SnapshotManager.InitializeAsync(escapedFragmentUrlPairs, snapshotsPath).Wait();

---

You may want to surround this with the `#if RELEASE` preprocessor directive to prevent snapshots
from being created during application debugging.

To hook Spoon into your site, rewrite your default action method as follows.

---

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

---

Please report any issues or bugs at https://github.com/LeviBotelho/spoon/issues.