using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Strings;

namespace Z3Wrap.Tests.Expressions.Strings;

[TestFixture]
public class RegexExprOperationsTests
{
    [Test]
    public void Star_MatchesZeroOrMoreRepetitions()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var regex = context.Regex("ab");
        var patternViaExtension = regex.Star();
        var patternViaContext = context.RegexStar(regex);

        // Test empty string (zero repetitions)
        var str1 = context.String("");
        solver.Assert(str1.Matches(patternViaExtension));
        solver.Assert(str1.Matches(patternViaContext));

        // Test one repetition
        var str2 = context.String("ab");
        solver.Assert(str2.Matches(patternViaExtension));
        solver.Assert(str2.Matches(patternViaContext));

        // Test multiple repetitions
        var str3 = context.String("ababab");
        solver.Assert(str3.Matches(patternViaExtension));
        solver.Assert(str3.Matches(patternViaContext));

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Plus_MatchesOneOrMoreRepetitions()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var regex = context.Regex("x");
        var patternViaExtension = regex.Plus();
        var patternViaContext = context.RegexPlus(regex);

        // Empty string should not match
        var str1 = context.String("");
        solver.Assert(!str1.Matches(patternViaExtension));
        solver.Assert(!str1.Matches(patternViaContext));

        // One or more should match
        var str2 = context.String("xxx");
        solver.Assert(str2.Matches(patternViaExtension));
        solver.Assert(str2.Matches(patternViaContext));

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Option_MatchesZeroOrOneOccurrence()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var regex = context.Regex("a");
        var patternViaExtension = regex.Option();
        var patternViaContext = context.RegexOption(regex);

        // Empty string should match
        var str1 = context.String("");
        solver.Assert(str1.Matches(patternViaExtension));
        solver.Assert(str1.Matches(patternViaContext));

        // Single occurrence should match
        var str2 = context.String("a");
        solver.Assert(str2.Matches(patternViaExtension));
        solver.Assert(str2.Matches(patternViaContext));

        // Multiple occurrences should not match
        var str3 = context.String("aa");
        solver.Assert(!str3.Matches(patternViaExtension));
        solver.Assert(!str3.Matches(patternViaContext));

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Complement_MatchesOppositeOfPattern()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var regex = context.Regex("exact");
        var patternViaExtension = regex.Complement();
        var patternViaContext = context.RegexComplement(regex);
        var patternViaOperator = ~regex;

        // "exact" should not match
        var str1 = context.String("exact");
        solver.Assert(!str1.Matches(patternViaExtension));
        solver.Assert(!str1.Matches(patternViaContext));
        solver.Assert(!str1.Matches(patternViaOperator));

        // Anything else should match
        var str2 = context.String("other");
        solver.Assert(str2.Matches(patternViaExtension));
        solver.Assert(str2.Matches(patternViaContext));
        solver.Assert(str2.Matches(patternViaOperator));

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Union_TwoPatterns_MatchesEither()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p1 = context.Regex("cat");
        var p2 = context.Regex("dog");
        var patternViaExtension = p1.Union(p2);
        var patternViaContext = context.RegexUnion(p1, p2);
        var patternViaOperator = p1 | p2;

        var str1 = context.String("cat");
        solver.Assert(str1.Matches(patternViaExtension));
        solver.Assert(str1.Matches(patternViaContext));
        solver.Assert(str1.Matches(patternViaOperator));

        var str2 = context.String("dog");
        solver.Assert(str2.Matches(patternViaExtension));
        solver.Assert(str2.Matches(patternViaContext));
        solver.Assert(str2.Matches(patternViaOperator));

        var str3 = context.String("bird");
        solver.Assert(!str3.Matches(patternViaExtension));
        solver.Assert(!str3.Matches(patternViaContext));
        solver.Assert(!str3.Matches(patternViaOperator));

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Union_ThreePatterns_MatchesAny()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p1 = context.Regex("cat");
        var p2 = context.Regex("dog");
        var p3 = context.Regex("bird");
        var patternViaExtension = p1.Union(p2, p3);
        var patternViaContext = context.RegexUnion(p1, p2, p3);

        var str = context.String("bird");
        solver.Assert(str.Matches(patternViaExtension));
        solver.Assert(str.Matches(patternViaContext));

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Concat_TwoPatterns_MatchesConcatenation()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p1 = context.Regex("hello");
        var p2 = context.Regex("world");
        var patternViaExtension = p1.Concat(p2);
        var patternViaContext = context.RegexConcat(p1, p2);
        var patternViaOperator = p1 + p2;

        var str1 = context.String("helloworld");
        solver.Assert(str1.Matches(patternViaExtension));
        solver.Assert(str1.Matches(patternViaContext));
        solver.Assert(str1.Matches(patternViaOperator));

        var str2 = context.String("hello world");
        solver.Assert(!str2.Matches(patternViaExtension));
        solver.Assert(!str2.Matches(patternViaContext));
        solver.Assert(!str2.Matches(patternViaOperator));

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Concat_ThreePatterns_MatchesConcatenation()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p1 = context.Regex("hello");
        var p2 = context.Regex(" ");
        var p3 = context.Regex("world");
        var patternViaExtension = p1.Concat(p2, p3);
        var patternViaContext = context.RegexConcat(p1, p2, p3);

