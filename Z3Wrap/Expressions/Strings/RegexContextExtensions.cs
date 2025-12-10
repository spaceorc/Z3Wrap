using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Logic;

namespace Spaceorc.Z3Wrap.Expressions.Strings;

/// <summary>
/// Provides regular expression creation methods for Z3Context.
/// </summary>
public static class RegexContextExtensions
{
    /// <summary>
    /// Creates regex that matches the specified string literally.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="pattern">The string pattern to match.</param>
    /// <returns>Regular expression matching the literal string.</returns>
    public static RegexExpr Regex(this Z3Context context, string pattern)
    {
        var stringExpr = context.String(pattern);
        var handle = context.Library.MkSeqToRe(context.Handle, stringExpr.Handle);
        return Z3Expr.Create<RegexExpr>(context, handle);
    }

    /// <summary>
    /// Creates regex from a string expression.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="stringExpr">The string expression to convert.</param>
    /// <returns>Regular expression matching the string.</returns>
    public static RegexExpr Regex(this Z3Context context, StringExpr stringExpr)
    {
        var handle = context.Library.MkSeqToRe(context.Handle, stringExpr.Handle);
        return Z3Expr.Create<RegexExpr>(context, handle);
    }

    /// <summary>
    /// Creates regex constant with specified name.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="name">The constant name.</param>
    /// <returns>Regular expression constant.</returns>
    public static RegexExpr RegexConst(this Z3Context context, string name)
    {
        var stringSort = context.Library.MkStringSort(context.Handle);
        var reSort = context.Library.MkReSort(context.Handle, stringSort);
        var handle = context.Library.MkConst(context.Handle, name, reSort);
        return Z3Expr.Create<RegexExpr>(context, handle);
    }

    /// <summary>
    /// Creates regex that matches any single character.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <returns>Regular expression matching any character.</returns>
    public static RegexExpr RegexAllChar(this Z3Context context)
    {
        var stringSort = context.Library.MkStringSort(context.Handle);
        var reSort = context.Library.MkReSort(context.Handle, stringSort);
        var handle = context.Library.MkReAllchar(context.Handle, reSort);
        return Z3Expr.Create<RegexExpr>(context, handle);
    }

    /// <summary>
    /// Creates regex that matches character range.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="from">Start character of range (inclusive).</param>
    /// <param name="to">End character of range (inclusive).</param>
    /// <returns>Regular expression matching character range.</returns>
    public static RegexExpr RegexRange(this Z3Context context, char from, char to)
    {
        var fromStr = context.String(from.ToString());
        var toStr = context.String(to.ToString());
        var handle = context.Library.MkReRange(context.Handle, fromStr.Handle, toStr.Handle);
        return Z3Expr.Create<RegexExpr>(context, handle);
    }

    /// <summary>
    /// Creates regex that matches character range from string expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="from">Start string (length 1).</param>
    /// <param name="to">End string (length 1).</param>
    /// <returns>Regular expression matching character range.</returns>
    public static RegexExpr RegexRange(this Z3Context context, StringExpr from, StringExpr to)
    {
        var handle = context.Library.MkReRange(context.Handle, from.Handle, to.Handle);
        return Z3Expr.Create<RegexExpr>(context, handle);
    }

    /// <summary>
    /// Creates empty regex that matches no strings.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <returns>Regular expression matching no strings.</returns>
    public static RegexExpr RegexEmpty(this Z3Context context)
    {
        var stringSort = context.Library.MkStringSort(context.Handle);
        var reSort = context.Library.MkReSort(context.Handle, stringSort);
        var handle = context.Library.MkReEmpty(context.Handle, reSort);
        return Z3Expr.Create<RegexExpr>(context, handle);
    }

    /// <summary>
    /// Creates full regex that matches all strings.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <returns>Regular expression matching all strings.</returns>
    public static RegexExpr RegexFull(this Z3Context context)
    {
        var stringSort = context.Library.MkStringSort(context.Handle);
        var reSort = context.Library.MkReSort(context.Handle, stringSort);
        var handle = context.Library.MkReFull(context.Handle, reSort);
        return Z3Expr.Create<RegexExpr>(context, handle);
    }

    /// <summary>
    /// Creates union of multiple regular expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="regexes">The regex patterns to unite.</param>
    /// <returns>Regular expression matching any of the patterns.</returns>
    public static RegexExpr RegexUnion(this Z3Context context, params IEnumerable<RegexExpr> regexes)
    {
        var args = regexes.Select(r => r.Handle).ToArray();
        if (args.Length == 0)
            throw new ArgumentException("RegexUnion requires at least one operand.", nameof(regexes));

        var handle = context.Library.MkReUnion(context.Handle, (uint)args.Length, args);
        return Z3Expr.Create<RegexExpr>(context, handle);
    }

