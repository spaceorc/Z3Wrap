using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Numerics;
using Spaceorc.Z3Wrap.Interop;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Spaceorc.Z3Wrap.Expressions.BitVectors;

public static partial class BvExprContextExtensions
{
    /// <summary>
    /// Converts a compile-time size-validated bitvector expression to an integer expression.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="expr">The type-safe bitvector expression to convert.</param>
    /// <param name="signed">If true, treats the bitvector as signed; otherwise as unsigned.</param>
    /// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
    /// <returns>A Z3 integer expression representing the converted value.</returns>
    public static IntExpr ToInt<TSize>(
        this Z3Context context,
        BvExpr<TSize> expr,
        bool signed = false
    )
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBv2Int(context.Handle, expr.Handle, signed);
        return Z3Expr.Create<IntExpr>(context, handle);
    }
}
