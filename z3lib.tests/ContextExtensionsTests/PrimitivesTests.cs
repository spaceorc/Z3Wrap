namespace z3lib.Tests.ContextExtensionsTests;

[TestFixture]
public class PrimitivesTests
{
    [Test]
    public void Int_CreatesIntegerConstant()
    {
        using var context = new Z3Context();
        
        var five = context.Int(5);
        var negTen = context.Int(-10);
        var zero = context.Int(0);

        Assert.That(five.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(negTen.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(zero.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(five.Context, Is.SameAs(context));
        Assert.That(negTen.Context, Is.SameAs(context));
        Assert.That(zero.Context, Is.SameAs(context));
    }

    [Test]
    public void IntConst_CreatesIntegerVariable()
    {
        using var context = new Z3Context();
        
        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var longName = context.IntConst("long_variable_name");

        Assert.That(x.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(y.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(longName.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(x.Context, Is.SameAs(context));
        Assert.That(y.Context, Is.SameAs(context));
        Assert.That(longName.Context, Is.SameAs(context));
    }

    [Test]
    public void Real_CreatesRealConstant()
    {
        using var context = new Z3Context();
        
        var pi = context.Real(3.14159);
        var half = context.Real(0.5);
        var negative = context.Real(-2.718);
        var zero = context.Real(0.0);

        Assert.That(pi.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(half.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(negative.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(zero.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(pi.Context, Is.SameAs(context));
        Assert.That(half.Context, Is.SameAs(context));
        Assert.That(negative.Context, Is.SameAs(context));
        Assert.That(zero.Context, Is.SameAs(context));
    }

    [Test]
    public void RealConst_CreatesRealVariable()
    {
        using var context = new Z3Context();
        
        var x = context.RealConst("x");
        var y = context.RealConst("y");
        var temperature = context.RealConst("temperature");

        Assert.That(x.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(y.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(temperature.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(x.Context, Is.SameAs(context));
        Assert.That(y.Context, Is.SameAs(context));
        Assert.That(temperature.Context, Is.SameAs(context));
    }

    [Test]
    public void True_CreatesTrueConstant()
    {
        using var context = new Z3Context();
        
        var trueExpr = context.True();

        Assert.That(trueExpr.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(trueExpr.Context, Is.SameAs(context));
    }

    [Test]
    public void False_CreatesFalseConstant()
    {
        using var context = new Z3Context();
        
        var falseExpr = context.False();

        Assert.That(falseExpr.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(falseExpr.Context, Is.SameAs(context));
    }

    [Test]
    public void Bool_CreatesBooleanFromBool()
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
    public void BoolConst_CreatesBooleanVariable()
    {
        using var context = new Z3Context();
        
        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        var isValid = context.BoolConst("isValid");

        Assert.That(p.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(q.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(isValid.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(p.Context, Is.SameAs(context));
        Assert.That(q.Context, Is.SameAs(context));
        Assert.That(isValid.Context, Is.SameAs(context));
    }

    [Test]
    public void Bool_TrueEquivalentToTrue()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var boolTrue = context.Bool(true);
        var trueExpr = context.True();
        
        // They should be logically equivalent
        solver.Assert(boolTrue.Iff(trueExpr));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Bool_FalseEquivalentToFalse()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var boolFalse = context.Bool(false);
        var falseExpr = context.False();
        
        // They should be logically equivalent
        solver.Assert(boolFalse.Iff(falseExpr));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }
}