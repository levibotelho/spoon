var config = require('./config');
var rl = require('readline');

var prompts = rl.createInterface(process.stdin, process.stdout);

console.log(config.azureTableConnectionString);
prompts.question("Hit Enter to exit...", function() {
    process.exit();
});