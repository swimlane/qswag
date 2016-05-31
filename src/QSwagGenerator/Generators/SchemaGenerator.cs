using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SwaggerSchema;

namespace SwaggerGenerator.Generators
{
    internal class SchemaGenerator
    {
        private SchemaGenerator()
        {
        }
        internal Dictionary<Type, SchemaObject> Schemas { get; set; } = new Dictionary<Type, SchemaObject>();

        internal static SchemaGenerator Create()
        {
            return new SchemaGenerator();
        }
        internal static bool IsParameterRequired(ParameterInfo parameter)
        {
            if (parameter == null)
                return false;

            if (parameter.GetCustomAttributes().Any(a => a.GetType().Name == "RequiredAttribute"))
                return true;

            if (parameter.HasDefaultValue)
                return false;

            var isNullable = Nullable.GetUnderlyingType(parameter.ParameterType) != null;
            if (isNullable)
                return false;

            return parameter.ParameterType.GetTypeInfo().IsValueType;
        }
        internal SchemaObject GetSchema(Type type)
        {
            return Schemas.ContainsKey(type)
                ? Schemas[type]
                : (Schemas[type] = GenerateSchema(type));
        }

        private SchemaObject GenerateSchema(Type type)
        {
            return new SchemaObject() {Ref="placeholder"};
        }
    }
}
