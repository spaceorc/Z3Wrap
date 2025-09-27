using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.Expressions.Logic;

/// <summary>
/// Provides extension methods for Z3Context to create Boolean expressions, constants, and literals.
/// Supports Boolean constant creation including true/false values and named variables.
/// </summary>
public static class BoolExprFactoryContextExtensions
{
    /// <summary>
    /// Creates a Boolean expression from the specified Boolean value.
    /// </summary>
    /// <param name="context">Z3 context.</param>
    /// <param name="value">Boolean value to convert.</param>
    /// <returns>A BoolExpr representing the Boolean value.</returns>
    public static BoolExpr Bool(this Z3Context context, bool value)
    {
        var handle = value
            ? SafeNativeMethods.Z3MkTrue(context.Handle)
            : SafeNativeMethods.Z3MkFalse(context.Handle);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }

    /// <summary>
    /// Creates a Boolean expression representing the value true.
    /// </summary>
    /// <param name="context">Z3 context.</param>
    /// <returns>A BoolExpr representing true.</returns>
    public static BoolExpr True(this Z3Context context)
    {
        var handle = SafeNativeMethods.Z3MkTrue(context.Handle);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }

    /// <summary>
    /// Creates a Boolean expression representing the value false.
    /// </summary>
    /// <param name="context">Z3 context.</param>
    /// <returns>A BoolExpr representing false.</returns>
    public static BoolExpr False(this Z3Context context)
    {
        var handle = SafeNativeMethods.Z3MkFalse(context.Handle);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }

    /// <summary>
    /// Creates a Boolean constant (variable) with the specified name.
    /// </summary>
    /// <param name="context">Z3 context.</param>
    /// <param name="name">Name of the Boolean constant.</param>
    /// <returns>A BoolExpr representing the Boolean constant.</returns>
    public static BoolExpr BoolConst(this Z3Context context, string name)
    {
        using var namePtr = new AnsiStringPtr(name);
        var symbol = SafeNativeMethods.Z3MkStringSymbol(context.Handle, namePtr);
        var boolSort = SafeNativeMethods.Z3MkBoolSort(context.Handle);
        var handle = SafeNativeMethods.Z3MkConst(context.Handle, symbol, boolSort);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }
}
