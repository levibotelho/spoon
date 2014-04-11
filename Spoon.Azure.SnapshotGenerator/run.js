var config = require('./src/config');
var BlobWriter = require('./src/blob-writer');
var blobWriter = new BlobWriter(config.azureStorageAccount, config.azureAccessKey, config.azureContainerName);
var blobNameGenerator = require('./src/blob-name-generator');
var scraper = require('./src/scraper');
var blobNames = [];
var urls = config.urls;

for (var i = 0; i < urls.length; i++) {
    var url = urls[i];
    if (blobNames.indexOf(url) != -1)
        throw new Error('Multiple URLs in the array share the same escaped fragment. Each url must have a unique escaped fragment and only ' +
            'one url may not have an escaped fragment at all.');
    blobNames.push(blobNameGenerator.generate(url));
}

blobWriter.clearContainer();
console.log("Blob container cleared.");
scraper.scrapeUrls(urls, function(err, result, i) {
    if (err) {
        console.error(err);
    }
    blobWriter.putBlob(blobNames[i], result);
    console.log(blobNames[i] + ' saved.');
});