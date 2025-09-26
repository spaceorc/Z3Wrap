using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.BitVecTheory;

#pragma warning disable CS0660, CS0661 // Type defines operator == or operator != but does not override Object.Equals/GetHashCode (handled by base class)
/// <summary>
/// Represents a compile-time size-validated bitvector expression in Z3 with comprehensive bitwise and arithmetic operations.
/// Provides type-safe operations where size mismatches are caught at compile time.
/// Supports both unsigned and signed operations, bit manipulation, and conversion to other numeric types.
/// </summary>
/// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
public sealed partial class Z3BitVec<TSize> : Z3NumericExpr, IZ3ExprType<Z3BitVec<TSize>>
    where TSize : ISize
{
    /// <summary>
    /// Gets the bit width (size in bits) of this bitvector expression, known at compile time.
    /// </summary>
    public static uint Size => TSize.Size;

    private Z3BitVec(Z3Context context, IntPtr handle)
        : base(context, handle) { }

    static Z3BitVec<TSize> IZ3ExprType<Z3BitVec<TSize>>.Create(Z3Context context, IntPtr handle) =>
        new(context, handle);

    static IntPtr IZ3ExprType<Z3BitVec<TSize>>.GetSort(Z3Context context) =>
        SafeNativeMethods.Z3MkBvSort(context.Handle, TSize.Size);

    /// <summary>
    /// Creates a bitvector representing zero with the compile-time specified bit width.
    /// </summary>
    public static Z3BitVec<TSize> Zero => Z3Context.Current.BitVec(BitVec<TSize>.Zero);

    /// <summary>
    /// Creates a bitvector representing one with the compile-time specified bit width.
    /// </summary>
    public static Z3BitVec<TSize> One => Z3Context.Current.BitVec(BitVec<TSize>.One);

    /// <summary>
    /// Creates a bitvector with the maximum possible value (all bits set to 1).
    /// </summary>
    public static Z3BitVec<TSize> Max => Z3Context.Current.BitVec(BitVec<TSize>.Max);

    /// <summary>
    /// Creates a bitvector with only the sign bit set (most significant bit).
    /// </summary>
    public static Z3BitVec<TSize> SignBit => Z3Context.Current.BitVec(BitVec<TSize>.SignBit);

    /// <summary>
    /// Creates a bitvector with all bits set to 1 (alias for Max).
    /// </summary>
    public static Z3BitVec<TSize> AllOnes => Max;

    /// <summary>
    /// Returns a string representation of this bitvector expression including its bit width.
    /// </summary>
    /// <returns>A string in the format "Z3BitVec[size](expression)".</returns>
    public override string ToString() => $"Z3BitVec[{Size}]({base.ToString()})";
}
