using Z3Wrap.DataTypes;
using Z3Wrap.Expressions;
using Z3Wrap.Interop;

namespace Z3Wrap;

public static partial class Z3ContextExtensions
{
    // Bitvector constant creation
    public static Z3BitVecExpr BitVecConst(this Z3Context context, string name, uint size)
    {
        var sort = NativeMethods.Z3MkBvSort(context.Handle, size);
        using var namePtr = new AnsiStringPtr(name);
        var symbol = NativeMethods.Z3MkStringSymbol(context.Handle, namePtr);
        var handle = NativeMethods.Z3MkConst(context.Handle, symbol, sort);

        return Z3BitVecExpr.Create(context, handle);
    }

    // Bitvector value creation from BitVec
    public static Z3BitVecExpr BitVec(this Z3Context context, BitVec value)
    {
        using var numeralPtr = new AnsiStringPtr(value.ToString());
        var sort = NativeMethods.Z3MkBvSort(context.Handle, value.Size);
        var handle = NativeMethods.Z3MkNumeral(context.Handle, numeralPtr, sort);

        return Z3BitVecExpr.Create(context, handle);
    }

    // Bitvector extension operations (adding bits)
    public static Z3BitVecExpr Extend(this Z3Context context, Z3BitVecExpr expr, uint additionalBits)
    {
        var handle = NativeMethods.Z3MkZeroExt(context.Handle, additionalBits, expr.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    public static Z3BitVecExpr SignedExtend(this Z3Context context, Z3BitVecExpr expr, uint additionalBits)
    {
        var handle = NativeMethods.Z3MkSignExt(context.Handle, additionalBits, expr.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    // Bitvector resizing operations (to specific size)
    public static Z3BitVecExpr Resize(this Z3Context context, Z3BitVecExpr expr, uint newSize)
    {
        if (newSize == expr.Size)
            return expr;

        if (newSize > expr.Size)
        {
            var additionalBits = newSize - expr.Size;
            return context.Extend(expr, additionalBits);
        }

        // Truncate by extracting lower bits
        return context.Extract(expr, newSize - 1, 0);
    }

    public static Z3BitVecExpr SignedResize(this Z3Context context, Z3BitVecExpr expr, uint newSize)
    {
        if (newSize == expr.Size)
            return expr;

        if (newSize > expr.Size)
        {
            var additionalBits = newSize - expr.Size;
            return context.SignedExtend(expr, additionalBits);
        }

        // Truncate by extracting lower bits
        return context.Extract(expr, newSize - 1, 0);
    }


    public static Z3BitVecExpr Extract(this Z3Context context, Z3BitVecExpr expr, uint high, uint low)
    {
        var handle = NativeMethods.Z3MkExtract(context.Handle, high, low, expr.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    // Integer to Bitvector conversion
    public static Z3BitVecExpr ToBitVec(this Z3Context context, Z3IntExpr expr, uint size)
    {
        var handle = NativeMethods.Z3MkInt2bv(context.Handle, size, expr.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    // Bitvector to Integer conversion
    public static Z3IntExpr ToInt(this Z3Context context, Z3BitVecExpr expr)
    {
        var handle = NativeMethods.Z3MkBv2int(context.Handle, expr.Handle, false);
        return Z3IntExpr.Create(context, handle);
    }

    public static Z3IntExpr ToSignedInt(this Z3Context context, Z3BitVecExpr expr)
    {
        var handle = NativeMethods.Z3MkBv2int(context.Handle, expr.Handle, true);
        return Z3IntExpr.Create(context, handle);
    }
}