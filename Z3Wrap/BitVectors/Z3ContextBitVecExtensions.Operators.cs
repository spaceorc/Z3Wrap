using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.BitVectors;

public static partial class Z3ContextBitVecExtensions2
{
    /// <summary>
    /// Creates an addition expression between two compile-time size-validated bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
    /// <returns>A type-safe Z3 bitvector expression representing the addition.</returns>
    public static Z3BitVec<TSize> Add<TSize>(
        this Z3Context context,
        Z3BitVec<TSize> left,
        Z3BitVec<TSize> right
    )
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvAdd(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<Z3BitVec<TSize>>(context, handle);
    }

    /// <summary>
    /// Creates a subtraction expression between two compile-time size-validated bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
    /// <returns>A type-safe Z3 bitvector expression representing the subtraction.</returns>
    public static Z3BitVec<TSize> Sub<TSize>(
        this Z3Context context,
        Z3BitVec<TSize> left,
        Z3BitVec<TSize> right
    )
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvSub(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<Z3BitVec<TSize>>(context, handle);
    }

    /// <summary>
    /// Creates a multiplication expression between two compile-time size-validated bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
    /// <returns>A type-safe Z3 bitvector expression representing the multiplication.</returns>
    public static Z3BitVec<TSize> Mul<TSize>(
        this Z3Context context,
        Z3BitVec<TSize> left,
        Z3BitVec<TSize> right
    )
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvMul(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<Z3BitVec<TSize>>(context, handle);
    }

    /// <summary>
    /// Creates a division expression between two compile-time size-validated bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <param name="signed">If true, treats operands as signed; otherwise as unsigned.</param>
    /// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
    /// <returns>A type-safe Z3 bitvector expression representing the division.</returns>
    public static Z3BitVec<TSize> Div<TSize>(
        this Z3Context context,
        Z3BitVec<TSize> left,
        Z3BitVec<TSize> right,
        bool signed = false
    )
        where TSize : ISize
    {
        var handle = signed
            ? SafeNativeMethods.Z3MkBvSDiv(context.Handle, left.Handle, right.Handle)
            : SafeNativeMethods.Z3MkBvUDiv(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<Z3BitVec<TSize>>(context, handle);
    }

    /// <summary>
    /// Creates a remainder expression between two compile-time size-validated bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <param name="signed">If true, treats operands as signed; otherwise as unsigned.</param>
    /// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
    /// <returns>A type-safe Z3 bitvector expression representing the remainder.</returns>
    public static Z3BitVec<TSize> Rem<TSize>(
        this Z3Context context,
        Z3BitVec<TSize> left,
        Z3BitVec<TSize> right,
        bool signed = false
    )
        where TSize : ISize
    {
        var handle = signed
            ? SafeNativeMethods.Z3MkBvSRem(context.Handle, left.Handle, right.Handle)
            : SafeNativeMethods.Z3MkBvURem(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<Z3BitVec<TSize>>(context, handle);
    }

    /// <summary>
    /// Creates a signed modulo expression between two compile-time size-validated bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
    /// <returns>A type-safe Z3 bitvector expression representing the signed modulo operation.</returns>
    public static Z3BitVec<TSize> SignedMod<TSize>(
        this Z3Context context,
        Z3BitVec<TSize> left,
        Z3BitVec<TSize> right
    )
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvSMod(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<Z3BitVec<TSize>>(context, handle);
    }

    /// <summary>
    /// Creates a negation expression for a compile-time size-validated bitvector expression.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="expr">The bitvector expression to negate.</param>
    /// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
    /// <returns>A type-safe Z3 bitvector expression representing the negation.</returns>
    public static Z3BitVec<TSize> Neg<TSize>(this Z3Context context, Z3BitVec<TSize> expr)
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvNeg(context.Handle, expr.Handle);
        return Z3Expr.Create<Z3BitVec<TSize>>(context, handle);
    }

    /// <summary>
    /// Performs bitwise AND on two compile-time size-validated bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
    /// <returns>A type-safe Z3 bitvector expression representing the bitwise AND.</returns>
    public static Z3BitVec<TSize> And<TSize>(
        this Z3Context context,
        Z3BitVec<TSize> left,
        Z3BitVec<TSize> right
    )
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvAnd(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<Z3BitVec<TSize>>(context, handle);
    }

    /// <summary>
    /// Performs bitwise OR on two compile-time size-validated bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
    /// <returns>A type-safe Z3 bitvector expression representing the bitwise OR.</returns>
    public static Z3BitVec<TSize> Or<TSize>(
        this Z3Context context,
        Z3BitVec<TSize> left,
        Z3BitVec<TSize> right
    )
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvOr(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<Z3BitVec<TSize>>(context, handle);
    }

    /// <summary>
    /// Performs bitwise XOR on two compile-time size-validated bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
    /// <returns>A type-safe Z3 bitvector expression representing the bitwise XOR.</returns>
    public static Z3BitVec<TSize> Xor<TSize>(
        this Z3Context context,
        Z3BitVec<TSize> left,
        Z3BitVec<TSize> right
    )
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvXor(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<Z3BitVec<TSize>>(context, handle);
    }

    /// <summary>
    /// Performs bitwise NOT on a compile-time size-validated bitvector expression.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="expr">The bitvector expression to negate bitwise.</param>
    /// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
    /// <returns>A type-safe Z3 bitvector expression representing the bitwise NOT.</returns>
    public static Z3BitVec<TSize> Not<TSize>(this Z3Context context, Z3BitVec<TSize> expr)
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvNot(context.Handle, expr.Handle);
        return Z3Expr.Create<Z3BitVec<TSize>>(context, handle);
    }

