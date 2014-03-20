using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Spoon
{
    static class ScriptFileGenerator
    {
        const string ScriptFileHeader = "try{var fs=require('fs');var page=require('webpage').create();function WritePageFile(a,b){page.open" +
            "(a,function(c){if(c!=='success'){fs.write('fileName',null,'w')}else{fs.write('fileName',page.content,'w')}})}var urlFilePairs=";
        const string ScriptFileFooter = ";for(var i=0;i<urlFilePairs.length;i++){WritePageFile(urlFilePairs[0],urlFilePairs[1])}}finally{phantom.exit()};";

        public static ScriptFile GenerateScriptFile(IEnumerable<KeyValuePair<string, string>> urlFileNamePairs)
        {
            var path = Path.GetTempFileName();
            File.WriteAllText(path, GenerateScript(urlFileNamePairs));
            return new ScriptFile(path);
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