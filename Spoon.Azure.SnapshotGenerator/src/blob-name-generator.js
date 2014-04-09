var namePrefix = '_escaped_fragment_=';
exports.generate = function(url) {
    var hashbangIndex = url.indexOf("#!");
    var name = url.substr(hashbangIndex + 2);
    if (name == '/') name = '';
    return hashbangIndex == -1 ? namePrefix + '.html' : namePrefix + name + '.html';
};