using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.BoolTheory;

public static partial class Z3ContextBoolExtensions
{
    /// <summary>
    /// Creates a logical AND expression from multiple boolean expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="operands">The boolean expressions to combine with AND.</param>
    /// <returns>A new Z3Bool representing the AND of all operands.</returns>
    public static Z3Bool And(this Z3Context context, params Z3Bool[] operands)
    {
        var args = new IntPtr[operands.Length];
        for (int i = 0; i < operands.Length; i++)
            args[i] = operands[i].Handle;

        var resultHandle = SafeNativeMethods.Z3MkAnd(context.Handle, (uint)args.Length, args);
        return Z3Expr.Create<Z3Bool>(context, resultHandle);
    }

    /// <summary>
    /// Creates a logical OR expression from multiple boolean expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="operands">The boolean expressions to combine with OR.</param>
    /// <returns>A new Z3Bool representing the OR of all operands.</returns>
    public static Z3Bool Or(this Z3Context context, params Z3Bool[] operands)
    {
        var args = new IntPtr[operands.Length];
        for (int i = 0; i < operands.Length; i++)
            args[i] = operands[i].Handle;

        var resultHandle = SafeNativeMethods.Z3MkOr(context.Handle, (uint)args.Length, args);
        return Z3Expr.Create<Z3Bool>(context, resultHandle);
    }

    /// <summary>
    /// Creates a logical XOR (exclusive or) expression between two boolean expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A new Z3Bool representing left XOR right.</returns>
    public static Z3Bool Xor(this Z3Context context, Z3Bool left, Z3Bool right)
    {
        var resultHandle = SafeNativeMethods.Z3MkXor(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<Z3Bool>(context, resultHandle);
    }

    /// <summary>
    /// Creates a logical NOT expression that negates a boolean expression.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="operand">The boolean expression to negate.</param>
    /// <returns>A new Z3Bool representing NOT operand.</returns>
    public static Z3Bool Not(this Z3Context context, Z3Bool operand)
    {
        var resultHandle = SafeNativeMethods.Z3MkNot(context.Handle, operand.Handle);
        return Z3Expr.Create<Z3Bool>(context, resultHandle);
    }

    /// <summary>
    /// Creates a logical implication expression between two boolean expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The antecedent (if part).</param>
    /// <param name="right">The consequent (then part).</param>
    /// <returns>A new Z3Bool representing left implies right.</returns>
    public static Z3Bool Implies(this Z3Context context, Z3Bool left, Z3Bool right)
    {
        var resultHandle = SafeNativeMethods.Z3MkImplies(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<Z3Bool>(context, resultHandle);
    }

    /// <summary>
    /// Creates a logical if-and-only-if (biconditional) expression between two boolean expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A new Z3Bool representing left if-and-only-if right.</returns>
    public static Z3Bool Iff(this Z3Context context, Z3Bool left, Z3Bool right)
    {
        var resultHandle = SafeNativeMethods.Z3MkIff(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<Z3Bool>(context, resultHandle);
    }
}
