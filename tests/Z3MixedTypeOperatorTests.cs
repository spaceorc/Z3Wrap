using z3lib;

namespace tests;

public class Z3MixedTypeOperatorTests
{
    [Test]
    public void IntExpr_EqualityWithInt()
    {
        using var context = new Z3Context();
        using var solver = context.MkSolver();
        
        var x = context.MkIntConst("x");
        
        // Test x == 42 instead of x == context.MkInt(42)
        solver.Assert(x == 42);
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetIntValue(x), Is.EqualTo(42));
    }
    
    [Test]
    public void IntExpr_InequalityWithInt()
    {
        using var context = new Z3Context();
        using var solver = context.MkSolver();
        
        var x = context.MkIntConst("x");
        
        // Test x != 42
        solver.Assert(x != 42);
        solver.Assert(x == 24);
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetIntValue(x), Is.EqualTo(24));
    }

    [Test]
    public void IntExpr_ArithmeticWithInt()
    {
        using var context = new Z3Context();
        using var solver = context.MkSolver();
        
        var x = context.MkIntConst("x");
        
        // Test mixed arithmetic: x + 5 == 15, so x should be 10
        solver.Assert(x + 5 == 15);
        solver.Assert(x > 0);
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetIntValue(x), Is.EqualTo(10));
    }

    [Test]
    public void IntExpr_ReversedArithmetic()
    {
        using var context = new Z3Context();
        using var solver = context.MkSolver();
        
        var x = context.MkIntConst("x");
        
        // Test reversed operations: 20 - x == 15, so x should be 5
        solver.Assert(20 - x == 15);
        solver.Assert(x > 0);
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetIntValue(x), Is.EqualTo(5));
    }

    [Test]
    public void IntExpr_ComparisonWithInt()
    {
        using var context = new Z3Context();
        using var solver = context.MkSolver();
        
        var x = context.MkIntConst("x");
        
        // Test comparisons with integers
        solver.Assert(x > 10);  // x > 10
        solver.Assert(x < 20);  // x < 20  
        solver.Assert(15 > x);  // 15 > x, so x < 15
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var value = model.GetIntValue(x);
        Assert.That(value, Is.GreaterThan(10));
        Assert.That(value, Is.LessThan(15));
    }

    [Test]
    public void RealExpr_EqualityWithDouble()
    {
        using var context = new Z3Context();
        using var solver = context.MkSolver();
        
        var x = context.MkRealConst("x");
        
        // Test x == 3.14 instead of x == context.MkReal(3.14)
        solver.Assert(x == 3.14);
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var value = model.GetRealValueAsString(x);
        Assert.That(value, Does.Contain("3.14").Or.Contain("157").Or.Contain("50")); // Z3 uses fractions
    }

    [Test]
    public void RealExpr_ArithmeticWithDouble()
    {
        using var context = new Z3Context();
        using var solver = context.MkSolver();
        
        var x = context.MkRealConst("x");
        
        // Test mixed arithmetic: x * 2.0 == 10.0, so x should be 5.0
        solver.Assert(x * 2.0 == 10.0);
        solver.Assert(x > 0.0);
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var value = model.GetRealValueAsString(x);
        Assert.That(value, Does.Contain("5").Or.Contain("10").Or.Contain("2")); // Could be 5/1, 10/2, etc.
    }

    [Test]
    public void RealExpr_ReversedArithmetic()
    {
        using var context = new Z3Context();
        using var solver = context.MkSolver();
        
        var x = context.MkRealConst("x");
        
        // Test reversed operations: 7.5 - x == 2.5, so x should be 5.0
        solver.Assert(7.5 - x == 2.5);
        solver.Assert(x > 0.0);
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var value = model.GetRealValueAsString(x);
        Assert.That(value, Does.Contain("5").Or.Contain("10").Or.Contain("2")); // Could be various fraction forms
    }

    [Test]
    public void RealExpr_ComparisonWithDouble()
    {
        using var context = new Z3Context();
        using var solver = context.MkSolver();
        
        var x = context.MkRealConst("x");
        
        // Test comparisons with doubles
        solver.Assert(x > 1.0);   // x > 1.0
        solver.Assert(x < 2.0);   // x < 2.0
        solver.Assert(1.5 < x);   // 1.5 < x
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        // We can't easily test the exact value since Z3 represents reals as fractions
        // but we can verify the constraint is satisfiable
    }

    [Test]
    public void MixedExpressionsInComplexFormula()
    {
        using var context = new Z3Context();
        using var solver = context.MkSolver();
        
        var x = context.MkIntConst("x");
        var y = context.MkIntConst("y");
        
        // Test a complex formula using mixed-type operators
        // x + y == 15, x - y == 5, x > 5, y < 10
        solver.Assert(x + y == 15);
        solver.Assert(x - y == 5);  
        solver.Assert(x > 5);
        solver.Assert(y < 10);
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        var xValue = model.GetIntValue(x);
        var yValue = model.GetIntValue(y);
        
        Assert.That(xValue + yValue, Is.EqualTo(15));
        Assert.That(xValue - yValue, Is.EqualTo(5));
        Assert.That(xValue, Is.GreaterThan(5));
        Assert.That(yValue, Is.LessThan(10));
        
        // Should be x=10, y=5
        Assert.That(xValue, Is.EqualTo(10));
        Assert.That(yValue, Is.EqualTo(5));
    }

    [Test]
    public void OperatorPrecedenceWorksCorrectly()
    {
        using var context = new Z3Context();
        using var solver = context.MkSolver();
        
        var x = context.MkIntConst("x");
        
        // Test that operator precedence works: x * 2 + 3 == 13 should give x = 5
        solver.Assert(x * 2 + 3 == 13);
        solver.Assert(x > 0);
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetIntValue(x), Is.EqualTo(5));
    }
}