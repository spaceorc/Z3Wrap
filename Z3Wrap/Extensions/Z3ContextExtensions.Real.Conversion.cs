using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.Extensions;

public static partial class Z3ContextExtensions
{
    // Real to Integer conversion (truncates)
    public static Z3IntExpr ToInt(this Z3Context context, Z3RealExpr expr)
    {
        var handle = NativeMethods.Z3MkReal2Int(context.Handle, expr.Handle);
        return Z3IntExpr.Create(context, handle);
    }
}