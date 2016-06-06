using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using SwaggerSchema;

namespace QSwagGenerator
{
    /// <summary>
    /// Settings for Swagger generation and Merge
    /// </summary>
    public class GeneratorSettings
    {

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
    }
}
