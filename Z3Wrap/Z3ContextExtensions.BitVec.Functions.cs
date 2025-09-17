using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap;

public static partial class Z3ContextExtensions
{
    // Bitvector extension operations (adding bits)
    public static Z3BitVecExpr Extend(this Z3Context context, Z3BitVecExpr expr, uint additionalBits, bool signed = false)
    {
        var handle = signed
            ? NativeMethods.Z3MkSignExt(context.Handle, additionalBits, expr.Handle)
            : NativeMethods.Z3MkZeroExt(context.Handle, additionalBits, expr.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    // Bitvector resizing operations (to specific size)
    public static Z3BitVecExpr Resize(this Z3Context context, Z3BitVecExpr expr, uint newSize, bool signed = false)
    {
        if (newSize == expr.Size)
            return expr;

        if (newSize > expr.Size)
        {
            var additionalBits = newSize - expr.Size;
            return context.Extend(expr, additionalBits, signed);
        }

        // Truncate by extracting lower bits
        return context.Extract(expr, newSize - 1, 0);
    }

    public static Z3BitVecExpr Extract(this Z3Context context, Z3BitVecExpr expr, uint high, uint low)
    {
        var handle = NativeMethods.Z3MkExtract(context.Handle, high, low, expr.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    public static Z3BitVecExpr Repeat(this Z3Context context, Z3BitVecExpr expr, uint count)
    {
        var handle = NativeMethods.Z3MkRepeat(context.Handle, count, expr.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }
}