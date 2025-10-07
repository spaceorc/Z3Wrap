using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Numerics;
using Spaceorc.Z3Wrap.Expressions.Strings;

namespace Z3Wrap.Tests.Expressions.Strings;

[TestFixture]
public class StringExprSmokeTests
{
    [Test]
    public void StringCreation_Literal_CreatesCorrectValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var str = context.String("hello");
        var result = context.StringConst("result");

        solver.Assert(result == str);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetStringValue(result), Is.EqualTo("hello"));
    }

    [Test]
    public void StringConcatenation_TwoStrings_CombinesCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var hello = context.String("Hello, ");
        var world = context.String("World!");
        var result = hello + world;

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetStringValue(result), Is.EqualTo("Hello, World!"));
    }

    [Test]
    public void StringLength_KnownString_ReturnsCorrectLength()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var str = context.String("hello");
        var length = str.Length();

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetIntValue(length), Is.EqualTo(new BigInteger(5)));
    }

    [Test]
    public void StringContains_Substring_FindsMatch()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var str = context.String("hello world");
        var contains = str.Contains(context.String("world"));

        solver.Assert(contains);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(contains), Is.True);
    }

    [Test]
    public void CharCreation_Literal_CreatesCorrectValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var ch = context.Char('A');
        var result = context.CharConst("result");

        solver.Assert(result == ch);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetCharValue(result), Is.EqualTo((uint)'A'));
    }

    [Test]
    public void CharToInt_LetterA_ReturnsCodepoint65()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var ch = context.Char('A');
        var codepoint = ch.ToInt();

        // Assert that the codepoint equals 65
        solver.Assert(codepoint == context.Int(65));

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        // Verify by checking the character value
        var model = solver.GetModel();
        Assert.That(model.GetCharValue(ch), Is.EqualTo((uint)65));
    }

    [Test]
    public void StringIndexer_FirstChar_ReturnsCorrectCharacter()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var str = context.String("Hello");
        var firstChar = str[context.Int(0)];

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetCharValue(firstChar), Is.EqualTo((uint)'H'));
    }

    [Test]
    public void IntToStr_Number42_ConvertsToString()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var num = context.Int(42);
        var str = num.ToStr();

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetStringValue(str), Is.EqualTo("42"));
    }
}
