using z3lib;

namespace tests;

public class Z3ExpressionTests
{

    [Test]
    public void CanCreateBooleanConstants()
    {
        using var context = new Z3Context();
        var trueExpr = context.True();
        var falseExpr = context.False();

        Assert.That(trueExpr.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(falseExpr.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void CanCreateBooleanFromBool()
    {
        using var context = new Z3Context();
        var trueExpr = context.Bool(true);
        var falseExpr = context.Bool(false);

        Assert.That(trueExpr.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(falseExpr.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(trueExpr.Context, Is.SameAs(context));
        Assert.That(falseExpr.Context, Is.SameAs(context));
    }

    [Test]
    public void CanCreateIntegerConstants()
    {
        using var context = new Z3Context();
        var five = context.Int(5);
        var negTen = context.Int(-10);

        Assert.That(five.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(negTen.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void CanCreateIntegerVariables()
    {
        using var context = new Z3Context();
        var x = context.IntConst("x");
        var y = context.IntConst("y");

        Assert.That(x.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(y.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void CanCreateRealConstants()
    {
        using var context = new Z3Context();
        var pi = context.Real(3.14159);
        var half = context.Real(0.5);

        Assert.That(pi.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(half.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void CanCreateRealVariables()
    {
        using var context = new Z3Context();
        var x = context.RealConst("x");
        var y = context.RealConst("y");

        Assert.That(x.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(y.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void CanUseBooleanOperators()
    {
        using var context = new Z3Context();
        var p = context.True();
        var q = context.False();

        var andExpr = p & q;
        var orExpr = p | q;
        var notExpr = !p;

        Assert.That(andExpr.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(orExpr.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(notExpr.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void CanUseIntegerArithmeticOperators()
    {
        using var context = new Z3Context();
        var five = context.Int(5);
        var three = context.Int(3);

        var sum = five + three;
        var diff = five - three;
        var prod = five * three;
        var quot = five / three;

        Assert.That(sum.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(diff.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(prod.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(quot.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void CanUseIntegerComparisonOperators()
    {
        using var context = new Z3Context();
        var five = context.Int(5);
        var three = context.Int(3);

        var lt = five < three;
        var le = five <= three;
        var gt = five > three;
        var ge = five >= three;
        var eq = five == three;

        Assert.That(lt.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(le.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(gt.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(ge.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(eq.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void CanUseRealArithmeticOperators()
    {
        using var context = new Z3Context();
        var pi = context.Real(3.14);
        var two = context.Real(2.0);

        var sum = pi + two;
        var diff = pi - two;
        var prod = pi * two;
        var quot = pi / two;

        Assert.That(sum.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(diff.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(prod.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(quot.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void CanUseRealComparisonOperators()
    {
        using var context = new Z3Context();
        var pi = context.Real(3.14);
        var two = context.Real(2.0);

        var lt = pi < two;
        var le = pi <= two;
        var gt = pi > two;
        var ge = pi >= two;
        var eq = pi == two;

        Assert.That(lt.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(le.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(gt.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(ge.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(eq.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void CanCreateComplexExpressions()
    {
        using var context = new Z3Context();
        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var ten = context.Int(10);

        // (x + y) == 10
        var constraint = (x + y) == ten;

        Assert.That(constraint.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void CanUseInequalityOperator()
    {
        using var context = new Z3Context();
        var five = context.Int(5);
        var three = context.Int(3);

        var notEqual = five != three;
        var alsoNotEqual = !(five == three);

        Assert.That(notEqual.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(alsoNotEqual.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void CanCreateBooleanVariables()
    {
        using var context = new Z3Context();
        var p = context.BoolConst("p");
        var q = context.BoolConst("q");

        Assert.That(p.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(q.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void ToStringReturnsValidRepresentation()
    {
        using var context = new Z3Context();
        var x = context.IntConst("x");
        var five = context.Int(5);
        var constraint = x == five;

        // ToString should work without throwing and return non-empty string
        var xStr = x.ToString();
        var fiveStr = five.ToString();
        var constraintStr = constraint.ToString();

        Assert.That(xStr, Is.EqualTo("x"));
        Assert.That(fiveStr, Is.Not.Null.And.Not.Empty);
        Assert.That(constraintStr, Is.Not.Null.And.Not.Empty);
    }

    [Test]
    public void ToStringWorksWhenContextDisposed()
    {
        var context = new Z3Context();
        var x = context.IntConst("x");
        var five = context.Int(5);
        
        context.Dispose();

        // ToString should not throw even when context is disposed
        var xStr = x.ToString();
        var fiveStr = five.ToString();

        Assert.That(xStr, Is.EqualTo("<disposed>"));
        Assert.That(fiveStr, Is.EqualTo("<disposed>"));
    }

    [Test]
    public void ToStringWorksOnDifferentExpressionTypes()
    {
        using var context = new Z3Context();
        
        // Test different primitive types
        var intConst = context.IntConst("x");
        var intValue = context.Int(42);
        var realConst = context.RealConst("y");
        var realValue = context.Real(3.14);
        var boolConst = context.BoolConst("p");
        var boolTrue = context.True();
        var boolFalse = context.False();

        // Test specific known values where possible
        Assert.That(intConst.ToString(), Is.EqualTo("x"));
        Assert.That(intValue.ToString(), Is.Not.Null.And.Not.Empty);
        Assert.That(realConst.ToString(), Is.EqualTo("y"));
        Assert.That(realValue.ToString(), Is.Not.Null.And.Not.Empty);
        Assert.That(boolConst.ToString(), Is.EqualTo("p"));
        Assert.That(boolTrue.ToString(), Is.EqualTo("true"));
        Assert.That(boolFalse.ToString(), Is.EqualTo("false"));
    }

    [Test]
    public void ToStringWorksOnComplexExpressions()
    {
        using var context = new Z3Context();
        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var p = context.BoolConst("p");
        var q = context.BoolConst("q");

        // Test arithmetic expressions
        var add = x + y;
        var sub = x - y;
        var mul = x * y;
        var div = x / y;
        
        // Test comparison expressions
        var eq = x == y;
        var lt = x < y;
        var gt = x > y;
        var le = x <= y;
        var ge = x >= y;
        
        // Test boolean expressions
        var and = p & q;
        var or = p | q;
        var not = !p;

        // All complex expressions should have valid string representations
        Assert.That(add.ToString(), Is.Not.Null.And.Not.Empty);
        Assert.That(sub.ToString(), Is.Not.Null.And.Not.Empty);
        Assert.That(mul.ToString(), Is.Not.Null.And.Not.Empty);
        Assert.That(div.ToString(), Is.Not.Null.And.Not.Empty);
        Assert.That(eq.ToString(), Is.Not.Null.And.Not.Empty);
        Assert.That(lt.ToString(), Is.Not.Null.And.Not.Empty);
        Assert.That(gt.ToString(), Is.Not.Null.And.Not.Empty);
        Assert.That(le.ToString(), Is.Not.Null.And.Not.Empty);
        Assert.That(ge.ToString(), Is.Not.Null.And.Not.Empty);
        Assert.That(and.ToString(), Is.Not.Null.And.Not.Empty);
        Assert.That(or.ToString(), Is.Not.Null.And.Not.Empty);
        Assert.That(not.ToString(), Is.Not.Null.And.Not.Empty);
    }

    [Test]
    public void ToStringHandlesNestedComplexExpressions()
    {
        using var context = new Z3Context();
        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var z = context.IntConst("z");

        // Deeply nested expression: ((x + y) * z) > ((x - y) / z)
        var nested = ((x + y) * z) > ((x - y) / z);

        // Should handle complex nesting without issues
        var result = nested.ToString();
        Assert.That(result, Is.Not.Null.And.Not.Empty);
        Assert.That(result, Does.Not.Contain("<error>"));
        Assert.That(result, Does.Not.Contain("<invalid>"));
        Assert.That(result, Does.Not.Contain("<disposed>"));
    }

    [Test]
    public void ToStringConsistentResults()
    {
        using var context = new Z3Context();
        var x = context.IntConst("x");
        
        // Multiple calls to ToString should return the same result
        var first = x.ToString();
        var second = x.ToString();
        var third = x.ToString();
        
        Assert.That(first, Is.EqualTo(second));
        Assert.That(second, Is.EqualTo(third));
        Assert.That(first, Is.Not.Null.And.Not.Empty);
    }
}