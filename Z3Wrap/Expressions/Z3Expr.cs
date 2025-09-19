using Spaceorc.Z3Wrap.Interop;

namespace Spaceorc.Z3Wrap.Expressions;

/// <summary>
/// Base class for all Z3 expressions, providing common functionality for equality, inequality, and string representation.
/// All Z3 expressions are immutable and associated with a specific Z3 context.
/// </summary>
public abstract class Z3Expr(Z3Context context, IntPtr handle)
{
    internal IntPtr Handle { get; } =
        handle != IntPtr.Zero ? handle : throw new ArgumentException("Invalid handle", nameof(handle));

    /// <summary>
    /// Gets the Z3 context that owns this expression.
    /// </summary>
    public Z3Context Context { get; } = context;

    /// <summary>
    /// Determines whether two Z3 expressions are equal using the == operator.
    /// Creates a Boolean expression representing the equality constraint.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A Boolean expression representing left == right.</returns>
    public static Z3BoolExpr operator ==(Z3Expr left, Z3Expr right) => left.Eq(right);

    /// <summary>
    /// Determines whether two Z3 expressions are not equal using the != operator.
    /// Creates a Boolean expression representing the inequality constraint.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>A Boolean expression representing left != right.</returns>
    public static Z3BoolExpr operator !=(Z3Expr left, Z3Expr right) => left.Neq(right);

    /// <summary>
    /// Creates a Boolean expression representing equality with another expression.
    /// </summary>
    /// <param name="other">The expression to compare with.</param>
    /// <returns>A Boolean expression representing this == other.</returns>
    public Z3BoolExpr Eq(Z3Expr other) => Context.Eq(this, other);

    /// <summary>
    /// Creates a Boolean expression representing inequality with another expression.
    /// </summary>
    /// <param name="other">The expression to compare with.</param>
    /// <returns>A Boolean expression representing this != other.</returns>
    public Z3BoolExpr Neq(Z3Expr other) => Context.Neq(this, other);

    /// <summary>
    /// Determines whether this expression is equal to the specified object.
    /// Uses handle-based equality for Z3 expressions.
    /// </summary>
    /// <param name="obj">The object to compare with this expression.</param>
    /// <returns>true if the object is a Z3Expr with the same handle; otherwise, false.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is Z3Expr expr)
            return Handle.Equals(expr.Handle);
        return false;
    }

    /// <summary>
    /// Returns the hash code for this expression based on its Z3 handle.
    /// </summary>
    /// <returns>A 32-bit signed integer hash code.</returns>
    public override int GetHashCode() => handle.GetHashCode();

    /// <summary>
    /// Returns a string representation of this expression in Z3's native format.
    /// </summary>
    /// <returns>A string representation of the expression, or status information if disposed/invalid.</returns>
    public override string ToString()
    {
        try
        {
            var stringPtr = NativeMethods.Z3AstToString(Context.Handle, Handle);
            return System.Runtime.InteropServices.Marshal.PtrToStringAnsi(stringPtr) ?? "<invalid>";
        }
        catch (ObjectDisposedException)
        {
            return "<disposed>";
        }
        catch
        {
            return "<error>";
        }
    }

    internal static Z3Expr Create(Z3Context context, IntPtr handle)
    {
        context.TrackExpression(handle);

        var sort = NativeMethods.Z3GetSort(context.Handle, handle);
        var sortKind = (Z3SortKind)NativeMethods.Z3GetSortKind(context.Handle, sort);

        return sortKind switch
        {
            Z3SortKind.Bool => new Z3BoolExpr(context, handle),
            Z3SortKind.Int => new Z3IntExpr(context, handle),
            Z3SortKind.Real => new Z3RealExpr(context, handle),
            Z3SortKind.BV => CreateBitVectorExpression(context, handle, sort),
            Z3SortKind.Array => CreateArrayExpression(context, handle, sort),
            _ => throw new InvalidOperationException($"Unsupported sort kind: {sortKind}")
        };
    }

    private static Z3BitVecExpr CreateBitVectorExpression(Z3Context context, IntPtr handle, IntPtr bvSort)
    {
        var size = NativeMethods.Z3GetBvSortSize(context.Handle, bvSort);
        return new Z3BitVecExpr(context, handle, size);
    }

    private static Z3Expr CreateArrayExpression(Z3Context context, IntPtr handle, IntPtr arraySort)
    {
        var domainSort = NativeMethods.Z3GetArraySortDomain(context.Handle, arraySort);
        var domainKind = (Z3SortKind)NativeMethods.Z3GetSortKind(context.Handle, domainSort);

        return domainKind switch
        {
            Z3SortKind.Bool => ArrayFactory<Z3BoolExpr>.CreateArray(context, handle, arraySort),
            Z3SortKind.Int => ArrayFactory<Z3IntExpr>.CreateArray(context, handle, arraySort),
            Z3SortKind.Real => ArrayFactory<Z3RealExpr>.CreateArray(context, handle, arraySort),
            _ => throw new InvalidOperationException($"Unsupported array domain sort kind: {domainKind}")
        };
    }

    private static class ArrayFactory<TIndex> where TIndex : Z3Expr
    {
        internal static Z3Expr CreateArray(Z3Context context, IntPtr handle, IntPtr arraySort)
        {
            var rangeSort = NativeMethods.Z3GetArraySortRange(context.Handle, arraySort);
            var rangeKind = (Z3SortKind)NativeMethods.Z3GetSortKind(context.Handle, rangeSort);

            return rangeKind switch
            {
                Z3SortKind.Bool => new Z3ArrayExpr<TIndex, Z3BoolExpr>(context, handle),
                Z3SortKind.Int => new Z3ArrayExpr<TIndex, Z3IntExpr>(context, handle),
                Z3SortKind.Real => new Z3ArrayExpr<TIndex, Z3RealExpr>(context, handle),
                _ => throw new InvalidOperationException($"Unsupported array range sort kind: {rangeKind}")
            };
        }
    }
}