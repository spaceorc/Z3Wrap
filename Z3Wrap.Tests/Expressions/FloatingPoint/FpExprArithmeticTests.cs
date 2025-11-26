using NUnit.Framework;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.FloatingPoint;

namespace Z3Wrap.Tests.Expressions.FloatingPoint;

[TestFixture]
public class FpExprArithmeticTests
{
    [Test]
    public void Add_TwoFloat32Values_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Fp<Float32>(2.5f);
        var b = context.Fp<Float32>(3.75f);
        var rm = context.RoundingMode(RoundingMode.NearestTiesToEven);

        var result = a + b;

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetFloatValue(result), Is.EqualTo(6.25f));
    }

    [Test]
    public void Add_WithExplicitRoundingMode_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Fp<Float32>(2.5f);
        var b = context.Fp<Float32>(3.75f);
        var rm = context.RoundingMode(RoundingMode.TowardZero);

        var result = a.Add(b, rm);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetFloatValue(result), Is.EqualTo(6.25f));
    }

    [Test]
    public void Sub_TwoFloat32Values_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Fp<Float32>(10.5f);
        var b = context.Fp<Float32>(3.25f);

        var result = a - b;

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetFloatValue(result), Is.EqualTo(7.25f));
    }

    [Test]
    public void Sub_WithExplicitRoundingMode_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Fp<Float32>(10.5f);
        var b = context.Fp<Float32>(3.25f);
        var rm = context.RoundingMode(RoundingMode.NearestTiesToEven);

        var result = a.Sub(b, rm);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetFloatValue(result), Is.EqualTo(7.25f));
    }

    [Test]
    public void Mul_TwoFloat32Values_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Fp<Float32>(4.0f);
        var b = context.Fp<Float32>(2.5f);

        var result = a * b;

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetFloatValue(result), Is.EqualTo(10.0f));
    }

    [Test]
    public void Mul_WithExplicitRoundingMode_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Fp<Float32>(4.0f);
        var b = context.Fp<Float32>(2.5f);
        var rm = context.RoundingMode(RoundingMode.NearestTiesToEven);

        var result = a.Mul(b, rm);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetFloatValue(result), Is.EqualTo(10.0f));
    }

    [Test]
    public void Div_TwoFloat32Values_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Fp<Float32>(10.0f);
        var b = context.Fp<Float32>(4.0f);

        var result = a / b;

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetFloatValue(result), Is.EqualTo(2.5f));
    }

    [Test]
    public void Div_WithExplicitRoundingMode_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Fp<Float32>(10.0f);
        var b = context.Fp<Float32>(4.0f);
        var rm = context.RoundingMode(RoundingMode.NearestTiesToEven);

        var result = a.Div(b, rm);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetFloatValue(result), Is.EqualTo(2.5f));
    }

    [Test]
    public void Rem_TwoFloat32Values_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Fp<Float32>(5.5f);
        var b = context.Fp<Float32>(2.0f);

        var result = a % b;

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        // IEEE 754 remainder: 5.5 - 3*2.0 = -0.5 (rounds to nearest)
        Assert.That(model.GetFloatValue(result), Is.EqualTo(-0.5f));
    }

    [Test]
    public void Rem_WithMethod_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Fp<Float32>(5.5f);
        var b = context.Fp<Float32>(2.0f);

        var result = a.Rem(b);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        // IEEE 754 remainder: 5.5 - 3*2.0 = -0.5 (rounds to nearest)
        Assert.That(model.GetFloatValue(result), Is.EqualTo(-0.5f));
    }

    [Test]
    public void Neg_Float32Value_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Fp<Float32>(5.5f);

        var result = -a;

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetFloatValue(result), Is.EqualTo(-5.5f));
    }

    [Test]
    public void Neg_WithMethod_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Fp<Float32>(5.5f);

        var result = a.Neg();

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetFloatValue(result), Is.EqualTo(-5.5f));
    }

    [Test]
    public void Abs_PositiveFloat32Value_ReturnsPositive()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Fp<Float32>(5.5f);

        var result = a.Abs();

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetFloatValue(result), Is.EqualTo(5.5f));
    }

    [Test]
    public void Abs_NegativeFloat32Value_ReturnsPositive()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Fp<Float32>(-5.5f);

        var result = a.Abs();

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetFloatValue(result), Is.EqualTo(5.5f));
    }

    [Test]
    public void Sqrt_Float32Value_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Fp<Float32>(16.0f);
        var rm = context.RoundingMode(RoundingMode.NearestTiesToEven);

        var result = a.Sqrt(rm);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetFloatValue(result), Is.EqualTo(4.0f));
    }

    [Test]
    public void RoundToIntegral_Float32Value_RoundsCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Fp<Float32>(5.7f);
        var rm = context.RoundingMode(RoundingMode.NearestTiesToEven);

        var result = a.RoundToIntegral(rm);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetFloatValue(result), Is.EqualTo(6.0f));
    }

    [Test]
    public void RoundToIntegral_TowardZero_RoundsTowardZero()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Fp<Float32>(5.7f);
        var rm = context.RoundingMode(RoundingMode.TowardZero);

        var result = a.RoundToIntegral(rm);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetFloatValue(result), Is.EqualTo(5.0f));
    }

    [Test]
    public void Fma_Float32Values_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.Fp<Float32>(2.0f);
        var y = context.Fp<Float32>(3.0f);
        var z = context.Fp<Float32>(4.0f);
        var rm = context.RoundingMode(RoundingMode.NearestTiesToEven);

        // x * y + z = 2 * 3 + 4 = 10
        var result = x.Fma(y, z, rm);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetFloatValue(result), Is.EqualTo(10.0f));
    }

    [Test]
    public void Min_TwoFloat32Values_ReturnsMinimum()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Fp<Float32>(5.5f);
        var b = context.Fp<Float32>(3.2f);

        var result = a.Min(b);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetFloatValue(result), Is.EqualTo(3.2f));
    }

    [Test]
    public void Max_TwoFloat32Values_ReturnsMaximum()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Fp<Float32>(5.5f);
        var b = context.Fp<Float32>(3.2f);

        var result = a.Max(b);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetFloatValue(result), Is.EqualTo(5.5f));
    }

    [Test]
    public void Add_Float64Values_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Fp<Float64>(2.5);
        var b = context.Fp<Float64>(3.75);

        var result = a + b;

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetDoubleValue(result), Is.EqualTo(6.25));
    }

    [Test]
    public void Mul_Float64Values_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Fp<Float64>(4.0);
        var b = context.Fp<Float64>(2.5);

        var result = a * b;

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetDoubleValue(result), Is.EqualTo(10.0));
    }

    [Test]
    public void Add_Float16Values_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Fp<Float16>((double)(Half)2.5f);
        var b = context.Fp<Float16>((double)(Half)3.5f);

        var result = a + b;

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetHalfValue(result), Is.EqualTo((Half)6.0f));
    }
}
