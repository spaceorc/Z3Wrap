using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.FloatingPoint;

namespace Z3Wrap.Tests.Expressions.FloatingPoint;

[TestFixture]
public class FpExprCreationTests
{
    [Test]
    public void FpConst_Float16_CreatesConstant()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.FpConst<Float16>("x");

        Assert.That(x, Is.Not.Null);
        Assert.That(x.Context, Is.SameAs(context));
    }

    [Test]
    public void FpConst_Float32_CreatesConstant()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.FpConst<Float32>("x");

        Assert.That(x, Is.Not.Null);
        Assert.That(x.Context, Is.SameAs(context));
    }

    [Test]
    public void FpConst_Float64_CreatesConstant()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.FpConst<Float64>("x");

        Assert.That(x, Is.Not.Null);
        Assert.That(x.Context, Is.SameAs(context));
    }

    [Test]
    public void Fp_FromHalf_CreatesValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var value = (Half)3.14f;
        var expr = context.Fp(value);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var result = model.GetHalfValue(expr);

        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void Fp_FromFloat_CreatesValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var value = 3.14f;
        var expr = context.Fp(value);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var result = model.GetFloatValue(expr);

        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void Fp_FromDouble_CreatesValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var value = 3.14159265358979;
        var expr = context.Fp(value);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var result = model.GetDoubleValue(expr);

        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void Fp_WithExplicitFormat_Float16_CreatesValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var expr = context.Fp<Float16>(2.5);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var result = model.GetHalfValue(expr);

        Assert.That(result, Is.EqualTo((Half)2.5));
    }

    [Test]
    public void Fp_WithExplicitFormat_Float32_CreatesValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var expr = context.Fp<Float32>(2.5);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var result = model.GetFloatValue(expr);

        Assert.That(result, Is.EqualTo(2.5f));
    }

    [Test]
    public void Fp_WithExplicitFormat_Float64_CreatesValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var expr = context.Fp<Float64>(2.5);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var result = model.GetDoubleValue(expr);

        Assert.That(result, Is.EqualTo(2.5));
    }

    [Test]
    public void FpNaN_Float16_CreatesNaN()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var nan = context.FpNaN<Float16>();
        solver.Assert(nan.IsNaN());

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var result = model.GetHalfValue(nan);

        Assert.That(Half.IsNaN(result), Is.True);
    }

    [Test]
    public void FpNaN_Float32_CreatesNaN()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var nan = context.FpNaN<Float32>();
        solver.Assert(nan.IsNaN());

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var result = model.GetFloatValue(nan);

        Assert.That(float.IsNaN(result), Is.True);
    }

    [Test]
    public void FpNaN_Float64_CreatesNaN()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var nan = context.FpNaN<Float64>();
        solver.Assert(nan.IsNaN());

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var result = model.GetDoubleValue(nan);

        Assert.That(double.IsNaN(result), Is.True);
    }

    [Test]
    public void FpInfinity_Float16_PositiveInfinity()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var inf = context.FpInfinity<Float16>(negative: false);
        solver.Assert(inf.IsInfinite());

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var result = model.GetHalfValue(inf);

        Assert.That(Half.IsPositiveInfinity(result), Is.True);
    }

    [Test]
    public void FpInfinity_Float16_NegativeInfinity()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var inf = context.FpInfinity<Float16>(negative: true);
        solver.Assert(inf.IsInfinite());

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var result = model.GetHalfValue(inf);

        Assert.That(Half.IsNegativeInfinity(result), Is.True);
    }

    [Test]
    public void FpInfinity_Float32_PositiveInfinity()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var inf = context.FpInfinity<Float32>(negative: false);
        solver.Assert(inf.IsInfinite());

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var result = model.GetFloatValue(inf);

        Assert.That(float.IsPositiveInfinity(result), Is.True);
    }

    [Test]
    public void FpInfinity_Float32_NegativeInfinity()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var inf = context.FpInfinity<Float32>(negative: true);
        solver.Assert(inf.IsInfinite());

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var result = model.GetFloatValue(inf);

        Assert.That(float.IsNegativeInfinity(result), Is.True);
    }

    [Test]
    public void FpInfinity_Float64_PositiveInfinity()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var inf = context.FpInfinity<Float64>(negative: false);
        solver.Assert(inf.IsInfinite());

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var result = model.GetDoubleValue(inf);

        Assert.That(double.IsPositiveInfinity(result), Is.True);
    }

    [Test]
    public void FpInfinity_Float64_NegativeInfinity()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var inf = context.FpInfinity<Float64>(negative: true);
        solver.Assert(inf.IsInfinite());

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var result = model.GetDoubleValue(inf);

        Assert.That(double.IsNegativeInfinity(result), Is.True);
    }

    [Test]
    public void FpZero_Float16_PositiveZero()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var zero = context.FpZero<Float16>(negative: false);
        solver.Assert(zero.IsZero());

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var result = model.GetHalfValue(zero);

        Assert.That(result, Is.EqualTo((Half)0.0));
    }

    [Test]
    public void FpZero_Float16_NegativeZero()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var zero = context.FpZero<Float16>(negative: true);
        solver.Assert(zero.IsZero());
        solver.Assert(zero.IsNegative());

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var result = model.GetHalfValue(zero);

        // Check for negative zero
        Assert.That(result, Is.EqualTo((Half)0.0));
        Assert.That(Half.IsNegative(result), Is.True);
    }

    [Test]
    public void FpZero_Float32_PositiveZero()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var zero = context.FpZero<Float32>(negative: false);
        solver.Assert(zero.IsZero());

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var result = model.GetFloatValue(zero);

        Assert.That(result, Is.EqualTo(0.0f));
    }

    [Test]
    public void FpZero_Float32_NegativeZero()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var zero = context.FpZero<Float32>(negative: true);
        solver.Assert(zero.IsZero());
        solver.Assert(zero.IsNegative());

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var result = model.GetFloatValue(zero);

        // Check for negative zero
        Assert.That(result, Is.EqualTo(0.0f));
        Assert.That(float.IsNegative(result), Is.True);
    }

    [Test]
    public void FpZero_Float64_PositiveZero()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var zero = context.FpZero<Float64>(negative: false);
        solver.Assert(zero.IsZero());

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var result = model.GetDoubleValue(zero);

        Assert.That(result, Is.EqualTo(0.0));
    }

    [Test]
    public void FpZero_Float64_NegativeZero()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var zero = context.FpZero<Float64>(negative: true);
        solver.Assert(zero.IsZero());
        solver.Assert(zero.IsNegative());

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var result = model.GetDoubleValue(zero);

        // Check for negative zero
        Assert.That(result, Is.EqualTo(0.0));
        Assert.That(double.IsNegative(result), Is.True);
    }

    [Test]
    public void RoundingMode_NearestTiesToEven_CreatesMode()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var rm = context.RoundingMode(RoundingMode.NearestTiesToEven);

        Assert.That(rm, Is.Not.Null);
        Assert.That(rm.Context, Is.SameAs(context));
    }

    [Test]
    public void RoundingMode_NearestTiesToAway_CreatesMode()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var rm = context.RoundingMode(RoundingMode.NearestTiesToAway);

        Assert.That(rm, Is.Not.Null);
        Assert.That(rm.Context, Is.SameAs(context));
    }

    [Test]
    public void RoundingMode_TowardPositive_CreatesMode()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var rm = context.RoundingMode(RoundingMode.TowardPositive);

        Assert.That(rm, Is.Not.Null);
        Assert.That(rm.Context, Is.SameAs(context));
    }

    [Test]
    public void RoundingMode_TowardNegative_CreatesMode()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var rm = context.RoundingMode(RoundingMode.TowardNegative);

        Assert.That(rm, Is.Not.Null);
        Assert.That(rm.Context, Is.SameAs(context));
    }

    [Test]
    public void RoundingMode_TowardZero_CreatesMode()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var rm = context.RoundingMode(RoundingMode.TowardZero);

        Assert.That(rm, Is.Not.Null);
        Assert.That(rm.Context, Is.SameAs(context));
    }

    [Test]
    public void GetFpComponents_ExtractsCorrectComponents()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var expr = context.Fp(3.14f);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var (sign, exponent, significand) = model.GetFpComponents(expr);

        // Verify components match IEEE 754 representation of 3.14f
        var bits = BitConverter.SingleToUInt32Bits(3.14f);
        var expectedSign = (bits & 0x80000000) != 0;
        var expectedExp = (ulong)((bits >> 23) & 0xFF);
        var expectedSig = (ulong)(bits & 0x7FFFFF);

        Assert.Multiple(() =>
        {
            Assert.That(sign, Is.EqualTo(expectedSign));
            Assert.That(exponent, Is.EqualTo(expectedExp));
            Assert.That(significand, Is.EqualTo(expectedSig));
        });
    }

    [Test]
    public void FpFromComponents_Float32_PositiveValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Components for 3.14f
        var value = 3.14f;
        var bits = BitConverter.SingleToUInt32Bits(value);
        var sign = (bits & 0x80000000) != 0;
        var exp = (ulong)((bits >> 23) & 0xFF);
        var sig = (ulong)(bits & 0x7FFFFF);

        var expr = context.FpFromComponents<Float32>(sign, exp, sig);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var result = model.GetFloatValue(expr);

        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void FpFromComponents_Float32_NegativeValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Components for -3.14f
        var value = -3.14f;
        var bits = BitConverter.SingleToUInt32Bits(value);
        var sign = (bits & 0x80000000) != 0;
        var exp = (ulong)((bits >> 23) & 0xFF);
        var sig = (ulong)(bits & 0x7FFFFF);

        var expr = context.FpFromComponents<Float32>(sign, exp, sig);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var result = model.GetFloatValue(expr);

        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void FpFromComponents_Float64_PositiveValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Components for 3.14159
        var value = 3.14159;
        var bits = BitConverter.DoubleToUInt64Bits(value);
        var sign = (bits & 0x8000000000000000) != 0;
        var exp = (bits >> 52) & 0x7FF;
        var sig = bits & 0xFFFFFFFFFFFFF;

        var expr = context.FpFromComponents<Float64>(sign, exp, sig);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var result = model.GetDoubleValue(expr);

        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void FpFromComponents_Float16_PositiveValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Components for (Half)2.5
        var value = (Half)2.5;
        var bits = BitConverter.HalfToUInt16Bits(value);
        var sign = (bits & 0x8000) != 0;
        var exp = (ulong)((bits >> 10) & 0x1F);
        var sig = (ulong)(bits & 0x3FF);

        var expr = context.FpFromComponents<Float16>(sign, exp, sig);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var result = model.GetHalfValue(expr);

        Assert.That(result, Is.EqualTo(value));
    }

    [Test]
    public void FpFromComponents_RoundTrip_Float32()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var original = context.Fp(6.28f);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var (sign, exp, sig) = model.GetFpComponents(original);

        // Reconstruct from components
        var reconstructed = context.FpFromComponents<Float32>(sign, exp, sig);

        // Create new solver for reconstructed value
        using var solver2 = context.CreateSolver();
        Assert.That(solver2.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model2 = solver2.GetModel();
        var originalValue = model.GetFloatValue(original);
        var reconstructedValue = model2.GetFloatValue(reconstructed);

        Assert.That(reconstructedValue, Is.EqualTo(originalValue));
    }

    [Test]
    public void FpFromComponents_Zero_Positive()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Components for +0.0
        var expr = context.FpFromComponents<Float32>(sign: false, exponent: 0, significand: 0);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var result = model.GetFloatValue(expr);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo(0.0f));
            Assert.That(float.IsNegative(result), Is.False);
        });
    }

    [Test]
    public void FpFromComponents_Zero_Negative()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Components for -0.0
        var expr = context.FpFromComponents<Float32>(sign: true, exponent: 0, significand: 0);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var result = model.GetFloatValue(expr);

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.EqualTo(0.0f));
            Assert.That(float.IsNegative(result), Is.True);
        });
    }

    [Test]
    public void FpFromComponents_Infinity_Positive()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Components for +∞ (exponent = all 1s, significand = 0)
        var expr = context.FpFromComponents<Float32>(sign: false, exponent: 0xFF, significand: 0);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var result = model.GetFloatValue(expr);

        Assert.That(float.IsPositiveInfinity(result), Is.True);
    }

    [Test]
    public void FpFromComponents_Infinity_Negative()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Components for -∞ (exponent = all 1s, significand = 0)
        var expr = context.FpFromComponents<Float32>(sign: true, exponent: 0xFF, significand: 0);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var result = model.GetFloatValue(expr);

        Assert.That(float.IsNegativeInfinity(result), Is.True);
    }

    [Test]
    public void FpFromComponents_NaN()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Components for NaN (exponent = all 1s, significand ≠ 0)
        var expr = context.FpFromComponents<Float32>(sign: false, exponent: 0xFF, significand: 1);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var result = model.GetFloatValue(expr);

        Assert.That(float.IsNaN(result), Is.True);
    }
}
