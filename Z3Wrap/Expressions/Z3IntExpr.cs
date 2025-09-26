using System.Numerics;
using Spaceorc.Z3Wrap.BitVectors;
using Spaceorc.Z3Wrap.Booleans;
using Spaceorc.Z3Wrap.Extensions;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.Expressions;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
/// <summary>
/// Represents an integer expression in Z3 with unlimited precision arithmetic using BigInteger.
/// Supports natural mathematical operations, comparisons, and conversions to other numeric types.
/// All arithmetic is performed with unlimited precision - no overflow or underflow occurs.
/// </summary>
public sealed class Z3IntExpr : Z3NumericExpr, IZ3ExprType<Z3IntExpr>
{
    private Z3IntExpr(Z3Context context, IntPtr handle)
        : base(context, handle) { }

    static Z3IntExpr IZ3ExprType<Z3IntExpr>.Create(Z3Context context, IntPtr handle) =>
        new(context, handle);

    static IntPtr IZ3ExprType<Z3IntExpr>.GetSort(Z3Context context) =>
        SafeNativeMethods.Z3MkIntSort(context.Handle);

    /// <summary>
    /// Implicitly converts an integer value to a Z3IntExpr using the current thread-local context.
    /// </summary>
    /// <param name="value">The integer value to convert.</param>
    /// <returns>A Z3IntExpr representing the integer constant.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no current context is set.</exception>
    public static implicit operator Z3IntExpr(int value) => Z3Context.Current.Int(value);

    /// <summary>
    /// Implicitly converts a long integer value to a Z3IntExpr using the current thread-local context.
    /// </summary>
    /// <param name="value">The long integer value to convert.</param>
    /// <returns>A Z3IntExpr representing the integer constant.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no current context is set.</exception>
    public static implicit operator Z3IntExpr(long value) => Z3Context.Current.Int(value);

    /// <summary>
    /// Implicitly converts a BigInteger value to a Z3IntExpr using the current thread-local context.
    /// </summary>
    /// <param name="value">The BigInteger value to convert.</param>
    /// <returns>A Z3IntExpr representing the integer constant.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no current context is set.</exception>
    public static implicit operator Z3IntExpr(BigInteger value) => Z3Context.Current.Int(value);

    /// <summary>
    /// Adds two integer expressions using the + operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>An integer expression representing left + right.</returns>
    public static Z3IntExpr operator +(Z3IntExpr left, Z3IntExpr right) =>
        left.Context.Add(left, right);

    /// <summary>
    /// Subtracts two integer expressions using the - operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>An integer expression representing left - right.</returns>
    public static Z3IntExpr operator -(Z3IntExpr left, Z3IntExpr right) =>
        left.Context.Sub(left, right);

    /// <summary>
    /// Multiplies two integer expressions using the * operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>An integer expression representing left * right.</returns>
    public static Z3IntExpr operator *(Z3IntExpr left, Z3IntExpr right) =>
        left.Context.Mul(left, right);

    /// <summary>
    /// Divides two integer expressions using the / operator (integer division).
    /// </summary>
    /// <param name="left">The left operand (dividend).</param>
    /// <param name="right">The right operand (divisor).</param>
    /// <returns>An integer expression representing left / right.</returns>
    public static Z3IntExpr operator /(Z3IntExpr left, Z3IntExpr right) =>
        left.Context.Div(left, right);

    /// <summary>
    /// Computes the modulo of two integer expressions using the % operator.
    /// </summary>
    /// <param name="left">The left operand (dividend).</param>
    /// <param name="right">The right operand (divisor).</param>
    /// <returns>An integer expression representing left % right.</returns>
    public static Z3IntExpr operator %(Z3IntExpr left, Z3IntExpr right) =>
        left.Context.Mod(left, right);

    /// <summary>
    /// Compares two integer expressions using the &lt; operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A boolean expression representing left &lt; right.</returns>
    public static Z3Bool operator <(Z3IntExpr left, Z3IntExpr right) =>
        left.Context.Lt(left, right);

    /// <summary>
    /// Compares two integer expressions using the &lt;= operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A boolean expression representing left &lt;= right.</returns>
    public static Z3Bool operator <=(Z3IntExpr left, Z3IntExpr right) =>
        left.Context.Le(left, right);

    /// <summary>
    /// Compares two integer expressions using the &gt; operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A boolean expression representing left &gt; right.</returns>
    public static Z3Bool operator >(Z3IntExpr left, Z3IntExpr right) =>
        left.Context.Gt(left, right);

    /// <summary>
    /// Compares two integer expressions using the &gt;= operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A boolean expression representing left &gt;= right.</returns>
    public static Z3Bool operator >=(Z3IntExpr left, Z3IntExpr right) =>
        left.Context.Ge(left, right);

    /// <summary>
    /// Negates an integer expression using the unary - operator.
    /// </summary>
    /// <param name="expr">The integer expression to negate.</param>
    /// <returns>An integer expression representing -expr.</returns>
    public static Z3IntExpr operator -(Z3IntExpr expr) => expr.Context.UnaryMinus(expr);

