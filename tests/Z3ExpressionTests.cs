using z3lib;

namespace tests;

public class Z3ExpressionTests
{
    [OneTimeSetUp]
    public void Setup()
    {
        NativeMethods.LoadLibrary("/opt/homebrew/opt/z3/lib/libz3.dylib");
    }

    [Test]
    public void CanCreateBooleanConstants()
    {
        using (var context = new Z3Context())
        {
            var trueExpr = context.MkTrue();
            var falseExpr = context.MkFalse();

            Assert.That(trueExpr.Handle, Is.Not.EqualTo(IntPtr.Zero));
            Assert.That(falseExpr.Handle, Is.Not.EqualTo(IntPtr.Zero));
        }
    }

    [Test]
    public void CanCreateIntegerConstants()
    {
        using (var context = new Z3Context())
        {
            var five = context.MkInt(5);
            var negTen = context.MkInt(-10);

            Assert.That(five.Handle, Is.Not.EqualTo(IntPtr.Zero));
            Assert.That(negTen.Handle, Is.Not.EqualTo(IntPtr.Zero));
        }
    }

    [Test]
    public void CanCreateIntegerVariables()
    {
        using (var context = new Z3Context())
        {
            var x = context.MkIntConst("x");
            var y = context.MkIntConst("y");

            Assert.That(x.Handle, Is.Not.EqualTo(IntPtr.Zero));
            Assert.That(y.Handle, Is.Not.EqualTo(IntPtr.Zero));
        }
    }

    [Test]
    public void CanCreateRealConstants()
    {
        using (var context = new Z3Context())
        {
            var pi = context.MkReal(3.14159);
            var half = context.MkReal(0.5);

            Assert.That(pi.Handle, Is.Not.EqualTo(IntPtr.Zero));
            Assert.That(half.Handle, Is.Not.EqualTo(IntPtr.Zero));
        }
    }

    [Test]
    public void CanCreateRealVariables()
    {
        using (var context = new Z3Context())
        {
            var x = context.MkRealConst("x");
            var y = context.MkRealConst("y");

            Assert.That(x.Handle, Is.Not.EqualTo(IntPtr.Zero));
            Assert.That(y.Handle, Is.Not.EqualTo(IntPtr.Zero));
        }
    }

    [Test]
    public void CanUseBooleanOperators()
    {
        using (var context = new Z3Context())
        {
            var p = context.MkTrue();
            var q = context.MkFalse();

            var andExpr = p & q;
            var orExpr = p | q;
            var notExpr = !p;

            Assert.That(andExpr.Handle, Is.Not.EqualTo(IntPtr.Zero));
            Assert.That(orExpr.Handle, Is.Not.EqualTo(IntPtr.Zero));
            Assert.That(notExpr.Handle, Is.Not.EqualTo(IntPtr.Zero));
        }
    }

    [Test]
    public void CanUseIntegerArithmeticOperators()
    {
        using (var context = new Z3Context())
        {
            var five = context.MkInt(5);
            var three = context.MkInt(3);

            var sum = five + three;
            var diff = five - three;
            var prod = five * three;
            var quot = five / three;

            Assert.That(sum.Handle, Is.Not.EqualTo(IntPtr.Zero));
            Assert.That(diff.Handle, Is.Not.EqualTo(IntPtr.Zero));
            Assert.That(prod.Handle, Is.Not.EqualTo(IntPtr.Zero));
            Assert.That(quot.Handle, Is.Not.EqualTo(IntPtr.Zero));
        }
    }

    [Test]
    public void CanUseIntegerComparisonOperators()
    {
        using (var context = new Z3Context())
        {
            var five = context.MkInt(5);
            var three = context.MkInt(3);

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
    }

    [Test]
    public void CanUseRealArithmeticOperators()
    {
        using (var context = new Z3Context())
        {
            var pi = context.MkReal(3.14);
            var two = context.MkReal(2.0);

            var sum = pi + two;
            var diff = pi - two;
            var prod = pi * two;
            var quot = pi / two;

            Assert.That(sum.Handle, Is.Not.EqualTo(IntPtr.Zero));
            Assert.That(diff.Handle, Is.Not.EqualTo(IntPtr.Zero));
            Assert.That(prod.Handle, Is.Not.EqualTo(IntPtr.Zero));
            Assert.That(quot.Handle, Is.Not.EqualTo(IntPtr.Zero));
        }
    }

    [Test]
    public void CanUseRealComparisonOperators()
    {
        using (var context = new Z3Context())
        {
            var pi = context.MkReal(3.14);
            var two = context.MkReal(2.0);

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
    }

    [Test]
    public void CanCreateComplexExpressions()
    {
        using (var context = new Z3Context())
        {
            var x = context.MkIntConst("x");
            var y = context.MkIntConst("y");
            var ten = context.MkInt(10);

            // (x + y) == 10
            var constraint = (x + y) == ten;

            Assert.That(constraint.Handle, Is.Not.EqualTo(IntPtr.Zero));
        }
    }
}