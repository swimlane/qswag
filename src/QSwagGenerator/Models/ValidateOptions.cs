namespace QSwagGenerator.Models
{
  /// <summary>
  ///   Some additional settings to enforce.
  /// </summary>
  public class ValidateOptions
  {
    /// <summary>
    ///   Gets or sets a value indicating whether methods should be unique between controllers.
    /// </summary>
    /// <value>
    ///   <c>true</c> if [unique methods only]; otherwise, <c>false</c>.
    /// </value>
    public bool UniqueMethodsOnly { get; set; } = false;
  }
}