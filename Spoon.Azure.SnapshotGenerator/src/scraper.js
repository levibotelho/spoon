function ScrapeUrl(urls, i, cb, phantom) {
    phantom.createPage(function(createErr, page) {
        if (createErr) {
            console.error(createErr);
            cb(createErr);
        }

        page.open(urls[i], function(openErr) {
            if (openErr) {
                console.error(openErr);
                cb(openErr);
            }

            page.evaluate(function() {
                return document.documentElement.outerHTML;
            }, function(evaluateErr, result) {
                if (evaluateErr) {
                    console.error(evaluateErr);
                    cb(evaluateErr);
                }

                cb(null, result, i++);
                if (i == urls.length)
                    phantom.exit();
                else
                    ScrapeUrl(urls, i, cb, phantom);
            });
        });
    });
}

exports.scrapeUrls = function(urls, cb) {
    var nodePhantom = require('node-phantom-simple');
    var phantomPath = require('phantomjs').path;
    nodePhantom.create(function(err, phantom) {
        console.log('phantom created');
        if (err) cb(err);
        ScrapeUrl(urls, 0, cb, phantom);
    }, { phantomPath: phantomPath });
};