using System;
using System.IO;

namespace Spoon
{
    class ScriptFile : IDisposable
    {
        public readonly string FullName;

        public ScriptFile(string fullName)
        {
            FullName = fullName;
        }

        public void Dispose()
        {
            File.Delete(FullName);
        }
    }
}
