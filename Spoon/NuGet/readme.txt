Spoon
-----------------------------------------------------------------------------------------

IMPORTANT: If you plan on hosting your web app as an Azure Web Site, then this is not the
solution for you. Spoon requires the ability to be able to launch a separate process and
at the time of writing Azure Web Sites do not provide this privilege. Spoon Standalone was
developed with this exact need in mind and is available at
https://github.com/LeviBotelho/spoon/tree/master/Spoon.Standalone.

-----------------------------------------------------------------------------------------

To use Spoon, hook the following code into your `Application_Start()` method. 

	// Dictionary mapping escaped fragments to page URLs. This may be generated from a Sitemap.
	var escapedFragmentUrlPairs = new Dictionary<string, string>
	{
		{ "/home", "http://www.example.com/!#/home" },
		{ "/about", "http://www.example.com/!#/about" },
		{ "/contact", "http://www.example.com/!#/contact" },
	};
		
	// TODO: Fill in the path to the directory where your snapshots are to be stored.
	var snapshotsPath = HostingEnvironment.MapPath("");
	var snapshotsDirectory = new DirectoryInfo(snapshotsPath);
	foreach (var file in snapshotsDirectory.EnumerateFiles())
		file.Delete();
			
	SnapshotManager.InitializeAsync(escapedFragmentUrlPairs, snapshotsPath).Wait();

To serve spoon Snapshots to web crawlers, hook Spoon in to your site's default action method
like so.

	public async Task<ActionResult> Index(string _escaped_fragment_)
	{
		if (_escaped_fragment_ != null)
		{
		    var path = string.Empty;
		    try
		    {
		        path = await SnapshotManager.GetSnapshotPathAsync(_escaped_fragment_);
		    }
		    catch (ArgumentException)
		    {
		        // Failure logic here.
		    }
		    return File(path, "text/html");
		}
		
		return View();
	}

Before using Spoon in production, you should test it out using Fiddler or a similar tool.
A call to a standard site URL should return a fairly empty HTML page, whereas a call to an
`_escaped_fragment_` url should return an HTML page containing all of the content you
normally see in the browser.

-----------------------------------------------------------------------------------------

Problems? Don't like what you see?

I wrote this project out of personal necessity but would be happy to adapt it for use by
a wider audience if the need arises. Please feel free to make feature requests or bug
reports in the issue tracker (https://github.com/LeviBotelho/spoon/issues).