using System.Numerics;

namespace Spaceorc.Z3Wrap.BitVectors;

public readonly partial struct BitVec<TSize>
{
    /// <summary>
    /// Implicitly converts a BigInteger to a bitvector.
    /// The value will be masked to fit within the bitvector's bit width.
    /// </summary>
    /// <param name="value">The BigInteger value to convert.</param>
    /// <returns>A new bitvector containing the masked value.</returns>
    public static implicit operator BitVec<TSize>(BigInteger value) => new(value);

    /// <summary>
    /// Implicitly converts an int to a bitvector.
    /// The value will be masked to fit within the bitvector's bit width.
    /// </summary>
    /// <param name="value">The int value to convert.</param>
    /// <returns>A new bitvector containing the masked value.</returns>
    public static implicit operator BitVec<TSize>(int value) => new(value);

    /// <summary>
    /// Implicitly converts a uint to a bitvector.
    /// The value will be masked to fit within the bitvector's bit width.
    /// </summary>
    /// <param name="value">The uint value to convert.</param>
    /// <returns>A new bitvector containing the masked value.</returns>
    public static implicit operator BitVec<TSize>(uint value) => new(value);

    /// <summary>
    /// Implicitly converts a long to a bitvector.
    /// The value will be masked to fit within the bitvector's bit width.
    /// </summary>
    /// <param name="value">The long value to convert.</param>
    /// <returns>A new bitvector containing the masked value.</returns>
    public static implicit operator BitVec<TSize>(long value) => new(value);

    /// <summary>
    /// Implicitly converts a ulong to a bitvector.
    /// The value will be masked to fit within the bitvector's bit width.
    /// </summary>
    /// <param name="value">The ulong value to convert.</param>
    /// <returns>A new bitvector containing the masked value.</returns>
    public static implicit operator BitVec<TSize>(ulong value) => new(value);
}
