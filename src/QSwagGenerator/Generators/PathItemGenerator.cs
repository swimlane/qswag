#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Schema;
using QSwagGenerator.Annotations;
using QSwagGenerator.Errors;
using QSwagGenerator.Models;
using QSwagSchema;
using QSwagGenerator.Misc;

#endregion

namespace QSwagGenerator.Generators
{
    internal class PathItemGenerator
    {
        private const string OBSOLETE_ATTRIBUTE = nameof(ObsoleteAttribute);
        private const string TAG_ATTRIBUTE = nameof(TagAttribute);
        private const string PRODUCES_ATTRIBUTE = nameof(ProducesAttribute);
        private string _route;
        private Scope _scope;
        private SchemaGenerator _schemaGenerator;
        internal PathItem PathItem { get; } = new PathItem();

        #region Access: Internal

        internal void Add(string originalRoute, MethodInfo method, Dictionary<string, List<Attribute>> methodAttr)
        {
            var parameters = method.GetParameters().ToList();
            var removedParameters = GetOptionalParameters(originalRoute);
            var doc = _scope.XmlDocs.GetDoc(method);
            var operation = new Operation
            {
                Summary = doc?.Summary,
                Description = doc?.Remarks ?? doc?.Summary,
                Deprecated = methodAttr.ContainsKey(OBSOLETE_ATTRIBUTE),
                Parameters = parameters
                    .Select(p => ParameterGenerator.CreateParameter(p, originalRoute, _schemaGenerator, doc))
                    .Where(p=> !removedParameters.Contains(p.Name))
                    .ToList(),
                OperationId = GetOperationId(method),
                Responses = GetResponses(method, methodAttr, doc).ToDictionary(r => r.Item1, r => r.Item2)
            };
            
            operation.Tags.Add((method.DeclaringType?.Name??"Unknown").Replace("Controller", string.Empty).ToCamelCase());
            if (methodAttr.ContainsKey(TAG_ATTRIBUTE))
            {
                operation.Tags=methodAttr[TAG_ATTRIBUTE].Cast<TagAttribute>().SelectMany(a=>a.Tags).ToList();
            }
            if (methodAttr.ContainsKey(PRODUCES_ATTRIBUTE))
            {
                operation.Produces = methodAttr[PRODUCES_ATTRIBUTE].Cast<ProducesAttribute>()
                    .SelectMany(a => a.ContentTypes).ToList();
            }
            AddOperation(methodAttr, operation);
        }

        private HashSet<string> GetOptionalParameters(string originalRoute)
        {
            HashSet<string> GetParams(string route) => new HashSet<string>(WebApiGenerator.RouteParamRegex.Matches(route)
                .Cast<Match>().Select(m => m.Groups[1].Value));
            var origParams = GetParams(originalRoute);
            var actualParams = GetParams(_route);
            origParams.ExceptWith(actualParams);
            return origParams;
        }

        internal static PathItemGenerator Create(string route, SchemaGenerator schemaGenerator, Scope scope)
        {
            return new PathItemGenerator {_route = route, _schemaGenerator=schemaGenerator, _scope = scope};
        }

        #endregion

        #region Access: Private

