using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.RealTheory;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
/// <summary>
/// Represents a real (rational) number expression in Z3 with exact precision arithmetic.
/// Supports unlimited precision rational arithmetic operations, comparisons, and conversions.
/// All operations maintain exact precision without floating-point approximation errors.
/// </summary>
public sealed partial class Z3Real : Z3NumericExpr, IZ3ExprType<Z3Real>
{
    private Z3Real(Z3Context context, IntPtr handle)
        : base(context, handle) { }

    static Z3Real IZ3ExprType<Z3Real>.Create(Z3Context context, IntPtr handle) =>
        new(context, handle);

    static IntPtr IZ3ExprType<Z3Real>.GetSort(Z3Context context) =>
        SafeNativeMethods.Z3MkRealSort(context.Handle);
}
