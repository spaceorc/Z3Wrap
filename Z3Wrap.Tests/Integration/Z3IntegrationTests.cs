using Spaceorc.Z3Wrap;
using Spaceorc.Z3Wrap.Extensions;

namespace Z3Wrap.Tests.Integration;

[TestFixture]
public class Z3IntegrationTests
{
    [Test]
    public void IntegerConstraints_ContradictoryBounds_ReturnsUnsatisfiable()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var zero = context.Int(0);
        var one = context.Int(1);

        // x > 0 and x < 1 - should be unsatisfiable for integers
        solver.Assert(x > zero);
        solver.Assert(x < one);

        var result = solver.Check();

        Assert.That(
            result,
            Is.EqualTo(Z3Status.Unsatisfiable),
            "x > 0 and x < 1 should be unsatisfiable for integers"
        );
    }

    [Test]
    public void AssertFalse_SimpleSolver_ReturnsUnsatisfiable()
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
    public void Contradiction_TrueAndNotTrue_ReturnsUnsatisfiable()
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
