function ScrapeUrl(urls, i, cb, phantom) {
    phantom.createPage(function(error, page) {
        if (error) throw error;
        page.open(urls[i], function(error) {
            if (error) throw error;
            page.evaluate(function() {
                return document.documentElement.outerHTML;
            }, function(error, result) {
                if (error) throw error;
                cb(result, i++);
                if (i == urls.length)
                    phantom.exit();
                else
                    ScrapeUrl(urls, i, cb, phantom);
            });
        });
    });
}

exports.scrapeUrls = function(urls, cb) {
    require('node-phantom-simple').create(function(error, phantom) {
        if (error) throw error;
            ScrapeUrl(urls, 0, cb, phantom);
    }, { phantomPath: require('phantomjs').path });
};