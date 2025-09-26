namespace Spaceorc.Z3Wrap.BitVecTheory;

/// <summary>
/// Represents a compile-time bit vector size specification.
/// Enables strongly-typed bitvector operations with size validation at compile time.
/// </summary>
public interface ISize
{
    /// <summary>
    /// Gets the bit width of the bitvector size.
    /// </summary>
    static abstract uint Size { get; }
}
