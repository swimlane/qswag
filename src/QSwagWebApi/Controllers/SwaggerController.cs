#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IISIntegration.Tools;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using QSwagGenerator;
using QSwagSchema;
using QSwagWebApi.Models;

#endregion

namespace QSwagWebApi.Controllers
{
    /// <summary>
    ///     Swagger controller for testing.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Route("api/Swagger")]
    public class SwaggerController : Controller
    {
        private readonly Licenses _licenses;

        /// <summary>
        /// Initializes a new instance of the <see cref="SwaggerController"/> class.
        /// </summary>
        /// <param name="licenses">The licenses.</param>
        public SwaggerController(IOptions<Licenses> licenses)
        {
            _licenses = licenses.Value;
        }
        #region Access: Public

        /// <summary>
        /// Gets the multi type swagger.
        /// </summary>
        /// <param name="types">The types.</param>
        /// <returns></returns>
        [HttpGet]
        public string GetMultiTypeSwagger(List<string> types)
        {
            var httpRequest = HttpContext?.Request;
            var generatorSettings = new GeneratorSettings(httpRequest)
            {
                DefaultUrlTemplate = "api/[controller]/{id?}",
                IgnoreObsolete = true,
                Info = new Info() { Title = "QSwag Test API", Version = "1.0" },
                XmlDocPath = Path.ChangeExtension(Assembly.GetEntryAssembly().Location, "xml"),
                SecurityDefinitions = new Dictionary<string, SecurityDefinition>()
                {
                    {
                        "jwt_token",
                        new SecurityDefinition("Authorization", SecuritySchemeType.ApiKey) {In = Location.Header}
                    }
                },
                JsonSchemaLicense = _licenses.Newtonsoft
            };
            generatorSettings.Security.Add(new SecurityRequirement("jwt_token"));
            var typeFromString = types.Select(GetTypeFromString).ToArray();
            if (typeFromString.Length<=0) return string.Empty;
            return WebApiToSwagger
                .GenerateForControllers(typeFromString, generatorSettings, nameof(GetSwagger));
        }
        /// <summary>
        ///     Gets the swagger definition by type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="xmlPath"></param>
        /// <returns></returns>
        [HttpGet("{type}")]
        public string GetSwagger(string type, string xmlPath = null)
        {
            var httpRequest = HttpContext?.Request;
            var generatorSettings = new GeneratorSettings(httpRequest)
            {
                DefaultUrlTemplate = "api/[controller]/{id}",
                IgnoreObsolete = true,
                Info = new Info() {Title = "QSwag Test API", Version = "1.0"},
                XmlDocPath = xmlPath ?? Path.ChangeExtension(Assembly.GetEntryAssembly().Location, "xml"),
                SecurityDefinitions = new Dictionary<string, SecurityDefinition>()
                {
                    {
                        "jwt_token",
                        new SecurityDefinition("Authorization", SecuritySchemeType.ApiKey) {In = Location.Header}
                    }
                },
                JsonSchemaLicense = _licenses.Newtonsoft
            };
            generatorSettings.Security.Add(new SecurityRequirement("jwt_token"));
            var typeFromString = GetTypeFromString(type);
            if (typeFromString == null) return string.Empty;
            return WebApiToSwagger
                .GenerateForController(typeFromString, generatorSettings, nameof(GetSwagger));
        }

        #endregion

        #region Access: Private

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

        #endregion
    }
}