        var str = context.String("hello world");
        solver.Assert(str.Matches(patternViaExtension));
        solver.Assert(str.Matches(patternViaContext));

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Intersect_TwoPatterns_MatchesBoth()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Pattern matching strings with 'a' and exactly 3 characters
        var hasA = context.RegexAllChar().Star() + context.Regex("a") + context.RegexAllChar().Star();
        var length3 = context.RegexAllChar().Power(3);
        var patternViaExtension = hasA.Intersect(length3);
        var patternViaContext = context.RegexIntersect(hasA, length3);
        var patternViaOperator = hasA & length3;

        var str1 = context.String("cat");
        solver.Assert(str1.Matches(patternViaExtension));
        solver.Assert(str1.Matches(patternViaContext));
        solver.Assert(str1.Matches(patternViaOperator));

        var str2 = context.String("dog"); // no 'a'
        solver.Assert(!str2.Matches(patternViaExtension));
        solver.Assert(!str2.Matches(patternViaContext));
        solver.Assert(!str2.Matches(patternViaOperator));

        var str3 = context.String("tall"); // has 'a' but length 4
        solver.Assert(!str3.Matches(patternViaExtension));
        solver.Assert(!str3.Matches(patternViaContext));
        solver.Assert(!str3.Matches(patternViaOperator));

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Diff_MatchesFirstButNotSecond()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var anyTwoChars = context.RegexAllChar().Power(2);
        var ab = context.Regex("ab");
        var patternViaExtension = anyTwoChars.Diff(ab);
        var patternViaContext = context.RegexDiff(anyTwoChars, ab);
        var patternViaOperator = anyTwoChars - ab;

        var str1 = context.String("xy");
        solver.Assert(str1.Matches(patternViaExtension));
        solver.Assert(str1.Matches(patternViaContext));
        solver.Assert(str1.Matches(patternViaOperator));

        var str2 = context.String("ab");
        solver.Assert(!str2.Matches(patternViaExtension));
        solver.Assert(!str2.Matches(patternViaContext));
        solver.Assert(!str2.Matches(patternViaOperator));

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Loop_BoundedRepetitions_MatchesCorrectRange()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var regex = context.Regex("a");
        var patternViaExtension = regex.Loop(2, 4);
        var patternViaContext = context.RegexLoop(regex, 2, 4);

        var str1 = context.String("a"); // too few
        solver.Assert(!str1.Matches(patternViaExtension));
        solver.Assert(!str1.Matches(patternViaContext));

        var str2 = context.String("aaa"); // just right
        solver.Assert(str2.Matches(patternViaExtension));
        solver.Assert(str2.Matches(patternViaContext));

        var str3 = context.String("aaaaa"); // too many
        solver.Assert(!str3.Matches(patternViaExtension));
        solver.Assert(!str3.Matches(patternViaContext));

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Loop_UnboundedMax_MatchesAtLeastMin()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var regex = context.Regex("x");
        var patternViaExtension = regex.Loop(3, 0); // at least 3
        var patternViaContext = context.RegexLoop(regex, 3, 0);

        var str1 = context.String("xx"); // too few
        solver.Assert(!str1.Matches(patternViaExtension));
        solver.Assert(!str1.Matches(patternViaContext));

        var str2 = context.String("xxx"); // minimum
        solver.Assert(str2.Matches(patternViaExtension));
        solver.Assert(str2.Matches(patternViaContext));

        var str3 = context.String("xxxxxx"); // more is ok
        solver.Assert(str3.Matches(patternViaExtension));
        solver.Assert(str3.Matches(patternViaContext));

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Power_ExactRepetitions_MatchesExactCount()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var regex = context.Regex("ab");
        var patternViaExtension = regex.Power(3);
        var patternViaContext = context.RegexPower(regex, 3);

        var str1 = context.String("ababab");
        solver.Assert(str1.Matches(patternViaExtension));
        solver.Assert(str1.Matches(patternViaContext));

        var str2 = context.String("abab"); // too few
        solver.Assert(!str2.Matches(patternViaExtension));
        solver.Assert(!str2.Matches(patternViaContext));

        var str3 = context.String("abababab"); // too many
        solver.Assert(!str3.Matches(patternViaExtension));
        solver.Assert(!str3.Matches(patternViaContext));

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ComplexPattern_EmailLike_MatchesCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Simple email-like pattern: letters+ @ letters+ . letters{2,3}
        var letter = context.RegexRange('a', 'z');
        var localPart = letter.Plus();
        var at = context.Regex("@");
        var domain = letter.Plus();
        var dot = context.Regex(".");
        var tld = letter.Loop(2, 3);

        var pattern = localPart + at + domain + dot + tld;

        var str = context.String("user@example.com");
        var matches = str.Matches(pattern);

        solver.Assert(matches);
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(matches), Is.True);
    }

    [Test]
    public void ComplexPattern_PhoneNumber_FindsValidFormat()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Pattern: 3 digits - 3 digits - 4 digits
        var digit = context.RegexRange('0', '9');
        var dash = context.Regex("-");
        var pattern = digit.Power(3) + dash + digit.Power(3) + dash + digit.Power(4);

        var phone = context.StringConst("phone");
        solver.Assert(phone.Matches(pattern));

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var phoneValue = model.GetStringValue(phone);

        // Verify it matches the pattern (e.g., "123-456-7890")
        Assert.That(phoneValue, Does.Match(@"^\d{3}-\d{3}-\d{4}$"));
    }
}
