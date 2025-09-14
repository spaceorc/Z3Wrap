using Z3Wrap.Expressions;
using Z3Wrap.Interop;

namespace Z3Wrap;

public static partial class Z3ContextExtensions
{
    // Integer to Real conversion
    public static Z3RealExpr ToReal(this Z3Context context, Z3IntExpr expr)
    {
        var handle = NativeMethods.Z3MkInt2Real(context.Handle, expr.Handle);
        return Z3RealExpr.Create(context, handle);
    }
}