using Z3Wrap.Interop;

namespace Z3Wrap.Expressions;

public abstract partial class Z3Expr
{
    internal static Z3Expr Create(Z3Context context, IntPtr handle)
    {
        // Track expression for memory management
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
            // Z3SortKind.BV => ArrayFactory<Z3BitVecExpr>.CreateArray(context, handle, arraySort),
            _ => throw new InvalidOperationException($"Unsupported array domain sort kind: {domainKind}")
        };
    }

    private static class ArrayFactory<TIndex> where TIndex : Z3Expr
    {
        public static Z3Expr CreateArray(Z3Context context, IntPtr handle, IntPtr arraySort)
        {
            var rangeSort = NativeMethods.Z3GetArraySortRange(context.Handle, arraySort);
            var rangeKind = (Z3SortKind)NativeMethods.Z3GetSortKind(context.Handle, rangeSort);

            return rangeKind switch
            {
                Z3SortKind.Bool => new Z3ArrayExpr<TIndex, Z3BoolExpr>(context, handle),
                Z3SortKind.Int => new Z3ArrayExpr<TIndex, Z3IntExpr>(context, handle),
                Z3SortKind.Real => new Z3ArrayExpr<TIndex, Z3RealExpr>(context, handle),
                // Z3SortKind.BV => CreateBitVectorArrayRange<TIndex>(context, handle, rangeSort),
                _ => throw new InvalidOperationException($"Unsupported array range sort kind: {rangeKind}")
            };
        }

        // private static Z3Expr CreateBitVectorArrayRange<TIndexType>(Z3Context context, IntPtr handle, IntPtr rangeSort)
        //     where TIndexType : Z3Expr
        // {
        //     var size = NativeMethods.Z3GetBvSortSize(context.Handle, rangeSort);
        //     return new Z3ArrayExpr<TIndexType, Z3BitVecExpr>(context, handle);
        // }
    }
}