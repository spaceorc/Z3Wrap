using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Expressions.Logic;

namespace Spaceorc.Z3Wrap.Expressions.BitVectors;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
public sealed partial class BvExpr<TSize>
{
    /// <summary>
    /// Equality operator for bitvector expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Boolean expression representing left == right.</returns>
    public static BoolExpr operator ==(BvExpr<TSize> left, BvExpr<TSize> right) => left.Eq(right);

    /// <summary>
    /// Inequality operator for bitvector expressions.
    /// </summary>
    /// <param name="left">Left operand.</param>
    /// <param name="right">Right operand.</param>
    /// <returns>Boolean expression representing left != right.</returns>
    public static BoolExpr operator !=(BvExpr<TSize> left, BvExpr<TSize> right) => left.Neq(right);
}
