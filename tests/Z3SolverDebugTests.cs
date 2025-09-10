using z3lib;

namespace tests;

public class Z3SolverDebugTests
{
    [OneTimeSetUp]
    public void Setup()
    {
        NativeMethods.LoadLibrary("/opt/homebrew/opt/z3/lib/libz3.dylib");
    }

    [Test]
    public void DebugUnknownResults()
    {
        // Try with solver timeout disabled
        var parameters = new Dictionary<string, string>
        {
            ["timeout"] = "0"  // Disable timeout
        };
        
        using var context = new Z3Context(parameters);
        using var solver = context.MkSolver();
        
        var x = context.MkIntConst("x");
        var five = context.MkInt(5);
        var ten = context.MkInt(10);
        
        // Add contradictory constraints: x == 5 and x == 10
        solver.Assert(x == five);
        solver.Assert(x == ten);
        
        var result = solver.Check();
        Console.WriteLine($"Result: {result}");
        
        if (result == Z3Status.Unknown)
        {
            Console.WriteLine($"Reason unknown: {solver.GetReasonUnknown()}");
        }
        
        // This should be unsatisfiable, not unknown
        Assert.That(result, Is.Not.EqualTo(Z3Status.Satisfiable));
    }

    [Test] 
    public void DebugSatisfiableCase()
    {
        using var context = new Z3Context();
        using var solver = context.MkSolver();
        
        var x = context.MkIntConst("x");
        var five = context.MkInt(5);
        
        // Simple satisfiable constraint
        solver.Assert(x == five);
        
        var result = solver.Check();
        Console.WriteLine($"Satisfiable case result: {result}");
        
        if (result == Z3Status.Unknown)
        {
            Console.WriteLine($"Reason unknown: {solver.GetReasonUnknown()}");
        }
        
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void CheckRealConstraints()
    {
        using var context = new Z3Context();
        using var solver = context.MkSolver();
        
        var x = context.MkRealConst("x");
        var pi = context.MkReal(3.14);
        
        solver.Assert(x == pi);
        
        var result = solver.Check();
        Console.WriteLine($"Real constraint result: {result}");
        
        if (result == Z3Status.Unknown)
        {
            Console.WriteLine($"Reason unknown: {solver.GetReasonUnknown()}");
        }
        
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
    }
}