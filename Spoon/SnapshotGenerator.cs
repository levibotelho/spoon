using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spoon
{
    public static class SnapshotGenerator
    {
        static SnapshotDictionary _snapshotDictionary;
        static IEnumerable<string> _urls;
        static string _targetDirectory;
        static bool _areSnapshotsGenerated;

        public static async Task InitializeAsync(IEnumerable<string>urls, string targetDirectory)
        {
            var assemblyExtraction = PhantomJs.ExtractAssemblyAsync();
            _urls = urls;
            _targetDirectory = targetDirectory;
            _snapshotDictionary = new SnapshotDictionary(_urls, _targetDirectory);
            await assemblyExtraction;
        }

        public static string GetSnapshotByEscapedFragment(string escapedFragment)
        {
            if (_snapshotDictionary == null)
                throw new InvalidOperationException("The InitializeAsync method must be called before retreiving a snapshot.");
            if (!_areSnapshotsGenerated)
                GenerateSnapshots();
            return _snapshotDictionary.GetSnapshotByEscapedFragment(escapedFragment);
        }

        public static string GetSnapshotByUrl(string url)
        {
            if (_snapshotDictionary == null)
                throw new InvalidOperationException("The InitializeAsync method must be called before retreiving a snapshot.");
            if (!_areSnapshotsGenerated)
                GenerateSnapshots();
            return _snapshotDictionary.GetSnapshotByUrl(url);
        }

        static void GenerateSnapshots()
        {
            var snapshotDictionary = new SnapshotDictionary(_urls, _targetDirectory);
            var script = ScriptFileGenerator.GenerateScriptFile(snapshotDictionary);
            PhantomJs.RunScript(script.FullName);
            _snapshotDictionary = snapshotDictionary;
            _areSnapshotsGenerated = true;
        }
    }
}