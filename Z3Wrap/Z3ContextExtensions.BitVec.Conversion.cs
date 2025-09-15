using Z3Wrap.Expressions;
using Z3Wrap.Interop;

namespace Z3Wrap;

public static partial class Z3ContextExtensions
{
    // Bitvector to Integer conversion
    public static Z3IntExpr ToInt(this Z3Context context, Z3BitVecExpr expr)
    {
        var handle = NativeMethods.Z3MkBv2Int(context.Handle, expr.Handle, false);
        return Z3IntExpr.Create(context, handle);
    }

    public static Z3IntExpr ToSignedInt(this Z3Context context, Z3BitVecExpr expr)
    {
        var handle = NativeMethods.Z3MkBv2Int(context.Handle, expr.Handle, true);
        return Z3IntExpr.Create(context, handle);
    }
}