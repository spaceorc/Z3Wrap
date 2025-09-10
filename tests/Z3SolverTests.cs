using z3lib;

namespace tests;

public class Z3SolverTests
{
    [OneTimeSetUp]
    public void Setup()
    {
        NativeMethods.LoadLibrary("/opt/homebrew/opt/z3/lib/libz3.dylib");
    }

    [Test]
    public void CanCreateSolver()
    {
        using var context = new Z3Context();
        using var solver = context.MkSolver();
        
        Assert.That(solver.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void CanCheckSatisfiabilityWithSatisfiableConstraints()
    {
        using var context = new Z3Context();
        using var solver = context.MkSolver();
        
        var x = context.MkIntConst("x");
        var y = context.MkIntConst("y");
        var ten = context.MkInt(10);
        
        // x + y == 10 and x > 0 and y > 0 (satisfiable)
        solver.Assert((x + y) == ten);
        solver.Assert(x > context.MkInt(0));
        solver.Assert(y > context.MkInt(0));
        
        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void CanCheckSatisfiabilityWithUnsatisfiableConstraints()
    {
        using var context = new Z3Context();
        using var solver = context.MkSolver();
        
        var x = context.MkIntConst("x");
        var five = context.MkInt(5);
        var ten = context.MkInt(10);
        
        // x == 5 and x == 10 (unsatisfiable)
        solver.Assert(x == five);
        solver.Assert(x == ten);
        
        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void CanGetSatisfiableResult()
    {
        using var context = new Z3Context();
        using var solver = context.MkSolver();
        
        var x = context.MkIntConst("x");
        var five = context.MkInt(5);
        
        // x == 5
        solver.Assert(x == five);
        
        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void CanSolveComplexConstraints()
    {
        using var context = new Z3Context();
        using var solver = context.MkSolver();
        
        var x = context.MkIntConst("x");
        var y = context.MkIntConst("y");
        var ten = context.MkInt(10);
        
        // x + y == 10 and x == 3
        solver.Assert((x + y) == ten);
        solver.Assert(x == context.MkInt(3));
        
        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void CanCreateBooleanExpressions()
    {
        using var context = new Z3Context();
        using var solver = context.MkSolver();
        
        var p = context.MkTrue();
        var q = context.MkFalse();
        var andExpr = p & q;
        var orExpr = p | q;
        
        // Should be able to assert boolean expressions
        solver.Assert(p);
        solver.Assert(orExpr);
        
        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void CanSolveRealConstraints()
    {
        using var context = new Z3Context();
        using var solver = context.MkSolver();
        
        var x = context.MkRealConst("x");
        var pi = context.MkReal(3.14);
        
        // x == 3.14
        solver.Assert(x == pi);
        
        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void CanUsePushAndPop()
    {
        using var context = new Z3Context();
        using var solver = context.MkSolver();
        
        var x = context.MkIntConst("x");
        var five = context.MkInt(5);
        var ten = context.MkInt(10);
        
        // Add x > 0
        solver.Assert(x > context.MkInt(0));
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
    public void CompleteUsagePattern()
    {
        using var context = new Z3Context();
        
        // Create variables using factory methods
        var x = context.MkIntConst("x");
        var y = context.MkIntConst("y");
        
        // Create solver
        using var solver = context.MkSolver();
        
        // Add constraints using natural operators
        solver.Assert(x > context.MkInt(0));
        solver.Assert(y > context.MkInt(0));
        solver.Assert((x + y) == context.MkInt(10));
        solver.Assert(x < context.MkInt(8));  // Additional constraint to make solution unique
        
        // Check satisfiability
        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void CanDetectUnsatisfiableConstraints()
    {
        using var context = new Z3Context();
        using var solver = context.MkSolver();
        
        var x = context.MkIntConst("x");
        
        // Add unsatisfiable constraints
        solver.Assert(x == context.MkInt(5));
        solver.Assert(x == context.MkInt(10));
        
        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Unsatisfiable));
    }
}