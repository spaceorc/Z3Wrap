using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Expressions.Numerics;

namespace Z3Wrap.Tests.Unit.Expressions.Z3BoolExprTests;

[TestFixture]
public class Z3BoolExprCreationTests
{
    [TestCase(true, Description = "Create Bool(true)")]
    [TestCase(false, Description = "Create Bool(false)")]
    public void CreateBool_WithLiteralValues_ReturnsCorrectExpression(bool value)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var boolExpr = context.Bool(value);

        Assert.Multiple(() =>
        {
            Assert.That(boolExpr, Is.Not.Null);
            Assert.That(boolExpr, Is.TypeOf<BoolExpr>());
            Assert.That(boolExpr.Handle, Is.Not.EqualTo(IntPtr.Zero));
            Assert.That(boolExpr.Context, Is.SameAs(context));
        });

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(boolExpr), Is.EqualTo(value));
    }

    [Test]
    public void CreateBoolConst_WithVariableName_ReturnsCorrectExpression()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var boolConst = context.BoolConst("testVariable");

        Assert.Multiple(() =>
        {
            Assert.That(boolConst, Is.Not.Null);
            Assert.That(boolConst, Is.TypeOf<BoolExpr>());
            Assert.That(boolConst.Handle, Is.Not.EqualTo(IntPtr.Zero));
            Assert.That(boolConst.Context, Is.SameAs(context));
        });
    }

    [TestCase("p", Description = "Single character variable")]
    [TestCase("condition", Description = "Descriptive variable name")]
    [TestCase("isValid123", Description = "Variable with numbers")]
    [TestCase("test_var", Description = "Variable with underscore")]
    public void CreateBoolConst_WithVariousNames_Works(string variableName)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var boolConst = context.BoolConst(variableName);

        Assert.Multiple(() =>
        {
            Assert.That(boolConst, Is.Not.Null);
            Assert.That(boolConst, Is.TypeOf<BoolExpr>());
            Assert.That(boolConst.Handle, Is.Not.EqualTo(IntPtr.Zero));
        });

        // Test that we can use the variable in constraints
        solver.Assert(boolConst);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var value = model.GetBoolValue(boolConst);
        Assert.That(value, Is.TypeOf<bool>());
    }

    [TestCase(true, Description = "Implicit conversion from true")]
    [TestCase(false, Description = "Implicit conversion from false")]
    public void ImplicitConversion_FromBoolToZ3BoolExpr_Works(bool value)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Test implicit conversion using thread-local context
        BoolExpr implicitExpr = value;

        Assert.Multiple(() =>
        {
            Assert.That(implicitExpr, Is.Not.Null);
            Assert.That(implicitExpr, Is.TypeOf<BoolExpr>());
            Assert.That(implicitExpr.Handle, Is.Not.EqualTo(IntPtr.Zero));
            Assert.That(implicitExpr.Context, Is.SameAs(context));
        });

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(implicitExpr), Is.EqualTo(value));
    }

    [Test]
    public void CreateBoolConstants_TrueAndFalse_Work()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var trueExpr = context.True();
        var falseExpr = context.False();

        Assert.Multiple(() =>
        {
            Assert.That(trueExpr, Is.Not.Null);
            Assert.That(trueExpr, Is.TypeOf<BoolExpr>());
            Assert.That(falseExpr, Is.Not.Null);
            Assert.That(falseExpr, Is.TypeOf<BoolExpr>());
        });

        solver.Assert(trueExpr);
        solver.Assert(!falseExpr);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(trueExpr), Is.True);
            Assert.That(model.GetBoolValue(falseExpr), Is.False);
        });
    }

    [Test]
    public void CreateBoolFromHandle_WithValidHandle_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var originalBool = context.Bool(true);
        var handle = originalBool.Handle;

        var recreatedBool = Z3Expr.Create<BoolExpr>(context, handle);

        Assert.Multiple(() =>
        {
            Assert.That(recreatedBool, Is.Not.Null);
            Assert.That(recreatedBool, Is.TypeOf<BoolExpr>());
            Assert.That(recreatedBool.Handle, Is.EqualTo(handle));
            Assert.That(recreatedBool.Context, Is.SameAs(context));
        });
    }

    [Test]
    public void CreateMultipleBoolConstants_UniqueInstances_Work()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var bool1 = context.BoolConst("var1");
        var bool2 = context.BoolConst("var2");
        var bool3 = context.BoolConst("var3");

        Assert.Multiple(() =>
        {
            Assert.That(bool1.Handle, Is.Not.EqualTo(bool2.Handle));
            Assert.That(bool2.Handle, Is.Not.EqualTo(bool3.Handle));
            Assert.That(bool1.Handle, Is.Not.EqualTo(bool3.Handle));
        });

        // Test that they can have independent values
        solver.Assert(bool1);
        solver.Assert(!bool2);
        solver.Assert(bool3);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();

        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(bool1), Is.True);
            Assert.That(model.GetBoolValue(bool2), Is.False);
            Assert.That(model.GetBoolValue(bool3), Is.True);
        });
    }

    [Test]
    public void CreateBoolFromCompoundExpression_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var y = context.IntConst("y");

        // Create bool expressions from comparisons
        var greaterThan = x > y;
        var lessThan = x < y;
        var equals = x == y;

        Assert.Multiple(() =>
        {
            Assert.That(greaterThan, Is.TypeOf<BoolExpr>());
            Assert.That(lessThan, Is.TypeOf<BoolExpr>());
            Assert.That(equals, Is.TypeOf<BoolExpr>());
        });

        solver.Assert(x == context.Int(10));
        solver.Assert(y == context.Int(5));
        solver.Assert(greaterThan);
        solver.Assert(!lessThan);
        solver.Assert(!equals);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void BoolConstReuseWithSameName_ReturnsSameHandle()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var bool1 = context.BoolConst("sameName");
        var bool2 = context.BoolConst("sameName");

        // Z3 should return the same expression for the same name
        Assert.That(bool1.Handle, Is.EqualTo(bool2.Handle));
    }

    [Test]
    public void BoolCreation_InDifferentContexts_ProducesIndependentVariables()
    {
        var bool1Handle = IntPtr.Zero;
        var bool2Handle = IntPtr.Zero;

        var contexts = new List<IntPtr>();

        var context1 = new Z3Context();
        var context2 = new Z3Context();
        using (context1)
        {
            contexts.Add(context1.Handle);
            using var scope1 = context1.SetUp();
            var bool1 = context1.BoolConst("test");
            bool1Handle = bool1.Handle;
        }

        using (context2)
        {
            contexts.Add(context2.Handle);
            using var scope2 = context2.SetUp();
            var bool2 = context2.BoolConst("test");
            bool2Handle = bool2.Handle;
        }

        // Different contexts should produce different handles even for same name
        Assert.That(bool1Handle, Is.Not.EqualTo(bool2Handle));
    }

    [Test]
    public void BoolCreation_ThreadLocalContext_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Test that implicit conversion works with thread-local context
        BoolExpr trueExpr = true;
        BoolExpr falseExpr = false;

        var p = context.BoolConst("p");

        // Use implicit conversions in operations
        var expr1 = p & true; // Should work with implicit conversion
        var expr2 = false | p; // Should work with implicit conversion

        solver.Assert(p);
        solver.Assert(expr1); // p & true = p (true)
        solver.Assert(expr2); // false | p = p (true)

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void BoolCreation_WithComplexExpressionBuilding_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Build complex expressions step by step
        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        var r = context.BoolConst("r");

        var step1 = p & q; // Conjunction
        var step2 = step1 | r; // Add disjunction
        var step3 = !step2; // Negate the whole thing
        var step4 = step3.Implies(p); // Add implication
        var final = step4.Iff(context.Bool(true)); // Final equivalence

        Assert.Multiple(() =>
        {
            Assert.That(step1, Is.TypeOf<BoolExpr>());
            Assert.That(step2, Is.TypeOf<BoolExpr>());
            Assert.That(step3, Is.TypeOf<BoolExpr>());
            Assert.That(step4, Is.TypeOf<BoolExpr>());
            Assert.That(final, Is.TypeOf<BoolExpr>());
        });

        solver.Assert(final);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void BoolCreation_HandleValidation_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var boolExpr = context.Bool(true);

        // Verify that handles are valid (non-zero)
        Assert.That(boolExpr.Handle, Is.Not.EqualTo(IntPtr.Zero));

        // Verify that the expression maintains its handle throughout operations
        var negated = !boolExpr;
        Assert.That(negated.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(negated.Handle, Is.Not.EqualTo(boolExpr.Handle)); // Should be different

        var combined = boolExpr & negated;
        Assert.That(combined.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void BoolCreation_ContextAssociation_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var boolLiteral = context.Bool(true);
        var boolConst = context.BoolConst("test");
        var boolFromComparison = context.Int(5) > context.Int(3);

        // All should be associated with the same context
        Assert.Multiple(() =>
        {
            Assert.That(boolLiteral.Context, Is.SameAs(context));
            Assert.That(boolConst.Context, Is.SameAs(context));
            Assert.That(boolFromComparison.Context, Is.SameAs(context));
        });
    }

    [Test]
    public void BoolCreation_EdgeCases_Work()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Test with empty string variable name (if allowed)
        var emptyNameVar = context.BoolConst("");
        Assert.That(emptyNameVar, Is.Not.Null);

        // Test creating many variables
        var manyVars = new List<BoolExpr>();
        for (int i = 0; i < 100; i++)
        {
            manyVars.Add(context.BoolConst($"var_{i}"));
        }

        Assert.That(manyVars, Has.Count.EqualTo(100));
        Assert.That(manyVars.Select(v => v.Handle).ToHashSet(), Has.Count.EqualTo(100)); // All unique handles

        // Test that we can use all variables
        var allTrue = manyVars.Aggregate((a, b) => a & b);
        solver.Assert(allTrue);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void BoolCreation_MemoryManagement_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        // Create many expressions to test that memory is handled properly
        var expressions = new List<BoolExpr>();

        for (int i = 0; i < 1000; i++)
        {
            var expr = context.Bool(i % 2 == 0);
            var constExpr = context.BoolConst($"const_{i}");
            var combinedExpr = expr & constExpr;

            expressions.Add(combinedExpr);
        }

        // All expressions should be valid
        Assert.That(expressions.All(e => e.Handle != IntPtr.Zero), Is.True);

        // Test that they can still be used
        using var solver = context.CreateSolver();
        var someExpr = expressions[0] | expressions[500] | expressions[999];
        solver.Assert(someExpr);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }
}
