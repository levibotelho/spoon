using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Spoon
{
    class ScriptFileGenerator
    {
        const string ScriptFileHeader = "try{var fs=require('fs');var page=require('webpage').create();function WritePageFile(a,b){page.open" +
            "(a,function(c){if(c!=='success'){fs.write('fileName',null,'w')}else{fs.write('fileName',page.content,'w')}})}var urlFilePairs=";
        const string ScriptFileFooter = ";for(var i=0;i<urlFilePairs.length;i++){WritePageFile(urlFilePairs[0],urlFilePairs[1])}}finally{phantom.exit()};";

        public ScriptFile GenerateScriptFile(IEnumerable<string> urls, string targetDirectory)
        {
            var path = Path.GetTempFileName();
            var urlFileNamePairs = GenerateUrlFileNamePairs(urls, targetDirectory);
            File.WriteAllText(path, GenerateScript(urlFileNamePairs));
            return new ScriptFile(path);
        }

        static IEnumerable<KeyValuePair<string, string>> GenerateUrlFileNamePairs(IEnumerable<string> urls, string targetDirectory)
        {
            var fileNameGenerator = new FileNameGenerator();
            return urls.Select(x => new KeyValuePair<string, string>(x, fileNameGenerator.GenerateFilePathFromUrl(x, targetDirectory)));
        }

        static string GenerateScript(IEnumerable<KeyValuePair<string, string>> urlFileNamePairs)
        {
            var urlFileNameArray = new StringBuilder(ScriptFileHeader + "[");
            foreach (var pair in urlFileNamePairs)
            {
                urlFileNameArray.AppendFormat("['{0}','{1}'],", pair.Key, pair.Value);
            }

            urlFileNameArray.Remove(urlFileNameArray.Length - 1, 1);
            urlFileNameArray.Append("]");
            urlFileNameArray.Append(ScriptFileFooter);
            return urlFileNameArray.ToString();
        }
    }
}