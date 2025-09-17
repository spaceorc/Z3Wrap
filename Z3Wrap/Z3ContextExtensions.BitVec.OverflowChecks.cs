using System.Numerics;
using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap;

public static partial class Z3ContextExtensions
{
    // Bitvector overflow and underflow checking operations

    public static Z3BoolExpr AddNoOverflow(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right, bool signed = false)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvAddNoOverflow(context.Handle, left.Handle, right.Handle, signed);
        return Z3BoolExpr.Create(context, handle);
    }

    public static Z3BoolExpr SignedSubNoOverflow(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvSubNoOverflow(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, handle);
    }

    public static Z3BoolExpr SubNoUnderflow(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right, bool signed = true)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvSubNoUnderflow(context.Handle, left.Handle, right.Handle, signed);
        return Z3BoolExpr.Create(context, handle);
    }

    public static Z3BoolExpr MulNoOverflow(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right, bool signed = false)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvMulNoOverflow(context.Handle, left.Handle, right.Handle, signed);
        return Z3BoolExpr.Create(context, handle);
    }

    public static Z3BoolExpr SignedMulNoUnderflow(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvMulNoUnderflow(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, handle);
    }

    public static Z3BoolExpr SignedAddNoUnderflow(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvAddNoUnderflow(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, handle);
    }

    public static Z3BoolExpr SignedDivNoOverflow(this Z3Context context, Z3BitVecExpr left, Z3BitVecExpr right)
    {
        if (left.Size != right.Size)
            throw new ArgumentException($"BitVector size mismatch: left={left.Size}, right={right.Size}");
        var handle = NativeMethods.Z3MkBvSDivNoOverflow(context.Handle, left.Handle, right.Handle);
        return Z3BoolExpr.Create(context, handle);
    }

    public static Z3BoolExpr SignedNegNoOverflow(this Z3Context context, Z3BitVecExpr operand)
    {
        var handle = NativeMethods.Z3MkBvNegNoOverflow(context.Handle, operand.Handle);
        return Z3BoolExpr.Create(context, handle);
    }

    // BigInteger overflow check operations (Z3BitVecExpr op BigInteger)
    public static Z3BoolExpr AddNoOverflow(this Z3Context context, Z3BitVecExpr left, BigInteger right, bool signed = false) => context.AddNoOverflow(left, context.BitVec(right, left.Size), signed);
    public static Z3BoolExpr SignedAddNoUnderflow(this Z3Context context, Z3BitVecExpr left, BigInteger right) => context.SignedAddNoUnderflow(left, context.BitVec(right, left.Size));
    public static Z3BoolExpr SignedSubNoOverflow(this Z3Context context, Z3BitVecExpr left, BigInteger right) => context.SignedSubNoOverflow(left, context.BitVec(right, left.Size));
    public static Z3BoolExpr SubNoUnderflow(this Z3Context context, Z3BitVecExpr left, BigInteger right, bool signed = true) => context.SubNoUnderflow(left, context.BitVec(right, left.Size), signed);
    public static Z3BoolExpr MulNoOverflow(this Z3Context context, Z3BitVecExpr left, BigInteger right, bool signed = false) => context.MulNoOverflow(left, context.BitVec(right, left.Size), signed);
    public static Z3BoolExpr SignedMulNoUnderflow(this Z3Context context, Z3BitVecExpr left, BigInteger right) => context.SignedMulNoUnderflow(left, context.BitVec(right, left.Size));
    public static Z3BoolExpr SignedDivNoOverflow(this Z3Context context, Z3BitVecExpr left, BigInteger right) => context.SignedDivNoOverflow(left, context.BitVec(right, left.Size));

    // BigInteger overflow check operations (BigInteger op Z3BitVecExpr)
    public static Z3BoolExpr AddNoOverflow(this Z3Context context, BigInteger left, Z3BitVecExpr right, bool signed = false) => context.AddNoOverflow(context.BitVec(left, right.Size), right, signed);
    public static Z3BoolExpr SignedAddNoUnderflow(this Z3Context context, BigInteger left, Z3BitVecExpr right) => context.SignedAddNoUnderflow(context.BitVec(left, right.Size), right);
    public static Z3BoolExpr SignedSubNoOverflow(this Z3Context context, BigInteger left, Z3BitVecExpr right) => context.SignedSubNoOverflow(context.BitVec(left, right.Size), right);
    public static Z3BoolExpr SubNoUnderflow(this Z3Context context, BigInteger left, Z3BitVecExpr right, bool signed = true) => context.SubNoUnderflow(context.BitVec(left, right.Size), right, signed);
    public static Z3BoolExpr MulNoOverflow(this Z3Context context, BigInteger left, Z3BitVecExpr right, bool signed = false) => context.MulNoOverflow(context.BitVec(left, right.Size), right, signed);
    public static Z3BoolExpr SignedMulNoUnderflow(this Z3Context context, BigInteger left, Z3BitVecExpr right) => context.SignedMulNoUnderflow(context.BitVec(left, right.Size), right);
    public static Z3BoolExpr SignedDivNoOverflow(this Z3Context context, BigInteger left, Z3BitVecExpr right) => context.SignedDivNoOverflow(context.BitVec(left, right.Size), right);
}