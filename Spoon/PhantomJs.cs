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

        public static async Task ExtractAssemblyAsync()
        {
            if (File.Exists(PhantomExecutablePath))
                return;
            if (!Directory.Exists(PhantomExecutableDirectory))
                Directory.CreateDirectory(PhantomExecutableDirectory);
            using (var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(PhantomResourceName))
            {
                if (resourceStream == null)
                    throw new InvalidOperationException("Failed to extract phantom.exe from assembly.");
                using (var fileStream = File.Create(PhantomExecutablePath))
                {
                    await resourceStream.CopyToAsync(fileStream);
                }
            }
        }

        public static Task RunScriptAsync(string scriptFileName)
        {
            scriptFileName = CleanFileNameSlashes(scriptFileName);
            var tcs = new TaskCompletionSource<bool>();
            var process = new Process
            {
                EnableRaisingEvents = true,
                StartInfo = new ProcessStartInfo(PhantomExecutablePath, scriptFileName)
                {
                    RedirectStandardError = true,
                    UseShellExecute = false
                }
            };
            process.Exited += (sender, args) =>
            {
                if (process.ExitCode != 0)
                {
                    var errorMessage = process.StandardError.ReadToEnd();
                    throw new InvalidOperationException("The phantomjs process did not exit correctly. The corresponding error message was: " + errorMessage);
                }
                tcs.SetResult(true);
                process.Dispose();
            };
            process.Start();
            return tcs.Task;
        }

        static string CleanFileNameSlashes(string scriptFileName)
        {
            return scriptFileName.Replace('\\', '/');
        }
    }
}