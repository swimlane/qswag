#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QSwagGenerator.Annotations;
using QSwagGenerator.Models;
using SwaggerSchema;

#endregion

namespace QSwagGenerator.Generators
{
    internal class PathItemGenerator
    {
        private const string OBSOLETE_ATTRIBUTE = nameof(ObsoleteAttribute);
        private string _httpPath;
        private Scope _scope;
        private SchemaGenerator _schemaGenerator;
        internal PathItem PathItem { get; set; } = new PathItem();

        #region Access: Internal

        internal void Add(MethodInfo method, Dictionary<string, List<Attribute>> methodAttr)
        {
            var parameters = method.GetParameters().ToList();
            var doc = _scope.XmlDocs.GetDoc(method);
            var operation = new Operation
            {
                Description = doc?.Summary,
                Deprecated = methodAttr.ContainsKey(OBSOLETE_ATTRIBUTE),
                Parameters = parameters
                    .Select(p => ParameterGenerator.CreateParameter(p, _httpPath, _schemaGenerator,doc))
                    .ToList(),
                OperationId = GetOperationId(method),
                Responses = GetResponses(method, methodAttr,doc).ToDictionary(r => r.Item1, r => r.Item2)
            };
            operation.Tags.Add(method.DeclaringType.Name.Replace("Controller", string.Empty));
            AddOperation(methodAttr, operation);
        }

        internal static PathItemGenerator Create(string httpPath,SchemaGenerator schemaGenerator, Scope scope)
        {
            return new PathItemGenerator {_httpPath = httpPath, _schemaGenerator=schemaGenerator, _scope = scope};
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
            var name = method.Name;
            if (_scope.ObjectIdTracker.ContainsKey(name))
                return string.Concat(name, _scope.ObjectIdTracker[name] += 1);
            _scope.ObjectIdTracker.Add(name, 1);
            return name;
        }

        private IEnumerable<Tuple<string, Response>> GetResponses(MethodInfo method, Dictionary<string, List<Attribute>> methodAttr, XmlDoc doc)
        {
            Func<Type, bool> isVoid = type => type == null || type.FullName == "System.Void";
            var returnType = method.ReturnType;
            if (returnType == typeof(Task))
                returnType = typeof(void);
            else if (returnType.Name == "Task`1")
                returnType = returnType.GenericTypeArguments[0];

            var description = doc?.Returns ?? string.Empty;

            var mayBeNull = !SchemaGenerator.IsParameterRequired(method.ReturnParameter);
            const string responsetypeattribute = nameof(ResponseTypeAttribute);
            if (methodAttr.ContainsKey(responsetypeattribute))
            {
                var responseTypeAttributes = methodAttr[responsetypeattribute].Cast<ResponseTypeAttribute>();
                foreach (var responseTypeAttribute in responseTypeAttributes)
                {
                    returnType = responseTypeAttribute.ResponseType;
                    var httpStatusCode = responseTypeAttribute.HttpStatusCode ??
                                         (isVoid(returnType) ? "204" : "200");

                    yield return Tuple.Create(httpStatusCode, new Response
                    {
                        Description = isVoid(returnType) ? "No Content" : description,
                        Schema = _schemaGenerator.MapToSchema(_schemaGenerator.GetSchema(returnType))
                    });
                }
            }
            else
            {
                yield return isVoid(returnType)
                    ? Tuple.Create("204", new Response() {Description = "No Content."})
                    : Tuple.Create("200", new Response
                    {
                        Description = description,
                        Schema = _schemaGenerator
                            .MapToSchema(_schemaGenerator.GetSchema(returnType))
                    });
            }
            yield return
                Tuple.Create("default",
                    new Response()
                    {
                        Description = "Unexected Error",
                        Schema = new SchemaObject() {Ref = "#/definitions/ErrorModel"}
                    });
        }

        #endregion
    }
}