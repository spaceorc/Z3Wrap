using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Expressions.Numerics;
using Spaceorc.Z3Wrap.Values.Numerics;

namespace Z3Wrap.Tests.Expressions.Common;

[TestFixture]
public class ArithmeticFunctionsTests
{
    [Test]
    public void Abs_IntValue_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Int(-42);

        var result = context.Abs(a);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetIntValue(result), Is.EqualTo(new BigInteger(42)));
    }

    [Test]
    public void Abs_RealValue_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Real(new Real(-7, 2)); // -7/2 = -3.5

        var result = context.Abs(a);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var realValue = model.GetRealValue(result);
        Assert.That(realValue.Numerator, Is.EqualTo(new BigInteger(7)));
        Assert.That(realValue.Denominator, Is.EqualTo(new BigInteger(2)));
    }

    [Test]
    public void Min_TwoInts_ReturnsMinimum()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Int(10);
        var b = context.Int(20);

        var result = context.Min(a, b);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetIntValue(result), Is.EqualTo(new BigInteger(10)));
    }

    [Test]
    public void Max_TwoReals_ReturnsMaximum()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Real(new Real(3, 2)); // 3/2 = 1.5
        var b = context.Real(new Real(5, 4)); // 5/4 = 1.25

        var result = context.Max(a, b);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var realValue = model.GetRealValue(result);
        Assert.That(realValue.Numerator, Is.EqualTo(new BigInteger(3)));
        Assert.That(realValue.Denominator, Is.EqualTo(new BigInteger(2)));
    }

    [Test]
    public void Power_IntSquare_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Int(5);
        var exp = context.Int(2);

        var resultViaContext = context.Power(a, exp);
        var resultViaFunc = a.Power(exp);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            var realValue1 = model.GetRealValue(resultViaContext);
            // 5^2 = 25/1
            Assert.That(realValue1.Numerator, Is.EqualTo(new BigInteger(25)));
            Assert.That(realValue1.Denominator, Is.EqualTo(new BigInteger(1)));

            var realValue2 = model.GetRealValue(resultViaFunc);
            Assert.That(realValue2.Numerator, Is.EqualTo(new BigInteger(25)));
            Assert.That(realValue2.Denominator, Is.EqualTo(new BigInteger(1)));
        });
    }

    [Test]
    public void Power_IntCube_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Int(3);
        var exp = context.Int(3);

        var resultViaContext = context.Power(a, exp);
        var resultViaFunc = a.Power(exp);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            var realValue1 = model.GetRealValue(resultViaContext);
            // 3^3 = 27/1
            Assert.That(realValue1.Numerator, Is.EqualTo(new BigInteger(27)));
            Assert.That(realValue1.Denominator, Is.EqualTo(new BigInteger(1)));

            var realValue2 = model.GetRealValue(resultViaFunc);
            Assert.That(realValue2.Numerator, Is.EqualTo(new BigInteger(27)));
            Assert.That(realValue2.Denominator, Is.EqualTo(new BigInteger(1)));
        });
    }

    [Test]
    public void Power_RealSquare_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Real(new Real(3, 2)); // 3/2
        var exp = context.Real(2); // 2

        var resultViaContext = context.Power(a, exp);
        var resultViaFunc = a.Power(exp);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            var realValue1 = model.GetRealValue(resultViaContext);
            // (3/2)^2 = 9/4
            Assert.That(realValue1.Numerator, Is.EqualTo(new BigInteger(9)));
            Assert.That(realValue1.Denominator, Is.EqualTo(new BigInteger(4)));

            var realValue2 = model.GetRealValue(resultViaFunc);
            Assert.That(realValue2.Numerator, Is.EqualTo(new BigInteger(9)));
            Assert.That(realValue2.Denominator, Is.EqualTo(new BigInteger(4)));
        });
    }

    [Test]
    public void Power_SquareRoot_CanSolveSimpleCase()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var two = context.Real(2);
        var half = context.Real(new Real(1, 2)); // 1/2

        // Compute 2^(1/2) = sqrt(2)
        // Z3 can actually handle this simple case and returns Satisfiable!
        var sqrtTwo = context.Power(two, half);

        var status = solver.Check();

        // Z3 returns Satisfiable for this case
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        // Note: Z3 represents sqrt(2) symbolically, not as a decimal approximation
        // The actual value is an algebraic number that Z3 can reason about
    }

    [Test]
    public void Power_VariableExponent_CanSolveWithSimplification()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var y = context.IntConst("y");

        // x^y where both are variables - nonlinear
        var result = context.Power(x, y);

        // Add some constraints that Z3 can simplify
        solver.Assert(x > 0);
        solver.Assert(y > 0);
        solver.Assert(result == context.Real(8));
        solver.Assert(x == 2);

        // Z3 can solve this because x=2 is fixed, so it simplifies to 2^y=8
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var yValue = model.GetIntValue(y);
        Assert.That(yValue, Is.EqualTo(new BigInteger(3)));
    }
}
