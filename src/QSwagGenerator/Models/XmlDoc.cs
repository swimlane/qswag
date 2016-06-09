#region Using

using System.Collections.Generic;

#endregion

namespace QSwagGenerator.Models
{
    /// <summary>
    ///     Xml doc model.
    /// </summary>
    internal class XmlDoc
    {
        /// <summary>
        ///     The name attribute of the member. i.e Type or Method id.
        /// </summary>
        /// <value>
        ///     The name.
        /// </value>
        internal string Name { get; set; }

        /// <summary>
        ///     Gets or sets the summary.
        /// </summary>
        /// <value>
        ///     The summary.
        /// </value>
        internal string Summary { get; set; }

        /// <summary>
        ///     Gets or sets the returns of the method.
        /// </summary>
        /// <value>
        ///     The returns.
        /// </value>
        internal string Returns { get; set; }

        /// <summary>
        ///     Gets or sets the parameters.
        /// </summary>
        /// <value>
        ///     The parameters.
        /// </value>
        internal Dictionary<string, string> Parameters { get; set; } = new Dictionary<string, string>();
    }
}