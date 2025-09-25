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
        private readonly Operation operation;
        private readonly Z3BitVec<TSize> left;
        private readonly Z3BitVec<TSize>? right;

        internal OperationBuilder(
            Z3Context context,
            Operation operation,
            Z3BitVec<TSize> left,
            Z3BitVec<TSize>? right
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
                Operation.Add => context.AddNoOverflow(left, right!, signed),
                Operation.Sub when signed => context.SignedSubNoOverflow(
                    left,
                    right!
                ),
                Operation.Sub when !signed => true, // unsigned subtraction can't overflow
                Operation.Mul => context.MulNoOverflow(left, right!, signed),
                Operation.Div when signed => context.SignedDivNoOverflow(
                    left,
                    right!
                ),
                Operation.Div when !signed => true, // unsigned division can't overflow
                Operation.Neg when signed => context.SignedNegNoOverflow(left),
                Operation
                    .Neg when !signed => true, // unsigned negation can't overflow (always underflows to valid range)
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
                Operation.Add when signed => context.SignedAddNoUnderflow(
                    left,
                    right!
                ),
                Operation.Add when !signed => true, // unsigned addition can't underflow
                Operation.Sub => context.SubNoUnderflow(left, right!, signed),
                Operation.Mul when signed => context.SignedMulNoUnderflow(
                    left,
                    right!
                ),
                Operation.Mul when !signed => true, // unsigned multiplication can't underflow
                Operation.Div => true, // division can't underflow in Z3
                Operation.Neg when signed => true, // signed negation can't underflow
                Operation
                    .Neg when !signed => left == 0, // unsigned negation: no underflow only when operand is 0
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
}