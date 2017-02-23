using System;

namespace QSwagGenerator.Annotations
{
    /// <summary>
    /// Tags to add for method in controller.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class TagAttribute : Attribute
    {
        /// <summary>
        /// Adds tags to attribute.
        /// </summary>
        /// <param name="tags">Tags</param>
        public TagAttribute(params string[] tags)
        {
            Tags = tags;
        }

        /// <summary>
        /// List of tags
        /// </summary>
        public string[] Tags { get; set; }
    }
}
