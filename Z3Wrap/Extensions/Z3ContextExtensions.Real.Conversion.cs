using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.Extensions;

public static partial class Z3ContextExtensions
{
    /// <summary>
    /// Converts a real expression to an integer expression by truncating towards zero.
    /// This operation removes the fractional part of the real number.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="expr">The real expression to convert.</param>
    /// <returns>A Z3IntExpr representing the truncated integer value of the real expression.</returns>
    public static Z3IntExpr ToInt(this Z3Context context, Z3RealExpr expr)
    {
        var handle = SafeNativeMethods.Z3MkReal2Int(context.Handle, expr.Handle);
        return Z3IntExpr.Create(context, handle);
    }
}
