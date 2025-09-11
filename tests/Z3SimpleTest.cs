using z3lib;

namespace tests;

public class Z3SimpleTest
{

    [Test]
    public void SimpleArithmeticUnsatisfiable()
    {
        using var context = new Z3Context();
        using var solver = context.MkSolver();
        
        var x = context.MkIntConst("x");
        var zero = context.MkInt(0);
        var one = context.MkInt(1);
        
        // x > 0 and x < 1 - should be unsatisfiable for integers
        solver.Assert(x > zero);
        solver.Assert(x < one);
        
        var result = solver.Check();
        Console.WriteLine($"Arithmetic result: {result}");
        
        if (result == Z3Status.Unknown)
        {
            Console.WriteLine($"Reason: {solver.GetReasonUnknown()}");
        }
    }

    [Test]
    public void VerySimpleUnsatisfiable()
    {
        using var context = new Z3Context();
        using var solver = context.MkSimpleSolver();  // Try simple solver
        
        // Just assert false directly
        var falseExpr = context.MkFalse();
        solver.Assert(falseExpr);
        
        var result = solver.Check();
        Console.WriteLine($"False assertion result (simple solver): {result}");
        
        if (result == Z3Status.Unknown)
        {
            Console.WriteLine($"Reason: {solver.GetReasonUnknown()}");
        }
        
        Assert.That(result, Is.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void SimpleContradiction()
    {
        using var context = new Z3Context();
        using var solver = context.MkSolver();
        
        var p = context.MkTrue();
        var notP = context.MkNot(p);
        
        // p and not p - classic contradiction
        solver.Assert(p);
        solver.Assert(notP);
        
        var result = solver.Check();
        Console.WriteLine($"Contradiction result: {result}");
        
        if (result == Z3Status.Unknown)
        {
            Console.WriteLine($"Reason: {solver.GetReasonUnknown()}");
        }
        
        Assert.That(result, Is.EqualTo(Z3Status.Unsatisfiable));
    }
}