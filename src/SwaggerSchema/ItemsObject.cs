#region Using

using System.Collections.Generic;

#endregion

namespace SwaggerSchema
{
    public class ItemsObject
    {
        /// <summary>
        ///     Determines the format of the array if type array is used. Possible values are:
        ///     csv - comma separated values foo, bar.
        ///     ssv - space separated values foo bar.
        ///     tsv - tab separated values foo\tbar.
        ///     pipes - pipe separated values foo|bar.
        ///     Default value is csv.
        /// </summary>
        /// <value>
        ///     The collection format.
        /// </value>
        public CollectionFormat? CollectionFormat { get; set; }

        /// <summary>
        /// Required. The type of the parameter. Since the parameter is not located at the request body, it is limited to simple types (that is, not an object). 
        /// The value MUST be one of "string", "number", "integer", "boolean", "array" or "file". 
        /// If type is "file", the consumes MUST be either "multipart/form-data", " application/x-www-form-urlencoded" or both and the parameter MUST be in "formData".
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public SchemaType? Type { get; set; }

        /// <summary>
        ///     The extending format for the previously mentioned type. See Data Type Formats for further details.
        /// </summary>
        /// <value>
        ///     The format.
        /// </value>
        public string Format { get; set; }

        public List<object> Enum { get; set; }

        /// <summary>
        ///     Required if type is "array". Describes the type of items in the array.
        /// </summary>
        /// <value>
        ///     The items.
        /// </value>
        public List<ItemsObject> Items { get; set; }
        public object Default { get; set; }
        public double? Maximum { get; set; }
        public bool? ExclusiveMaximum { get; set; }
        public double? Minimum { get; set; }
        public bool? ExclusiveMinimum { get; set; }
        public long? MaxLength { get; set; }
        public long? MinLength { get; set; }
        public string Pattern { get; set; }
        public long? MaxItems { get; set; }
        public long? MinItems { get; set; }
        public bool? UniqueItems { get; set; }

    }
}