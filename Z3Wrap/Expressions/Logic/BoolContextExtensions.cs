using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Common;

namespace Spaceorc.Z3Wrap.Expressions.Logic;

/// <summary>
/// Provides boolean logic operation methods for Z3Context.
/// </summary>
public static class BoolContextExtensions
{
    /// <summary>
    /// Creates boolean expression from boolean value.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="value">The boolean value.</param>
    /// <returns>Boolean expression representing the value.</returns>
    public static BoolExpr Bool(this Z3Context context, bool value)
    {
        var handle = value ? context.Library.Z3MkTrue(context.Handle) : context.Library.Z3MkFalse(context.Handle);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }

    /// <summary>
    /// Creates boolean true expression.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <returns>Boolean expression representing true.</returns>
    public static BoolExpr True(this Z3Context context)
    {
        var handle = context.Library.Z3MkTrue(context.Handle);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }

    /// <summary>
    /// Creates boolean false expression.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <returns>Boolean expression representing false.</returns>
    public static BoolExpr False(this Z3Context context)
    {
        var handle = context.Library.Z3MkFalse(context.Handle);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }

    /// <summary>
    /// Creates boolean constant with specified name.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="name">The constant name.</param>
    /// <returns>Boolean expression constant.</returns>
    public static BoolExpr BoolConst(this Z3Context context, string name)
    {
        var boolSort = context.Library.Z3MkBoolSort(context.Handle);
        var handle = context.Library.Z3MkConst(context.Handle, name, boolSort);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }

    /// <summary>
    /// Creates logical AND expression for multiple boolean operands.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="operands">The operands to combine with AND.</param>
    /// <returns>Boolean expression representing conjunction of operands.</returns>
    public static BoolExpr And(this Z3Context context, params BoolExpr[] operands)
    {
        var args = new IntPtr[operands.Length];
        for (int i = 0; i < operands.Length; i++)
            args[i] = operands[i].Handle;

        var resultHandle = context.Library.Z3MkAnd(context.Handle, (uint)args.Length, args);
        return Z3Expr.Create<BoolExpr>(context, resultHandle);
    }

    /// <summary>
    /// Creates logical OR expression for multiple boolean operands.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="operands">The operands to combine with OR.</param>
    /// <returns>Boolean expression representing disjunction of operands.</returns>
    public static BoolExpr Or(this Z3Context context, params BoolExpr[] operands)
    {
        var args = new IntPtr[operands.Length];
        for (int i = 0; i < operands.Length; i++)
            args[i] = operands[i].Handle;

        var resultHandle = context.Library.Z3MkOr(context.Handle, (uint)args.Length, args);
        return Z3Expr.Create<BoolExpr>(context, resultHandle);
    }

    /// <summary>
    /// Creates logical XOR expression for boolean operands.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Boolean expression representing left XOR right.</returns>
    public static BoolExpr Xor(this Z3Context context, BoolExpr left, BoolExpr right)
    {
        var resultHandle = context.Library.Z3MkXor(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, resultHandle);
    }

    /// <summary>
    /// Creates logical NOT expression for boolean operand.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="operand">The operand to negate.</param>
    /// <returns>Boolean expression representing !operand.</returns>
    public static BoolExpr Not(this Z3Context context, BoolExpr operand)
    {
        var resultHandle = context.Library.Z3MkNot(context.Handle, operand.Handle);
        return Z3Expr.Create<BoolExpr>(context, resultHandle);
    }

    /// <summary>
    /// Creates logical implication expression.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The antecedent.</param>
    /// <param name="right">The consequent.</param>
    /// <returns>Boolean expression representing left implies right.</returns>
    public static BoolExpr Implies(this Z3Context context, BoolExpr left, BoolExpr right)
    {
        var resultHandle = context.Library.Z3MkImplies(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, resultHandle);
    }

    /// <summary>
    /// Creates logical if-and-only-if expression.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Boolean expression representing left if-and-only-if right.</returns>
    public static BoolExpr Iff(this Z3Context context, BoolExpr left, BoolExpr right)
    {
        var resultHandle = context.Library.Z3MkIff(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, resultHandle);
    }

    /// <summary>
    /// Creates if-then-else expression.
    /// </summary>
    /// <typeparam name="T">Expression type for branches.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="condition">The condition to test.</param>
    /// <param name="thenExpr">Expression returned when condition is true.</param>
    /// <param name="elseExpr">Expression returned when condition is false.</param>
    /// <returns>Expression representing conditional selection.</returns>
    public static T Ite<T>(this Z3Context context, BoolExpr condition, T thenExpr, T elseExpr)
        where T : Z3Expr, IExprType<T>
    {
        var resultHandle = context.Library.Z3MkIte(context.Handle, condition.Handle, thenExpr.Handle, elseExpr.Handle);
        return Z3Expr.Create<T>(context, resultHandle);
    }
}
