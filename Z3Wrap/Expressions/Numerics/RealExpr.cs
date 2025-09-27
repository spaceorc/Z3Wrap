using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.Expressions.Numerics;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
/// <summary>
/// Represents a real (rational) number expression in Z3 with exact precision arithmetic.
/// Supports unlimited precision rational arithmetic operations, comparisons, and conversions.
/// All operations maintain exact precision without floating-point approximation errors.
/// </summary>
public sealed partial class RealExpr : Z3Expr, IArithmeticExpr, IExprType<RealExpr>
{
    private RealExpr(Z3Context context, IntPtr handle)
        : base(context, handle) { }

    static RealExpr IExprType<RealExpr>.Create(Z3Context context, IntPtr handle) =>
        new(context, handle);

    static IntPtr IExprType<RealExpr>.Sort(Z3Context context) =>
        SafeNativeMethods.Z3MkRealSort(context.Handle);
}
