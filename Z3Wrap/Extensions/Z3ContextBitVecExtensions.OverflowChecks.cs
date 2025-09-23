using System.Numerics;
using Spaceorc.Z3Wrap.BoundaryChecks;
using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.Extensions;

public static partial class Z3ContextBitVecExtensions
{
    /// <summary>
    /// Creates a fluent boundary check builder for bitvector operations.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <returns>A builder for constructing boundary check expressions.</returns>
    public static Z3BitVecBoundaryCheckBuilder BitVecBoundaryCheck(this Z3Context context)
    {
        return new Z3BitVecBoundaryCheckBuilder(context);
    }

    /// <summary>
    /// Checks if addition would cause overflow between two bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <param name="signed">If true, treats operands as signed; otherwise as unsigned.</param>
    /// <returns>A Z3 boolean expression that is true if addition would not overflow.</returns>
    public static Z3BoolExpr AddNoOverflow(
        this Z3Context context,
        Z3BitVecExpr left,
        Z3BitVecExpr right,
        bool signed = false
    )
    {
        if (left.Size != right.Size)
            throw new ArgumentException(
                $"BitVector size mismatch: left={left.Size}, right={right.Size}"
            );
        var handle = SafeNativeMethods.Z3MkBvAddNoOverflow(
            context.Handle,
            left.Handle,
            right.Handle,
            signed
        );
        return Z3BoolExpr.Create(context, handle);
    }

    /// <summary>
    /// Checks if signed subtraction would cause overflow between two bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <returns>A Z3 boolean expression that is true if signed subtraction would not overflow.</returns>
    public static Z3BoolExpr SignedSubNoOverflow(
        this Z3Context context,
        Z3BitVecExpr left,
        Z3BitVecExpr right
    )
    {
        if (left.Size != right.Size)
            throw new ArgumentException(
                $"BitVector size mismatch: left={left.Size}, right={right.Size}"
            );
        var handle = SafeNativeMethods.Z3MkBvSubNoOverflow(
            context.Handle,
            left.Handle,
            right.Handle
        );
        return Z3BoolExpr.Create(context, handle);
    }

    /// <summary>
    /// Checks if subtraction would cause underflow between two bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <param name="signed">If true, treats operands as signed; otherwise as unsigned.</param>
    /// <returns>A Z3 boolean expression that is true if subtraction would not underflow.</returns>
    public static Z3BoolExpr SubNoUnderflow(
        this Z3Context context,
        Z3BitVecExpr left,
        Z3BitVecExpr right,
        bool signed = true
    )
    {
        if (left.Size != right.Size)
            throw new ArgumentException(
                $"BitVector size mismatch: left={left.Size}, right={right.Size}"
            );
        var handle = SafeNativeMethods.Z3MkBvSubNoUnderflow(
            context.Handle,
            left.Handle,
            right.Handle,
            signed
        );
        return Z3BoolExpr.Create(context, handle);
    }

    /// <summary>
    /// Checks if multiplication would cause overflow between two bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <param name="signed">If true, treats operands as signed; otherwise as unsigned.</param>
    /// <returns>A Z3 boolean expression that is true if multiplication would not overflow.</returns>
    public static Z3BoolExpr MulNoOverflow(
        this Z3Context context,
        Z3BitVecExpr left,
        Z3BitVecExpr right,
        bool signed = false
    )
    {
        if (left.Size != right.Size)
            throw new ArgumentException(
                $"BitVector size mismatch: left={left.Size}, right={right.Size}"
            );
        var handle = SafeNativeMethods.Z3MkBvMulNoOverflow(
            context.Handle,
            left.Handle,
            right.Handle,
            signed
        );
        return Z3BoolExpr.Create(context, handle);
    }

    /// <summary>
    /// Checks if signed multiplication would cause underflow between two bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <returns>A Z3 boolean expression that is true if signed multiplication would not underflow.</returns>
    public static Z3BoolExpr SignedMulNoUnderflow(
        this Z3Context context,
        Z3BitVecExpr left,
        Z3BitVecExpr right
    )
    {
        if (left.Size != right.Size)
            throw new ArgumentException(
                $"BitVector size mismatch: left={left.Size}, right={right.Size}"
            );
        var handle = SafeNativeMethods.Z3MkBvMulNoUnderflow(
            context.Handle,
            left.Handle,
            right.Handle
        );
        return Z3BoolExpr.Create(context, handle);
    }

