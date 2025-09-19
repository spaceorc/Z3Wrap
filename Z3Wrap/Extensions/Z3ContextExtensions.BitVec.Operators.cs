using System.Numerics;
using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.Extensions;

public static partial class Z3ContextExtensions
{
    /// <summary>
    /// Creates an equality expression between a bitvector expression and a BigInteger value.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right BigInteger value.</param>
    /// <returns>A Z3 boolean expression representing the equality comparison.</returns>
    public static Z3BoolExpr Eq(this Z3Context context, Z3BitVecExpr left, BigInteger right) =>
        context.Eq(left, context.BitVec(right, left.Size));

    /// <summary>
    /// Creates an equality expression between a BigInteger value and a bitvector expression.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left BigInteger value.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <returns>A Z3 boolean expression representing the equality comparison.</returns>
    public static Z3BoolExpr Eq(this Z3Context context, BigInteger left, Z3BitVecExpr right) =>
        context.Eq(context.BitVec(left, right.Size), right);

    /// <summary>
    /// Creates an inequality expression between a bitvector expression and a BigInteger value.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right BigInteger value.</param>
    /// <returns>A Z3 boolean expression representing the inequality comparison.</returns>
    public static Z3BoolExpr Neq(this Z3Context context, Z3BitVecExpr left, BigInteger right) =>
        context.Neq(left, context.BitVec(right, left.Size));

    /// <summary>
    /// Creates an inequality expression between a BigInteger value and a bitvector expression.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left BigInteger value.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <returns>A Z3 boolean expression representing the inequality comparison.</returns>
    public static Z3BoolExpr Neq(this Z3Context context, BigInteger left, Z3BitVecExpr right) =>
        context.Neq(context.BitVec(left, right.Size), right);

