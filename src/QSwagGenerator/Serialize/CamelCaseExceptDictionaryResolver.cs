using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;

namespace QSwagGenerator.Serialize
{
    internal class CamelCaseExceptDictionaryResolver : CamelCasePropertyNamesContractResolver
    {
        #region Overrides of DefaultContractResolver

        protected override string ResolveDictionaryKey(string dictionaryKey)
        {
            return dictionaryKey;
        }

        #endregion
    }
}
