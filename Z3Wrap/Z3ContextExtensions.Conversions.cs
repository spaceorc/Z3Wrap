using Z3Wrap.Expressions;
using Z3Wrap.Interop;

namespace Z3Wrap;

public static partial class Z3ContextExtensions
{
    // Type conversions between expression types

    // Integer to Real conversion
    public static Z3RealExpr ToReal(this Z3Context context, Z3IntExpr expr)
    {
        var handle = NativeMethods.Z3MkInt2Real(context.Handle, expr.Handle);
        return Z3RealExpr.Create(context, handle);
    }

    // Real to Integer conversion (truncates)
    public static Z3IntExpr ToInt(this Z3Context context, Z3RealExpr expr)
    {
        var handle = NativeMethods.Z3MkReal2Int(context.Handle, expr.Handle);
        return Z3IntExpr.Create(context, handle);
    }

    // Note: BitVector conversion methods (ToBitVec, ToInt, ToSignedInt)
    // are defined in Z3ContextExtensions.BitVec.cs to keep all bitvector
    // operations together
}