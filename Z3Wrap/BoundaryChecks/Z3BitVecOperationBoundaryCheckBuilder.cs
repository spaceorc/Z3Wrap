using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Extensions;

namespace Spaceorc.Z3Wrap.BoundaryChecks;

/// <summary>
/// Provides methods for creating specific boundary check expressions for a bitvector operation.
/// </summary>
public class Z3BitVecOperationBoundaryCheckBuilder
{
    private readonly Z3Context context;
    private readonly Z3BitVecBoundaryCheckOperation operation;
    private readonly Z3BitVecExpr left;
    private readonly Z3BitVecExpr? right;

    internal Z3BitVecOperationBoundaryCheckBuilder(
        Z3Context context,
        Z3BitVecBoundaryCheckOperation operation,
        Z3BitVecExpr left,
        Z3BitVecExpr? right
    )
    {
        this.context = context;
        this.operation = operation;
        this.left = left;
        this.right = right;
    }

    /// <summary>
    /// Creates a boolean expression that is true when the operation does not overflow.
    /// </summary>
    /// <param name="signed">Whether to use signed overflow checking. Default is false (unsigned).</param>
    /// <returns>A boolean expression that is true when the operation does not overflow.</returns>
    public Z3BoolExpr NoOverflow(bool signed = false)
    {
        return operation switch
        {
            Z3BitVecBoundaryCheckOperation.Add => context.AddNoOverflow(left, right!, signed),
            Z3BitVecBoundaryCheckOperation.Sub when signed => context.SignedSubNoOverflow(
                left,
                right!
            ),
            Z3BitVecBoundaryCheckOperation.Sub when !signed => true, // unsigned subtraction can't overflow
            Z3BitVecBoundaryCheckOperation.Mul => context.MulNoOverflow(left, right!, signed),
            Z3BitVecBoundaryCheckOperation.Div when signed => context.SignedDivNoOverflow(
                left,
                right!
            ),
            Z3BitVecBoundaryCheckOperation.Div when !signed => true, // unsigned division can't overflow
            Z3BitVecBoundaryCheckOperation.Neg when signed => context.SignedNegNoOverflow(left),
            Z3BitVecBoundaryCheckOperation.Neg when !signed => true, // unsigned negation can't overflow (always underflows to valid range)
            _ => throw new InvalidOperationException($"Unsupported operation: {operation}"),
        };
    }

    /// <summary>
    /// Creates a boolean expression that is true when the operation does not underflow.
    /// </summary>
    /// <param name="signed">Whether to use signed underflow checking. Default is false (unsigned).</param>
    /// <returns>A boolean expression that is true when the operation does not underflow.</returns>
    public Z3BoolExpr NoUnderflow(bool signed = false)
    {
        return operation switch
        {
            Z3BitVecBoundaryCheckOperation.Add when signed => context.SignedAddNoUnderflow(
                left,
                right!
            ),
            Z3BitVecBoundaryCheckOperation.Add when !signed => true, // unsigned addition can't underflow
            Z3BitVecBoundaryCheckOperation.Sub => context.SubNoUnderflow(left, right!, signed),
            Z3BitVecBoundaryCheckOperation.Mul when signed => context.SignedMulNoUnderflow(
                left,
                right!
            ),
            Z3BitVecBoundaryCheckOperation.Mul when !signed => true, // unsigned multiplication can't underflow
            Z3BitVecBoundaryCheckOperation.Div => true, // division can't underflow in Z3
            Z3BitVecBoundaryCheckOperation.Neg when signed => true, // signed negation can't underflow
            Z3BitVecBoundaryCheckOperation.Neg when !signed => left == 0, // unsigned negation: no underflow only when operand is 0
            _ => throw new InvalidOperationException($"Unsupported operation: {operation}"),
        };
    }

    /// <summary>
    /// Creates a boolean expression that is true when the operation overflows.
    /// </summary>
    /// <param name="signed">Whether to use signed overflow checking. Default is false (unsigned).</param>
    /// <returns>A boolean expression that is true when the operation overflows.</returns>
    public Z3BoolExpr Overflow(bool signed = false) => !NoOverflow(signed);

    /// <summary>
    /// Creates a boolean expression that is true when the operation underflows.
    /// </summary>
    /// <param name="signed">Whether to use signed underflow checking. Default is false (unsigned).</param>
    /// <returns>A boolean expression that is true when the operation underflows.</returns>
    public Z3BoolExpr Underflow(bool signed = false) => !NoUnderflow(signed);
}
