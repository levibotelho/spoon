using System.IO;
using System.Linq;

namespace Spoon
{
    class FileNameGenerator
    {
        static readonly char[] InvalidFileNameChars = Path.GetInvalidFileNameChars().Concat(new[] { '.' }).ToArray();

        public string GenerateFilePathFromUrl(string url, string targetDirectory)
        {
            var validPath = InvalidFileNameChars.Aggregate(url, (current, invalidCharacter) => current.Replace(invalidCharacter, '%'));
            if (!targetDirectory.EndsWith("/"))
                targetDirectory += "/";
            return targetDirectory + validPath;
        }
    }
}