function GetUrls(cb) {
    if (config.sitemapUrl) {
        var sitemapReader = require('./src/sitemap-reader');
        sitemapReader.getUrls(config.sitemapUrl, cb);
    } else if (config.urls) {
        cb(null, config.urls);
    } else {
        cb('A sitemap URL or an array of URLs to crawl must be defined in the application config.');
    }
}

function GetBlobNames(urls) {
    for (var i = 0; i < urls.length; i++) {
        var url = urls[i];
        if (blobNames.indexOf(url) != -1)
            throw new Error('Multiple URLs in the array share the same escaped fragment. Each url must have a unique escaped fragment and only ' +
                'one url may not have an escaped fragment at all.');
        blobNames.push(blobNameGenerator.generate(url));
    }
    return blobNames;
}

function ScrapeUrls(urls, blobNames) {
    scraper.scrapeUrls(urls, function(err, result, i) {
        if (err) console.error(err);
        blobWriter.putBlob(blobNames[i], result);
        console.log(blobNames[i] + ' saved.');
    });
}

var config = require('./src/config');
var BlobWriter = require('./src/blob-writer');
var blobWriter = new BlobWriter(config.azureStorageAccount, config.azureAccessKey, config.azureContainerName);
var blobNameGenerator = require('./src/blob-name-generator');
var scraper = require('./src/scraper');
var blobNames = [];

GetUrls(function(err, urls) {
    if (err) console.error(err);
    var blobNames = GetBlobNames(urls);
    blobWriter.clearContainer();
    console.log("Blob container cleared.");
    ScrapeUrls(urls, blobNames);
});