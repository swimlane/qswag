using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace QSwagGenerator.Models
{
    internal class XmlDocs: Dictionary<string,XmlDoc>
    {
        internal XmlDoc GetDoc(MethodInfo method)
        {
            var signature = string.Join(",", method.GetParameters().Select(p=>p.ParameterType.FullName));
            var key = $"M:{method.DeclaringType.FullName}.{method.Name}({signature})";
            return ContainsKey(key) ? this[key] : null;
        }
    }
}
