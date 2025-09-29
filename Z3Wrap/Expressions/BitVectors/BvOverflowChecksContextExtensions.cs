using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Core.Interop;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Spaceorc.Z3Wrap.Expressions.BitVectors;

/// <summary>
/// Provides bit-vector overflow and underflow check methods for Z3Context.
/// </summary>
public static class BvOverflowChecksContextExtensions
{
    /// <summary>
    /// Creates predicate checking if addition does not overflow.
    /// </summary>
    /// <typeparam name="TSize">Bit-vector size type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <param name="signed">True for signed overflow check, false for unsigned.</param>
    /// <returns>Boolean expression true if addition does not overflow.</returns>
    public static BoolExpr AddNoOverflow<TSize>(
        this Z3Context context,
        BvExpr<TSize> left,
        BvExpr<TSize> right,
        bool signed = false
    )
        where TSize : ISize
    {
        var handle = context.Library.Z3MkBvAddNoOverflow(context.Handle, left.Handle, right.Handle, signed);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }

    /// <summary>
    /// Creates predicate checking if signed subtraction does not overflow.
    /// </summary>
    /// <typeparam name="TSize">Bit-vector size type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Boolean expression true if signed subtraction does not overflow.</returns>
    public static BoolExpr SignedSubNoOverflow<TSize>(this Z3Context context, BvExpr<TSize> left, BvExpr<TSize> right)
        where TSize : ISize
    {
        var handle = context.Library.Z3MkBvSubNoOverflow(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }

    /// <summary>
    /// Creates predicate checking if subtraction does not underflow.
    /// </summary>
    /// <typeparam name="TSize">Bit-vector size type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <param name="signed">True for signed underflow check, false for unsigned.</param>
    /// <returns>Boolean expression true if subtraction does not underflow.</returns>
    public static BoolExpr SubNoUnderflow<TSize>(
        this Z3Context context,
        BvExpr<TSize> left,
        BvExpr<TSize> right,
        bool signed = true
    )
        where TSize : ISize
    {
        var handle = context.Library.Z3MkBvSubNoUnderflow(context.Handle, left.Handle, right.Handle, signed);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }

    /// <summary>
    /// Creates predicate checking if multiplication does not overflow.
    /// </summary>
    /// <typeparam name="TSize">Bit-vector size type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <param name="signed">True for signed overflow check, false for unsigned.</param>
    /// <returns>Boolean expression true if multiplication does not overflow.</returns>
    public static BoolExpr MulNoOverflow<TSize>(
        this Z3Context context,
        BvExpr<TSize> left,
        BvExpr<TSize> right,
        bool signed = false
    )
        where TSize : ISize
    {
        var handle = context.Library.Z3MkBvMulNoOverflow(context.Handle, left.Handle, right.Handle, signed);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }

    /// <summary>
    /// Creates predicate checking if signed multiplication does not underflow.
    /// </summary>
    /// <typeparam name="TSize">Bit-vector size type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Boolean expression true if signed multiplication does not underflow.</returns>
    public static BoolExpr SignedMulNoUnderflow<TSize>(this Z3Context context, BvExpr<TSize> left, BvExpr<TSize> right)
        where TSize : ISize
    {
        var handle = context.Library.Z3MkBvMulNoUnderflow(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }

    /// <summary>
    /// Creates predicate checking if signed addition does not underflow.
    /// </summary>
    /// <typeparam name="TSize">Bit-vector size type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Boolean expression true if signed addition does not underflow.</returns>
    public static BoolExpr SignedAddNoUnderflow<TSize>(this Z3Context context, BvExpr<TSize> left, BvExpr<TSize> right)
        where TSize : ISize
    {
        var handle = context.Library.Z3MkBvAddNoUnderflow(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }

    /// <summary>
    /// Creates predicate checking if signed division does not overflow.
    /// </summary>
    /// <typeparam name="TSize">Bit-vector size type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Boolean expression true if signed division does not overflow.</returns>
    public static BoolExpr SignedDivNoOverflow<TSize>(this Z3Context context, BvExpr<TSize> left, BvExpr<TSize> right)
        where TSize : ISize
    {
        var handle = context.Library.Z3MkBvSDivNoOverflow(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }

    /// <summary>
    /// Creates predicate checking if signed negation does not overflow.
    /// </summary>
    /// <typeparam name="TSize">Bit-vector size type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="operand">The operand to negate.</param>
    /// <returns>Boolean expression true if signed negation does not overflow.</returns>
    public static BoolExpr SignedNegNoOverflow<TSize>(this Z3Context context, BvExpr<TSize> operand)
        where TSize : ISize
    {
        var handle = context.Library.Z3MkBvNegNoOverflow(context.Handle, operand.Handle);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }
}
