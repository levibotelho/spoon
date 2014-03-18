using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spoon
{
    public class SnapshotGenerator
    {
        readonly IEnumerable<string> _paths;

        public SnapshotGenerator(IEnumerable<string> paths, string targetFolder)
        {
            _paths = paths;
        }


    }
}
