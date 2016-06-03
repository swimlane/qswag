#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using QSwagGenerator.Annotations;
using QSwagGenerator.Serialize;
using SwaggerSchema;

#endregion

namespace QSwagGenerator.Generators
{
    internal class WebApiGenerator
    {
        private const string OBSOLETE_ATTRIBUTE = nameof(ObsoleteAttribute);
        private const string IGNORE_ATTRIBUTE = nameof(IgnoreAttribute);
        private readonly SchemaGenerator _schemaGenerator;
        private readonly GeneratorSettings _settings;

        public WebApiGenerator(GeneratorSettings settings)
        {
            _schemaGenerator = SchemaGenerator.Create();
            _settings = settings;
        }

        internal HashSet<string> ExcludedMethodsName { get; set; }

        internal HashSet<string> ExcludedTypeName { get; set; } = new HashSet<string>
        {
            "System.Object",
            "System.Web.Http.ApiController",
            "System.Web.Mvc.Controller"
        };

        #region Access: Internal

        internal string GenerateForControllers(IEnumerable<Type> types)
        {
            var swagger = new SwaggerRoot();
            swagger.Paths = types.SelectMany(GeneratePaths).ToDictionary(t => t.Item1, t => t.Item2);
            return swagger.ToJson();
        }

        #endregion

        #region Access: Private

        private bool AttributeDisallowed(Dictionary<string, List<Attribute>> methodAttr)
        {
            return methodAttr.ContainsKey(IGNORE_ATTRIBUTE) ||
                   (_settings.IgnoreObsolete && methodAttr.ContainsKey(OBSOLETE_ATTRIBUTE));
        }

        private IEnumerable<Tuple<string, PathItem>> GeneratePaths(Type type)
        {
            var controllerAttr = type.GetTypeInfo().GetCustomAttributes().ToDictionary(m => m.GetType().Name);
            var methods = GetMethods(type);
            var pathsInController = new Dictionary<string, PathItemGenerator>();
            foreach (var method in methods)
            {
                var methodAttr = method.GetCustomAttributes()
                    .GroupBy(m => m.GetType().Name)
                    .ToDictionary(g => g.Key, g => g.ToList());
                if (AttributeDisallowed(methodAttr)) continue;
                var httpPaths = GetHttpPath(controllerAttr, methodAttr, type, method);
                foreach (var httpPath in httpPaths)
                {
                    if (!pathsInController.ContainsKey(httpPath))
                        pathsInController.Add(httpPath, PathItemGenerator.Create(httpPath, _schemaGenerator));
                    pathsInController[httpPath].Add(method, methodAttr);
                }
            }
            return pathsInController.Select(p => Tuple.Create(p.Key, p.Value.PathItem));
        }

        private IEnumerable<string> GetHttpPath(Dictionary<string, Attribute> controllerAttr,
            Dictionary<string, List<Attribute>> methodAttr, Type controller, MethodInfo method)
        {
            const string routeAttributeName = nameof(RouteAttribute);
            var actionName = methodAttr.ContainsKey(nameof(ActionNameAttribute))
                ? ((ActionNameAttribute) methodAttr[nameof(ActionNameAttribute)].First()).Name
                : method.Name;
            var baseRoute = controllerAttr.ContainsKey(routeAttributeName)
                ? ((RouteAttribute) controllerAttr[routeAttributeName]).Template
                : string.Empty;
            var controllerName = controller.Name.Replace("Controller", string.Empty);

            foreach (var routableAttribute in new[]
            {
                routeAttributeName,
                nameof(HttpGetAttribute),
                nameof(HttpDeleteAttribute),
                nameof(HttpPostAttribute),
                nameof(HttpPutAttribute)
            })
            {
                if (methodAttr.ContainsKey(routableAttribute))
                {
                    return GetRoutes(baseRoute, methodAttr, routableAttribute, actionName, controllerName);
                }
            }
            return new[]
            {
                (_settings.DefaultUrlTemplate ?? string.Empty)
                    .Replace("{controller}", controllerName)
                    .Replace("{action}", actionName)
            };
        }

        private IEnumerable<MethodInfo> GetMethods(Type type)
        {
            return type.GetRuntimeMethods()
                .Where(m => m.IsPublic && m.IsSpecialName == false &&
                            !ExcludedMethodsName.Contains(m.Name) &&
                            m.DeclaringType != null &&
                            !ExcludedTypeName.Contains(m.DeclaringType.FullName) &&
                            !m.DeclaringType.FullName.StartsWith("Microsoft.AspNet"));
        }

        private static IEnumerable<string> GetRoutes(string baseRoute, Dictionary<string, List<Attribute>> methodAttr,
            string routeAttributeName,
            string actionName, string controllerName)
        {
            var routeAttributes = methodAttr[routeAttributeName]
                .Cast<IRouteTemplateProvider>()
                .Select(r => (r.Template ?? string.Empty).Replace("[action]", actionName));
            foreach (var routeAttribute in routeAttributes)
            {
                yield return (!string.IsNullOrEmpty(baseRoute) && !routeAttribute.StartsWith("/")
                    ? baseRoute + "/" + routeAttribute
                    : routeAttribute).Replace("[controller]", controllerName);
            }
        }

        #endregion
    }
}