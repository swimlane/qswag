#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using QSwagGenerator.Annotations;
using QSwagGenerator.Models;
using QSwagGenerator.Serialize;
using QSwagSchema;

#endregion

namespace QSwagGenerator.Generators
{
    internal class WebApiGenerator
    {
        private readonly Scope _scope;
        private readonly SchemaGenerator _schemaGenerator;
        private static readonly Regex RouteParamRegex = new Regex(@"\{([^:?]+)[^\}]*\}");
        private static readonly Regex RouteParamNullableRegex = new Regex(@"/\{([^:?]+)\?[^\}]*\}");
        private const string OBSOLETE_ATTRIBUTE = nameof(ObsoleteAttribute);
        private const string IGNORE_ATTRIBUTE = nameof(IgnoreAttribute);


        public WebApiGenerator(GeneratorSettings settings)
        {
            var xmlDocs = XmlDocGenerator.GetXmlDocs(settings.XmlDocPath);
            _scope =new Scope {Settings = settings, XmlDocs = xmlDocs};
            _schemaGenerator = SchemaGenerator.Create(_scope,settings.JsonSchemaLicense);
        }


        internal HashSet<string> ExcludedMethodsName { get; set; }

        private HashSet<string> ExcludedTypeName { get;} = new HashSet<string>
        {
            "System.Object",
            "System.Web.Http.ApiController",
            "System.Web.Mvc.Controller"
        };

        #region Access: Private

        private bool IgnoreMethod(Dictionary<string, List<Attribute>> methodAttr)
        {
            return methodAttr.ContainsKey(IGNORE_ATTRIBUTE) ||
                   (_scope.Settings.IgnoreObsolete && methodAttr.ContainsKey(OBSOLETE_ATTRIBUTE));
        }

        private IEnumerable<string> GetHttpPaths(Dictionary<string, Attribute> controllerAttr,
            Dictionary<string, List<Attribute>> methodAttr, Type controller, MethodInfo method)
        {
            var baseRoute = GetBaseRoute(controllerAttr);

            var controllerName = controller.Name.Replace("Controller", string.Empty);
            var actionName = methodAttr.ContainsKey(nameof(ActionNameAttribute))
                ? ((ActionNameAttribute) methodAttr[nameof(ActionNameAttribute)].First()).Name
                : method.Name;
            foreach (var routableAttribute in new[]
            {
                nameof(RouteAttribute),
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
                (_scope.Settings.DefaultUrlTemplate ?? string.Empty)
                    .Replace("[controller]", controllerName)
                    .Replace("[action]", actionName)
            };
        }

        private string GetBaseRoute(Dictionary<string, Attribute> controllerAttr)
        {
            var routeAttributeName = nameof(RouteAttribute);
            var template = controllerAttr.ContainsKey(routeAttributeName)?
                ((RouteAttribute) controllerAttr[routeAttributeName]).Template
                : string.Empty;

            var baseRouteBuilder = new StringBuilder("/");

            if (!string.IsNullOrEmpty(_scope.Settings.BaseRoute) && !template.StartsWith("/"))
            {
                baseRouteBuilder.Append(_scope.Settings.BaseRoute.Trim('/'));
                baseRouteBuilder.Append("/");
            }
            if (!string.IsNullOrEmpty(template))
            {
                baseRouteBuilder.Append(template.Trim('/'));
                baseRouteBuilder.Append("/");
            }

            return baseRouteBuilder.ToString();
        }

        private static Dictionary<string, List<Attribute>> GetMethodAttributes(MethodInfo method)
        {
            return method
                .GetCustomAttributes()
                .GroupBy(m => m.GetType().Name)
                .ToDictionary(g => g.Key, g => g.ToList());
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

        private static IEnumerable<string> GetRoutes(string baseRoute, 
            Dictionary<string, List<Attribute>> methodAttr,
            string routeAttributeName,
            string actionName, 
            string controllerName)
        {
            var routeAttributes = methodAttr[routeAttributeName]
                .Cast<IRouteTemplateProvider>()
                .Select(r => (r.Template ?? string.Empty)
                .Replace("[action]", actionName))
                .SelectMany(GetRoutesFromAttribute);
            
            foreach (var routeAttribute in routeAttributes)
            {
                string combinedRoute;
                if (string.IsNullOrEmpty(routeAttribute))
                    combinedRoute = baseRoute;
                else if (routeAttribute.StartsWith("/"))
                   combinedRoute = routeAttribute;
                else
                    combinedRoute = $"{baseRoute}{routeAttribute}";
                yield return combinedRoute.Replace("[controller]", controllerName);
            }
        }

        private static IEnumerable<string> GetRoutesFromAttribute(string route)
        {
            var mandatory = RouteParamRegex.Replace(route, @"{$1}");
            if (RouteParamNullableRegex.IsMatch(route))
            {
                var matchCollection = RouteParamNullableRegex.Matches(route);
                var list = (from Match match in matchCollection select $"/{{{match.Groups[1].Value}}}").ToList();
                foreach (var permutation in GetAllPermutation(list))
                {
                    yield return permutation.Aggregate(mandatory, (current, nullable) => current.Replace(nullable, string.Empty));
                }
            }
            yield return mandatory;
        }

        private static List<List<T>> GetAllPermutation<T>(List<T> list)
        {
            var result = new List<List<T>> {new List<T>()};
            result.Last().Add(list[0]);
            if (list.Count == 1)
                return result;
            var tailCombos = GetAllPermutation(list.Skip(1).ToList());
            tailCombos.ForEach(combo =>
            {
                result.Add(new List<T>(combo));
                combo.Add(list[0]);
                result.Add(new List<T>(combo));
            });
            return result;
        }

        #endregion

        #region Main

        internal string GenerateForControllers(IEnumerable<Type> types)
        {
            var swagger = new SwaggerRoot
            {
                Info = _scope.Settings.Info,
                Host = _scope.Settings.Host,
                Schemes = _scope.Settings.Schemes,
                Paths = new Dictionary<string, PathItem>(),
                Definitions = _scope.SwaggerSchemas,
                Security = _scope.Settings.Security,
                SecurityDefinitions = _scope.Settings.SecurityDefinitions
            };
            foreach (var result in types.SelectMany(GeneratePaths))
            {
                if(swagger.Paths.ContainsKey(result.key))
                    throw new Exception($"The route {result.key} was already added.");
                swagger.Paths[result.key] = result.item;
            }
            
            swagger.Definitions.Add(ErrorModel.Name,ErrorModel.Schema);
            return swagger.ToJson();
        }

        private IEnumerable<(string key, PathItem item)> GeneratePaths(Type controller)
        {
            var controllerAttr = controller.GetTypeInfo().GetCustomAttributes().ToDictionary(m => m.GetType().Name);
            var methods = GetMethods(controller);
            var pathsInController = new Dictionary<string, PathItemGenerator>();
            foreach (var method in methods)
            {
                var methodAttr = GetMethodAttributes(method);
                if (IgnoreMethod(methodAttr)) continue;
                var httpPaths = GetHttpPaths(controllerAttr, methodAttr, controller, method);
                foreach (var httpPath in httpPaths)
                {
                    if (!pathsInController.ContainsKey(httpPath))
                        pathsInController.Add(httpPath, PathItemGenerator.Create(httpPath, _schemaGenerator, _scope));
                    pathsInController[httpPath].Add(method, methodAttr);
                }
            }
            return pathsInController.Select(p => (p.Key, p.Value.PathItem));
        }

        #endregion
    }
}