using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Core.Interop;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Spaceorc.Z3Wrap.Expressions.BitVectors;

/// <summary>
/// Provides bit-vector arithmetic and bitwise operation methods for Z3Context.
/// </summary>
public static class BvOperationsContextExtensions
{
    /// <summary>
    /// Creates bit-vector addition expression.
    /// </summary>
    /// <typeparam name="TSize">Bit-vector size type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Bit-vector expression representing left + right.</returns>
    public static BvExpr<TSize> Add<TSize>(this Z3Context context, BvExpr<TSize> left, BvExpr<TSize> right)
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvAdd(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BvExpr<TSize>>(context, handle);
    }

    /// <summary>
    /// Creates bit-vector subtraction expression.
    /// </summary>
    /// <typeparam name="TSize">Bit-vector size type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Bit-vector expression representing left - right.</returns>
    public static BvExpr<TSize> Sub<TSize>(this Z3Context context, BvExpr<TSize> left, BvExpr<TSize> right)
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvSub(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BvExpr<TSize>>(context, handle);
    }

    /// <summary>
    /// Creates bit-vector multiplication expression.
    /// </summary>
    /// <typeparam name="TSize">Bit-vector size type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Bit-vector expression representing left * right.</returns>
    public static BvExpr<TSize> Mul<TSize>(this Z3Context context, BvExpr<TSize> left, BvExpr<TSize> right)
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvMul(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BvExpr<TSize>>(context, handle);
    }

    /// <summary>
    /// Creates bit-vector division expression.
    /// </summary>
    /// <typeparam name="TSize">Bit-vector size type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <param name="signed">True for signed division, false for unsigned.</param>
    /// <returns>Bit-vector expression representing left / right.</returns>
    public static BvExpr<TSize> Div<TSize>(
        this Z3Context context,
        BvExpr<TSize> left,
        BvExpr<TSize> right,
        bool signed = false
    )
        where TSize : ISize
    {
        var handle = signed
            ? SafeNativeMethods.Z3MkBvSDiv(context.Handle, left.Handle, right.Handle)
            : SafeNativeMethods.Z3MkBvUDiv(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BvExpr<TSize>>(context, handle);
    }

    /// <summary>
    /// Creates bit-vector remainder expression.
    /// </summary>
    /// <typeparam name="TSize">Bit-vector size type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <param name="signed">True for signed remainder, false for unsigned.</param>
    /// <returns>Bit-vector expression representing left % right.</returns>
    public static BvExpr<TSize> Rem<TSize>(
        this Z3Context context,
        BvExpr<TSize> left,
        BvExpr<TSize> right,
        bool signed = false
    )
        where TSize : ISize
    {
        var handle = signed
            ? SafeNativeMethods.Z3MkBvSRem(context.Handle, left.Handle, right.Handle)
            : SafeNativeMethods.Z3MkBvURem(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BvExpr<TSize>>(context, handle);
    }

    /// <summary>
    /// Creates signed bit-vector modulo expression.
    /// </summary>
    /// <typeparam name="TSize">Bit-vector size type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Bit-vector expression representing signed modulo operation.</returns>
    public static BvExpr<TSize> SignedMod<TSize>(this Z3Context context, BvExpr<TSize> left, BvExpr<TSize> right)
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvSMod(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BvExpr<TSize>>(context, handle);
    }

    /// <summary>
    /// Creates bit-vector negation expression.
    /// </summary>
    /// <typeparam name="TSize">Bit-vector size type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="expr">The operand to negate.</param>
    /// <returns>Bit-vector expression representing -expr.</returns>
    public static BvExpr<TSize> Neg<TSize>(this Z3Context context, BvExpr<TSize> expr)
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvNeg(context.Handle, expr.Handle);
        return Z3Expr.Create<BvExpr<TSize>>(context, handle);
    }

    /// <summary>
    /// Creates bit-vector bitwise AND expression.
    /// </summary>
    /// <typeparam name="TSize">Bit-vector size type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Bit-vector expression representing left &amp; right.</returns>
    public static BvExpr<TSize> And<TSize>(this Z3Context context, BvExpr<TSize> left, BvExpr<TSize> right)
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvAnd(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BvExpr<TSize>>(context, handle);
    }

    /// <summary>
    /// Creates bit-vector bitwise OR expression.
    /// </summary>
    /// <typeparam name="TSize">Bit-vector size type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Bit-vector expression representing left | right.</returns>
    public static BvExpr<TSize> Or<TSize>(this Z3Context context, BvExpr<TSize> left, BvExpr<TSize> right)
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvOr(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BvExpr<TSize>>(context, handle);
    }

    /// <summary>
    /// Creates bit-vector bitwise XOR expression.
    /// </summary>
    /// <typeparam name="TSize">Bit-vector size type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Bit-vector expression representing left ^ right.</returns>
    public static BvExpr<TSize> Xor<TSize>(this Z3Context context, BvExpr<TSize> left, BvExpr<TSize> right)
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvXor(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BvExpr<TSize>>(context, handle);
    }

    /// <summary>
    /// Creates bit-vector bitwise NOT expression.
    /// </summary>
    /// <typeparam name="TSize">Bit-vector size type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="expr">The operand to negate bitwise.</param>
    /// <returns>Bit-vector expression representing ~expr.</returns>
    public static BvExpr<TSize> Not<TSize>(this Z3Context context, BvExpr<TSize> expr)
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvNot(context.Handle, expr.Handle);
        return Z3Expr.Create<BvExpr<TSize>>(context, handle);
    }

    /// <summary>
    /// Creates bit-vector left shift expression.
    /// </summary>
    /// <typeparam name="TSize">Bit-vector size type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The value to shift.</param>
    /// <param name="right">The shift amount.</param>
    /// <returns>Bit-vector expression representing left &lt;&lt; right.</returns>
    public static BvExpr<TSize> Shl<TSize>(this Z3Context context, BvExpr<TSize> left, BvExpr<TSize> right)
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvShl(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BvExpr<TSize>>(context, handle);
    }

    /// <summary>
    /// Creates bit-vector right shift expression.
    /// </summary>
    /// <typeparam name="TSize">Bit-vector size type.</typeparam>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The value to shift.</param>
    /// <param name="right">The shift amount.</param>
    /// <param name="signed">True for arithmetic shift, false for logical shift.</param>
    /// <returns>Bit-vector expression representing right shift.</returns>
    public static BvExpr<TSize> Shr<TSize>(
        this Z3Context context,
        BvExpr<TSize> left,
        BvExpr<TSize> right,
        bool signed = false
    )
        where TSize : ISize
    {
        var handle = signed
            ? SafeNativeMethods.Z3MkBvAShr(context.Handle, left.Handle, right.Handle)
            : SafeNativeMethods.Z3MkBvLShr(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BvExpr<TSize>>(context, handle);
    }
}
