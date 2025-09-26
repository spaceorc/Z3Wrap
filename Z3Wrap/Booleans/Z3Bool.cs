using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.Booleans;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
/// <summary>
/// Represents a Boolean expression in Z3 with support for logical operations, comparisons, and natural syntax.
/// Supports implicit conversion from bool values and provides comprehensive logical operators.
/// </summary>
public sealed partial class Z3Bool : Z3Expr, IZ3ExprType<Z3Bool>
{
    private Z3Bool(Z3Context context, IntPtr handle)
        : base(context, handle) { }

    static Z3Bool IZ3ExprType<Z3Bool>.Create(Z3Context context, IntPtr handle) =>
        new(context, handle);

    static IntPtr IZ3ExprType<Z3Bool>.GetSort(Z3Context context) =>
        SafeNativeMethods.Z3MkBoolSort(context.Handle);

}
#pragma warning restore CS0660, CS0661
