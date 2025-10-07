using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.BitVectors;
using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Spaceorc.Z3Wrap.Expressions.Strings;

/// <summary>
/// Provides character expression creation methods for Z3Context.
/// </summary>
public static class CharContextExtensions
{
    /// <summary>
    /// Creates character expression from Unicode codepoint.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="codepoint">The Unicode codepoint.</param>
    /// <returns>Character expression representing the codepoint.</returns>
    public static CharExpr Char(this Z3Context context, uint codepoint)
    {
        var handle = context.Library.MkChar(context.Handle, codepoint);
        return Z3Expr.Create<CharExpr>(context, handle);
    }

    /// <summary>
    /// Creates character expression from char value.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="value">The character value.</param>
    /// <returns>Character expression representing the character.</returns>
    public static CharExpr Char(this Z3Context context, char value)
    {
        return context.Char((uint)value);
    }

    /// <summary>
    /// Creates character constant with specified name.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="name">The constant name.</param>
    /// <returns>Character expression constant.</returns>
    public static CharExpr CharConst(this Z3Context context, string name)
    {
        var charSort = context.Library.MkCharSort(context.Handle);
        var handle = context.Library.MkConst(context.Handle, name, charSort);
        return Z3Expr.Create<CharExpr>(context, handle);
    }
}
