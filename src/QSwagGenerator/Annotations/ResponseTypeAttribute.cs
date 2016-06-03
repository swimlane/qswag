using System;

namespace QSwagGenerator.Annotations
{
    /// <summary>
    /// Additional or Explicit Response types.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ResponseTypeAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseTypeAttribute"/> class.
        /// </summary>
        /// <param name="responseType">Type of the response.</param>
        public ResponseTypeAttribute(Type responseType)
        {
            HttpStatusCode = "200";
            ResponseType = responseType;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ResponseTypeAttribute"/> class.
        /// </summary>
        /// <param name="httpStatusCode">The HTTP status code.</param>
        /// <param name="responseType">Type of the response.</param>
        public ResponseTypeAttribute(string httpStatusCode, Type responseType)
        {
            HttpStatusCode = httpStatusCode;
            ResponseType = responseType;
        }
        /// <summary>
        /// Gets or sets the HTTP status code.
        /// </summary>
        /// <value>
        /// The HTTP status code.
        /// </value>
        public string HttpStatusCode { get; set; }
        /// <summary>
        /// Gets or sets the type of the response.
        /// </summary>
        /// <value>
        /// The type of the response.
        /// </value>
        public Type ResponseType { get; set; }
    }
}
