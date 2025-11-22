using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Strings;

namespace Z3Wrap.Tests.Expressions.Strings;

[TestFixture]
public class RegexExprFactoryTests
{
    [Test]
    public void Regex_LiteralString_CreatesCorrectPattern()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var pattern = context.Regex("hello");
        var str = context.String("hello");
        var matches = str.Matches(pattern);

        solver.Assert(matches);
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(matches), Is.True);
    }

    [Test]
    public void Regex_FromStringExpr_CreatesCorrectPattern()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var str = context.String("test");
        var pattern = context.Regex(str);
        var matches = str.Matches(pattern);

        solver.Assert(matches);
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(matches), Is.True);
    }

    [Test]
    public void RegexConst_CreatesNamedConstant()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var pattern = context.RegexConst("pattern");

        Assert.That(pattern, Is.Not.Null);
        Assert.That(pattern.ToString(), Does.Contain("pattern"));
    }

    [Test]
    public void RegexAllChar_MatchesAnySingleCharacter()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var pattern = context.RegexAllChar();
        var str = context.String("x");
        var matches = str.Matches(pattern);

        solver.Assert(matches);
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(matches), Is.True);
    }

    [Test]
    public void RegexAllChar_DoesNotMatchEmptyString()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var pattern = context.RegexAllChar();
        var str = context.String("");
        var matches = str.Matches(pattern);

        solver.Assert(matches);
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void RegexRange_MatchesCharactersInRange()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var pattern = context.RegexRange('a', 'z');
        var str = context.String("m");
        var matches = str.Matches(pattern);

        solver.Assert(matches);
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(matches), Is.True);
    }

    [Test]
    public void RegexRange_DoesNotMatchOutsideRange()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var pattern = context.RegexRange('a', 'z');
        var str = context.String("5");
        var matches = str.Matches(pattern);

        solver.Assert(matches);
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void RegexRange_WithStringExprs_MatchesCharactersInRange()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var from = context.String("A");
        var to = context.String("Z");
        var pattern = context.RegexRange(from, to);
        var str = context.String("M");
        var matches = str.Matches(pattern);

        solver.Assert(matches);
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(matches), Is.True);
    }

    [Test]
    public void RegexEmpty_MatchesNoStrings()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var pattern = context.RegexEmpty();
        var str = context.StringConst("s");
        var matches = str.Matches(pattern);

        solver.Assert(matches);
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void RegexFull_MatchesAllStrings()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var pattern = context.RegexFull();
        var str = context.String("any string at all");
        var matches = str.Matches(pattern);

        solver.Assert(matches);
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(matches), Is.True);
    }

    [Test]
    public void RegexUnion_MultiplePatterns_MatchesAny()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p1 = context.Regex("cat");
        var p2 = context.Regex("dog");
        var p3 = context.Regex("bird");
        var pattern = context.RegexUnion(p1, p2, p3);

        var str = context.String("dog");
        var matches = str.Matches(pattern);

        solver.Assert(matches);
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(matches), Is.True);
    }

    [Test]
    public void RegexConcat_MultiplePatterns_MatchesConcatenation()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p1 = context.Regex("hello");
        var p2 = context.Regex(" ");
        var p3 = context.Regex("world");
        var pattern = context.RegexConcat(p1, p2, p3);

        var str = context.String("hello world");
        var matches = str.Matches(pattern);

        solver.Assert(matches);
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(matches), Is.True);
    }

    [Test]
    public void RegexIntersect_MultiplePatterns_MatchesAll()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Pattern that matches strings containing 'a' AND strings of length 3
        var containsA = context.RegexAllChar().Star() + context.Regex("a") + context.RegexAllChar().Star();
        var length3 = context.RegexAllChar().Power(3);
        var pattern = context.RegexIntersect(containsA, length3);

        var str = context.String("cat");
        var matches = str.Matches(pattern);

        solver.Assert(matches);
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(matches), Is.True);
    }

    [Test]
    public void InRegex_MatchingString_ReturnsTrue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var str = context.String("test123");
        var pattern = context.Regex("test") + context.RegexRange('0', '9').Plus();
        var matches = context.InRegex(str, pattern);

        solver.Assert(matches);
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(matches), Is.True);
    }
}
