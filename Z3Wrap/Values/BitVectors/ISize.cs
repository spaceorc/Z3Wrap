namespace Spaceorc.Z3Wrap.Values.BitVectors;

/// <summary>
/// Represents a compile-time bit vector size specification.
/// </summary>
public interface ISize
{
    /// <summary>
    /// Gets the bit width of the bitvector size.
    /// </summary>
    static abstract uint Size { get; }
}
