using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Expressions.Numerics;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Spaceorc.Z3Wrap.Expressions.BitVectors;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)

/// <summary>
/// Represents bit-vector expression with fixed bit width for Z3 solving.
/// </summary>
/// <typeparam name="TSize">The bit-vector size type.</typeparam>
public sealed class BvExpr<TSize> : Z3Expr, INumericExpr, IExprType<BvExpr<TSize>>
    where TSize : ISize
{
    /// <summary>
    /// Gets the bit-vector width in bits.
    /// </summary>
    public static uint Size => TSize.Size;

    private BvExpr(Z3Context context, IntPtr handle)
        : base(context, handle) { }

    static BvExpr<TSize> IExprType<BvExpr<TSize>>.Create(Z3Context context, IntPtr handle) => new(context, handle);

    static IntPtr IExprType<BvExpr<TSize>>.Sort(Z3Context context) =>
        context.Library.Z3MkBvSort(context.Handle, TSize.Size);

    /// <summary>
    /// Returns a string representation of this bit-vector expression.
    /// </summary>
    /// <returns>String representation of the expression.</returns>
    public override string ToString() => $"Z3BitVec[{Size}]({base.ToString()})";

    /// <summary>
    /// Implicit conversion from BigInteger to bit-vector expression.
    /// </summary>
    /// <param name="value">BigInteger value to convert.</param>
    /// <returns>Bit-vector expression.</returns>
    public static implicit operator BvExpr<TSize>(BigInteger value) => Z3Context.Current.Bv<TSize>(value);

    /// <summary>
    /// Implicit conversion from int to bit-vector expression.
    /// </summary>
    /// <param name="value">Integer value to convert.</param>
    /// <returns>Bit-vector expression.</returns>
    public static implicit operator BvExpr<TSize>(int value) => Z3Context.Current.Bv<TSize>(value);

    /// <summary>
    /// Implicit conversion from uint to bit-vector expression.
    /// </summary>
    /// <param name="value">Unsigned integer value to convert.</param>
    /// <returns>Bit-vector expression.</returns>
    public static implicit operator BvExpr<TSize>(uint value) => Z3Context.Current.Bv<TSize>(value);

    /// <summary>
    /// Implicit conversion from long to bit-vector expression.
    /// </summary>
    /// <param name="value">Long value to convert.</param>
    /// <returns>Bit-vector expression.</returns>
    public static implicit operator BvExpr<TSize>(long value) => Z3Context.Current.Bv<TSize>(value);

    /// <summary>
    /// Implicit conversion from ulong to bit-vector expression.
    /// </summary>
    /// <param name="value">Unsigned long value to convert.</param>
    /// <returns>Bit-vector expression.</returns>
    public static implicit operator BvExpr<TSize>(ulong value) => Z3Context.Current.Bv<TSize>(value);

    /// <summary>
    /// Implicit conversion from Bv value to bit-vector expression.
    /// </summary>
    /// <param name="value">Bit-vector value to convert.</param>
    /// <returns>Bit-vector expression.</returns>
    public static implicit operator BvExpr<TSize>(Bv<TSize> value) => Z3Context.Current.Bv(value);

    /// <summary>
    /// Converts this bit-vector to an integer expression.
    /// </summary>
    /// <param name="signed">True for signed conversion; false for unsigned.</param>
    /// <returns>Integer expression representing this bit-vector value.</returns>
    public IntExpr ToInt(bool signed = false) => Context.ToInt(this, signed);

    /// <summary>
    /// Extracts a sub-bit-vector starting from the specified bit position.
    /// </summary>
    /// <typeparam name="TOutputSize">The output bit-vector size type.</typeparam>
    /// <param name="startBit">The starting bit position for extraction.</param>
    /// <returns>Bit-vector expression containing the extracted bits.</returns>
    public BvExpr<TOutputSize> Extract<TOutputSize>(uint startBit)
        where TOutputSize : ISize => Context.Extract<TSize, TOutputSize>(this, startBit);

    /// <summary>
    /// Resizes this bit-vector to a different bit width.
    /// </summary>
    /// <typeparam name="TOutputSize">The target bit-vector size type.</typeparam>
    /// <param name="signed">True for signed extension/truncation; false for unsigned.</param>
    /// <returns>Bit-vector expression with the new bit width.</returns>
    public BvExpr<TOutputSize> Resize<TOutputSize>(bool signed = false)
        where TOutputSize : ISize => Context.Resize<TSize, TOutputSize>(this, signed);

    /// <summary>
    /// Repeats this bit-vector pattern to create a larger bit-vector.
    /// </summary>
    /// <typeparam name="TOutputSize">The target bit-vector size type.</typeparam>
    /// <returns>Bit-vector expression with repeated pattern.</returns>
    public BvExpr<TOutputSize> Repeat<TOutputSize>()
        where TOutputSize : ISize => Context.Repeat<TSize, TOutputSize>(this);

    /// <summary>
    /// Equality comparison of two bit-vector expressions.
    /// </summary>
    public static BoolExpr operator ==(BvExpr<TSize> left, BvExpr<TSize> right) => left.Eq(right);

    /// <summary>
    /// Inequality comparison of two bit-vector expressions.
    /// </summary>
    public static BoolExpr operator !=(BvExpr<TSize> left, BvExpr<TSize> right) => left.Neq(right);

    /// <summary>
    /// Less-than comparison of two bit-vector expressions.
    /// </summary>
    public static BoolExpr operator <(BvExpr<TSize> left, BvExpr<TSize> right) => left.Lt(right);

    /// <summary>
    /// Less-than-or-equal comparison of two bit-vector expressions.
    /// </summary>
    public static BoolExpr operator <=(BvExpr<TSize> left, BvExpr<TSize> right) => left.Le(right);

    /// <summary>
    /// Greater-than comparison of two bit-vector expressions.
    /// </summary>
    public static BoolExpr operator >(BvExpr<TSize> left, BvExpr<TSize> right) => left.Gt(right);

    /// <summary>
    /// Greater-than-or-equal comparison of two bit-vector expressions.
    /// </summary>
    public static BoolExpr operator >=(BvExpr<TSize> left, BvExpr<TSize> right) => left.Ge(right);

    /// <summary>
    /// Addition of two bit-vector expressions.
    /// </summary>
    public static BvExpr<TSize> operator +(BvExpr<TSize> left, BvExpr<TSize> right) => left.Add(right);

    /// <summary>
    /// Subtraction of two bit-vector expressions.
    /// </summary>
    public static BvExpr<TSize> operator -(BvExpr<TSize> left, BvExpr<TSize> right) => left.Sub(right);

    /// <summary>
    /// Multiplication of two bit-vector expressions.
    /// </summary>
    public static BvExpr<TSize> operator *(BvExpr<TSize> left, BvExpr<TSize> right) => left.Mul(right);

    /// <summary>
    /// Division of two bit-vector expressions.
    /// </summary>
    public static BvExpr<TSize> operator /(BvExpr<TSize> left, BvExpr<TSize> right) => left.Div(right);

    /// <summary>
    /// Remainder of two bit-vector expressions.
    /// </summary>
    public static BvExpr<TSize> operator %(BvExpr<TSize> left, BvExpr<TSize> right) => left.Rem(right);

    /// <summary>
    /// Negation of a bit-vector expression.
    /// </summary>
    public static BvExpr<TSize> operator -(BvExpr<TSize> operand) => operand.Neg();

    /// <summary>
    /// Bitwise AND of two bit-vector expressions.
    /// </summary>
    public static BvExpr<TSize> operator &(BvExpr<TSize> left, BvExpr<TSize> right) => left.And(right);

    /// <summary>
    /// Bitwise OR of two bit-vector expressions.
    /// </summary>
    public static BvExpr<TSize> operator |(BvExpr<TSize> left, BvExpr<TSize> right) => left.Or(right);

    /// <summary>
    /// Bitwise XOR of two bit-vector expressions.
    /// </summary>
    public static BvExpr<TSize> operator ^(BvExpr<TSize> left, BvExpr<TSize> right) => left.Xor(right);

    /// <summary>
    /// Bitwise NOT of a bit-vector expression.
    /// </summary>
    public static BvExpr<TSize> operator ~(BvExpr<TSize> operand) => operand.Not();

    /// <summary>
    /// Left shift of bit-vector expression.
    /// </summary>
    public static BvExpr<TSize> operator <<(BvExpr<TSize> left, BvExpr<TSize> right) => left.Shl(right);

    /// <summary>
    /// Right shift of bit-vector expression.
    /// </summary>
    public static BvExpr<TSize> operator >>(BvExpr<TSize> left, BvExpr<TSize> right) => left.Shr(right);
}
