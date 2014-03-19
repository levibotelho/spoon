using System.IO;
using System.Linq;

namespace Spoon
{
    class FileNameGenerator
    {
        public string GenerateFileNameFromUrl(string url)
        {
            return Path.GetInvalidFileNameChars().Aggregate(url, (current, invalidCharacter) => current.Replace(invalidCharacter, '%'));
        }
    }
}
