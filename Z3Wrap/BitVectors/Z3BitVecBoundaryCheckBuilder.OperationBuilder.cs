using Spaceorc.Z3Wrap.Expressions;

namespace Spaceorc.Z3Wrap.BitVectors;

public partial class Z3BitVecBoundaryCheckBuilder
{
    /// <summary>
    /// Provides methods for creating specific boundary check expressions for a bitvector operation.
    /// </summary>
    public class OperationBuilder<TSize>
        where TSize : ISize
    {
        private readonly Z3Context context;
        private readonly OperationType operationType;
        private readonly Z3BitVec<TSize> left;
        private readonly Z3BitVec<TSize>? right;

        internal OperationBuilder(
            Z3Context context,
            OperationType operationType,
            Z3BitVec<TSize> left,
            Z3BitVec<TSize>? right
        )
        {
            this.context = context;
            this.operationType = operationType;
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
            return operationType switch
            {
                OperationType.Add => context.AddNoOverflow(left, right!, signed),
                OperationType.Sub when signed => context.SignedSubNoOverflow(left, right!),
                OperationType.Sub when !signed => true, // unsigned subtraction can't overflow
                OperationType.Mul => context.MulNoOverflow(left, right!, signed),
                OperationType.Div when signed => context.SignedDivNoOverflow(left, right!),
                OperationType.Div when !signed => true, // unsigned division can't overflow
                OperationType.Neg when signed => context.SignedNegNoOverflow(left),
                OperationType.Neg when !signed => true, // unsigned negation can't overflow (always underflows to valid range)
                _ => throw new InvalidOperationException($"Unsupported operation: {operationType}"),
            };
        }

        /// <summary>
        /// Creates a boolean expression that is true when the operation does not underflow.
        /// </summary>
        /// <param name="signed">Whether to use signed underflow checking. Default is false (unsigned).</param>
        /// <returns>A boolean expression that is true when the operation does not underflow.</returns>
        public Z3BoolExpr NoUnderflow(bool signed = false)
        {
            return operationType switch
            {
                OperationType.Add when signed => context.SignedAddNoUnderflow(left, right!),
                OperationType.Add when !signed => true, // unsigned addition can't underflow
                OperationType.Sub => context.SubNoUnderflow(left, right!, signed),
                OperationType.Mul when signed => context.SignedMulNoUnderflow(left, right!),
                OperationType.Mul when !signed => true, // unsigned multiplication can't underflow
                OperationType.Div => true, // division can't underflow in Z3
                OperationType.Neg when signed => true, // signed negation can't underflow
                OperationType.Neg when !signed => left == 0, // unsigned negation: no underflow only when operand is 0
                _ => throw new InvalidOperationException($"Unsupported operation: {operationType}"),
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
}
