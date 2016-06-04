namespace QSwagGenerator
{
    /// <summary>
    /// Settings for Swagger generation and Merge
    /// </summary>
    public class GeneratorSettings
    {
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
    }
}
