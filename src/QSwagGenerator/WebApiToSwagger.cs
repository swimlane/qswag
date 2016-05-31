using System;
using System.Collections.Generic;
using SwaggerGenerator.Generators;

namespace SwaggerGenerator
{
    public class WebApiToSwagger
    {
 
        public static string GenerateForController<TController>(GeneratorSettings settings, string[] excludedMethodName)
        {
            return GenerateForController(typeof(TController),settings, excludedMethodName);
        }
        private static string GenerateForController(Type type, GeneratorSettings settings, params string[] excludedMethodName)
        {
            return GenerateForControllers(new[] {type},settings, excludedMethodName);
        }
        public static string GenerateForControllers(IEnumerable<Type> types, GeneratorSettings settings, params string[] excludedMethodName)
        {
            var generator = new WebApiGenerator(settings) { ExcludedMethodsName = new HashSet<string>(excludedMethodName)};
            return generator.GenerateForControllers(types);
        }
       
    }
}
