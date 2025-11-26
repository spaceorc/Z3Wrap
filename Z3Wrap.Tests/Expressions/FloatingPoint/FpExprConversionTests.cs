using NUnit.Framework;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.BitVectors;
using Spaceorc.Z3Wrap.Expressions.FloatingPoint;
using Spaceorc.Z3Wrap.Expressions.Numerics;
using Spaceorc.Z3Wrap.Values.BitVectors;

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

    [Test]
    public void FpFromSignedBv_PositiveValue_ConvertsCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Bit-vector value 42 (signed)
        var bv = context.Bv<Size32>(42);
        var rm = context.RoundingMode(RoundingMode.NearestTiesToEven);

        var result = context.FpFromSignedBv<Float32>(bv, rm);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetFloatValue(result), Is.EqualTo(42.0f));
    }

    [Test]
    public void FpFromSignedBv_NegativeValue_ConvertsCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Bit-vector value -10 (signed 32-bit)
        var bv = context.Bv<Size32>(-10);
        var rm = context.RoundingMode(RoundingMode.NearestTiesToEven);

        var result = context.FpFromSignedBv<Float32>(bv, rm);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetFloatValue(result), Is.EqualTo(-10.0f));
    }

    [Test]
    public void FpFromSignedBv_ToFloat64_ConvertsCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var bv = context.Bv<Size64>(1234567890L);
        var rm = context.RoundingMode(RoundingMode.NearestTiesToEven);

        var result = context.FpFromSignedBv<Float64>(bv, rm);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetDoubleValue(result), Is.EqualTo(1234567890.0));
    }

    [Test]
    public void FpFromUnsignedBv_PositiveValue_ConvertsCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Unsigned bit-vector value 255
        var bv = context.Bv<Size32>(255);
        var rm = context.RoundingMode(RoundingMode.NearestTiesToEven);

        var result = context.FpFromUnsignedBv<Float32>(bv, rm);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetFloatValue(result), Is.EqualTo(255.0f));
    }

    [Test]
    public void FpFromUnsignedBv_LargeValue_ConvertsCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Large unsigned value that would be negative if interpreted as signed
        var bv = context.Bv<Size32>(0xFFFFFFFF); // 4294967295 unsigned
        var rm = context.RoundingMode(RoundingMode.NearestTiesToEven);

        var result = context.FpFromUnsignedBv<Float32>(bv, rm);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetFloatValue(result), Is.EqualTo(4294967295.0f).Within(1.0f));
    }

    [Test]
    public void FpFromUnsignedBv_ToFloat64_ConvertsCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var bv = context.Bv<Size64>(9876543210UL);
        var rm = context.RoundingMode(RoundingMode.NearestTiesToEven);

        var result = context.FpFromUnsignedBv<Float64>(bv, rm);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetDoubleValue(result), Is.EqualTo(9876543210.0));
    }

    [Test]
    public void FpFromSignedBv_Vs_FpFromUnsignedBv_DifferentResults()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Value that is negative when signed, positive when unsigned
        var bv = context.Bv<Size32>(0xFFFFFFFF);
        var rm = context.RoundingMode(RoundingMode.NearestTiesToEven);

        var signed = context.FpFromSignedBv<Float32>(bv, rm);
        var unsigned = context.FpFromUnsignedBv<Float32>(bv, rm);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var signedVal = model.GetFloatValue(signed);
        var unsignedVal = model.GetFloatValue(unsigned);

        // Signed: -1, Unsigned: 4294967295
        Assert.That(signedVal, Is.EqualTo(-1.0f));
        Assert.That(unsignedVal, Is.EqualTo(4294967295.0f).Within(1.0f));
        Assert.That(signedVal, Is.Not.EqualTo(unsignedVal));
    }
}
