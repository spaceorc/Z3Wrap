using z3lib;

namespace tests;

public class Z3ExtendedOperationsTests
{
    [Test]
    public void ImpliesOperation_FactoryMethod_Works()
    {
        using var context = new Z3Context();
        var p = context.MkBoolConst("p");
        var q = context.MkBoolConst("q");
        
        var implies = context.MkImplies(p, q);
        
        Assert.That(implies.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(implies.Context, Is.SameAs(context));
    }

    [Test]
    public void ImpliesOperation_InstanceMethod_Works()
    {
        using var context = new Z3Context();
        var p = context.MkBoolConst("p");
        var q = context.MkBoolConst("q");
        
        var implies = p.Implies(q);
        
        Assert.That(implies.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(implies.Context, Is.SameAs(context));
    }

    [Test]
    public void IffOperation_FactoryMethod_Works()
    {
        using var context = new Z3Context();
        var p = context.MkBoolConst("p");
        var q = context.MkBoolConst("q");
        
        var iff = context.MkIff(p, q);
        
        Assert.That(iff.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(iff.Context, Is.SameAs(context));
    }

    [Test]
    public void IffOperation_InstanceMethod_Works()
    {
        using var context = new Z3Context();
        var p = context.MkBoolConst("p");
        var q = context.MkBoolConst("q");
        
        var iff = p.Iff(q);
        
        Assert.That(iff.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(iff.Context, Is.SameAs(context));
    }

    [Test]
    public void XorOperation_FactoryMethod_Works()
    {
        using var context = new Z3Context();
        var p = context.MkBoolConst("p");
        var q = context.MkBoolConst("q");
        
        var xor = context.MkXor(p, q);
        
        Assert.That(xor.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(xor.Context, Is.SameAs(context));
    }

    [Test]
    public void XorOperation_InstanceMethod_Works()
    {
        using var context = new Z3Context();
        var p = context.MkBoolConst("p");
        var q = context.MkBoolConst("q");
        
        var xor = p.Xor(q);
        
        Assert.That(xor.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(xor.Context, Is.SameAs(context));
    }

    [Test]
    public void ModOperation_FactoryMethod_Works()
    {
        using var context = new Z3Context();
        var x = context.MkIntConst("x");
        var y = context.MkIntConst("y");
        
        var mod = context.MkMod(x, y);
        
        Assert.That(mod.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(mod.Context, Is.SameAs(context));
    }

    [Test]
    public void ModOperation_InstanceMethod_Works()
    {
        using var context = new Z3Context();
        var x = context.MkIntConst("x");
        var y = context.MkIntConst("y");
        
        var mod = x.Mod(y);
        
        Assert.That(mod.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(mod.Context, Is.SameAs(context));
    }

    [Test]
    public void AbsOperation_IntFactoryMethod_Works()
    {
        using var context = new Z3Context();
        var x = context.MkIntConst("x");
        
        var abs = context.MkAbs(x);
        
        Assert.That(abs.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(abs.Context, Is.SameAs(context));
    }

    [Test]
    public void AbsOperation_IntInstanceMethod_Works()
    {
        using var context = new Z3Context();
        var x = context.MkIntConst("x");
        
        var abs = x.Abs();
        
        Assert.That(abs.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(abs.Context, Is.SameAs(context));
    }

    [Test]
    public void AbsOperation_RealFactoryMethod_Works()
    {
        using var context = new Z3Context();
        var x = context.MkRealConst("x");
        
        var abs = context.MkAbs(x);
        
        Assert.That(abs.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(abs.Context, Is.SameAs(context));
    }

    [Test]
    public void AbsOperation_RealInstanceMethod_Works()
    {
        using var context = new Z3Context();
        var x = context.MkRealConst("x");
        
        var abs = x.Abs();
        
        Assert.That(abs.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(abs.Context, Is.SameAs(context));
    }

    [Test]
    public void ExtendedBooleanOperations_WithSolver_Work()
    {
        using var context = new Z3Context();
        var p = context.MkBoolConst("p");
        var q = context.MkBoolConst("q");
        var r = context.MkBoolConst("r");

        using var solver = context.MkSolver();

        // Test implies: p => q (if p is true, q must be true)
        solver.Push();
        solver.Assert(p);
        solver.Assert(p.Implies(q));
        var result1 = solver.Check();
        if (result1 == Z3Status.Satisfiable)
        {
            var model = solver.GetModel();
            var pVal = model.GetBoolValue(p);
            var qVal = model.GetBoolValue(q);
            Assert.That(pVal == Z3BoolValue.False || qVal == Z3BoolValue.True, Is.True,
                "If p is true, q must be true in implies relation");
        }
        solver.Pop();

        // Test iff: p <=> q (p and q must have same truth value)
        solver.Push();
        solver.Assert(p.Iff(q));
        solver.Assert(p);
        var result2 = solver.Check();
        if (result2 == Z3Status.Satisfiable)
        {
            var model = solver.GetModel();
            var pVal = model.GetBoolValue(p);
            var qVal = model.GetBoolValue(q);
            Assert.That(pVal, Is.EqualTo(qVal), "In iff relation, p and q must have same truth value");
        }
        solver.Pop();

        // Test xor: p ⊕ q (exactly one of p or q is true)
        solver.Push();
        solver.Assert(p.Xor(q));
        var result3 = solver.Check();
        if (result3 == Z3Status.Satisfiable)
        {
            var model = solver.GetModel();
            var pVal = model.GetBoolValue(p);
            var qVal = model.GetBoolValue(q);
            Assert.That(pVal != qVal, Is.True, "In xor relation, p and q must have different truth values");
        }
        solver.Pop();
    }

    [Test]
    public void ExtendedArithmeticOperations_WithSolver_Work()
    {
        using var context = new Z3Context();
        var x = context.MkIntConst("x");
        var y = context.MkIntConst("y");

        using var solver = context.MkSolver();

        // Test modulo: x % y
        solver.Push();
        solver.Assert(x == context.MkInt(17));
        solver.Assert(y == context.MkInt(5));
        solver.Assert(x.Mod(y) == context.MkInt(2));
        var result1 = solver.Check();
        Assert.That(result1, Is.EqualTo(Z3Status.Satisfiable), "17 % 5 == 2 should be satisfiable");
        solver.Pop();

        // Test absolute value with negative number
        solver.Push();
        solver.Assert(x == context.MkInt(-10));
        solver.Assert(x.Abs() == context.MkInt(10));
        var result2 = solver.Check();
        Assert.That(result2, Is.EqualTo(Z3Status.Satisfiable), "abs(-10) == 10 should be satisfiable");
        solver.Pop();

        // Test absolute value with positive number
        solver.Push();
        solver.Assert(x == context.MkInt(7));
        solver.Assert(x.Abs() == context.MkInt(7));
        var result3 = solver.Check();
        Assert.That(result3, Is.EqualTo(Z3Status.Satisfiable), "abs(7) == 7 should be satisfiable");
        solver.Pop();
    }

    [Test]
    public void ExtendedRealArithmeticOperations_WithSolver_Work()
    {
        using var context = new Z3Context();
        var x = context.MkRealConst("x");

        using var solver = context.MkSolver();

        // Test absolute value with negative real
        solver.Push();
        solver.Assert(x == context.MkReal(-3.5));
        solver.Assert(x.Abs() == context.MkReal(3.5));
        var result1 = solver.Check();
        Assert.That(result1, Is.EqualTo(Z3Status.Satisfiable), "abs(-3.5) == 3.5 should be satisfiable");
        solver.Pop();

        // Test absolute value with positive real
        solver.Push();
        solver.Assert(x == context.MkReal(2.7));
        solver.Assert(x.Abs() == context.MkReal(2.7));
        var result2 = solver.Check();
        Assert.That(result2, Is.EqualTo(Z3Status.Satisfiable), "abs(2.7) == 2.7 should be satisfiable");
        solver.Pop();
    }

    [Test]
    public void ComplexExtendedOperations_CombinedUse_Work()
    {
        using var context = new Z3Context();
        var p = context.MkBoolConst("p");
        var q = context.MkBoolConst("q");
        var x = context.MkIntConst("x");
        var y = context.MkIntConst("y");

        using var solver = context.MkSolver();

        // Simplified test focusing on the core extended operations
        solver.Assert(x == context.MkInt(15));
        solver.Assert(y == context.MkInt(5));
        
        // Test modulo operation: 15 % 5 should be 0
        solver.Assert(x.Mod(y) == context.MkInt(0));
        
        // Test absolute value: abs(15) should be 15
        solver.Assert(x.Abs() == context.MkInt(15));
        
        // Test boolean operations with explicit values
        solver.Assert(p == context.MkTrue());
        solver.Assert(q == context.MkFalse());
        solver.Assert(p.Xor(q)); // true XOR false should be true

        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable), "Complex constraint should be satisfiable");

        if (result == Z3Status.Satisfiable)
        {
            var model = solver.GetModel();
            var xVal = model.GetIntValue(x);
            var yVal = model.GetIntValue(y);
            var pVal = model.GetBoolValue(p);
            var qVal = model.GetBoolValue(q);

            Assert.That(xVal, Is.EqualTo(15));
            Assert.That(yVal, Is.EqualTo(5));
            Assert.That(pVal, Is.EqualTo(Z3BoolValue.True));
            Assert.That(qVal, Is.EqualTo(Z3BoolValue.False).Or.EqualTo(Z3BoolValue.Undefined));
            
            // Test that modulo expression evaluates correctly
            var modExpr = x.Mod(y);
            var modVal = model.GetIntValue(modExpr);
            Assert.That(modVal, Is.EqualTo(0), "15 % 5 should equal 0");
            
            // Test that abs expression evaluates correctly
            var absExpr = x.Abs();
            var absVal = model.GetIntValue(absExpr);
            Assert.That(absVal, Is.EqualTo(15), "abs(15) should equal 15");
        }
    }

    [Test]
    public void ExtendedOperations_ToStringNeverThrows()
    {
        using var context = new Z3Context();
        var p = context.MkBoolConst("p");
        var q = context.MkBoolConst("q");
        var x = context.MkIntConst("x");
        var y = context.MkIntConst("y");
        var z = context.MkRealConst("z");

        // All extended boolean operations should have working ToString
        Assert.DoesNotThrow(() => p.Implies(q).ToString());
        Assert.DoesNotThrow(() => p.Iff(q).ToString());
        Assert.DoesNotThrow(() => p.Xor(q).ToString());

        // All extended arithmetic operations should have working ToString
        Assert.DoesNotThrow(() => x.Mod(y).ToString());
        Assert.DoesNotThrow(() => x.Abs().ToString());
        Assert.DoesNotThrow(() => z.Abs().ToString());

        // ToString should return non-empty strings
        Assert.That(p.Implies(q).ToString(), Is.Not.Empty);
        Assert.That(p.Iff(q).ToString(), Is.Not.Empty);
        Assert.That(p.Xor(q).ToString(), Is.Not.Empty);
        Assert.That(x.Mod(y).ToString(), Is.Not.Empty);
        Assert.That(x.Abs().ToString(), Is.Not.Empty);
        Assert.That(z.Abs().ToString(), Is.Not.Empty);
    }

    [Test]
    public void ExtendedOperations_AfterContextDisposal_ToStringIsSafe()
    {
        Z3BoolExpr? implies = null;
        Z3BoolExpr? iff = null;
        Z3BoolExpr? xor = null;
        Z3IntExpr? mod = null;
        Z3IntExpr? absInt = null;
        Z3RealExpr? absReal = null;

        using (var context = new Z3Context())
        {
            var p = context.MkBoolConst("p");
            var q = context.MkBoolConst("q");
            var x = context.MkIntConst("x");
            var y = context.MkIntConst("y");
            var z = context.MkRealConst("z");

            implies = p.Implies(q);
            iff = p.Iff(q);
            xor = p.Xor(q);
            mod = x.Mod(y);
            absInt = x.Abs();
            absReal = z.Abs();
        } // Context disposed here

        // ToString should not throw after context disposal
        Assert.DoesNotThrow(() => implies.ToString());
        Assert.DoesNotThrow(() => iff.ToString());
        Assert.DoesNotThrow(() => xor.ToString());
        Assert.DoesNotThrow(() => mod.ToString());
        Assert.DoesNotThrow(() => absInt.ToString());
        Assert.DoesNotThrow(() => absReal.ToString());

        // Should return disposed indicator strings
        Assert.That(implies.ToString(), Contains.Substring("disposed"));
        Assert.That(iff.ToString(), Contains.Substring("disposed"));
        Assert.That(xor.ToString(), Contains.Substring("disposed"));
        Assert.That(mod.ToString(), Contains.Substring("disposed"));
        Assert.That(absInt.ToString(), Contains.Substring("disposed"));
        Assert.That(absReal.ToString(), Contains.Substring("disposed"));
    }

    [Test]
    public void ModuloOperator_IntExpr_Works()
    {
        using var context = new Z3Context();
        var x = context.MkIntConst("x");
        var y = context.MkIntConst("y");
        
        // Test % operator with Z3IntExpr operands
        var mod1 = x % y;
        Assert.That(mod1.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(mod1.Context, Is.SameAs(context));
        
        // Test % operator with int operand (right)
        var mod2 = x % 5;
        Assert.That(mod2.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(mod2.Context, Is.SameAs(context));
        
        // Test % operator with int operand (left)
        var mod3 = 17 % x;
        Assert.That(mod3.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(mod3.Context, Is.SameAs(context));
    }

    [Test]
    public void UnaryMinusOperator_IntExpr_Works()
    {
        using var context = new Z3Context();
        var x = context.MkIntConst("x");
        
        var negX = -x;
        Assert.That(negX.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(negX.Context, Is.SameAs(context));
    }

    [Test]
    public void UnaryMinusOperator_RealExpr_Works()
    {
        using var context = new Z3Context();
        var x = context.MkRealConst("x");
        
        var negX = -x;
        Assert.That(negX.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(negX.Context, Is.SameAs(context));
    }

    [Test]
    public void NewOperators_WithSolver_Work()
    {
        using var context = new Z3Context();
        var x = context.MkIntConst("x");
        var y = context.MkRealConst("y");

        using var solver = context.MkSolver();

        // Test modulo operator: x % 7 == 3 with x = 17
        solver.Push();
        solver.Assert(x == context.MkInt(17));
        solver.Assert(x % 7 == context.MkInt(3));
        var result1 = solver.Check();
        Assert.That(result1, Is.EqualTo(Z3Status.Satisfiable), "17 % 7 == 3 should be satisfiable");
        solver.Pop();

        // Test unary minus operator with integers: -x == -5 with x = 5
        solver.Push();
        solver.Assert(x == context.MkInt(5));
        solver.Assert(-x == context.MkInt(-5));
        var result2 = solver.Check();
        Assert.That(result2, Is.EqualTo(Z3Status.Satisfiable), "-5 == -5 should be satisfiable");
        solver.Pop();

        // Test unary minus operator with reals: -y == -3.14 with y = 3.14
        solver.Push();
        solver.Assert(y == context.MkReal(3.14));
        solver.Assert(-y == context.MkReal(-3.14));
        var result3 = solver.Check();
        Assert.That(result3, Is.EqualTo(Z3Status.Satisfiable), "-3.14 == -3.14 should be satisfiable");
        solver.Pop();

        // Test complex expression with new operators: -(x % 10) == -7 with x = 17
        solver.Push();
        solver.Assert(x == context.MkInt(17));
        solver.Assert(-(x % 10) == context.MkInt(-7));
        var result4 = solver.Check();
        Assert.That(result4, Is.EqualTo(Z3Status.Satisfiable), "-(17 % 10) == -7 should be satisfiable");
        if (result4 == Z3Status.Satisfiable)
        {
            var model = solver.GetModel();
            var xVal = model.GetIntValue(x);
            var modVal = model.GetIntValue(x % 10);
            var negModVal = model.GetIntValue(-(x % 10));
            
            Assert.That(xVal, Is.EqualTo(17));
            Assert.That(modVal, Is.EqualTo(7));
            Assert.That(negModVal, Is.EqualTo(-7));
        }
        solver.Pop();
    }

    [Test]
    public void NewOperators_ToStringWorks()
    {
        using var context = new Z3Context();
        var x = context.MkIntConst("x");
        var y = context.MkIntConst("y");
        var z = context.MkRealConst("z");

        // Test ToString for modulo operators
        Assert.DoesNotThrow(() => (x % y).ToString());
        Assert.DoesNotThrow(() => (x % 5).ToString());
        Assert.DoesNotThrow(() => (17 % x).ToString());

        // Test ToString for unary minus operators
        Assert.DoesNotThrow(() => (-x).ToString());
        Assert.DoesNotThrow(() => (-z).ToString());

        // Verify non-empty strings
        Assert.That((x % y).ToString(), Is.Not.Empty);
        Assert.That((x % 5).ToString(), Is.Not.Empty);
        Assert.That((-x).ToString(), Is.Not.Empty);
        Assert.That((-z).ToString(), Is.Not.Empty);
    }

    [Test]
    public void NeqHelperFunction_Works()
    {
        using var context = new Z3Context();
        var x = context.MkIntConst("x");
        var y = context.MkIntConst("y");

        // Test Neq helper function
        var neq = x.Neq(y);
        Assert.That(neq.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(neq.Context, Is.SameAs(context));

        // Test with solver to ensure it works correctly
        using var solver = context.MkSolver();
        solver.Assert(x == context.MkInt(5));
        solver.Assert(y == context.MkInt(3));
        solver.Assert(x.Neq(y)); // x != y should be true since 5 != 3

        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable), "5 != 3 should be satisfiable");

        if (result == Z3Status.Satisfiable)
        {
            var model = solver.GetModel();
            var xVal = model.GetIntValue(x);
            var yVal = model.GetIntValue(y);
            
            Assert.That(xVal, Is.EqualTo(5));
            Assert.That(yVal, Is.EqualTo(3));
            Assert.That(xVal, Is.Not.EqualTo(yVal));
        }
    }

    [Test]
    public void NeqHelperFunction_ConsistentWithOperator()
    {
        using var context = new Z3Context();
        var x = context.MkIntConst("x");
        var y = context.MkIntConst("y");

        using var solver = context.MkSolver();
        
        // Both x.Neq(y) and x != y should be equivalent
        solver.Push();
        solver.Assert(x == context.MkInt(7));
        solver.Assert(y == context.MkInt(7));
        solver.Assert(x.Neq(y)); // This should be false (7 == 7)
        var result1 = solver.Check();
        Assert.That(result1, Is.EqualTo(Z3Status.Unsatisfiable), "x.Neq(y) with x=y=7 should be unsatisfiable");
        solver.Pop();

        solver.Push();
        solver.Assert(x == context.MkInt(7));
        solver.Assert(y == context.MkInt(7));
        solver.Assert(x != y); // This should also be false (7 == 7)
        var result2 = solver.Check();
        Assert.That(result2, Is.EqualTo(Z3Status.Unsatisfiable), "x != y with x=y=7 should be unsatisfiable");
        solver.Pop();

        // Both should be equivalent in satisfiable case too
        solver.Push();
        solver.Assert(x == context.MkInt(5));
        solver.Assert(y == context.MkInt(3));
        solver.Assert(x.Neq(y));
        var result3 = solver.Check();
        Assert.That(result3, Is.EqualTo(Z3Status.Satisfiable), "x.Neq(y) with x=5, y=3 should be satisfiable");
        solver.Pop();
    }
    
    [Test]
    public void IfThenElse_IntegerExpressions_ReturnsCorrectType()
    {
        using var context = new Z3Context();
        var x = context.MkIntConst("x");
        var y = context.MkIntConst("y");
        var condition = x > context.MkInt(0);
        
        var result = condition.If(x, y);
        
        Assert.That(result, Is.TypeOf<Z3IntExpr>());
        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));
    }
    
    [Test]
    public void IfThenElse_RealExpressions_ReturnsCorrectType()
    {
        using var context = new Z3Context();
        var a = context.MkRealConst("a");
        var b = context.MkRealConst("b");
        var condition = a > context.MkReal(0.0);
        
        var result = condition.If(a, b);
        
        Assert.That(result, Is.TypeOf<Z3RealExpr>());
        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));
    }
    
    [Test]
    public void IfThenElse_BooleanExpressions_ReturnsCorrectType()
    {
        using var context = new Z3Context();
        var p = context.MkBoolConst("p");
        var q = context.MkBoolConst("q");
        var r = context.MkBoolConst("r");
        
        var result = p.If(q, r);
        
        Assert.That(result, Is.TypeOf<Z3BoolExpr>());
        Assert.That(result.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(result.Context, Is.SameAs(context));
    }
    
    [Test]
    public void IfThenElse_SolverIntegration_Works()
    {
        using var context = new Z3Context();
        var x = context.MkIntConst("x");
        var y = context.MkIntConst("y");
        var condition = x > context.MkInt(0);
        
        var result = condition.If(context.MkInt(10), context.MkInt(-5));
        
        using var solver = context.MkSolver();
        solver.Assert(x == context.MkInt(3));
        solver.Assert(y == result);
        
        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
        
        if (status == Z3Status.Satisfiable)
        {
            var model = solver.GetModel();
            var yValue = model.GetIntValue(y);
            Assert.That(yValue, Is.EqualTo(10)); // x > 0, so result should be 10
        }
    }
    
    [Test]
    public void IfThenElse_TypeSafety_CompileTimeChecked()
    {
        using var context = new Z3Context();
        var x = context.MkIntConst("x");
        var y = context.MkIntConst("y");
        var condition = x > context.MkInt(0);
        
        // This should compile and return Z3IntExpr without casting
        Z3IntExpr result = condition.If(x, y);
        
        // Should be able to call integer-specific methods immediately
        var doubled = result.Add(result);
        
        Assert.That(doubled, Is.TypeOf<Z3IntExpr>());
    }
}