    /// <summary>
    /// Checks if signed addition would cause underflow between two bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <returns>A Z3 boolean expression that is true if signed addition would not underflow.</returns>
    public static Z3BoolExpr SignedAddNoUnderflow(
        this Z3Context context,
        Z3BitVecExpr left,
        Z3BitVecExpr right
    )
    {
        if (left.Size != right.Size)
            throw new ArgumentException(
                $"BitVector size mismatch: left={left.Size}, right={right.Size}"
            );
        var handle = SafeNativeMethods.Z3MkBvAddNoUnderflow(
            context.Handle,
            left.Handle,
            right.Handle
        );
        return Z3BoolExpr.Create(context, handle);
    }

    /// <summary>
    /// Checks if signed division would cause overflow between two bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <returns>A Z3 boolean expression that is true if signed division would not overflow.</returns>
    public static Z3BoolExpr SignedDivNoOverflow(
        this Z3Context context,
        Z3BitVecExpr left,
        Z3BitVecExpr right
    )
    {
        if (left.Size != right.Size)
            throw new ArgumentException(
                $"BitVector size mismatch: left={left.Size}, right={right.Size}"
            );
        var handle = SafeNativeMethods.Z3MkBvSDivNoOverflow(
            context.Handle,
            left.Handle,
            right.Handle
        );
        return Z3BoolExpr.Create(context, handle);
    }

    /// <summary>
    /// Checks if signed negation would cause overflow for a bitvector expression.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="operand">The bitvector expression to negate.</param>
    /// <returns>A Z3 boolean expression that is true if signed negation would not overflow.</returns>
    public static Z3BoolExpr SignedNegNoOverflow(this Z3Context context, Z3BitVecExpr operand)
    {
        var handle = SafeNativeMethods.Z3MkBvNegNoOverflow(context.Handle, operand.Handle);
        return Z3BoolExpr.Create(context, handle);
    }

    /// <summary>
    /// Checks if addition would cause overflow between a bitvector expression and a BigInteger value.
    /// Works with unlimited precision values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right BigInteger value.</param>
    /// <param name="signed">If true, treats operands as signed; otherwise as unsigned.</param>
    /// <returns>A Z3 boolean expression that is true if addition would not overflow.</returns>
    public static Z3BoolExpr AddNoOverflow(
        this Z3Context context,
        Z3BitVecExpr left,
        BigInteger right,
        bool signed = false
    ) => context.AddNoOverflow(left, context.BitVec(right, left.Size), signed);

    /// <summary>
    /// Checks if signed addition would cause underflow between a bitvector expression and a BigInteger value.
    /// Works with unlimited precision values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right BigInteger value.</param>
    /// <returns>A Z3 boolean expression that is true if signed addition would not underflow.</returns>
    public static Z3BoolExpr SignedAddNoUnderflow(
        this Z3Context context,
        Z3BitVecExpr left,
        BigInteger right
    ) => context.SignedAddNoUnderflow(left, context.BitVec(right, left.Size));

    /// <summary>
    /// Checks if signed subtraction would cause overflow between a bitvector expression and a BigInteger value.
    /// Works with unlimited precision values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right BigInteger value.</param>
    /// <returns>A Z3 boolean expression that is true if signed subtraction would not overflow.</returns>
    public static Z3BoolExpr SignedSubNoOverflow(
        this Z3Context context,
        Z3BitVecExpr left,
        BigInteger right
    ) => context.SignedSubNoOverflow(left, context.BitVec(right, left.Size));

    /// <summary>
    /// Checks if subtraction would cause underflow between a bitvector expression and a BigInteger value.
    /// Works with unlimited precision values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right BigInteger value.</param>
    /// <param name="signed">If true, treats operands as signed; otherwise as unsigned.</param>
    /// <returns>A Z3 boolean expression that is true if subtraction would not underflow.</returns>
    public static Z3BoolExpr SubNoUnderflow(
        this Z3Context context,
        Z3BitVecExpr left,
        BigInteger right,
        bool signed = true
    ) => context.SubNoUnderflow(left, context.BitVec(right, left.Size), signed);

    /// <summary>
    /// Checks if multiplication would cause overflow between a bitvector expression and a BigInteger value.
    /// Works with unlimited precision values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right BigInteger value.</param>
    /// <param name="signed">If true, treats operands as signed; otherwise as unsigned.</param>
    /// <returns>A Z3 boolean expression that is true if multiplication would not overflow.</returns>
    public static Z3BoolExpr MulNoOverflow(
        this Z3Context context,
        Z3BitVecExpr left,
        BigInteger right,
        bool signed = false
    ) => context.MulNoOverflow(left, context.BitVec(right, left.Size), signed);

