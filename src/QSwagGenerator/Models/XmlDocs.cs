#region Using

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#endregion

namespace QSwagGenerator.Models
{
    internal class XmlDocs : Dictionary<string, XmlDoc>
    {
        #region Access: Internal

        internal XmlDoc GetDoc(MethodInfo method)
        {
          var paramInfos = method.GetParameters().ToArray();
          var paramSignature = paramInfos.Length <= 0 ? string.Empty
            : $"({string.Join(",", paramInfos.Select(p => p.ParameterType.FullName))})";
            var key = $"M:{method.DeclaringType.FullName}.{method.Name}{paramSignature}";
            return ContainsKey(key) ? this[key] : null;
        }

        #endregion
    }
}