using Z3Wrap.DataTypes;
using Z3Wrap.Expressions;
using Z3Wrap.Interop;
using System.Numerics;

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

    // Bitvector value creation from BigInteger and size
    public static Z3BitVecExpr BitVec(this Z3Context context, BigInteger value, uint size)
    {
        return context.BitVec(new BitVec(value, size));
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

    // Arithmetic operations
    public static Z3BitVecExpr Add(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvadd(context.Handle, left.Handle, right.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    public static Z3BitVecExpr Sub(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvsub(context.Handle, left.Handle, right.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    public static Z3BitVecExpr Mul(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvmul(context.Handle, left.Handle, right.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    public static Z3BitVecExpr UDiv(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvudiv(context.Handle, left.Handle, right.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    public static Z3BitVecExpr SDiv(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvsdiv(context.Handle, left.Handle, right.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    public static Z3BitVecExpr URem(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvurem(context.Handle, left.Handle, right.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    public static Z3BitVecExpr SRem(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvsrem(context.Handle, left.Handle, right.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    public static Z3BitVecExpr SMod(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvsmod(context.Handle, left.Handle, right.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    public static Z3BitVecExpr Neg(this Z3Context context, Z3BitVecExpr expr)
    {
        var handle = NativeMethods.Z3MkBvneg(context.Handle, expr.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    // Bitwise operations
    public static Z3BitVecExpr And(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvand(context.Handle, left.Handle, right.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    public static Z3BitVecExpr Or(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvor(context.Handle, left.Handle, right.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    public static Z3BitVecExpr Xor(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvxor(context.Handle, left.Handle, right.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    public static Z3BitVecExpr Not(this Z3Context context, Z3BitVecExpr expr)
    {
        var handle = NativeMethods.Z3MkBvnot(context.Handle, expr.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    // Shift operations
    public static Z3BitVecExpr Shl(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvshl(context.Handle, left.Handle, right.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    public static Z3BitVecExpr Lshr(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvlshr(context.Handle, left.Handle, right.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    public static Z3BitVecExpr Ashr(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvashr(context.Handle, left.Handle, right.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    // Comparison operations (return Z3BoolExpr)
    public static Z3BoolExpr Ult(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvult(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, handle);
    }

    public static Z3BoolExpr Slt(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvslt(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, handle);
    }

    public static Z3BoolExpr Ule(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvule(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, handle);
    }

    public static Z3BoolExpr Sle(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvsle(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, handle);
    }

    public static Z3BoolExpr Ugt(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvugt(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, handle);
    }

    public static Z3BoolExpr Sgt(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvsgt(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, handle);
    }

    public static Z3BoolExpr Uge(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvuge(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, handle);
    }

    public static Z3BoolExpr Sge(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvsge(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, handle);
    }

    // BigInteger arithmetic operations (Z3BitVecExpr op BigInteger)
    public static Z3BitVecExpr Add(this Z3Context context, Z3BitVecExpr left, BigInteger right)
    {
        var rightBitVec = context.BitVec(right, left.Size);
        return context.Add(left, rightBitVec);
    }

    public static Z3BitVecExpr Sub(this Z3Context context, Z3BitVecExpr left, BigInteger right)
    {
        var rightBitVec = context.BitVec(right, left.Size);
        return context.Sub(left, rightBitVec);
    }

    public static Z3BitVecExpr Mul(this Z3Context context, Z3BitVecExpr left, BigInteger right)
    {
        var rightBitVec = context.BitVec(right, left.Size);
        return context.Mul(left, rightBitVec);
    }

    public static Z3BitVecExpr UDiv(this Z3Context context, Z3BitVecExpr left, BigInteger right)
    {
        var rightBitVec = context.BitVec(right, left.Size);
        return context.UDiv(left, rightBitVec);
    }

    public static Z3BitVecExpr SDiv(this Z3Context context, Z3BitVecExpr left, BigInteger right)
    {
        var rightBitVec = context.BitVec(right, left.Size);
        return context.SDiv(left, rightBitVec);
    }

    public static Z3BitVecExpr URem(this Z3Context context, Z3BitVecExpr left, BigInteger right)
    {
        var rightBitVec = context.BitVec(right, left.Size);
        return context.URem(left, rightBitVec);
    }

    public static Z3BitVecExpr SRem(this Z3Context context, Z3BitVecExpr left, BigInteger right)
    {
        var rightBitVec = context.BitVec(right, left.Size);
        return context.SRem(left, rightBitVec);
    }

    public static Z3BitVecExpr SMod(this Z3Context context, Z3BitVecExpr left, BigInteger right)
    {
        var rightBitVec = context.BitVec(right, left.Size);
        return context.SMod(left, rightBitVec);
    }

    // BigInteger arithmetic operations (BigInteger op Z3BitVecExpr)
    public static Z3BitVecExpr Add(this Z3Context context, BigInteger left, Z3BitVecExpr right)
    {
        var leftBitVec = context.BitVec(left, right.Size);
        return context.Add(leftBitVec, right);
    }

    public static Z3BitVecExpr Sub(this Z3Context context, BigInteger left, Z3BitVecExpr right)
    {
        var leftBitVec = context.BitVec(left, right.Size);
        return context.Sub(leftBitVec, right);
    }

    public static Z3BitVecExpr Mul(this Z3Context context, BigInteger left, Z3BitVecExpr right)
    {
        var leftBitVec = context.BitVec(left, right.Size);
        return context.Mul(leftBitVec, right);
    }

    public static Z3BitVecExpr UDiv(this Z3Context context, BigInteger left, Z3BitVecExpr right)
    {
        var leftBitVec = context.BitVec(left, right.Size);
        return context.UDiv(leftBitVec, right);
    }

    public static Z3BitVecExpr SDiv(this Z3Context context, BigInteger left, Z3BitVecExpr right)
    {
        var leftBitVec = context.BitVec(left, right.Size);
        return context.SDiv(leftBitVec, right);
    }

    public static Z3BitVecExpr URem(this Z3Context context, BigInteger left, Z3BitVecExpr right)
    {
        var leftBitVec = context.BitVec(left, right.Size);
        return context.URem(leftBitVec, right);
    }

    public static Z3BitVecExpr SRem(this Z3Context context, BigInteger left, Z3BitVecExpr right)
    {
        var leftBitVec = context.BitVec(left, right.Size);
        return context.SRem(leftBitVec, right);
    }

    public static Z3BitVecExpr SMod(this Z3Context context, BigInteger left, Z3BitVecExpr right)
    {
        var leftBitVec = context.BitVec(left, right.Size);
        return context.SMod(leftBitVec, right);
    }

    // BigInteger bitwise operations (Z3BitVecExpr op BigInteger)
    public static Z3BitVecExpr And(this Z3Context context, Z3BitVecExpr left, BigInteger right)
    {
        var rightBitVec = context.BitVec(right, left.Size);
        return context.And(left, rightBitVec);
    }

    public static Z3BitVecExpr Or(this Z3Context context, Z3BitVecExpr left, BigInteger right)
    {
        var rightBitVec = context.BitVec(right, left.Size);
        return context.Or(left, rightBitVec);
    }

    public static Z3BitVecExpr Xor(this Z3Context context, Z3BitVecExpr left, BigInteger right)
    {
        var rightBitVec = context.BitVec(right, left.Size);
        return context.Xor(left, rightBitVec);
    }

    // BigInteger bitwise operations (BigInteger op Z3BitVecExpr)
    public static Z3BitVecExpr And(this Z3Context context, BigInteger left, Z3BitVecExpr right)
    {
        var leftBitVec = context.BitVec(left, right.Size);
        return context.And(leftBitVec, right);
    }

    public static Z3BitVecExpr Or(this Z3Context context, BigInteger left, Z3BitVecExpr right)
    {
        var leftBitVec = context.BitVec(left, right.Size);
        return context.Or(leftBitVec, right);
    }

    public static Z3BitVecExpr Xor(this Z3Context context, BigInteger left, Z3BitVecExpr right)
    {
        var leftBitVec = context.BitVec(left, right.Size);
        return context.Xor(leftBitVec, right);
    }

    // BigInteger shift operations
    public static Z3BitVecExpr Shl(this Z3Context context, Z3BitVecExpr left, BigInteger right)
    {
        var rightBitVec = context.BitVec(right, left.Size);
        return context.Shl(left, rightBitVec);
    }

    public static Z3BitVecExpr Lshr(this Z3Context context, Z3BitVecExpr left, BigInteger right)
    {
        var rightBitVec = context.BitVec(right, left.Size);
        return context.Lshr(left, rightBitVec);
    }

    public static Z3BitVecExpr Ashr(this Z3Context context, Z3BitVecExpr left, BigInteger right)
    {
        var rightBitVec = context.BitVec(right, left.Size);
        return context.Ashr(left, rightBitVec);
    }

    // BigInteger comparison operations (Z3BitVecExpr op BigInteger)
    public static Z3BoolExpr Ult(this Z3Context context, Z3BitVecExpr left, BigInteger right)
    {
        var rightBitVec = context.BitVec(right, left.Size);
        return context.Ult(left, rightBitVec);
    }

    public static Z3BoolExpr Slt(this Z3Context context, Z3BitVecExpr left, BigInteger right)
    {
        var rightBitVec = context.BitVec(right, left.Size);
        return context.Slt(left, rightBitVec);
    }

    public static Z3BoolExpr Ule(this Z3Context context, Z3BitVecExpr left, BigInteger right)
    {
        var rightBitVec = context.BitVec(right, left.Size);
        return context.Ule(left, rightBitVec);
    }

    public static Z3BoolExpr Sle(this Z3Context context, Z3BitVecExpr left, BigInteger right)
    {
        var rightBitVec = context.BitVec(right, left.Size);
        return context.Sle(left, rightBitVec);
    }

    public static Z3BoolExpr Ugt(this Z3Context context, Z3BitVecExpr left, BigInteger right)
    {
        var rightBitVec = context.BitVec(right, left.Size);
        return context.Ugt(left, rightBitVec);
    }

    public static Z3BoolExpr Sgt(this Z3Context context, Z3BitVecExpr left, BigInteger right)
    {
        var rightBitVec = context.BitVec(right, left.Size);
        return context.Sgt(left, rightBitVec);
    }

    public static Z3BoolExpr Uge(this Z3Context context, Z3BitVecExpr left, BigInteger right)
    {
        var rightBitVec = context.BitVec(right, left.Size);
        return context.Uge(left, rightBitVec);
    }

    public static Z3BoolExpr Sge(this Z3Context context, Z3BitVecExpr left, BigInteger right)
    {
        var rightBitVec = context.BitVec(right, left.Size);
        return context.Sge(left, rightBitVec);
    }

    // BigInteger comparison operations (BigInteger op Z3BitVecExpr)
    public static Z3BoolExpr Ult(this Z3Context context, BigInteger left, Z3BitVecExpr right)
    {
        var leftBitVec = context.BitVec(left, right.Size);
        return context.Ult(leftBitVec, right);
    }

    public static Z3BoolExpr Slt(this Z3Context context, BigInteger left, Z3BitVecExpr right)
    {
        var leftBitVec = context.BitVec(left, right.Size);
        return context.Slt(leftBitVec, right);
    }

    public static Z3BoolExpr Ule(this Z3Context context, BigInteger left, Z3BitVecExpr right)
    {
        var leftBitVec = context.BitVec(left, right.Size);
        return context.Ule(leftBitVec, right);
    }

    public static Z3BoolExpr Sle(this Z3Context context, BigInteger left, Z3BitVecExpr right)
    {
        var leftBitVec = context.BitVec(left, right.Size);
        return context.Sle(leftBitVec, right);
    }

    public static Z3BoolExpr Ugt(this Z3Context context, BigInteger left, Z3BitVecExpr right)
    {
        var leftBitVec = context.BitVec(left, right.Size);
        return context.Ugt(leftBitVec, right);
    }

    public static Z3BoolExpr Sgt(this Z3Context context, BigInteger left, Z3BitVecExpr right)
    {
        var leftBitVec = context.BitVec(left, right.Size);
        return context.Sgt(leftBitVec, right);
    }

    public static Z3BoolExpr Uge(this Z3Context context, BigInteger left, Z3BitVecExpr right)
    {
        var leftBitVec = context.BitVec(left, right.Size);
        return context.Uge(leftBitVec, right);
    }

    public static Z3BoolExpr Sge(this Z3Context context, BigInteger left, Z3BitVecExpr right)
    {
        var leftBitVec = context.BitVec(left, right.Size);
        return context.Sge(leftBitVec, right);
    }
}