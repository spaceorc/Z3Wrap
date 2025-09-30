using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Expressions.Numerics;

namespace Z3Wrap.Tests.Expressions.Logic;

[TestFixture]
public class BoolExprLogicTests
{
    [TestCase(true, true, true)]
    [TestCase(true, false, false)]
    [TestCase(false, true, false)]
    [TestCase(false, false, false)]
    public void And_TwoValues_ComputesCorrectResult(bool aValue, bool bValue, bool expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Bool(aValue);
        var b = context.Bool(bValue);

        var result = a & b;
        var resultViaBoolLeft = aValue & b;
        var resultViaBoolRight = a & bValue;
        var resultViaContext = context.And(a, b);
        var resultViaContextBoolLeft = context.And(aValue, b);
        var resultViaContextBoolRight = context.And(a, bValue);
        var resultViaFunc = a.And(b);
        var resultViaFuncBoolRight = a.And(bValue);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(result), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaBoolLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaBoolRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContext), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextBoolLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextBoolRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFunc), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFuncBoolRight), Is.EqualTo(expected));
        });
    }

    [Test]
    public void And_SingleValue_ReturnsSameValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Bool(true);
        var result = context.And(a);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(result), Is.True);
    }

    [Test]
    public void And_ThreeValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Bool(true);
        var b = context.Bool(true);
        var c = context.Bool(true);
        var resultViaContext = context.And(a, b, c);
        var resultViaExpr = a.And(b, c);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(resultViaContext), Is.True);
            Assert.That(model.GetBoolValue(resultViaExpr), Is.True);
        });
    }

    [Test]
    public void And_FourValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Bool(true);
        var b = context.Bool(true);
        var c = context.Bool(true);
        var d = context.Bool(false);
        var resultViaContext = context.And(a, b, c, d);
        var resultViaExpr = a.And(b, c, d);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(resultViaContext), Is.False);
            Assert.That(model.GetBoolValue(resultViaExpr), Is.False);
        });
    }

    [TestCase(true, true, true)]
    [TestCase(true, false, true)]
    [TestCase(false, true, true)]
    [TestCase(false, false, false)]
    public void Or_TwoValues_ComputesCorrectResult(bool aValue, bool bValue, bool expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Bool(aValue);
        var b = context.Bool(bValue);

        var result = a | b;
        var resultViaBoolLeft = aValue | b;
        var resultViaBoolRight = a | bValue;
        var resultViaContext = context.Or(a, b);
        var resultViaContextBoolLeft = context.Or(aValue, b);
        var resultViaContextBoolRight = context.Or(a, bValue);
        var resultViaFunc = a.Or(b);
        var resultViaFuncBoolRight = a.Or(bValue);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(result), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaBoolLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaBoolRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContext), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextBoolLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextBoolRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFunc), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFuncBoolRight), Is.EqualTo(expected));
        });
    }

    [Test]
    public void Or_SingleValue_ReturnsSameValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Bool(false);
        var result = context.Or(a);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(result), Is.False);
    }

    [Test]
    public void Or_ThreeValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Bool(false);
        var b = context.Bool(false);
        var c = context.Bool(true);
        var resultViaContext = context.Or(a, b, c);
        var resultViaExpr = a.Or(b, c);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(resultViaContext), Is.True);
            Assert.That(model.GetBoolValue(resultViaExpr), Is.True);
        });
    }

    [Test]
    public void Or_FourValues_ComputesCorrectResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Bool(false);
        var b = context.Bool(false);
        var c = context.Bool(false);
        var d = context.Bool(false);
        var resultViaContext = context.Or(a, b, c, d);
        var resultViaExpr = a.Or(b, c, d);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(resultViaContext), Is.False);
            Assert.That(model.GetBoolValue(resultViaExpr), Is.False);
        });
    }

    [TestCase(true, true, false)]
    [TestCase(true, false, true)]
    [TestCase(false, true, true)]
    [TestCase(false, false, false)]
    public void Xor_TwoValues_ComputesCorrectResult(bool aValue, bool bValue, bool expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Bool(aValue);
        var b = context.Bool(bValue);

        var result = a ^ b;
        var resultViaBoolLeft = aValue ^ b;
        var resultViaBoolRight = a ^ bValue;
        var resultViaContext = context.Xor(a, b);
        var resultViaContextBoolLeft = context.Xor(aValue, b);
        var resultViaContextBoolRight = context.Xor(a, bValue);
        var resultViaFunc = a.Xor(b);
        var resultViaFuncBoolRight = a.Xor(bValue);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(result), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaBoolLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaBoolRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContext), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextBoolLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextBoolRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFunc), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFuncBoolRight), Is.EqualTo(expected));
        });
    }

    [TestCase(true, false)]
    [TestCase(false, true)]
    public void Not_SingleValue_ComputesCorrectResult(bool aValue, bool expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Bool(aValue);

        var result = !a;
        var resultViaContext = context.Not(a);
        var resultViaFunc = a.Not();

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(result), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContext), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFunc), Is.EqualTo(expected));
        });
    }

    [TestCase(true, true, true)]
    [TestCase(true, false, false)]
    [TestCase(false, true, true)]
    [TestCase(false, false, true)]
    public void Implies_TwoValues_ComputesCorrectResult(bool aValue, bool bValue, bool expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Bool(aValue);
        var b = context.Bool(bValue);

        var resultViaContext = context.Implies(a, b);
        var resultViaContextBoolLeft = context.Implies(aValue, b);
        var resultViaContextBoolRight = context.Implies(a, bValue);
        var resultViaFunc = a.Implies(b);
        var resultViaFuncBoolRight = a.Implies(bValue);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(resultViaContext), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextBoolLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextBoolRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFunc), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFuncBoolRight), Is.EqualTo(expected));
        });
    }

    [TestCase(true, true, true)]
    [TestCase(true, false, false)]
    [TestCase(false, true, false)]
    [TestCase(false, false, true)]
    public void Iff_TwoValues_ComputesCorrectResult(bool aValue, bool bValue, bool expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Bool(aValue);
        var b = context.Bool(bValue);

        var resultViaContext = context.Iff(a, b);
        var resultViaContextBoolLeft = context.Iff(aValue, b);
        var resultViaContextBoolRight = context.Iff(a, bValue);
        var resultViaFunc = a.Iff(b);
        var resultViaFuncBoolRight = a.Iff(bValue);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(resultViaContext), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextBoolLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextBoolRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFunc), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaFuncBoolRight), Is.EqualTo(expected));
        });
    }

    [TestCase(true, 42, 99, 42)]
    [TestCase(false, 42, 99, 99)]
    public void Ite_ConditionalSelection_ComputesCorrectResult(
        bool conditionValue,
        int ifTrue,
        int ifFalse,
        int expected
    )
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var condition = context.Bool(conditionValue);
        var thenValue = context.Int(ifTrue);
        var elseValue = context.Int(ifFalse);

        var resultViaContext = context.Ite(condition, thenValue, elseValue);
        var resultViaFunc = condition.Ite(thenValue, elseValue);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetIntValue(resultViaContext), Is.EqualTo(new BigInteger(expected)));
            Assert.That(model.GetIntValue(resultViaFunc), Is.EqualTo(new BigInteger(expected)));
        });
    }
}
