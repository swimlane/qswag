#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using Newtonsoft.Json.Serialization;
using QSwagGenerator.Models;
using QSwagSchema;
using License = Newtonsoft.Json.Schema.License;

#endregion

namespace QSwagGenerator.Generators
{
    internal class SchemaGenerator
    {
        private readonly JSchemaGenerator _generator;
        private readonly Scope _scope;

        #region Access: Internal

        internal JSchema GetSchema(Type type)
        {
            return _scope.Schemas.ContainsKey(type)
                ? _scope.Schemas[type]
                : (_scope.Schemas[type] = GenerateSchema(type));
        }

        internal ItemsObject MapToItem(JSchema jSchema)
        {
            var item = new ItemsObject();
            item.Default = GetDefault(jSchema.Default);
            if (jSchema.Maximum.HasValue)
            {
                item.Maximum = jSchema.Maximum;
                item.ExclusiveMaximum = jSchema.ExclusiveMaximum;
            }
            if (jSchema.Minimum.HasValue)
            {
                item.Minimum = jSchema.Minimum;
                item.ExclusiveMinimum = jSchema.ExclusiveMinimum;
            }
            item.MaxLength = jSchema.MaximumLength;
            item.MinLength = jSchema.MinimumLength;
            item.Pattern = jSchema.Pattern;
            item.MaxItems = jSchema.MaximumItems;
            item.MinItems = jSchema.MinimumItems;
            if (jSchema.UniqueItems) //If not present, this keyword may be considered present with boolean value false.
                item.UniqueItems = jSchema.UniqueItems;
            item.Enum = GetEnum(jSchema.Enum);
            item.Type = GetType(jSchema.Type);
            if (jSchema.Items.Count > 0)
                item.Items = jSchema.Items.Select(MapToItem).ToList();
            return item;
        }

        internal SchemaObject MapToSchema(JSchema jSchema)
        {
            var schema = new SchemaObject();
            schema.Id = jSchema.Id;
            schema.Title = jSchema.Title;
            schema.Description = jSchema.Description;
            schema.Default = GetDefault(jSchema.Default);
            schema.MultipleOf = jSchema.MultipleOf;
            if (jSchema.Maximum.HasValue)
            {
                schema.Maximum = jSchema.Maximum;
                schema.ExclusiveMaximum = jSchema.ExclusiveMaximum;
            }
            if (jSchema.Minimum.HasValue)
            {
                schema.Minimum = jSchema.Minimum;
                schema.ExclusiveMinimum = jSchema.ExclusiveMinimum;
            }
            schema.MaxLength = jSchema.MaximumLength;
            schema.MinLength = jSchema.MinimumLength;
            schema.Pattern = jSchema.Pattern;
            schema.MaxItems = jSchema.MaximumItems;
            schema.MinItems = jSchema.MinimumItems;
            if (jSchema.UniqueItems) //If not present, this keyword may be considered present with boolean value false.
                schema.UniqueItems = jSchema.UniqueItems;
            schema.MaxProperties = jSchema.MaximumProperties;
            schema.MinProperties = jSchema.MinimumProperties;
            if (jSchema.Required.Count > 0)
                schema.Required = jSchema.Required.ToList();
            schema.Enum = GetEnum(jSchema.Enum);
            schema.Type = GetType(jSchema.Type);
            if (jSchema.Items.Count > 0)
                schema.Items = jSchema.Items.Select(MapToSchema).ToList();
            //schema.AllOf = jSchema.AllOf;
            if (jSchema.Properties.Count > 0)
                schema.Properties = jSchema.Properties.ToDictionary(kv => kv.Key, kv => MapToSchema(kv.Value));
            //schema.AdditionalProperties = jSchema.AdditionalProperties;

            //Change object schema to reference
            if (jSchema.Type != null && jSchema.Type.Value.HasFlag(JSchemaType.Object))
            {
                //collect schemas for definitions.
                var id = jSchema.Id.ToString();
                if(!_scope.SwaggerSchemas.ContainsKey(id))
                    _scope.SwaggerSchemas.Add(id, schema);
                return new SchemaObject() { Ref = $"#/definitions/{id}" };
            }
            return schema;
        }

        #endregion

        #region Access: Private

        private JSchema GenerateSchema(Type type)
        {
            var jSchema = _generator.Generate(type);
            return jSchema;
        }

        private object GetDefault(JToken @default)
        {
            return @default?.Value<object>();
        }

        private static List<object> GetEnum(IList<JToken> @enum)
        {
            return @enum == null || @enum.Count <= 0
                ? null
                : @enum.Select(e => e?.Value<object>()).ToList();
        }

        private static SchemaType? GetType(JSchemaType? type)
        {
            if (!type.HasValue) return null;
            type &= ~JSchemaType.Null;
            return (SchemaType) type;
        }

        #endregion

        #region c-tor

        /// <summary>
        ///     Initializes a new instance of the <see cref="SchemaGenerator" /> class.
        /// </summary>
        /// <param name="scope">The scope.</param>
        /// <param name="jsonSchemaLicense"></param>
        private SchemaGenerator(Scope scope, string jsonSchemaLicense)
        {
            License.RegisterLicense(jsonSchemaLicense);
            _scope = scope;
            _generator = new JSchemaGenerator
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                SchemaIdGenerationHandling = SchemaIdGenerationHandling.FullTypeName
            };
            if (_scope.Settings.StringEnum)
                _generator.GenerationProviders.Add(new StringEnumGenerationProvider());
        }

        /// <summary>
        ///     Factory Method. Creates new SchemaGenerator.
        /// </summary>
        /// <param name="scope">The scope.</param>
        /// <param name="jsonSchemaLicense"></param>
        /// <returns></returns>
        internal static SchemaGenerator Create(Scope scope, string jsonSchemaLicense)
        {
            return new SchemaGenerator(scope,jsonSchemaLicense);
        }

        #endregion

        #region Static

        internal static bool IsComplex(JSchema schema)
        {
            if (!schema.Type.HasValue)
                return false;
            if (schema.Type.Value.HasFlag(JSchemaType.Object))
                return true;
            if (schema.Type.Value.HasFlag(JSchemaType.Array))
            {
                var innerType = schema.Items.FirstOrDefault()?.Type;
                if (innerType != null && innerType.Value.HasFlag(JSchemaType.Object))
                    return true;
            }
            return false;
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
            if (isNullable) return false;

            return parameter.ParameterType.GetTypeInfo().IsValueType;
        }

        #endregion
    }
}