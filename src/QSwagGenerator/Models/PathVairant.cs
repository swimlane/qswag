using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace QSwagGenerator.Models
{
    internal struct PathVairant
    {
        internal PathVairant(string original, string variant)
        {
            Original = original;
            Variant = variant;
        }

        internal string Variant { get; private set; }

        internal string Original { get; private set; }
    }
}
