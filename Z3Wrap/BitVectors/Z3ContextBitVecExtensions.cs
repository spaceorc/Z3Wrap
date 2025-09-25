using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.BitVectors;

/// <summary>
/// Provides extension methods for Z3Context to work with compile-time size-validated bitvector expressions.
/// Supports type-safe bitvector operations where size mismatches are caught at compile time.
/// </summary>
public static partial class Z3ContextBitVecExtensions2
{
    /// <summary>
    /// Creates a compile-time size-validated bitvector constant with the specified name.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="name">The name of the bitvector constant.</param>
    /// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
    /// <returns>A type-safe Z3 bitvector expression representing the constant.</returns>
    public static Z3BitVec<TSize> BitVecConst<TSize>(this Z3Context context, string name)
        where TSize : ISize
    {
        var sort = SafeNativeMethods.Z3MkBvSort(context.Handle, TSize.Size);
        using var namePtr = new AnsiStringPtr(name);
        var symbol = SafeNativeMethods.Z3MkStringSymbol(context.Handle, namePtr);
        var handle = SafeNativeMethods.Z3MkConst(context.Handle, symbol, sort);

        return Z3BitVec<TSize>.Create(context, handle);
    }

    /// <summary>
    /// Creates a compile-time size-validated bitvector expression from a typed BitVec value.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="value">The typed BitVec value to convert to an expression.</param>
    /// <typeparam name="TSize">The size specification implementing ISize for compile-time validation.</typeparam>
    /// <returns>A type-safe Z3 bitvector expression representing the value.</returns>
    public static Z3BitVec<TSize> BitVec<TSize>(this Z3Context context, BitVec<TSize> value)
        where TSize : ISize
    {
        using var numeralPtr = new AnsiStringPtr(value.ToString());
        var sort = SafeNativeMethods.Z3MkBvSort(context.Handle, TSize.Size);
        var handle = SafeNativeMethods.Z3MkNumeral(context.Handle, numeralPtr, sort);

        return Z3BitVec<TSize>.Create(context, handle);
    }
}
