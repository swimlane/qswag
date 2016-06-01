using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using Newtonsoft.Json.Serialization;
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
            var generator = new JSchemaGenerator
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                SchemaIdGenerationHandling = SchemaIdGenerationHandling.FullTypeName
            };
            var jSchema = generator.Generate(type);
            var schema = new SchemaObject() ;
            Map(schema, jSchema);
            return schema;
        }

        private void Map(SchemaObject schema, JSchema jSchema)
        {
            schema.Id = jSchema.Id;
            schema.Title = jSchema.Title;
            schema.Description = jSchema.Description;
            schema.Default = GetDefault(jSchema.Default);
            schema.MultipleOf = jSchema.MultipleOf;
            schema.Maximum = jSchema.Maximum;
            schema.ExclusiveMaximum = jSchema.ExclusiveMaximum;
            schema.Minimum = jSchema.Minimum;
            schema.ExclusiveMinimum = jSchema.ExclusiveMinimum;
            schema.MaxLength = jSchema.MaximumLength;
            schema.MinLength = jSchema.MinimumLength;
            schema.Pattern = jSchema.Pattern;
            schema.MaxItems = jSchema.MaximumItems;
            schema.MinItems = jSchema.MinimumItems;
            schema.UniqueItems = jSchema.UniqueItems;
            schema.MaxProperties = jSchema.MaximumProperties;
            schema.MinProperties = jSchema.MinimumProperties;
            schema.Required = jSchema.Required;
            schema.Enum = GetEnum(jSchema.Enum);
            //schema.Items = jSchema.Items;
            //schema.AllOf = jSchema.AllOf;
            //schema.Properties = jSchema.Properties;
            //schema.AdditionalProperties = jSchema.AdditionalProperties;
        }

        private IList<object> GetEnum(IList<JToken> @enum)
        {
            return @enum.Select(e=>e.ToString()).Cast<object>().ToList();
        }

        private object GetDefault(JToken @default)
        {
            return null;
        }
    }
}
