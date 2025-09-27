using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Interop;
using Spaceorc.Z3Wrap.Values.Numerics;

namespace Spaceorc.Z3Wrap.Expressions.Numerics;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
/// <summary>
/// Real (rational) number expression with exact precision arithmetic.
/// </summary>
public sealed class RealExpr : Z3Expr, IArithmeticExpr<RealExpr>, IExprType<RealExpr>
{
    #region Core Implementation

    private RealExpr(Z3Context context, IntPtr handle)
        : base(context, handle) { }

    static RealExpr IExprType<RealExpr>.Create(Z3Context context, IntPtr handle) =>
        new(context, handle);

    static IntPtr IExprType<RealExpr>.Sort(Z3Context context) =>
        SafeNativeMethods.Z3MkRealSort(context.Handle);

    static RealExpr IArithmeticExpr<RealExpr>.Zero(Z3Context context) => context.Real(0);

    #endregion

    #region Implicit Conversions

    /// <summary>
    /// Converts integer to real expression.
    /// </summary>
    /// <param name="value">Integer value.</param>
    /// <returns>Real expression.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no current context is set.</exception>
    public static implicit operator RealExpr(int value) => Z3Context.Current.Real(value);

    /// <summary>
    /// Converts long to real expression.
    /// </summary>
    /// <param name="value">Long value.</param>
    /// <returns>Real expression.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no current context is set.</exception>
    public static implicit operator RealExpr(long value) => Z3Context.Current.Real(value);

    /// <summary>
    /// Converts decimal to real expression.
    /// </summary>
    /// <param name="value">Decimal value.</param>
    /// <returns>Real expression.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no current context is set.</exception>
    public static implicit operator RealExpr(decimal value) => Z3Context.Current.Real(value);

    /// <summary>
    /// Converts BigInteger to real expression.
    /// </summary>
    /// <param name="value">BigInteger value.</param>
    /// <returns>Real expression.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no current context is set.</exception>
    public static implicit operator RealExpr(BigInteger value) => Z3Context.Current.Real(value);

    /// <summary>
    /// Converts Real to real expression.
    /// </summary>
    /// <param name="value">Real value.</param>
    /// <returns>Real expression.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no current context is set.</exception>
    public static implicit operator RealExpr(Real value) => Z3Context.Current.Real(value);

    #endregion

    #region Arithmetic Operators

    /// <summary>
    /// Adds two real expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Expression representing left + right.</returns>
    public static RealExpr operator +(RealExpr left, RealExpr right) => left.Add(right);

    /// <summary>
    /// Subtracts two real expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Expression representing left - right.</returns>
    public static RealExpr operator -(RealExpr left, RealExpr right) => left.Sub(right);

    /// <summary>
    /// Multiplies two real expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Expression representing left * right.</returns>
    public static RealExpr operator *(RealExpr left, RealExpr right) => left.Mul(right);

    /// <summary>
    /// Divides two real expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Expression representing left / right.</returns>
    public static RealExpr operator /(RealExpr left, RealExpr right) => left.Div(right);

    /// <summary>
    /// Negates a real expression.
    /// </summary>
    /// <param name="expr">Expression to negate.</param>
    /// <returns>Expression representing -expr.</returns>
    public static RealExpr operator -(RealExpr expr) => expr.UnaryMinus();

    #endregion

    #region Comparison Operators

    /// <summary>
    /// Creates less-than comparison between real expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Boolean expression representing left &lt; right.</returns>
    public static BoolExpr operator <(RealExpr left, RealExpr right) => left.Lt(right);

    /// <summary>
    /// Creates less-than-or-equal comparison between real expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Boolean expression representing left ≤ right.</returns>
    public static BoolExpr operator <=(RealExpr left, RealExpr right) => left.Le(right);

    /// <summary>
    /// Creates greater-than comparison between real expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Boolean expression representing left &gt; right.</returns>
    public static BoolExpr operator >(RealExpr left, RealExpr right) => left.Gt(right);

    /// <summary>
    /// Creates greater-than-or-equal comparison between real expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Boolean expression representing left ≥ right.</returns>
    public static BoolExpr operator >=(RealExpr left, RealExpr right) => left.Ge(right);

    #endregion

    #region Equality Operators

    /// <summary>
    /// Creates equality comparison between real expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Boolean expression representing left == right.</returns>
    public static BoolExpr operator ==(RealExpr left, RealExpr right) => left.Eq(right);

    /// <summary>
    /// Creates inequality comparison between real expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Boolean expression representing left != right.</returns>
    public static BoolExpr operator !=(RealExpr left, RealExpr right) => left.Neq(right);

    #endregion
}
#pragma warning restore CS0660, CS0661
