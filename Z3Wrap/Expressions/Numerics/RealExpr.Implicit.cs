using System.Numerics;
using Spaceorc.Z3Wrap.Values.Numerics;

namespace Spaceorc.Z3Wrap.Expressions.Numerics;

public sealed partial class RealExpr
{
    /// <summary>
    /// Implicitly converts an integer value to a Z3Real using the current thread-local context.
    /// </summary>
    /// <param name="value">The integer value to convert.</param>
    /// <returns>A Z3Real representing the rational number.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no current context is set.</exception>
    public static implicit operator RealExpr(int value) => Z3Context.Current.Real(value);

    /// <summary>
    /// Implicitly converts a long integer value to a Z3Real using the current thread-local context.
    /// </summary>
    /// <param name="value">The long integer value to convert.</param>
    /// <returns>A Z3Real representing the rational number.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no current context is set.</exception>
    public static implicit operator RealExpr(long value) => Z3Context.Current.Real(value);

    /// <summary>
    /// Implicitly converts a decimal value to a Z3Real using the current thread-local context.
    /// </summary>
    /// <param name="value">The decimal value to convert.</param>
    /// <returns>A Z3Real representing the exact rational number.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no current context is set.</exception>
    public static implicit operator RealExpr(decimal value) => Z3Context.Current.Real(value);

    /// <summary>
    /// Implicitly converts a BigInteger value to a Z3Real using the current thread-local context.
    /// </summary>
    /// <param name="value">The BigInteger value to convert.</param>
    /// <returns>A Z3Real representing the rational number.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no current context is set.</exception>
    public static implicit operator RealExpr(BigInteger value) => Z3Context.Current.Real(value);

    /// <summary>
    /// Implicitly converts a Real value to a Z3Real using the current thread-local context.
    /// </summary>
    /// <param name="value">The Real value to convert.</param>
    /// <returns>A Z3Real representing the rational number.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no current context is set.</exception>
    public static implicit operator RealExpr(Real value) => Z3Context.Current.Real(value);
}
