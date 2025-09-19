using System.Numerics;
using Spaceorc.Z3Wrap.DataTypes;

namespace Spaceorc.Z3Wrap.Expressions;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
/// <summary>
/// Represents a fixed-width bitvector expression in Z3 with comprehensive bitwise and arithmetic operations.
/// Supports both unsigned and signed operations, bit manipulation, and conversion to other numeric types.
/// </summary>
public sealed class Z3BitVecExpr : Z3NumericExpr
{
    /// <summary>
    /// Gets the bit width (size in bits) of this bitvector expression.
    /// </summary>
    public uint Size { get; }

    internal Z3BitVecExpr(Z3Context context, IntPtr handle, uint size) : base(context, handle)
    {
        Size = size;
    }

    internal new static Z3BitVecExpr Create(Z3Context context, IntPtr handle)
    {
        return (Z3BitVecExpr)Z3Expr.Create(context, handle);
    }

    /// <summary>
    /// Implicitly converts a BitVec value to a Z3BitVecExpr using the current thread-local context.
    /// </summary>
    /// <param name="value">The BitVec value to convert.</param>
    /// <returns>A Z3BitVecExpr representing the bitvector constant.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no current context is set.</exception>
    public static implicit operator Z3BitVecExpr(BitVec value) => Z3Context.Current.BitVec(value);

    /// <summary>
    /// Returns a string representation of this bitvector expression including its bit width.
    /// </summary>
    /// <returns>A string in the format "BitVec[size](expression)".</returns>
    public override string ToString() => $"BitVec[{Size}]({base.ToString()})";

    public Z3BitVecExpr Extend(uint additionalBits, bool signed = false) => Context.Extend(this, additionalBits, signed);
    public Z3BitVecExpr Extract(uint high, uint low) => Context.Extract(this, high, low);
    public Z3BitVecExpr Resize(uint newSize, bool signed = false) => Context.Resize(this, newSize, signed);
    public Z3BitVecExpr Repeat(uint count) => Context.Repeat(this, count);
    public Z3IntExpr ToInt(bool signed = false) => Context.ToInt(this, signed);

    public Z3BoolExpr Lt(Z3BitVecExpr other, bool signed = false) => Context.Lt(this, other, signed);
    public Z3BoolExpr Le(Z3BitVecExpr other, bool signed = false) => Context.Le(this, other, signed);
    public Z3BoolExpr Gt(Z3BitVecExpr other, bool signed = false) => Context.Gt(this, other, signed);
    public Z3BoolExpr Ge(Z3BitVecExpr other, bool signed = false) => Context.Ge(this, other, signed);

    public Z3BoolExpr Lt(BigInteger other, bool signed = false) => Context.Lt(this, other, signed);
    public Z3BoolExpr Le(BigInteger other, bool signed = false) => Context.Le(this, other, signed);
    public Z3BoolExpr Gt(BigInteger other, bool signed = false) => Context.Gt(this, other, signed);
    public Z3BoolExpr Ge(BigInteger other, bool signed = false) => Context.Ge(this, other, signed);
    
    public Z3BitVecExpr Add(Z3BitVecExpr other) => Context.Add(this, other);
    public Z3BitVecExpr Sub(Z3BitVecExpr other) => Context.Sub(this, other);
    public Z3BitVecExpr Mul(Z3BitVecExpr other) => Context.Mul(this, other);
    public Z3BitVecExpr Div(Z3BitVecExpr other, bool signed = false) => Context.Div(this, other, signed);
    public Z3BitVecExpr Rem(Z3BitVecExpr other, bool signed = false) => Context.Rem(this, other, signed);
    public Z3BitVecExpr Neg() => Context.Neg(this);
    
    public Z3BitVecExpr Add(BigInteger other) => Context.Add(this, other);
    public Z3BitVecExpr Sub(BigInteger other) => Context.Sub(this, other);
    public Z3BitVecExpr Mul(BigInteger other) => Context.Mul(this, other);
    public Z3BitVecExpr Div(BigInteger other, bool signed = false) => Context.Div(this, other, signed);
    public Z3BitVecExpr Rem(BigInteger other, bool signed = false) => Context.Rem(this, other, signed);

    public Z3BitVecExpr SignedMod(Z3BitVecExpr other) => Context.SignedMod(this, other);
    public Z3BitVecExpr SignedMod(BigInteger other) => Context.SignedMod(this, other);

    public Z3BitVecExpr Shl(Z3BitVecExpr amount) => Context.Shl(this, amount);
    public Z3BitVecExpr Shr(Z3BitVecExpr amount, bool signed = false) => Context.Shr(this, amount, signed);

