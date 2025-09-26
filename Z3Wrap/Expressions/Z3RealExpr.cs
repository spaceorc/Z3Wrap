using System.Numerics;
using Spaceorc.Z3Wrap.Booleans;
using Spaceorc.Z3Wrap.DataTypes;
using Spaceorc.Z3Wrap.Extensions;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.Expressions;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
/// <summary>
/// Represents a real (rational) number expression in Z3 with exact precision arithmetic.
/// Supports unlimited precision rational arithmetic operations, comparisons, and conversions.
/// All operations maintain exact precision without floating-point approximation errors.
/// </summary>
public sealed class Z3RealExpr : Z3NumericExpr, IZ3ExprType<Z3RealExpr>
{
    private Z3RealExpr(Z3Context context, IntPtr handle)
        : base(context, handle) { }

    static Z3RealExpr IZ3ExprType<Z3RealExpr>.Create(Z3Context context, IntPtr handle) =>
        new(context, handle);

    static IntPtr IZ3ExprType<Z3RealExpr>.GetSort(Z3Context context) =>
        SafeNativeMethods.Z3MkRealSort(context.Handle);

    /// <summary>
    /// Implicitly converts an integer value to a Z3RealExpr using the current thread-local context.
    /// </summary>
    /// <param name="value">The integer value to convert.</param>
    /// <returns>A Z3RealExpr representing the rational number.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no current context is set.</exception>
    public static implicit operator Z3RealExpr(int value) => Z3Context.Current.Real(value);

    /// <summary>
    /// Implicitly converts a long integer value to a Z3RealExpr using the current thread-local context.
    /// </summary>
    /// <param name="value">The long integer value to convert.</param>
    /// <returns>A Z3RealExpr representing the rational number.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no current context is set.</exception>
    public static implicit operator Z3RealExpr(long value) => Z3Context.Current.Real(value);

    /// <summary>
    /// Implicitly converts a decimal value to a Z3RealExpr using the current thread-local context.
    /// </summary>
    /// <param name="value">The decimal value to convert.</param>
    /// <returns>A Z3RealExpr representing the exact rational number.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no current context is set.</exception>
    public static implicit operator Z3RealExpr(decimal value) => Z3Context.Current.Real(value);

    /// <summary>
    /// Implicitly converts a BigInteger value to a Z3RealExpr using the current thread-local context.
    /// </summary>
    /// <param name="value">The BigInteger value to convert.</param>
    /// <returns>A Z3RealExpr representing the rational number.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no current context is set.</exception>
    public static implicit operator Z3RealExpr(BigInteger value) => Z3Context.Current.Real(value);

    /// <summary>
    /// Implicitly converts a Real value to a Z3RealExpr using the current thread-local context.
    /// </summary>
    /// <param name="value">The Real value to convert.</param>
    /// <returns>A Z3RealExpr representing the rational number.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no current context is set.</exception>
    public static implicit operator Z3RealExpr(Real value) => Z3Context.Current.Real(value);

    /// <summary>
    /// Adds two real expressions using the + operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A real expression representing left + right.</returns>
    public static Z3RealExpr operator +(Z3RealExpr left, Z3RealExpr right) =>
        left.Context.Add(left, right);

    /// <summary>
    /// Subtracts two real expressions using the - operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A real expression representing left - right.</returns>
    public static Z3RealExpr operator -(Z3RealExpr left, Z3RealExpr right) =>
        left.Context.Sub(left, right);

    /// <summary>
    /// Multiplies two real expressions using the * operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A real expression representing left * right.</returns>
    public static Z3RealExpr operator *(Z3RealExpr left, Z3RealExpr right) =>
        left.Context.Mul(left, right);

    /// <summary>
    /// Divides two real expressions using the / operator (exact rational division).
    /// </summary>
    /// <param name="left">The left operand (dividend).</param>
    /// <param name="right">The right operand (divisor).</param>
    /// <returns>A real expression representing left / right.</returns>
    public static Z3RealExpr operator /(Z3RealExpr left, Z3RealExpr right) =>
        left.Context.Div(left, right);

    /// <summary>
    /// Compares two real expressions using the &lt; operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A boolean expression representing left &lt; right.</returns>
    public static Z3Bool operator <(Z3RealExpr left, Z3RealExpr right) =>
        left.Context.Lt(left, right);

    /// <summary>
    /// Compares two real expressions using the &lt;= operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A boolean expression representing left &lt;= right.</returns>
    public static Z3Bool operator <=(Z3RealExpr left, Z3RealExpr right) =>
        left.Context.Le(left, right);

    /// <summary>
    /// Compares two real expressions using the &gt; operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A boolean expression representing left &gt; right.</returns>
    public static Z3Bool operator >(Z3RealExpr left, Z3RealExpr right) =>
        left.Context.Gt(left, right);

    /// <summary>
    /// Compares two real expressions using the &gt;= operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A boolean expression representing left &gt;= right.</returns>
    public static Z3Bool operator >=(Z3RealExpr left, Z3RealExpr right) =>
        left.Context.Ge(left, right);

