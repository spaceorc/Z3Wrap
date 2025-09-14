using Z3Wrap.Expressions;
using Z3Wrap.Interop;

namespace Z3Wrap;

public static partial class Z3ContextExtensions
{
    // Real to Integer conversion (truncates)
    public static Z3IntExpr ToInt(this Z3Context context, Z3RealExpr expr)
    {
        var handle = NativeMethods.Z3MkReal2Int(context.Handle, expr.Handle);
        return Z3IntExpr.Create(context, handle);
    }
}