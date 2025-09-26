using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.BoolTheory;

/// <summary>
/// Provides extension methods for Z3Context to work with boolean expressions, constants, and literals.
/// Supports boolean logic operations including true/false constants and boolean variables.
/// </summary>
public static partial class Z3ContextBoolExtensions
{
    /// <summary>
    /// Creates a boolean expression from the specified boolean value.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="value">The boolean value to convert.</param>
    /// <returns>A new Z3Bool representing the boolean value.</returns>
    public static Z3Bool Bool(this Z3Context context, bool value)
    {
        var handle = value
            ? SafeNativeMethods.Z3MkTrue(context.Handle)
            : SafeNativeMethods.Z3MkFalse(context.Handle);
        return Z3Expr.Create<Z3Bool>(context, handle);
    }

    /// <summary>
    /// Creates a boolean expression representing the value true.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <returns>A new Z3Bool representing true.</returns>
    public static Z3Bool True(this Z3Context context)
    {
        var handle = SafeNativeMethods.Z3MkTrue(context.Handle);
        return Z3Expr.Create<Z3Bool>(context, handle);
    }

    /// <summary>
    /// Creates a boolean expression representing the value false.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <returns>A new Z3Bool representing false.</returns>
    public static Z3Bool False(this Z3Context context)
    {
        var handle = SafeNativeMethods.Z3MkFalse(context.Handle);
        return Z3Expr.Create<Z3Bool>(context, handle);
    }

    /// <summary>
    /// Creates a boolean constant (variable) with the specified name.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="name">The name of the boolean constant.</param>
    /// <returns>A new Z3Bool representing the boolean constant.</returns>
    public static Z3Bool BoolConst(this Z3Context context, string name)
    {
        using var namePtr = new AnsiStringPtr(name);
        var symbol = SafeNativeMethods.Z3MkStringSymbol(context.Handle, namePtr);
        var boolSort = SafeNativeMethods.Z3MkBoolSort(context.Handle);
        var handle = SafeNativeMethods.Z3MkConst(context.Handle, symbol, boolSort);
        return Z3Expr.Create<Z3Bool>(context, handle);
    }
}
