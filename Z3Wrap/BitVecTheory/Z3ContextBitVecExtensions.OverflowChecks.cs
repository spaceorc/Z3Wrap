using Spaceorc.Z3Wrap.BoolTheory;
using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Interop;
using Spaceorc.Z3Wrap.Values;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Spaceorc.Z3Wrap.BitVecTheory;

public static partial class Z3ContextBitVecExtensions
{
    /// <summary>
    /// Checks if addition would cause overflow between two compile-time size-validated bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <param name="signed">If true, treats operands as signed; otherwise as unsigned.</param>
    /// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
    /// <returns>A Z3 boolean expression that is true if addition would not overflow.</returns>
    public static Z3Bool AddNoOverflow<TSize>(
        this Z3Context context,
        Z3BitVec<TSize> left,
        Z3BitVec<TSize> right,
        bool signed = false
    )
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvAddNoOverflow(
            context.Handle,
            left.Handle,
            right.Handle,
            signed
        );
        return Z3Expr.Create<Z3Bool>(context, handle);
    }

    /// <summary>
    /// Checks if signed subtraction would cause overflow between two compile-time size-validated bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
    /// <returns>A Z3 boolean expression that is true if signed subtraction would not overflow.</returns>
    public static Z3Bool SignedSubNoOverflow<TSize>(
        this Z3Context context,
        Z3BitVec<TSize> left,
        Z3BitVec<TSize> right
    )
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvSubNoOverflow(
            context.Handle,
            left.Handle,
            right.Handle
        );
        return Z3Expr.Create<Z3Bool>(context, handle);
    }

    /// <summary>
    /// Checks if subtraction would cause underflow between two compile-time size-validated bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <param name="signed">If true, treats operands as signed; otherwise as unsigned.</param>
    /// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
    /// <returns>A Z3 boolean expression that is true if subtraction would not underflow.</returns>
    public static Z3Bool SubNoUnderflow<TSize>(
        this Z3Context context,
        Z3BitVec<TSize> left,
        Z3BitVec<TSize> right,
        bool signed = true
    )
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvSubNoUnderflow(
            context.Handle,
            left.Handle,
            right.Handle,
            signed
        );
        return Z3Expr.Create<Z3Bool>(context, handle);
    }

    /// <summary>
    /// Checks if multiplication would cause overflow between two compile-time size-validated bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <param name="signed">If true, treats operands as signed; otherwise as unsigned.</param>
    /// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
    /// <returns>A Z3 boolean expression that is true if multiplication would not overflow.</returns>
    public static Z3Bool MulNoOverflow<TSize>(
        this Z3Context context,
        Z3BitVec<TSize> left,
        Z3BitVec<TSize> right,
        bool signed = false
    )
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvMulNoOverflow(
            context.Handle,
            left.Handle,
            right.Handle,
            signed
        );
        return Z3Expr.Create<Z3Bool>(context, handle);
    }

    /// <summary>
    /// Checks if signed multiplication would cause underflow between two compile-time size-validated bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
    /// <returns>A Z3 boolean expression that is true if signed multiplication would not underflow.</returns>
    public static Z3Bool SignedMulNoUnderflow<TSize>(
        this Z3Context context,
        Z3BitVec<TSize> left,
        Z3BitVec<TSize> right
    )
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvMulNoUnderflow(
            context.Handle,
            left.Handle,
            right.Handle
        );
        return Z3Expr.Create<Z3Bool>(context, handle);
    }

    /// <summary>
    /// Checks if signed addition would cause underflow between two compile-time size-validated bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
    /// <returns>A Z3 boolean expression that is true if signed addition would not underflow.</returns>
    public static Z3Bool SignedAddNoUnderflow<TSize>(
        this Z3Context context,
        Z3BitVec<TSize> left,
        Z3BitVec<TSize> right
    )
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvAddNoUnderflow(
            context.Handle,
            left.Handle,
            right.Handle
        );
        return Z3Expr.Create<Z3Bool>(context, handle);
    }

    /// <summary>
    /// Checks if signed division would cause overflow between two compile-time size-validated bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
    /// <returns>A Z3 boolean expression that is true if signed division would not overflow.</returns>
    public static Z3Bool SignedDivNoOverflow<TSize>(
        this Z3Context context,
        Z3BitVec<TSize> left,
        Z3BitVec<TSize> right
    )
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvSDivNoOverflow(
            context.Handle,
            left.Handle,
            right.Handle
        );
        return Z3Expr.Create<Z3Bool>(context, handle);
    }

    /// <summary>
    /// Checks if signed negation would cause overflow for a compile-time size-validated bitvector expression.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="operand">The bitvector expression to negate.</param>
    /// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
    /// <returns>A Z3 boolean expression that is true if signed negation would not overflow.</returns>
    public static Z3Bool SignedNegNoOverflow<TSize>(this Z3Context context, Z3BitVec<TSize> operand)
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvNegNoOverflow(context.Handle, operand.Handle);
        return Z3Expr.Create<Z3Bool>(context, handle);
    }
}