    /// <summary>
    /// Negates a real expression using the unary - operator.
    /// </summary>
    /// <param name="expr">The real expression to negate.</param>
    /// <returns>A real expression representing -expr.</returns>
    public static Z3RealExpr operator -(Z3RealExpr expr) => expr.Context.UnaryMinus(expr);

    /// <summary>
    /// Checks equality between a real expression and a Real value using the == operator.
    /// </summary>
    /// <param name="left">The real expression.</param>
    /// <param name="right">The Real value.</param>
    /// <returns>A boolean expression representing left == right.</returns>
    public static Z3Bool operator ==(Z3RealExpr left, Real right) =>
        left.Context.Eq(left, right);

    /// <summary>
    /// Checks inequality between a real expression and a Real value using the != operator.
    /// </summary>
    /// <param name="left">The real expression.</param>
    /// <param name="right">The Real value.</param>
    /// <returns>A boolean expression representing left != right.</returns>
    public static Z3Bool operator !=(Z3RealExpr left, Real right) =>
        left.Context.Neq(left, right);

    /// <summary>
    /// Checks equality between a Real value and a real expression using the == operator.
    /// </summary>
    /// <param name="left">The Real value.</param>
    /// <param name="right">The real expression.</param>
    /// <returns>A boolean expression representing left == right.</returns>
    public static Z3Bool operator ==(Real left, Z3RealExpr right) =>
        right.Context.Eq(left, right);

    /// <summary>
    /// Checks inequality between a Real value and a real expression using the != operator.
    /// </summary>
    /// <param name="left">The Real value.</param>
    /// <param name="right">The real expression.</param>
    /// <returns>A boolean expression representing left != right.</returns>
    public static Z3Bool operator !=(Real left, Z3RealExpr right) =>
        right.Context.Neq(left, right);

    /// <summary>
    /// Adds this real expression to another real expression.
    /// </summary>
    /// <param name="other">The real expression to add.</param>
    /// <returns>A real expression representing this + other.</returns>
    public Z3RealExpr Add(Z3RealExpr other) => Context.Add(this, other);

    /// <summary>
    /// Subtracts another real expression from this real expression.
    /// </summary>
    /// <param name="other">The real expression to subtract.</param>
    /// <returns>A real expression representing this - other.</returns>
    public Z3RealExpr Sub(Z3RealExpr other) => Context.Sub(this, other);

    /// <summary>
    /// Multiplies this real expression by another real expression.
    /// </summary>
    /// <param name="other">The real expression to multiply by.</param>
    /// <returns>A real expression representing this * other.</returns>
    public Z3RealExpr Mul(Z3RealExpr other) => Context.Mul(this, other);

    /// <summary>
    /// Divides this real expression by another real expression (exact rational division).
    /// </summary>
    /// <param name="other">The real expression to divide by.</param>
    /// <returns>A real expression representing this / other.</returns>
    public Z3RealExpr Div(Z3RealExpr other) => Context.Div(this, other);

    /// <summary>
    /// Creates a less-than comparison with another real expression.
    /// </summary>
    /// <param name="other">The real expression to compare with.</param>
    /// <returns>A boolean expression representing this &lt; other.</returns>
    public Z3Bool Lt(Z3RealExpr other) => Context.Lt(this, other);

    /// <summary>
    /// Creates a less-than-or-equal comparison with another real expression.
    /// </summary>
    /// <param name="other">The real expression to compare with.</param>
    /// <returns>A boolean expression representing this &lt;= other.</returns>
    public Z3Bool Le(Z3RealExpr other) => Context.Le(this, other);

    /// <summary>
    /// Creates a greater-than comparison with another real expression.
    /// </summary>
    /// <param name="other">The real expression to compare with.</param>
    /// <returns>A boolean expression representing this &gt; other.</returns>
    public Z3Bool Gt(Z3RealExpr other) => Context.Gt(this, other);

    /// <summary>
    /// Creates a greater-than-or-equal comparison with another real expression.
    /// </summary>
    /// <param name="other">The real expression to compare with.</param>
    /// <returns>A boolean expression representing this &gt;= other.</returns>
    public Z3Bool Ge(Z3RealExpr other) => Context.Ge(this, other);

    /// <summary>
    /// Negates this real expression (computes the unary minus).
    /// </summary>
    /// <returns>A real expression representing -this.</returns>
    public Z3RealExpr UnaryMinus() => Context.UnaryMinus(this);

    /// <summary>
    /// Computes the absolute value of this real expression.
    /// </summary>
    /// <returns>A real expression representing |this|.</returns>
    public Z3RealExpr Abs() => Context.Abs(this);

    /// <summary>
    /// Converts this real expression to an integer expression (truncates towards zero).
    /// </summary>
    /// <returns>An integer expression representing the integer part of this real number.</returns>
    public Z3IntExpr ToInt() => Context.ToInt(this);
}
