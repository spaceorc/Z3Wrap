using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Core.Interop;
using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Values.Numerics;

namespace Spaceorc.Z3Wrap.Expressions.Numerics;

/// <summary>
/// Represents a real number expression for arithmetic operations with exact rational precision.
/// </summary>
#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
public sealed class RealExpr : Z3Expr, IArithmeticExpr<RealExpr>, IExprType<RealExpr>
{
    private RealExpr(Z3Context context, IntPtr handle)
        : base(context, handle) { }

    static RealExpr IExprType<RealExpr>.Create(Z3Context context, IntPtr handle) => new(context, handle);

    static IntPtr IExprType<RealExpr>.Sort(Z3Context context) => context.Library.Z3MkRealSort(context.Handle);

    static RealExpr IArithmeticExpr<RealExpr>.Zero(Z3Context context) => context.Real(0);

    /// <summary>
    /// Implicit conversion from 32-bit integer to real expression.
    /// </summary>
    /// <param name="value">The integer value.</param>
    /// <returns>Real expression representing the value.</returns>
    public static implicit operator RealExpr(int value) => Z3Context.Current.Real(value);

    /// <summary>
    /// Implicit conversion from 64-bit integer to real expression.
    /// </summary>
    /// <param name="value">The long value.</param>
    /// <returns>Real expression representing the value.</returns>
    public static implicit operator RealExpr(long value) => Z3Context.Current.Real(value);

    /// <summary>
    /// Implicit conversion from decimal to real expression.
    /// </summary>
    /// <param name="value">The decimal value.</param>
    /// <returns>Real expression representing the value.</returns>
    public static implicit operator RealExpr(decimal value) => Z3Context.Current.Real(value);

    /// <summary>
    /// Implicit conversion from BigInteger to real expression.
    /// </summary>
    /// <param name="value">The BigInteger value.</param>
    /// <returns>Real expression representing the value.</returns>
    public static implicit operator RealExpr(BigInteger value) => Z3Context.Current.Real(value);

    /// <summary>
    /// Implicit conversion from Real value to real expression.
    /// </summary>
    /// <param name="value">The Real value.</param>
    /// <returns>Real expression representing the value.</returns>
    public static implicit operator RealExpr(Real value) => Z3Context.Current.Real(value);

    /// <summary>
    /// Converts this real expression to an integer expression.
    /// </summary>
    /// <returns>Integer expression representing this real.</returns>
    public IntExpr ToInt() => Context.ToInt(this);

    /// <summary>
    /// Addition of two real expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Addition expression.</returns>
    public static RealExpr operator +(RealExpr left, RealExpr right) => left.Add(right);

    /// <summary>
    /// Subtraction of two real expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Subtraction expression.</returns>
    public static RealExpr operator -(RealExpr left, RealExpr right) => left.Sub(right);

    /// <summary>
    /// Multiplication of two real expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Multiplication expression.</returns>
    public static RealExpr operator *(RealExpr left, RealExpr right) => left.Mul(right);

    /// <summary>
    /// Division of two real expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Division expression.</returns>
    public static RealExpr operator /(RealExpr left, RealExpr right) => left.Div(right);

    /// <summary>
    /// Unary minus of a real expression.
    /// </summary>
    /// <param name="expr">The expression to negate.</param>
    /// <returns>Negated expression.</returns>
    public static RealExpr operator -(RealExpr expr) => expr.UnaryMinus();

    /// <summary>
    /// Less-than comparison of two real expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Less-than comparison expression.</returns>
    public static BoolExpr operator <(RealExpr left, RealExpr right) => left.Lt(right);

    /// <summary>
    /// Less-than-or-equal comparison of two real expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Less-than-or-equal comparison expression.</returns>
    public static BoolExpr operator <=(RealExpr left, RealExpr right) => left.Le(right);

    /// <summary>
    /// Greater-than comparison of two real expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Greater-than comparison expression.</returns>
    public static BoolExpr operator >(RealExpr left, RealExpr right) => left.Gt(right);

    /// <summary>
    /// Greater-than-or-equal comparison of two real expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Greater-than-or-equal comparison expression.</returns>
    public static BoolExpr operator >=(RealExpr left, RealExpr right) => left.Ge(right);

    /// <summary>
    /// Equality comparison of two real expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Equality comparison expression.</returns>
    public static BoolExpr operator ==(RealExpr left, RealExpr right) => left.Eq(right);

    /// <summary>
    /// Inequality comparison of two real expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Inequality comparison expression.</returns>
    public static BoolExpr operator !=(RealExpr left, RealExpr right) => left.Neq(right);
}
#pragma warning restore CS0660, CS0661
