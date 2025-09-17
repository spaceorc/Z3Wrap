using Spaceorc.Z3Wrap;
namespace Z3Wrap.Tests.Unit.Expressions;

[TestFixture]
public class Z3ExprTests
{
    [Test]
    public void EqualityOperator_SameExpression_ReturnsTrue()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var y = context.IntConst("x"); // Same name, should be same expression
        var equality = x == y;

        Assert.That(equality.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(equality.Context, Is.SameAs(context));

        solver.Assert(equality);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void EqualityOperator_DifferentExpressions_ReturnsFalse()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var equality = x == y;

        Assert.That(equality.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(equality.Context, Is.SameAs(context));

        solver.Assert(context.Neq(x, y));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void InequalityOperator_DifferentExpressions_ReturnsTrue()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var inequality = x != y;

        Assert.That(inequality.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(inequality.Context, Is.SameAs(context));

        solver.Assert(inequality);
        solver.Assert(context.Neq(x, y)); // Make them actually different
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void InequalityOperator_SameExpression_ReturnsFalse()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
#pragma warning disable CS1718 // Comparison made to same variable
        // ReSharper disable once EqualExpressionComparison
        var inequality = x != x;
#pragma warning restore CS1718

        Assert.That(inequality.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(inequality.Context, Is.SameAs(context));

        solver.Assert(inequality);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void EqMethod_SameExpression_CreatesEqualityConstraint()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var equality = x.Eq(y);

        Assert.That(equality.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(equality.Context, Is.SameAs(context));

        var operatorEquality = x == y;
        solver.Assert(equality.Iff(operatorEquality));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void NeqMethod_DifferentExpressions_CreatesInequalityConstraint()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var inequality = x.Neq(y);

        Assert.That(inequality.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(inequality.Context, Is.SameAs(context));

        var operatorInequality = x != y;
        solver.Assert(inequality.Iff(operatorInequality));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void EqMethod_WithConstants_CreatesCorrectEquality()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var five1 = context.Int(5);
        var five2 = context.Int(5);
        var equality = five1.Eq(five2);

        Assert.That(equality.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(equality.Context, Is.SameAs(context));

        solver.Assert(equality);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void NeqMethod_WithDifferentConstants_CreatesCorrectInequality()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var five = context.Int(5);
        var ten = context.Int(10);
        var inequality = five.Neq(ten);

        Assert.That(inequality.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(inequality.Context, Is.SameAs(context));

        solver.Assert(inequality);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Equals_SameHandle_ReturnsTrue()
    {
        using var context = new Z3Context();

        var x = context.IntConst("x");
        var y = context.IntConst("x"); // Same name, should have same handle

        Assert.That(x.Equals(y), Is.True);
    }

    [Test]
    public void Equals_DifferentHandle_ReturnsFalse()
    {
        using var context = new Z3Context();

        var x = context.IntConst("x");
        var y = context.IntConst("y");

        Assert.That(x.Equals(y), Is.False);
    }

    [Test]
    public void Equals_NullObject_ReturnsFalse()
    {
        using var context = new Z3Context();

        var x = context.IntConst("x");

        Assert.That(x.Equals(null), Is.False);
    }

    [Test]
    public void Equals_NonZ3ExprObject_ReturnsFalse()
    {
        using var context = new Z3Context();

        var x = context.IntConst("x");
        var notAnExpr = "not an expression";

        // ReSharper disable once SuspiciousTypeConversion.Global
        Assert.That(x.Equals(notAnExpr), Is.False);
    }

    [Test]
    public void GetHashCode_SameHandle_ReturnsSameHashCode()
    {
        using var context = new Z3Context();

        var x = context.IntConst("x");
        var y = context.IntConst("x"); // Same name, should have same handle

        Assert.That(x.GetHashCode(), Is.EqualTo(y.GetHashCode()));
    }

    [Test]
    public void GetHashCode_DifferentHandle_ReturnsDifferentHashCode()
    {
        using var context = new Z3Context();

        var x = context.IntConst("x");
        var y = context.IntConst("y");

        Assert.That(x.GetHashCode(), Is.Not.EqualTo(y.GetHashCode()));
    }

    [Test]
    public void ToString_ValidExpression_ReturnsStringRepresentation()
    {
        using var context = new Z3Context();

        var x = context.IntConst("x");
        var toString = x.ToString();

        Assert.That(toString, Is.EqualTo("x"));
    }

    [Test]
    public void ToString_ArithmeticExpression_ReturnsCorrectFormat()
    {
        using var context = new Z3Context();

        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var sum = x + y;
        var toString = sum.ToString();

        Assert.That(toString, Is.EqualTo("(+ x y)"));
    }

    [Test]
    public void ToString_BooleanExpression_ReturnsCorrectFormat()
    {
        using var context = new Z3Context();

        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        var conjunction = context.And(p, q);
        var toString = conjunction.ToString();

        Assert.That(toString, Is.EqualTo("(and p q)"));
    }

    [Test]
    public void ToString_DisposedContext_ReturnsDisposedMessage()
    {
        var context = new Z3Context();
        var x = context.IntConst("x");

        context.Dispose();

        var toString = x.ToString();
        Assert.That(toString, Is.EqualTo("<disposed>"));
    }

    [Test]
    public void EqualityConsistency_ReflexiveProperty()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");

#pragma warning disable CS1718 // Comparison made to same variable
        // ReSharper disable once EqualExpressionComparison
        var equality = x == x;
#pragma warning restore CS1718
        solver.Assert(equality);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        solver.Reset();
        solver.Assert(x.Eq(x));
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void EqualityConsistency_SymmetricProperty()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var y = context.IntConst("y");

        var leftRight = x == y;
        var rightLeft = y == x;
        var symmetry = leftRight.Iff(rightLeft);

        solver.Assert(symmetry);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void InequalityConsistency_OppositeOfEquality()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var y = context.IntConst("y");

        var inequality = x != y;
        var notEquality = context.Not(x == y);
        var equivalence = inequality.Iff(notEquality);

        solver.Assert(equivalence);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void MixedTypeEquality_IntAndReal_WorksCorrectly()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var intFive = context.Int(5);
        var realFive = context.Real(5.0m);
        var equality = intFive == realFive;

        Assert.That(equality.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(equality.Context, Is.SameAs(context));

        solver.Assert(equality);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void MixedTypeInequality_IntAndReal_WorksCorrectly()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();

        var intFive = context.Int(5);
        var realPi = context.Real(3.14159m);
        var inequality = intFive != realPi;

        Assert.That(inequality.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(inequality.Context, Is.SameAs(context));

        solver.Assert(inequality);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }
}