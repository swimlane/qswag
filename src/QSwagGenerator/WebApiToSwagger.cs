using System;
using System.Collections.Generic;
using QSwagGenerator.Generators;


namespace QSwagGenerator
{
    /// <summary>
    /// Static class for generator methods.
    /// </summary>
    public class WebApiToSwagger
    {

        /// <summary>
        /// Generates for controller.
        /// </summary>
        /// <typeparam name="TController">The type of the controller.</typeparam>
        /// <param name="settings">The settings.</param>
        /// <param name="excludedMethodName">Name of the excluded method.</param>
        /// <returns></returns>
        public static string GenerateForController<TController>(GeneratorSettings settings, string[] excludedMethodName)
        {
            return GenerateForController(typeof(TController),settings, excludedMethodName);
        }
        /// <summary>
        /// Generates for controller.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="excludedMethodName">Name of the excluded method.</param>
        /// <returns></returns>
        public static string GenerateForController(Type type, GeneratorSettings settings, params string[] excludedMethodName)
        {
            return GenerateForControllers(new[] {type},settings, excludedMethodName);
        }
        /// <summary>
        /// Generates for controllers.
        /// </summary>
        /// <param name="types">The types.</param>
        /// <param name="settings">The settings.</param>
        /// <param name="excludedMethodName">Name of the excluded method.</param>
        /// <returns></returns>
        public static string GenerateForControllers(IEnumerable<Type> types, GeneratorSettings settings, params string[] excludedMethodName)
        {
            var generator = new WebApiGenerator(settings) { ExcludedMethodsName = new HashSet<string>(excludedMethodName)};
            return generator.GenerateForControllers(types);
        }
       
    }
}
