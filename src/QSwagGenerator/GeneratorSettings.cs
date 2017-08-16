using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using QSwagSchema;

namespace QSwagGenerator
{
    /// <summary>
    /// Settings for Swagger generation and Merge
    /// </summary>
    public class GeneratorSettings
    {
        /// <summary>
        /// Gets or sets the newtonsoft json schema license.
        /// </summary>
        /// <value>
        /// The json schema license.
        /// </value>
        public string JsonSchemaLicense { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="GeneratorSettings"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        public GeneratorSettings(HttpRequest request)
        {
            if (request == null) return;
            Protocol protocol;
            if(Enum.TryParse(request.Scheme, true, out protocol))
                Schemes = new List<Protocol>() {protocol};
            Host = request.Host.Value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether [ignore obsolete].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [ignore obsolete]; otherwise, <c>false</c>.
        /// </value>
        public bool IgnoreObsolete { get; set; }
        /// <summary>
        /// Gets or sets the default URL template.
        /// </summary>
        /// <value>
        /// The default URL template.
        /// </value>
        public string DefaultUrlTemplate { get; set; } = "{controller}/{id?}";

        /// <summary>
        /// Gets or sets a value indicating whether enums will be presented as strings.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enums are strings; otherwise, <c>false</c>. Default true.
        /// </value>
        public bool StringEnum { get; set; } = true;
        /// <summary>
        /// Gets or sets the Swagger info to use in merging.
        /// </summary>
        /// <value>
        /// The information.
        /// </value>
        public Info Info { get; set; }
        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        /// <value>
        /// The host.
        /// </value>
        public string Host { get; set; }
        /// <summary>
        /// Gets or sets the schemes.
        /// </summary>
        /// <value>
        /// The schemes.
        /// </value>
        public List<Protocol> Schemes { get; set; } = new List<Protocol>() {Protocol.Http};
        /// <summary>
        /// Gets or sets the XML document path.
        /// </summary>
        /// <value>
        /// The XML document.
        /// </value>
        public string XmlDocPath { get; set; }
        /// <summary>
        /// Gets or sets the security.
        /// </summary>
        /// <value>
        /// The security.
        /// </value>
        public List<SecurityRequirement> Security { get; set; } = new List<SecurityRequirement>();

        /// <summary>
        /// Gets or sets the security definitions.
        /// </summary>
        /// <value>
        /// The security definitions.
        /// </value>
        public Dictionary<string, SecurityDefinition> SecurityDefinitions { get; set; }

        /// <summary>
        /// Gets or sets the base route for cases when api calls are not appended to host name.
        /// </summary>
        /// <value>
        /// The base route.
        /// </value>
        public string BaseRoute { get; set; }

        /// <summary>
        /// Gets or sets the the default mime type .
        /// </summary>
        /// <value>
        /// The mime type.
        /// </value>
        public List<string> Produces { get; set; } = new List<string> { "application/json" };
    }
}
