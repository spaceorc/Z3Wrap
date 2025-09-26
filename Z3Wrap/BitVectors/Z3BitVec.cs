using System.Numerics;
using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Extensions;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.BitVectors;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
/// <summary>
/// Represents a compile-time size-validated bitvector expression in Z3 with comprehensive bitwise and arithmetic operations.
/// Provides type-safe operations where size mismatches are caught at compile time.
/// Supports both unsigned and signed operations, bit manipulation, and conversion to other numeric types.
/// </summary>
/// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
public sealed class Z3BitVec<TSize> : Z3NumericExpr, IZ3ExprType<Z3BitVec<TSize>>
    where TSize : ISize
{
    /// <summary>
    /// Gets the bit width (size in bits) of this bitvector expression, known at compile time.
    /// </summary>
    public static uint Size => TSize.Size;

    private Z3BitVec(Z3Context context, IntPtr handle)
        : base(context, handle) { }

    static Z3BitVec<TSize> IZ3ExprType<Z3BitVec<TSize>>.Create(Z3Context context, IntPtr handle) =>
        new(context, handle);

    static IntPtr IZ3ExprType<Z3BitVec<TSize>>.GetSort(Z3Context context) =>
        SafeNativeMethods.Z3MkBvSort(context.Handle, TSize.Size);

    /// <summary>
    /// Creates a bitvector representing zero with the compile-time specified bit width.
    /// </summary>
    public static Z3BitVec<TSize> Zero => Z3Context.Current.BitVec(BitVec<TSize>.Zero);

    /// <summary>
    /// Creates a bitvector representing one with the compile-time specified bit width.
    /// </summary>
    public static Z3BitVec<TSize> One => Z3Context.Current.BitVec(BitVec<TSize>.One);

    /// <summary>
    /// Creates a bitvector with the maximum possible value (all bits set to 1).
    /// </summary>
    public static Z3BitVec<TSize> Max => Z3Context.Current.BitVec(BitVec<TSize>.Max);

    /// <summary>
    /// Creates a bitvector with only the sign bit set (most significant bit).
    /// </summary>
    public static Z3BitVec<TSize> SignBit => Z3Context.Current.BitVec(BitVec<TSize>.SignBit);

    /// <summary>
    /// Creates a bitvector with all bits set to 1 (alias for Max).
    /// </summary>
    public static Z3BitVec<TSize> AllOnes => Max;

    /// <summary>
    /// Implicitly converts a BigInteger value to a Z3BitVec using the current thread-local context.
    /// </summary>
    /// <param name="value">The BigInteger value to convert.</param>
    /// <returns>A Z3BitVec representing the bitvector constant.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no current context is set.</exception>
    public static implicit operator Z3BitVec<TSize>(BigInteger value) =>
        Z3Context.Current.BitVec<TSize>(value);

    /// <summary>
    /// Implicitly converts an integer value to a Z3BitVec using the current thread-local context.
    /// </summary>
    /// <param name="value">The integer value to convert.</param>
    /// <returns>A Z3BitVec representing the bitvector constant.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no current context is set.</exception>
    public static implicit operator Z3BitVec<TSize>(int value) =>
        Z3Context.Current.BitVec<TSize>(value);

    /// <summary>
    /// Implicitly converts an unsigned integer value to a Z3BitVec using the current thread-local context.
    /// </summary>
    /// <param name="value">The unsigned integer value to convert.</param>
    /// <returns>A Z3BitVec representing the bitvector constant.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no current context is set.</exception>
    public static implicit operator Z3BitVec<TSize>(uint value) =>
        Z3Context.Current.BitVec<TSize>(value);

    /// <summary>
    /// Implicitly converts a long integer value to a Z3BitVec using the current thread-local context.
    /// </summary>
    /// <param name="value">The long integer value to convert.</param>
    /// <returns>A Z3BitVec representing the bitvector constant.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no current context is set.</exception>
    public static implicit operator Z3BitVec<TSize>(long value) =>
        Z3Context.Current.BitVec<TSize>(value);

    /// <summary>
    /// Implicitly converts an unsigned long integer value to a Z3BitVec using the current thread-local context.
    /// </summary>
    /// <param name="value">The unsigned long integer value to convert.</param>
    /// <returns>A Z3BitVec representing the bitvector constant.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no current context is set.</exception>
    public static implicit operator Z3BitVec<TSize>(ulong value) =>
        Z3Context.Current.BitVec<TSize>(value);

    /// <summary>
    /// Implicitly converts a typed BitVec value to a Z3BitVec using the current thread-local context.
    /// </summary>
    /// <param name="value">The BitVec value to convert.</param>
    /// <returns>A Z3BitVec representing the bitvector constant.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no current context is set.</exception>
    public static implicit operator Z3BitVec<TSize>(BitVec<TSize> value) =>
        Z3Context.Current.BitVec(value);

    /// <summary>
    /// Returns a string representation of this bitvector expression including its bit width.
    /// </summary>
    /// <returns>A string in the format "Z3BitVec[size](expression)".</returns>
    public override string ToString() => $"Z3BitVec[{Size}]({base.ToString()})";

    /// <summary>
    /// Extracts a compile-time size-validated range of bits from this bitvector starting at the specified bit.
    /// </summary>
    /// <typeparam name="TOutputSize">The output size specification implementing ISize for compile-time validation.</typeparam>
    /// <param name="startBit">The starting bit index (inclusive).</param>
    /// <returns>A type-safe Z3 bitvector expression containing the extracted bits.</returns>
    public Z3BitVec<TOutputSize> Extract<TOutputSize>(uint startBit)
        where TOutputSize : ISize => Context.Extract<TSize, TOutputSize>(this, startBit);

    /// <summary>
    /// Resizes this bitvector to a compile-time validated target size by truncating or extending.
    /// </summary>
    /// <typeparam name="TOutputSize">The target size specification implementing ISize for compile-time validation.</typeparam>
    /// <param name="signed">Whether to sign-extend when growing or truncate when shrinking.</param>
    /// <returns>A type-safe Z3 bitvector expression with the target size.</returns>
    public Z3BitVec<TOutputSize> Resize<TOutputSize>(bool signed = false)
        where TOutputSize : ISize => Context.Resize<TSize, TOutputSize>(this, signed);

    /// <summary>
    /// Repeats this bitvector to create a larger compile-time size-validated bitvector expression.
    /// </summary>
    /// <typeparam name="TOutputSize">The target size specification implementing ISize for compile-time validation.</typeparam>
    /// <returns>A type-safe Z3 bitvector expression containing the repeated pattern.</returns>
    public Z3BitVec<TOutputSize> Repeat<TOutputSize>()
        where TOutputSize : ISize => Context.Repeat<TSize, TOutputSize>(this);

    /// <summary>
    /// Converts this bitvector to an integer expression.
    /// </summary>
    /// <param name="signed">Whether to interpret the bitvector as signed (true) or unsigned (false).</param>
    /// <returns>An integer expression representing this bitvector value.</returns>
    public Z3IntExpr ToInt(bool signed = false) => Context.ToInt(this, signed);

    /// <summary>
    /// Creates a less-than comparison with another bitvector expression of the same size.
    /// </summary>
    /// <param name="other">The bitvector to compare with.</param>
    /// <param name="signed">Whether to perform signed (true) or unsigned (false) comparison.</param>
    /// <returns>A boolean expression representing this &lt; other.</returns>
    public Z3BoolExpr Lt(Z3BitVec<TSize> other, bool signed = false) =>
        Context.Lt(this, other, signed);

    /// <summary>
    /// Creates a less-than-or-equal comparison with another bitvector expression of the same size.
    /// </summary>
    /// <param name="other">The bitvector to compare with.</param>
    /// <param name="signed">Whether to perform signed (true) or unsigned (false) comparison.</param>
    /// <returns>A boolean expression representing this &lt;= other.</returns>
    public Z3BoolExpr Le(Z3BitVec<TSize> other, bool signed = false) =>
        Context.Le(this, other, signed);

    /// <summary>
    /// Creates a greater-than comparison with another bitvector expression of the same size.
    /// </summary>
    /// <param name="other">The bitvector to compare with.</param>
    /// <param name="signed">Whether to perform signed (true) or unsigned (false) comparison.</param>
    /// <returns>A boolean expression representing this &gt; other.</returns>
    public Z3BoolExpr Gt(Z3BitVec<TSize> other, bool signed = false) =>
        Context.Gt(this, other, signed);

    /// <summary>
    /// Creates a greater-than-or-equal comparison with another bitvector expression of the same size.
    /// </summary>
    /// <param name="other">The bitvector to compare with.</param>
    /// <param name="signed">Whether to perform signed (true) or unsigned (false) comparison.</param>
    /// <returns>A boolean expression representing this &gt;= other.</returns>
    public Z3BoolExpr Ge(Z3BitVec<TSize> other, bool signed = false) =>
        Context.Ge(this, other, signed);

    /// <summary>
    /// Adds this bitvector to another bitvector expression of the same size.
    /// </summary>
    /// <param name="other">The bitvector to add.</param>
    /// <returns>A bitvector expression representing this + other.</returns>
    public Z3BitVec<TSize> Add(Z3BitVec<TSize> other) => Context.Add(this, other);

    /// <summary>
    /// Subtracts another bitvector expression of the same size from this bitvector.
    /// </summary>
    /// <param name="other">The bitvector to subtract.</param>
    /// <returns>A bitvector expression representing this - other.</returns>
    public Z3BitVec<TSize> Sub(Z3BitVec<TSize> other) => Context.Sub(this, other);

    /// <summary>
    /// Multiplies this bitvector by another bitvector expression of the same size.
    /// </summary>
    /// <param name="other">The bitvector to multiply by.</param>
    /// <returns>A bitvector expression representing this * other.</returns>
    public Z3BitVec<TSize> Mul(Z3BitVec<TSize> other) => Context.Mul(this, other);

    /// <summary>
    /// Divides this bitvector by another bitvector expression of the same size.
    /// </summary>
    /// <param name="other">The bitvector to divide by.</param>
    /// <param name="signed">Whether to perform signed (true) or unsigned (false) division.</param>
    /// <returns>A bitvector expression representing this / other.</returns>
    public Z3BitVec<TSize> Div(Z3BitVec<TSize> other, bool signed = false) =>
        Context.Div(this, other, signed);

    /// <summary>
    /// Computes the remainder of dividing this bitvector by another bitvector expression of the same size.
    /// </summary>
    /// <param name="other">The bitvector to divide by.</param>
    /// <param name="signed">Whether to perform signed (true) or unsigned (false) remainder.</param>
    /// <returns>A bitvector expression representing this % other.</returns>
    public Z3BitVec<TSize> Rem(Z3BitVec<TSize> other, bool signed = false) =>
        Context.Rem(this, other, signed);

    /// <summary>
    /// Negates this bitvector (computes two's complement).
    /// </summary>
    /// <returns>A bitvector expression representing -this.</returns>
    public Z3BitVec<TSize> Neg() => Context.Neg(this);

    /// <summary>
    /// Computes the signed modulo of this bitvector with another bitvector expression of the same size.
    /// </summary>
    /// <param name="other">The bitvector to compute modulo with.</param>
    /// <returns>A bitvector expression representing signed this mod other.</returns>
    public Z3BitVec<TSize> SignedMod(Z3BitVec<TSize> other) => Context.SignedMod(this, other);

    /// <summary>
    /// Performs bitwise AND with another bitvector expression of the same size.
    /// </summary>
    /// <param name="other">The bitvector to AND with.</param>
    /// <returns>A bitvector expression representing this &amp; other.</returns>
    public Z3BitVec<TSize> And(Z3BitVec<TSize> other) => Context.And(this, other);

    /// <summary>
    /// Performs bitwise OR with another bitvector expression of the same size.
    /// </summary>
    /// <param name="other">The bitvector to OR with.</param>
    /// <returns>A bitvector expression representing this | other.</returns>
    public Z3BitVec<TSize> Or(Z3BitVec<TSize> other) => Context.Or(this, other);

    /// <summary>
    /// Performs bitwise XOR with another bitvector expression of the same size.
    /// </summary>
    /// <param name="other">The bitvector to XOR with.</param>
    /// <returns>A bitvector expression representing this ^ other.</returns>
    public Z3BitVec<TSize> Xor(Z3BitVec<TSize> other) => Context.Xor(this, other);

    /// <summary>
    /// Performs bitwise NOT of this bitvector.
    /// </summary>
    /// <returns>A bitvector expression representing ~this.</returns>
    public Z3BitVec<TSize> Not() => Context.Not(this);

    /// <summary>
    /// Left-shifts this bitvector by the specified amount.
    /// </summary>
    /// <param name="amount">The bitvector expression specifying shift amount.</param>
    /// <returns>A bitvector expression representing this &lt;&lt; amount.</returns>
    public Z3BitVec<TSize> Shl(Z3BitVec<TSize> amount) => Context.Shl(this, amount);

    /// <summary>
    /// Right-shifts this bitvector by the specified amount.
    /// </summary>
    /// <param name="amount">The bitvector expression specifying shift amount.</param>
    /// <param name="signed">Whether to perform arithmetic (true) or logical (false) shift.</param>
    /// <returns>A bitvector expression representing this &gt;&gt; amount.</returns>
    public Z3BitVec<TSize> Shr(Z3BitVec<TSize> amount, bool signed = false) =>
        Context.Shr(this, amount, signed);

    /// <summary>
    /// Checks equality between a bitvector expression and a BigInteger using the == operator.
    /// </summary>
    /// <param name="left">The bitvector operand.</param>
    /// <param name="right">The BigInteger operand.</param>
    /// <returns>A boolean expression representing left == right.</returns>
    public static Z3BoolExpr operator ==(Z3BitVec<TSize> left, BigInteger right) =>
        left.Context.Eq(left, right);

    /// <summary>
    /// Checks equality between a BigInteger and a bitvector expression using the == operator.
    /// </summary>
    /// <param name="left">The BigInteger operand.</param>
    /// <param name="right">The bitvector operand.</param>
    /// <returns>A boolean expression representing left == right.</returns>
    public static Z3BoolExpr operator ==(BigInteger left, Z3BitVec<TSize> right) =>
        right.Context.Eq(left, right);

    /// <summary>
    /// Checks inequality between a bitvector expression and a BigInteger using the != operator.
    /// </summary>
    /// <param name="left">The bitvector operand.</param>
    /// <param name="right">The BigInteger operand.</param>
    /// <returns>A boolean expression representing left != right.</returns>
    public static Z3BoolExpr operator !=(Z3BitVec<TSize> left, BigInteger right) =>
        left.Context.Neq(left, right);

    /// <summary>
    /// Checks inequality between a BigInteger and a bitvector expression using the != operator.
    /// </summary>
    /// <param name="left">The BigInteger operand.</param>
    /// <param name="right">The bitvector operand.</param>
    /// <returns>A boolean expression representing left != right.</returns>
    public static Z3BoolExpr operator !=(BigInteger left, Z3BitVec<TSize> right) =>
        right.Context.Neq(left, right);

    // === Arithmetic Operators ===

    /// <summary>
    /// Adds two bitvector expressions using the + operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A bitvector expression representing left + right.</returns>
    public static Z3BitVec<TSize> operator +(Z3BitVec<TSize> left, Z3BitVec<TSize> right) =>
        left.Add(right);

    /// <summary>
    /// Subtracts two bitvector expressions using the - operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A bitvector expression representing left - right.</returns>
    public static Z3BitVec<TSize> operator -(Z3BitVec<TSize> left, Z3BitVec<TSize> right) =>
        left.Sub(right);

    /// <summary>
    /// Multiplies two bitvector expressions using the * operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A bitvector expression representing left * right.</returns>
    public static Z3BitVec<TSize> operator *(Z3BitVec<TSize> left, Z3BitVec<TSize> right) =>
        left.Mul(right);

    /// <summary>
    /// Divides two bitvector expressions using the / operator (unsigned division).
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A bitvector expression representing left / right.</returns>
    public static Z3BitVec<TSize> operator /(Z3BitVec<TSize> left, Z3BitVec<TSize> right) =>
        left.Div(right);

    /// <summary>
    /// Computes the remainder of two bitvector expressions using the % operator (unsigned remainder).
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A bitvector expression representing left % right.</returns>
    public static Z3BitVec<TSize> operator %(Z3BitVec<TSize> left, Z3BitVec<TSize> right) =>
        left.Rem(right);

    /// <summary>
    /// Negates a bitvector expression using the unary - operator (two's complement).
    /// </summary>
    /// <param name="operand">The operand to negate.</param>
    /// <returns>A bitvector expression representing -operand.</returns>
    public static Z3BitVec<TSize> operator -(Z3BitVec<TSize> operand) => operand.Neg();

    // === Bitwise Operators ===

    /// <summary>
    /// Performs bitwise AND between two bitvector expressions using the & operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A bitvector expression representing left & right.</returns>
    public static Z3BitVec<TSize> operator &(Z3BitVec<TSize> left, Z3BitVec<TSize> right) =>
        left.And(right);

    /// <summary>
    /// Performs bitwise OR between two bitvector expressions using the | operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A bitvector expression representing left | right.</returns>
    public static Z3BitVec<TSize> operator |(Z3BitVec<TSize> left, Z3BitVec<TSize> right) =>
        left.Or(right);

    /// <summary>
    /// Performs bitwise XOR between two bitvector expressions using the ^ operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A bitvector expression representing left ^ right.</returns>
    public static Z3BitVec<TSize> operator ^(Z3BitVec<TSize> left, Z3BitVec<TSize> right) =>
        left.Xor(right);

    /// <summary>
    /// Performs bitwise NOT of a bitvector expression using the ~ operator.
    /// </summary>
    /// <param name="operand">The operand to negate.</param>
    /// <returns>A bitvector expression representing ~operand.</returns>
    public static Z3BitVec<TSize> operator ~(Z3BitVec<TSize> operand) => operand.Not();

    /// <summary>
    /// Left-shifts a bitvector expression using the << operator.
    /// </summary>
    /// <param name="left">The operand to shift.</param>
    /// <param name="right">The shift amount.</param>
    /// <returns>A bitvector expression representing left << right.</returns>
    public static Z3BitVec<TSize> operator <<(Z3BitVec<TSize> left, Z3BitVec<TSize> right) =>
        left.Shl(right);

    /// <summary>
    /// Right-shifts a bitvector expression using the >> operator (logical shift).
    /// </summary>
    /// <param name="left">The operand to shift.</param>
    /// <param name="right">The shift amount.</param>
    /// <returns>A bitvector expression representing left >> right.</returns>
    public static Z3BitVec<TSize> operator >>(Z3BitVec<TSize> left, Z3BitVec<TSize> right) =>
        left.Shr(right);

    // === Comparison Operators ===

    /// <summary>
    /// Compares two bitvector expressions for equality using the == operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A boolean expression representing left == right.</returns>
    public static Z3BoolExpr operator ==(Z3BitVec<TSize> left, Z3BitVec<TSize> right) =>
        left.Context.Eq(left, right);

    /// <summary>
    /// Compares two bitvector expressions for inequality using the != operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A boolean expression representing left != right.</returns>
    public static Z3BoolExpr operator !=(Z3BitVec<TSize> left, Z3BitVec<TSize> right) =>
        left.Context.Neq(left, right);

    /// <summary>
    /// Compares two bitvector expressions using the < operator (unsigned comparison).
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A boolean expression representing left < right.</returns>
    public static Z3BoolExpr operator <(Z3BitVec<TSize> left, Z3BitVec<TSize> right) =>
        left.Lt(right);

    /// <summary>
    /// Compares two bitvector expressions using the <= operator (unsigned comparison).
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A boolean expression representing left <= right.</returns>
    public static Z3BoolExpr operator <=(Z3BitVec<TSize> left, Z3BitVec<TSize> right) =>
        left.Le(right);

    /// <summary>
    /// Compares two bitvector expressions using the > operator (unsigned comparison).
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A boolean expression representing left > right.</returns>
    public static Z3BoolExpr operator >(Z3BitVec<TSize> left, Z3BitVec<TSize> right) =>
        left.Gt(right);

    /// <summary>
    /// Compares two bitvector expressions using the >= operator (unsigned comparison).
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A boolean expression representing left >= right.</returns>
    public static Z3BoolExpr operator >=(Z3BitVec<TSize> left, Z3BitVec<TSize> right) =>
        left.Ge(right);
}
#pragma warning restore CS0660, CS0661
