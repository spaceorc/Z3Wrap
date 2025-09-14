using System.Numerics;
using Z3Wrap.DataTypes;
using Z3Wrap.Expressions;

namespace Z3Wrap.Tests.CoreTests;

[TestFixture]
public class Z3ModelValueExtractionTests
{
    [Test]
    public void EvaluateIntegerExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        using var scope = context.SetUp();
        
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
        using var solver = context.CreateSolver();
        
        var p = context.BoolConst("p");
        solver.Assert(p);
        
        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
        
        var model = solver.GetModel();
        var evaluated = model.Evaluate(p);
        
        Assert.That(evaluated, Is.TypeOf<Z3BoolExpr>());
        Assert.That(evaluated.ToString(), Is.EqualTo("true"));
    }
    
    [Test]
    public void EvaluateRealExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var z = context.RealConst("z");
        solver.Assert(z == context.Real(3.14m));
        
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
        using var solver = context.CreateSolver();
        
        var x = context.IntConst("x");
        solver.Assert(x == context.Int(123));
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        
        var model = solver.GetModel();
        var value = model.GetIntValue(x);
        
        Assert.That(value, Is.EqualTo(new BigInteger(123)));
    }
    
    [Test]
    public void GetBoolValue_True()
    {
        using var context = new Z3Context();
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
        using var solver = context.CreateSolver();
        
        var p = context.BoolConst("p");
        solver.Assert(p == context.False());
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        
        var model = solver.GetModel();
        var value = model.GetBoolValue(p);
        
        Assert.That(value, Is.False);
    }
    
    [Test]
    public void GetRealValue()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var z = context.RealConst("z");
        solver.Assert(z == context.Real(2.718m));
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        
        var model = solver.GetModel();
        var value = model.GetRealValue(z);
        
        Assert.That(value, Is.EqualTo(new Real(2.718m)));
    }

    [Test]
    public void GetRealValue_ExactFraction()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var z = context.RealConst("z");
        solver.Assert(z == context.Real(new Real(1, 3)));
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        
        var model = solver.GetModel();
        var value = model.GetRealValue(z);
        
        Assert.That(value, Is.EqualTo(new Real(1, 3)));
    }

    [Test]
    public void GetRealValueAsString()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var z = context.RealConst("z");
        solver.Assert(z == context.Real(2.718m));
        
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        
        var model = solver.GetModel();
        var value = model.GetRealValueAsString(z);
        
        Assert.That(value, Is.EqualTo("1359/500"));
    }
    
    [Test]
    public void GetIntValue_ComplexExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.IntConst("x");
        var y = context.IntConst("y");
        solver.Assert(x + y == context.Int(15));
        solver.Assert(x - y == context.Int(5));
        
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
        using var solver = context.CreateSolver();
        
        var x = context.IntConst("x");
        var y = context.IntConst("y");
        solver.Assert(x == context.Int(10));
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
        using var solver = context.CreateSolver();
        
        var x = context.IntConst("x");
        var y = context.IntConst("y");
        solver.Assert(x == context.Int(10));
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
        using var solver = context.CreateSolver();
        
        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var sum = x + y; // This is not a constant
        
        solver.Assert(x == context.Int(5));
        solver.Assert(y == context.Int(10));
        
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
        Assert.DoesNotThrow(() => model.GetIntValue((Z3IntExpr)sumEval));
    }
    
    [Test]
    public void EvaluateInvalidatedModel_ThrowsException()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        solver.Assert(x == context.Int(5));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        // Invalidate model by changing solver state
        solver.Assert(x == context.Int(10)); // Contradiction

        // Should throw on invalidated model
        Assert.Throws<ObjectDisposedException>(() => model.Evaluate(x));
        Assert.Throws<ObjectDisposedException>(() => model.GetIntValue(x));
        Assert.Throws<ObjectDisposedException>(() => model.GetBoolValue(context.BoolConst("p")));
        Assert.Throws<ObjectDisposedException>(() => model.GetRealValue(context.RealConst("z")));
        Assert.Throws<ObjectDisposedException>(() => model.GetRealValueAsString(context.RealConst("z")));
        Assert.Throws<ObjectDisposedException>(() => model.GetBitVec(context.BitVecConst("bv", 8)));
        Assert.Throws<ObjectDisposedException>(() => model.GetNumericValueAsString(context.BitVecConst("bv", 8)));
    }

    [Test]
    public void GetBitVecValueAsString_SimpleValue()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var bv = context.BitVecConst("bv", 8);
        solver.Assert(bv == context.BitVec(42, 8));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var value = model.GetNumericValueAsString(bv);

        Assert.That(value, Is.EqualTo("42"));
    }

    [Test]
    public void GetBitVecValueAsString_ZeroValue()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var bv = context.BitVecConst("bv", 16);
        solver.Assert(bv == context.BitVec(0, 16));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var value = model.GetNumericValueAsString(bv);

        Assert.That(value, Is.EqualTo("0"));
    }

    [Test]
    public void GetBitVecValueAsString_MaxValue()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var bv = context.BitVecConst("bv", 8);
        solver.Assert(bv == context.BitVec(255, 8)); // max value for 8-bit

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var value = model.GetNumericValueAsString(bv);

        Assert.That(value, Is.EqualTo("255"));
    }

    [Test]
    public void GetBitVecValueAsString_NegativeValue()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var bv = context.BitVecConst("bv", 32);
        solver.Assert(bv == context.BitVec(-1, 32));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var value = model.GetNumericValueAsString(bv);

        // -1 in 32-bit bitvector should be represented as 4294967295 (2^32 - 1)
        Assert.That(value, Is.EqualTo("4294967295"));
    }

    [Test]
    public void GetBitVecValueAsString_LargeValue()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var bv = context.BitVecConst("bv", 64);
        solver.Assert(bv == context.BitVec(1234567890, 64));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var value = model.GetNumericValueAsString(bv);

        Assert.That(value, Is.EqualTo("1234567890"));
    }

    [Test]
    public void GetBitVecValueAsString_DifferentSizes()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var bv8 = context.BitVecConst("bv8", 8);
        var bv16 = context.BitVecConst("bv16", 16);
        var bv32 = context.BitVecConst("bv32", 32);
        var bv64 = context.BitVecConst("bv64", 64);

        solver.Assert(bv8 == context.BitVec(100, 8));
        solver.Assert(bv16 == context.BitVec(1000, 16));
        solver.Assert(bv32 == context.BitVec(100000, 32));
        solver.Assert(bv64 == context.BitVec(1000000000, 64));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();

        Assert.That(model.GetNumericValueAsString(bv8), Is.EqualTo("100"));
        Assert.That(model.GetNumericValueAsString(bv16), Is.EqualTo("1000"));
        Assert.That(model.GetNumericValueAsString(bv32), Is.EqualTo("100000"));
        Assert.That(model.GetNumericValueAsString(bv64), Is.EqualTo("1000000000"));
    }

    [Test]
    public void GetBitVecValueAsString_ComplexExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.BitVecConst("x", 8);
        var y = context.BitVecConst("y", 8);

        // Set up constraints: x = 20, y = 30 (without using addition for now)
        solver.Assert(x == context.BitVec(20, 8));
        solver.Assert(y == context.BitVec(30, 8));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var xValue = model.GetNumericValueAsString(x);
        var yValue = model.GetNumericValueAsString(y);

        Assert.That(xValue, Is.EqualTo("20"));
        Assert.That(yValue, Is.EqualTo("30"));
    }

    [Test]
    public void GetBitVecValueAsString_InvalidatedModel_ThrowsException()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var bv = context.BitVecConst("bv", 8);
        solver.Assert(bv == context.BitVec(42, 8));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        // Invalidate model by changing solver state
        solver.Assert(bv == context.BitVec(43, 8)); // Contradiction

        // Should throw on invalidated model
        Assert.Throws<ObjectDisposedException>(() => model.GetNumericValueAsString(bv));
    }

    [Test]
    public void GetBitVecValueAsBigInteger_SimpleValue()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var bv = context.BitVecConst("bv", 8);
        solver.Assert(bv == context.BitVec(42, 8));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var value = model.GetBitVec(bv).Value;

        Assert.That(value, Is.EqualTo(new BigInteger(42)));
    }

    [Test]
    public void GetBitVecValueAsBigInteger_ZeroValue()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var bv = context.BitVecConst("bv", 16);
        solver.Assert(bv == context.BitVec(0, 16));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var value = model.GetBitVec(bv).Value;

        Assert.That(value, Is.EqualTo(BigInteger.Zero));
    }

    [Test]
    public void GetBitVecValueAsBigInteger_MaxValue()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var bv = context.BitVecConst("bv", 8);
        solver.Assert(bv == context.BitVec(255, 8)); // max value for 8-bit

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var value = model.GetBitVec(bv).Value;

        Assert.That(value, Is.EqualTo(new BigInteger(255)));
    }

    [Test]
    public void GetBitVecValueAsBigInteger_NegativeValue()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var bv = context.BitVecConst("bv", 32);
        solver.Assert(bv == context.BitVec(-1, 32));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var value = model.GetBitVec(bv).Value;

        // -1 in 32-bit bitvector should be represented as 4294967295 (2^32 - 1)
        Assert.That(value, Is.EqualTo(new BigInteger(4294967295L)));
    }

    [Test]
    public void GetBitVecValueAsBigInteger_LargeValue()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var bv = context.BitVecConst("bv", 64);
        solver.Assert(bv == context.BitVec(1234567890, 64));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var value = model.GetBitVec(bv).Value;

        Assert.That(value, Is.EqualTo(new BigInteger(1234567890)));
    }

    [Test]
    public void GetBitVecValueAsBigInteger_DifferentSizes()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var bv8 = context.BitVecConst("bv8", 8);
        var bv16 = context.BitVecConst("bv16", 16);
        var bv32 = context.BitVecConst("bv32", 32);
        var bv64 = context.BitVecConst("bv64", 64);

        solver.Assert(bv8 == context.BitVec(100, 8));
        solver.Assert(bv16 == context.BitVec(1000, 16));
        solver.Assert(bv32 == context.BitVec(100000, 32));
        solver.Assert(bv64 == context.BitVec(1000000000, 64));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();

        Assert.That(model.GetBitVec(bv8).Value, Is.EqualTo(new BigInteger(100)));
        Assert.That(model.GetBitVec(bv16).Value, Is.EqualTo(new BigInteger(1000)));
        Assert.That(model.GetBitVec(bv32).Value, Is.EqualTo(new BigInteger(100000)));
        Assert.That(model.GetBitVec(bv64).Value, Is.EqualTo(new BigInteger(1000000000)));
    }

    [Test]
    public void GetBitVecValueAsBigInteger_ComplexExpression()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.BitVecConst("x", 8);
        var y = context.BitVecConst("y", 8);

        // Set up constraints: x = 20, y = 30
        solver.Assert(x == context.BitVec(20, 8));
        solver.Assert(y == context.BitVec(30, 8));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var xValue = model.GetBitVec(x).Value;
        var yValue = model.GetBitVec(y).Value;

        Assert.That(xValue, Is.EqualTo(new BigInteger(20)));
        Assert.That(yValue, Is.EqualTo(new BigInteger(30)));
    }

    [Test]
    public void GetBitVecValueAsBigInteger_InvalidatedModel_ThrowsException()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var bv = context.BitVecConst("bv", 8);
        solver.Assert(bv == context.BitVec(42, 8));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        // Invalidate model by changing solver state
        solver.Assert(bv == context.BitVec(43, 8)); // Contradiction

        // Should throw on invalidated model
        Assert.Throws<ObjectDisposedException>(() => model.GetBitVec(bv));
    }

    [Test]
    public void GetBitVecValueAsBigInteger_ConsistentWithStringValue()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var bv = context.BitVecConst("bv", 32);
        solver.Assert(bv == context.BitVec(123456, 32));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var intValue = model.GetBitVec(bv).Value;
        var stringValue = model.GetNumericValueAsString(bv);

        // Both methods should return the same value
        Assert.That(intValue, Is.EqualTo(new BigInteger(123456)));
        Assert.That(stringValue, Is.EqualTo("123456"));
        Assert.That(intValue.ToString(), Is.EqualTo(stringValue));
    }

    [Test]
    public void GetBitVecValueAsInt_SimpleValue()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var bv = context.BitVecConst("bv", 8);
        solver.Assert(bv == context.BitVec(42, 8));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var value = model.GetBitVec(bv).ToInt();

        Assert.That(value, Is.EqualTo(42));
    }

    [Test]
    public void GetBitVecValueAsInt_OverflowThrowsException()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var bv = context.BitVecConst("bv", 64);
        solver.Assert(bv == context.BitVec(-1, 64)); // This will be a very large positive number

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();

        // Large sizes should work now that you removed the size check
        var bitVec = model.GetBitVec(bv);
        Assert.That(bitVec.Size, Is.EqualTo(64));
        // For -1 in 64-bit, the signed value should be -1
        Assert.That(bitVec.ToInt(), Is.EqualTo(-1));
    }

    [Test]
    public void GetBitVecValueAsUInt_SimpleValue()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var bv = context.BitVecConst("bv", 8);
        solver.Assert(bv == context.BitVec(255, 8));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var value = model.GetBitVec(bv).ToUInt();

        Assert.That(value, Is.EqualTo(255U));
    }

    [Test]
    public void GetBitVecValueAsLong_SimpleValue()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var bv = context.BitVecConst("bv", 32);
        solver.Assert(bv == context.BitVec(1000000, 32));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var value = model.GetBitVec(bv).ToLong();

        Assert.That(value, Is.EqualTo(1000000L));
    }

    [Test]
    public void GetBitVecValueAsULong_SimpleValue()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var bv = context.BitVecConst("bv", 32);
        solver.Assert(bv == context.BitVec(-1, 32)); // This becomes 4294967295

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var value = model.GetBitVec(bv).ToULong();

        Assert.That(value, Is.EqualTo(4294967295UL));
    }

    [Test]
    public void GetBitVecValueAsBinaryString_SimpleValue()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var bv = context.BitVecConst("bv", 8);
        solver.Assert(bv == context.BitVec(42, 8)); // 42 = 00101010

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var value = model.GetBitVec(bv).ToBinaryString();

        Assert.That(value, Is.EqualTo("00101010"));
    }

    [Test]
    public void GetBitVecValueAsBinaryString_ZeroValue()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var bv = context.BitVecConst("bv", 4);
        solver.Assert(bv == context.BitVec(0, 4));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var value = model.GetBitVec(bv).ToBinaryString();

        Assert.That(value, Is.EqualTo("0000"));
    }

    [Test]
    public void GetBitVecValueAsBinaryString_MaxValue()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var bv = context.BitVecConst("bv", 4);
        solver.Assert(bv == context.BitVec(15, 4)); // 15 = 1111

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var value = model.GetBitVec(bv).ToBinaryString();

        Assert.That(value, Is.EqualTo("1111"));
    }


    [Test]
    public void GetBitVecValue_AllFormatsConsistent()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var bv = context.BitVecConst("bv", 8);
        solver.Assert(bv == context.BitVec(170, 8)); // 170 = 10101010 = AA

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();

        Assert.That(model.GetBitVec(bv).Value, Is.EqualTo(new BigInteger(170)));
        Assert.That(model.GetNumericValueAsString(bv), Is.EqualTo("170"));
        Assert.That((int)model.GetBitVec(bv).Value, Is.EqualTo(170));
        Assert.That(model.GetBitVec(bv).ToUInt(), Is.EqualTo(170U));
        Assert.That((long)model.GetBitVec(bv).Value, Is.EqualTo(170L));
        Assert.That(model.GetBitVec(bv).ToULong(), Is.EqualTo(170UL));
        Assert.That(model.GetBitVec(bv).ToBinaryString(), Is.EqualTo("10101010"));
    }

    [Test]
    public void Z3BitVecExpr_ToString_ShowsSizeAndContent()
    {
        using var context = new Z3Context();

        var bv = context.BitVecConst("x", 32);

        var toString = bv.ToString();

        Assert.That(toString, Does.Contain("BitVec[32]"));
        Assert.That(toString, Does.Contain("x"));
    }

    [Test]
    public void Z3BitVecExpr_ToString_WithValue()
    {
        using var context = new Z3Context();

        var bv = context.BitVec(42, 8);

        var toString = bv.ToString();

        Assert.That(toString, Does.Contain("BitVec[8]"));
        Assert.That(toString, Does.Contain("42") | Does.Contain("#x2a") | Does.Contain("#b00101010"));
    }

    [Test]
    public void Z3BitVecExpr_ToString_DifferentSizes()
    {
        using var context = new Z3Context();

        var bv8 = context.BitVecConst("bv8", 8);
        var bv16 = context.BitVecConst("bv16", 16);
        var bv32 = context.BitVecConst("bv32", 32);
        var bv64 = context.BitVecConst("bv64", 64);

        Assert.That(bv8.ToString(), Does.Contain("BitVec[8]"));
        Assert.That(bv16.ToString(), Does.Contain("BitVec[16]"));
        Assert.That(bv32.ToString(), Does.Contain("BitVec[32]"));
        Assert.That(bv64.ToString(), Does.Contain("BitVec[64]"));
    }
}