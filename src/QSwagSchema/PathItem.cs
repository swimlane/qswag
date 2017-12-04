using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace QSwagSchema
{
    /// <summary>
    /// Describes the operations available on a single path. A Path Item may be empty, due to ACL constraints. 
    /// The path itself is still exposed to the documentation viewer but they will not know which operations and parameters are available.
    /// </summary>
    /// <example>{
    ///  "get": {
    ///    "description": "Returns pets based on ID",
    ///    "summary": "Find pets by ID",
    ///    "operationId": "getPetsById",
    ///    "produces": [
    ///      "application/json",
    ///      "text/html"
    ///    ],
    ///    "responses": {
    ///      "200": {
    ///        "description": "pet response",
    ///        "schema": {
    ///          "type": "array",
    ///          "items": {
    ///            "$ref": "#/definitions/Pet"
    ///          }
    ///        }
    ///      },
    ///      "default": {
    ///        "description": "error payload",
    ///        "schema": {
    ///          "$ref": "#/definitions/ErrorModel"
    ///        }
    ///      }
    ///    }
    ///  },
    ///  "parameters": [
    ///    {
    ///      "name": "id",
    ///      "in": "path",
    ///      "description": "ID of pet to use",
    ///      "required": true,
    ///      "type": "array",
    ///      "items": {
    ///        "type": "string"
    ///      },
    ///      "collectionFormat": "csv"
    ///    }
    ///  ]
    ///}</example>
    public class PathItem
    {
        public Operation Get { get; set; }
        public Operation Put { get; set; }
        public Operation Post { get; set; }
        public Operation Delete { get; set; }
        public Operation Options { get; set; }
        public Operation Head { get; set; }
        public Operation Patch { get; set; }
        public List<Parameter> Parameters { get; set; }

        /// <summary>
        /// Merges the specified items.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static PathItem Merge(params PathItem[] items)
        {
            if (items.Length == 1) return items[0];
            var tp = typeof(PathItem);
            var operations = tp.GetRuntimeProperties().Where(o => o.PropertyType == typeof(Operation)).ToArray();
            var newPathItem = new PathItem();
            foreach (var pathItem in items)
            {
                foreach (var propertyInfo in operations)
                {
                    var newValue = propertyInfo.GetValue(pathItem);
                    if(newValue==null) continue;

                    var oldValue = propertyInfo.GetValue(newPathItem);
                    if (oldValue == null)
                    {
                        propertyInfo.SetValue(newPathItem, newValue);
                    }
                    else
                    {
                        throw new Exception($"Item {propertyInfo.Name} already added to that route.");
                    }
                }
            }
            return newPathItem;
        }
    }
}