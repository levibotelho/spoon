using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Spoon
{
    class PhantomJs
    {
        public Task RunScriptAsync(string scriptFileName)
        {
            return Task.Run(() => RunScript(scriptFileName));
        }

        static void RunScript(string scriptFileName)
        {
            using (var process = Process.Start("phantomjs.exe", scriptFileName))
            {
                if (process == null)
                    throw new InvalidOperationException("Started process is null. Is the process already running?");

                process.WaitForExit();
            }
        }
    }
}
