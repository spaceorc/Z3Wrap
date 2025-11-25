using NUnit.Framework;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.FloatingPoint;
using Spaceorc.Z3Wrap.Expressions.Numerics;

namespace Z3Wrap.Tests.Expressions.FloatingPoint;

[TestFixture]
public class FpExprConversionTests
{
    [Test]
    public void ToFloat32_FromFloat64_ConvertsCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var value = context.Fp(3.14159);
        var rm = context.RoundingMode(RoundingMode.NearestTiesToEven);

        var result = value.ToFloat32(rm);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetFloatValue(result), Is.EqualTo(3.14159f).Within(0.0001f));
    }

    [Test]
    public void ToFloat64_FromFloat32_ConvertsCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var value = context.Fp(3.14f);
        var rm = context.RoundingMode(RoundingMode.NearestTiesToEven);

        var result = value.ToFloat64(rm);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetDoubleValue(result), Is.EqualTo(3.14f).Within(0.01));
    }

    [Test]
    public void ToFloat16_FromFloat32_ConvertsCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var value = context.Fp(3.14f);
        var rm = context.RoundingMode(RoundingMode.NearestTiesToEven);

        var result = value.ToFloat16(rm);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That((float)model.GetHalfValue(result), Is.EqualTo(3.14f).Within(0.01f));
    }

    [Test]
    public void ToFormat_Float32ToFloat64_ConvertsCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var value = context.Fp(2.5f);
        var rm = context.RoundingMode(RoundingMode.NearestTiesToEven);

        var result = value.ToFormat<Float32, Float64>(rm);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetDoubleValue(result), Is.EqualTo(2.5));
    }

    [Test]
    public void ToReal_FromFloat32_ConvertsCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var value = context.Fp(3.5f);

        var result = value.ToReal();

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var realValue = model.GetRealValue(result);
        // 3.5 = 7/2
        Assert.That(realValue.Numerator, Is.EqualTo(new System.Numerics.BigInteger(7)));
        Assert.That(realValue.Denominator, Is.EqualTo(new System.Numerics.BigInteger(2)));
    }

    [Test]
    public void FpFromReal_Float32_ConvertsCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // 7/2 = 3.5
        var realValue = context.Real(new Spaceorc.Z3Wrap.Values.Numerics.Real(7, 2));
        var rm = context.RoundingMode(RoundingMode.NearestTiesToEven);

        var result = context.FpFromReal<Float32>(realValue, rm);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetFloatValue(result), Is.EqualTo(3.5f));
    }

    [Test]
    public void FpFromReal_Float64_ConvertsCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // 11/4 = 2.75
        var realValue = context.Real(new Spaceorc.Z3Wrap.Values.Numerics.Real(11, 4));
        var rm = context.RoundingMode(RoundingMode.NearestTiesToEven);

        var result = context.FpFromReal<Float64>(realValue, rm);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetDoubleValue(result), Is.EqualTo(2.75));
    }

    [Test]
    public void ToFloat32_WithTowardZero_ConvertsCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var value = context.Fp(3.14159);
        var rm = context.RoundingMode(RoundingMode.TowardZero);

        var result = value.ToFloat32(rm);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetFloatValue(result), Is.EqualTo(3.14159f).Within(0.0001f));
    }

    [Test]
    public void ToFloat64_WithTowardPositive_ConvertsCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var value = context.Fp(2.5f);
        var rm = context.RoundingMode(RoundingMode.TowardPositive);

        var result = value.ToFloat64(rm);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetDoubleValue(result), Is.EqualTo(2.5));
    }
}
