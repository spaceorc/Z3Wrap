using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.BitVectors;

public static partial class Z3ContextBitVecExtensions
{
    /// <summary>
    /// Converts a compile-time size-validated bitvector expression to an integer expression.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="expr">The type-safe bitvector expression to convert.</param>
    /// <param name="signed">If true, treats the bitvector as signed; otherwise as unsigned.</param>
    /// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
    /// <returns>A Z3 integer expression representing the converted value.</returns>
    public static Z3IntExpr ToInt<TSize>(
        this Z3Context context,
        Z3BitVec<TSize> expr,
        bool signed = false
    )
        where TSize : ISize
    {
        var handle = SafeNativeMethods.Z3MkBv2Int(context.Handle, expr.Handle, signed);
        return Z3Expr.Create<Z3IntExpr>(context, handle);
    }
}
