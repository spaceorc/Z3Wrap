using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Numerics;
using Spaceorc.Z3Wrap.Interop;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Spaceorc.Z3Wrap.Expressions.BitVectors;

public static class BvCoreContextExtensions
{
    public static BvExpr<TSize> BitVecConst<TSize>(this Z3Context context, string name)
        where TSize : ISize
    {
        var sort = SafeNativeMethods.Z3MkBvSort(context.Handle, TSize.Size);
        using var namePtr = new AnsiStringPtr(name);
        var symbol = SafeNativeMethods.Z3MkStringSymbol(context.Handle, namePtr);
        var handle = SafeNativeMethods.Z3MkConst(context.Handle, symbol, sort);

        return Z3Expr.Create<BvExpr<TSize>>(context, handle);
    }

    public static BvExpr<TSize> BitVec<TSize>(this Z3Context context, Bv<TSize> value)
        where TSize : ISize
    {
        using var numeralPtr = new AnsiStringPtr(value.ToString());
        var sort = SafeNativeMethods.Z3MkBvSort(context.Handle, TSize.Size);
        var handle = SafeNativeMethods.Z3MkNumeral(context.Handle, numeralPtr, sort);

        return Z3Expr.Create<BvExpr<TSize>>(context, handle);
    }

    public static IntExpr ToInt<TSize>(this Z3Context context, BvExpr<TSize> expr, bool signed = false)
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBv2Int(context.Handle, expr.Handle, signed);
        return Z3Expr.Create<IntExpr>(context, handle);
    }

    public static BvExpr<TOutputSize> Resize<TInputSize, TOutputSize>(
        this Z3Context context,
        BvExpr<TInputSize> expr,
        bool signed = false
    )
        where TInputSize : ISize
        where TOutputSize : ISize
    {
        if (TOutputSize.Size == TInputSize.Size)
            return Z3Expr.Create<BvExpr<TOutputSize>>(context, expr.Handle);

        if (TOutputSize.Size > TInputSize.Size)
        {
            var additionalBits = TOutputSize.Size - TInputSize.Size;
            var handle = signed
                ? SafeNativeMethods.Z3MkSignExt(context.Handle, additionalBits, expr.Handle)
                : SafeNativeMethods.Z3MkZeroExt(context.Handle, additionalBits, expr.Handle);
            return Z3Expr.Create<BvExpr<TOutputSize>>(context, handle);
        }

        // Truncate by extracting lower bits
        return context.Extract<TInputSize, TOutputSize>(expr, 0);
    }

    public static BvExpr<TOutputSize> Extract<TInputSize, TOutputSize>(
        this Z3Context context,
        BvExpr<TInputSize> expr,
        uint startBit
    )
        where TInputSize : ISize
        where TOutputSize : ISize
    {
        if (startBit + TOutputSize.Size > TInputSize.Size)
            throw new ArgumentException(
                $"Extraction would exceed input bounds: startBit({startBit}) + outputSize({TOutputSize.Size}) > inputSize({TInputSize.Size})"
            );

        var high = startBit + TOutputSize.Size - 1;
        var handle = SafeNativeMethods.Z3MkExtract(context.Handle, high, startBit, expr.Handle);
        return Z3Expr.Create<BvExpr<TOutputSize>>(context, handle);
    }

    public static BvExpr<TOutputSize> Repeat<TInputSize, TOutputSize>(this Z3Context context, BvExpr<TInputSize> expr)
        where TInputSize : ISize
        where TOutputSize : ISize
    {
        if (TOutputSize.Size % TInputSize.Size != 0)
            throw new ArgumentException(
                $"Target size {TOutputSize.Size} must be a multiple of source size {TInputSize.Size}"
            );

        var count = TOutputSize.Size / TInputSize.Size;
        var handle = SafeNativeMethods.Z3MkRepeat(context.Handle, count, expr.Handle);
        return Z3Expr.Create<BvExpr<TOutputSize>>(context, handle);
    }
}
