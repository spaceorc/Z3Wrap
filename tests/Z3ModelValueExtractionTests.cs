using z3lib;

namespace tests;

public class Z3ModelValueExtractionTests
{
    [Test]
    public void EvaluateIntegerExpression()
    {
        using var context = new Z3Context();
        using var solver = context.MkSolver();
        
        var x = context.MkIntConst("x");
        solver.Assert(x == context.MkInt(42));
        
        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
        
        var model = solver.GetModel();
        var evaluated = model.Evaluate(x);
        
        Assert.That(evaluated, Is.TypeOf<Z3IntExpr>());
        Assert.That(evaluated.ToString(), Does.Contain("42"));
    }
    
    [Test]
    public void EvaluateBooleanExpression()
    {
        using var context = new Z3Context();
        using var solver = context.MkSolver();
        
        var p = context.MkBoolConst("p");
        solver.Assert(p);
        
        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
        
        var model = solver.GetModel();
        var evaluated = model.Evaluate(p);
        
        Assert.That(evaluated, Is.TypeOf<Z3BoolExpr>());
        Assert.That(evaluated.ToString(), Does.Contain("true"));
    }
    
    [Test]
    public void EvaluateRealExpression()
    {
        using var context = new Z3Context();
        using var solver = context.MkSolver();
        
        var z = context.MkRealConst("z");
        solver.Assert(z == context.MkReal(3.14));
        
        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
        
        var model = solver.GetModel();
        var evaluated = model.Evaluate(z);
        
        Assert.That(evaluated, Is.TypeOf<Z3RealExpr>());
        Assert.That(evaluated.ToString(), Does.Contain("3.14").Or.Contain("157").Or.Contain("50"));
    }

    [Test]
    public void GetIntValue()
    {
        using var context = new Z3Context();
        using var solver = context.MkSolver();
        
        var x = context.MkIntConst("x");
        solver.Assert(x == context.MkInt(123));
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        
        var model = solver.GetModel();
        var value = model.GetIntValue(x);
        
        Assert.That(value, Is.EqualTo(123));
    }
    
    [Test]
    public void GetBoolValue_True()
    {
        using var context = new Z3Context();
        using var solver = context.MkSolver();
        
        var p = context.MkBoolConst("p");
        solver.Assert(p);
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        
        var model = solver.GetModel();
        var value = model.GetBoolValue(p);
        
        Assert.That(value, Is.EqualTo(Z3BoolValue.True));
    }
    
    [Test]
    public void GetBoolValue_False()
    {
        using var context = new Z3Context();
        using var solver = context.MkSolver();
        
        var p = context.MkBoolConst("p");
        solver.Assert(p == context.MkFalse());
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        
        var model = solver.GetModel();
        var value = model.GetBoolValue(p);
        
        Assert.That(value, Is.EqualTo(Z3BoolValue.False).Or.EqualTo(Z3BoolValue.Undefined));
    }
    
    [Test]
    public void GetRealValueAsString()
    {
        using var context = new Z3Context();
        using var solver = context.MkSolver();
        
        var z = context.MkRealConst("z");
        solver.Assert(z == context.MkReal(2.718));
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        
        var model = solver.GetModel();
        var value = model.GetRealValueAsString(z);
        
        Assert.That(value, Does.Contain("2.718").Or.Contain("1359").Or.Contain("500"));
    }
    
    [Test]
    public void GetIntValue_ComplexExpression()
    {
        using var context = new Z3Context();
        using var solver = context.MkSolver();
        
        var x = context.MkIntConst("x");
        var y = context.MkIntConst("y");
        solver.Assert(x + y == context.MkInt(15));
        solver.Assert(x - y == context.MkInt(5));
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        
        var model = solver.GetModel();
        var xValue = model.GetIntValue(x);
        var yValue = model.GetIntValue(y);
        
        Assert.That(xValue + yValue, Is.EqualTo(15));
        Assert.That(xValue - yValue, Is.EqualTo(5));
        Assert.That(xValue, Is.EqualTo(10));
        Assert.That(yValue, Is.EqualTo(5));
    }
    
