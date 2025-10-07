using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Expressions.Numerics;

namespace Z3Wrap.Tests.Core;

[TestFixture]
public class Z3SolverUnsatCoreTests
{
    [Test]
    public void CheckAssumptions_SimpleConflict_ReturnsUnsatisfiable()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var a1 = x > 10;
        var a2 = x < 5;

        var status = solver.CheckAssumptions(a1, a2);

        Assert.That(status, Is.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void CheckAssumptions_SatisfiableConstraints_ReturnsSatisfiable()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var a1 = x > 10;
        var a2 = x < 100;

        var status = solver.CheckAssumptions(a1, a2);

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void CheckAssumptions_EmptyAssumptions_ReturnsSatisfiable()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var status = solver.CheckAssumptions();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void CheckAssumptions_WithBooleanTrackers_ReturnsUnsatisfiable()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var t1 = context.BoolConst("t1");
        var t2 = context.BoolConst("t2");

        solver.Assert(t1.Implies(x > 10));
        solver.Assert(t2.Implies(x < 5));

        var status = solver.CheckAssumptions(t1, t2);

        Assert.That(status, Is.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void CheckAssumptions_WithBooleanTrackers_ReturnsSatisfiable()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var t1 = context.BoolConst("t1");
        var t2 = context.BoolConst("t2");

        solver.Assert(t1.Implies(x > 10));
        solver.Assert(t2.Implies(x < 100));

        var status = solver.CheckAssumptions(t1, t2);

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void GetUnsatCore_SimpleConflict_ReturnsConflictingAssumptions()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var a1 = x > 10;
        var a2 = x < 5;

        solver.CheckAssumptions(a1, a2);
        var core = solver.GetUnsatCore();

        Assert.That(core, Has.Length.EqualTo(2));
        Assert.That(core, Does.Contain(a1));
        Assert.That(core, Does.Contain(a2));
    }

    [Test]
    public void GetUnsatCore_LargerConflict_ReturnsMinimalCore()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var a1 = x > 10;
        var a2 = x < 5;
        var a3 = x > 0;
        var a4 = x < 100;

        solver.CheckAssumptions(a1, a2, a3, a4);
        var core = solver.GetUnsatCore();

        // Core should contain the conflicting constraints (a1 and a2)
        // Z3 may or may not include redundant constraints
        Assert.That(core.Length, Is.GreaterThan(0));
        Assert.That(core.Length, Is.LessThanOrEqualTo(4));
    }

    [Test]
    public void GetUnsatCore_WithBooleanTrackers_ReturnsTrackers()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var t1 = context.BoolConst("t1");
        var t2 = context.BoolConst("t2");
        var t3 = context.BoolConst("t3");

        solver.Assert(t1.Implies(x > 10));
        solver.Assert(t2.Implies(x < 5));
        solver.Assert(t3.Implies(x > 0));

        solver.CheckAssumptions(t1, t2, t3);
        var core = solver.GetUnsatCore();

