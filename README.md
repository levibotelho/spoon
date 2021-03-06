#Spoon

## Deprecation notice

Google now supports crawling of web applications that use JavaScript to present their content, and has [deprecated](https://webmasters.googleblog.com/2015/10/deprecating-our-ajax-crawling-scheme.html) the escaped fragment Ajax crawling scheme that Spoon was designed to take advantage of. As such, this library is no longer needed, and no longer maintained.

##What does it do?
Spoon provides search engine optimisation (SEO) for single page ASP.NET applications, such as those written in AngularJS, by making their dynamic content crawlable.

##How does it do this?
Spoon takes snapshots of your dynamic pages and hooks in to standard ASP.NET action methods to serve up these snapshots when a crawler requests a page by way of the ["escaped fragment" protocol](https://developers.google.com/webmasters/ajax-crawling/docs/specification).

##How do I use it?

*Are you developing a Windows Azure Web Site? If so, then please see Spoon.Standalone.*

1. Grab a copy of Spoon off NuGet (`PM> Install-Package Spoon`)
2. Hook the following code into your `Application_Start()` method.

		// Dictionary mapping escaped fragments to page URLs. This may be generated from a Sitemap.
		var escapedFragmentUrlPairs = new Dictionary<string, string>
		{
		    { "/home", "http://www.example.com/#!/home" },
		    { "/about", "http://www.example.com/#!/about" },
		    { "/contact", "http://www.example.com/#!/contact" },
		};

		// TODO: Fill in the path to the directory where your snapshots are to be stored.
		var snapshotsPath = HostingEnvironment.MapPath("");
		var snapshotsDirectory = new DirectoryInfo(snapshotsPath);
		foreach (var file in snapshotsDirectory.EnumerateFiles())
		    file.Delete();

		SnapshotManager.InitializeAsync(escapedFragmentUrlPairs, snapshotsPath).Wait();

3. Hook in Spoon to your site's default Action method.

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

4. Test it out using Fiddler or a similar tool. A call to a standard site URL should return a fairly empty HTML page, whereas a call to an `_escaped_fragment_` url should return an HTML page containing all of the content you normally see in the browser.

## Problems? Don't like what you see?
I wrote this project out of personal necessity but would be happy to adapt it for use by a wider audience if the need arises. Please feel free to make feature requests or bug reports in the issue tracker.