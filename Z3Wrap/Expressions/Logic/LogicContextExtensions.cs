using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.Expressions.Logic;

/// <summary>
/// Provides extension methods for Z3Context to create Boolean logical operations and functions.
/// Supports logical operations (∧, ∨, ¬, →, ↔, ⊕) and conditional expressions.
/// </summary>
public static class LogicContextExtensions
{
    #region Logical Operations

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

    #endregion

    #region Functions

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

    #endregion
}
