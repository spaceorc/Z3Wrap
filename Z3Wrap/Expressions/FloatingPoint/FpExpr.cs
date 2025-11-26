using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Expressions.Logic;

namespace Spaceorc.Z3Wrap.Expressions.FloatingPoint;

/// <summary>
/// Represents a floating-point expression with specified IEEE 754 format.
/// </summary>
/// <typeparam name="TFormat">The floating-point format (Float16, Float32, or Float64).</typeparam>
#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
public sealed class FpExpr<TFormat> : Z3Expr, IExprType<FpExpr<TFormat>>
    where TFormat : IFloatFormat
{
    internal FpExpr(Z3Context context, IntPtr handle)
        : base(context, handle) { }

    static FpExpr<TFormat> IExprType<FpExpr<TFormat>>.Create(Z3Context context, IntPtr handle) => new(context, handle);

    static IntPtr IExprType<FpExpr<TFormat>>.Sort(Z3Context context) =>
        context.Library.MkFpaSort(context.Handle, TFormat.ExponentBits, TFormat.SignificandBits);

    /// <summary>
    /// Implicit conversion from double to floating-point expression.
    /// </summary>
    /// <param name="value">The double value.</param>
    /// <returns>Floating-point expression representing the value.</returns>
    public static implicit operator FpExpr<TFormat>(double value) => Z3Context.Current.Fp<TFormat>(value);

    /// <summary>
    /// Less than comparison of two floating-point values.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Boolean expression representing left &lt; right.</returns>
    public static BoolExpr operator <(FpExpr<TFormat> left, FpExpr<TFormat> right)
    {
        var handle = left.Context.Library.MkFpaLt(left.Context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(left.Context, handle);
    }

    /// <summary>
    /// Less than or equal comparison of two floating-point values.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Boolean expression representing left &lt;= right.</returns>
    public static BoolExpr operator <=(FpExpr<TFormat> left, FpExpr<TFormat> right)
    {
        var handle = left.Context.Library.MkFpaLeq(left.Context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(left.Context, handle);
    }

    /// <summary>
    /// Greater than comparison of two floating-point values.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Boolean expression representing left &gt; right.</returns>
    public static BoolExpr operator >(FpExpr<TFormat> left, FpExpr<TFormat> right)
    {
        var handle = left.Context.Library.MkFpaGt(left.Context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(left.Context, handle);
    }

    /// <summary>
    /// Greater than or equal comparison of two floating-point values.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Boolean expression representing left &gt;= right.</returns>
    public static BoolExpr operator >=(FpExpr<TFormat> left, FpExpr<TFormat> right)
    {
        var handle = left.Context.Library.MkFpaGeq(left.Context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(left.Context, handle);
    }

    /// <summary>
    /// Equality comparison of two floating-point values.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Boolean expression representing left == right.</returns>
    public static BoolExpr operator ==(FpExpr<TFormat> left, FpExpr<TFormat> right)
    {
        var handle = left.Context.Library.MkFpaEq(left.Context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(left.Context, handle);
    }

    /// <summary>
    /// Inequality comparison of two floating-point values.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Boolean expression representing left != right.</returns>
    public static BoolExpr operator !=(FpExpr<TFormat> left, FpExpr<TFormat> right)
    {
        var handle = left.Context.Library.MkNot(
            left.Context.Handle,
            left.Context.Library.MkFpaEq(left.Context.Handle, left.Handle, right.Handle)
        );
        return Z3Expr.Create<BoolExpr>(left.Context, handle);
    }

    /// <summary>
    /// Addition of two floating-point values.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Sum of the operands.</returns>
    public static FpExpr<TFormat> operator +(FpExpr<TFormat> left, FpExpr<TFormat> right)
    {
        var rm = left.Context.RoundingMode(RoundingMode.NearestTiesToEven);
        return left.Add(right, rm);
    }

    /// <summary>
    /// Subtraction of two floating-point values.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Difference of the operands.</returns>
    public static FpExpr<TFormat> operator -(FpExpr<TFormat> left, FpExpr<TFormat> right)
    {
        var rm = left.Context.RoundingMode(RoundingMode.NearestTiesToEven);
        return left.Sub(right, rm);
    }

    /// <summary>
    /// Multiplication of two floating-point values.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Product of the operands.</returns>
    public static FpExpr<TFormat> operator *(FpExpr<TFormat> left, FpExpr<TFormat> right)
    {
        var rm = left.Context.RoundingMode(RoundingMode.NearestTiesToEven);
        return left.Mul(right, rm);
    }

    /// <summary>
    /// Division of two floating-point values.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Quotient of the operands.</returns>
    public static FpExpr<TFormat> operator /(FpExpr<TFormat> left, FpExpr<TFormat> right)
    {
        var rm = left.Context.RoundingMode(RoundingMode.NearestTiesToEven);
        return left.Div(right, rm);
    }

    /// <summary>
    /// Remainder of two floating-point values.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Remainder of the operands.</returns>
    public static FpExpr<TFormat> operator %(FpExpr<TFormat> left, FpExpr<TFormat> right)
    {
        return left.Rem(right);
    }

    /// <summary>
    /// Negation of floating-point value.
    /// </summary>
    /// <param name="operand">The operand.</param>
    /// <returns>Negated value.</returns>
    public static FpExpr<TFormat> operator -(FpExpr<TFormat> operand)
    {
        return operand.Neg();
    }
}
#pragma warning restore CS0660, CS0661
