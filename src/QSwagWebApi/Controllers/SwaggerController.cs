using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using QSwagWebApi.Models;

namespace QSwagWebApi.Controllers
{
    [Route("api/Swagger")]
    public class SwaggerController : Controller
    {
        [HttpGet("{type}")]
        public string GetSwagger(string type)
        {
            var generatorSettings = new QSwagGenerator.GeneratorSettings() {
                DefaultUrlTemplate = "api/[controller]/{id?}",
                IgnoreObsolete =true };
            var typeFromString = GetTypeFromString(type);
            if (typeFromString == null) return string.Empty;
            return  QSwagGenerator
                .WebApiToSwagger
                .GenerateForController(typeFromString,generatorSettings,nameof(GetSwagger));
        }

        private Type GetTypeFromString(string type)
        {
            var typeFromString = Type.GetType(type);
            if(typeFromString!=null)
                return typeFromString;
            if (!type.Contains("."))
                return GetTypeFromString(string.Join(".", GetType().Namespace, type));
            if(!type.EndsWith("controller",StringComparison.CurrentCultureIgnoreCase))
                return GetTypeFromString(string.Concat(type,"Controller"));
            return null;
        }
    }
}
