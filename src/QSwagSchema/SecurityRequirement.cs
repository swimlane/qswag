
using System.Collections.Generic;

namespace QSwagSchema
{
    /// <summary>
    /// Swagger security Requirement
    /// </summary>
    public class SecurityRequirement : Dictionary<string, string[]>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityRequirement"/> class.
        /// </summary>
        public SecurityRequirement()
        {
            
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityRequirement"/> class.
        /// </summary>
        /// <param name="defaultKey">The default key.</param>
        public SecurityRequirement(string defaultKey)
        {
            Add(defaultKey,new string[0]);
        }
    }
}