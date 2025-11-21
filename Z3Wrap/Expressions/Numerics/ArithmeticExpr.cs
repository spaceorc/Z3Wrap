using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Common;

namespace Spaceorc.Z3Wrap.Expressions.Numerics;

/// <summary>
/// Base class for arithmetic expressions (integers and real numbers).
/// </summary>
public abstract class ArithmeticExpr : Z3Expr, INumericExpr
{
    /// <summary>
    /// Initializes a new instance of the ArithmeticExpr class.
    /// </summary>
    protected ArithmeticExpr(Z3Context context, IntPtr handle)
        : base(context, handle) { }

    /// <summary>
    /// Creates the appropriate ArithmeticExpr subtype (IntExpr or RealExpr) based on Z3 sort.
    /// Used for dynamic creation from optimizer results.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="handle">The Z3 AST handle.</param>
    /// <returns>An IntExpr or RealExpr instance.</returns>
    internal static ArithmeticExpr CreateDynamic(Z3Context context, IntPtr handle)
    {
        // CRITICAL: Track handle immediately to prevent premature garbage collection
        context.TrackHandle(handle);

        // Get sort info to determine which type to create
        var sort = context.Library.GetSort(context.Handle, handle);
        var sortKind = context.Library.GetSortKind(context.Handle, sort);

        // Create the appropriate wrapper using Z3Expr.Create which calls the static interface method
        // Note: This will call TrackHandle again (in constructor), but that's OK - just a redundant IncRef
        return sortKind switch
        {
            Z3Library.SortKind.Z3_INT_SORT => Create<IntExpr>(context, handle),
            Z3Library.SortKind.Z3_REAL_SORT => Create<RealExpr>(context, handle),
            _ => throw new InvalidOperationException($"Expected Int or Real sort from optimizer, but got {sortKind}"),
        };
    }
}
