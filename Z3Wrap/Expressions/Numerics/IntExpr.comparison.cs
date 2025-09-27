using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Expressions.Logic;

namespace Spaceorc.Z3Wrap.Expressions.Numerics;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
public sealed partial class IntExpr
{
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
}
