namespace Z3Wrap.Tests.CoreTests;

public class Z3SimpleTest
{
    [Test]
    public void SimpleArithmeticUnsatisfiable()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var zero = context.Int(0);
        var one = context.Int(1);

        // x > 0 and x < 1 - should be unsatisfiable for integers
        solver.Assert(x > zero);
        solver.Assert(x < one);

        var result = solver.Check();

        Assert.That(result, Is.EqualTo(Z3Status.Unsatisfiable), "x > 0 and x < 1 should be unsatisfiable for integers");
    }

    [Test]
    public void VerySimpleUnsatisfiable()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSimpleSolver(); // Try simple solver

        // Just assert false directly
        var falseExpr = context.False();
        solver.Assert(falseExpr);

        var result = solver.Check();

        Assert.That(result, Is.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void SimpleContradiction()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var p = context.True();
        var notP = context.Not(p);

        // p and not p - classic contradiction
        solver.Assert(p);
        solver.Assert(notP);

        var result = solver.Check();

        Assert.That(result, Is.EqualTo(Z3Status.Unsatisfiable));
    }
}