using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.Extensions;

public static partial class Z3ContextExtensions
{
    // Bitvector to Integer conversion
    public static Z3IntExpr ToInt(this Z3Context context, Z3BitVecExpr expr, bool signed = false)
    {
        var handle = NativeMethods.Z3MkBv2Int(context.Handle, expr.Handle, signed);
        return Z3IntExpr.Create(context, handle);
    }
}