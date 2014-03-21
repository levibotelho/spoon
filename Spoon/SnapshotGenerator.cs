using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spoon
{
    public static class SnapshotGenerator
    {
        public static async Task<SnapshotCollection> GenerateSnapshotCollectionAsync(IEnumerable<string> urls, string targetDirectory)
        {
            var snapshotCollection = new SnapshotCollection(urls, targetDirectory);
            var script = ScriptFileGenerator.GenerateScriptFile(snapshotCollection);
            await PhantomJs.RunScriptAsync(script.FullName);
            return snapshotCollection;
        }
    }
}