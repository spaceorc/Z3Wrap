using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Core.Interop;
using Spaceorc.Z3Wrap.Expressions.BitVectors;
using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Spaceorc.Z3Wrap.Expressions.Numerics;

/// <summary>
/// Represents an integer expression for arithmetic operations and constraints.
/// </summary>
#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
public sealed class IntExpr : Z3Expr, IArithmeticExpr<IntExpr>, IExprType<IntExpr>
{
    private IntExpr(Z3Context context, IntPtr handle)
        : base(context, handle) { }

    static IntExpr IExprType<IntExpr>.Create(Z3Context context, IntPtr handle) => new(context, handle);

    static IntPtr IExprType<IntExpr>.Sort(Z3Context context) => context.Library.Z3MkIntSort(context.Handle);

    static IntExpr IArithmeticExpr<IntExpr>.Zero(Z3Context context) => context.Int(0);

    /// <summary>
    /// Implicit conversion from 32-bit integer to integer expression.
    /// </summary>
    /// <param name="value">The integer value.</param>
    /// <returns>Integer expression representing the value.</returns>
    public static implicit operator IntExpr(int value) => Z3Context.Current.Int(value);

    /// <summary>
    /// Implicit conversion from 64-bit integer to integer expression.
    /// </summary>
    /// <param name="value">The long value.</param>
    /// <returns>Integer expression representing the value.</returns>
    public static implicit operator IntExpr(long value) => Z3Context.Current.Int(value);

    /// <summary>
    /// Implicit conversion from BigInteger to integer expression.
    /// </summary>
    /// <param name="value">The BigInteger value.</param>
    /// <returns>Integer expression representing the value.</returns>
    public static implicit operator IntExpr(BigInteger value) => Z3Context.Current.Int(value);

    /// <summary>
    /// Converts this integer expression to a real expression.
    /// </summary>
    /// <returns>Real expression representing this integer.</returns>
    public RealExpr ToReal() => Context.ToReal(this);

    /// <summary>
    /// Converts this integer expression to a bit-vector expression.
    /// </summary>
    /// <typeparam name="TSize">The bit-vector size type.</typeparam>
    /// <returns>Bit-vector expression representing this integer.</returns>
    public BvExpr<TSize> ToBitVec<TSize>()
        where TSize : ISize => Context.ToBitVec<TSize>(this);

    /// <summary>
    /// Addition of two integer expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Addition expression.</returns>
    public static IntExpr operator +(IntExpr left, IntExpr right) => left.Add(right);

    /// <summary>
    /// Subtraction of two integer expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Subtraction expression.</returns>
    public static IntExpr operator -(IntExpr left, IntExpr right) => left.Sub(right);

    /// <summary>
    /// Multiplication of two integer expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Multiplication expression.</returns>
    public static IntExpr operator *(IntExpr left, IntExpr right) => left.Mul(right);

    /// <summary>
    /// Division of two integer expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Division expression.</returns>
    public static IntExpr operator /(IntExpr left, IntExpr right) => left.Div(right);

    /// <summary>
    /// Modulo of two integer expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Modulo expression.</returns>
    public static IntExpr operator %(IntExpr left, IntExpr right) => left.Mod(right);

    /// <summary>
    /// Unary minus of an integer expression.
    /// </summary>
    /// <param name="expr">The expression to negate.</param>
    /// <returns>Negated expression.</returns>
    public static IntExpr operator -(IntExpr expr) => expr.UnaryMinus();

    /// <summary>
    /// Less-than comparison of two integer expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Less-than comparison expression.</returns>
    public static BoolExpr operator <(IntExpr left, IntExpr right) => left.Lt(right);

    /// <summary>
    /// Less-than-or-equal comparison of two integer expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Less-than-or-equal comparison expression.</returns>
    public static BoolExpr operator <=(IntExpr left, IntExpr right) => left.Le(right);

    /// <summary>
    /// Greater-than comparison of two integer expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Greater-than comparison expression.</returns>
    public static BoolExpr operator >(IntExpr left, IntExpr right) => left.Gt(right);

    /// <summary>
    /// Greater-than-or-equal comparison of two integer expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Greater-than-or-equal comparison expression.</returns>
    public static BoolExpr operator >=(IntExpr left, IntExpr right) => left.Ge(right);

    /// <summary>
    /// Equality comparison of two integer expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Equality comparison expression.</returns>
    public static BoolExpr operator ==(IntExpr left, IntExpr right) => left.Eq(right);

    /// <summary>
    /// Inequality comparison of two integer expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Inequality comparison expression.</returns>
    public static BoolExpr operator !=(IntExpr left, IntExpr right) => left.Neq(right);
}
#pragma warning restore CS0660, CS0661
