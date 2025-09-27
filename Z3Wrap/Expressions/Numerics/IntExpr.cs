using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.Expressions.Numerics;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
/// <summary>
/// Represents an integer expression in Z3 with unlimited precision arithmetic using BigInteger.
/// Supports natural mathematical operations, comparisons, and conversions to other numeric types.
/// All arithmetic is performed with unlimited precision - no overflow or underflow occurs.
/// </summary>
public sealed partial class IntExpr : Z3Expr, IArithmeticExpr<IntExpr>, IExprType<IntExpr>
{
    private IntExpr(Z3Context context, IntPtr handle)
        : base(context, handle) { }

    static IntExpr IExprType<IntExpr>.Create(Z3Context context, IntPtr handle) =>
        new(context, handle);

    static IntPtr IExprType<IntExpr>.Sort(Z3Context context) =>
        SafeNativeMethods.Z3MkIntSort(context.Handle);

    static IntExpr IArithmeticExpr<IntExpr>.Zero(Z3Context context) => context.Int(0);
}
