using System.Numerics;

namespace Spaceorc.Z3Wrap.IntTheory;

public sealed partial class Z3Int
{
    /// <summary>
    /// Implicitly converts an integer value to a Z3Int using the current thread-local context.
    /// </summary>
    /// <param name="value">The integer value to convert.</param>
    /// <returns>A Z3Int representing the integer constant.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no current context is set.</exception>
    public static implicit operator Z3Int(int value) => Z3Context.Current.Int(value);

    /// <summary>
    /// Implicitly converts a long integer value to a Z3Int using the current thread-local context.
    /// </summary>
    /// <param name="value">The long integer value to convert.</param>
    /// <returns>A Z3Int representing the integer constant.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no current context is set.</exception>
    public static implicit operator Z3Int(long value) => Z3Context.Current.Int(value);

    /// <summary>
    /// Implicitly converts a BigInteger value to a Z3Int using the current thread-local context.
    /// </summary>
    /// <param name="value">The BigInteger value to convert.</param>
    /// <returns>A Z3Int representing the integer constant.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no current context is set.</exception>
    public static implicit operator Z3Int(BigInteger value) => Z3Context.Current.Int(value);
}
