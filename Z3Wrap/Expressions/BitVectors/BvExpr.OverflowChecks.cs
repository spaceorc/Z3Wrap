using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Spaceorc.Z3Wrap.Expressions.BitVectors;

public sealed partial class BvExpr<TSize>
    where TSize : ISize
{
    /// <summary>
    /// Checks if addition with another bitvector would cause overflow.
    /// </summary>
    /// <param name="other">The bitvector to add.</param>
    /// <param name="signed">If true, treats operands as signed; otherwise as unsigned.</param>
    /// <returns>A Z3 boolean expression that is true if addition would not overflow.</returns>
    public BoolExpr AddNoOverflow(BvExpr<TSize> other, bool signed = false) =>
        Context.AddNoOverflow(this, other, signed);

    /// <summary>
    /// Checks if signed subtraction with another bitvector would cause overflow.
    /// </summary>
    /// <param name="other">The bitvector to subtract.</param>
    /// <returns>A Z3 boolean expression that is true if signed subtraction would not overflow.</returns>
    public BoolExpr SignedSubNoOverflow(BvExpr<TSize> other) =>
        Context.SignedSubNoOverflow(this, other);

    /// <summary>
    /// Checks if subtraction with another bitvector would cause underflow.
    /// </summary>
    /// <param name="other">The bitvector to subtract.</param>
    /// <param name="signed">If true, treats operands as signed; otherwise as unsigned.</param>
    /// <returns>A Z3 boolean expression that is true if subtraction would not underflow.</returns>
    public BoolExpr SubNoUnderflow(BvExpr<TSize> other, bool signed = true) =>
        Context.SubNoUnderflow(this, other, signed);

    /// <summary>
    /// Checks if multiplication with another bitvector would cause overflow.
    /// </summary>
    /// <param name="other">The bitvector to multiply by.</param>
    /// <param name="signed">If true, treats operands as signed; otherwise as unsigned.</param>
    /// <returns>A Z3 boolean expression that is true if multiplication would not overflow.</returns>
    public BoolExpr MulNoOverflow(BvExpr<TSize> other, bool signed = false) =>
        Context.MulNoOverflow(this, other, signed);

    /// <summary>
    /// Checks if signed multiplication with another bitvector would cause underflow.
    /// </summary>
    /// <param name="other">The bitvector to multiply by.</param>
    /// <returns>A Z3 boolean expression that is true if signed multiplication would not underflow.</returns>
    public BoolExpr SignedMulNoUnderflow(BvExpr<TSize> other) =>
        Context.SignedMulNoUnderflow(this, other);

    /// <summary>
    /// Checks if signed addition with another bitvector would cause underflow.
    /// </summary>
    /// <param name="other">The bitvector to add.</param>
    /// <returns>A Z3 boolean expression that is true if signed addition would not underflow.</returns>
    public BoolExpr SignedAddNoUnderflow(BvExpr<TSize> other) =>
        Context.SignedAddNoUnderflow(this, other);

    /// <summary>
    /// Checks if signed division with another bitvector would cause overflow.
    /// </summary>
    /// <param name="other">The bitvector to divide by.</param>
    /// <returns>A Z3 boolean expression that is true if signed division would not overflow.</returns>
    public BoolExpr SignedDivNoOverflow(BvExpr<TSize> other) =>
        Context.SignedDivNoOverflow(this, other);

    /// <summary>
    /// Checks if signed negation of this bitvector would cause overflow.
    /// </summary>
    /// <returns>A Z3 boolean expression that is true if signed negation would not overflow.</returns>
    public BoolExpr SignedNegNoOverflow() => Context.SignedNegNoOverflow(this);
}
