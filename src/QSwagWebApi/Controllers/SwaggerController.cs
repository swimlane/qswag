#region Using

using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

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
        #region Access: Public

        /// <summary>
        ///     Gets the swagger definition by type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        [HttpGet("{type}")]
        public string GetSwagger(string type)
        {
            var generatorSettings = new QSwagGenerator.GeneratorSettings(HttpContext?.Request)
            {
                DefaultUrlTemplate = "api/[controller]/{id?}",
                IgnoreObsolete = true,
                Info = new SwaggerSchema.Info() {Title = "QSwag Test API", Version = "1.0"},
                XmlDoc = Path.ChangeExtension(Assembly.GetEntryAssembly().Location, "xml")
            };
            var typeFromString = GetTypeFromString(type);
            if (typeFromString == null) return string.Empty;
            return QSwagGenerator
                .WebApiToSwagger
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