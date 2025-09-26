using System.Numerics;

namespace Spaceorc.Z3Wrap.RealTheory;

public readonly partial struct Real
{
    /// <summary>
    /// Implicitly converts an integer to a rational number.
    /// </summary>
    /// <param name="value">The integer value to convert.</param>
    /// <returns>A rational number representing the integer.</returns>
    public static implicit operator Real(int value) => new(value);

    /// <summary>
    /// Implicitly converts a long integer to a rational number.
    /// </summary>
    /// <param name="value">The long integer value to convert.</param>
    /// <returns>A rational number representing the long integer.</returns>
    public static implicit operator Real(long value) => new(value);

    /// <summary>
    /// Implicitly converts a decimal to a rational number.
    /// </summary>
    /// <param name="value">The decimal value to convert.</param>
    /// <returns>A rational number representing the decimal with exact precision.</returns>
    public static implicit operator Real(decimal value) => new(value);

    /// <summary>
    /// Implicitly converts a BigInteger to a rational number.
    /// </summary>
    /// <param name="value">The BigInteger value to convert.</param>
    /// <returns>A rational number representing the BigInteger.</returns>
    public static implicit operator Real(BigInteger value) => new(value, 1);
}
