var namePrefix = "_escaped_fragment_=";
exports.generate = function(url) {
    var fragmentIndex = url.indexOf("#!") + 2;
    return fragmentIndex == -1 ? namePrefix + '.html' : namePrefix + url.substr(fragmentIndex) + '.html';
};