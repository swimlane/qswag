# qswag

Fast & Light Swagger generator for .NET Core. For more information, checkout the [documentation](https://swimlane.gitbooks.io/qswag/content/)

## Example

```csharp
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using QSwagGenerator;
using QSwagSchema;

namespace Controllers
{
    /// <summary>
    /// Swagger spec controller.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    public class SwaggerController : Controller
    {
        private readonly List<Type> _types;

        /// <summary>
        /// Initializes a new instance of the <see cref="SwaggerController"/> class.
        /// </summary>
        public SwaggerController()
        {
            _types = new List<Type>
            {
                typeof(GroupsController),
                typeof(SettingsController)

            };
        }
        /// <summary>
        /// Gets the swagger.
        /// </summary>
        /// <returns>Swagger specification Json
        /// </returns>
        [HttpGet("/swagger")]
        public ActionResult GetSwagger(params string[] type)
        {
            var types = type == null || type.Length <= 0 ? _types : type.Select(GetTypeFromString);
            var httpRequest = HttpContext?.Request;
            
            var generatorSettings = new GeneratorSettings(httpRequest)
            {
                DefaultUrlTemplate = "/[controller]/{id?}",
                IgnoreObsolete = true,
                Info = new Info() { Title = "Swimlane API", Version = "3.0" },
                XmlDocPath = Path.ChangeExtension(Assembly.GetEntryAssembly().Location, "xml"),
                SecurityDefinitions = new Dictionary<string, SecurityDefinition>()
                {
                    {
                        "jwt_token",
                        new SecurityDefinition("Authorization", SecuritySchemeType.ApiKey) {In = Location.Header}
                    }
                },
                JsonSchemaLicense = "YourJsonSchemaLicense"
            };
            
            generatorSettings.Security.Add(new SecurityRequirement("jwt_token"));
            var generateForControllers = WebApiToSwagger.GenerateForControllers(types, generatorSettings, nameof(GetSwagger));
            
            return new FileContentResult(Encoding.UTF8.GetBytes(generateForControllers), "application/json");
        }

        private Type GetTypeFromString(string type)
        {
            var typeFromString = Type.GetType(type);
            if (typeFromString != null)
                return typeFromString;
            if (!type.Contains("."))
                return GetTypeFromString(string.Join(".", GetType().Namespace, type));
            if (!type.EndsWith("controller", StringComparison.CurrentCultureIgnoreCase))
                return GetTypeFromString(string.Concat(type, "Controller"));
            return null;
        }
    }
}
```

## Credits
`qswag` is a [Swimlane](http://swimlane.com) open-source project; we believe in giving back to the open-source community by sharing some of the projects we build for our application. Swimlane is an automated cyber security operations and incident response platform that enables cyber security teams to leverage threat intelligence, speed up incident response and automate security operations.
