using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.Extensions;

public static partial class Z3ContextExtensions
{
    /// <summary>
    /// Converts an integer expression to a real expression using Z3's integer-to-real conversion.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="expr">The integer expression to convert.</param>
    /// <returns>A real expression representing the same value as the input integer expression.</returns>
    public static Z3RealExpr ToReal(this Z3Context context, Z3IntExpr expr)
    {
        var handle = NativeMethods.Z3MkInt2Real(context.Handle, expr.Handle);
        return Z3RealExpr.Create(context, handle);
    }

    /// <summary>
    /// Converts an integer expression to a bitvector expression with the specified size.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="expr">The integer expression to convert.</param>
    /// <param name="size">The size in bits for the resulting bitvector.</param>
    /// <returns>A bitvector expression representing the integer value with the specified bit width.</returns>
    public static Z3BitVecExpr ToBitVec(this Z3Context context, Z3IntExpr expr, uint size)
    {
        var handle = NativeMethods.Z3MkInt2Bv(context.Handle, size, expr.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }
}