    public Z3BitVecExpr Shl(BigInteger amount) => Context.Shl(this, amount);
    public Z3BitVecExpr Shr(BigInteger amount, bool signed = false) => Context.Shr(this, amount, signed);

    public Z3BoolExpr AddNoOverflow(Z3BitVecExpr other, bool signed = false) => Context.AddNoOverflow(this, other, signed);
    public Z3BoolExpr SignedAddNoUnderflow(Z3BitVecExpr other) => Context.SignedAddNoUnderflow(this, other);
    public Z3BoolExpr SignedSubNoOverflow(Z3BitVecExpr other) => Context.SignedSubNoOverflow(this, other);
    public Z3BoolExpr SubNoUnderflow(Z3BitVecExpr other, bool signed = true) => Context.SubNoUnderflow(this, other, signed);
    public Z3BoolExpr MulNoOverflow(Z3BitVecExpr other, bool signed = false) => Context.MulNoOverflow(this, other, signed);
    public Z3BoolExpr SignedMulNoUnderflow(Z3BitVecExpr other) => Context.SignedMulNoUnderflow(this, other);
    public Z3BoolExpr SignedDivNoOverflow(Z3BitVecExpr other) => Context.SignedDivNoOverflow(this, other);
    public Z3BoolExpr SignedNegNoOverflow() => Context.SignedNegNoOverflow(this);

    public Z3BoolExpr AddNoOverflow(BigInteger other, bool signed = false) => Context.AddNoOverflow(this, other, signed);
    public Z3BoolExpr SignedAddNoUnderflow(BigInteger other) => Context.SignedAddNoUnderflow(this, other);
    public Z3BoolExpr SignedSubNoOverflow(BigInteger other) => Context.SignedSubNoOverflow(this, other);
    public Z3BoolExpr SubNoUnderflow(BigInteger other, bool signed = true) => Context.SubNoUnderflow(this, other, signed);
    public Z3BoolExpr MulNoOverflow(BigInteger other, bool signed = false) => Context.MulNoOverflow(this, other, signed);
    public Z3BoolExpr SignedMulNoUnderflow(BigInteger other) => Context.SignedMulNoUnderflow(this, other);
    public Z3BoolExpr SignedDivNoOverflow(BigInteger other) => Context.SignedDivNoOverflow(this, other);

    public static Z3BitVecExpr operator +(Z3BitVecExpr left, Z3BitVecExpr right) => left.Context.Add(left, right);
    public static Z3BitVecExpr operator -(Z3BitVecExpr left, Z3BitVecExpr right) => left.Context.Sub(left, right);
    public static Z3BitVecExpr operator *(Z3BitVecExpr left, Z3BitVecExpr right) => left.Context.Mul(left, right);
    public static Z3BitVecExpr operator /(Z3BitVecExpr left, Z3BitVecExpr right) => left.Context.Div(left, right);
    public static Z3BitVecExpr operator %(Z3BitVecExpr left, Z3BitVecExpr right) => left.Context.Rem(left, right);
    public static Z3BitVecExpr operator -(Z3BitVecExpr operand) => operand.Context.Neg(operand);

    public static Z3BitVecExpr operator &(Z3BitVecExpr left, Z3BitVecExpr right) => left.Context.And(left, right);
    public static Z3BitVecExpr operator |(Z3BitVecExpr left, Z3BitVecExpr right) => left.Context.Or(left, right);
    public static Z3BitVecExpr operator ^(Z3BitVecExpr left, Z3BitVecExpr right) => left.Context.Xor(left, right);
    public static Z3BitVecExpr operator ~(Z3BitVecExpr operand) => operand.Context.Not(operand);

    public static Z3BitVecExpr operator <<(Z3BitVecExpr left, Z3BitVecExpr right) => left.Context.Shl(left, right);
    public static Z3BitVecExpr operator >>(Z3BitVecExpr left, Z3BitVecExpr right) => left.Context.Shr(left, right);

    public Z3BitVecExpr And(Z3BitVecExpr other) => Context.And(this, other);
    public Z3BitVecExpr Or(Z3BitVecExpr other) => Context.Or(this, other);
    public Z3BitVecExpr Xor(Z3BitVecExpr other) => Context.Xor(this, other);
    public Z3BitVecExpr Not() => Context.Not(this);

