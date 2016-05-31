using System.Collections.Generic;

namespace SwaggerSchema
{
    public class SwaggerRoot
    {
        public string Swagger { get; set; } = "2.0";

        public Info Info { get; set; } = new Info();

        public string Host { get; set; }

        public string BasePath { get; set; }

        public List<Protocol> Schemes { get; set; }

        public List<string> Consumes { get; set; }

        public List<string> Produces { get; set; }

        public Dictionary<string, PathItem> Paths { get; set; } = new Dictionary<string, PathItem>();

        public Dictionary<string, SchemaObject> Definitions { get; set; } = new Dictionary<string, SchemaObject>();

        public List<Parameter> Parameters { get; set; }

        public Dictionary<string, Response> Responses { get; set; }

        public Dictionary<string, SecurityDefinition> SecurityDefinitions { get; set; }

       public List<SecurityRequirement> Security { get; set; }

        public List<Tag> Tags { get; set; }

        public string BaseUrl { get; set; }

        public ExternalDoc ExternalDocs { get; set; }
   
    }

    public class SecurityDefinition
    {
    }
}