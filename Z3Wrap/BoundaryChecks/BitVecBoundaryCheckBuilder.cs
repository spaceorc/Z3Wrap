using Spaceorc.Z3Wrap.Expressions;
using System.Numerics;

namespace Spaceorc.Z3Wrap.BoundaryChecks;

/// <summary>
/// Provides a fluent API for creating bitvector boundary check expressions.
/// </summary>
public class BitVecBoundaryCheckBuilder
{
    private readonly Z3Context context;

    internal BitVecBoundaryCheckBuilder(Z3Context context)
    {
        this.context = context;
    }

    /// <summary>
    /// Creates a builder for checking boundary conditions on bitvector addition.
    /// </summary>
    /// <param name="left">The left operand of the addition.</param>
    /// <param name="right">The right operand of the addition.</param>
    /// <returns>An operation builder for the addition operation.</returns>
    public BitVecOperationBuilder Add(Z3BitVecExpr left, Z3BitVecExpr right) => new(context, BoundaryOperation.Add, left, right);

    /// <summary>
    /// Creates a builder for checking boundary conditions on bitvector subtraction.
    /// </summary>
    /// <param name="left">The left operand of the subtraction.</param>
    /// <param name="right">The right operand of the subtraction.</param>
    /// <returns>An operation builder for the subtraction operation.</returns>
    public BitVecOperationBuilder Sub(Z3BitVecExpr left, Z3BitVecExpr right) => new(context, BoundaryOperation.Sub, left, right);

    /// <summary>
    /// Creates a builder for checking boundary conditions on bitvector multiplication.
    /// </summary>
    /// <param name="left">The left operand of the multiplication.</param>
    /// <param name="right">The right operand of the multiplication.</param>
    /// <returns>An operation builder for the multiplication operation.</returns>
    public BitVecOperationBuilder Mul(Z3BitVecExpr left, Z3BitVecExpr right) => new(context, BoundaryOperation.Mul, left, right);

    /// <summary>
    /// Creates a builder for checking boundary conditions on bitvector division.
    /// </summary>
    /// <param name="left">The left operand of the division.</param>
    /// <param name="right">The right operand of the division.</param>
    /// <returns>An operation builder for the division operation.</returns>
    public BitVecOperationBuilder Div(Z3BitVecExpr left, Z3BitVecExpr right) => new(context, BoundaryOperation.Div, left, right);

    /// <summary>
    /// Creates a builder for checking boundary conditions on bitvector negation.
    /// </summary>
    /// <param name="operand">The operand to negate.</param>
    /// <returns>An operation builder for the negation operation.</returns>
    public BitVecOperationBuilder Neg(Z3BitVecExpr operand) => new(context, BoundaryOperation.Neg, operand, null);

    // BigInteger overloads

    /// <summary>
    /// Creates a builder for checking boundary conditions on bitvector addition with BigInteger.
    /// </summary>
    /// <param name="left">The left operand of the addition.</param>
    /// <param name="right">The right operand of the addition.</param>
    /// <returns>An operation builder for the addition operation.</returns>
    public BitVecOperationBuilder Add(Z3BitVecExpr left, BigInteger right) => new(context, BoundaryOperation.Add, left, context.BitVec(right, left.Size));

    /// <summary>
    /// Creates a builder for checking boundary conditions on bitvector addition with BigInteger.
    /// </summary>
    /// <param name="left">The left operand of the addition.</param>
    /// <param name="right">The right operand of the addition.</param>
    /// <returns>An operation builder for the addition operation.</returns>
    public BitVecOperationBuilder Add(BigInteger left, Z3BitVecExpr right) => new(context, BoundaryOperation.Add, context.BitVec(left, right.Size), right);

    /// <summary>
    /// Creates a builder for checking boundary conditions on bitvector subtraction with BigInteger.
    /// </summary>
    /// <param name="left">The left operand of the subtraction.</param>
    /// <param name="right">The right operand of the subtraction.</param>
    /// <returns>An operation builder for the subtraction operation.</returns>
    public BitVecOperationBuilder Sub(Z3BitVecExpr left, BigInteger right) => new(context, BoundaryOperation.Sub, left, context.BitVec(right, left.Size));

    /// <summary>
    /// Creates a builder for checking boundary conditions on bitvector subtraction with BigInteger.
    /// </summary>
    /// <param name="left">The left operand of the subtraction.</param>
    /// <param name="right">The right operand of the subtraction.</param>
    /// <returns>An operation builder for the subtraction operation.</returns>
    public BitVecOperationBuilder Sub(BigInteger left, Z3BitVecExpr right) => new(context, BoundaryOperation.Sub, context.BitVec(left, right.Size), right);

    /// <summary>
    /// Creates a builder for checking boundary conditions on bitvector multiplication with BigInteger.
    /// </summary>
    /// <param name="left">The left operand of the multiplication.</param>
    /// <param name="right">The right operand of the multiplication.</param>
    /// <returns>An operation builder for the multiplication operation.</returns>
    public BitVecOperationBuilder Mul(Z3BitVecExpr left, BigInteger right) => new(context, BoundaryOperation.Mul, left, context.BitVec(right, left.Size));

    /// <summary>
    /// Creates a builder for checking boundary conditions on bitvector multiplication with BigInteger.
    /// </summary>
    /// <param name="left">The left operand of the multiplication.</param>
    /// <param name="right">The right operand of the multiplication.</param>
    /// <returns>An operation builder for the multiplication operation.</returns>
    public BitVecOperationBuilder Mul(BigInteger left, Z3BitVecExpr right) => new(context, BoundaryOperation.Mul, context.BitVec(left, right.Size), right);

    /// <summary>
    /// Creates a builder for checking boundary conditions on bitvector division with BigInteger.
    /// </summary>
    /// <param name="left">The left operand of the division.</param>
    /// <param name="right">The right operand of the division.</param>
    /// <returns>An operation builder for the division operation.</returns>
    public BitVecOperationBuilder Div(Z3BitVecExpr left, BigInteger right) => new(context, BoundaryOperation.Div, left, context.BitVec(right, left.Size));

    /// <summary>
    /// Creates a builder for checking boundary conditions on bitvector division with BigInteger.
    /// </summary>
    /// <param name="left">The left operand of the division.</param>
    /// <param name="right">The right operand of the division.</param>
    /// <returns>An operation builder for the division operation.</returns>
    public BitVecOperationBuilder Div(BigInteger left, Z3BitVecExpr right) => new(context, BoundaryOperation.Div, context.BitVec(left, right.Size), right);
}