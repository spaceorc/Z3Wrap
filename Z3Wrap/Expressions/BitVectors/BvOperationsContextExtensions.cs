using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Core.Interop;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Spaceorc.Z3Wrap.Expressions.BitVectors;

public static class BvOperationsContextExtensions
{
    public static BvExpr<TSize> Add<TSize>(this Z3Context context, BvExpr<TSize> left, BvExpr<TSize> right)
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvAdd(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BvExpr<TSize>>(context, handle);
    }

    public static BvExpr<TSize> Sub<TSize>(this Z3Context context, BvExpr<TSize> left, BvExpr<TSize> right)
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvSub(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BvExpr<TSize>>(context, handle);
    }

    public static BvExpr<TSize> Mul<TSize>(this Z3Context context, BvExpr<TSize> left, BvExpr<TSize> right)
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvMul(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BvExpr<TSize>>(context, handle);
    }

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

    public static BvExpr<TSize> SignedMod<TSize>(this Z3Context context, BvExpr<TSize> left, BvExpr<TSize> right)
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvSMod(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BvExpr<TSize>>(context, handle);
    }

    public static BvExpr<TSize> Neg<TSize>(this Z3Context context, BvExpr<TSize> expr)
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvNeg(context.Handle, expr.Handle);
        return Z3Expr.Create<BvExpr<TSize>>(context, handle);
    }

    public static BvExpr<TSize> And<TSize>(this Z3Context context, BvExpr<TSize> left, BvExpr<TSize> right)
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvAnd(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BvExpr<TSize>>(context, handle);
    }

    public static BvExpr<TSize> Or<TSize>(this Z3Context context, BvExpr<TSize> left, BvExpr<TSize> right)
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvOr(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BvExpr<TSize>>(context, handle);
    }

    public static BvExpr<TSize> Xor<TSize>(this Z3Context context, BvExpr<TSize> left, BvExpr<TSize> right)
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvXor(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BvExpr<TSize>>(context, handle);
    }

    public static BvExpr<TSize> Not<TSize>(this Z3Context context, BvExpr<TSize> expr)
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvNot(context.Handle, expr.Handle);
        return Z3Expr.Create<BvExpr<TSize>>(context, handle);
    }

    public static BvExpr<TSize> Shl<TSize>(this Z3Context context, BvExpr<TSize> left, BvExpr<TSize> right)
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvShl(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BvExpr<TSize>>(context, handle);
    }

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
