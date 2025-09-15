using System.Numerics;
using Z3Wrap.Expressions;
using Z3Wrap.Interop;

namespace Z3Wrap;

public static partial class Z3ContextExtensions
{
    // Equality operations
    public static Z3BoolExpr Eq(this Z3Context context, Z3BitVecExpr left, BigInteger right) => context.Eq(left, context.BitVec(right, left.Size));
    public static Z3BoolExpr Eq(this Z3Context context, BigInteger left, Z3BitVecExpr right) => context.Eq(context.BitVec(left, right.Size), right);
    public static Z3BoolExpr Neq(this Z3Context context, Z3BitVecExpr left, BigInteger right) => context.Neq(left, context.BitVec(right, left.Size));
    public static Z3BoolExpr Neq(this Z3Context context, BigInteger left, Z3BitVecExpr right) => context.Neq(context.BitVec(left, right.Size), right);

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

    public static Z3BitVecExpr Div(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvudiv(context.Handle, left.Handle, right.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    public static Z3BitVecExpr SignedDiv(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvsdiv(context.Handle, left.Handle, right.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    public static Z3BitVecExpr Rem(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvurem(context.Handle, left.Handle, right.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    public static Z3BitVecExpr SignedRem(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvsrem(context.Handle, left.Handle, right.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    public static Z3BitVecExpr SignedMod(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
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

    public static Z3BitVecExpr Shr(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvlshr(context.Handle, left.Handle, right.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    public static Z3BitVecExpr SignedShr(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvashr(context.Handle, left.Handle, right.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    // Comparison operations (return Z3BoolExpr)
    public static Z3BoolExpr Lt(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvult(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, handle);
    }

    public static Z3BoolExpr SignedLt(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvslt(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, handle);
    }

    public static Z3BoolExpr Le(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvule(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, handle);
    }

    public static Z3BoolExpr SignedLe(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvsle(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, handle);
    }

    public static Z3BoolExpr Gt(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvugt(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, handle);
    }

    public static Z3BoolExpr SignedGt(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvsgt(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, handle);
    }

    public static Z3BoolExpr Ge(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvuge(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, handle);
    }

    public static Z3BoolExpr SignedGe(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvsge(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, handle);
    }

    // BigInteger arithmetic operations (Z3BitVecExpr op BigInteger)
    public static Z3BitVecExpr Add(this Z3Context context, Z3BitVecExpr left, BigInteger right) => context.Add(left, context.BitVec(right, left.Size));
    public static Z3BitVecExpr Sub(this Z3Context context, Z3BitVecExpr left, BigInteger right) => context.Sub(left, context.BitVec(right, left.Size));
    public static Z3BitVecExpr Mul(this Z3Context context, Z3BitVecExpr left, BigInteger right) => context.Mul(left, context.BitVec(right, left.Size));
    public static Z3BitVecExpr Div(this Z3Context context, Z3BitVecExpr left, BigInteger right) => context.Div(left, context.BitVec(right, left.Size));
    public static Z3BitVecExpr SignedDiv(this Z3Context context, Z3BitVecExpr left, BigInteger right) => context.SignedDiv(left, context.BitVec(right, left.Size));
    public static Z3BitVecExpr Rem(this Z3Context context, Z3BitVecExpr left, BigInteger right) => context.Rem(left, context.BitVec(right, left.Size));
    public static Z3BitVecExpr SignedRem(this Z3Context context, Z3BitVecExpr left, BigInteger right) => context.SignedRem(left, context.BitVec(right, left.Size));
    public static Z3BitVecExpr SignedMod(this Z3Context context, Z3BitVecExpr left, BigInteger right) => context.SignedMod(left, context.BitVec(right, left.Size));

    // BigInteger arithmetic operations (BigInteger op Z3BitVecExpr)
    public static Z3BitVecExpr Add(this Z3Context context, BigInteger left, Z3BitVecExpr right) => context.Add(context.BitVec(left, right.Size), right);
    public static Z3BitVecExpr Sub(this Z3Context context, BigInteger left, Z3BitVecExpr right) => context.Sub(context.BitVec(left, right.Size), right);
    public static Z3BitVecExpr Mul(this Z3Context context, BigInteger left, Z3BitVecExpr right) => context.Mul(context.BitVec(left, right.Size), right);
    public static Z3BitVecExpr Div(this Z3Context context, BigInteger left, Z3BitVecExpr right) => context.Div(context.BitVec(left, right.Size), right);
    public static Z3BitVecExpr SignedDiv(this Z3Context context, BigInteger left, Z3BitVecExpr right) => context.SignedDiv(context.BitVec(left, right.Size), right);
    public static Z3BitVecExpr Rem(this Z3Context context, BigInteger left, Z3BitVecExpr right) => context.Rem(context.BitVec(left, right.Size), right);
    public static Z3BitVecExpr SignedRem(this Z3Context context, BigInteger left, Z3BitVecExpr right) => context.SignedRem(context.BitVec(left, right.Size), right);
    public static Z3BitVecExpr SignedMod(this Z3Context context, BigInteger left, Z3BitVecExpr right) => context.SignedMod(context.BitVec(left, right.Size), right);

    // BigInteger bitwise operations (Z3BitVecExpr op BigInteger)
    public static Z3BitVecExpr And(this Z3Context context, Z3BitVecExpr left, BigInteger right) => context.And(left, context.BitVec(right, left.Size));
    public static Z3BitVecExpr Or(this Z3Context context, Z3BitVecExpr left, BigInteger right) => context.Or(left, context.BitVec(right, left.Size));
    public static Z3BitVecExpr Xor(this Z3Context context, Z3BitVecExpr left, BigInteger right) => context.Xor(left, context.BitVec(right, left.Size));

    // BigInteger bitwise operations (BigInteger op Z3BitVecExpr)
    public static Z3BitVecExpr And(this Z3Context context, BigInteger left, Z3BitVecExpr right) => context.And(context.BitVec(left, right.Size), right);
    public static Z3BitVecExpr Or(this Z3Context context, BigInteger left, Z3BitVecExpr right) => context.Or(context.BitVec(left, right.Size), right);
    public static Z3BitVecExpr Xor(this Z3Context context, BigInteger left, Z3BitVecExpr right) => context.Xor(context.BitVec(left, right.Size), right);

    // BigInteger shift operations
    public static Z3BitVecExpr Shl(this Z3Context context, Z3BitVecExpr left, BigInteger right) => context.Shl(left, context.BitVec(right, left.Size));
    public static Z3BitVecExpr Shr(this Z3Context context, Z3BitVecExpr left, BigInteger right) => context.Shr(left, context.BitVec(right, left.Size));
    public static Z3BitVecExpr SignedShr(this Z3Context context, Z3BitVecExpr left, BigInteger right) => context.SignedShr(left, context.BitVec(right, left.Size));

    // BigInteger comparison operations (Z3BitVecExpr op BigInteger)
    public static Z3BoolExpr Lt(this Z3Context context, Z3BitVecExpr left, BigInteger right) => context.Lt(left, context.BitVec(right, left.Size));
    public static Z3BoolExpr SignedLt(this Z3Context context, Z3BitVecExpr left, BigInteger right) => context.SignedLt(left, context.BitVec(right, left.Size));
    public static Z3BoolExpr Le(this Z3Context context, Z3BitVecExpr left, BigInteger right) => context.Le(left, context.BitVec(right, left.Size));
    public static Z3BoolExpr SignedLe(this Z3Context context, Z3BitVecExpr left, BigInteger right) => context.SignedLe(left, context.BitVec(right, left.Size));
    public static Z3BoolExpr Gt(this Z3Context context, Z3BitVecExpr left, BigInteger right) => context.Gt(left, context.BitVec(right, left.Size));
    public static Z3BoolExpr SignedGt(this Z3Context context, Z3BitVecExpr left, BigInteger right) => context.SignedGt(left, context.BitVec(right, left.Size));
    public static Z3BoolExpr Ge(this Z3Context context, Z3BitVecExpr left, BigInteger right) => context.Ge(left, context.BitVec(right, left.Size));
    public static Z3BoolExpr SignedGe(this Z3Context context, Z3BitVecExpr left, BigInteger right) => context.SignedGe(left, context.BitVec(right, left.Size));

    // BigInteger comparison operations (BigInteger op Z3BitVecExpr)
    public static Z3BoolExpr Lt(this Z3Context context, BigInteger left, Z3BitVecExpr right) => context.Lt(context.BitVec(left, right.Size), right);
    public static Z3BoolExpr SignedLt(this Z3Context context, BigInteger left, Z3BitVecExpr right) => context.SignedLt(context.BitVec(left, right.Size), right);
    public static Z3BoolExpr Le(this Z3Context context, BigInteger left, Z3BitVecExpr right) => context.Le(context.BitVec(left, right.Size), right);
    public static Z3BoolExpr SignedLe(this Z3Context context, BigInteger left, Z3BitVecExpr right) => context.SignedLe(context.BitVec(left, right.Size), right);
    public static Z3BoolExpr Gt(this Z3Context context, BigInteger left, Z3BitVecExpr right) => context.Gt(context.BitVec(left, right.Size), right);
    public static Z3BoolExpr SignedGt(this Z3Context context, BigInteger left, Z3BitVecExpr right) => context.SignedGt(context.BitVec(left, right.Size), right);
    public static Z3BoolExpr Ge(this Z3Context context, BigInteger left, Z3BitVecExpr right) => context.Ge(context.BitVec(left, right.Size), right);
    public static Z3BoolExpr SignedGe(this Z3Context context, BigInteger left, Z3BitVecExpr right) => context.SignedGe(context.BitVec(left, right.Size), right);
}