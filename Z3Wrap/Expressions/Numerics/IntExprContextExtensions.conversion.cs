using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.BitVectors;
using Spaceorc.Z3Wrap.Interop;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Spaceorc.Z3Wrap.Expressions.Numerics;

public static partial class IntExprContextExtensions
{
    /// <summary>
    /// Converts an integer expression to a real expression using Z3's integer-to-real conversion.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="expr">The integer expression to convert.</param>
    /// <returns>A real expression representing the same value as the input integer expression.</returns>
    public static RealExpr ToReal(this Z3Context context, IntExpr expr)
    {
        var handle = SafeNativeMethods.Z3MkInt2Real(context.Handle, expr.Handle);
        return Z3Expr.Create<RealExpr>(context, handle);
    }

    /// <summary>
    /// Converts an integer expression to a bitvector expression with the specified size.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="expr">The integer expression to convert.</param>
    /// <typeparam name="TSize">The size type that determines the bit width of the resulting bitvector.</typeparam>
    /// <returns>A bitvector expression representing the integer value with the specified bit width.</returns>
    public static BvExpr<TSize> ToBitVec<TSize>(this Z3Context context, IntExpr expr)
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkInt2Bv(context.Handle, TSize.Size, expr.Handle);
        return Z3Expr.Create<BvExpr<TSize>>(context, handle);
    }
}
