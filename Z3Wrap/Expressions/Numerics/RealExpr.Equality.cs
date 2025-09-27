using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Expressions.Logic;

namespace Spaceorc.Z3Wrap.Expressions.Numerics;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
public sealed partial class RealExpr
{
    /// <summary>
    /// Equality operator for real expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Boolean expression representing left == right.</returns>
    public static BoolExpr operator ==(RealExpr left, RealExpr right) => left.Eq(right);

    /// <summary>
    /// Inequality operator for real expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Boolean expression representing left != right.</returns>
    public static BoolExpr operator !=(RealExpr left, RealExpr right) => left.Neq(right);
}
