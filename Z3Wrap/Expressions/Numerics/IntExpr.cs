using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.Expressions.Numerics;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
/// <summary>
/// Represents an integer expression in Z3 with unlimited precision arithmetic using BigInteger.
/// Supports natural mathematical operations, comparisons, and conversions to other numeric types.
/// All arithmetic is performed with unlimited precision - no overflow or underflow occurs.
/// </summary>
public sealed class IntExpr : Z3Expr, IArithmeticExpr<IntExpr>, IExprType<IntExpr>
{
    #region Core Implementation

    private IntExpr(Z3Context context, IntPtr handle)
        : base(context, handle) { }

    static IntExpr IExprType<IntExpr>.Create(Z3Context context, IntPtr handle) =>
        new(context, handle);

    static IntPtr IExprType<IntExpr>.Sort(Z3Context context) =>
        SafeNativeMethods.Z3MkIntSort(context.Handle);

    static IntExpr IArithmeticExpr<IntExpr>.Zero(Z3Context context) => context.Int(0);

    #endregion

    #region Implicit Conversions

    /// <summary>
    /// Implicitly converts an integer value to a Z3Int using the current thread-local context.
    /// </summary>
    /// <param name="value">The integer value to convert.</param>
    /// <returns>A Z3Int representing the integer constant.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no current context is set.</exception>
    public static implicit operator IntExpr(int value) => Z3Context.Current.Int(value);

    /// <summary>
    /// Implicitly converts a long integer value to a Z3Int using the current thread-local context.
    /// </summary>
    /// <param name="value">The long integer value to convert.</param>
    /// <returns>A Z3Int representing the integer constant.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no current context is set.</exception>
    public static implicit operator IntExpr(long value) => Z3Context.Current.Int(value);

    /// <summary>
    /// Implicitly converts a BigInteger value to a Z3Int using the current thread-local context.
    /// </summary>
    /// <param name="value">The BigInteger value to convert.</param>
    /// <returns>A Z3Int representing the integer constant.</returns>
    /// <exception cref="InvalidOperationException">Thrown when no current context is set.</exception>
    public static implicit operator IntExpr(BigInteger value) => Z3Context.Current.Int(value);

    #endregion

    #region Arithmetic Operators

    /// <summary>
    /// Addition operator for integer expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Expression representing left + right.</returns>
    public static IntExpr operator +(IntExpr left, IntExpr right) => left.Add(right);

    /// <summary>
    /// Subtraction operator for integer expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Expression representing left - right.</returns>
    public static IntExpr operator -(IntExpr left, IntExpr right) => left.Sub(right);

    /// <summary>
    /// Multiplication operator for integer expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Expression representing left * right.</returns>
    public static IntExpr operator *(IntExpr left, IntExpr right) => left.Mul(right);

    /// <summary>
    /// Division operator for integer expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Expression representing left / right.</returns>
    public static IntExpr operator /(IntExpr left, IntExpr right) => left.Div(right);

    /// <summary>
    /// Modulo operator for integer expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Expression representing left % right.</returns>
    public static IntExpr operator %(IntExpr left, IntExpr right) => left.Mod(right);

    /// <summary>
    /// Unary minus operator for integer expressions.
    /// </summary>
    /// <param name="expr">Expression to negate.</param>
    /// <returns>Expression representing -expr.</returns>
    public static IntExpr operator -(IntExpr expr) => expr.UnaryMinus();

    #endregion

    #region Comparison Operators

    /// <summary>
    /// Less-than operator for integer expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Boolean expression representing left &lt; right.</returns>
    public static BoolExpr operator <(IntExpr left, IntExpr right) => left.Lt(right);

    /// <summary>
    /// Less-than-or-equal operator for integer expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Boolean expression representing left &lt;= right.</returns>
    public static BoolExpr operator <=(IntExpr left, IntExpr right) => left.Le(right);

    /// <summary>
    /// Greater-than operator for integer expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Boolean expression representing left &gt; right.</returns>
    public static BoolExpr operator >(IntExpr left, IntExpr right) => left.Gt(right);

    /// <summary>
    /// Greater-than-or-equal operator for integer expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Boolean expression representing left &gt;= right.</returns>
    public static BoolExpr operator >=(IntExpr left, IntExpr right) => left.Ge(right);

    #endregion

    #region Equality Operators

    /// <summary>
    /// Equality operator for integer expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Boolean expression representing left == right.</returns>
    public static BoolExpr operator ==(IntExpr left, IntExpr right) => left.Eq(right);

    /// <summary>
    /// Inequality operator for integer expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Boolean expression representing left != right.</returns>
    public static BoolExpr operator !=(IntExpr left, IntExpr right) => left.Neq(right);

    #endregion
}
#pragma warning restore CS0660, CS0661
