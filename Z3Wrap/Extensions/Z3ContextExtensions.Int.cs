using System.Numerics;
using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.Extensions;

public static partial class Z3ContextExtensions
{
    /// <summary>
    /// Creates an integer expression from a BigInteger value.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="value">The integer value to create an expression for.</param>
    /// <returns>A Z3 integer expression representing the given value.</returns>
    public static Z3IntExpr Int(this Z3Context context, BigInteger value)
    {
        using var valueStr = new AnsiStringPtr(value.ToString());
        var intSort = NativeMethods.Z3MkIntSort(context.Handle);
        var handle = NativeMethods.Z3MkNumeral(context.Handle, valueStr, intSort);
        return Z3IntExpr.Create(context, handle);
    }

    /// <summary>
    /// Creates a named integer constant (variable) that can be used in expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="name">The name of the integer variable.</param>
    /// <returns>A Z3 integer expression representing a variable with the given name.</returns>
    public static Z3IntExpr IntConst(this Z3Context context, string name)
    {
        using var namePtr = new AnsiStringPtr(name);
        var symbol = NativeMethods.Z3MkStringSymbol(context.Handle, namePtr);
        var intSort = NativeMethods.Z3MkIntSort(context.Handle);
        var handle = NativeMethods.Z3MkConst(context.Handle, symbol, intSort);
        return Z3IntExpr.Create(context, handle);
    }
}
