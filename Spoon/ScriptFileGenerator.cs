using System.IO;
using System.Text;

namespace Spoon
{
    static class ScriptFileGenerator
    {
        const string ScriptFileHeader =
            "try {" +
                "var fs = require('fs');" +
                "var page = require('webpage').create();" +
                "function WritePageFile(url, fileName, exitPhantom) {" +
                    "page.open(url, function (result) {" +
                        "if (result !== 'success') {" +
                            "fs.write(fileName, null, 'w');" +
                        "} else {" +
                            "fs.write(fileName, page.content, 'w');" +
                        "}" +
                        "if (exitPhantom) {" +
                            "phantom.exit();" +
                        "}" +
                    "});" +
                "}" +
                "var urlFilePairs = ";

        const string ScriptFileFooter =
                ";" +
                "for (var i = 0; i < urlFilePairs.length; i++) {" +
                    "WritePageFile(urlFilePairs[i][0], urlFilePairs[i][1], i == urlFilePairs.length - 1);" +
                "}" +
            "} catch(e) {" +
                "phantom.exit();" +
            "}";

        public static ScriptFile GenerateScriptFile(string url, string fileName)
        {
            var path = Path.GetTempFileName();
            File.WriteAllText(path, GenerateScript(url, fileName));
            return new ScriptFile(path);
        }

        static string GenerateScript(string url, string fileName)
        {
            var urlFileNameArray = new StringBuilder(ScriptFileHeader + "[");
            urlFileNameArray.AppendFormat("['{0}','{1}']", url, fileName.Replace("\\", "\\\\"));
            urlFileNameArray.Append("]");
            urlFileNameArray.Append(ScriptFileFooter);
            return urlFileNameArray.ToString();
        }
    }
}