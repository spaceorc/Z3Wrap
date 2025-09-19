using System.Numerics;
using Spaceorc.Z3Wrap.DataTypes;
using Spaceorc.Z3Wrap.Extensions;

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

    internal Z3BitVecExpr(Z3Context context, IntPtr handle, uint size)
        : base(context, handle)
    {
        Size = size;
    }

    internal static new Z3BitVecExpr Create(Z3Context context, IntPtr handle)
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

    /// <summary>
    /// Extends this bitvector by adding additional bits on the most significant side.
    /// </summary>
    /// <param name="additionalBits">The number of bits to add.</param>
    /// <param name="signed">Whether to extend with sign bits (true) or zero bits (false).</param>
    /// <returns>A bitvector expression with extended bit width.</returns>
    public Z3BitVecExpr Extend(uint additionalBits, bool signed = false) =>
        Context.Extend(this, additionalBits, signed);

    /// <summary>
    /// Extracts a sub-bitvector from bit positions [high, low] (inclusive).
    /// </summary>
    /// <param name="high">The highest bit position to extract.</param>
    /// <param name="low">The lowest bit position to extract.</param>
    /// <returns>A bitvector expression containing bits from high to low.</returns>
    public Z3BitVecExpr Extract(uint high, uint low) => Context.Extract(this, high, low);

    /// <summary>
    /// Resizes this bitvector to a new bit width by truncating or extending.
    /// </summary>
    /// <param name="newSize">The new bit width.</param>
    /// <param name="signed">Whether to sign-extend when growing or truncate when shrinking.</param>
    /// <returns>A bitvector expression with the new size.</returns>
    public Z3BitVecExpr Resize(uint newSize, bool signed = false) =>
        Context.Resize(this, newSize, signed);

    /// <summary>
    /// Concatenates this bitvector with itself a specified number of times.
    /// </summary>
    /// <param name="count">The number of repetitions.</param>
    /// <returns>A bitvector expression with repeated pattern.</returns>
    public Z3BitVecExpr Repeat(uint count) => Context.Repeat(this, count);

    /// <summary>
    /// Converts this bitvector to an integer expression.
    /// </summary>
    /// <param name="signed">Whether to interpret the bitvector as signed (true) or unsigned (false).</param>
    /// <returns>An integer expression representing this bitvector value.</returns>
    public Z3IntExpr ToInt(bool signed = false) => Context.ToInt(this, signed);

    /// <summary>
    /// Creates a less-than comparison with another bitvector expression.
    /// </summary>
    /// <param name="other">The bitvector to compare with.</param>
    /// <param name="signed">Whether to perform signed (true) or unsigned (false) comparison.</param>
    /// <returns>A boolean expression representing this &lt; other.</returns>
    public Z3BoolExpr Lt(Z3BitVecExpr other, bool signed = false) =>
        Context.Lt(this, other, signed);

    /// <summary>
    /// Creates a less-than-or-equal comparison with another bitvector expression.
    /// </summary>
    /// <param name="other">The bitvector to compare with.</param>
    /// <param name="signed">Whether to perform signed (true) or unsigned (false) comparison.</param>
    /// <returns>A boolean expression representing this &lt;= other.</returns>
    public Z3BoolExpr Le(Z3BitVecExpr other, bool signed = false) =>
        Context.Le(this, other, signed);

    /// <summary>
    /// Creates a greater-than comparison with another bitvector expression.
    /// </summary>
    /// <param name="other">The bitvector to compare with.</param>
    /// <param name="signed">Whether to perform signed (true) or unsigned (false) comparison.</param>
    /// <returns>A boolean expression representing this &gt; other.</returns>
    public Z3BoolExpr Gt(Z3BitVecExpr other, bool signed = false) =>
        Context.Gt(this, other, signed);

    /// <summary>
    /// Creates a greater-than-or-equal comparison with another bitvector expression.
    /// </summary>
    /// <param name="other">The bitvector to compare with.</param>
    /// <param name="signed">Whether to perform signed (true) or unsigned (false) comparison.</param>
    /// <returns>A boolean expression representing this &gt;= other.</returns>
    public Z3BoolExpr Ge(Z3BitVecExpr other, bool signed = false) =>
        Context.Ge(this, other, signed);

    /// <summary>
    /// Creates a less-than comparison with a BigInteger value.
    /// </summary>
    /// <param name="other">The BigInteger value to compare with.</param>
    /// <param name="signed">Whether to perform signed (true) or unsigned (false) comparison.</param>
    /// <returns>A boolean expression representing this &lt; other.</returns>
    public Z3BoolExpr Lt(BigInteger other, bool signed = false) => Context.Lt(this, other, signed);

    /// <summary>
    /// Creates a less-than-or-equal comparison with a BigInteger value.
    /// </summary>
    /// <param name="other">The BigInteger value to compare with.</param>
    /// <param name="signed">Whether to perform signed (true) or unsigned (false) comparison.</param>
    /// <returns>A boolean expression representing this &lt;= other.</returns>
    public Z3BoolExpr Le(BigInteger other, bool signed = false) => Context.Le(this, other, signed);

    /// <summary>
    /// Creates a greater-than comparison with a BigInteger value.
    /// </summary>
    /// <param name="other">The BigInteger value to compare with.</param>
    /// <param name="signed">Whether to perform signed (true) or unsigned (false) comparison.</param>
    /// <returns>A boolean expression representing this &gt; other.</returns>
    public Z3BoolExpr Gt(BigInteger other, bool signed = false) => Context.Gt(this, other, signed);

    /// <summary>
    /// Creates a greater-than-or-equal comparison with a BigInteger value.
    /// </summary>
    /// <param name="other">The BigInteger value to compare with.</param>
    /// <param name="signed">Whether to perform signed (true) or unsigned (false) comparison.</param>
    /// <returns>A boolean expression representing this &gt;= other.</returns>
    public Z3BoolExpr Ge(BigInteger other, bool signed = false) => Context.Ge(this, other, signed);

    /// <summary>
    /// Adds this bitvector to another bitvector expression.
    /// </summary>
    /// <param name="other">The bitvector to add.</param>
    /// <returns>A bitvector expression representing this + other.</returns>
    public Z3BitVecExpr Add(Z3BitVecExpr other) => Context.Add(this, other);

    /// <summary>
    /// Subtracts another bitvector expression from this bitvector.
    /// </summary>
    /// <param name="other">The bitvector to subtract.</param>
    /// <returns>A bitvector expression representing this - other.</returns>
    public Z3BitVecExpr Sub(Z3BitVecExpr other) => Context.Sub(this, other);

    /// <summary>
    /// Multiplies this bitvector by another bitvector expression.
    /// </summary>
    /// <param name="other">The bitvector to multiply by.</param>
    /// <returns>A bitvector expression representing this * other.</returns>
    public Z3BitVecExpr Mul(Z3BitVecExpr other) => Context.Mul(this, other);

    /// <summary>
    /// Divides this bitvector by another bitvector expression.
    /// </summary>
    /// <param name="other">The bitvector to divide by.</param>
    /// <param name="signed">Whether to perform signed (true) or unsigned (false) division.</param>
    /// <returns>A bitvector expression representing this / other.</returns>
    public Z3BitVecExpr Div(Z3BitVecExpr other, bool signed = false) =>
        Context.Div(this, other, signed);

    /// <summary>
    /// Computes the remainder of dividing this bitvector by another bitvector expression.
    /// </summary>
    /// <param name="other">The bitvector to divide by.</param>
    /// <param name="signed">Whether to perform signed (true) or unsigned (false) remainder.</param>
    /// <returns>A bitvector expression representing this % other.</returns>
    public Z3BitVecExpr Rem(Z3BitVecExpr other, bool signed = false) =>
        Context.Rem(this, other, signed);

    /// <summary>
    /// Negates this bitvector (computes two's complement).
    /// </summary>
    /// <returns>A bitvector expression representing -this.</returns>
    public Z3BitVecExpr Neg() => Context.Neg(this);

    /// <summary>
    /// Adds this bitvector to a BigInteger value.
    /// </summary>
    /// <param name="other">The BigInteger value to add.</param>
    /// <returns>A bitvector expression representing this + other.</returns>
    public Z3BitVecExpr Add(BigInteger other) => Context.Add(this, other);

    /// <summary>
    /// Subtracts a BigInteger value from this bitvector.
    /// </summary>
    /// <param name="other">The BigInteger value to subtract.</param>
    /// <returns>A bitvector expression representing this - other.</returns>
    public Z3BitVecExpr Sub(BigInteger other) => Context.Sub(this, other);

    /// <summary>
    /// Multiplies this bitvector by a BigInteger value.
    /// </summary>
    /// <param name="other">The BigInteger value to multiply by.</param>
    /// <returns>A bitvector expression representing this * other.</returns>
    public Z3BitVecExpr Mul(BigInteger other) => Context.Mul(this, other);

    /// <summary>
    /// Divides this bitvector by a BigInteger value.
    /// </summary>
    /// <param name="other">The BigInteger value to divide by.</param>
    /// <param name="signed">Whether to perform signed (true) or unsigned (false) division.</param>
    /// <returns>A bitvector expression representing this / other.</returns>
    public Z3BitVecExpr Div(BigInteger other, bool signed = false) =>
        Context.Div(this, other, signed);

    /// <summary>
    /// Computes the remainder of dividing this bitvector by a BigInteger value.
    /// </summary>
    /// <param name="other">The BigInteger value to divide by.</param>
    /// <param name="signed">Whether to perform signed (true) or unsigned (false) remainder.</param>
    /// <returns>A bitvector expression representing this % other.</returns>
    public Z3BitVecExpr Rem(BigInteger other, bool signed = false) =>
        Context.Rem(this, other, signed);

    /// <summary>
    /// Computes the signed modulo of this bitvector with another bitvector expression.
    /// </summary>
    /// <param name="other">The bitvector to compute modulo with.</param>
    /// <returns>A bitvector expression representing signed this mod other.</returns>
    public Z3BitVecExpr SignedMod(Z3BitVecExpr other) => Context.SignedMod(this, other);

    /// <summary>
    /// Computes the signed modulo of this bitvector with a BigInteger value.
    /// </summary>
    /// <param name="other">The BigInteger value to compute modulo with.</param>
    /// <returns>A bitvector expression representing signed this mod other.</returns>
    public Z3BitVecExpr SignedMod(BigInteger other) => Context.SignedMod(this, other);

    /// <summary>
    /// Left-shifts this bitvector by the specified amount.
    /// </summary>
    /// <param name="amount">The bitvector expression specifying shift amount.</param>
    /// <returns>A bitvector expression representing this &lt;&lt; amount.</returns>
    public Z3BitVecExpr Shl(Z3BitVecExpr amount) => Context.Shl(this, amount);

    /// <summary>
    /// Right-shifts this bitvector by the specified amount.
    /// </summary>
    /// <param name="amount">The bitvector expression specifying shift amount.</param>
    /// <param name="signed">Whether to perform arithmetic (true) or logical (false) shift.</param>
    /// <returns>A bitvector expression representing this &gt;&gt; amount.</returns>
    public Z3BitVecExpr Shr(Z3BitVecExpr amount, bool signed = false) =>
        Context.Shr(this, amount, signed);

    /// <summary>
    /// Left-shifts this bitvector by the specified amount.
    /// </summary>
    /// <param name="amount">The BigInteger value specifying shift amount.</param>
    /// <returns>A bitvector expression representing this &lt;&lt; amount.</returns>
    public Z3BitVecExpr Shl(BigInteger amount) => Context.Shl(this, amount);

    /// <summary>
    /// Right-shifts this bitvector by the specified amount.
    /// </summary>
    /// <param name="amount">The BigInteger value specifying shift amount.</param>
    /// <param name="signed">Whether to perform arithmetic (true) or logical (false) shift.</param>
    /// <returns>A bitvector expression representing this &gt;&gt; amount.</returns>
    public Z3BitVecExpr Shr(BigInteger amount, bool signed = false) =>
        Context.Shr(this, amount, signed);

    /// <summary>
    /// Checks whether addition with another bitvector would cause overflow.
    /// </summary>
    /// <param name="other">The bitvector to add.</param>
    /// <param name="signed">Whether to check for signed (true) or unsigned (false) overflow.</param>
    /// <returns>A boolean expression that is true if this + other does not overflow.</returns>
    public Z3BoolExpr AddNoOverflow(Z3BitVecExpr other, bool signed = false) =>
        Context.AddNoOverflow(this, other, signed);

    /// <summary>
    /// Checks whether signed addition with another bitvector would cause underflow.
    /// </summary>
    /// <param name="other">The bitvector to add.</param>
    /// <returns>A boolean expression that is true if signed this + other does not underflow.</returns>
    public Z3BoolExpr SignedAddNoUnderflow(Z3BitVecExpr other) =>
        Context.SignedAddNoUnderflow(this, other);

    /// <summary>
    /// Checks whether signed subtraction of another bitvector would cause overflow.
    /// </summary>
    /// <param name="other">The bitvector to subtract.</param>
    /// <returns>A boolean expression that is true if signed this - other does not overflow.</returns>
    public Z3BoolExpr SignedSubNoOverflow(Z3BitVecExpr other) =>
        Context.SignedSubNoOverflow(this, other);

    /// <summary>
    /// Checks whether subtraction of another bitvector would cause underflow.
    /// </summary>
    /// <param name="other">The bitvector to subtract.</param>
    /// <param name="signed">Whether to check for signed (true) or unsigned (false) underflow.</param>
    /// <returns>A boolean expression that is true if this - other does not underflow.</returns>
    public Z3BoolExpr SubNoUnderflow(Z3BitVecExpr other, bool signed = true) =>
        Context.SubNoUnderflow(this, other, signed);

    /// <summary>
    /// Checks whether multiplication with another bitvector would cause overflow.
    /// </summary>
    /// <param name="other">The bitvector to multiply by.</param>
    /// <param name="signed">Whether to check for signed (true) or unsigned (false) overflow.</param>
    /// <returns>A boolean expression that is true if this * other does not overflow.</returns>
    public Z3BoolExpr MulNoOverflow(Z3BitVecExpr other, bool signed = false) =>
        Context.MulNoOverflow(this, other, signed);

    /// <summary>
    /// Checks whether signed multiplication with another bitvector would cause underflow.
    /// </summary>
    /// <param name="other">The bitvector to multiply by.</param>
    /// <returns>A boolean expression that is true if signed this * other does not underflow.</returns>
    public Z3BoolExpr SignedMulNoUnderflow(Z3BitVecExpr other) =>
        Context.SignedMulNoUnderflow(this, other);

    /// <summary>
    /// Checks whether signed division by another bitvector would cause overflow.
    /// </summary>
    /// <param name="other">The bitvector to divide by.</param>
    /// <returns>A boolean expression that is true if signed this / other does not overflow.</returns>
    public Z3BoolExpr SignedDivNoOverflow(Z3BitVecExpr other) =>
        Context.SignedDivNoOverflow(this, other);

    /// <summary>
    /// Checks whether signed negation would cause overflow.
    /// </summary>
    /// <returns>A boolean expression that is true if signed -this does not overflow.</returns>
    public Z3BoolExpr SignedNegNoOverflow() => Context.SignedNegNoOverflow(this);

    /// <summary>
    /// Checks whether addition with a BigInteger value would cause overflow.
    /// </summary>
    /// <param name="other">The BigInteger value to add.</param>
    /// <param name="signed">Whether to check for signed (true) or unsigned (false) overflow.</param>
    /// <returns>A boolean expression that is true if this + other does not overflow.</returns>
    public Z3BoolExpr AddNoOverflow(BigInteger other, bool signed = false) =>
        Context.AddNoOverflow(this, other, signed);

    /// <summary>
    /// Checks whether signed addition with a BigInteger value would cause underflow.
    /// </summary>
    /// <param name="other">The BigInteger value to add.</param>
    /// <returns>A boolean expression that is true if signed this + other does not underflow.</returns>
    public Z3BoolExpr SignedAddNoUnderflow(BigInteger other) =>
        Context.SignedAddNoUnderflow(this, other);

    /// <summary>
    /// Checks whether signed subtraction of a BigInteger value would cause overflow.
    /// </summary>
    /// <param name="other">The BigInteger value to subtract.</param>
    /// <returns>A boolean expression that is true if signed this - other does not overflow.</returns>
    public Z3BoolExpr SignedSubNoOverflow(BigInteger other) =>
        Context.SignedSubNoOverflow(this, other);

    /// <summary>
    /// Checks whether subtraction of a BigInteger value would cause underflow.
    /// </summary>
    /// <param name="other">The BigInteger value to subtract.</param>
    /// <param name="signed">Whether to check for signed (true) or unsigned (false) underflow.</param>
    /// <returns>A boolean expression that is true if this - other does not underflow.</returns>
    public Z3BoolExpr SubNoUnderflow(BigInteger other, bool signed = true) =>
        Context.SubNoUnderflow(this, other, signed);

    /// <summary>
    /// Checks whether multiplication with a BigInteger value would cause overflow.
    /// </summary>
    /// <param name="other">The BigInteger value to multiply by.</param>
    /// <param name="signed">Whether to check for signed (true) or unsigned (false) overflow.</param>
    /// <returns>A boolean expression that is true if this * other does not overflow.</returns>
    public Z3BoolExpr MulNoOverflow(BigInteger other, bool signed = false) =>
        Context.MulNoOverflow(this, other, signed);

    /// <summary>
    /// Checks whether signed multiplication with a BigInteger value would cause underflow.
    /// </summary>
    /// <param name="other">The BigInteger value to multiply by.</param>
    /// <returns>A boolean expression that is true if signed this * other does not underflow.</returns>
    public Z3BoolExpr SignedMulNoUnderflow(BigInteger other) =>
        Context.SignedMulNoUnderflow(this, other);

    /// <summary>
    /// Checks whether signed division by a BigInteger value would cause overflow.
    /// </summary>
    /// <param name="other">The BigInteger value to divide by.</param>
    /// <returns>A boolean expression that is true if signed this / other does not overflow.</returns>
    public Z3BoolExpr SignedDivNoOverflow(BigInteger other) =>
        Context.SignedDivNoOverflow(this, other);

    /// <summary>
    /// Adds two bitvector expressions using the + operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A bitvector expression representing left + right.</returns>
    public static Z3BitVecExpr operator +(Z3BitVecExpr left, Z3BitVecExpr right) =>
        left.Context.Add(left, right);

    /// <summary>
    /// Subtracts two bitvector expressions using the - operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A bitvector expression representing left - right.</returns>
    public static Z3BitVecExpr operator -(Z3BitVecExpr left, Z3BitVecExpr right) =>
        left.Context.Sub(left, right);

    /// <summary>
    /// Multiplies two bitvector expressions using the * operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A bitvector expression representing left * right.</returns>
    public static Z3BitVecExpr operator *(Z3BitVecExpr left, Z3BitVecExpr right) =>
        left.Context.Mul(left, right);

    /// <summary>
    /// Divides two bitvector expressions using the / operator (unsigned division).
    /// </summary>
    /// <param name="left">The left operand (dividend).</param>
    /// <param name="right">The right operand (divisor).</param>
    /// <returns>A bitvector expression representing left / right.</returns>
    public static Z3BitVecExpr operator /(Z3BitVecExpr left, Z3BitVecExpr right) =>
        left.Context.Div(left, right);

    /// <summary>
    /// Computes remainder of two bitvector expressions using the % operator (unsigned remainder).
    /// </summary>
    /// <param name="left">The left operand (dividend).</param>
    /// <param name="right">The right operand (divisor).</param>
    /// <returns>A bitvector expression representing left % right.</returns>
    public static Z3BitVecExpr operator %(Z3BitVecExpr left, Z3BitVecExpr right) =>
        left.Context.Rem(left, right);

    /// <summary>
    /// Negates a bitvector expression using the unary - operator (two's complement).
    /// </summary>
    /// <param name="operand">The bitvector to negate.</param>
    /// <returns>A bitvector expression representing -operand.</returns>
    public static Z3BitVecExpr operator -(Z3BitVecExpr operand) => operand.Context.Neg(operand);

    /// <summary>
    /// Performs bitwise AND of two bitvector expressions using the &amp; operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A bitvector expression representing left &amp; right.</returns>
    public static Z3BitVecExpr operator &(Z3BitVecExpr left, Z3BitVecExpr right) =>
        left.Context.And(left, right);

    /// <summary>
    /// Performs bitwise OR of two bitvector expressions using the | operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A bitvector expression representing left | right.</returns>
    public static Z3BitVecExpr operator |(Z3BitVecExpr left, Z3BitVecExpr right) =>
        left.Context.Or(left, right);

    /// <summary>
    /// Performs bitwise XOR of two bitvector expressions using the ^ operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A bitvector expression representing left ^ right.</returns>
    public static Z3BitVecExpr operator ^(Z3BitVecExpr left, Z3BitVecExpr right) =>
        left.Context.Xor(left, right);

    /// <summary>
    /// Performs bitwise NOT of a bitvector expression using the ~ operator.
    /// </summary>
    /// <param name="operand">The bitvector to negate bitwise.</param>
    /// <returns>A bitvector expression representing ~operand.</returns>
    public static Z3BitVecExpr operator ~(Z3BitVecExpr operand) => operand.Context.Not(operand);

    /// <summary>
    /// Left-shifts a bitvector expression using the &lt;&lt; operator.
    /// </summary>
    /// <param name="left">The bitvector to shift.</param>
    /// <param name="right">The shift amount.</param>
    /// <returns>A bitvector expression representing left &lt;&lt; right.</returns>
    public static Z3BitVecExpr operator <<(Z3BitVecExpr left, Z3BitVecExpr right) =>
        left.Context.Shl(left, right);

    /// <summary>
    /// Right-shifts a bitvector expression using the &gt;&gt; operator (logical shift).
    /// </summary>
    /// <param name="left">The bitvector to shift.</param>
    /// <param name="right">The shift amount.</param>
    /// <returns>A bitvector expression representing left &gt;&gt; right.</returns>
    public static Z3BitVecExpr operator >>(Z3BitVecExpr left, Z3BitVecExpr right) =>
        left.Context.Shr(left, right);

    /// <summary>
    /// Performs bitwise AND with another bitvector expression.
    /// </summary>
    /// <param name="other">The bitvector to AND with.</param>
    /// <returns>A bitvector expression representing this &amp; other.</returns>
    public Z3BitVecExpr And(Z3BitVecExpr other) => Context.And(this, other);

    /// <summary>
    /// Performs bitwise OR with another bitvector expression.
    /// </summary>
    /// <param name="other">The bitvector to OR with.</param>
    /// <returns>A bitvector expression representing this | other.</returns>
    public Z3BitVecExpr Or(Z3BitVecExpr other) => Context.Or(this, other);

    /// <summary>
    /// Performs bitwise XOR with another bitvector expression.
    /// </summary>
    /// <param name="other">The bitvector to XOR with.</param>
    /// <returns>A bitvector expression representing this ^ other.</returns>
    public Z3BitVecExpr Xor(Z3BitVecExpr other) => Context.Xor(this, other);

    /// <summary>
    /// Performs bitwise NOT of this bitvector.
    /// </summary>
    /// <returns>A bitvector expression representing ~this.</returns>
    public Z3BitVecExpr Not() => Context.Not(this);

    /// <summary>
    /// Performs bitwise AND with a BigInteger value.
    /// </summary>
    /// <param name="other">The BigInteger value to AND with.</param>
    /// <returns>A bitvector expression representing this &amp; other.</returns>
    public Z3BitVecExpr And(BigInteger other) => Context.And(this, other);

    /// <summary>
    /// Performs bitwise OR with a BigInteger value.
    /// </summary>
    /// <param name="other">The BigInteger value to OR with.</param>
    /// <returns>A bitvector expression representing this | other.</returns>
    public Z3BitVecExpr Or(BigInteger other) => Context.Or(this, other);

    /// <summary>
    /// Performs bitwise XOR with a BigInteger value.
    /// </summary>
    /// <param name="other">The BigInteger value to XOR with.</param>
    /// <returns>A bitvector expression representing this ^ other.</returns>
    public Z3BitVecExpr Xor(BigInteger other) => Context.Xor(this, other);

    /// <summary>
    /// Compares two bitvector expressions using the &lt; operator (unsigned comparison).
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A boolean expression representing left &lt; right.</returns>
    public static Z3BoolExpr operator <(Z3BitVecExpr left, Z3BitVecExpr right) =>
        left.Context.Lt(left, right);

    /// <summary>
    /// Compares two bitvector expressions using the &lt;= operator (unsigned comparison).
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A boolean expression representing left &lt;= right.</returns>
    public static Z3BoolExpr operator <=(Z3BitVecExpr left, Z3BitVecExpr right) =>
        left.Context.Le(left, right);

    /// <summary>
    /// Compares two bitvector expressions using the &gt; operator (unsigned comparison).
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A boolean expression representing left &gt; right.</returns>
    public static Z3BoolExpr operator >(Z3BitVecExpr left, Z3BitVecExpr right) =>
        left.Context.Gt(left, right);

    /// <summary>
    /// Compares two bitvector expressions using the &gt;= operator (unsigned comparison).
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A boolean expression representing left &gt;= right.</returns>
    public static Z3BoolExpr operator >=(Z3BitVecExpr left, Z3BitVecExpr right) =>
        left.Context.Ge(left, right);

    /// <summary>
    /// Adds a bitvector expression and a BigInteger using the + operator.
    /// </summary>
    /// <param name="left">The bitvector operand.</param>
    /// <param name="right">The BigInteger operand.</param>
    /// <returns>A bitvector expression representing left + right.</returns>
    public static Z3BitVecExpr operator +(Z3BitVecExpr left, BigInteger right) =>
        left.Context.Add(left, right);

    /// <summary>
    /// Subtracts a BigInteger from a bitvector expression using the - operator.
    /// </summary>
    /// <param name="left">The bitvector operand.</param>
    /// <param name="right">The BigInteger operand.</param>
    /// <returns>A bitvector expression representing left - right.</returns>
    public static Z3BitVecExpr operator -(Z3BitVecExpr left, BigInteger right) =>
        left.Context.Sub(left, right);

    /// <summary>
    /// Multiplies a bitvector expression and a BigInteger using the * operator.
    /// </summary>
    /// <param name="left">The bitvector operand.</param>
    /// <param name="right">The BigInteger operand.</param>
    /// <returns>A bitvector expression representing left * right.</returns>
    public static Z3BitVecExpr operator *(Z3BitVecExpr left, BigInteger right) =>
        left.Context.Mul(left, right);

    /// <summary>
    /// Divides a bitvector expression by a BigInteger using the / operator.
    /// </summary>
    /// <param name="left">The bitvector operand.</param>
    /// <param name="right">The BigInteger operand.</param>
    /// <returns>A bitvector expression representing left / right.</returns>
    public static Z3BitVecExpr operator /(Z3BitVecExpr left, BigInteger right) =>
        left.Context.Div(left, right);

    /// <summary>
    /// Computes remainder of a bitvector expression and a BigInteger using the % operator.
    /// </summary>
    /// <param name="left">The bitvector operand.</param>
    /// <param name="right">The BigInteger operand.</param>
    /// <returns>A bitvector expression representing left % right.</returns>
    public static Z3BitVecExpr operator %(Z3BitVecExpr left, BigInteger right) =>
        left.Context.Rem(left, right);

    /// <summary>
    /// Adds a BigInteger and a bitvector expression using the + operator.
    /// </summary>
    /// <param name="left">The BigInteger operand.</param>
    /// <param name="right">The bitvector operand.</param>
    /// <returns>A bitvector expression representing left + right.</returns>
    public static Z3BitVecExpr operator +(BigInteger left, Z3BitVecExpr right) =>
        right.Context.Add(left, right);

    /// <summary>
    /// Subtracts a bitvector expression from a BigInteger using the - operator.
    /// </summary>
    /// <param name="left">The BigInteger operand.</param>
    /// <param name="right">The bitvector operand.</param>
    /// <returns>A bitvector expression representing left - right.</returns>
    public static Z3BitVecExpr operator -(BigInteger left, Z3BitVecExpr right) =>
        right.Context.Sub(left, right);

    /// <summary>
    /// Multiplies a BigInteger and a bitvector expression using the * operator.
    /// </summary>
    /// <param name="left">The BigInteger operand.</param>
    /// <param name="right">The bitvector operand.</param>
    /// <returns>A bitvector expression representing left * right.</returns>
    public static Z3BitVecExpr operator *(BigInteger left, Z3BitVecExpr right) =>
        right.Context.Mul(left, right);

    /// <summary>
    /// Divides a BigInteger by a bitvector expression using the / operator.
    /// </summary>
    /// <param name="left">The BigInteger operand.</param>
    /// <param name="right">The bitvector operand.</param>
    /// <returns>A bitvector expression representing left / right.</returns>
    public static Z3BitVecExpr operator /(BigInteger left, Z3BitVecExpr right) =>
        right.Context.Div(left, right);

    /// <summary>
    /// Computes remainder of a BigInteger and a bitvector expression using the % operator.
    /// </summary>
    /// <param name="left">The BigInteger operand.</param>
    /// <param name="right">The bitvector operand.</param>
    /// <returns>A bitvector expression representing left % right.</returns>
    public static Z3BitVecExpr operator %(BigInteger left, Z3BitVecExpr right) =>
        right.Context.Rem(left, right);

    /// <summary>
    /// Performs bitwise AND of a bitvector expression and a BigInteger using the &amp; operator.
    /// </summary>
    /// <param name="left">The bitvector operand.</param>
    /// <param name="right">The BigInteger operand.</param>
    /// <returns>A bitvector expression representing left &amp; right.</returns>
    public static Z3BitVecExpr operator &(Z3BitVecExpr left, BigInteger right) =>
        left.Context.And(left, right);

    /// <summary>
    /// Performs bitwise OR of a bitvector expression and a BigInteger using the | operator.
    /// </summary>
    /// <param name="left">The bitvector operand.</param>
    /// <param name="right">The BigInteger operand.</param>
    /// <returns>A bitvector expression representing left | right.</returns>
    public static Z3BitVecExpr operator |(Z3BitVecExpr left, BigInteger right) =>
        left.Context.Or(left, right);

    /// <summary>
    /// Performs bitwise XOR of a bitvector expression and a BigInteger using the ^ operator.
    /// </summary>
    /// <param name="left">The bitvector operand.</param>
    /// <param name="right">The BigInteger operand.</param>
    /// <returns>A bitvector expression representing left ^ right.</returns>
    public static Z3BitVecExpr operator ^(Z3BitVecExpr left, BigInteger right) =>
        left.Context.Xor(left, right);

    /// <summary>
    /// Performs bitwise AND of a BigInteger and a bitvector expression using the &amp; operator.
    /// </summary>
    /// <param name="left">The BigInteger operand.</param>
    /// <param name="right">The bitvector operand.</param>
    /// <returns>A bitvector expression representing left &amp; right.</returns>
    public static Z3BitVecExpr operator &(BigInteger left, Z3BitVecExpr right) =>
        right.Context.And(left, right);

    /// <summary>
    /// Performs bitwise OR of a BigInteger and a bitvector expression using the | operator.
    /// </summary>
    /// <param name="left">The BigInteger operand.</param>
    /// <param name="right">The bitvector operand.</param>
    /// <returns>A bitvector expression representing left | right.</returns>
    public static Z3BitVecExpr operator |(BigInteger left, Z3BitVecExpr right) =>
        right.Context.Or(left, right);

    /// <summary>
    /// Performs bitwise XOR of a BigInteger and a bitvector expression using the ^ operator.
    /// </summary>
    /// <param name="left">The BigInteger operand.</param>
    /// <param name="right">The bitvector operand.</param>
    /// <returns>A bitvector expression representing left ^ right.</returns>
    public static Z3BitVecExpr operator ^(BigInteger left, Z3BitVecExpr right) =>
        right.Context.Xor(left, right);

    /// <summary>
    /// Left-shifts a bitvector expression by a BigInteger amount using the &lt;&lt; operator.
    /// </summary>
    /// <param name="left">The bitvector to shift.</param>
    /// <param name="right">The shift amount.</param>
    /// <returns>A bitvector expression representing left &lt;&lt; right.</returns>
    public static Z3BitVecExpr operator <<(Z3BitVecExpr left, BigInteger right) =>
        left.Context.Shl(left, right);

    /// <summary>
    /// Right-shifts a bitvector expression by a BigInteger amount using the &gt;&gt; operator.
    /// </summary>
    /// <param name="left">The bitvector to shift.</param>
    /// <param name="right">The shift amount.</param>
    /// <returns>A bitvector expression representing left &gt;&gt; right.</returns>
    public static Z3BitVecExpr operator >>(Z3BitVecExpr left, BigInteger right) =>
        left.Context.Shr(left, right);

    /// <summary>
    /// Compares a bitvector expression and a BigInteger using the &lt; operator (unsigned comparison).
    /// </summary>
    /// <param name="left">The bitvector operand.</param>
    /// <param name="right">The BigInteger operand.</param>
    /// <returns>A boolean expression representing left &lt; right.</returns>
    public static Z3BoolExpr operator <(Z3BitVecExpr left, BigInteger right) =>
        left.Context.Lt(left, right);

    /// <summary>
    /// Compares a bitvector expression and a BigInteger using the &lt;= operator (unsigned comparison).
    /// </summary>
    /// <param name="left">The bitvector operand.</param>
    /// <param name="right">The BigInteger operand.</param>
    /// <returns>A boolean expression representing left &lt;= right.</returns>
    public static Z3BoolExpr operator <=(Z3BitVecExpr left, BigInteger right) =>
        left.Context.Le(left, right);

    /// <summary>
    /// Compares a bitvector expression and a BigInteger using the &gt; operator (unsigned comparison).
    /// </summary>
    /// <param name="left">The bitvector operand.</param>
    /// <param name="right">The BigInteger operand.</param>
    /// <returns>A boolean expression representing left &gt; right.</returns>
    public static Z3BoolExpr operator >(Z3BitVecExpr left, BigInteger right) =>
        left.Context.Gt(left, right);

    /// <summary>
    /// Compares a bitvector expression and a BigInteger using the &gt;= operator (unsigned comparison).
    /// </summary>
    /// <param name="left">The bitvector operand.</param>
    /// <param name="right">The BigInteger operand.</param>
    /// <returns>A boolean expression representing left &gt;= right.</returns>
    public static Z3BoolExpr operator >=(Z3BitVecExpr left, BigInteger right) =>
        left.Context.Ge(left, right);

    /// <summary>
    /// Compares a BigInteger and a bitvector expression using the &lt; operator (unsigned comparison).
    /// </summary>
    /// <param name="left">The BigInteger operand.</param>
    /// <param name="right">The bitvector operand.</param>
    /// <returns>A boolean expression representing left &lt; right.</returns>
    public static Z3BoolExpr operator <(BigInteger left, Z3BitVecExpr right) =>
        right.Context.Lt(left, right);

    /// <summary>
    /// Compares a BigInteger and a bitvector expression using the &lt;= operator (unsigned comparison).
    /// </summary>
    /// <param name="left">The BigInteger operand.</param>
    /// <param name="right">The bitvector operand.</param>
    /// <returns>A boolean expression representing left &lt;= right.</returns>
    public static Z3BoolExpr operator <=(BigInteger left, Z3BitVecExpr right) =>
        right.Context.Le(left, right);

    /// <summary>
    /// Compares a BigInteger and a bitvector expression using the &gt; operator (unsigned comparison).
    /// </summary>
    /// <param name="left">The BigInteger operand.</param>
    /// <param name="right">The bitvector operand.</param>
    /// <returns>A boolean expression representing left &gt; right.</returns>
    public static Z3BoolExpr operator >(BigInteger left, Z3BitVecExpr right) =>
        right.Context.Gt(left, right);

    /// <summary>
    /// Compares a BigInteger and a bitvector expression using the &gt;= operator (unsigned comparison).
    /// </summary>
    /// <param name="left">The BigInteger operand.</param>
    /// <param name="right">The bitvector operand.</param>
    /// <returns>A boolean expression representing left &gt;= right.</returns>
    public static Z3BoolExpr operator >=(BigInteger left, Z3BitVecExpr right) =>
        right.Context.Ge(left, right);

    /// <summary>
    /// Checks equality between a bitvector expression and a BigInteger using the == operator.
    /// </summary>
    /// <param name="left">The bitvector operand.</param>
    /// <param name="right">The BigInteger operand.</param>
    /// <returns>A boolean expression representing left == right.</returns>
    public static Z3BoolExpr operator ==(Z3BitVecExpr left, BigInteger right) =>
        left.Context.Eq(left, right);

    /// <summary>
    /// Checks equality between a BigInteger and a bitvector expression using the == operator.
    /// </summary>
    /// <param name="left">The BigInteger operand.</param>
    /// <param name="right">The bitvector operand.</param>
    /// <returns>A boolean expression representing left == right.</returns>
    public static Z3BoolExpr operator ==(BigInteger left, Z3BitVecExpr right) =>
        right.Context.Eq(left, right);

    /// <summary>
    /// Checks inequality between a bitvector expression and a BigInteger using the != operator.
    /// </summary>
    /// <param name="left">The bitvector operand.</param>
    /// <param name="right">The BigInteger operand.</param>
    /// <returns>A boolean expression representing left != right.</returns>
    public static Z3BoolExpr operator !=(Z3BitVecExpr left, BigInteger right) =>
        left.Context.Neq(left, right);

    /// <summary>
    /// Checks inequality between a BigInteger and a bitvector expression using the != operator.
    /// </summary>
    /// <param name="left">The BigInteger operand.</param>
    /// <param name="right">The bitvector operand.</param>
    /// <returns>A boolean expression representing left != right.</returns>
    public static Z3BoolExpr operator !=(BigInteger left, Z3BitVecExpr right) =>
        right.Context.Neq(left, right);
}
