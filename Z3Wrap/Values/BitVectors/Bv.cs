using System.Numerics;

namespace Spaceorc.Z3Wrap.Values.BitVectors;

/// <summary>
/// Represents a strongly-typed fixed-width bitvector with compile-time size validation.
/// </summary>
/// <typeparam name="TSize">The size specification implementing ISize.</typeparam>
public readonly partial struct Bv<TSize> : IEquatable<Bv<TSize>>, IComparable<Bv<TSize>>, ISpanFormattable
    where TSize : ISize
{
    private readonly BigInteger value;

    /// <summary>
    /// Gets the bit width of this bitvector type.
    /// </summary>
    public static uint Size => TSize.Size;

    /// <summary>
    /// Gets the underlying value as a BigInteger.
    /// </summary>
    public BigInteger Value => value;

    /// <summary>
    /// Gets whether this bitvector represents zero.
    /// </summary>
    public bool IsZero => value == 0;

    /// <summary>
    /// Initializes a new bitvector with the specified value.
    /// </summary>
    /// <param name="value">The value to store, masked to fit the bit width.</param>
    public Bv(BigInteger value)
    {
        // Mask to ensure value fits in the specified bit width
        var maxValue = (BigInteger.One << (int)Size) - 1;
        this.value = value & maxValue;
    }
}
