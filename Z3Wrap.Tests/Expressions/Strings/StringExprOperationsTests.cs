using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Numerics;
using Spaceorc.Z3Wrap.Expressions.Strings;

namespace Z3Wrap.Tests.Expressions.Strings;

[TestFixture]
public class StringExprOperationsTests
{
    [Test]
    public void Length_ReturnsCorrectValue()
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
    public void Length_EmptyString_ReturnsZero()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var str = context.String("");
        var length = str.Length();

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetIntValue(length), Is.EqualTo(new BigInteger(0)));
    }

    [Test]
    public void Contains_SubstringPresent_ReturnsTrue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var str = context.String("hello world");
        var substring = context.String("world");
        var result = str.Contains(substring);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(result), Is.True);
    }

    [Test]
    public void Contains_SubstringAbsent_ReturnsFalse()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var str = context.String("hello world");
        var substring = context.String("xyz");
        var result = str.Contains(substring);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(result), Is.False);
    }

    [Test]
    public void StartsWith_ValidPrefix_ReturnsTrue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var str = context.String("hello world");
        var prefix = context.String("hello");
        var result = str.StartsWith(prefix);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(result), Is.True);
    }

    [Test]
    public void StartsWith_InvalidPrefix_ReturnsFalse()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var str = context.String("hello world");
        var prefix = context.String("world");
        var result = str.StartsWith(prefix);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(result), Is.False);
    }

    [Test]
    public void EndsWith_ValidSuffix_ReturnsTrue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var str = context.String("hello world");
        var suffix = context.String("world");
        var result = str.EndsWith(suffix);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(result), Is.True);
    }

    [Test]
    public void EndsWith_InvalidSuffix_ReturnsFalse()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var str = context.String("hello world");
        var suffix = context.String("hello");
        var result = str.EndsWith(suffix);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(result), Is.False);
    }

    [Test]
    public void Substring_ExtractsCorrectPortion()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var str = context.String("hello world");
        var offset = context.Int(6);
        var length = context.Int(5);
        var result = str.Substring(offset, length);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetStringValue(result), Is.EqualTo("world"));
    }

    [Test]
    public void Substring_FromStart_ExtractsCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var str = context.String("hello");
        var offset = context.Int(0);
        var length = context.Int(3);
        var result = str.Substring(offset, length);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetStringValue(result), Is.EqualTo("hel"));
    }

    [Test]
    public void Replace_FirstOccurrence_ReplacesCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var str = context.String("hello world");
        var source = context.String("world");
        var destination = context.String("there");
        var result = str.Replace(source, destination);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetStringValue(result), Is.EqualTo("hello there"));
    }

    [Test]
    public void Replace_NoMatch_ReturnsOriginal()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var str = context.String("hello world");
        var source = context.String("xyz");
        var destination = context.String("abc");
        var result = str.Replace(source, destination);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetStringValue(result), Is.EqualTo("hello world"));
    }

    [Test]
    public void IndexOf_SubstringPresent_ReturnsCorrectIndex()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var str = context.String("hello world");
        var substring = context.String("world");
        var offset = context.Int(0);
        var result = str.IndexOf(substring, offset);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetIntValue(result), Is.EqualTo(new BigInteger(6)));
    }

    [Test]
    public void IndexOf_SubstringAbsent_ReturnsMinusOne()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var str = context.String("hello world");
        var substring = context.String("xyz");
        var offset = context.Int(0);
        var result = str.IndexOf(substring, offset);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetIntValue(result), Is.EqualTo(new BigInteger(-1)));
    }

    [Test]
    public void IndexOf_WithOffset_FindsCorrectOccurrence()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var str = context.String("hello hello");
        var substring = context.String("hello");
        var offset = context.Int(1);
        var result = str.IndexOf(substring, offset);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetIntValue(result), Is.EqualTo(new BigInteger(6)));
    }

    [Test]
    public void LastIndexOf_SubstringPresent_ReturnsLastIndex()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var str = context.String("hello hello");
        var substring = context.String("hello");
        var result = str.LastIndexOf(substring);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetIntValue(result), Is.EqualTo(new BigInteger(6)));
    }

    [Test]
    public void LastIndexOf_SubstringAbsent_ReturnsMinusOne()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var str = context.String("hello world");
        var substring = context.String("xyz");
        var result = str.LastIndexOf(substring);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetIntValue(result), Is.EqualTo(new BigInteger(-1)));
    }

    [Test]
    public void CharAt_ReturnsCorrectCharacter()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var str = context.String("hello");
        var index = context.Int(1);
        var ch = str.CharAt(index);

        solver.Assert(ch == 'e');

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Indexer_ReturnsCorrectCharacter()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var str = context.String("world");
        var index = context.Int(0);
        var ch = str[index];

        solver.Assert(ch == 'w');

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void At_ReturnsUnitString()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var str = context.String("hello");
        var index = context.Int(1);
        var unitStr = str.At(index);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetStringValue(unitStr), Is.EqualTo("e"));
    }
}
