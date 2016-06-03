using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Schema;
using SwaggerSchema;

namespace QSwagGenerator.Generators
{
    internal class ParameterGenerator
    {
        internal Parameter Parameter { get; set; }

        private ParameterGenerator(ParameterInfo parameter, string httpPath, SchemaGenerator schemaGenerator)
        {
            Parameter = new Parameter { Name = parameter.Name };
            Parameter.In = GetBinding(parameter, httpPath);
            //Parameter.Description = "placeholder"; //TODO: Get it from xml
            Parameter.Required = SchemaGenerator.IsParameterRequired(parameter);
            var jSchema = schemaGenerator.GetSchema(parameter.ParameterType);
            if(Parameter.In==Location.Body)
                Parameter.Schema = schemaGenerator.MapToSchema(jSchema);
            else
            {
                var item = schemaGenerator.MapToItem(jSchema);
                Parameter.Map(item);
            }
        }


        private Location GetBinding(ParameterInfo parameter, string httpPath)
        {
            var attributes = parameter.GetCustomAttributes().ToDictionary(a => a.GetType().Name);
            if(attributes.ContainsKey(nameof(FromQueryAttribute)))
                return Location.Query;
            if (attributes.ContainsKey(nameof(FromBodyAttribute)))
                return Location.Body;
            if (attributes.ContainsKey(nameof(FromFormAttribute)))
                return Location.FormData;
            if (attributes.ContainsKey(nameof(FromHeaderAttribute)))
                return Location.Header;
            var paramRegex = new Regex(@"\{" + parameter.Name + @"[:\?\}]");
            if(attributes.ContainsKey(nameof(FromRouteAttribute)) || paramRegex.IsMatch(httpPath))
                return Location.Path;

            return GetDefaultBindng(parameter);
        }
        private Location GetDefaultBindng(ParameterInfo parameter)
        {
            return parameter.ParameterType.IsByRef ? Location.Body : Location.Query;
        }
        internal static ParameterGenerator CreateParameter(ParameterInfo parameter, string httpPath, SchemaGenerator schemaGenerator)
        {
            return new ParameterGenerator(parameter,httpPath,schemaGenerator);
        }
    }
}
