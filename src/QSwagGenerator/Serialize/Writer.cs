using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using SwaggerSchema;

namespace QSwagGenerator.Serialize
{
    internal static class Writer
    {
        // ReSharper disable once InconsistentNaming
        private static readonly JsonSerializerSettings _settings;

        static Writer()
        {
            _settings = new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
            _settings.Converters.Add(new StringEnumConverter() {CamelCaseText = true});
        }

        internal static string ToJson(this SwaggerRoot value)
        {
            return JsonConvert.SerializeObject(value, _settings);
        }
    }
}
