#Spoon

##What does it do?
Spoon provides search engine optimisation (SEO) for single page ASP.NET applications, such as those written in AngularJS, by making their dynamic content crawlable.

##How does it do this?
Spoon takes snapshots of your dynamic pages and hooks in to standard ASP.NET action methods to serve up these snapshots when a crawler requests a page by way of the ["escaped fragment" protocol](https://developers.google.com/webmasters/ajax-crawling/docs/specification).

##How do I use it?
1. Build and compile the sources to obtain a copy of `Spoon.dll`.
2. Reference the DLL in an ASP.NET project.
3. In your `global.asax`, add the following code to `Application_Start()`. 

		// Dictionary mapping escaped fragments to page URLs
		var escapedFragmentUrlPairs = new Dictionary<string, string>
		{
			{ "home", "http://www.example.com/home" },
			{ "about", "http://www.example.com/about" },
			{ "contact", "http://www.example.com/contact" },
		}
	
		var snapshotsPath = HostingEnvironment.MapPath("[Path to snapshot folder]");
		foreach (var file in new DirectoryInfo(snapshotsPath).EnumerateFiles())
			file.Delete();
		SnapshotManager.InitializeAsync(, snapshotsPath).Wait();

	You may want to surround this with the `#if RELEASE` preprocessor directive to prevent snapshots from being created during application debugging.

4. Hook in Spoon to your site's default Action method with the following code.

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

5. Test it out using Fiddler or a similar tool. A call to `http://www.mysite.com/home` (or possibly `http://www.mysite.com/#!/home`) should return a fairly empty HTML page, whereas a call to `http://www.mysite.com?_escaped_fragment_=home` should return an HTML page containing all of the content you normally see in the browser. 