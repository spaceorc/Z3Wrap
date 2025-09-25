namespace Spaceorc.Z3Wrap.BitVectors;

/// <summary>
/// Provides a fluent API for creating bitvector boundary check expressions.
/// </summary>
public partial class Z3BitVecBoundaryCheckBuilder
{
    private readonly Z3Context context;

    internal Z3BitVecBoundaryCheckBuilder(Z3Context context)
    {
        this.context = context;
    }

    /// <summary>
    /// Creates a builder for checking boundary conditions on typed bitvector addition.
    /// </summary>
    /// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
    /// <param name="left">The left operand of the addition.</param>
    /// <param name="right">The right operand of the addition.</param>
    /// <returns>An operation builder for the addition operation.</returns>
    public OperationBuilder<TSize> Add<TSize>(Z3BitVec<TSize> left, Z3BitVec<TSize> right)
        where TSize : ISize => new(context, OperationType.Add, left, right);

    /// <summary>
    /// Creates a builder for checking boundary conditions on typed bitvector subtraction.
    /// </summary>
    /// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
    /// <param name="left">The left operand of the subtraction.</param>
    /// <param name="right">The right operand of the subtraction.</param>
    /// <returns>An operation builder for the subtraction operation.</returns>
    public OperationBuilder<TSize> Sub<TSize>(Z3BitVec<TSize> left, Z3BitVec<TSize> right)
        where TSize : ISize => new(context, OperationType.Sub, left, right);

    /// <summary>
    /// Creates a builder for checking boundary conditions on typed bitvector multiplication.
    /// </summary>
    /// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
    /// <param name="left">The left operand of the multiplication.</param>
    /// <param name="right">The right operand of the multiplication.</param>
    /// <returns>An operation builder for the multiplication operation.</returns>
    public OperationBuilder<TSize> Mul<TSize>(Z3BitVec<TSize> left, Z3BitVec<TSize> right)
        where TSize : ISize => new(context, OperationType.Mul, left, right);

    /// <summary>
    /// Creates a builder for checking boundary conditions on typed bitvector division.
    /// </summary>
    /// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
    /// <param name="left">The left operand of the division.</param>
    /// <param name="right">The right operand of the division.</param>
    /// <returns>An operation builder for the division operation.</returns>
    public OperationBuilder<TSize> Div<TSize>(Z3BitVec<TSize> left, Z3BitVec<TSize> right)
        where TSize : ISize => new(context, OperationType.Div, left, right);

    /// <summary>
    /// Creates a builder for checking boundary conditions on typed bitvector negation.
    /// </summary>
    /// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
    /// <param name="operand">The operand to negate.</param>
    /// <returns>An operation builder for the negation operation.</returns>
    public OperationBuilder<TSize> Neg<TSize>(Z3BitVec<TSize> operand)
        where TSize : ISize => new(context, OperationType.Neg, operand, null);
}
