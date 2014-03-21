using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Spoon
{
    static class PhantomJs
    {
        const string PhantomResourceName = "Spoon.lib.phantomjs.exe";
        static readonly string PhantomExecutableDirectory = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "lib");
        static readonly string PhantomExecutablePath = Path.Combine(PhantomExecutableDirectory, "phantomjs.exe");

        public static Task RunScriptAsync(string scriptFileName)
        {
            ExtractAssemblyAsync().Wait();

            var process = Process.Start(PhantomExecutablePath, scriptFileName);
            if (process == null)
                throw new InvalidOperationException("Started process is null. Is the process already running?");

            var tcs = new TaskCompletionSource<bool>();
            process.Exited += (sender, args) =>
            {
                tcs.SetResult(true);
                process.Dispose();
            };

            return tcs.Task;
        }

        static async Task ExtractAssemblyAsync()
        {
            if (File.Exists(PhantomExecutablePath))
                return;
            if (!Directory.Exists(PhantomExecutableDirectory))
                Directory.CreateDirectory(PhantomExecutableDirectory);
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(PhantomResourceName))
            {
                if (stream == null)
                    throw new InvalidOperationException("Failed to extract phantom.exe from assembly.");
                await stream.CopyToAsync(File.Create(PhantomExecutablePath));
            }
        }
    }
}