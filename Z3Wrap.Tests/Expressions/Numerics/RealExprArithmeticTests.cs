using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Expressions.Numerics;
using Spaceorc.Z3Wrap.Values.Numerics;

namespace Z3Wrap.Tests.Expressions.Numerics;

[TestFixture]
public class RealExprArithmeticTests
{
    [Test]
    public void Add_TwoValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Real(10.5m);
        var b = context.Real(31.5m);

        var sum = a + b;
        var sumViaDecimalLeft = 10.5m + b;
        var sumViaDecimalRight = a + 31.5m;
        var sumViaContext = context.Add(a, b);
        var sumViaContextDecimalLeft = context.Add(10.5m, b);
        var sumViaContextDecimalRight = context.Add(a, 31.5m);
        var sumViaFunc = a.Add(b);
        var sumViaFuncDecimalRight = a.Add(31.5m);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetRealValue(sum).ToDecimal(), Is.EqualTo(42.0m));
            Assert.That(model.GetRealValue(sumViaDecimalLeft).ToDecimal(), Is.EqualTo(42.0m));
            Assert.That(model.GetRealValue(sumViaDecimalRight).ToDecimal(), Is.EqualTo(42.0m));
            Assert.That(model.GetRealValue(sumViaContext).ToDecimal(), Is.EqualTo(42.0m));
            Assert.That(model.GetRealValue(sumViaContextDecimalLeft).ToDecimal(), Is.EqualTo(42.0m));
            Assert.That(model.GetRealValue(sumViaContextDecimalRight).ToDecimal(), Is.EqualTo(42.0m));
            Assert.That(model.GetRealValue(sumViaFunc).ToDecimal(), Is.EqualTo(42.0m));
            Assert.That(model.GetRealValue(sumViaFuncDecimalRight).ToDecimal(), Is.EqualTo(42.0m));
        });
    }

    [Test]
    public void Add_SingleValue_ReturnsSameValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Real(42.0m);
        var sum = context.Add(a);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetRealValue(sum).ToDecimal(), Is.EqualTo(42.0m));
    }

    [Test]
    public void Add_ThreeValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Real(10.5m);
        var b = context.Real(20.0m);
        var c = context.Real(11.5m);
        var sumViaContext = context.Add(a, b, c);
        var sumViaExpr = a.Add(b, c);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetRealValue(sumViaContext).ToDecimal(), Is.EqualTo(42.0m));
            Assert.That(model.GetRealValue(sumViaExpr).ToDecimal(), Is.EqualTo(42.0m));
        });
    }

    [Test]
    public void Add_FourValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Real(5.5m);
        var b = context.Real(10.0m);
        var c = context.Real(15.5m);
        var d = context.Real(11.0m);
        var sumViaContext = context.Add(a, b, c, d);
        var sumViaExpr = a.Add(b, c, d);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetRealValue(sumViaContext).ToDecimal(), Is.EqualTo(42.0m));
            Assert.That(model.GetRealValue(sumViaExpr).ToDecimal(), Is.EqualTo(42.0m));
        });
    }

    [Test]
    public void Subtract_TwoValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Real(50.5m);
        var b = context.Real(8.5m);

        var difference = a - b;
        var differenceViaDecimalLeft = 50.5m - b;
        var differenceViaDecimalRight = a - 8.5m;
        var differenceViaContext = context.Sub(a, b);
        var differenceViaContextDecimalLeft = context.Sub(50.5m, b);
        var differenceViaContextDecimalRight = context.Sub(a, 8.5m);
        var differenceViaFunc = a.Sub(b);
        var differenceViaFuncDecimalRight = a.Sub(8.5m);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetRealValue(difference).ToDecimal(), Is.EqualTo(42.0m));
            Assert.That(model.GetRealValue(differenceViaDecimalLeft).ToDecimal(), Is.EqualTo(42.0m));
            Assert.That(model.GetRealValue(differenceViaDecimalRight).ToDecimal(), Is.EqualTo(42.0m));
            Assert.That(model.GetRealValue(differenceViaContext).ToDecimal(), Is.EqualTo(42.0m));
            Assert.That(model.GetRealValue(differenceViaContextDecimalLeft).ToDecimal(), Is.EqualTo(42.0m));
            Assert.That(model.GetRealValue(differenceViaContextDecimalRight).ToDecimal(), Is.EqualTo(42.0m));
            Assert.That(model.GetRealValue(differenceViaFunc).ToDecimal(), Is.EqualTo(42.0m));
            Assert.That(model.GetRealValue(differenceViaFuncDecimalRight).ToDecimal(), Is.EqualTo(42.0m));
        });
    }

    [Test]
    public void Subtract_SingleValue_ReturnsSameValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Real(42.0m);
        var difference = context.Sub(a);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetRealValue(difference).ToDecimal(), Is.EqualTo(42.0m));
    }

    [Test]
    public void Subtract_ThreeValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Real(100.0m);
        var b = context.Real(30.5m);
        var c = context.Real(27.5m);
        var differenceViaContext = context.Sub(a, b, c);
        var differenceViaExpr = a.Sub(b, c);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetRealValue(differenceViaContext).ToDecimal(), Is.EqualTo(42.0m));
            Assert.That(model.GetRealValue(differenceViaExpr).ToDecimal(), Is.EqualTo(42.0m));
        });
    }

    [Test]
    public void Subtract_FourValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Real(200.0m);
        var b = context.Real(100.0m);
        var c = context.Real(50.5m);
        var d = context.Real(7.5m);
        var differenceViaContext = context.Sub(a, b, c, d);
        var differenceViaExpr = a.Sub(b, c, d);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetRealValue(differenceViaContext).ToDecimal(), Is.EqualTo(42.0m));
            Assert.That(model.GetRealValue(differenceViaExpr).ToDecimal(), Is.EqualTo(42.0m));
        });
    }

    [Test]
    public void Multiply_TwoValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Real(6.0m);
        var b = context.Real(7.0m);

        var product = a * b;
        var productViaDecimalLeft = 6.0m * b;
        var productViaDecimalRight = a * 7.0m;
        var productViaContext = context.Mul(a, b);
        var productViaContextDecimalLeft = context.Mul(6.0m, b);
        var productViaContextDecimalRight = context.Mul(a, 7.0m);
        var productViaFunc = a.Mul(b);
        var productViaFuncDecimalRight = a.Mul(7.0m);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetRealValue(product).ToDecimal(), Is.EqualTo(42.0m));
            Assert.That(model.GetRealValue(productViaDecimalLeft).ToDecimal(), Is.EqualTo(42.0m));
            Assert.That(model.GetRealValue(productViaDecimalRight).ToDecimal(), Is.EqualTo(42.0m));
            Assert.That(model.GetRealValue(productViaContext).ToDecimal(), Is.EqualTo(42.0m));
            Assert.That(model.GetRealValue(productViaContextDecimalLeft).ToDecimal(), Is.EqualTo(42.0m));
            Assert.That(model.GetRealValue(productViaContextDecimalRight).ToDecimal(), Is.EqualTo(42.0m));
            Assert.That(model.GetRealValue(productViaFunc).ToDecimal(), Is.EqualTo(42.0m));
            Assert.That(model.GetRealValue(productViaFuncDecimalRight).ToDecimal(), Is.EqualTo(42.0m));
        });
    }

    [Test]
    public void Multiply_SingleValue_ReturnsSameValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Real(42.0m);
        var product = context.Mul(a);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetRealValue(product).ToDecimal(), Is.EqualTo(42.0m));
    }

    [Test]
    public void Multiply_ThreeValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Real(2.0m);
        var b = context.Real(3.0m);
        var c = context.Real(7.0m);
        var productViaContext = context.Mul(a, b, c);
        var productViaExpr = a.Mul(b, c);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetRealValue(productViaContext).ToDecimal(), Is.EqualTo(42.0m));
            Assert.That(model.GetRealValue(productViaExpr).ToDecimal(), Is.EqualTo(42.0m));
        });
    }

    [Test]
    public void Multiply_FourValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Real(2.0m);
        var b = context.Real(3.0m);
        var c = context.Real(7.0m);
        var d = context.Real(1.0m);
        var productViaContext = context.Mul(a, b, c, d);
        var productViaExpr = a.Mul(b, c, d);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetRealValue(productViaContext).ToDecimal(), Is.EqualTo(42.0m));
            Assert.That(model.GetRealValue(productViaExpr).ToDecimal(), Is.EqualTo(42.0m));
        });
    }

    [Test]
    public void Add_ZeroArguments_ReturnsZero()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var result = context.Add<RealExpr>();

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetRealValue(result), Is.EqualTo(Real.Zero));
    }

    [Test]
    public void Subtract_ZeroArguments_ThrowsException()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        Assert.Throws<InvalidOperationException>(() => context.Sub<RealExpr>());
    }

    [Test]
    public void Multiply_ZeroArguments_ReturnsOne()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var result = context.Mul<RealExpr>();

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetRealValue(result), Is.EqualTo(Real.One));
    }

    [Test]
    public void Divide_TwoValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Real(84.0m);
        var b = context.Real(2.0m);

        var quotient = a / b;
        var quotientViaDecimalLeft = 84.0m / b;
        var quotientViaDecimalRight = a / 2.0m;
        var quotientViaContext = context.Div(a, b);
        var quotientViaContextDecimalLeft = context.Div(84.0m, b);
        var quotientViaContextDecimalRight = context.Div(a, 2.0m);
        var quotientViaFunc = a.Div(b);
        var quotientViaFuncDecimalRight = a.Div(2.0m);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetRealValue(quotient).ToDecimal(), Is.EqualTo(42.0m));
            Assert.That(model.GetRealValue(quotientViaDecimalLeft).ToDecimal(), Is.EqualTo(42.0m));
            Assert.That(model.GetRealValue(quotientViaDecimalRight).ToDecimal(), Is.EqualTo(42.0m));
            Assert.That(model.GetRealValue(quotientViaContext).ToDecimal(), Is.EqualTo(42.0m));
            Assert.That(model.GetRealValue(quotientViaContextDecimalLeft).ToDecimal(), Is.EqualTo(42.0m));
            Assert.That(model.GetRealValue(quotientViaContextDecimalRight).ToDecimal(), Is.EqualTo(42.0m));
            Assert.That(model.GetRealValue(quotientViaFunc).ToDecimal(), Is.EqualTo(42.0m));
            Assert.That(model.GetRealValue(quotientViaFuncDecimalRight).ToDecimal(), Is.EqualTo(42.0m));
        });
    }

    [Test]
    public void UnaryMinus_SingleValue_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Real(42.0m);

        var negation = -a;
        var negationViaContext = context.UnaryMinus(a);
        var negationViaFunc = a.UnaryMinus();

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetRealValue(negation).ToDecimal(), Is.EqualTo(-42.0m));
            Assert.That(model.GetRealValue(negationViaContext).ToDecimal(), Is.EqualTo(-42.0m));
            Assert.That(model.GetRealValue(negationViaFunc).ToDecimal(), Is.EqualTo(-42.0m));
        });
    }

    [Test]
    public void Add_ExactRationalValues_PreservesPrecision()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var oneThird = context.Real(new Real(1, 3));
        var twoThirds = context.Real(new Real(2, 3));
        var result = oneThird + twoThirds;

        solver.Check();
        var model = solver.GetModel();

        var value = model.GetRealValue(result);
        Assert.That(value.ToDecimal(), Is.EqualTo(1.0m));
    }

    [Test]
    public void Multiply_RationalValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var oneHalf = context.Real(new Real(1, 2));
        var twoThirds = context.Real(new Real(2, 3));
        var result = oneHalf * twoThirds;

        solver.Check();
        var model = solver.GetModel();

        var value = model.GetRealValue(result);
        // 1/2 * 2/3 = 2/6 = 1/3
        Assert.That(value, Is.EqualTo(new Real(1, 3)));
    }

    [TestCase(42.5, 42.5)]
    [TestCase(-42.5, 42.5)]
    [TestCase(0.0, 0.0)]
    public void Abs_Value_ReturnsAbsoluteValue(double inputValue, double expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Real((decimal)inputValue);

        var absValue = context.Abs(a);
        var absViaFunc = a.Abs();

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetRealValue(absValue).ToDecimal(), Is.EqualTo((decimal)expected));
            Assert.That(model.GetRealValue(absViaFunc).ToDecimal(), Is.EqualTo((decimal)expected));
        });
    }

    [Test]
    public void Abs_RationalValue_PreservesPrecision()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var negativeThird = context.Real(new Real(-1, 3));

        var absValue = context.Abs(negativeThird);
        var absViaFunc = negativeThird.Abs();

        solver.Check();
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(model.GetRealValue(absValue), Is.EqualTo(new Real(1, 3)));
            Assert.That(model.GetRealValue(absViaFunc), Is.EqualTo(new Real(1, 3)));
        });
    }

    [TestCase(42.5, 17.5, 17.5)]
    [TestCase(42.0, 42.0, 42.0)]
    [TestCase(-10.5, -50.5, -50.5)]
    public void Min_TwoValues_ReturnsMinimum(double aValue, double bValue, double expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Real((decimal)aValue);
        var b = context.Real((decimal)bValue);

        var minValue = context.Min(a, b);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetRealValue(minValue).ToDecimal(), Is.EqualTo((decimal)expected));
    }

    [Test]
    public void Min_RationalValues_PreservesPrecision()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var oneThird = context.Real(new Real(1, 3));
        var twoThirds = context.Real(new Real(2, 3));

        var minValue = context.Min(oneThird, twoThirds);

        solver.Check();
        var model = solver.GetModel();

        Assert.That(model.GetRealValue(minValue), Is.EqualTo(new Real(1, 3)));
    }

    [TestCase(42.5, 17.5, 42.5)]
    [TestCase(42.0, 42.0, 42.0)]
    [TestCase(-10.5, -50.5, -10.5)]
    public void Max_TwoValues_ReturnsMaximum(double aValue, double bValue, double expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Real((decimal)aValue);
        var b = context.Real((decimal)bValue);

        var maxValue = context.Max(a, b);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetRealValue(maxValue).ToDecimal(), Is.EqualTo((decimal)expected));
    }

    [Test]
    public void Max_RationalValues_PreservesPrecision()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var oneThird = context.Real(new Real(1, 3));
        var twoThirds = context.Real(new Real(2, 3));

        var maxValue = context.Max(oneThird, twoThirds);

        solver.Check();
        var model = solver.GetModel();

        Assert.That(model.GetRealValue(maxValue), Is.EqualTo(new Real(2, 3)));
    }
}
