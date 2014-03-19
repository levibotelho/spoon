using System;
using System.Collections.Generic;

namespace Spoon
{
    public class SnapshotGenerator
    {
        public SnapshotCollection GenerateSnapshotCollection(IEnumerable<string> urls, string targetDirectory)
        {
            var script = new ScriptFileGenerator().GenerateScriptFile(urls, targetDirectory);

            throw new NotImplementedException();
        }
    }
}