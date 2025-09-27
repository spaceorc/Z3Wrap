using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Core.Interop;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Spaceorc.Z3Wrap.Expressions.BitVectors;

public static class BvComparisonContextExtensions
{
    public static BoolExpr Lt<TSize>(
        this Z3Context context,
        BvExpr<TSize> left,
        BvExpr<TSize> right,
        bool signed = false
    )
        where TSize : ISize
    {
        var handle = signed
            ? SafeNativeMethods.Z3MkBvSLt(context.Handle, left.Handle, right.Handle)
            : SafeNativeMethods.Z3MkBvULt(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }

    public static BoolExpr Le<TSize>(
        this Z3Context context,
        BvExpr<TSize> left,
        BvExpr<TSize> right,
        bool signed = false
    )
        where TSize : ISize
    {
        var handle = signed
            ? SafeNativeMethods.Z3MkBvSLe(context.Handle, left.Handle, right.Handle)
            : SafeNativeMethods.Z3MkBvULe(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }

    public static BoolExpr Gt<TSize>(
        this Z3Context context,
        BvExpr<TSize> left,
        BvExpr<TSize> right,
        bool signed = false
    )
        where TSize : ISize
    {
        var handle = signed
            ? SafeNativeMethods.Z3MkBvSGt(context.Handle, left.Handle, right.Handle)
            : SafeNativeMethods.Z3MkBvUGt(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }

    public static BoolExpr Ge<TSize>(
        this Z3Context context,
        BvExpr<TSize> left,
        BvExpr<TSize> right,
        bool signed = false
    )
        where TSize : ISize
    {
        var handle = signed
            ? SafeNativeMethods.Z3MkBvSGe(context.Handle, left.Handle, right.Handle)
            : SafeNativeMethods.Z3MkBvUGe(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }
}
