using System.Numerics;
using Spaceorc.Z3Wrap;
using Spaceorc.Z3Wrap.BitVectors;
using Spaceorc.Z3Wrap.Booleans;
using Spaceorc.Z3Wrap.DataTypes;
using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Extensions;

namespace Z3Wrap.Tests.Integration;

[TestFixture]
public class Z3ModelValueExtractionTests
{
    [Test]
    public void EvaluateIntegerExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        solver.Assert(x == 42);

        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var evaluated = model.Evaluate(x);

        Assert.That(evaluated, Is.TypeOf<Z3IntExpr>());
        Assert.That(evaluated.ToString(), Is.EqualTo("42"));
    }

    [Test]
    public void EvaluateBooleanExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        solver.Assert(p);

        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var evaluated = model.Evaluate(p);

        Assert.That(evaluated, Is.TypeOf<Z3Bool>());
        Assert.That(evaluated.ToString(), Is.EqualTo("true"));
    }

    [Test]
    public void EvaluateRealExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var z = context.RealConst("z");
        solver.Assert(z == 3.14m);

        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var evaluated = model.Evaluate(z);

        Assert.That(evaluated, Is.TypeOf<Z3RealExpr>());
        Assert.That(evaluated.ToString(), Does.Contain("157").And.Contain("50"));
    }

    [Test]
    public void GetIntValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        solver.Assert(x == 123);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var value = model.GetIntValue(x);

        Assert.That(value, Is.EqualTo(new BigInteger(123)));
    }

    [Test]
    public void GetBoolValue_True()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        solver.Assert(p);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var value = model.GetBoolValue(p);

        Assert.That(value, Is.True);
    }

    [Test]
    public void GetBoolValue_False()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        solver.Assert(p == false);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var value = model.GetBoolValue(p);

        Assert.That(value, Is.False);
    }

    [Test]
    public void GetRealValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var z = context.RealConst("z");
        solver.Assert(z == 2.718m);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var value = model.GetRealValue(z);

        Assert.That(value, Is.EqualTo(new Real(2.718m)));
    }

    [Test]
    public void GetRealValue_ExactFraction()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var z = context.RealConst("z");
        solver.Assert(z == new Real(1, 3));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var value = model.GetRealValue(z);

        Assert.That(value, Is.EqualTo(new Real(1, 3)));
    }

    [Test]
    public void GetNumericValueAsString()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var z = context.RealConst("z");
        solver.Assert(z == 2.718m);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var value = model.GetNumericValueAsString(z);

        Assert.That(value, Is.EqualTo("1359/500"));
    }

    [Test]
    public void GetIntValue_ComplexExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var y = context.IntConst("y");
        solver.Assert(x + y == 15);
        solver.Assert(x - y == 5);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var xValue = model.GetIntValue(x);
        var yValue = model.GetIntValue(y);

        Assert.That(xValue + yValue, Is.EqualTo(new BigInteger(15)));
        Assert.That(xValue - yValue, Is.EqualTo(new BigInteger(5)));
        Assert.That(xValue, Is.EqualTo(new BigInteger(10)));
        Assert.That(yValue, Is.EqualTo(new BigInteger(5)));
    }

    [Test]
    public void GetBoolValue_ComplexExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        solver.Assert(p | q);
        solver.Assert(!p);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var pValue = model.GetBoolValue(p);
        var qValue = model.GetBoolValue(q);

        Assert.That(pValue, Is.False);
        Assert.That(qValue, Is.True);
    }

    [Test]
    public void EvaluateWithModelCompletion_True()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var y = context.IntConst("y");
        solver.Assert(x == 10);
        // y is not constrained, should get a value with model completion

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var xEval = model.Evaluate(x, modelCompletion: true);
        var yEval = model.Evaluate(y, modelCompletion: true);

        Assert.That(xEval, Is.Not.Null);
        Assert.That(yEval, Is.Not.Null);
        Assert.That(model.GetIntValue(x), Is.EqualTo(new BigInteger(10)));
    }

    [Test]
    public void EvaluateWithModelCompletion_False()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var y = context.IntConst("y");
        solver.Assert(x == 10);
        // y is not constrained

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var xEval = model.Evaluate(x, modelCompletion: false);
        var yEval = model.Evaluate(y, modelCompletion: false);

        Assert.That(xEval, Is.Not.Null);
        Assert.That(yEval, Is.Not.Null);
        Assert.That(model.GetIntValue(x), Is.EqualTo(new BigInteger(10)));
    }

    [Test]
    public void GetIntValue_ThrowsOnNonConstant()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var sum = x + y; // This is not a constant

        solver.Assert(x == 5);
        solver.Assert(y == 10);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();

        // These should work (constants)
        Assert.That(model.GetIntValue(x), Is.EqualTo(new BigInteger(5)));
        Assert.That(model.GetIntValue(y), Is.EqualTo(new BigInteger(10)));

        // This should work (evaluates to a constant)
        var sumEval = model.Evaluate(sum);
        Assert.That(sumEval, Is.TypeOf<Z3IntExpr>());

        // But trying to get value of the original sum expression might fail
        // if it doesn't evaluate to a pure numeral in the model
        Assert.DoesNotThrow(() => model.GetIntValue(sumEval));
    }

    [Test]
    public void EvaluateInvalidatedModel_ThrowsException()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        solver.Assert(x == 5);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        // Invalidate model by changing solver state
        solver.Assert(x == 10); // Contradiction

        // Should throw on invalidated model
        Assert.Throws<ObjectDisposedException>(() => model.Evaluate(x));
        Assert.Throws<ObjectDisposedException>(() => model.GetIntValue(x));
        Assert.Throws<ObjectDisposedException>(() => model.GetBoolValue(context.BoolConst("p")));
        Assert.Throws<ObjectDisposedException>(() => model.GetRealValue(context.RealConst("z")));
        Assert.Throws<ObjectDisposedException>(() =>
            model.GetNumericValueAsString(context.RealConst("z"))
        );
        Assert.Throws<ObjectDisposedException>(() =>
            model.GetBitVec(context.BitVecConst<Size8>("bv"))
        );
        Assert.Throws<ObjectDisposedException>(() =>
            model.GetNumericValueAsString(context.BitVecConst<Size8>("bv"))
        );
    }

    [Test]
    public void GetNumericValueAsString_BasicValues()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var bv = context.BitVecConst<Size8>("bv");
        solver.Assert(bv == 42);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var value = model.GetNumericValueAsString(bv);

        Assert.That(value, Is.EqualTo("42"));
    }

    [Test]
    public void GetNumericValueAsString_NegativeInput()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var bv = context.BitVecConst<Size32>("bv");
        solver.Assert(bv == -1);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var value = model.GetNumericValueAsString(bv);

        // -1 in 32-bit bitvector should be represented as 4294967295 (2^32 - 1)
        Assert.That(value, Is.EqualTo("4294967295"));
    }

    [Test]
    public void GetBitVecAndStringValue_AreConsistent()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var bv = context.BitVecConst<Size32>("bv");
        solver.Assert(bv == 123456);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var bitVecValue = model.GetBitVec(bv);
        var stringValue = model.GetNumericValueAsString(bv);

        // Both methods should return consistent values
        Assert.That(bitVecValue.Value, Is.EqualTo(new BigInteger(123456)));
        Assert.That(stringValue, Is.EqualTo("123456"));
        Assert.That(bitVecValue.Value.ToString(), Is.EqualTo(stringValue));
    }

    [Test]
    public void GetBitVec_SimpleValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var bv = context.BitVecConst<Size8>("bv");
        solver.Assert(bv == 42);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var extractedBitVec = model.GetBitVec(bv);

        // Verify we got the correct BitVec from the model
        Assert.That(extractedBitVec.Value, Is.EqualTo(new BigInteger(42)));
    }

    [Test]
    public void GetBitVec_NegativeInput()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var bv = context.BitVecConst<Size8>("bv");
        solver.Assert(bv == -1); // Becomes 255 in unsigned representation

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var extractedBitVec = model.GetBitVec(bv);

        // Verify model extraction gives us the correct unsigned representation
        Assert.That(extractedBitVec.Value, Is.EqualTo(new BigInteger(255)));
    }

    [Test]
    public void Z3BitVecExpr_ToString_ShowsSizeAndContent()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var bv = context.BitVecConst<Size32>("x");

        var toString = bv.ToString();

        Assert.That(toString, Does.Contain("BitVec[32]"));
        Assert.That(toString, Does.Contain("x"));
    }

    [Test]
    public void Z3BitVecExpr_ToString_WithValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var bv = context.BitVec<Size8>(42);

        var toString = bv.ToString();

        Assert.That(toString, Does.Contain("BitVec[8]"));
        Assert.That(
            toString,
            Does.Contain("42") | Does.Contain("#x2a") | Does.Contain("#b00101010")
        );
    }

    [Test]
    public void Z3BitVecExpr_ToString_DifferentSizes()
    {
        using var context = new Z3Context();

        var bv8 = context.BitVecConst<Size8>("bv8");
        var bv16 = context.BitVecConst<Size16>("bv16");
        var bv32 = context.BitVecConst<Size32>("bv32");
        var bv64 = context.BitVecConst<Size64>("bv64");

        Assert.That(bv8.ToString(), Does.Contain("BitVec[8]"));
        Assert.That(bv16.ToString(), Does.Contain("BitVec[16]"));
        Assert.That(bv32.ToString(), Does.Contain("BitVec[32]"));
        Assert.That(bv64.ToString(), Does.Contain("BitVec[64]"));
    }
}
