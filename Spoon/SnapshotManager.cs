using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Spoon
{
    public static class SnapshotManager
    {
        static Dictionary<string, string> _escapedFragmentUrlMapping;
        static Dictionary<string, string> _escapedFragmentSnapshotPathMapping;
        static string _targetDirectory;
        static bool _isInitialized;

        public static async Task InitializeAsync(Dictionary<string, string> escapedFragmentUrlPairs, string targetDirectory)
        {
            _escapedFragmentUrlMapping = escapedFragmentUrlPairs;
            _targetDirectory = targetDirectory;
            _escapedFragmentSnapshotPathMapping = new Dictionary<string, string>(escapedFragmentUrlPairs.Count);
            await PhantomJs.ExtractAssemblyAsync();
            _isInitialized = true;
        }

        public static async Task<string> GetSnapshotUrlAsync(string escapedFragment)
        {
            if (!_isInitialized)
                throw new InvalidOperationException("The InitializeAsync method must be called before retreiving a snapshot.");

            if (!_escapedFragmentUrlMapping.ContainsKey(escapedFragment))
                throw new InvalidOperationException("No snapshot has been defined for the escaped fragment \"" + escapedFragment + "\".");

            if (_escapedFragmentSnapshotPathMapping.ContainsKey(escapedFragment))
                return _escapedFragmentSnapshotPathMapping[escapedFragment];

            var snapshotPath = Path.Combine(_targetDirectory, Guid.NewGuid() + ".html");
            using (var script = ScriptFileGenerator.GenerateScriptFile(_escapedFragmentUrlMapping[escapedFragment], snapshotPath))
            {
                await PhantomJs.RunScriptAsync(script.FullName);
            }
            _escapedFragmentSnapshotPathMapping[escapedFragment] = snapshotPath;
            return snapshotPath;
        }
    }
}