    public Z3BitVecExpr And(BigInteger other) => Context.And(this, other);
    public Z3BitVecExpr Or(BigInteger other) => Context.Or(this, other);
    public Z3BitVecExpr Xor(BigInteger other) => Context.Xor(this, other);

    public static Z3BoolExpr operator <(Z3BitVecExpr left, Z3BitVecExpr right) => left.Context.Lt(left, right);
    public static Z3BoolExpr operator <=(Z3BitVecExpr left, Z3BitVecExpr right) => left.Context.Le(left, right);
    public static Z3BoolExpr operator >(Z3BitVecExpr left, Z3BitVecExpr right) => left.Context.Gt(left, right);
    public static Z3BoolExpr operator >=(Z3BitVecExpr left, Z3BitVecExpr right) => left.Context.Ge(left, right);

    public static Z3BitVecExpr operator +(Z3BitVecExpr left, BigInteger right) => left.Context.Add(left, right);
    public static Z3BitVecExpr operator -(Z3BitVecExpr left, BigInteger right) => left.Context.Sub(left, right);
    public static Z3BitVecExpr operator *(Z3BitVecExpr left, BigInteger right) => left.Context.Mul(left, right);
    public static Z3BitVecExpr operator /(Z3BitVecExpr left, BigInteger right) => left.Context.Div(left, right);
    public static Z3BitVecExpr operator %(Z3BitVecExpr left, BigInteger right) => left.Context.Rem(left, right);

    public static Z3BitVecExpr operator +(BigInteger left, Z3BitVecExpr right) => right.Context.Add(left, right);
    public static Z3BitVecExpr operator -(BigInteger left, Z3BitVecExpr right) => right.Context.Sub(left, right);
    public static Z3BitVecExpr operator *(BigInteger left, Z3BitVecExpr right) => right.Context.Mul(left, right);
    public static Z3BitVecExpr operator /(BigInteger left, Z3BitVecExpr right) => right.Context.Div(left, right);
    public static Z3BitVecExpr operator %(BigInteger left, Z3BitVecExpr right) => right.Context.Rem(left, right);

    public static Z3BitVecExpr operator &(Z3BitVecExpr left, BigInteger right) => left.Context.And(left, right);
    public static Z3BitVecExpr operator |(Z3BitVecExpr left, BigInteger right) => left.Context.Or(left, right);
    public static Z3BitVecExpr operator ^(Z3BitVecExpr left, BigInteger right) => left.Context.Xor(left, right);

    public static Z3BitVecExpr operator &(BigInteger left, Z3BitVecExpr right) => right.Context.And(left, right);
    public static Z3BitVecExpr operator |(BigInteger left, Z3BitVecExpr right) => right.Context.Or(left, right);
    public static Z3BitVecExpr operator ^(BigInteger left, Z3BitVecExpr right) => right.Context.Xor(left, right);

    public static Z3BitVecExpr operator <<(Z3BitVecExpr left, BigInteger right) => left.Context.Shl(left, right);
    public static Z3BitVecExpr operator >>(Z3BitVecExpr left, BigInteger right) => left.Context.Shr(left, right);

    public static Z3BoolExpr operator <(Z3BitVecExpr left, BigInteger right) => left.Context.Lt(left, right);
    public static Z3BoolExpr operator <=(Z3BitVecExpr left, BigInteger right) => left.Context.Le(left, right);
    public static Z3BoolExpr operator >(Z3BitVecExpr left, BigInteger right) => left.Context.Gt(left, right);
    public static Z3BoolExpr operator >=(Z3BitVecExpr left, BigInteger right) => left.Context.Ge(left, right);

    public static Z3BoolExpr operator <(BigInteger left, Z3BitVecExpr right) => right.Context.Lt(left, right);
    public static Z3BoolExpr operator <=(BigInteger left, Z3BitVecExpr right) => right.Context.Le(left, right);
    public static Z3BoolExpr operator >(BigInteger left, Z3BitVecExpr right) => right.Context.Gt(left, right);
    public static Z3BoolExpr operator >=(BigInteger left, Z3BitVecExpr right) => right.Context.Ge(left, right);

    public static Z3BoolExpr operator ==(Z3BitVecExpr left, BigInteger right) => left.Context.Eq(left, right);
    public static Z3BoolExpr operator ==(BigInteger left, Z3BitVecExpr right) => right.Context.Eq(left, right);
    public static Z3BoolExpr operator !=(Z3BitVecExpr left, BigInteger right) => left.Context.Neq(left, right);
    public static Z3BoolExpr operator !=(BigInteger left, Z3BitVecExpr right) => right.Context.Neq(left, right);
}