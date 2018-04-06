using System;

namespace QSwagGenerator.Errors
{
  /// <inheritdoc />
  /// <summary>
  ///   Exception thrown on validation part.
  /// </summary>
  /// <seealso cref="T:System.Exception" />
  public class ValidationException : Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public ValidationException(string message) : base(message)
    {
    }
  }
}