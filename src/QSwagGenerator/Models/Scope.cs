using System;
using System.Collections.Generic;
using Newtonsoft.Json.Schema;
using QSwagSchema;

namespace QSwagGenerator.Models
{
  using operationLookup = Dictionary<string, OperationIdTracker>;
  using schemaLookup = Dictionary<Type, JSchema>;
  using swaggerLookup = Dictionary<string, SchemaObject>;

  /// <summary>
  /// Globle scope type. For tracking global values.
  /// </summary>
  internal class Scope
  {
    internal schemaLookup Schemas { get; set; } = new schemaLookup();

    /// <summary>
    /// Gets or sets the settings.
    /// </summary>
    /// <value>
    /// The settings.
    /// </value>
    internal GeneratorSettings Settings { get; set; }

    /// <summary>
    /// Gets or sets the operation identifier tracker lookup.
    /// </summary>
    /// <value>
    /// The operation identifier tracker lookup.
    /// </value>
    internal operationLookup OperationIdTrackerLookup { get; set; } = new operationLookup();

    /// <summary>
    /// Gets or sets the swagger schemas.
    /// </summary>
    /// <value>
    /// The swagger schemas.
    /// </value>
    internal swaggerLookup SwaggerSchemas { get; set; } = new swaggerLookup();

    /// <summary>
    /// Gets or sets the XML docs.
    /// </summary>
    /// <value>
    /// The XML docs.
    /// </value>
    internal XmlDocs XmlDocs { get; set; } = new XmlDocs();
  }
}