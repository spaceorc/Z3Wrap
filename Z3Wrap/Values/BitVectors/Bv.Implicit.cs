using System.Numerics;

namespace Spaceorc.Z3Wrap.Values.BitVectors;

public readonly partial struct Bv<TSize>
{
    /// <summary>
    /// Implicitly converts a BigInteger to a bitvector.
    /// The value will be masked to fit within the bitvector's bit width.
    /// </summary>
    /// <param name="value">The BigInteger value to convert.</param>
    /// <returns>A new bitvector containing the masked value.</returns>
    public static implicit operator Bv<TSize>(BigInteger value) => new(value);

    /// <summary>
    /// Implicitly converts an int to a bitvector.
    /// The value will be masked to fit within the bitvector's bit width.
    /// </summary>
    /// <param name="value">The int value to convert.</param>
    /// <returns>A new bitvector containing the masked value.</returns>
    public static implicit operator Bv<TSize>(int value) => new(value);

    /// <summary>
    /// Implicitly converts a uint to a bitvector.
    /// The value will be masked to fit within the bitvector's bit width.
    /// </summary>
    /// <param name="value">The uint value to convert.</param>
    /// <returns>A new bitvector containing the masked value.</returns>
    public static implicit operator Bv<TSize>(uint value) => new(value);

    /// <summary>
    /// Implicitly converts a long to a bitvector.
    /// The value will be masked to fit within the bitvector's bit width.
    /// </summary>
    /// <param name="value">The long value to convert.</param>
    /// <returns>A new bitvector containing the masked value.</returns>
    public static implicit operator Bv<TSize>(long value) => new(value);

    /// <summary>
    /// Implicitly converts a ulong to a bitvector.
    /// The value will be masked to fit within the bitvector's bit width.
    /// </summary>
    /// <param name="value">The ulong value to convert.</param>
    /// <returns>A new bitvector containing the masked value.</returns>
    public static implicit operator Bv<TSize>(ulong value) => new(value);
}
