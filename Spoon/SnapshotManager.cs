using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Spoon
{
    /// <summary>
    /// Creates and manages page snapshots.
    /// </summary>
    public static class SnapshotManager
    {
        static Dictionary<string, string> _escapedFragmentUrlMapping;
        static Dictionary<string, string> _escapedFragmentSnapshotPathMapping;
        static string _targetDirectory;
        static bool _isInitialized;

        /// <summary>
        /// Initializes the SnapshotManager. This method must be called before the SnapshotManager is used for the first time.
        /// </summary>
        /// <param name="escapedFragmentUrlPairs">The collection of escaped fragments to handle, each mapped to a corresponding snapshot URL.</param>
        /// <param name="targetDirectory">The directory in which to create the snapshot files.</param>
        public static async Task InitializeAsync(Dictionary<string, string> escapedFragmentUrlPairs, string targetDirectory)
        {
            _escapedFragmentUrlMapping = escapedFragmentUrlPairs;
            _targetDirectory = targetDirectory;
            _escapedFragmentSnapshotPathMapping = new Dictionary<string, string>(escapedFragmentUrlPairs.Count);
            await PhantomJs.ExtractAssemblyAsync();
            _isInitialized = true;
        }

        /// <summary>
        /// Returns the path to the snapshot file corresponding to an escaped fragment value.
        /// </summary>
        /// <param name="escapedFragment">The escaped fragment value for which to retrieve a page snapshot.</param>
        /// <returns>The path to the snapshot file corresponding to the escaped fragment.</returns>
        public static async Task<string> GetSnapshotPathAsync(string escapedFragment)
        {
            if (!_isInitialized)
                throw new InvalidOperationException("The InitializeAsync method must be called before retreiving a snapshot.");

            if (!_escapedFragmentUrlMapping.ContainsKey(escapedFragment))
                throw new ArgumentException("No snapshot has been defined for the escaped fragment \"" + escapedFragment + "\".");

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