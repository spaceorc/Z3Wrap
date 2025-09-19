using System.Numerics;
using Spaceorc.Z3Wrap.DataTypes;
using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.Extensions;

public static partial class Z3ContextExtensions
{
    /// <summary>
    /// Creates a bitvector constant with the specified name and bit width.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="name">The name of the bitvector constant.</param>
    /// <param name="size">The bit width of the bitvector.</param>
    /// <returns>A Z3 bitvector expression representing the constant.</returns>
    public static Z3BitVecExpr BitVecConst(this Z3Context context, string name, uint size)
    {
        var sort = NativeMethods.Z3MkBvSort(context.Handle, size);
        using var namePtr = new AnsiStringPtr(name);
        var symbol = NativeMethods.Z3MkStringSymbol(context.Handle, namePtr);
        var handle = NativeMethods.Z3MkConst(context.Handle, symbol, sort);

        return Z3BitVecExpr.Create(context, handle);
    }

    /// <summary>
    /// Creates a bitvector expression from a BitVec value.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="value">The BitVec value to convert to an expression.</param>
    /// <returns>A Z3 bitvector expression representing the value.</returns>
    public static Z3BitVecExpr BitVec(this Z3Context context, BitVec value)
    {
        using var numeralPtr = new AnsiStringPtr(value.ToString());
        var sort = NativeMethods.Z3MkBvSort(context.Handle, value.Size);
        var handle = NativeMethods.Z3MkNumeral(context.Handle, numeralPtr, sort);

        return Z3BitVecExpr.Create(context, handle);
    }

    /// <summary>
    /// Creates a bitvector expression from a BigInteger value with the specified bit width.
    /// Works with unlimited precision values.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="value">The BigInteger value to convert to a bitvector.</param>
    /// <param name="size">The bit width of the bitvector.</param>
    /// <returns>A Z3 bitvector expression representing the value.</returns>
    public static Z3BitVecExpr BitVec(this Z3Context context, BigInteger value, uint size)
    {
        return context.BitVec(new BitVec(value, size));
    }
}