    /// <summary>
    /// Checks if signed multiplication would cause underflow between a bitvector expression and a BigInteger value.
    /// Works with unlimited precision values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right BigInteger value.</param>
    /// <returns>A Z3 boolean expression that is true if signed multiplication would not underflow.</returns>
    public static Z3BoolExpr SignedMulNoUnderflow(
        this Z3Context context,
        Z3BitVecExpr left,
        BigInteger right
    ) => context.SignedMulNoUnderflow(left, context.BitVec(right, left.Size));

    /// <summary>
    /// Checks if signed division would cause overflow between a bitvector expression and a BigInteger value.
    /// Works with unlimited precision values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right BigInteger value.</param>
    /// <returns>A Z3 boolean expression that is true if signed division would not overflow.</returns>
    public static Z3BoolExpr SignedDivNoOverflow(
        this Z3Context context,
        Z3BitVecExpr left,
        BigInteger right
    ) => context.SignedDivNoOverflow(left, context.BitVec(right, left.Size));

    /// <summary>
    /// Checks if addition would cause overflow between a BigInteger value and a bitvector expression.
    /// Works with unlimited precision values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left BigInteger value.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <param name="signed">If true, treats operands as signed; otherwise as unsigned.</param>
    /// <returns>A Z3 boolean expression that is true if addition would not overflow.</returns>
    public static Z3BoolExpr AddNoOverflow(
        this Z3Context context,
        BigInteger left,
        Z3BitVecExpr right,
        bool signed = false
    ) => context.AddNoOverflow(context.BitVec(left, right.Size), right, signed);

    /// <summary>
    /// Checks if signed addition would cause underflow between a BigInteger value and a bitvector expression.
    /// Works with unlimited precision values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left BigInteger value.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <returns>A Z3 boolean expression that is true if signed addition would not underflow.</returns>
    public static Z3BoolExpr SignedAddNoUnderflow(
        this Z3Context context,
        BigInteger left,
        Z3BitVecExpr right
    ) => context.SignedAddNoUnderflow(context.BitVec(left, right.Size), right);

    /// <summary>
    /// Checks if signed subtraction would cause overflow between a BigInteger value and a bitvector expression.
    /// Works with unlimited precision values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left BigInteger value.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <returns>A Z3 boolean expression that is true if signed subtraction would not overflow.</returns>
    public static Z3BoolExpr SignedSubNoOverflow(
        this Z3Context context,
        BigInteger left,
        Z3BitVecExpr right
    ) => context.SignedSubNoOverflow(context.BitVec(left, right.Size), right);

    /// <summary>
    /// Checks if subtraction would cause underflow between a BigInteger value and a bitvector expression.
    /// Works with unlimited precision values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left BigInteger value.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <param name="signed">If true, treats operands as signed; otherwise as unsigned.</param>
    /// <returns>A Z3 boolean expression that is true if subtraction would not underflow.</returns>
    public static Z3BoolExpr SubNoUnderflow(
        this Z3Context context,
        BigInteger left,
        Z3BitVecExpr right,
        bool signed = true
    ) => context.SubNoUnderflow(context.BitVec(left, right.Size), right, signed);

    /// <summary>
    /// Checks if multiplication would cause overflow between a BigInteger value and a bitvector expression.
    /// Works with unlimited precision values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left BigInteger value.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <param name="signed">If true, treats operands as signed; otherwise as unsigned.</param>
    /// <returns>A Z3 boolean expression that is true if multiplication would not overflow.</returns>
    public static Z3BoolExpr MulNoOverflow(
        this Z3Context context,
        BigInteger left,
        Z3BitVecExpr right,
        bool signed = false
    ) => context.MulNoOverflow(context.BitVec(left, right.Size), right, signed);

    /// <summary>
    /// Checks if signed multiplication would cause underflow between a BigInteger value and a bitvector expression.
    /// Works with unlimited precision values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left BigInteger value.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <returns>A Z3 boolean expression that is true if signed multiplication would not underflow.</returns>
    public static Z3BoolExpr SignedMulNoUnderflow(
        this Z3Context context,
        BigInteger left,
        Z3BitVecExpr right
    ) => context.SignedMulNoUnderflow(context.BitVec(left, right.Size), right);

    /// <summary>
    /// Checks if signed division would cause overflow between a BigInteger value and a bitvector expression.
    /// Works with unlimited precision values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left BigInteger value.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <returns>A Z3 boolean expression that is true if signed division would not overflow.</returns>
    public static Z3BoolExpr SignedDivNoOverflow(
        this Z3Context context,
        BigInteger left,
        Z3BitVecExpr right
    ) => context.SignedDivNoOverflow(context.BitVec(left, right.Size), right);
}
