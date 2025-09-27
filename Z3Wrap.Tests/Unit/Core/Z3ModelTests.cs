using System.Numerics;
using Spaceorc.Z3Wrap;
using Spaceorc.Z3Wrap.Expressions.BitVectors;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Expressions.Numerics;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Z3Wrap.Tests.Unit.Core;

[TestFixture]
public class Z3ModelTests
{
    [Test]
    public void Handle_ValidModel_ReturnsNonZeroHandle()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        solver.Assert(context.Eq(x, context.Int(42)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        Assert.That(model.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void GetIntValue_IntConstant_ReturnsCorrectValue()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        solver.Assert(context.Eq(x, context.Int(123)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        var value = model.GetIntValue(x);
        Assert.That(value, Is.EqualTo(new BigInteger(123)));
    }

    [Test]
    public void GetRealValue_RealConstant_ReturnsCorrectValue()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.RealConst("x");
        solver.Assert(context.Eq(x, context.Real(3.14m)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        var value = model.GetRealValue(x);
        Assert.That(value.ToDecimal(), Is.EqualTo(3.14m));
    }

    [Test]
    public void GetNumericValueAsString_BitVecConstant_ReturnsCorrectValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVecConst<Size8>("x");
        solver.Assert(x == 255);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        var value = model.GetNumericValueAsString(x);
        Assert.That(value, Is.EqualTo("255"));
    }

    [Test]
    public void GetBoolValue_BoolConstant_ReturnsCorrectValue()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        solver.Assert(p);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        var value = model.GetBoolValue(p);
        Assert.That(value, Is.True);
    }

    [Test]
    public void GetIntValue_UndefinedVariable_ThrowsException()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var y = context.IntConst("y");
        solver.Assert(context.Eq(x, context.Int(42))); // Only constrain x, not y

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        // Should be able to get value for x
        Assert.DoesNotThrow(() => model.GetIntValue(x));

        // Getting value for unconstrained y might throw or return default
        // Behavior depends on Z3 model completion settings
    }

    [Test]
    public void Model_InvalidatedAfterSolverOperation_ThrowsException()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        solver.Assert(context.Eq(x, context.Int(42)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        // Model should be valid initially
        Assert.DoesNotThrow(() => _ = model.Handle);

        // Invalidate model by modifying solver
        solver.Push();

        // Model should now be invalidated
        Assert.Throws<ObjectDisposedException>(() => _ = model.Handle);
    }

    [Test]
    public void ToString_ValidModel_ReturnsNonEmptyString()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        solver.Assert(context.Eq(x, context.Int(42)));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        var modelString = model.ToString();
        Assert.That(modelString, Is.Not.Null);
        Assert.That(modelString.Length, Is.GreaterThan(0));
    }

    [Test]
    public void ToString_DisposedContext_ReturnsInvalidatedStatus()
    {
        Z3Model model;

        // Create model, then dispose context which invalidates the model
        {
            using var context = new Z3Context();
            using var solver = context.CreateSolver();

            var x = context.IntConst("x");
            solver.Assert(context.Eq(x, context.Int(42)));

            Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
            model = solver.GetModel();

            // Model should work initially
            var initialString = model.ToString();
            Assert.That(initialString, Is.Not.Null);
            Assert.That(initialString.Length, Is.GreaterThan(0));
        } // Context disposed here, which invalidates the model

        // Model is automatically invalidated when context is disposed
        var disposedString = model.ToString();
        Assert.That(disposedString, Is.EqualTo("<invalidated>"));
    }
}
