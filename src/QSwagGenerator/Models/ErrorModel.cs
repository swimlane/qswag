using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QSwagSchema;

namespace QSwagGenerator.Models
{
    internal static class ErrorModel
    {
        static ErrorModel()
        {
            Schema = new SchemaObject
            {
                Description = "Default Error Object",
                Type = SchemaType.Object,
                Properties = new Dictionary<string, SchemaObject>()
                {
                    {"message", new SchemaObject() {Type = SchemaType.String}},
                    {"code", new SchemaObject() {Type = SchemaType.Integer, Minimum = 100, Maximum = 600}}
                },
                Required = new List<string>() {"message", "code"}
            };
        }
        public static SchemaObject Schema { get; }
        public static string Name => "ErrorModel";
    }
}
