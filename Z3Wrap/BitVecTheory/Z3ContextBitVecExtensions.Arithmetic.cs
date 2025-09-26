using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Interop;
using Spaceorc.Z3Wrap.Values;

namespace Spaceorc.Z3Wrap.BitVecTheory;

public static partial class Z3ContextBitVecExtensions
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
}
