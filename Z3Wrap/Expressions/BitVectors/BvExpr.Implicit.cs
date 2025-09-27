using System.Numerics;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Spaceorc.Z3Wrap.Expressions.BitVectors;

public sealed partial class BvExpr<TSize>
{
    /// <summary>
    /// Implicitly converts a BigInteger value to a Z3BitVec using the current thread-local context.
    /// </summary>
    /// <param name="value">The BigInteger value to convert.</param>
    /// <returns>A Z3BitVec representing the bitvector constant.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no current context is set.</exception>
    public static implicit operator BvExpr<TSize>(BigInteger value) =>
        Z3Context.Current.BitVec<TSize>(value);

    /// <summary>
    /// Implicitly converts an integer value to a Z3BitVec using the current thread-local context.
    /// </summary>
    /// <param name="value">The integer value to convert.</param>
    /// <returns>A Z3BitVec representing the bitvector constant.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no current context is set.</exception>
    public static implicit operator BvExpr<TSize>(int value) =>
        Z3Context.Current.BitVec<TSize>(value);

    /// <summary>
    /// Implicitly converts an unsigned integer value to a Z3BitVec using the current thread-local context.
    /// </summary>
    /// <param name="value">The unsigned integer value to convert.</param>
    /// <returns>A Z3BitVec representing the bitvector constant.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no current context is set.</exception>
    public static implicit operator BvExpr<TSize>(uint value) =>
        Z3Context.Current.BitVec<TSize>(value);

    /// <summary>
    /// Implicitly converts a long integer value to a Z3BitVec using the current thread-local context.
    /// </summary>
    /// <param name="value">The long integer value to convert.</param>
    /// <returns>A Z3BitVec representing the bitvector constant.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no current context is set.</exception>
    public static implicit operator BvExpr<TSize>(long value) =>
        Z3Context.Current.BitVec<TSize>(value);

    /// <summary>
    /// Implicitly converts an unsigned long integer value to a Z3BitVec using the current thread-local context.
    /// </summary>
    /// <param name="value">The unsigned long integer value to convert.</param>
    /// <returns>A Z3BitVec representing the bitvector constant.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no current context is set.</exception>
    public static implicit operator BvExpr<TSize>(ulong value) =>
        Z3Context.Current.BitVec<TSize>(value);

    /// <summary>
    /// Implicitly converts a typed BitVec value to a Z3BitVec using the current thread-local context.
    /// </summary>
    /// <param name="value">The BitVec value to convert.</param>
    /// <returns>A Z3BitVec representing the bitvector constant.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no current context is set.</exception>
    public static implicit operator BvExpr<TSize>(Bv<TSize> value) =>
        Z3Context.Current.BitVec(value);
}
