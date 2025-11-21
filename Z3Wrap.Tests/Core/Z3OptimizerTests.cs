using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.BitVectors;
using Spaceorc.Z3Wrap.Expressions.Numerics;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Z3Wrap.Tests.Core;

[TestFixture]
public class Z3OptimizerTests
{
    [Test]
    public void CreateOptimizer_ReturnsValidInstance()
    {
        using var context = new Z3Context();
        using var optimizer = context.CreateOptimizer();

        Assert.That(optimizer, Is.Not.Null);
        Assert.That(optimizer.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void Dispose_ReleasesResources()
    {
        using var context = new Z3Context();
        var optimizer = context.CreateOptimizer();

        optimizer.Dispose();

        Assert.Throws<ObjectDisposedException>(() => _ = optimizer.Handle);
    }

    [Test]
    public void Assert_AddsConstraint()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var optimizer = context.CreateOptimizer();

        var x = context.IntConst("x");

        optimizer.Assert(x > 0);
        optimizer.Assert(x < 10);

        var status = optimizer.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Check_UnsatisfiableConstraints_ReturnsUnsatisfiable()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var optimizer = context.CreateOptimizer();

        var x = context.IntConst("x");

        optimizer.Assert(x > 10);
        optimizer.Assert(x < 5);

        var status = optimizer.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void GetModel_AfterSatisfiableCheck_ReturnsModel()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var optimizer = context.CreateOptimizer();

        var x = context.IntConst("x");
        var y = context.IntConst("y");

        optimizer.Assert(x + y == 10);
        optimizer.Assert(x > 0);
        optimizer.Assert(y > 0);

        var status = optimizer.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = optimizer.GetModel();
        Assert.That(model, Is.Not.Null);

        var xVal = model.GetIntValue(x);
        var yVal = model.GetIntValue(y);
        Assert.That(xVal + yVal, Is.EqualTo(new BigInteger(10)));
    }

    [Test]
    public void GetModel_BeforeCheck_ThrowsInvalidOperationException()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var optimizer = context.CreateOptimizer();

        var x = context.IntConst("x");
        optimizer.Assert(x > 0);

        Assert.Throws<InvalidOperationException>(() => optimizer.GetModel());
    }

    [Test]
    public void GetModel_AfterUnsatisfiable_ThrowsInvalidOperationException()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var optimizer = context.CreateOptimizer();

        var x = context.IntConst("x");
        optimizer.Assert(x > 10);
        optimizer.Assert(x < 5);

