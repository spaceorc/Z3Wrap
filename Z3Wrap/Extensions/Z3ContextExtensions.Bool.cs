using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.Extensions;

public static partial class Z3ContextExtensions
{
    /// <summary>
    /// Creates a boolean expression from the specified boolean value.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="value">The boolean value to convert.</param>
    /// <returns>A new Z3BoolExpr representing the boolean value.</returns>
    public static Z3BoolExpr Bool(this Z3Context context, bool value)
    {
        var handle = value
            ? SafeNativeMethods.Z3MkTrue(context.Handle)
            : SafeNativeMethods.Z3MkFalse(context.Handle);
        return Z3BoolExpr.Create(context, handle);
    }

    /// <summary>
    /// Creates a boolean expression representing the value true.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <returns>A new Z3BoolExpr representing true.</returns>
    public static Z3BoolExpr True(this Z3Context context)
    {
        var handle = SafeNativeMethods.Z3MkTrue(context.Handle);
        return Z3BoolExpr.Create(context, handle);
    }

    /// <summary>
    /// Creates a boolean expression representing the value false.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <returns>A new Z3BoolExpr representing false.</returns>
    public static Z3BoolExpr False(this Z3Context context)
    {
        var handle = SafeNativeMethods.Z3MkFalse(context.Handle);
        return Z3BoolExpr.Create(context, handle);
    }

    /// <summary>
    /// Creates a boolean constant (variable) with the specified name.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="name">The name of the boolean constant.</param>
    /// <returns>A new Z3BoolExpr representing the boolean constant.</returns>
    public static Z3BoolExpr BoolConst(this Z3Context context, string name)
    {
        using var namePtr = new AnsiStringPtr(name);
        var symbol = SafeNativeMethods.Z3MkStringSymbol(context.Handle, namePtr);
        var boolSort = SafeNativeMethods.Z3MkBoolSort(context.Handle);
        var handle = SafeNativeMethods.Z3MkConst(context.Handle, symbol, boolSort);
        return Z3BoolExpr.Create(context, handle);
    }
}
