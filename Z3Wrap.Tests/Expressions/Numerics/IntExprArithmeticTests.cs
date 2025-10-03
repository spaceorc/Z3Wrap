using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Expressions.Numerics;

namespace Z3Wrap.Tests.Expressions.Numerics;

[TestFixture]
public class IntExprArithmeticTests
{
    [Test]
    public void Add_TwoValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Int(10);
        var b = context.Int(32);

        var sum = a + b;
        var sumViaIntLeft = 10 + b;
        var sumViaIntRight = a + 32;
        var sumViaContext = context.Add(a, b);
        var sumViaContextIntLeft = context.Add(10, b);
        var sumViaContextIntRight = context.Add(a, 32);
        var sumViaFunc = a.Add(b);
        var sumViaFuncIntRight = a.Add(32);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetIntValue(sum), Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetIntValue(sumViaIntLeft), Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetIntValue(sumViaIntRight), Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetIntValue(sumViaContext), Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetIntValue(sumViaContextIntLeft), Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetIntValue(sumViaContextIntRight), Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetIntValue(sumViaFunc), Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetIntValue(sumViaFuncIntRight), Is.EqualTo(new BigInteger(42)));
        });
    }

    [Test]
    public void Add_SingleValue_ReturnsSameValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Int(42);
        var sum = context.Add(a);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetIntValue(sum), Is.EqualTo(new BigInteger(42)));
    }

    [Test]
    public void Add_ThreeValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Int(10);
        var b = context.Int(20);
        var c = context.Int(12);
        var sumViaContext = context.Add(a, b, c);
        var sumViaExpr = a.Add(b, c);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetIntValue(sumViaContext), Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetIntValue(sumViaExpr), Is.EqualTo(new BigInteger(42)));
        });
    }

    [Test]
    public void Add_FourValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Int(5);
        var b = context.Int(10);
        var c = context.Int(15);
        var d = context.Int(12);
        var sumViaContext = context.Add(a, b, c, d);
        var sumViaExpr = a.Add(b, c, d);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetIntValue(sumViaContext), Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetIntValue(sumViaExpr), Is.EqualTo(new BigInteger(42)));
        });
    }

    [Test]
    public void Subtract_TwoValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Int(50);
        var b = context.Int(8);

        var difference = a - b;
        var differenceViaIntLeft = 50 - b;
        var differenceViaIntRight = a - 8;
        var differenceViaContext = context.Sub(a, b);
        var differenceViaContextIntLeft = context.Sub(50, b);
        var differenceViaContextIntRight = context.Sub(a, 8);
        var differenceViaFunc = a.Sub(b);
        var differenceViaFuncIntRight = a.Sub(8);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetIntValue(difference), Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetIntValue(differenceViaIntLeft), Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetIntValue(differenceViaIntRight), Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetIntValue(differenceViaContext), Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetIntValue(differenceViaContextIntLeft), Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetIntValue(differenceViaContextIntRight), Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetIntValue(differenceViaFunc), Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetIntValue(differenceViaFuncIntRight), Is.EqualTo(new BigInteger(42)));
        });
    }

    [Test]
    public void Subtract_SingleValue_ReturnsSameValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Int(42);
        var difference = context.Sub(a);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetIntValue(difference), Is.EqualTo(new BigInteger(42)));
    }

    [Test]
    public void Subtract_ThreeValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Int(100);
        var b = context.Int(30);
        var c = context.Int(28);
        var differenceViaContext = context.Sub(a, b, c);
        var differenceViaExpr = a.Sub(b, c);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetIntValue(differenceViaContext), Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetIntValue(differenceViaExpr), Is.EqualTo(new BigInteger(42)));
        });
    }

    [Test]
    public void Subtract_FourValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Int(200);
        var b = context.Int(100);
        var c = context.Int(50);
        var d = context.Int(8);
        var differenceViaContext = context.Sub(a, b, c, d);
        var differenceViaExpr = a.Sub(b, c, d);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetIntValue(differenceViaContext), Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetIntValue(differenceViaExpr), Is.EqualTo(new BigInteger(42)));
        });
    }

    [Test]
    public void Multiply_TwoValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Int(6);
        var b = context.Int(7);

        var product = a * b;
        var productViaIntLeft = 6 * b;
        var productViaIntRight = a * 7;
        var productViaContext = context.Mul(a, b);
        var productViaContextIntLeft = context.Mul(6, b);
        var productViaContextIntRight = context.Mul(a, 7);
        var productViaFunc = a.Mul(b);
        var productViaFuncIntRight = a.Mul(7);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetIntValue(product), Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetIntValue(productViaIntLeft), Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetIntValue(productViaIntRight), Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetIntValue(productViaContext), Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetIntValue(productViaContextIntLeft), Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetIntValue(productViaContextIntRight), Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetIntValue(productViaFunc), Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetIntValue(productViaFuncIntRight), Is.EqualTo(new BigInteger(42)));
        });
    }

    [Test]
    public void Multiply_SingleValue_ReturnsSameValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Int(42);
        var product = context.Mul(a);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetIntValue(product), Is.EqualTo(new BigInteger(42)));
    }

    [Test]
    public void Multiply_ThreeValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Int(2);
        var b = context.Int(3);
        var c = context.Int(7);
        var productViaContext = context.Mul(a, b, c);
        var productViaExpr = a.Mul(b, c);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetIntValue(productViaContext), Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetIntValue(productViaExpr), Is.EqualTo(new BigInteger(42)));
        });
    }

    [Test]
    public void Multiply_FourValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Int(2);
        var b = context.Int(3);
        var c = context.Int(7);
        var d = context.Int(1);
        var productViaContext = context.Mul(a, b, c, d);
        var productViaExpr = a.Mul(b, c, d);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetIntValue(productViaContext), Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetIntValue(productViaExpr), Is.EqualTo(new BigInteger(42)));
        });
    }

    [Test]
    public void Add_ZeroArguments_ThrowsException()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        Assert.Throws<InvalidOperationException>(() => context.Add<IntExpr>());
    }

    [Test]
    public void Subtract_ZeroArguments_ThrowsException()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        Assert.Throws<InvalidOperationException>(() => context.Sub<IntExpr>());
    }

    [Test]
    public void Multiply_ZeroArguments_ThrowsException()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        Assert.Throws<InvalidOperationException>(() => context.Mul<IntExpr>());
    }

    [Test]
    public void Divide_TwoValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Int(84);
        var b = context.Int(2);

        var quotient = a / b;
        var quotientViaIntLeft = 84 / b;
        var quotientViaIntRight = a / 2;
        var quotientViaContext = context.Div(a, b);
        var quotientViaContextIntLeft = context.Div(84, b);
        var quotientViaContextIntRight = context.Div(a, 2);
        var quotientViaFunc = a.Div(b);
        var quotientViaFuncIntRight = a.Div(2);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetIntValue(quotient), Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetIntValue(quotientViaIntLeft), Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetIntValue(quotientViaIntRight), Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetIntValue(quotientViaContext), Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetIntValue(quotientViaContextIntLeft), Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetIntValue(quotientViaContextIntRight), Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetIntValue(quotientViaFunc), Is.EqualTo(new BigInteger(42)));
            Assert.That(model.GetIntValue(quotientViaFuncIntRight), Is.EqualTo(new BigInteger(42)));
        });
    }

    [Test]
    public void Rem_TwoValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Int(47);
        var b = context.Int(5);

        var remainder = a % b;
        var remainderViaIntLeft = 47 % b;
        var remainderViaIntRight = a % 5;
        var remainderViaContext = context.Rem(a, b);
        var remainderViaContextIntLeft = context.Rem(47, b);
        var remainderViaContextIntRight = context.Rem(a, 5);
        var remainderViaFunc = a.Rem(b);
        var remainderViaFuncIntRight = a.Rem(5);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetIntValue(remainder), Is.EqualTo(new BigInteger(2)));
            Assert.That(model.GetIntValue(remainderViaIntLeft), Is.EqualTo(new BigInteger(2)));
            Assert.That(model.GetIntValue(remainderViaIntRight), Is.EqualTo(new BigInteger(2)));
            Assert.That(model.GetIntValue(remainderViaContext), Is.EqualTo(new BigInteger(2)));
            Assert.That(model.GetIntValue(remainderViaContextIntLeft), Is.EqualTo(new BigInteger(2)));
            Assert.That(model.GetIntValue(remainderViaContextIntRight), Is.EqualTo(new BigInteger(2)));
            Assert.That(model.GetIntValue(remainderViaFunc), Is.EqualTo(new BigInteger(2)));
            Assert.That(model.GetIntValue(remainderViaFuncIntRight), Is.EqualTo(new BigInteger(2)));
        });
    }

    [Test]
    public void Mod_TwoValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Int(47);
        var b = context.Int(5);

        var modulo = context.Mod(a, b);
        var moduloViaIntLeft = context.Mod(47, b);
        var moduloViaIntRight = context.Mod(a, 5);
        var moduloViaFunc = a.Mod(b);
        var moduloViaFuncIntRight = a.Mod(5);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetIntValue(modulo), Is.EqualTo(new BigInteger(2)));
            Assert.That(model.GetIntValue(moduloViaIntLeft), Is.EqualTo(new BigInteger(2)));
            Assert.That(model.GetIntValue(moduloViaIntRight), Is.EqualTo(new BigInteger(2)));
            Assert.That(model.GetIntValue(moduloViaFunc), Is.EqualTo(new BigInteger(2)));
            Assert.That(model.GetIntValue(moduloViaFuncIntRight), Is.EqualTo(new BigInteger(2)));
        });
    }

    [Test]
    public void UnaryMinus_SingleValue_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Int(42);

        var negation = -a;
        var negationViaContext = context.UnaryMinus(a);
        var negationViaFunc = a.UnaryMinus();

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetIntValue(negation), Is.EqualTo(new BigInteger(-42)));
            Assert.That(model.GetIntValue(negationViaContext), Is.EqualTo(new BigInteger(-42)));
            Assert.That(model.GetIntValue(negationViaFunc), Is.EqualTo(new BigInteger(-42)));
        });
    }

    [TestCase(42, 42)]
    [TestCase(-42, 42)]
    [TestCase(0, 0)]
    public void Abs_Value_ReturnsAbsoluteValue(int inputValue, int expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Int(inputValue);

        var absValue = context.Abs(a);
        var absViaFunc = a.Abs();

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetIntValue(absValue), Is.EqualTo(new BigInteger(expected)));
            Assert.That(model.GetIntValue(absViaFunc), Is.EqualTo(new BigInteger(expected)));
        });
    }

    [Test]
    public void Abs_SymbolicValue_CanSolveConstraints()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var absX = x.Abs();

        solver.Assert(absX == 42);
        solver.Assert(x < 0);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetIntValue(x), Is.EqualTo(new BigInteger(-42)));
    }

    [TestCase(42, 17, 17)]
    [TestCase(42, 42, 42)]
    [TestCase(-10, -50, -50)]
    public void Min_TwoValues_ReturnsMinimum(int aValue, int bValue, int expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Int(aValue);
        var b = context.Int(bValue);

        var minValue = context.Min(a, b);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetIntValue(minValue), Is.EqualTo(new BigInteger(expected)));
    }

    [Test]
    public void Min_SymbolicValues_CanSolveConstraints()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var minXy = context.Min(x, y);

        solver.Assert(minXy == 10);
        solver.Assert(x == 15);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetIntValue(y), Is.EqualTo(new BigInteger(10)));
    }

    [TestCase(42, 17, 42)]
    [TestCase(42, 42, 42)]
    [TestCase(-10, -50, -10)]
    public void Max_TwoValues_ReturnsMaximum(int aValue, int bValue, int expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Int(aValue);
        var b = context.Int(bValue);

        var maxValue = context.Max(a, b);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetIntValue(maxValue), Is.EqualTo(new BigInteger(expected)));
    }

    [Test]
    public void Max_SymbolicValues_CanSolveConstraints()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var maxXy = context.Max(x, y);

        solver.Assert(maxXy == 20);
        solver.Assert(x == 15);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetIntValue(y), Is.EqualTo(new BigInteger(20)));
    }
}
