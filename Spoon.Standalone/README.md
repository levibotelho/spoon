# Spoon.Standalone

Spoon Standalone is an alternative solution to Spoon, built to work in cases where it is not possible to launch a separate process to crawl your site. Azure Web Sites are a good example of this.

## How does it work?

Spoon Standalone works in much the same way as Spoon does, crawling your site and creating snapshots of your dynamic content that can be served up to search engines when they request it. However, there are two key differences.

+ Spoon Standalone is not embedded into a web application like Spoon is. It is launched manually once a given site is up and running.
+ Spoon Standalone stores its snapshots in an Azure blob storage container and not in a directory in your web application's hierarchy.
+ Spoon Standalone is a Node.js solution. I would be happy to port it to C# if there was a need for such a thing.

## How do I use it?

To use Spoon Standalone you will need an Azure storage account with a free blob storage container. This container must be reserved exclusively for Spoon, as Spoon will clear its contents before uploading snapshots in order to use as little space as possible.

To create your snapshots, download the Spoon Standalone project and fill in the config file located at `/src/config.js` as follows. 

	// The name of your storage account
	exports.azureStorageAccount = '';
	// The access key of your storage account
	exports.azureAccessKey = '';
	// The name of the container to use to store the snapshots
	exports.azureContainerName = '';

	// The URL of a text-style sitemap containing the links to crawl.
	exports.sitemapUrl = '';

	// An array of URLs to crawl. All URLs must be in hashbang form
	// (http://www.example.com/#!/about) with the exception of the home
	// page which does not have to be (http://www.example.com is valid).
	exports.urls = [];

Please note the following.

+ Spoon will crawl either a sitemap or the URLs in the URL array, not both. If a sitemap URL is specified, the URL array will be ignored. If you want to use the URL array then simply leave the sitemap URL as an empty string.
+ At the moment the only type of sitemap that is supported is a text file containing one URL per line. XML-style Sitemaps are not currently supported, but could be in the future if there was need for such a feature.


## Problems? Don't like what you see?
I wrote this project out of personal necessity but would be happy to adapt it for use by a wider audience if the need arises. Please feel free to make feature requests or bug reports in the issue tracker.