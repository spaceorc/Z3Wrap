using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.Expressions.Logic;

/// <summary>
/// Provides extension methods for Z3Context to create Boolean expressions, constants, and literals.
/// Supports Boolean constant creation including true/false values and named variables.
/// </summary>
public static class BoolContextExtensions
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

    /// <summary>
    /// Creates a logical AND expression from multiple Boolean expressions (operand₁ ∧ operand₂ ∧ ... ∧ operandₙ).
    /// </summary>
    /// <param name="context">Z3 context.</param>
    /// <param name="operands">Boolean expressions to combine with AND.</param>
    /// <returns>A BoolExpr representing the AND of all operands.</returns>
    public static BoolExpr And(this Z3Context context, params BoolExpr[] operands)
    {
        var args = new IntPtr[operands.Length];
        for (int i = 0; i < operands.Length; i++)
            args[i] = operands[i].Handle;

        var resultHandle = SafeNativeMethods.Z3MkAnd(context.Handle, (uint)args.Length, args);
        return Z3Expr.Create<BoolExpr>(context, resultHandle);
    }

    /// <summary>
    /// Creates a logical OR expression from multiple Boolean expressions (operand₁ ∨ operand₂ ∨ ... ∨ operandₙ).
    /// </summary>
    /// <param name="context">Z3 context.</param>
    /// <param name="operands">Boolean expressions to combine with OR.</param>
    /// <returns>A BoolExpr representing the OR of all operands.</returns>
    public static BoolExpr Or(this Z3Context context, params BoolExpr[] operands)
    {
        var args = new IntPtr[operands.Length];
        for (int i = 0; i < operands.Length; i++)
            args[i] = operands[i].Handle;

        var resultHandle = SafeNativeMethods.Z3MkOr(context.Handle, (uint)args.Length, args);
        return Z3Expr.Create<BoolExpr>(context, resultHandle);
    }

    /// <summary>
    /// Creates a logical XOR expression between two Boolean expressions (left ⊕ right).
    /// </summary>
    /// <param name="context">Z3 context.</param>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>A BoolExpr representing left XOR right.</returns>
    public static BoolExpr Xor(this Z3Context context, BoolExpr left, BoolExpr right)
    {
        var resultHandle = SafeNativeMethods.Z3MkXor(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, resultHandle);
    }

    /// <summary>
    /// Creates a logical NOT expression that negates a Boolean expression (¬operand).
    /// </summary>
    /// <param name="context">Z3 context.</param>
    /// <param name="operand">Boolean expression to negate.</param>
    /// <returns>A BoolExpr representing NOT operand.</returns>
    public static BoolExpr Not(this Z3Context context, BoolExpr operand)
    {
        var resultHandle = SafeNativeMethods.Z3MkNot(context.Handle, operand.Handle);
        return Z3Expr.Create<BoolExpr>(context, resultHandle);
    }

    /// <summary>
    /// Creates a logical implication expression between two Boolean expressions (left → right).
    /// </summary>
    /// <param name="context">Z3 context.</param>
    /// <param name="left">Antecedent (if part).</param>
    /// <param name="right">Consequent (then part).</param>
    /// <returns>A BoolExpr representing left IMPLIES right.</returns>
    public static BoolExpr Implies(this Z3Context context, BoolExpr left, BoolExpr right)
    {
        var resultHandle = SafeNativeMethods.Z3MkImplies(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, resultHandle);
    }

    /// <summary>
    /// Creates a logical if-and-only-if expression between two Boolean expressions (left ↔ right).
    /// </summary>
    /// <param name="context">Z3 context.</param>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>A BoolExpr representing left IFF right.</returns>
    public static BoolExpr Iff(this Z3Context context, BoolExpr left, BoolExpr right)
    {
        var resultHandle = SafeNativeMethods.Z3MkIff(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, resultHandle);
    }

    /// <summary>
    /// Creates an if-then-else expression that selects between two expressions based on a condition.
    /// </summary>
    /// <typeparam name="T">Type of expressions to choose between.</typeparam>
    /// <param name="context">Z3 context.</param>
    /// <param name="condition">Boolean condition to evaluate.</param>
    /// <param name="thenExpr">Expression to return if condition is true.</param>
    /// <param name="elseExpr">Expression to return if condition is false.</param>
    /// <returns>An expression representing if condition then thenExpr else elseExpr.</returns>
    public static T Ite<T>(this Z3Context context, BoolExpr condition, T thenExpr, T elseExpr)
        where T : Z3Expr, IExprType<T>
    {
        var resultHandle = SafeNativeMethods.Z3MkIte(
            context.Handle,
            condition.Handle,
            thenExpr.Handle,
            elseExpr.Handle
        );
        return Z3Expr.Create<T>(context, resultHandle);
    }
}
