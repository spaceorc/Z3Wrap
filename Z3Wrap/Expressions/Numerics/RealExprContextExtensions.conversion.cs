using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.Expressions.Numerics;

public static partial class RealExprContextExtensions
{
    /// <summary>
    /// Converts a real expression to an integer expression by truncating towards zero.
    /// This operation removes the fractional part of the real number.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="expr">The real expression to convert.</param>
    /// <returns>A Z3IntExpr representing the truncated integer value of the real expression.</returns>
    public static IntExpr ToInt(this Z3Context context, RealExpr expr)
    {
        var handle = SafeNativeMethods.Z3MkReal2Int(context.Handle, expr.Handle);
        return Z3Expr.Create<IntExpr>(context, handle);
    }
}
