namespace Spaceorc.Z3Wrap.Expressions.Strings;

/// <summary>
/// Provides regex-specific operations for regular expression expressions.
/// </summary>
public static class RegexExprExtensions
{
    /// <summary>
    /// Creates Kleene star (zero or more repetitions) of this regex.
    /// </summary>
    /// <param name="regex">The regex to repeat.</param>
    /// <returns>Regular expression matching zero or more repetitions.</returns>
    public static RegexExpr Star(this RegexExpr regex) => regex.Context.RegexStar(regex);

    /// <summary>
    /// Creates Kleene plus (one or more repetitions) of this regex.
    /// </summary>
    /// <param name="regex">The regex to repeat.</param>
    /// <returns>Regular expression matching one or more repetitions.</returns>
    public static RegexExpr Plus(this RegexExpr regex) => regex.Context.RegexPlus(regex);

    /// <summary>
    /// Creates optional match (zero or one occurrence) of this regex.
    /// </summary>
    /// <param name="regex">The regex to make optional.</param>
    /// <returns>Regular expression matching zero or one occurrence.</returns>
    public static RegexExpr Option(this RegexExpr regex) => regex.Context.RegexOption(regex);

    /// <summary>
    /// Creates complement of this regex.
    /// </summary>
    /// <param name="regex">The regex to complement.</param>
    /// <returns>Regular expression matching all strings not matched by this regex.</returns>
    public static RegexExpr Complement(this RegexExpr regex) => regex.Context.RegexComplement(regex);

    /// <summary>
    /// Creates union (alternation) with another regex.
    /// </summary>
    /// <param name="left">The left regex pattern.</param>
    /// <param name="others">Additional regex patterns to unite.</param>
    /// <returns>Regular expression matching any of the patterns.</returns>
    public static RegexExpr Union(this RegexExpr left, params IEnumerable<RegexExpr> others) =>
        left.Context.RegexUnion([left, .. others]);

    /// <summary>
    /// Creates concatenation with another regex.
    /// </summary>
    /// <param name="left">The left regex.</param>
    /// <param name="others">Additional regexes to concatenate.</param>
    /// <returns>Regular expression matching concatenated patterns.</returns>
    public static RegexExpr Concat(this RegexExpr left, params IEnumerable<RegexExpr> others) =>
        left.Context.RegexConcat([left, .. others]);

    /// <summary>
    /// Creates intersection with another regex.
    /// </summary>
    /// <param name="left">The left regex.</param>
    /// <param name="others">Additional regexes to intersect.</param>
    /// <returns>Regular expression matching all patterns.</returns>
    public static RegexExpr Intersect(this RegexExpr left, params IEnumerable<RegexExpr> others) =>
        left.Context.RegexIntersect([left, .. others]);

    /// <summary>
    /// Creates difference with another regex.
    /// </summary>
    /// <param name="left">The regex to subtract from.</param>
    /// <param name="right">The regex to subtract.</param>
    /// <returns>Regular expression matching strings matched by left but not right.</returns>
    public static RegexExpr Diff(this RegexExpr left, RegexExpr right) => left.Context.RegexDiff(left, right);

    /// <summary>
    /// Creates loop (bounded repetition) of this regex.
    /// </summary>
    /// <param name="regex">The regex to repeat.</param>
    /// <param name="min">Minimum number of repetitions.</param>
    /// <param name="max">Maximum number of repetitions (0 means unbounded).</param>
    /// <returns>Regular expression matching between min and max repetitions.</returns>
    public static RegexExpr Loop(this RegexExpr regex, uint min, uint max) => regex.Context.RegexLoop(regex, min, max);

    /// <summary>
    /// Creates exact repetition of this regex.
    /// </summary>
    /// <param name="regex">The regex to repeat.</param>
    /// <param name="count">Exact number of repetitions.</param>
    /// <returns>Regular expression matching exactly count repetitions.</returns>
    public static RegexExpr Power(this RegexExpr regex, uint count) => regex.Context.RegexPower(regex, count);
}
