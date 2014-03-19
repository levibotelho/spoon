using System;
using System.IO;

namespace Spoon
{
    class ScriptFile : IDisposable
    {
        public readonly string Path;

        public ScriptFile(string path)
        {
            Path = path;
        }

        public void Dispose()
        {
            File.Delete(Path);
        }
    }
}