        private void AddOperation(Dictionary<string, List<Attribute>> methodAttr, Operation operation)
        {
            if (methodAttr.ContainsKey("HttpGetAttribute"))
                PathItem.Get = operation;
            if (methodAttr.ContainsKey("HttpPostAttribute"))
                PathItem.Post = operation;
            if (methodAttr.ContainsKey("HttpPutAttribute"))
                PathItem.Put = operation;
            if (methodAttr.ContainsKey("HttpDeleteAttribute"))
                PathItem.Delete = operation;
            if (methodAttr.ContainsKey("HttpPatchAttribute"))
                PathItem.Patch = operation;
            if (methodAttr.ContainsKey("HttpOptionsAttribute"))
                PathItem.Options = operation;

            if (!methodAttr.ContainsKey("AcceptVerbsAttribute")) return;
            var acceptVerbsAttribute = (AcceptVerbsAttribute) methodAttr["AcceptVerbsAttribute"].First();
            foreach (var verb in acceptVerbsAttribute.HttpMethods.Select(v => v.ToLowerInvariant()))
            {
                switch (verb)
                {
                    case "get":
                        PathItem.Get = operation;
                        break;
                    case "post":
                        PathItem.Post = operation;
                        break;
                    case "put":
                        PathItem.Put = operation;
                        break;
                    case "delete":
                        PathItem.Delete = operation;
                        break;
                    case "options":
                        PathItem.Options = operation;
                        break;
                    case "head":
                        PathItem.Head = operation;
                        break;
                    case "patch":
                        PathItem.Patch = operation;
                        break;
                }
            }
        }

      private string GetOperationId(MethodInfo method)
      {
        var source = method.DeclaringType.FullName;
        var name = method.Name.ToCamelCase();
        var lookup = _scope.OperationIdTrackerLookup;
        if (lookup.ContainsKey(name))
        {
          if (_scope.Settings.ValidateOptions.UniqueMethodsOnly &&
              lookup[name].Source != method)
          {
            throw new ValidationException("Duplicate method name.");
          }

          return string.Concat(name, lookup[name].Sequence += 1);
        }

        lookup.Add(name, new OperationIdTracker(method, 1));
        return name;
      }

      private IEnumerable<Tuple<string, Response>> GetResponses(MethodInfo method,
        Dictionary<string, List<Attribute>> methodAttr, XmlDoc doc)
      {
        bool IsVoid(Type type) => type == null || type.FullName == "System.Void";

        var docDescription = doc?.Returns ?? string.Empty;

        var methodReturnType = method.ReturnType;
        if (methodReturnType == typeof(Task))
          methodReturnType = typeof(void);
        else if (methodReturnType.Name == "Task`1")
          methodReturnType = methodReturnType.GenericTypeArguments[0];
        else if (methodReturnType == typeof(HttpResponseMessage) || typeof(IActionResult).IsAssignableFrom(methodReturnType))
          methodReturnType = typeof(object);
        var methodStatusCode = IsVoid(methodReturnType) ? "204" : "200";
        var methodDescription = IsVoid(methodReturnType) ? "No Content" : docDescription;

        const string responsetypeattribute = nameof(ResponseTypeAttribute);
        var overriddenCodes = new HashSet<string>();
        if (methodAttr.ContainsKey(responsetypeattribute))
        {
          var responseTypeAttributes = methodAttr[responsetypeattribute].Cast<ResponseTypeAttribute>();
          foreach (var responseTypeAttribute in responseTypeAttributes)
          {
            var returnType = responseTypeAttribute.ResponseType;
            var statusCode = responseTypeAttribute.HttpStatusCode ?? (IsVoid(returnType) ? "204" : "200");
            overriddenCodes.Add(statusCode);
            var schema = _schemaGenerator.MapToSchema(_schemaGenerator.GetSchema(returnType));
            var description = IsVoid(returnType) ? "No Content" : docDescription;
            yield return Tuple.Create(statusCode, new Response
            {
              Description = description,
              Schema = schema
            });
          }
        }

        if (!overriddenCodes.Contains(methodStatusCode))
        {
          var response = new Response {Description = methodDescription};
          if (!IsVoid(methodReturnType))
          {
            response.Schema = _schemaGenerator.MapToSchema(_schemaGenerator.GetSchema(methodReturnType));
          }

          yield return Tuple.Create(methodStatusCode, response);
        }

        yield return
          Tuple.Create("default",
            new Response
            {
              Description = "Unexpected Error",
              Schema = new SchemaObject() {Ref = "#/definitions/ErrorModel"}
            });
      }

        #endregion
    }
}
