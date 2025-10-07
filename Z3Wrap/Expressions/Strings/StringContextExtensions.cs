using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Common;

namespace Spaceorc.Z3Wrap.Expressions.Strings;

/// <summary>
/// Provides string expression creation methods for Z3Context.
/// </summary>
public static class StringContextExtensions
{
    /// <summary>
    /// Creates string expression from string value.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="value">The string value.</param>
    /// <returns>String expression representing the value.</returns>
    public static StringExpr String(this Z3Context context, string value)
    {
        var handle = context.Library.MkString(context.Handle, value);
        return Z3Expr.Create<StringExpr>(context, handle);
    }

    /// <summary>
    /// Creates string constant with specified name.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="name">The constant name.</param>
    /// <returns>String expression constant.</returns>
    public static StringExpr StringConst(this Z3Context context, string name)
    {
        var stringSort = context.Library.MkStringSort(context.Handle);
        var handle = context.Library.MkConst(context.Handle, name, stringSort);
        return Z3Expr.Create<StringExpr>(context, handle);
    }

    /// <summary>
    /// Concatenates multiple string expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="strings">The strings to concatenate.</param>
    /// <returns>Concatenated string expression.</returns>
    public static StringExpr Concat(this Z3Context context, params StringExpr[] strings)
    {
        if (strings.Length == 0)
            throw new ArgumentException("At least one string is required for concatenation", nameof(strings));

        if (strings.Length == 1)
            return strings[0];

        var handles = strings.Select(s => s.Handle).ToArray();
        var handle = context.Library.MkSeqConcat(context.Handle, (uint)handles.Length, handles);
        return Z3Expr.Create<StringExpr>(context, handle);
    }
}
