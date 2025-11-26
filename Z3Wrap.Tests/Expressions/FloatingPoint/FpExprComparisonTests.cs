using NUnit.Framework;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.FloatingPoint;

namespace Z3Wrap.Tests.Expressions.FloatingPoint;

[TestFixture]
public class FpExprComparisonTests
{
    [TestCase(2.5f, 5.0f, true)]
    [TestCase(5.0f, 2.5f, false)]
    [TestCase(3.0f, 3.0f, false)]
    public void LessThan_Float32Values_ComputesCorrectResult(float aValue, float bValue, bool expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Fp<Float32>(aValue);
        var b = context.Fp<Float32>(bValue);

        var result = a < b;

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(result), Is.EqualTo(expected));
    }

    [TestCase(2.5f, 5.0f, true)]
    [TestCase(5.0f, 2.5f, false)]
    [TestCase(3.0f, 3.0f, true)]
    public void LessThanOrEqual_Float32Values_ComputesCorrectResult(float aValue, float bValue, bool expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Fp<Float32>(aValue);
        var b = context.Fp<Float32>(bValue);

        var result = a <= b;

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(result), Is.EqualTo(expected));
    }

    [TestCase(5.0f, 2.5f, true)]
    [TestCase(2.5f, 5.0f, false)]
    [TestCase(3.0f, 3.0f, false)]
    public void GreaterThan_Float32Values_ComputesCorrectResult(float aValue, float bValue, bool expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Fp<Float32>(aValue);
        var b = context.Fp<Float32>(bValue);

        var result = a > b;

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(result), Is.EqualTo(expected));
    }

    [TestCase(5.0f, 2.5f, true)]
    [TestCase(2.5f, 5.0f, false)]
    [TestCase(3.0f, 3.0f, true)]
    public void GreaterThanOrEqual_Float32Values_ComputesCorrectResult(float aValue, float bValue, bool expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Fp<Float32>(aValue);
        var b = context.Fp<Float32>(bValue);

        var result = a >= b;

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(result), Is.EqualTo(expected));
    }

    [TestCase(3.0f, 3.0f, true)]
    [TestCase(3.0f, 5.0f, false)]
    [TestCase(5.0f, 3.0f, false)]
    public void Equality_Float32Values_ComputesCorrectResult(float aValue, float bValue, bool expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Fp<Float32>(aValue);
        var b = context.Fp<Float32>(bValue);

        var result = a == b;

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(result), Is.EqualTo(expected));
    }

    [TestCase(3.0f, 3.0f, false)]
    [TestCase(3.0f, 5.0f, true)]
    [TestCase(5.0f, 3.0f, true)]
    public void Inequality_Float32Values_ComputesCorrectResult(float aValue, float bValue, bool expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Fp<Float32>(aValue);
        var b = context.Fp<Float32>(bValue);

        var result = a != b;

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(result), Is.EqualTo(expected));
    }

    [Test]
    public void LessThan_Float64Values_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Fp<Float64>(2.5);
        var b = context.Fp<Float64>(5.0);

        var result = a < b;

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(result), Is.True);
    }

    [Test]
    public void Equality_Float64Values_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Fp<Float64>(3.14159);
        var b = context.Fp<Float64>(3.14159);

        var result = a == b;

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(result), Is.True);
    }

    [Test]
    public void LessThan_Float16Values_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Fp<Float16>((double)(Half)2.5f);
        var b = context.Fp<Float16>((double)(Half)5.0f);

        var result = a < b;

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(result), Is.True);
    }

    [Test]
    public void Equality_Float16Values_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Fp<Float16>((double)(Half)3.14f);
        var b = context.Fp<Float16>((double)(Half)3.14f);

        var result = a == b;

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(result), Is.True);
    }
}