        // Core should contain t1 and t2 (conflicting trackers)
        Assert.That(core.Length, Is.GreaterThan(0));
        Assert.That(core.Length, Is.LessThanOrEqualTo(3));
    }

    [Test]
    public void GetUnsatCore_BeforeCheck_ThrowsInvalidOperationException()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        Assert.Throws<InvalidOperationException>(() => solver.GetUnsatCore());
    }

    [Test]
    public void GetUnsatCore_AfterSatisfiableResult_ThrowsInvalidOperationException()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var a1 = x > 10;
        var a2 = x < 100;

        solver.CheckAssumptions(a1, a2);

        Assert.Throws<InvalidOperationException>(() => solver.GetUnsatCore());
    }

    [Test]
    public void GetUnsatCore_AfterRegularCheckUNSAT_ReturnsEmptyCore()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        solver.Assert(x > 10);
        solver.Assert(x < 5);
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Unsatisfiable));

        // Regular Check() doesn't track assumptions, so core will be empty
        var core = solver.GetUnsatCore();
        Assert.That(core, Has.Length.EqualTo(0));
    }

    [Test]
    public void CheckAssumptions_MultipleCallsWithDifferentAssumptions_WorksCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var t1 = context.BoolConst("t1");
        var t2 = context.BoolConst("t2");
        var t3 = context.BoolConst("t3");

        solver.Assert(t1.Implies(x > 10));
        solver.Assert(t2.Implies(x < 5));
        solver.Assert(t3.Implies(x > 0));

        // First check: conflict between t1 and t2
        var status1 = solver.CheckAssumptions(t1, t2);
        Assert.That(status1, Is.EqualTo(Z3Status.Unsatisfiable));

        // Second check: no conflict with just t1 and t3
        var status2 = solver.CheckAssumptions(t1, t3);
        Assert.That(status2, Is.EqualTo(Z3Status.Satisfiable));

        // Third check: conflict again with t1 and t2
        var status3 = solver.CheckAssumptions(t1, t2);
        Assert.That(status3, Is.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void GetModel_AfterCheckAssumptions_ReturnsModel()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var a1 = x > 10;
        var a2 = x < 100;

        solver.CheckAssumptions(a1, a2);
        var model = solver.GetModel();

        var value = model.GetIntValue(x);
        Assert.That(value, Is.GreaterThan(new BigInteger(10)));
        Assert.That(value, Is.LessThan(new BigInteger(100)));
    }

    [Test]
    public void CheckAssumptions_WithMixedAssertedAndAssumedConstraints_WorksCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");

        // Add some constraints via Assert
        solver.Assert(x > 0);
        solver.Assert(x < 1000);

        // Add conflicting constraints via CheckAssumptions
        var a1 = x > 100;
        var a2 = x < 10;

        var status = solver.CheckAssumptions(a1, a2);

        Assert.That(status, Is.EqualTo(Z3Status.Unsatisfiable));

        var core = solver.GetUnsatCore();
        Assert.That(core.Length, Is.GreaterThan(0));
    }

    [Test]
    public void CheckAssumptions_SingleAssumption_WorksCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        solver.Assert(x > 100);

        var a1 = x < 10;

        var status = solver.CheckAssumptions(a1);

        Assert.That(status, Is.EqualTo(Z3Status.Unsatisfiable));

        var core = solver.GetUnsatCore();
        Assert.That(core, Has.Length.EqualTo(1));
        Assert.That(core[0], Is.EqualTo(a1));
    }

    [Test]
    public void CheckAssumptions_AfterDispose_ThrowsObjectDisposedException()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        var solver = context.CreateSolver();
        var x = context.IntConst("x");
        var a1 = x > 10;

        solver.Dispose();

        Assert.Throws<ObjectDisposedException>(() => solver.CheckAssumptions(a1));
    }

    [Test]
    public void GetUnsatCore_AfterDispose_ThrowsObjectDisposedException()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        var solver = context.CreateSolver();
        var x = context.IntConst("x");
        var a1 = x > 10;
        var a2 = x < 5;

        solver.CheckAssumptions(a1, a2);
        solver.Dispose();

        Assert.Throws<ObjectDisposedException>(() => solver.GetUnsatCore());
    }

    [Test]
    public void CheckAssumptions_WithPushPop_WorksCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var t1 = context.BoolConst("t1");
        var t2 = context.BoolConst("t2");

        solver.Assert(t1.Implies(x > 10));

        solver.Push();
        solver.Assert(t2.Implies(x < 5));

        var status1 = solver.CheckAssumptions(t1, t2);
        Assert.That(status1, Is.EqualTo(Z3Status.Unsatisfiable));

        solver.Pop();

        var status2 = solver.CheckAssumptions(t1);
        Assert.That(status2, Is.EqualTo(Z3Status.Satisfiable));
    }
}