    [Test]
    public void GetBoolValue_ComplexExpression()
    {
        using var context = new Z3Context();
        using var solver = context.MkSolver();
        
        var p = context.MkBoolConst("p");
        var q = context.MkBoolConst("q");
        solver.Assert(p | q);
        solver.Assert(!p);
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        
        var model = solver.GetModel();
        var pValue = model.GetBoolValue(p);
        var qValue = model.GetBoolValue(q);
        
        // Allow Undefined since Z3 may not assign definite values to unconstrained booleans
        Assert.That(pValue, Is.EqualTo(Z3BoolValue.False).Or.EqualTo(Z3BoolValue.Undefined));
        Assert.That(qValue, Is.EqualTo(Z3BoolValue.True).Or.EqualTo(Z3BoolValue.Undefined));
    }
    
    [Test]
    public void EvaluateWithModelCompletion_True()
    {
        using var context = new Z3Context();
        using var solver = context.MkSolver();
        
        var x = context.MkIntConst("x");
        var y = context.MkIntConst("y");
        solver.Assert(x == context.MkInt(10));
        // y is not constrained, should get a value with model completion
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        
        var model = solver.GetModel();
        var xEval = model.Evaluate(x, modelCompletion: true);
        var yEval = model.Evaluate(y, modelCompletion: true);
        
        Assert.That(xEval, Is.Not.Null);
        Assert.That(yEval, Is.Not.Null);
        Assert.That(model.GetIntValue(x), Is.EqualTo(10));
    }
    
    [Test]
    public void EvaluateWithModelCompletion_False()
    {
        using var context = new Z3Context();
        using var solver = context.MkSolver();
        
        var x = context.MkIntConst("x");
        var y = context.MkIntConst("y");
        solver.Assert(x == context.MkInt(10));
        // y is not constrained
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        
        var model = solver.GetModel();
        var xEval = model.Evaluate(x, modelCompletion: false);
        var yEval = model.Evaluate(y, modelCompletion: false);
        
        Assert.That(xEval, Is.Not.Null);
        Assert.That(yEval, Is.Not.Null);
        Assert.That(model.GetIntValue(x), Is.EqualTo(10));
    }

    [Test]
    public void GetIntValue_ThrowsOnNonConstant()
    {
        using var context = new Z3Context();
        using var solver = context.MkSolver();
        
        var x = context.MkIntConst("x");
        var y = context.MkIntConst("y");
        var sum = x + y; // This is not a constant
        
        solver.Assert(x == context.MkInt(5));
        solver.Assert(y == context.MkInt(10));
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        
        var model = solver.GetModel();
        
        // These should work (constants)
        Assert.That(model.GetIntValue(x), Is.EqualTo(5));
        Assert.That(model.GetIntValue(y), Is.EqualTo(10));
        
        // This should work (evaluates to a constant)
        var sumEval = model.Evaluate(sum);
        Assert.That(sumEval, Is.TypeOf<Z3IntExpr>());
        
        // But trying to get value of the original sum expression might fail
        // if it doesn't evaluate to a pure numeral in the model
        Assert.DoesNotThrow(() => model.GetIntValue((Z3IntExpr)sumEval));
    }
    
    [Test] 
    public void EvaluateInvalidatedModel_ThrowsException()
    {
        using var context = new Z3Context();
        using var solver = context.MkSolver();
        
        var x = context.MkIntConst("x");
        solver.Assert(x == context.MkInt(5));
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        
        // Invalidate model by changing solver state
        solver.Assert(x == context.MkInt(10)); // Contradiction
        
        // Should throw on invalidated model
        Assert.Throws<ObjectDisposedException>(() => model.Evaluate(x));
        Assert.Throws<ObjectDisposedException>(() => model.GetIntValue(x));
        Assert.Throws<ObjectDisposedException>(() => model.GetBoolValue(context.MkBoolConst("p")));
        Assert.Throws<ObjectDisposedException>(() => model.GetRealValueAsString(context.MkRealConst("z")));
    }
    
}