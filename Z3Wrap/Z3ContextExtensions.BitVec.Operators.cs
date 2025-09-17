using System.Numerics;
using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap;

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
        var handle = NativeMethods.Z3MkBvAdd(context.Handle, left.Handle, right.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    public static Z3BitVecExpr Sub(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvSub(context.Handle, left.Handle, right.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    public static Z3BitVecExpr Mul(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvMul(context.Handle, left.Handle, right.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    public static Z3BitVecExpr Div(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right, bool signed = false)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = signed
            ? NativeMethods.Z3MkBvSDiv(context.Handle, left.Handle, right.Handle)
            : NativeMethods.Z3MkBvUDiv(context.Handle, left.Handle, right.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    public static Z3BitVecExpr Rem(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right, bool signed = false)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = signed
            ? NativeMethods.Z3MkBvSRem(context.Handle, left.Handle, right.Handle)
            : NativeMethods.Z3MkBvURem(context.Handle, left.Handle, right.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    public static Z3BitVecExpr SignedMod(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvSMod(context.Handle, left.Handle, right.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    public static Z3BitVecExpr Neg(this Z3Context context, Z3BitVecExpr expr)
    {
        var handle = NativeMethods.Z3MkBvNeg(context.Handle, expr.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    // Bitwise operations
    public static Z3BitVecExpr And(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvAnd(context.Handle, left.Handle, right.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    public static Z3BitVecExpr Or(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvOr(context.Handle, left.Handle, right.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    public static Z3BitVecExpr Xor(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvXor(context.Handle, left.Handle, right.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    public static Z3BitVecExpr Not(this Z3Context context, Z3BitVecExpr expr)
    {
        var handle = NativeMethods.Z3MkBvNot(context.Handle, expr.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    // Shift operations
    public static Z3BitVecExpr Shl(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvShl(context.Handle, left.Handle, right.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    public static Z3BitVecExpr Shr(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right, bool signed = false)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = signed
            ? NativeMethods.Z3MkBvAShr(context.Handle, left.Handle, right.Handle)
            : NativeMethods.Z3MkBvLShr(context.Handle, left.Handle, right.Handle);
        return Z3BitVecExpr.Create(context, handle);
    }

    // Comparison operations (return Z3BoolExpr)
    public static Z3BoolExpr Lt(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right, bool signed = false)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = signed
            ? NativeMethods.Z3MkBvSLt(context.Handle, left.Handle, right.Handle)
            : NativeMethods.Z3MkBvULt(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, handle);
    }

    public static Z3BoolExpr Le(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right, bool signed = false)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = signed
            ? NativeMethods.Z3MkBvSLe(context.Handle, left.Handle, right.Handle)
            : NativeMethods.Z3MkBvULe(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, handle);
    }

    public static Z3BoolExpr Gt(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right, bool signed = false)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = signed
            ? NativeMethods.Z3MkBvSGt(context.Handle, left.Handle, right.Handle)
            : NativeMethods.Z3MkBvUGt(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, handle);
    }

    public static Z3BoolExpr Ge(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right, bool signed = false)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = signed
            ? NativeMethods.Z3MkBvSGe(context.Handle, left.Handle, right.Handle)
            : NativeMethods.Z3MkBvUGe(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, handle);
    }

    // BigInteger arithmetic operations (Z3BitVecExpr op BigInteger)
    public static Z3BitVecExpr Add(this Z3Context context, Z3BitVecExpr left, BigInteger right) => context.Add(left, context.BitVec(right, left.Size));
    public static Z3BitVecExpr Sub(this Z3Context context, Z3BitVecExpr left, BigInteger right) => context.Sub(left, context.BitVec(right, left.Size));
    public static Z3BitVecExpr Mul(this Z3Context context, Z3BitVecExpr left, BigInteger right) => context.Mul(left, context.BitVec(right, left.Size));
    public static Z3BitVecExpr Div(this Z3Context context, Z3BitVecExpr left, BigInteger right, bool signed = false) => context.Div(left, context.BitVec(right, left.Size), signed);
    public static Z3BitVecExpr Rem(this Z3Context context, Z3BitVecExpr left, BigInteger right, bool signed = false) => context.Rem(left, context.BitVec(right, left.Size), signed);
    public static Z3BitVecExpr SignedMod(this Z3Context context, Z3BitVecExpr left, BigInteger right) => context.SignedMod(left, context.BitVec(right, left.Size));

    // BigInteger arithmetic operations (BigInteger op Z3BitVecExpr)
    public static Z3BitVecExpr Add(this Z3Context context, BigInteger left, Z3BitVecExpr right) => context.Add(context.BitVec(left, right.Size), right);
    public static Z3BitVecExpr Sub(this Z3Context context, BigInteger left, Z3BitVecExpr right) => context.Sub(context.BitVec(left, right.Size), right);
    public static Z3BitVecExpr Mul(this Z3Context context, BigInteger left, Z3BitVecExpr right) => context.Mul(context.BitVec(left, right.Size), right);
    public static Z3BitVecExpr Div(this Z3Context context, BigInteger left, Z3BitVecExpr right, bool signed = false) => context.Div(context.BitVec(left, right.Size), right, signed);
    public static Z3BitVecExpr Rem(this Z3Context context, BigInteger left, Z3BitVecExpr right, bool signed = false) => context.Rem(context.BitVec(left, right.Size), right, signed);
    public static Z3BitVecExpr SignedMod(this Z3Context context, BigInteger left, Z3BitVecExpr right) => context.SignedMod(context.BitVec(left, right.Size), right);

    // BigInteger bitwise operations (Z3BitVecExpr op BigInteger)
    public static Z3BitVecExpr And(this Z3Context context, Z3BitVecExpr left, BigInteger right) => context.And(left, context.BitVec(right, left.Size));
    public static Z3BitVecExpr Or(this Z3Context context, Z3BitVecExpr left, BigInteger right) => context.Or(left, context.BitVec(right, left.Size));
    public static Z3BitVecExpr Xor(this Z3Context context, Z3BitVecExpr left, BigInteger right) => context.Xor(left, context.BitVec(right, left.Size));

    // BigInteger bitwise operations (BigInteger op Z3BitVecExpr)
    public static Z3BitVecExpr And(this Z3Context context, BigInteger left, Z3BitVecExpr right) => context.And(context.BitVec(left, right.Size), right);
    public static Z3BitVecExpr Or(this Z3Context context, BigInteger left, Z3BitVecExpr right) => context.Or(context.BitVec(left, right.Size), right);
    public static Z3BitVecExpr Xor(this Z3Context context, BigInteger left, Z3BitVecExpr right) => context.Xor(context.BitVec(left, right.Size), right);

    // BigInteger shift operations (Z3BitVecExpr op BigInteger)
    public static Z3BitVecExpr Shl(this Z3Context context, Z3BitVecExpr left, BigInteger right) => context.Shl(left, context.BitVec(right, left.Size));
    public static Z3BitVecExpr Shr(this Z3Context context, Z3BitVecExpr left, BigInteger right, bool signed = false) => context.Shr(left, context.BitVec(right, left.Size), signed);

    // BigInteger shift operations (BigInteger op Z3BitVecExpr)
    public static Z3BitVecExpr Shl(this Z3Context context, BigInteger left, Z3BitVecExpr right) => context.Shl(context.BitVec(left, right.Size), right);
    public static Z3BitVecExpr Shr(this Z3Context context, BigInteger left, Z3BitVecExpr right, bool signed = false) => context.Shr(context.BitVec(left, right.Size), right, signed);

    // BigInteger comparison operations (Z3BitVecExpr op BigInteger)
    public static Z3BoolExpr Lt(this Z3Context context, Z3BitVecExpr left, BigInteger right, bool signed = false) => context.Lt(left, context.BitVec(right, left.Size), signed);
    public static Z3BoolExpr Le(this Z3Context context, Z3BitVecExpr left, BigInteger right, bool signed = false) => context.Le(left, context.BitVec(right, left.Size), signed);
    public static Z3BoolExpr Gt(this Z3Context context, Z3BitVecExpr left, BigInteger right, bool signed = false) => context.Gt(left, context.BitVec(right, left.Size), signed);
    public static Z3BoolExpr Ge(this Z3Context context, Z3BitVecExpr left, BigInteger right, bool signed = false) => context.Ge(left, context.BitVec(right, left.Size), signed);

    // BigInteger comparison operations (BigInteger op Z3BitVecExpr)
    public static Z3BoolExpr Lt(this Z3Context context, BigInteger left, Z3BitVecExpr right, bool signed = false) => context.Lt(context.BitVec(left, right.Size), right, signed);
    public static Z3BoolExpr Le(this Z3Context context, BigInteger left, Z3BitVecExpr right, bool signed = false) => context.Le(context.BitVec(left, right.Size), right, signed);
    public static Z3BoolExpr Gt(this Z3Context context, BigInteger left, Z3BitVecExpr right, bool signed = false) => context.Gt(context.BitVec(left, right.Size), right, signed);
    public static Z3BoolExpr Ge(this Z3Context context, BigInteger left, Z3BitVecExpr right, bool signed = false) => context.Ge(context.BitVec(left, right.Size), right, signed);
}