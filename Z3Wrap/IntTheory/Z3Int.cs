using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.IntTheory;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
/// <summary>
/// Represents an integer expression in Z3 with unlimited precision arithmetic using BigInteger.
/// Supports natural mathematical operations, comparisons, and conversions to other numeric types.
/// All arithmetic is performed with unlimited precision - no overflow or underflow occurs.
/// </summary>
public sealed partial class Z3Int : Z3NumericExpr, IZ3ExprType<Z3Int>
{
    private Z3Int(Z3Context context, IntPtr handle)
        : base(context, handle) { }

    static Z3Int IZ3ExprType<Z3Int>.Create(Z3Context context, IntPtr handle) =>
        new(context, handle);

    static IntPtr IZ3ExprType<Z3Int>.GetSort(Z3Context context) =>
        SafeNativeMethods.Z3MkIntSort(context.Handle);
}
