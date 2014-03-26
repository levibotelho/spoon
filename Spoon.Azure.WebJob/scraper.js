var page = require('webpage').create();

function GetDomString(url) {
    page.open(url, function(result) {
        if (result !== 'success')
            throw new Error(result);
        else
            return page.content;
    });
}

exports.getDomString = GetDomString;