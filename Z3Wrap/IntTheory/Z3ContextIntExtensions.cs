using System.Numerics;
using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.IntTheory;

/// <summary>
/// Provides extension methods for Z3Context to work with integer expressions, constants, and arithmetic operations.
/// Supports unlimited precision integer arithmetic and conversion operations to other numeric types.
/// </summary>
public static partial class Z3ContextIntExtensions
{
    /// <summary>
    /// Creates an integer expression from a BigInteger value.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="value">The integer value to create an expression for.</param>
    /// <returns>A Z3 integer expression representing the given value.</returns>
    public static Z3Int Int(this Z3Context context, BigInteger value)
    {
        using var valueStr = new AnsiStringPtr(value.ToString());
        var intSort = SafeNativeMethods.Z3MkIntSort(context.Handle);
        var handle = SafeNativeMethods.Z3MkNumeral(context.Handle, valueStr, intSort);
        return Z3Expr.Create<Z3Int>(context, handle);
    }

    /// <summary>
    /// Creates a named integer constant (variable) that can be used in expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="name">The name of the integer variable.</param>
    /// <returns>A Z3 integer expression representing a variable with the given name.</returns>
    public static Z3Int IntConst(this Z3Context context, string name)
    {
        using var namePtr = new AnsiStringPtr(name);
        var symbol = SafeNativeMethods.Z3MkStringSymbol(context.Handle, namePtr);
        var intSort = SafeNativeMethods.Z3MkIntSort(context.Handle);
        var handle = SafeNativeMethods.Z3MkConst(context.Handle, symbol, intSort);
        return Z3Expr.Create<Z3Int>(context, handle);
    }
}
