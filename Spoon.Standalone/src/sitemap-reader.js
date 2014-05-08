function ParseChunk(chunk, urls) {
    chunk = chunk.toString();
    var currentIndex = urls.length - 1;
    var incremented = false;
    for (var i = 0, length = chunk.length; i < length; i++) {
        var character = chunk.charAt(i);
        if (character == '\n') {
            urls[currentIndex] = urls[currentIndex].trim();
            currentIndex++;
            incremented = true;
        } else {
            if (incremented) {
                incremented = false;
                urls[currentIndex] = character;
            } else {
                urls[currentIndex] += character;
            }
        }
    }
}

exports.getUrls = function(sitemapUrl, cb) {
    var url = require('url');
    var http = require('http');
    var parsedUrl = url.parse(sitemapUrl);
    var options = {
        host: parsedUrl.host,
        port: 80,
        path: parsedUrl.path
    };
    http.get(options, function(res) {
        var urls = [''];
        res.on('data', function(chunk) {
            ParseChunk(chunk, urls);
        });
        res.on('end', function() {
            cb(null, urls);
        });
    }).on('error', function(e) {
        cb(e);
    });
};