    /// <summary>
    /// Performs left shift operation on compile-time size-validated bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The bitvector expression to shift.</param>
    /// <param name="right">The bitvector expression specifying the shift amount.</param>
    /// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
    /// <returns>A type-safe Z3 bitvector expression representing the left shift.</returns>
    public static Z3BitVec<TSize> Shl<TSize>(
        this Z3Context context,
        Z3BitVec<TSize> left,
        Z3BitVec<TSize> right
    )
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvShl(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<Z3BitVec<TSize>>(context, handle);
    }

    /// <summary>
    /// Performs right shift operation on compile-time size-validated bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The bitvector expression to shift.</param>
    /// <param name="right">The bitvector expression specifying the shift amount.</param>
    /// <param name="signed">If true, performs arithmetic right shift; otherwise logical right shift.</param>
    /// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
    /// <returns>A type-safe Z3 bitvector expression representing the right shift.</returns>
    public static Z3BitVec<TSize> Shr<TSize>(
        this Z3Context context,
        Z3BitVec<TSize> left,
        Z3BitVec<TSize> right,
        bool signed = false
    )
        where TSize : ISize
    {
        var handle = signed
            ? SafeNativeMethods.Z3MkBvAShr(context.Handle, left.Handle, right.Handle)
            : SafeNativeMethods.Z3MkBvLShr(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<Z3BitVec<TSize>>(context, handle);
    }

    /// <summary>
    /// Creates a less-than comparison expression between two compile-time size-validated bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <param name="signed">If true, treats operands as signed; otherwise as unsigned.</param>
    /// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
    /// <returns>A Z3 boolean expression representing the less-than comparison.</returns>
    public static Z3BoolExpr Lt<TSize>(
        this Z3Context context,
        Z3BitVec<TSize> left,
        Z3BitVec<TSize> right,
        bool signed = false
    )
        where TSize : ISize
    {
        var handle = signed
            ? SafeNativeMethods.Z3MkBvSLt(context.Handle, left.Handle, right.Handle)
            : SafeNativeMethods.Z3MkBvULt(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<Z3BoolExpr>(context, handle);
    }

    /// <summary>
    /// Creates a less-than-or-equal comparison expression between two compile-time size-validated bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <param name="signed">If true, treats operands as signed; otherwise as unsigned.</param>
    /// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
    /// <returns>A Z3 boolean expression representing the less-than-or-equal comparison.</returns>
    public static Z3BoolExpr Le<TSize>(
        this Z3Context context,
        Z3BitVec<TSize> left,
        Z3BitVec<TSize> right,
        bool signed = false
    )
        where TSize : ISize
    {
        var handle = signed
            ? SafeNativeMethods.Z3MkBvSLe(context.Handle, left.Handle, right.Handle)
            : SafeNativeMethods.Z3MkBvULe(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<Z3BoolExpr>(context, handle);
    }

    /// <summary>
    /// Creates a greater-than comparison expression between two compile-time size-validated bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <param name="signed">If true, treats operands as signed; otherwise as unsigned.</param>
    /// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
    /// <returns>A Z3 boolean expression representing the greater-than comparison.</returns>
    public static Z3BoolExpr Gt<TSize>(
        this Z3Context context,
        Z3BitVec<TSize> left,
        Z3BitVec<TSize> right,
        bool signed = false
    )
        where TSize : ISize
    {
        var handle = signed
            ? SafeNativeMethods.Z3MkBvSGt(context.Handle, left.Handle, right.Handle)
            : SafeNativeMethods.Z3MkBvUGt(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<Z3BoolExpr>(context, handle);
    }

    /// <summary>
    /// Creates a greater-than-or-equal comparison expression between two compile-time size-validated bitvector expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left bitvector expression.</param>
    /// <param name="right">The right bitvector expression.</param>
    /// <param name="signed">If true, treats operands as signed; otherwise as unsigned.</param>
    /// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
    /// <returns>A Z3 boolean expression representing the greater-than-or-equal comparison.</returns>
    public static Z3BoolExpr Ge<TSize>(
        this Z3Context context,
        Z3BitVec<TSize> left,
        Z3BitVec<TSize> right,
        bool signed = false
    )
        where TSize : ISize
    {
        var handle = signed
            ? SafeNativeMethods.Z3MkBvSGe(context.Handle, left.Handle, right.Handle)
            : SafeNativeMethods.Z3MkBvUGe(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<Z3BoolExpr>(context, handle);
    }
}
