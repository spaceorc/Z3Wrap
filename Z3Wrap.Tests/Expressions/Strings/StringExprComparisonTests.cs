using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Strings;

namespace Z3Wrap.Tests.Expressions.Strings;

[TestFixture]
public class StringExprComparisonTests
{
    [Test]
    public void Lt_TwoValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var str1 = context.String("aaa");
        var str2 = context.String("bbb");

        // Test all 8 syntax variants
        var cmp1 = str1 < str2; // 1. Operator
        var cmp2 = "aaa" < str2; // 2. Literal left
        var cmp3 = str1 < "bbb"; // 3. Literal right
        var cmp4 = context.Lt(str1, str2); // 4. Context extension
        var cmp5 = context.Lt("aaa", str2); // 5. Context + literal left
        var cmp6 = context.Lt(str1, "bbb"); // 6. Context + literal right
        var cmp7 = str1.Lt(str2); // 7. Expression method
        var cmp8 = str1.Lt("bbb"); // 8. Expression + literal right

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(cmp1), Is.True);
            Assert.That(model.GetBoolValue(cmp2), Is.True);
            Assert.That(model.GetBoolValue(cmp3), Is.True);
            Assert.That(model.GetBoolValue(cmp4), Is.True);
            Assert.That(model.GetBoolValue(cmp5), Is.True);
            Assert.That(model.GetBoolValue(cmp6), Is.True);
            Assert.That(model.GetBoolValue(cmp7), Is.True);
            Assert.That(model.GetBoolValue(cmp8), Is.True);
        });
    }

    [Test]
    public void Le_TwoValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var str1 = context.String("aaa");
        var str2 = context.String("bbb");

        // Test all 8 syntax variants
        var cmp1 = str1 <= str2; // 1. Operator
        var cmp2 = "aaa" <= str2; // 2. Literal left
        var cmp3 = str1 <= "bbb"; // 3. Literal right
        var cmp4 = context.Le(str1, str2); // 4. Context extension
        var cmp5 = context.Le("aaa", str2); // 5. Context + literal left
        var cmp6 = context.Le(str1, "bbb"); // 6. Context + literal right
        var cmp7 = str1.Le(str2); // 7. Expression method
        var cmp8 = str1.Le("bbb"); // 8. Expression + literal right

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(cmp1), Is.True);
            Assert.That(model.GetBoolValue(cmp2), Is.True);
            Assert.That(model.GetBoolValue(cmp3), Is.True);
            Assert.That(model.GetBoolValue(cmp4), Is.True);
            Assert.That(model.GetBoolValue(cmp5), Is.True);
            Assert.That(model.GetBoolValue(cmp6), Is.True);
            Assert.That(model.GetBoolValue(cmp7), Is.True);
            Assert.That(model.GetBoolValue(cmp8), Is.True);
        });
    }

    [Test]
    public void Le_EqualValues_ReturnsTrue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var str1 = context.String("test");
        var str2 = context.String("test");
        var result = str1 <= str2;

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(result), Is.True);
    }

    [Test]
    public void Gt_TwoValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var str1 = context.String("bbb");
        var str2 = context.String("aaa");

        // Test all 8 syntax variants
        var cmp1 = str1 > str2; // 1. Operator
        var cmp2 = "bbb" > str2; // 2. Literal left
        var cmp3 = str1 > "aaa"; // 3. Literal right
        var cmp4 = context.Gt(str1, str2); // 4. Context extension
        var cmp5 = context.Gt("bbb", str2); // 5. Context + literal left
        var cmp6 = context.Gt(str1, "aaa"); // 6. Context + literal right
        var cmp7 = str1.Gt(str2); // 7. Expression method
        var cmp8 = str1.Gt("aaa"); // 8. Expression + literal right

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(cmp1), Is.True);
            Assert.That(model.GetBoolValue(cmp2), Is.True);
            Assert.That(model.GetBoolValue(cmp3), Is.True);
            Assert.That(model.GetBoolValue(cmp4), Is.True);
            Assert.That(model.GetBoolValue(cmp5), Is.True);
            Assert.That(model.GetBoolValue(cmp6), Is.True);
            Assert.That(model.GetBoolValue(cmp7), Is.True);
            Assert.That(model.GetBoolValue(cmp8), Is.True);
        });
    }

    [Test]
    public void Ge_TwoValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var str1 = context.String("bbb");
        var str2 = context.String("aaa");

        // Test all 8 syntax variants
        var cmp1 = str1 >= str2; // 1. Operator
        var cmp2 = "bbb" >= str2; // 2. Literal left
        var cmp3 = str1 >= "aaa"; // 3. Literal right
        var cmp4 = context.Ge(str1, str2); // 4. Context extension
        var cmp5 = context.Ge("bbb", str2); // 5. Context + literal left
        var cmp6 = context.Ge(str1, "aaa"); // 6. Context + literal right
        var cmp7 = str1.Ge(str2); // 7. Expression method
        var cmp8 = str1.Ge("aaa"); // 8. Expression + literal right

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(cmp1), Is.True);
            Assert.That(model.GetBoolValue(cmp2), Is.True);
            Assert.That(model.GetBoolValue(cmp3), Is.True);
            Assert.That(model.GetBoolValue(cmp4), Is.True);
            Assert.That(model.GetBoolValue(cmp5), Is.True);
            Assert.That(model.GetBoolValue(cmp6), Is.True);
            Assert.That(model.GetBoolValue(cmp7), Is.True);
            Assert.That(model.GetBoolValue(cmp8), Is.True);
        });
    }

    [Test]
    public void Ge_EqualValues_ReturnsTrue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var str1 = context.String("test");
        var str2 = context.String("test");
        var result = str1 >= str2;

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(result), Is.True);
    }

    [Test]
    public void Eq_EqualValues_ReturnsTrue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var str1 = context.String("test");
        var str2 = context.String("test");
        var result = str1 == str2;

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(result), Is.True);
    }

    [Test]
    public void Eq_DifferentValues_ReturnsFalse()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var str1 = context.String("hello");
        var str2 = context.String("world");
        var result = str1 == str2;

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(result), Is.False);
    }

    [Test]
    public void Ne_DifferentValues_ReturnsTrue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var str1 = context.String("hello");
        var str2 = context.String("world");
        var result = str1 != str2;

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(result), Is.True);
    }

    [Test]
    public void Ne_EqualValues_ReturnsFalse()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var str1 = context.String("test");
        var str2 = context.String("test");
        var result = str1 != str2;

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(result), Is.False);
    }

    [Test]
    public void Comparison_LexicographicOrdering_WorksCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.String("apple");
        var b = context.String("banana");
        var c = context.String("cherry");

        var result1 = a < b;
        var result2 = b < c;
        var result3 = a < c;

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(result1), Is.True);
            Assert.That(model.GetBoolValue(result2), Is.True);
            Assert.That(model.GetBoolValue(result3), Is.True);
        });
    }

    [Test]
    public void Comparison_EmptyString_IsLessThanNonEmpty()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var empty = context.String("");
        var nonEmpty = context.String("a");
        var result = empty < nonEmpty;

        // Assert the comparison is true
        solver.Assert(result);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Comparison_PrefixString_IsLessThanExtendedString()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var prefix = context.String("test");
        var extended = context.String("testing");
        var result = prefix < extended;

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(result), Is.True);
    }
}
