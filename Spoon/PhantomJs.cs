using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;

namespace Spoon
{
    static class PhantomJs
    {
        const string PhantomExecutableResourceName = "Spoon.lib.phantomjs.exe";
        const string PhantomLicenseResourceName = "Spoon.lib.phantomjs-license.txt";
        static readonly string PhantomTargetDirectory = Path.Combine(HttpRuntime.BinDirectory, "lib");
        static readonly string PhantomExecutableTargetPath = Path.Combine(PhantomTargetDirectory, "phantomjs.exe");
        static readonly string PhantomLicenseTargetPath = Path.Combine(PhantomTargetDirectory, "phantomjs-license.txt");

        public static async Task ExtractAssemblyAsync()
        {
            if (File.Exists(PhantomExecutableTargetPath))
                return;
            if (!Directory.Exists(PhantomTargetDirectory))
                Directory.CreateDirectory(PhantomTargetDirectory);

            var phantomExtraction = ExtractResourceAsync(PhantomExecutableResourceName, PhantomExecutableTargetPath);
            var phantomLicenseExtraction = ExtractResourceAsync(PhantomLicenseResourceName, PhantomLicenseTargetPath);
            await Task.WhenAll(phantomExtraction, phantomLicenseExtraction);
        }

        public static Task RunScriptAsync(string scriptFileName)
        {
            scriptFileName = CleanFileNameSlashes(scriptFileName);
            var tcs = new TaskCompletionSource<bool>();
            var process = new Process
            {
                EnableRaisingEvents = true,
                StartInfo = new ProcessStartInfo(PhantomExecutableTargetPath, scriptFileName)
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

        static async Task ExtractResourceAsync(string resourceName, string targetPath)
        {
            using (var resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                if (resourceStream == null)
                    throw new InvalidOperationException("Failed to extract resource \"" + resourceName + "\" from assembly.");
                using (var fileStream = File.Create(targetPath))
                {
                    await resourceStream.CopyToAsync(fileStream);
                }
            }
        }

        static string CleanFileNameSlashes(string scriptFileName)
        {
            return scriptFileName.Replace('\\', '/');
        }
    }
}