using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Expressions.Numerics;

namespace Spaceorc.Z3Wrap.Expressions.Strings;

/// <summary>
/// Provides string conversion methods for integer expressions.
/// </summary>
public static class IntExprStringExtensions
{
    /// <summary>
    /// Converts integer expression to string expression.
    /// </summary>
    /// <param name="intExpr">The integer expression.</param>
    /// <returns>String expression representing the integer.</returns>
    public static StringExpr ToStr(this IntExpr intExpr)
    {
        var handle = intExpr.Context.Library.MkIntToStr(intExpr.Context.Handle, intExpr.Handle);
        return Z3Expr.Create<StringExpr>(intExpr.Context, handle);
    }
}
