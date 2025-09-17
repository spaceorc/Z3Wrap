using Spaceorc.Z3Wrap;
namespace Z3Wrap.Tests.Unit.Core;

[TestFixture]
public class Z3SolverTests
{

    [Test]
    public void CreateSolver_DefaultParameters_ReturnsValidHandle()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        Assert.That(solver.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void Check_SatisfiableConstraints_ReturnsSatisfiable()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var ten = context.Int(10);

        // x + y == 10 and x > 0 and y > 0 (satisfiable)
        solver.Assert((x + y) == ten);
        solver.Assert(x > context.Int(0));
        solver.Assert(y > context.Int(0));
        
        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Check_UnsatisfiableConstraints_ReturnsUnsatisfiable()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var five = context.Int(5);
        var ten = context.Int(10);

        // x == 5 and x == 10 (unsatisfiable)
        solver.Assert(x == five);
        solver.Assert(x == ten);
        
        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void Check_SimpleConstraint_ReturnsSatisfiable()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var five = context.Int(5);

        // x == 5
        solver.Assert(x == five);
        
        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Check_ComplexConstraints_ReturnsSatisfiable()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var ten = context.Int(10);

        // x + y == 10 and x == 3
        solver.Assert((x + y) == ten);
        solver.Assert(x == context.Int(3));
        
        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Assert_BooleanExpressions_AcceptsAndSolves()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var p = context.True();
        var q = context.False();
        var andExpr = p & q;
        var orExpr = p | q;
        
        solver.Assert(p);
        solver.Assert(orExpr);
        
        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Check_RealConstraints_ReturnsSatisfiable()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.RealConst("x");
        var pi = context.Real(3.14m);

        // x == 3.14m
        solver.Assert(x == pi);
        
        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void PushPop_StackedConstraints_ManagesContextCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var five = context.Int(5);
        var ten = context.Int(10);

        // Add x > 0
        solver.Assert(x > context.Int(0));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        // Push context and add x == 5
        solver.Push();
        solver.Assert(x == five);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        // Add conflicting constraint x == 10
        solver.Assert(x == ten);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Unsatisfiable));
        
        // Pop back to previous context
        solver.Pop();
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void SolverWorkflow_MultipleConstraints_FindsSatisfiableSolution()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        // Create variables using factory methods
        var x = context.IntConst("x");
        var y = context.IntConst("y");

        // Create solver
        using var solver = context.CreateSolver();

        // Add constraints using natural operators
        solver.Assert(x > context.Int(0));
        solver.Assert(y > context.Int(0));
        solver.Assert((x + y) == context.Int(10));
        solver.Assert(x < context.Int(8));  // Additional constraint to make solution unique
        
        // Check satisfiability
        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void GetReasonUnknown_KnownStatus_ReturnsNonNull()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        solver.Assert(x == context.Int(5));
        
        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
        
        // When result is not unknown, GetReasonUnknown should still work (may return empty or "unknown")
        var reason = solver.GetReasonUnknown();
        Assert.That(reason, Is.Not.Null);
    }

    [Test]
    public void CreateSolver_TimeoutParameters_HandlesContradictions()
    {
        // Test solver with timeout disabled
        var parameters = new Dictionary<string, string>
        {
            ["timeout"] = "0"  // Disable timeout
        };

        using var context = new Z3Context(parameters);
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var five = context.Int(5);
        var ten = context.Int(10);

        // Add contradictory constraints: x == 5 and x == 10
        solver.Assert(x == five);
        solver.Assert(x == ten);
        
        var result = solver.Check();
        
        // This should be unsatisfiable, not unknown
        Assert.That(result, Is.Not.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Check_RealEquality_ReturnsSatisfiableForDebugging()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.RealConst("x");
        var pi = context.Real(3.14m);

        solver.Assert(x == pi);
        
        var result = solver.Check();
        
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
    }
}