using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.BitVectors;
using Spaceorc.Z3Wrap.Expressions.Strings;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Z3Wrap.Tests.Expressions.Strings;

[TestFixture]
public class CharExprOperationsTests
{
    [TestCase('A', 65)]
    [TestCase('0', 48)]
    [TestCase('z', 122)]
    public void ToInt_ReturnsCorrectCodepoint(char ch, int expectedCodepoint)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var charExpr = context.Char(ch);
        var intResult = charExpr.ToInt();

        // Assert that the integer equals the expected codepoint
        solver.Assert(intResult == expectedCodepoint);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ToBv_ReturnsCorrect18BitBitvector()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var charExpr = context.Char('A');
        var bvResult = charExpr.ToBv();

        // Assert the bitvector equals 65
        solver.Assert(bvResult == context.Bv<Size18>(65));

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ToBv_Generic_WithSize18_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var charExpr = context.Char('B');
        var bvResult = charExpr.ToBv<Size18>();

        // Assert the bitvector equals 66
        solver.Assert(bvResult == context.Bv<Size18>(66));

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ToBv_Generic_WithWrongSize_ThrowsException()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var charExpr = context.Char('A');

        Assert.Throws<ArgumentException>(() => charExpr.ToBv<Size16>());
    }

    [Test]
    public void IsDigit_WithDigitCharacter_ReturnsTrue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var charExpr = context.Char('5');
        var result = charExpr.IsDigit();

        // Assert that it is a digit
        solver.Assert(result);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [TestCase('0')]
    [TestCase('5')]
    [TestCase('9')]
    public void IsDigit_WithDigits_ReturnsTrue(char digit)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var charExpr = context.Char(digit);
        var result = charExpr.IsDigit();

        // Assert that it is a digit
        solver.Assert(result);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [TestCase('A')]
    [TestCase('z')]
    [TestCase(' ')]
    [TestCase('!')]
    public void IsDigit_WithNonDigits_ReturnsFalse(char nonDigit)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var charExpr = context.Char(nonDigit);
        var result = charExpr.IsDigit();

        // Assert that it is NOT a digit
        solver.Assert(!result);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Le_TwoCharacters_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var char1 = context.Char('A');
        var char2 = context.Char('B');
        var result = char1 <= char2;

        // Assert the comparison is true
        solver.Assert(result);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Le_EqualCharacters_ReturnsTrue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var char1 = context.Char('X');
        var char2 = context.Char('X');
        var result = char1 <= char2;

        // Assert the comparison is true
        solver.Assert(result);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Ge_TwoCharacters_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var char1 = context.Char('Z');
        var char2 = context.Char('A');
        var result = char1 >= char2;

        // Assert the comparison is true
        solver.Assert(result);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Lt_TwoCharacters_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var char1 = context.Char('A');
        var char2 = context.Char('B');
        var result = char1 < char2;

        // Assert the comparison is true
        solver.Assert(result);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Lt_EqualCharacters_ReturnsFalse()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var char1 = context.Char('M');
        var char2 = context.Char('M');
        var result = char1 < char2;

        // Assert the comparison is false
        solver.Assert(!result);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Gt_TwoCharacters_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var char1 = context.Char('Z');
        var char2 = context.Char('A');
        var result = char1 > char2;

        // Assert the comparison is true
        solver.Assert(result);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Eq_EqualCharacters_ReturnsTrue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var char1 = context.Char('T');
        var char2 = context.Char('T');
        var result = char1 == char2;

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(result), Is.True);
    }

    [Test]
    public void Eq_DifferentCharacters_ReturnsFalse()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var char1 = context.Char('X');
        var char2 = context.Char('Y');
        var result = char1 == char2;

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(result), Is.False);
    }

    [Test]
    public void Ne_DifferentCharacters_ReturnsTrue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var char1 = context.Char('A');
        var char2 = context.Char('B');
        var result = char1 != char2;

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(result), Is.True);
    }

    [Test]
    public void Ne_EqualCharacters_ReturnsFalse()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var char1 = context.Char('K');
        var char2 = context.Char('K');
        var result = char1 != char2;

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(result), Is.False);
    }

    [Test]
    public void CharComparison_WithImplicitConversion_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var charExpr = context.Char('M');
        var result1 = charExpr <= 'Z';
        var result2 = 'A' <= charExpr;

        // Assert both comparisons are true
        solver.Assert(result1);
        solver.Assert(result2);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }
}
