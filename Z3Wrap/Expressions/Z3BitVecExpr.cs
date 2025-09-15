using System.Numerics;
using Z3Wrap.DataTypes;

namespace Z3Wrap.Expressions;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
public sealed class Z3BitVecExpr : Z3NumericExpr
{
    public uint Size { get; }

    internal Z3BitVecExpr(Z3Context context, IntPtr handle, uint size) : base(context, handle)
    {
        Size = size;
    }

    public new static Z3BitVecExpr Create(Z3Context context, IntPtr handle)
    {
        return (Z3BitVecExpr)Z3Expr.Create(context, handle);
    }

    // Implicit conversion from BitVec using current context
    public static implicit operator Z3BitVecExpr(BitVec value) => Z3Context.Current.BitVec(value);

    public override string ToString() => $"BitVec[{Size}]({base.ToString()})";

    // Fluent API for bitvector operations
    public Z3BitVecExpr Extend(uint additionalBits) => Context.Extend(this, additionalBits);
    public Z3BitVecExpr SignedExtend(uint additionalBits) => Context.SignedExtend(this, additionalBits);
    public Z3BitVecExpr Extract(uint high, uint low) => Context.Extract(this, high, low);
    public Z3BitVecExpr Resize(uint newSize) => Context.Resize(this, newSize);
    public Z3BitVecExpr SignedResize(uint newSize) => Context.SignedResize(this, newSize);
    public Z3BitVecExpr Repeat(uint count) => Context.Repeat(this, count);
    public Z3IntExpr ToInt() => Context.ToInt(this);
    public Z3IntExpr ToSignedInt() => Context.ToSignedInt(this);

    // Signed comparison methods (for explicit signed comparisons)
    public Z3BoolExpr SignedLt(Z3BitVecExpr other) => Context.SignedLt(this, other);
    public Z3BoolExpr SignedLe(Z3BitVecExpr other) => Context.SignedLe(this, other);
    public Z3BoolExpr SignedGt(Z3BitVecExpr other) => Context.SignedGt(this, other);
    public Z3BoolExpr SignedGe(Z3BitVecExpr other) => Context.SignedGe(this, other);

    // Signed division methods (for explicit signed operations)
    public Z3BitVecExpr SignedDiv(Z3BitVecExpr other) => Context.SignedDiv(this, other);
    public Z3BitVecExpr SignedRem(Z3BitVecExpr other) => Context.SignedRem(this, other);
    public Z3BitVecExpr SignedMod(Z3BitVecExpr other) => Context.SignedMod(this, other);

    // Shift methods (for explicit shift types)
    public Z3BitVecExpr Shl(Z3BitVecExpr amount) => Context.Shl(this, amount);
    public Z3BitVecExpr Shr(Z3BitVecExpr amount) => Context.Shr(this, amount);
    public Z3BitVecExpr SignedShr(Z3BitVecExpr amount) => Context.SignedShr(this, amount);

    // Overflow/underflow checking methods
    public Z3BoolExpr AddNoOverflow(Z3BitVecExpr other, bool isSigned = false) => Context.AddNoOverflow(this, other, isSigned);
    public Z3BoolExpr SubNoOverflow(Z3BitVecExpr other) => Context.SubNoOverflow(this, other);
    public Z3BoolExpr SubNoUnderflow(Z3BitVecExpr other, bool isSigned = true) => Context.SubNoUnderflow(this, other, isSigned);
    public Z3BoolExpr MulNoOverflow(Z3BitVecExpr other, bool isSigned = false) => Context.MulNoOverflow(this, other, isSigned);
    public Z3BoolExpr MulNoUnderflow(Z3BitVecExpr other) => Context.MulNoUnderflow(this, other);

    // Overflow/underflow checking methods with BigInteger
    public Z3BoolExpr AddNoOverflow(BigInteger other, bool isSigned = false) => Context.AddNoOverflow(this, other, isSigned);
    public Z3BoolExpr SubNoOverflow(BigInteger other) => Context.SubNoOverflow(this, other);
    public Z3BoolExpr SubNoUnderflow(BigInteger other, bool isSigned = true) => Context.SubNoUnderflow(this, other, isSigned);
    public Z3BoolExpr MulNoOverflow(BigInteger other, bool isSigned = false) => Context.MulNoOverflow(this, other, isSigned);
    public Z3BoolExpr MulNoUnderflow(BigInteger other) => Context.MulNoUnderflow(this, other);

    // Arithmetic operators (unsigned by default)
    public static Z3BitVecExpr operator +(Z3BitVecExpr left, Z3BitVecExpr right) => left.Context.Add(left, right);
    public static Z3BitVecExpr operator -(Z3BitVecExpr left, Z3BitVecExpr right) => left.Context.Sub(left, right);
    public static Z3BitVecExpr operator *(Z3BitVecExpr left, Z3BitVecExpr right) => left.Context.Mul(left, right);
    public static Z3BitVecExpr operator /(Z3BitVecExpr left, Z3BitVecExpr right) => left.Context.Div(left, right);
    public static Z3BitVecExpr operator %(Z3BitVecExpr left, Z3BitVecExpr right) => left.Context.Rem(left, right);
    public static Z3BitVecExpr operator -(Z3BitVecExpr operand) => operand.Context.Neg(operand);

    // Bitwise operators
    public static Z3BitVecExpr operator &(Z3BitVecExpr left, Z3BitVecExpr right) => left.Context.And(left, right);
    public static Z3BitVecExpr operator |(Z3BitVecExpr left, Z3BitVecExpr right) => left.Context.Or(left, right);
    public static Z3BitVecExpr operator ^(Z3BitVecExpr left, Z3BitVecExpr right) => left.Context.Xor(left, right);
    public static Z3BitVecExpr operator ~(Z3BitVecExpr operand) => operand.Context.Not(operand);

