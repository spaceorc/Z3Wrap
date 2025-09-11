using z3lib;

namespace tests;

public class Z3RealIntMixedTest
{
    [Test]
    public void RealExpr_ComparisonWithInt()
    {
        using var context = new Z3Context();
        using var solver = context.MkSolver();
        
        var x = context.MkRealConst("x");
        
        // This should work: myRealExpr < 10
        solver.Assert(x < 10);  // int constant with real expression
        solver.Assert(x > 5);   // int constant with real expression
        solver.Assert(7 < x);   // reversed: int < real
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test] 
    public void RealExpr_EqualityWithInt()
    {
        using var context = new Z3Context();
        using var solver = context.MkSolver();
        
        var x = context.MkRealConst("x");
        
        // Test equality with int: x == 7 instead of x == 7.0
        solver.Assert(x == 7);
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var value = model.GetRealValueAsString(x);
        Assert.That(value, Does.Contain("7"));
    }

    [Test]
    public void RealExpr_ArithmeticWithInt()
    {
        using var context = new Z3Context();
        using var solver = context.MkSolver();
        
        var x = context.MkRealConst("x");
        
        // Test arithmetic with int: x + 3 == 10, so x should be 7
        solver.Assert(x + 3 == 10);
        solver.Assert(x > 0);
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var value = model.GetRealValueAsString(x);
        Assert.That(value, Does.Contain("7"));
    }

    [Test]
    public void RealExpr_ReversedArithmeticWithInt()
    {
        using var context = new Z3Context();
        using var solver = context.MkSolver();
        
        var x = context.MkRealConst("x");
        
        // Test reversed arithmetic: 15 - x == 10, so x should be 5
        solver.Assert(15 - x == 10);
        solver.Assert(x > 0);
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var value = model.GetRealValueAsString(x);
        Assert.That(value, Does.Contain("5"));
    }
}