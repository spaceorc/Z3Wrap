using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Core.Interop;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Spaceorc.Z3Wrap.Expressions.BitVectors;

public static class BvOverflowChecksContextExtensions
{
    public static BoolExpr AddNoOverflow<TSize>(
        this Z3Context context,
        BvExpr<TSize> left,
        BvExpr<TSize> right,
        bool signed = false
    )
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvAddNoOverflow(context.Handle, left.Handle, right.Handle, signed);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }

    public static BoolExpr SignedSubNoOverflow<TSize>(this Z3Context context, BvExpr<TSize> left, BvExpr<TSize> right)
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvSubNoOverflow(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }

    public static BoolExpr SubNoUnderflow<TSize>(
        this Z3Context context,
        BvExpr<TSize> left,
        BvExpr<TSize> right,
        bool signed = true
    )
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvSubNoUnderflow(context.Handle, left.Handle, right.Handle, signed);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }

    public static BoolExpr MulNoOverflow<TSize>(
        this Z3Context context,
        BvExpr<TSize> left,
        BvExpr<TSize> right,
        bool signed = false
    )
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvMulNoOverflow(context.Handle, left.Handle, right.Handle, signed);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }

    public static BoolExpr SignedMulNoUnderflow<TSize>(this Z3Context context, BvExpr<TSize> left, BvExpr<TSize> right)
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvMulNoUnderflow(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }

    public static BoolExpr SignedAddNoUnderflow<TSize>(this Z3Context context, BvExpr<TSize> left, BvExpr<TSize> right)
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvAddNoUnderflow(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }

    public static BoolExpr SignedDivNoOverflow<TSize>(this Z3Context context, BvExpr<TSize> left, BvExpr<TSize> right)
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvSDivNoOverflow(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }

    public static BoolExpr SignedNegNoOverflow<TSize>(this Z3Context context, BvExpr<TSize> operand)
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBvNegNoOverflow(context.Handle, operand.Handle);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }
}