        var status = optimizer.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Unsatisfiable));

        Assert.Throws<InvalidOperationException>(() => optimizer.GetModel());
    }

    [Test]
    public void PushPop_BacktracksConstraints()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var optimizer = context.CreateOptimizer();

        var x = context.IntConst("x");

        optimizer.Assert(x > 0);
        var status1 = optimizer.Check();
        Assert.That(status1, Is.EqualTo(Z3Status.Satisfiable));

        optimizer.Push();
        optimizer.Assert(x < 0);
        var status2 = optimizer.Check();
        Assert.That(status2, Is.EqualTo(Z3Status.Unsatisfiable));

        optimizer.Pop();
        var status3 = optimizer.Check();
        Assert.That(status3, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ModelInvalidation_AfterAssert_InvalidatesModel()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var optimizer = context.CreateOptimizer();

        var x = context.IntConst("x");

        optimizer.Assert(x == 5);
        optimizer.Check();
        var model = optimizer.GetModel();

        // Model should be valid
        Assert.That(model.GetIntValue(x), Is.EqualTo(new BigInteger(5)));

        // Adding constraint invalidates model
        optimizer.Assert(x > 0);

        // Model should now be invalid
        Assert.Throws<ObjectDisposedException>(() => model.GetIntValue(x));
    }

    [Test]
    public void ModelInvalidation_AfterCheck_InvalidatesPreviousModel()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var optimizer = context.CreateOptimizer();

        var x = context.IntConst("x");

        optimizer.Assert(x == 5);
        optimizer.Check();
        var model1 = optimizer.GetModel();
        Assert.That(model1.GetIntValue(x), Is.EqualTo(new BigInteger(5)));

        // Second check invalidates first model
        optimizer.Check();
        Assert.Throws<ObjectDisposedException>(() => model1.GetIntValue(x));

        // But new model is valid
        var model2 = optimizer.GetModel();
        Assert.That(model2.GetIntValue(x), Is.EqualTo(new BigInteger(5)));
    }

    [Test]
    public void ToString_ReturnsValidString()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var optimizer = context.CreateOptimizer();

        var x = context.IntConst("x");
        optimizer.Assert(x > 0);

        var str = optimizer.ToString();
        Assert.That(str, Is.Not.Null);
        Assert.That(str, Does.Contain("x"));
    }

    [Test]
    public void ContextDisposal_DisposesOptimizer()
    {
        var context = new Z3Context();
        var optimizer = context.CreateOptimizer();

        context.Dispose();

        // Optimizer should be disposed
        Assert.Throws<ObjectDisposedException>(() => _ = optimizer.Handle);
    }

    [Test]
    public void Maximize_IntExpr_ReturnsTypedObjective()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var optimizer = context.CreateOptimizer();

        var x = context.IntConst("x");
        var y = context.IntConst("y");

        optimizer.Assert(x + y <= 100);
        optimizer.Assert(x >= 0);
        optimizer.Assert(y >= 0);

        var objective = optimizer.Maximize(x + y);

        Assert.That(objective, Is.Not.Null);
        Assert.That(objective.ObjectiveId, Is.GreaterThanOrEqualTo(0));
    }

    [Test]
    public void Minimize_IntExpr_ReturnsTypedObjective()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var optimizer = context.CreateOptimizer();

        var x = context.IntConst("x");

        optimizer.Assert(x >= 10);

        var objective = optimizer.Minimize(x);

        Assert.That(objective, Is.Not.Null);
        Assert.That(objective.ObjectiveId, Is.GreaterThanOrEqualTo(0));
    }

    [Test]
    public void Maximize_RealExpr_ReturnsTypedObjective()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var optimizer = context.CreateOptimizer();

        var x = context.RealConst("x");

        optimizer.Assert(x >= 0);
        optimizer.Assert(x <= 100);

        var objective = optimizer.Maximize(x);

        Assert.That(objective, Is.Not.Null);
    }

    [Test]
    public void Maximize_BvExpr_ReturnsTypedObjective()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var optimizer = context.CreateOptimizer();

        var x = context.BvConst<Size8>("x");

        var objective = optimizer.Maximize(x);

        Assert.That(objective, Is.Not.Null);
    }

    [Test]
    public void ModelInvalidation_AfterMaximize_InvalidatesModel()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var optimizer = context.CreateOptimizer();

        var x = context.IntConst("x");

        optimizer.Assert(x >= 0);
        optimizer.Check();
        var model = optimizer.GetModel();

        // Adding objective invalidates model
        optimizer.Maximize(x);

        Assert.Throws<ObjectDisposedException>(() => model.GetIntValue(x));
    }

    [Test]
    public void Maximize_GetUpper_ReturnsOptimalValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var optimizer = context.CreateOptimizer();

        var x = context.IntConst("x");
        var y = context.IntConst("y");

        optimizer.Assert(x + y <= 10);
        optimizer.Assert(x >= 0);
        optimizer.Assert(y >= 0);

        var objective = optimizer.Maximize(x + y);

        var status = optimizer.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = optimizer.GetModel();
        var optimalValue = optimizer.GetUpper(objective);

        // The optimal value should be 10 (when x+y = 10)
        Assert.That(model.GetIntValue(optimalValue), Is.EqualTo(new BigInteger(10)));
    }

    [Test]
    public void Minimize_GetLower_ReturnsOptimalValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var optimizer = context.CreateOptimizer();

        var x = context.IntConst("x");

        optimizer.Assert(x >= 5);
        optimizer.Assert(x <= 20);

        var objective = optimizer.Minimize(x);

        var status = optimizer.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = optimizer.GetModel();
        var optimalValue = optimizer.GetLower(objective);

        // The optimal value should be 5
        Assert.That(model.GetIntValue(optimalValue), Is.EqualTo(new BigInteger(5)));
    }

    [Test]
    public void Maximize_RealExpr_GetUpper_ReturnsOptimalValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var optimizer = context.CreateOptimizer();

        var x = context.RealConst("x");

        optimizer.Assert(x >= 0);
        optimizer.Assert(x <= 10);

        var objective = optimizer.Maximize(x);

        var status = optimizer.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = optimizer.GetModel();
        var optimalValue = optimizer.GetUpper(objective);

        // The optimal value should be 10 (returned as IntExpr due to integer bounds)
        Assert.That(optimalValue.IsIntExpr(), Is.True);
        var result = model.GetNumericValueAsString(optimalValue.AsIntExpr());
        Assert.That(result, Is.EqualTo("10"));
    }

    [Test]
    public void GetUpper_BeforeSatisfiable_ThrowsInvalidOperationException()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var optimizer = context.CreateOptimizer();

        var x = context.IntConst("x");
        var objective = optimizer.Maximize(x);

        Assert.Throws<InvalidOperationException>(() => optimizer.GetUpper(objective));
    }

    [Test]
    public void GetUpper_AfterUnsatisfiable_ThrowsInvalidOperationException()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var optimizer = context.CreateOptimizer();

        var x = context.IntConst("x");
        optimizer.Assert(x > 10);
        optimizer.Assert(x < 5);

        var objective = optimizer.Maximize(x);

        var status = optimizer.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Unsatisfiable));

        Assert.Throws<InvalidOperationException>(() => optimizer.GetUpper(objective));
    }

    [Test]
    public void Minimize_RealExpr_GetLower_ReturnsOptimalValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var optimizer = context.CreateOptimizer();

        var x = context.RealConst("x");

        optimizer.Assert(x >= 5);
        optimizer.Assert(x <= 20);

        var objective = optimizer.Minimize(x);

        var status = optimizer.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = optimizer.GetModel();
        var optimalValue = optimizer.GetLower(objective);

        // The optimal value should be 5 (returned as IntExpr due to integer bounds)
        Assert.That(optimalValue.IsIntExpr(), Is.True);
        var result = model.GetNumericValueAsString(optimalValue.AsIntExpr());
        Assert.That(result, Is.EqualTo("5"));
    }

    [Test]
    public void Minimize_BvExpr_GetLower_ReturnsOptimalValue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var optimizer = context.CreateOptimizer();

        var x = context.BvConst<Size8>("x");

        optimizer.Assert(x >= 10);
        optimizer.Assert(x <= 100);

        var objective = optimizer.Minimize(x);

        var status = optimizer.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = optimizer.GetModel();
        var optimalValue = optimizer.GetLower(objective);

        // The optimal value should be 10
        var result = model.GetIntValue(optimalValue);
        Assert.That(result, Is.EqualTo(new BigInteger(10)));
    }

    [Test]
    public void Maximize_RealExpr_NonIntegerBounds_ReturnsRealExpr()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var optimizer = context.CreateOptimizer();

        var x = context.RealConst("x");

        optimizer.Assert(x >= 0);
        optimizer.Assert(x <= 10.5m);

        var objective = optimizer.Maximize(x);

        var status = optimizer.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = optimizer.GetModel();
        var optimalValue = optimizer.GetUpper(objective);

        // With non-integer bound, Z3 returns RealExpr
        Assert.That(optimalValue.IsRealExpr(), Is.True);
        var result = model.GetNumericValueAsString(optimalValue.AsRealExpr());
        Assert.That(result, Is.EqualTo("21/2"));
    }

    [Test]
    public void ArithmeticExprExtensions_AsRealExpr_OnIntExpr_ConvertsToReal()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var optimizer = context.CreateOptimizer();

        var x = context.RealConst("x");

        optimizer.Assert(x >= 0);
        optimizer.Assert(x <= 10);

        var objective = optimizer.Maximize(x);
        optimizer.Check();

        var optimalValue = optimizer.GetUpper(objective);

        // Z3 returns IntExpr for integer bounds
        Assert.That(optimalValue.IsIntExpr(), Is.True);

        // AsRealExpr() should convert it
        var asReal = optimalValue.AsRealExpr();
        Assert.That(asReal, Is.InstanceOf<RealExpr>());
    }

    [Test]
    public void ArithmeticExprExtensions_AsRealExpr_OnRealExpr_ReturnsSelf()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var optimizer = context.CreateOptimizer();

        var x = context.RealConst("x");

        optimizer.Assert(x >= 0);
        optimizer.Assert(x <= 10.5m);

        var objective = optimizer.Maximize(x);
        optimizer.Check();

        var optimalValue = optimizer.GetUpper(objective);

        // Z3 returns RealExpr for non-integer bounds
        Assert.That(optimalValue.IsRealExpr(), Is.True);

        // AsRealExpr() should return self
        var asReal = optimalValue.AsRealExpr();
        Assert.That(asReal, Is.SameAs(optimalValue));
    }

    [Test]
    public void ArithmeticExprExtensions_AsIntExpr_OnRealExpr_ThrowsException()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var optimizer = context.CreateOptimizer();

        var x = context.RealConst("x");

        optimizer.Assert(x >= 0);
        optimizer.Assert(x <= 10.5m);

        var objective = optimizer.Maximize(x);
        optimizer.Check();

        var optimalValue = optimizer.GetUpper(objective);

        // Z3 returns RealExpr for non-integer bounds
        Assert.That(optimalValue.IsRealExpr(), Is.True);

        // AsIntExpr() should throw
        var ex = Assert.Throws<InvalidOperationException>(() => optimalValue.AsIntExpr());
        Assert.That(ex!.Message, Does.Contain("Optimal value is RealExpr, cannot convert to IntExpr"));
    }

    [Test]
    public void LinearProgramming_Example()
    {
        // Maximize 3x + 2y
        // Subject to: x + y <= 10, x >= 0, y >= 0
        // Expected: x=10, y=0, objective=30

        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var optimizer = context.CreateOptimizer();

        var x = context.IntConst("x");
        var y = context.IntConst("y");

        optimizer.Assert(x + y <= 10);
        optimizer.Assert(x >= 0);
        optimizer.Assert(y >= 0);

        var objective = optimizer.Maximize(3 * x + 2 * y);

        var status = optimizer.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = optimizer.GetModel();
        var optimalValue = optimizer.GetUpper(objective);

        // Optimal: x=10, y=0, objective=30
        Assert.That(model.GetIntValue(x), Is.EqualTo(new BigInteger(10)));
        Assert.That(model.GetIntValue(y), Is.EqualTo(new BigInteger(0)));
        Assert.That(model.GetIntValue(optimalValue), Is.EqualTo(new BigInteger(30)));
    }
}
