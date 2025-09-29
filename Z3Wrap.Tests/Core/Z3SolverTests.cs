using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Expressions.Numerics;

namespace Z3Wrap.Tests.Core;

[TestFixture]
public class Z3SolverTests
{
    [Test]
    public void CreateSolver_ReturnsValidHandle()
    {
        using var context = new Z3Context();

        using var solver = context.CreateSolver();

        Assert.That(solver.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void Check_WithoutConstraints_ReturnsSatisfiable()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var result = solver.Check();

        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Assert_SimpleConstraint_DoesNotThrow()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();
        var trueExpr = context.True();

        Assert.DoesNotThrow(() => solver.Assert(trueExpr));
    }

    [Test]
    public void Reset_ClearsConstraints()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();
        solver.Assert(context.False());

        solver.Reset();
        var result = solver.Check();

        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Push_Pop_ManagesScopes()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();
        var x = context.IntConst("x");
        solver.Assert(x > 0);

        solver.Push();
        solver.Assert(x < 0);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Unsatisfiable));

        solver.Pop();
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void GetModel_AfterSatisfiableCheck_ReturnsModel()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();
        solver.Assert(context.True());

        solver.Check();
        var model = solver.GetModel();

        Assert.That(model, Is.Not.Null);
    }

    [Test]
    public void GetReasonUnknown_ReturnsNonNull()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        solver.Check();

        var reason = solver.GetReasonUnknown();

        Assert.That(reason, Is.Not.Null);
    }
}
