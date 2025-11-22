using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Expressions.Numerics;

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
    public static StringExpr Concat(this Z3Context context, params ReadOnlySpan<StringExpr> strings)
    {
        if (strings.Length == 0)
            throw new ArgumentException("Concat requires at least one operand.", nameof(strings));

        var args = new IntPtr[strings.Length];
        for (var i = 0; i < strings.Length; i++)
            args[i] = strings[i].Handle;

        var resultHandle = context.Library.MkSeqConcat(context.Handle, (uint)args.Length, args);
        return Z3Expr.Create<StringExpr>(context, resultHandle);
    }

    /// <summary>
    /// Converts string expression to integer expression.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="expr">The string expression to convert.</param>
    /// <returns>Integer expression representing the parsed string.</returns>
    /// <remarks>Result is unspecified if the string does not represent a valid integer.</remarks>
    public static IntExpr StrToInt(this Z3Context context, StringExpr expr)
    {
        var handle = context.Library.MkStrToInt(context.Handle, expr.Handle);
        return Z3Expr.Create<IntExpr>(context, handle);
    }

    /// <summary>
    /// Creates less-than comparison for string expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Boolean expression for lexicographic less-than comparison.</returns>
    public static BoolExpr Lt(this Z3Context context, StringExpr left, StringExpr right)
    {
        var handle = context.Library.MkStrLt(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }

    /// <summary>
    /// Creates less-than-or-equal comparison for string expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Boolean expression for lexicographic less-than-or-equal comparison.</returns>
    public static BoolExpr Le(this Z3Context context, StringExpr left, StringExpr right)
    {
        var handle = context.Library.MkStrLe(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }

    /// <summary>
    /// Creates greater-than comparison for string expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Boolean expression for lexicographic greater-than comparison.</returns>
    public static BoolExpr Gt(this Z3Context context, StringExpr left, StringExpr right)
    {
        var handle = context.Library.MkStrLt(context.Handle, right.Handle, left.Handle);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }

    /// <summary>
    /// Creates greater-than-or-equal comparison for string expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>Boolean expression for lexicographic greater-than-or-equal comparison.</returns>
    public static BoolExpr Ge(this Z3Context context, StringExpr left, StringExpr right)
    {
        var handle = context.Library.MkStrLe(context.Handle, right.Handle, left.Handle);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }
}
