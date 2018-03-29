using System.Reflection;

namespace QSwagGenerator.Models
{
  /// <summary>
  ///   Tracker for method names across controllers/types.
  /// </summary>
  internal class OperationIdTracker
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="OperationIdTracker"/> class.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="sequence">The sequence.</param>
    public OperationIdTracker(MethodInfo source, short sequence)
    {
      Source = source;
      Sequence = sequence;
    }

    /// <summary>
    ///   Gets or sets the sequence that makes methods unique.
    /// </summary>
    /// <value>
    ///   The sequence.
    /// </value>
    internal short Sequence { get; set; }

    /// <summary>
    ///   Gets or sets the source type.
    /// </summary>
    /// <value>
    ///   The source.
    /// </value>
    internal MethodInfo Source { get; set; }
  }
}