    /// <summary>
    /// Creates an addition expression between two bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <returns>A Z3 bitvector expression representing the addition.</returns>
    public static Z3BitVecExpr Add(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException(
                $"BitVector size mismatch: left={left.Size}, right={right.Size}"
            );
        var handle = NativeMethods.Z3MkBvAdd(context.Handle, left.Handle, right.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    /// <summary>
    /// Creates a subtraction expression between two bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <returns>A Z3 bitvector expression representing the subtraction.</returns>
    public static Z3BitVecExpr Sub(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException(
                $"BitVector size mismatch: left={left.Size}, right={right.Size}"
            );
        var handle = NativeMethods.Z3MkBvSub(context.Handle, left.Handle, right.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    /// <summary>
    /// Creates a multiplication expression between two bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <returns>A Z3 bitvector expression representing the multiplication.</returns>
    public static Z3BitVecExpr Mul(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException(
                $"BitVector size mismatch: left={left.Size}, right={right.Size}"
            );
        var handle = NativeMethods.Z3MkBvMul(context.Handle, left.Handle, right.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    /// <summary>
    /// Creates a division expression between two bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <param name="signed">If true, treats operands as signed; otherwise as unsigned.</param>
    /// <returns>A Z3 bitvector expression representing the division.</returns>
    public static Z3BitVecExpr Div(
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
        var handle = signed
            ? NativeMethods.Z3MkBvSDiv(context.Handle, left.Handle, right.Handle)
            : NativeMethods.Z3MkBvUDiv(context.Handle, left.Handle, right.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    /// <summary>
    /// Creates a remainder expression between two bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <param name="signed">If true, treats operands as signed; otherwise as unsigned.</param>
    /// <returns>A Z3 bitvector expression representing the remainder.</returns>
    public static Z3BitVecExpr Rem(
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
        var handle = signed
            ? NativeMethods.Z3MkBvSRem(context.Handle, left.Handle, right.Handle)
            : NativeMethods.Z3MkBvURem(context.Handle, left.Handle, right.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    /// <summary>
    /// Creates a signed modulo expression between two bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <returns>A Z3 bitvector expression representing the signed modulo operation.</returns>
    public static Z3BitVecExpr SignedMod(
        this Z3Context context,
        Z3BitVecExpr left,
        Z3BitVecExpr right
    )
    {
        if (left.Size != right.Size)
            throw new ArgumentException(
                $"BitVector size mismatch: left={left.Size}, right={right.Size}"
            );
        var handle = NativeMethods.Z3MkBvSMod(context.Handle, left.Handle, right.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    /// <summary>
    /// Creates a negation expression for a bitvector expression.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="expr">The bitvector expression to negate.</param>
    /// <returns>A Z3 bitvector expression representing the negation.</returns>
    public static Z3BitVecExpr Neg(this Z3Context context, Z3BitVecExpr expr)
    {
        var handle = NativeMethods.Z3MkBvNeg(context.Handle, expr.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    /// <summary>
    /// Performs bitwise AND on two bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <returns>A Z3 bitvector expression representing the bitwise AND.</returns>
    public static Z3BitVecExpr And(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException(
                $"BitVector size mismatch: left={left.Size}, right={right.Size}"
            );
        var handle = NativeMethods.Z3MkBvAnd(context.Handle, left.Handle, right.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    /// <summary>
    /// Performs bitwise OR on two bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <returns>A Z3 bitvector expression representing the bitwise OR.</returns>
    public static Z3BitVecExpr Or(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException(
                $"BitVector size mismatch: left={left.Size}, right={right.Size}"
            );
        var handle = NativeMethods.Z3MkBvOr(context.Handle, left.Handle, right.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    /// <summary>
    /// Performs bitwise XOR on two bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <returns>A Z3 bitvector expression representing the bitwise XOR.</returns>
    public static Z3BitVecExpr Xor(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException(
                $"BitVector size mismatch: left={left.Size}, right={right.Size}"
            );
        var handle = NativeMethods.Z3MkBvXor(context.Handle, left.Handle, right.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    /// <summary>
    /// Performs bitwise NOT on a bitvector expression.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="expr">The bitvector expression to negate bitwise.</param>
    /// <returns>A Z3 bitvector expression representing the bitwise NOT.</returns>
    public static Z3BitVecExpr Not(this Z3Context context, Z3BitVecExpr expr)
    {
        var handle = NativeMethods.Z3MkBvNot(context.Handle, expr.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    /// <summary>
    /// Performs left shift operation on bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The bitvector expression to shift.</param>
    /// <param name="right">The bitvector expression specifying the shift amount.</param>
    /// <returns>A Z3 bitvector expression representing the left shift.</returns>
    public static Z3BitVecExpr Shl(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException(
                $"BitVector size mismatch: left={left.Size}, right={right.Size}"
            );
        var handle = NativeMethods.Z3MkBvShl(context.Handle, left.Handle, right.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    /// <summary>
    /// Performs right shift operation on bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The bitvector expression to shift.</param>
    /// <param name="right">The bitvector expression specifying the shift amount.</param>
    /// <param name="signed">If true, performs arithmetic right shift; otherwise logical right shift.</param>
    /// <returns>A Z3 bitvector expression representing the right shift.</returns>
    public static Z3BitVecExpr Shr(
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
        var handle = signed
            ? NativeMethods.Z3MkBvAShr(context.Handle, left.Handle, right.Handle)
            : NativeMethods.Z3MkBvLShr(context.Handle, left.Handle, right.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    /// <summary>
    /// Creates a less-than comparison expression between two bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <param name="signed">If true, treats operands as signed; otherwise as unsigned.</param>
    /// <returns>A Z3 boolean expression representing the less-than comparison.</returns>
    public static Z3BoolExpr Lt(
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
        var handle = signed
            ? NativeMethods.Z3MkBvSLt(context.Handle, left.Handle, right.Handle)
            : NativeMethods.Z3MkBvULt(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, handle);
    }

    /// <summary>
    /// Creates a less-than-or-equal comparison expression between two bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <param name="signed">If true, treats operands as signed; otherwise as unsigned.</param>
    /// <returns>A Z3 boolean expression representing the less-than-or-equal comparison.</returns>
    public static Z3BoolExpr Le(
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
        var handle = signed
            ? NativeMethods.Z3MkBvSLe(context.Handle, left.Handle, right.Handle)
            : NativeMethods.Z3MkBvULe(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, handle);
    }

    /// <summary>
    /// Creates a greater-than comparison expression between two bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <param name="signed">If true, treats operands as signed; otherwise as unsigned.</param>
    /// <returns>A Z3 boolean expression representing the greater-than comparison.</returns>
    public static Z3BoolExpr Gt(
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
        var handle = signed
            ? NativeMethods.Z3MkBvSGt(context.Handle, left.Handle, right.Handle)
            : NativeMethods.Z3MkBvUGt(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, handle);
    }

    /// <summary>
    /// Creates a greater-than-or-equal comparison expression between two bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <param name="signed">If true, treats operands as signed; otherwise as unsigned.</param>
    /// <returns>A Z3 boolean expression representing the greater-than-or-equal comparison.</returns>
    public static Z3BoolExpr Ge(
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
        var handle = signed
            ? NativeMethods.Z3MkBvSGe(context.Handle, left.Handle, right.Handle)
            : NativeMethods.Z3MkBvUGe(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, handle);
    }

    /// <summary>
    /// Creates an addition expression between a bitvector expression and a BigInteger value.
    /// Works with unlimited precision values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right BigInteger value.</param>
    /// <returns>A Z3 bitvector expression representing the addition.</returns>
    public static Z3BitVecExpr Add(this Z3Context context, Z3BitVecExpr left, BigInteger right) =>
        context.Add(left, context.BitVec(right, left.Size));

    /// <summary>
    /// Creates a subtraction expression between a bitvector expression and a BigInteger value.
    /// Works with unlimited precision values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right BigInteger value.</param>
    /// <returns>A Z3 bitvector expression representing the subtraction.</returns>
    public static Z3BitVecExpr Sub(this Z3Context context, Z3BitVecExpr left, BigInteger right) =>
        context.Sub(left, context.BitVec(right, left.Size));

    /// <summary>
    /// Creates a multiplication expression between a bitvector expression and a BigInteger value.
    /// Works with unlimited precision values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right BigInteger value.</param>
    /// <returns>A Z3 bitvector expression representing the multiplication.</returns>
    public static Z3BitVecExpr Mul(this Z3Context context, Z3BitVecExpr left, BigInteger right) =>
        context.Mul(left, context.BitVec(right, left.Size));

    /// <summary>
    /// Creates a division expression between a bitvector expression and a BigInteger value.
    /// Works with unlimited precision values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right BigInteger value.</param>
    /// <param name="signed">If true, treats operands as signed; otherwise as unsigned.</param>
    /// <returns>A Z3 bitvector expression representing the division.</returns>
    public static Z3BitVecExpr Div(
        this Z3Context context,
        Z3BitVecExpr left,
        BigInteger right,
        bool signed = false
    ) => context.Div(left, context.BitVec(right, left.Size), signed);

    /// <summary>
    /// Creates a remainder expression between a bitvector expression and a BigInteger value.
    /// Works with unlimited precision values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right BigInteger value.</param>
    /// <param name="signed">If true, treats operands as signed; otherwise as unsigned.</param>
    /// <returns>A Z3 bitvector expression representing the remainder.</returns>
    public static Z3BitVecExpr Rem(
        this Z3Context context,
        Z3BitVecExpr left,
        BigInteger right,
        bool signed = false
    ) => context.Rem(left, context.BitVec(right, left.Size), signed);

    /// <summary>
    /// Creates a signed modulo expression between a bitvector expression and a BigInteger value.
    /// Works with unlimited precision values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right BigInteger value.</param>
    /// <returns>A Z3 bitvector expression representing the signed modulo operation.</returns>
    public static Z3BitVecExpr SignedMod(
        this Z3Context context,
        Z3BitVecExpr left,
        BigInteger right
    ) => context.SignedMod(left, context.BitVec(right, left.Size));

    /// <summary>
    /// Creates an addition expression between a BigInteger value and a bitvector expression.
    /// Works with unlimited precision values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left BigInteger value.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <returns>A Z3 bitvector expression representing the addition.</returns>
    public static Z3BitVecExpr Add(this Z3Context context, BigInteger left, Z3BitVecExpr right) =>
        context.Add(context.BitVec(left, right.Size), right);

    /// <summary>
    /// Creates a subtraction expression between a BigInteger value and a bitvector expression.
    /// Works with unlimited precision values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left BigInteger value.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <returns>A Z3 bitvector expression representing the subtraction.</returns>
    public static Z3BitVecExpr Sub(this Z3Context context, BigInteger left, Z3BitVecExpr right) =>
        context.Sub(context.BitVec(left, right.Size), right);

    /// <summary>
    /// Creates a multiplication expression between a BigInteger value and a bitvector expression.
    /// Works with unlimited precision values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left BigInteger value.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <returns>A Z3 bitvector expression representing the multiplication.</returns>
    public static Z3BitVecExpr Mul(this Z3Context context, BigInteger left, Z3BitVecExpr right) =>
        context.Mul(context.BitVec(left, right.Size), right);

    /// <summary>
    /// Creates a division expression between a BigInteger value and a bitvector expression.
    /// Works with unlimited precision values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left BigInteger value.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <param name="signed">If true, treats operands as signed; otherwise as unsigned.</param>
    /// <returns>A Z3 bitvector expression representing the division.</returns>
    public static Z3BitVecExpr Div(
        this Z3Context context,
        BigInteger left,
        Z3BitVecExpr right,
        bool signed = false
    ) => context.Div(context.BitVec(left, right.Size), right, signed);

    /// <summary>
    /// Creates a remainder expression between a BigInteger value and a bitvector expression.
    /// Works with unlimited precision values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left BigInteger value.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <param name="signed">If true, treats operands as signed; otherwise as unsigned.</param>
    /// <returns>A Z3 bitvector expression representing the remainder.</returns>
    public static Z3BitVecExpr Rem(
        this Z3Context context,
        BigInteger left,
        Z3BitVecExpr right,
        bool signed = false
    ) => context.Rem(context.BitVec(left, right.Size), right, signed);

    /// <summary>
    /// Creates a signed modulo expression between a BigInteger value and a bitvector expression.
    /// Works with unlimited precision values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left BigInteger value.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <returns>A Z3 bitvector expression representing the signed modulo operation.</returns>
    public static Z3BitVecExpr SignedMod(
        this Z3Context context,
        BigInteger left,
        Z3BitVecExpr right
    ) => context.SignedMod(context.BitVec(left, right.Size), right);

    /// <summary>
    /// Performs bitwise AND between a bitvector expression and a BigInteger value.
    /// Works with unlimited precision values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right BigInteger value.</param>
    /// <returns>A Z3 bitvector expression representing the bitwise AND.</returns>
    public static Z3BitVecExpr And(this Z3Context context, Z3BitVecExpr left, BigInteger right) =>
        context.And(left, context.BitVec(right, left.Size));

    /// <summary>
    /// Performs bitwise OR between a bitvector expression and a BigInteger value.
    /// Works with unlimited precision values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right BigInteger value.</param>
    /// <returns>A Z3 bitvector expression representing the bitwise OR.</returns>
    public static Z3BitVecExpr Or(this Z3Context context, Z3BitVecExpr left, BigInteger right) =>
        context.Or(left, context.BitVec(right, left.Size));

    /// <summary>
    /// Performs bitwise XOR between a bitvector expression and a BigInteger value.
    /// Works with unlimited precision values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right BigInteger value.</param>
    /// <returns>A Z3 bitvector expression representing the bitwise XOR.</returns>
    public static Z3BitVecExpr Xor(this Z3Context context, Z3BitVecExpr left, BigInteger right) =>
        context.Xor(left, context.BitVec(right, left.Size));

    /// <summary>
    /// Performs bitwise AND between a BigInteger value and a bitvector expression.
    /// Works with unlimited precision values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left BigInteger value.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <returns>A Z3 bitvector expression representing the bitwise AND.</returns>
    public static Z3BitVecExpr And(this Z3Context context, BigInteger left, Z3BitVecExpr right) =>
        context.And(context.BitVec(left, right.Size), right);

    /// <summary>
    /// Performs bitwise OR between a BigInteger value and a bitvector expression.
    /// Works with unlimited precision values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left BigInteger value.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <returns>A Z3 bitvector expression representing the bitwise OR.</returns>
    public static Z3BitVecExpr Or(this Z3Context context, BigInteger left, Z3BitVecExpr right) =>
        context.Or(context.BitVec(left, right.Size), right);

    /// <summary>
    /// Performs bitwise XOR between a BigInteger value and a bitvector expression.
    /// Works with unlimited precision values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left BigInteger value.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <returns>A Z3 bitvector expression representing the bitwise XOR.</returns>
    public static Z3BitVecExpr Xor(this Z3Context context, BigInteger left, Z3BitVecExpr right) =>
        context.Xor(context.BitVec(left, right.Size), right);

    /// <summary>
    /// Performs left shift operation on a bitvector expression with a BigInteger shift amount.
    /// Works with unlimited precision values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The bitvector expression to shift.</param>
    /// <param name="right">The BigInteger value specifying the shift amount.</param>
    /// <returns>A Z3 bitvector expression representing the left shift.</returns>
    public static Z3BitVecExpr Shl(this Z3Context context, Z3BitVecExpr left, BigInteger right) =>
        context.Shl(left, context.BitVec(right, left.Size));

    /// <summary>
    /// Performs right shift operation on a bitvector expression with a BigInteger shift amount.
    /// Works with unlimited precision values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The bitvector expression to shift.</param>
    /// <param name="right">The BigInteger value specifying the shift amount.</param>
    /// <param name="signed">If true, performs arithmetic right shift; otherwise logical right shift.</param>
    /// <returns>A Z3 bitvector expression representing the right shift.</returns>
    public static Z3BitVecExpr Shr(
        this Z3Context context,
        Z3BitVecExpr left,
        BigInteger right,
        bool signed = false
    ) => context.Shr(left, context.BitVec(right, left.Size), signed);

    /// <summary>
    /// Performs left shift operation on a BigInteger value with a bitvector expression shift amount.
    /// Works with unlimited precision values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The BigInteger value to shift.</param>
    /// <param name="right">The bitvector expression specifying the shift amount.</param>
    /// <returns>A Z3 bitvector expression representing the left shift.</returns>
    public static Z3BitVecExpr Shl(this Z3Context context, BigInteger left, Z3BitVecExpr right) =>
        context.Shl(context.BitVec(left, right.Size), right);

    /// <summary>
    /// Performs right shift operation on a BigInteger value with a bitvector expression shift amount.
    /// Works with unlimited precision values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The BigInteger value to shift.</param>
    /// <param name="right">The bitvector expression specifying the shift amount.</param>
    /// <param name="signed">If true, performs arithmetic right shift; otherwise logical right shift.</param>
    /// <returns>A Z3 bitvector expression representing the right shift.</returns>
    public static Z3BitVecExpr Shr(
        this Z3Context context,
        BigInteger left,
        Z3BitVecExpr right,
        bool signed = false
    ) => context.Shr(context.BitVec(left, right.Size), right, signed);

    /// <summary>
    /// Creates a less-than comparison expression between a bitvector expression and a BigInteger value.
    /// Works with unlimited precision values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right BigInteger value.</param>
    /// <param name="signed">If true, treats operands as signed; otherwise as unsigned.</param>
    /// <returns>A Z3 boolean expression representing the less-than comparison.</returns>
    public static Z3BoolExpr Lt(
        this Z3Context context,
        Z3BitVecExpr left,
        BigInteger right,
        bool signed = false
    ) => context.Lt(left, context.BitVec(right, left.Size), signed);

    /// <summary>
    /// Creates a less-than-or-equal comparison expression between a bitvector expression and a BigInteger value.
    /// Works with unlimited precision values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right BigInteger value.</param>
    /// <param name="signed">If true, treats operands as signed; otherwise as unsigned.</param>
    /// <returns>A Z3 boolean expression representing the less-than-or-equal comparison.</returns>
    public static Z3BoolExpr Le(
        this Z3Context context,
        Z3BitVecExpr left,
        BigInteger right,
        bool signed = false
    ) => context.Le(left, context.BitVec(right, left.Size), signed);

    /// <summary>
    /// Creates a greater-than comparison expression between a bitvector expression and a BigInteger value.
    /// Works with unlimited precision values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right BigInteger value.</param>
    /// <param name="signed">If true, treats operands as signed; otherwise as unsigned.</param>
    /// <returns>A Z3 boolean expression representing the greater-than comparison.</returns>
    public static Z3BoolExpr Gt(
        this Z3Context context,
        Z3BitVecExpr left,
        BigInteger right,
        bool signed = false
    ) => context.Gt(left, context.BitVec(right, left.Size), signed);

    /// <summary>
    /// Creates a greater-than-or-equal comparison expression between a bitvector expression and a BigInteger value.
    /// Works with unlimited precision values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right BigInteger value.</param>
    /// <param name="signed">If true, treats operands as signed; otherwise as unsigned.</param>
    /// <returns>A Z3 boolean expression representing the greater-than-or-equal comparison.</returns>
    public static Z3BoolExpr Ge(
        this Z3Context context,
        Z3BitVecExpr left,
        BigInteger right,
        bool signed = false
    ) => context.Ge(left, context.BitVec(right, left.Size), signed);

    /// <summary>
    /// Creates a less-than comparison expression between a BigInteger value and a bitvector expression.
    /// Works with unlimited precision values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left BigInteger value.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <param name="signed">If true, treats operands as signed; otherwise as unsigned.</param>
    /// <returns>A Z3 boolean expression representing the less-than comparison.</returns>
    public static Z3BoolExpr Lt(
        this Z3Context context,
        BigInteger left,
        Z3BitVecExpr right,
        bool signed = false
    ) => context.Lt(context.BitVec(left, right.Size), right, signed);

    /// <summary>
    /// Creates a less-than-or-equal comparison expression between a BigInteger value and a bitvector expression.
    /// Works with unlimited precision values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left BigInteger value.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <param name="signed">If true, treats operands as signed; otherwise as unsigned.</param>
    /// <returns>A Z3 boolean expression representing the less-than-or-equal comparison.</returns>
    public static Z3BoolExpr Le(
        this Z3Context context,
        BigInteger left,
        Z3BitVecExpr right,
        bool signed = false
    ) => context.Le(context.BitVec(left, right.Size), right, signed);

    /// <summary>
    /// Creates a greater-than comparison expression between a BigInteger value and a bitvector expression.
    /// Works with unlimited precision values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left BigInteger value.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <param name="signed">If true, treats operands as signed; otherwise as unsigned.</param>
    /// <returns>A Z3 boolean expression representing the greater-than comparison.</returns>
    public static Z3BoolExpr Gt(
        this Z3Context context,
        BigInteger left,
        Z3BitVecExpr right,
        bool signed = false
    ) => context.Gt(context.BitVec(left, right.Size), right, signed);

    /// <summary>
    /// Creates a greater-than-or-equal comparison expression between a BigInteger value and a bitvector expression.
    /// Works with unlimited precision values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left BigInteger value.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <param name="signed">If true, treats operands as signed; otherwise as unsigned.</param>
    /// <returns>A Z3 boolean expression representing the greater-than-or-equal comparison.</returns>
    public static Z3BoolExpr Ge(
        this Z3Context context,
        BigInteger left,
        Z3BitVecExpr right,
        bool signed = false
    ) => context.Ge(context.BitVec(left, right.Size), right, signed);
}
