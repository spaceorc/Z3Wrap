using Spaceorc.Z3Wrap.Values;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Spaceorc.Z3Wrap.BitVecTheory;

public sealed partial class Z3BitVec<TSize>
    where TSize : ISize
{
    /// <summary>
    /// Extracts a compile-time size-validated range of bits from this bitvector starting at the specified bit.
    /// </summary>
    /// <typeparam name="TOutputSize">The output size specification implementing ISize for compile-time validation.</typeparam>
    /// <param name="startBit">The starting bit index (inclusive).</param>
    /// <returns>A type-safe Z3 bitvector expression containing the extracted bits.</returns>
    public Z3BitVec<TOutputSize> Extract<TOutputSize>(uint startBit)
        where TOutputSize : ISize => Context.Extract<TSize, TOutputSize>(this, startBit);

    /// <summary>
    /// Resizes this bitvector to a compile-time validated target size by truncating or extending.
    /// </summary>
    /// <typeparam name="TOutputSize">The target size specification implementing ISize for compile-time validation.</typeparam>
    /// <param name="signed">Whether to sign-extend when growing or truncate when shrinking.</param>
    /// <returns>A type-safe Z3 bitvector expression with the target size.</returns>
    public Z3BitVec<TOutputSize> Resize<TOutputSize>(bool signed = false)
        where TOutputSize : ISize => Context.Resize<TSize, TOutputSize>(this, signed);

    /// <summary>
    /// Repeats this bitvector to create a larger compile-time size-validated bitvector expression.
    /// </summary>
    /// <typeparam name="TOutputSize">The target size specification implementing ISize for compile-time validation.</typeparam>
    /// <returns>A type-safe Z3 bitvector expression containing the repeated pattern.</returns>
    public Z3BitVec<TOutputSize> Repeat<TOutputSize>()
        where TOutputSize : ISize => Context.Repeat<TSize, TOutputSize>(this);
}
