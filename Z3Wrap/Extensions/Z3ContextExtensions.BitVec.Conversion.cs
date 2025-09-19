using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.Extensions;

public static partial class Z3ContextExtensions
{
    /// <summary>
    /// Converts a bitvector expression to an integer expression.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="expr">The bitvector expression to convert.</param>
    /// <param name="signed">If true, treats the bitvector as signed; otherwise as unsigned.</param>
    /// <returns>A Z3 integer expression representing the converted value.</returns>
    public static Z3IntExpr ToInt(this Z3Context context, Z3BitVecExpr expr, bool signed = false)
    {
        var handle = NativeMethods.Z3MkBv2Int(context.Handle, expr.Handle, signed);
        return Z3IntExpr.Create(context, handle);
    }
}