    /// <summary>
    /// Creates concatenation of multiple regular expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="regexes">The regex patterns to concatenate.</param>
    /// <returns>Regular expression matching concatenated patterns.</returns>
    public static RegexExpr RegexConcat(this Z3Context context, params IEnumerable<RegexExpr> regexes)
    {
        var args = regexes.Select(r => r.Handle).ToArray();
        if (args.Length == 0)
            throw new ArgumentException("RegexConcat requires at least one operand.", nameof(regexes));

        var handle = context.Library.MkReConcat(context.Handle, (uint)args.Length, args);
        return Z3Expr.Create<RegexExpr>(context, handle);
    }

    /// <summary>
    /// Creates intersection of multiple regular expressions.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="regexes">The regex patterns to intersect.</param>
    /// <returns>Regular expression matching all patterns.</returns>
    public static RegexExpr RegexIntersect(this Z3Context context, params IEnumerable<RegexExpr> regexes)
    {
        var args = regexes.Select(r => r.Handle).ToArray();
        if (args.Length == 0)
            throw new ArgumentException("RegexIntersect requires at least one operand.", nameof(regexes));

        var handle = context.Library.MkReIntersect(context.Handle, (uint)args.Length, args);
        return Z3Expr.Create<RegexExpr>(context, handle);
    }

    /// <summary>
    /// Creates Kleene star (zero or more repetitions) of a regex.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="regex">The regex to repeat.</param>
    /// <returns>Regular expression matching zero or more repetitions.</returns>
    public static RegexExpr RegexStar(this Z3Context context, RegexExpr regex)
    {
        var handle = context.Library.MkReStar(context.Handle, regex.Handle);
        return Z3Expr.Create<RegexExpr>(context, handle);
    }

    /// <summary>
    /// Creates Kleene plus (one or more repetitions) of a regex.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="regex">The regex to repeat.</param>
    /// <returns>Regular expression matching one or more repetitions.</returns>
    public static RegexExpr RegexPlus(this Z3Context context, RegexExpr regex)
    {
        var handle = context.Library.MkRePlus(context.Handle, regex.Handle);
        return Z3Expr.Create<RegexExpr>(context, handle);
    }

    /// <summary>
    /// Creates optional match (zero or one occurrence) of a regex.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="regex">The regex to make optional.</param>
    /// <returns>Regular expression matching zero or one occurrence.</returns>
    public static RegexExpr RegexOption(this Z3Context context, RegexExpr regex)
    {
        var handle = context.Library.MkReOption(context.Handle, regex.Handle);
        return Z3Expr.Create<RegexExpr>(context, handle);
    }

    /// <summary>
    /// Creates complement of a regex.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="regex">The regex to complement.</param>
    /// <returns>Regular expression matching all strings not matched by the regex.</returns>
    public static RegexExpr RegexComplement(this Z3Context context, RegexExpr regex)
    {
        var handle = context.Library.MkReComplement(context.Handle, regex.Handle);
        return Z3Expr.Create<RegexExpr>(context, handle);
    }

    /// <summary>
    /// Creates difference of two regexes.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="left">The regex to subtract from.</param>
    /// <param name="right">The regex to subtract.</param>
    /// <returns>Regular expression matching strings matched by left but not right.</returns>
    public static RegexExpr RegexDiff(this Z3Context context, RegexExpr left, RegexExpr right)
    {
        var handle = context.Library.MkReDiff(context.Handle, left.Handle, right.Handle);
        return Z3Expr.Create<RegexExpr>(context, handle);
    }

    /// <summary>
    /// Creates loop (bounded repetition) of a regex.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="regex">The regex to repeat.</param>
    /// <param name="min">Minimum number of repetitions.</param>
    /// <param name="max">Maximum number of repetitions (0 means unbounded).</param>
    /// <returns>Regular expression matching between min and max repetitions.</returns>
    public static RegexExpr RegexLoop(this Z3Context context, RegexExpr regex, uint min, uint max)
    {
        var handle = context.Library.MkReLoop(context.Handle, regex.Handle, min, max);
        return Z3Expr.Create<RegexExpr>(context, handle);
    }

    /// <summary>
    /// Creates exact repetition of a regex.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="regex">The regex to repeat.</param>
    /// <param name="count">Exact number of repetitions.</param>
    /// <returns>Regular expression matching exactly count repetitions.</returns>
    public static RegexExpr RegexPower(this Z3Context context, RegexExpr regex, uint count)
    {
        var handle = context.Library.MkRePower(context.Handle, regex.Handle, count);
        return Z3Expr.Create<RegexExpr>(context, handle);
    }

    /// <summary>
    /// Creates regex that matches a string against a regular expression.
    /// </summary>
    /// <param name="context">The Z3 context.</param>
    /// <param name="str">The string to match.</param>
    /// <param name="regex">The regular expression pattern.</param>
    /// <returns>Boolean expression that is true if the string matches the pattern.</returns>
    public static BoolExpr InRegex(this Z3Context context, StringExpr str, RegexExpr regex)
    {
        var handle = context.Library.MkSeqInRe(context.Handle, str.Handle, regex.Handle);
        return Z3Expr.Create<BoolExpr>(context, handle);
    }
}
