using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Functions;
using Spaceorc.Z3Wrap.Expressions.Numerics;
using Spaceorc.Z3Wrap.Expressions.Quantifiers;
using Spaceorc.Z3Wrap.Values.Numerics;

namespace Z3Wrap.Tests.Expressions.Quantifiers;

[TestFixture]
public class QuantifiersTests
{
    [Test]
    public void ForAll_SingleVariable_DefinesUninterpretedFunction()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();
        solver.SetTimeout(TimeSpan.FromSeconds(30));

        var func = context.Func<IntExpr, IntExpr>("sqr");

        var x = context.IntConst("x");
        solver.Assert(context.ForAll(x, func.Apply(x) == x * x));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetIntValue(func.Apply(12)), Is.EqualTo(new BigInteger(144)));
    }

    [Test]
    public void ForAll_TwoVariables_DefinesCommutativeProperty()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();
        solver.SetTimeout(TimeSpan.FromSeconds(30));

        var func = context.Func<IntExpr, IntExpr, IntExpr>("add");

        var x = context.IntConst("x");
        var y = context.IntConst("y");
        solver.Assert(context.ForAll(x, y, func.Apply(x, y) == func.Apply(y, x)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var result1 = model.GetIntValue(func.Apply(3, 5));
        var result2 = model.GetIntValue(func.Apply(5, 3));
        Assert.That(result1, Is.EqualTo(result2));
    }

    [Test]
    public void ForAll_ThreeVariables_DefinesAssociativeProperty()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();
        solver.SetTimeout(TimeSpan.FromSeconds(30));

        var func = context.Func<IntExpr, IntExpr, IntExpr>("mul");

        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var z = context.IntConst("z");
        solver.Assert(context.ForAll(x, y, z, func.Apply(func.Apply(x, y), z) == func.Apply(x, func.Apply(y, z))));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var left = model.GetIntValue(func.Apply(func.Apply(2, 3), 4));
        var right = model.GetIntValue(func.Apply(2, func.Apply(3, 4)));
        Assert.That(left, Is.EqualTo(right));
    }

    [Test]
    public void ForAll_FourVariables_DefinesRelationBetweenVariables()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();
        solver.SetTimeout(TimeSpan.FromSeconds(30));

        var func = context.Func<IntExpr, IntExpr, IntExpr, IntExpr>("f");

        var a = context.IntConst("a");
        var b = context.IntConst("b");
        var c = context.IntConst("c");
        var d = context.IntConst("d");
        solver.Assert(context.ForAll(a, b, c, d, func.Apply(a, b, c) == func.Apply(b, a, c)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var result1 = model.GetIntValue(func.Apply(1, 2, 3));
        var result2 = model.GetIntValue(func.Apply(2, 1, 3));
        Assert.That(result1, Is.EqualTo(result2));
    }

    [TestCase(16, Z3Status.Satisfiable)] // 4*4
    [TestCase(20, Z3Status.Unsatisfiable)] // ?*?
    public void Exists_SingleVariable_FindsPerfectSquare(int square, Z3Status status)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();
        solver.SetTimeout(TimeSpan.FromSeconds(30));

        var x = context.IntConst("x");
        solver.Assert(context.Exists(x, x * x == square));

        Assert.That(solver.Check(), Is.EqualTo(status));
    }

    [TestCase(25, Z3Status.Satisfiable)] // 3*3 + 4*4 = 25
    [TestCase(3, Z3Status.Unsatisfiable)] // too small for two positive integers
    public void Exists_TwoVariables_FindsPythagoreanTriple(int target, Z3Status status)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();
        solver.SetTimeout(TimeSpan.FromSeconds(30));

        var x = context.IntConst("x");
        var y = context.IntConst("y");
        solver.Assert(context.Exists(x, y, (x * x + y * y == target) & (x > 0) & (y > 0)));

        Assert.That(solver.Check(), Is.EqualTo(status));
    }

    [TestCase(10, Z3Status.Satisfiable)] // e.g., 6 + 3 + 1
    [TestCase(3, Z3Status.Unsatisfiable)] // impossible with x > y > z > 0
    public void Exists_ThreeVariables_FindsOrderedSum(int sum, Z3Status status)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();
        solver.SetTimeout(TimeSpan.FromSeconds(30));

        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var z = context.IntConst("z");
        solver.Assert(context.Exists(x, y, z, (x + y + z == sum) & (x > y) & (y > z) & (z > 0)));

        Assert.That(solver.Check(), Is.EqualTo(status));
    }

    [TestCase(20, Z3Status.Satisfiable)] // e.g., 10 + 6 + 3 + 1
    [TestCase(6, Z3Status.Unsatisfiable)] // impossible with a > b > c > d > 0
    public void Exists_FourVariables_FindsOrderedQuadrupleWithSum(int sum, Z3Status status)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();
        solver.SetTimeout(TimeSpan.FromSeconds(30));

        var a = context.IntConst("a");
        var b = context.IntConst("b");
        var c = context.IntConst("c");
        var d = context.IntConst("d");
        solver.Assert(context.Exists(a, b, c, d, (a + b + c + d == sum) & (a > b) & (b > c) & (c > d) & (d > 0)));

        Assert.That(solver.Check(), Is.EqualTo(status));
    }

    [Test]
    public void ForAll_WithWeight_AndTriggers_DefinesPositiveFunction()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();
        solver.SetTimeout(TimeSpan.FromSeconds(30));

        var func = context.Func<IntExpr, IntExpr>("f");
        var x = context.IntConst("x");

        var trigger = func.Apply(x);
        solver.Assert(context.ForAll(1, [x], [trigger], func.Apply(x) > 0));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        // Verify that the function is indeed positive for some concrete values
        Assert.That(model.GetIntValue(func.Apply(5)), Is.GreaterThan(BigInteger.Zero));
        Assert.That(model.GetIntValue(func.Apply(-3)), Is.GreaterThan(BigInteger.Zero));
    }

    [Test]
    public void Exists_WithWeight_AndTriggers_FindsSpecificValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();
        solver.SetTimeout(TimeSpan.FromSeconds(30));

        var func = context.Func<IntExpr, IntExpr>("f");
        var x = context.IntConst("x");

        var trigger = func.Apply(x);
        solver.Assert(context.Exists(1, new[] { x }, new[] { trigger }, func.Apply(x) == 42));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        // Verify that we can find at least one value that maps to 42
        var y = context.IntConst("y");
        solver.Assert(func.Apply(y) == 42);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ForAll_MixedTypes_DefinesIntegerToRealConversion()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();
        solver.SetTimeout(TimeSpan.FromSeconds(30));

        var func = context.Func<IntExpr, RealExpr>("toReal");

        var x = context.IntConst("x");
        solver.Assert(context.ForAll(x, func.Apply(x) == x.ToReal()));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        // Verify the conversion works for specific values
        Assert.That(model.GetRealValue(func.Apply(5)), Is.EqualTo(new Real(5)));
        Assert.That(model.GetRealValue(func.Apply(-3)), Is.EqualTo(new Real(-3)));
        Assert.That(model.GetRealValue(func.Apply(0)), Is.EqualTo(new Real(0)));
    }

    [Test]
    public void ForAll_Exists_EveryIntegerHasSuccessor()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();
        solver.SetTimeout(TimeSpan.FromSeconds(30));

        var func = context.Func<IntExpr, IntExpr>("successor");

        var x = context.IntConst("x");
        var y = context.IntConst("y");
        // For all x, there exists y such that func(x) = y and y = x + 1
        solver.Assert(context.ForAll(x, context.Exists(y, (func.Apply(x) == y) & (y == x + 1))));
        solver.Assert(context.ForAll(x, func.Apply(x) == x + 1));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        // Verify the successor function works
        Assert.That(model.GetIntValue(func.Apply(5)), Is.EqualTo(new BigInteger(6)));
        Assert.That(model.GetIntValue(func.Apply(0)), Is.EqualTo(new BigInteger(1)));
        Assert.That(model.GetIntValue(func.Apply(-10)), Is.EqualTo(new BigInteger(-9)));
    }
}
