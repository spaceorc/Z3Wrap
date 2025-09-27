using System.Numerics;

namespace Spaceorc.Z3Wrap.Expressions.Numerics;

public sealed partial class IntExpr
{
    /// <summary>
    /// Implicitly converts an integer value to a Z3Int using the current thread-local context.
    /// </summary>
    /// <param name="value">The integer value to convert.</param>
    /// <returns>A Z3Int representing the integer constant.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no current context is set.</exception>
    public static implicit operator IntExpr(int value) => Z3Context.Current.Int(value);

    /// <summary>
    /// Implicitly converts a long integer value to a Z3Int using the current thread-local context.
    /// </summary>
    /// <param name="value">The long integer value to convert.</param>
    /// <returns>A Z3Int representing the integer constant.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no current context is set.</exception>
    public static implicit operator IntExpr(long value) => Z3Context.Current.Int(value);

    /// <summary>
    /// Implicitly converts a BigInteger value to a Z3Int using the current thread-local context.
    /// </summary>
    /// <param name="value">The BigInteger value to convert.</param>
    /// <returns>A Z3Int representing the integer constant.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no current context is set.</exception>
    public static implicit operator IntExpr(BigInteger value) => Z3Context.Current.Int(value);
}
