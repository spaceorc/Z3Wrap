using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.BitVectors;
using Spaceorc.Z3Wrap.Interop;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Spaceorc.Z3Wrap.Expressions.Numerics;

public static class IntContextExtensions
{
    public static IntExpr Int(this Z3Context context, BigInteger value)
    {
        using var valueStr = new AnsiStringPtr(value.ToString());
        var intSort = SafeNativeMethods.Z3MkIntSort(context.Handle);
        var handle = SafeNativeMethods.Z3MkNumeral(context.Handle, valueStr, intSort);
        return Z3Expr.Create<IntExpr>(context, handle);
    }

    public static IntExpr Int(this Z3Context context, int value) => context.Int(new BigInteger(value));

    public static IntExpr Int(this Z3Context context, long value) => context.Int(new BigInteger(value));

    public static IntExpr IntConst(this Z3Context context, string name)
    {
        using var namePtr = new AnsiStringPtr(name);
        var symbol = SafeNativeMethods.Z3MkStringSymbol(context.Handle, namePtr);
        var intSort = SafeNativeMethods.Z3MkIntSort(context.Handle);
        var handle = SafeNativeMethods.Z3MkConst(context.Handle, symbol, intSort);
        return Z3Expr.Create<IntExpr>(context, handle);
    }

    public static RealExpr ToReal(this Z3Context context, IntExpr expr)
    {
        var handle = SafeNativeMethods.Z3MkInt2Real(context.Handle, expr.Handle);
        return Z3Expr.Create<RealExpr>(context, handle);
    }

    public static BvExpr<TSize> ToBitVec<TSize>(this Z3Context context, IntExpr expr)
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkInt2Bv(context.Handle, TSize.Size, expr.Handle);
        return Z3Expr.Create<BvExpr<TSize>>(context, handle);
    }
}
