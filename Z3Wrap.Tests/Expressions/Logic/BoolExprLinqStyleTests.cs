using NUnit.Framework;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Expressions.Numerics;

namespace Z3Wrap.Tests.Expressions.Logic;

/// <summary>
/// Tests for LINQ-style boolean extension methods (All, Any).
/// </summary>
[TestFixture]
public class BoolExprLinqStyleTests
{
    [Test]
    public void All_WithPredicate_AllTrue_ReturnsTrue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var values = new[] { 2, 4, 6, 8 };
        var result = values.All(x => context.Int(x) > 0);

        solver.Assert(result);
        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(result), Is.True);
    }

    [Test]
    public void All_WithPredicate_OneFalse_ReturnsFalse()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var values = new[] { 2, 4, -1, 8 };
        var result = values.All(x => context.Int(x) > 0);

        solver.Assert(result);
        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void All_EmptySequence_ReturnsTrue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var values = Array.Empty<int>();
        var result = values.All(x => context.Int(x) > 0);

        solver.Assert(result);
        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(result), Is.True);
    }

    [Test]
    public void Any_WithPredicate_OneTrue_ReturnsTrue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var values = new[] { -2, -4, 1, -8 };
        var result = values.Any(x => context.Int(x) > 0);

        solver.Assert(result);
        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(result), Is.True);
    }

    [Test]
    public void Any_WithPredicate_AllFalse_ReturnsFalse()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var values = new[] { -2, -4, -1, -8 };
        var result = values.Any(x => context.Int(x) > 0);

        solver.Assert(result);
        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void Any_EmptySequence_ReturnsFalse()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var values = Array.Empty<int>();
        var result = values.Any(x => context.Int(x) > 0);

        solver.Assert(result);
        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Unsatisfiable));
    }
}