    /// <summary>
    /// Checks equality between an integer expression and a BigInteger using the == operator.
    /// </summary>
    /// <param name="left">The integer expression.</param>
    /// <param name="right">The BigInteger value.</param>
    /// <returns>A boolean expression representing left == right.</returns>
    public static Z3Bool operator ==(Z3IntExpr left, BigInteger right) =>
        left.Context.Eq(left, right);

    /// <summary>
    /// Checks inequality between an integer expression and a BigInteger using the != operator.
    /// </summary>
    /// <param name="left">The integer expression.</param>
    /// <param name="right">The BigInteger value.</param>
    /// <returns>A boolean expression representing left != right.</returns>
    public static Z3Bool operator !=(Z3IntExpr left, BigInteger right) =>
        left.Context.Neq(left, right);

    /// <summary>
    /// Checks equality between a BigInteger and an integer expression using the == operator.
    /// </summary>
    /// <param name="left">The BigInteger value.</param>
    /// <param name="right">The integer expression.</param>
    /// <returns>A boolean expression representing left == right.</returns>
    public static Z3Bool operator ==(BigInteger left, Z3IntExpr right) =>
        right.Context.Eq(left, right);

    /// <summary>
    /// Checks inequality between a BigInteger and an integer expression using the != operator.
    /// </summary>
    /// <param name="left">The BigInteger value.</param>
    /// <param name="right">The integer expression.</param>
    /// <returns>A boolean expression representing left != right.</returns>
    public static Z3Bool operator !=(BigInteger left, Z3IntExpr right) =>
        right.Context.Neq(left, right);

    /// <summary>
    /// Adds this integer expression to another integer expression.
    /// </summary>
    /// <param name="other">The integer expression to add.</param>
    /// <returns>An integer expression representing this + other.</returns>
    public Z3IntExpr Add(Z3IntExpr other) => Context.Add(this, other);

    /// <summary>
    /// Subtracts another integer expression from this integer expression.
    /// </summary>
    /// <param name="other">The integer expression to subtract.</param>
    /// <returns>An integer expression representing this - other.</returns>
    public Z3IntExpr Sub(Z3IntExpr other) => Context.Sub(this, other);

    /// <summary>
    /// Multiplies this integer expression by another integer expression.
    /// </summary>
    /// <param name="other">The integer expression to multiply by.</param>
    /// <returns>An integer expression representing this * other.</returns>
    public Z3IntExpr Mul(Z3IntExpr other) => Context.Mul(this, other);

    /// <summary>
    /// Divides this integer expression by another integer expression (integer division).
    /// </summary>
    /// <param name="other">The integer expression to divide by.</param>
    /// <returns>An integer expression representing this / other.</returns>
    public Z3IntExpr Div(Z3IntExpr other) => Context.Div(this, other);

    /// <summary>
    /// Computes the modulo of this integer expression with another integer expression.
    /// </summary>
    /// <param name="other">The integer expression to compute modulo with.</param>
    /// <returns>An integer expression representing this % other.</returns>
    public Z3IntExpr Mod(Z3IntExpr other) => Context.Mod(this, other);

    /// <summary>
    /// Creates a less-than comparison with another integer expression.
    /// </summary>
    /// <param name="other">The integer expression to compare with.</param>
    /// <returns>A boolean expression representing this &lt; other.</returns>
    public Z3Bool Lt(Z3IntExpr other) => Context.Lt(this, other);

    /// <summary>
    /// Creates a less-than-or-equal comparison with another integer expression.
    /// </summary>
    /// <param name="other">The integer expression to compare with.</param>
    /// <returns>A boolean expression representing this &lt;= other.</returns>
    public Z3Bool Le(Z3IntExpr other) => Context.Le(this, other);

    /// <summary>
    /// Creates a greater-than comparison with another integer expression.
    /// </summary>
    /// <param name="other">The integer expression to compare with.</param>
    /// <returns>A boolean expression representing this &gt; other.</returns>
    public Z3Bool Gt(Z3IntExpr other) => Context.Gt(this, other);

    /// <summary>
    /// Creates a greater-than-or-equal comparison with another integer expression.
    /// </summary>
    /// <param name="other">The integer expression to compare with.</param>
    /// <returns>A boolean expression representing this &gt;= other.</returns>
    public Z3Bool Ge(Z3IntExpr other) => Context.Ge(this, other);

    /// <summary>
    /// Negates this integer expression (computes the unary minus).
    /// </summary>
    /// <returns>An integer expression representing -this.</returns>
    public Z3IntExpr UnaryMinus() => Context.UnaryMinus(this);

    /// <summary>
    /// Computes the absolute value of this integer expression.
    /// </summary>
    /// <returns>An integer expression representing |this|.</returns>
    public Z3IntExpr Abs() => Context.Abs(this);

    /// <summary>
    /// Converts this integer expression to a real (rational) number expression.
    /// </summary>
    /// <returns>A real expression representing this integer as a rational number.</returns>
    public Z3RealExpr ToReal() => Context.ToReal(this);

    /// <summary>
    /// Converts this integer expression to a bitvector expression with the specified bit width.
    /// </summary>
    /// <typeparam name="TSize">The size type that determines the bit width of the resulting bitvector.</typeparam>
    /// <returns>A bitvector expression representing this integer value.</returns>
    public Z3BitVec<TSize> ToBitVec<TSize>()
        where TSize : ISize => Context.ToBitVec<TSize>(this);
}
