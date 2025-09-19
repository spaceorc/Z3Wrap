using Spaceorc.Z3Wrap.DataTypes;
using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.Extensions;

public static partial class Z3ContextExtensions
{
    /// <summary>
    /// Creates a real number expression from a Real value with unlimited precision.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="value">The Real value to create an expression from.</param>
    /// <returns>A Z3RealExpr representing the given Real value.</returns>
    public static Z3RealExpr Real(this Z3Context context, Real value)
    {
        using var valueStr = new AnsiStringPtr(value.ToString());
        var realSort = NativeMethods.Z3MkRealSort(context.Handle);
        var handle = NativeMethods.Z3MkNumeral(context.Handle, valueStr, realSort);
        return Z3RealExpr.Create(context, handle);
    }

    /// <summary>
    /// Creates a real number constant expression with the specified name.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="name">The name of the real constant.</param>
    /// <returns>A Z3RealExpr representing a real constant with the given name.</returns>
    public static Z3RealExpr RealConst(this Z3Context context, string name)
    {
        using var namePtr = new AnsiStringPtr(name);
        var symbol = NativeMethods.Z3MkStringSymbol(context.Handle, namePtr);
        var realSort = NativeMethods.Z3MkRealSort(context.Handle);
        var handle = NativeMethods.Z3MkConst(context.Handle, symbol, realSort);
        return Z3RealExpr.Create(context, handle);
    }
}
