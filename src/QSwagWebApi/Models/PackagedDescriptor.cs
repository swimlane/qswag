using System.Collections.Generic;

namespace QSwagWebApi.Models
{
    public class PackagedDescriptor
    {
        public IEnumerable<string> InstallRequires { get; set; }

        public IEnumerable<string> Packages { get; set; }
    }
}
