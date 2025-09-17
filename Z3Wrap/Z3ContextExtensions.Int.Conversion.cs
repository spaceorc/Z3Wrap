using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap;

public static partial class Z3ContextExtensions
{
    // Integer to Real conversion
    public static Z3RealExpr ToReal(this Z3Context context, Z3IntExpr expr)
    {
        var handle = NativeMethods.Z3MkInt2Real(context.Handle, expr.Handle);
        return Z3RealExpr.Create(context, handle);
    }

    // Integer to Bitvector conversion
    public static Z3BitVecExpr ToBitVec(this Z3Context context, Z3IntExpr expr, uint size)
    {
        var handle = NativeMethods.Z3MkInt2Bv(context.Handle, size, expr.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }
}