    // Shift operators (logical by default)
    public static Z3BitVecExpr operator <<(Z3BitVecExpr left, Z3BitVecExpr right) => left.Context.Shl(left, right);
    public static Z3BitVecExpr operator >>(Z3BitVecExpr left, Z3BitVecExpr right) => left.Context.Shr(left, right);

    // Comparison operators (unsigned by default)
    public static Z3BoolExpr operator <(Z3BitVecExpr left, Z3BitVecExpr right) => left.Context.Lt(left, right);
    public static Z3BoolExpr operator <=(Z3BitVecExpr left, Z3BitVecExpr right) => left.Context.Le(left, right);
    public static Z3BoolExpr operator >(Z3BitVecExpr left, Z3BitVecExpr right) => left.Context.Gt(left, right);
    public static Z3BoolExpr operator >=(Z3BitVecExpr left, Z3BitVecExpr right) => left.Context.Ge(left, right);

    // BigInteger operators - Arithmetic (Z3BitVecExpr op BigInteger)
    public static Z3BitVecExpr operator +(Z3BitVecExpr left, BigInteger right) => left.Context.Add(left, right);
    public static Z3BitVecExpr operator -(Z3BitVecExpr left, BigInteger right) => left.Context.Sub(left, right);
    public static Z3BitVecExpr operator *(Z3BitVecExpr left, BigInteger right) => left.Context.Mul(left, right);
    public static Z3BitVecExpr operator /(Z3BitVecExpr left, BigInteger right) => left.Context.Div(left, right);
    public static Z3BitVecExpr operator %(Z3BitVecExpr left, BigInteger right) => left.Context.Rem(left, right);

    // BigInteger operators - Arithmetic (BigInteger op Z3BitVecExpr)
    public static Z3BitVecExpr operator +(BigInteger left, Z3BitVecExpr right) => right.Context.Add(left, right);
    public static Z3BitVecExpr operator -(BigInteger left, Z3BitVecExpr right) => right.Context.Sub(left, right);
    public static Z3BitVecExpr operator *(BigInteger left, Z3BitVecExpr right) => right.Context.Mul(left, right);
    public static Z3BitVecExpr operator /(BigInteger left, Z3BitVecExpr right) => right.Context.Div(left, right);
    public static Z3BitVecExpr operator %(BigInteger left, Z3BitVecExpr right) => right.Context.Rem(left, right);

    // BigInteger operators - Bitwise (Z3BitVecExpr op BigInteger)
    public static Z3BitVecExpr operator &(Z3BitVecExpr left, BigInteger right) => left.Context.And(left, right);
    public static Z3BitVecExpr operator |(Z3BitVecExpr left, BigInteger right) => left.Context.Or(left, right);
    public static Z3BitVecExpr operator ^(Z3BitVecExpr left, BigInteger right) => left.Context.Xor(left, right);

    // BigInteger operators - Bitwise (BigInteger op Z3BitVecExpr)
    public static Z3BitVecExpr operator &(BigInteger left, Z3BitVecExpr right) => right.Context.And(left, right);
    public static Z3BitVecExpr operator |(BigInteger left, Z3BitVecExpr right) => right.Context.Or(left, right);
    public static Z3BitVecExpr operator ^(BigInteger left, Z3BitVecExpr right) => right.Context.Xor(left, right);

    // BigInteger operators - Shift (only left-to-right makes sense)
    public static Z3BitVecExpr operator <<(Z3BitVecExpr left, BigInteger right) => left.Context.Shl(left, right);
    public static Z3BitVecExpr operator >>(Z3BitVecExpr left, BigInteger right) => left.Context.Shr(left, right);

    // BigInteger operators - Comparison (Z3BitVecExpr op BigInteger)
    public static Z3BoolExpr operator <(Z3BitVecExpr left, BigInteger right) => left.Context.Lt(left, right);
    public static Z3BoolExpr operator <=(Z3BitVecExpr left, BigInteger right) => left.Context.Le(left, right);
    public static Z3BoolExpr operator >(Z3BitVecExpr left, BigInteger right) => left.Context.Gt(left, right);
    public static Z3BoolExpr operator >=(Z3BitVecExpr left, BigInteger right) => left.Context.Ge(left, right);

    // BigInteger operators - Comparison (BigInteger op Z3BitVecExpr)
    public static Z3BoolExpr operator <(BigInteger left, Z3BitVecExpr right) => right.Context.Lt(left, right);
    public static Z3BoolExpr operator <=(BigInteger left, Z3BitVecExpr right) => right.Context.Le(left, right);
    public static Z3BoolExpr operator >(BigInteger left, Z3BitVecExpr right) => right.Context.Gt(left, right);
    public static Z3BoolExpr operator >=(BigInteger left, Z3BitVecExpr right) => right.Context.Ge(left, right);

    // BigInteger operators - Equality
    public static Z3BoolExpr operator ==(Z3BitVecExpr left, BigInteger right) => left.Context.Eq(left, right);
    public static Z3BoolExpr operator ==(BigInteger left, Z3BitVecExpr right) => right.Context.Eq(left, right);
    public static Z3BoolExpr operator !=(Z3BitVecExpr left, BigInteger right) => left.Context.Neq(left, right);
    public static Z3BoolExpr operator !=(BigInteger left, Z3BitVecExpr right) => right.Context.Neq(left, right);
}