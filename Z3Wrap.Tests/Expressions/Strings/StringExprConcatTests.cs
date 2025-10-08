using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Strings;

namespace Z3Wrap.Tests.Expressions.Strings;

[TestFixture]
public class StringExprConcatTests
{
    [Test]
    public void Concat_TwoValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var str1 = context.String("Hello, ");
        var str2 = context.String("World!");

        // Test all 8 syntax variants
        var concat1 = str1 + str2; // 1. Operator
        var concat2 = "Hello, " + str2; // 2. Literal left
        var concat3 = str1 + "World!"; // 3. Literal right
        var concat4 = context.Concat(str1, str2); // 4. Context extension
        var concat5 = context.Concat("Hello, ", str2); // 5. Context + literal left
        var concat6 = context.Concat(str1, "World!"); // 6. Context + literal right
        var concat7 = str1.Concat(str2); // 7. Expression method
        var concat8 = str1.Concat("World!"); // 8. Expression + literal right

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetStringValue(concat1), Is.EqualTo("Hello, World!"));
            Assert.That(model.GetStringValue(concat2), Is.EqualTo("Hello, World!"));
            Assert.That(model.GetStringValue(concat3), Is.EqualTo("Hello, World!"));
            Assert.That(model.GetStringValue(concat4), Is.EqualTo("Hello, World!"));
            Assert.That(model.GetStringValue(concat5), Is.EqualTo("Hello, World!"));
            Assert.That(model.GetStringValue(concat6), Is.EqualTo("Hello, World!"));
            Assert.That(model.GetStringValue(concat7), Is.EqualTo("Hello, World!"));
            Assert.That(model.GetStringValue(concat8), Is.EqualTo("Hello, World!"));
        });
    }

    [Test]
    public void Concat_ThreeValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var str1 = context.String("Hello");
        var str2 = context.String(", ");
        var str3 = context.String("World!");
        var concatViaContext = context.Concat(str1, str2, str3);
        var concatViaExpr = str1.Concat(str2, str3);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetStringValue(concatViaContext), Is.EqualTo("Hello, World!"));
            Assert.That(model.GetStringValue(concatViaExpr), Is.EqualTo("Hello, World!"));
        });
    }

    [Test]
    public void Concat_FourValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var str1 = context.String("A");
        var str2 = context.String("B");
        var str3 = context.String("C");
        var str4 = context.String("D");
        var concatViaContext = context.Concat(str1, str2, str3, str4);
        var concatViaExpr = str1.Concat(str2, str3, str4);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetStringValue(concatViaContext), Is.EqualTo("ABCD"));
            Assert.That(model.GetStringValue(concatViaExpr), Is.EqualTo("ABCD"));
        });
    }

    [Test]
    public void Concat_EmptyStrings_ReturnsEmptyString()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var empty1 = context.String("");
        var empty2 = context.String("");
        var result = empty1 + empty2;

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetStringValue(result), Is.EqualTo(""));
    }

    [Test]
    public void Concat_EmptyWithNonEmpty_ReturnsNonEmpty()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var empty = context.String("");
        var nonEmpty = context.String("test");
        var result1 = empty + nonEmpty;
        var result2 = nonEmpty + empty;

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetStringValue(result1), Is.EqualTo("test"));
            Assert.That(model.GetStringValue(result2), Is.EqualTo("test"));
        });
    }

    [Test]
    public void Concat_WithVariable_SolvesCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var prefix = context.String("Hello, ");
        var suffix = context.StringConst("suffix");
        var result = prefix + suffix;

        solver.Assert(result == "Hello, World!");

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetStringValue(suffix), Is.EqualTo("World!"));
    }

    [Test]
    public void Concat_ChainedOperators_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var str1 = context.String("A");
        var str2 = context.String("B");
        var str3 = context.String("C");
        var result = str1 + str2 + str3;

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetStringValue(result), Is.EqualTo("ABC"));
    }
}
