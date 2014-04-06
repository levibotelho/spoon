var namePrefix = "_escaped_fragment_=";
exports.generate = function(url) {
    var fragmentIndex = url.indexOf("#!") + 2;
    var name = url.substr(fragmentIndex);
    if (name == '/') name = '';
    return fragmentIndex == -1 ? namePrefix + '.html' : namePrefix + name + '.html';
};