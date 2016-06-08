using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Schema;
using QSwagGenerator.Generators;
using QSwagSchema;

namespace QSwagGenerator.Models
{
    internal class Scope
    {
        internal Dictionary<Type, JSchema> Schemas { get; set; } = new Dictionary<Type, JSchema>();
        internal GeneratorSettings Settings { get; set; }
        internal Dictionary<string, short> ObjectIdTracker { get; set; } = new Dictionary<string, short>();
        internal Dictionary<string,SchemaObject> SwaggerSchemas { get; set; } = new Dictionary<string, SchemaObject>();
        internal XmlDocs XmlDocs { get; set; } = new XmlDocs();
    }
}
