using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Spoon
{
    class SnapshotDictionary : IEnumerable<KeyValuePair<string, string>>
    {
        readonly Dictionary<string, string> _urlFileNamePairs;

        internal SnapshotDictionary(IEnumerable<string> urls, string targetDirectory)
        {
            targetDirectory = ValidateTargetDirectory(targetDirectory);
            ValidateFragmentUnicity(urls);
            _urlFileNamePairs = urls.ToDictionary(x => x, x => targetDirectory + Guid.NewGuid() + ".html");
        }

        public string GetSnapshotByUrl(string url)
        {
            return _urlFileNamePairs[url];
        }

        public string GetSnapshotByEscapedFragment(string escapedFragment)
        {
            return _urlFileNamePairs.FirstOrDefault(x => x.Key.EndsWith(escapedFragment)).Value;
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _urlFileNamePairs.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _urlFileNamePairs.GetEnumerator();
        }

        static string ValidateTargetDirectory(string targetDirectory)
        {
            if (targetDirectory.Contains("/") && !targetDirectory.EndsWith("/"))
                return targetDirectory + "/";
            if (!targetDirectory.EndsWith("\\"))
                return targetDirectory + "\\";
            return targetDirectory;
        }

        static void ValidateFragmentUnicity(IEnumerable<string> urls)
        {
            if (urls.Select(GetFragment).Distinct().Count() != urls.Count())
                throw new ArgumentException("Multiple urls exist which share the same fragment.");
        }

        static string GetFragment(string url)
        {
            var startIndex = url.IndexOf("#!", StringComparison.InvariantCultureIgnoreCase);
            return startIndex < 0 ? default(string) : url.Substring(startIndex);
        }
    }
}