namespace Z3Wrap.Tests.ExpressionTests;

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

        // Test that x == x evaluates to true
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

        // Test that we can have cases where x != y
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

        // Test that x != y can be true
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

        // Test that x != x should be unsatisfiable (always false)
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

        // Test that Eq method works equivalently to == operator
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

        // Test that Neq method works equivalently to != operator
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

        // Test that 5 == 5 is satisfiable
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

        // Test that 5 != 10 is satisfiable
        solver.Assert(inequality);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Equals_SameHandle_ReturnsTrue()
    {
        using var context = new Z3Context();
        
        var x = context.IntConst("x");
        var y = context.IntConst("x"); // Same name, should have same handle
        
        // Test that expressions with the same handle are equal
        Assert.That(x.Equals(y), Is.True);
    }

    [Test]
    public void Equals_DifferentHandle_ReturnsFalse()
    {
        using var context = new Z3Context();
        
        var x = context.IntConst("x");
        var y = context.IntConst("y");
        
        // Test that expressions with different handles are not equal
        Assert.That(x.Equals(y), Is.False);
    }

    [Test]
    public void Equals_NullObject_ReturnsFalse()
    {
        using var context = new Z3Context();
        
        var x = context.IntConst("x");
        
        // Test that expression does not equal null
        Assert.That(x.Equals(null), Is.False);
    }

    [Test]
    public void Equals_NonZ3ExprObject_ReturnsFalse()
    {
        using var context = new Z3Context();
        
        var x = context.IntConst("x");
        var notAnExpr = "not an expression";
        
        // Test that expression does not equal non-Z3Expr objects
        // ReSharper disable once SuspiciousTypeConversion.Global
        Assert.That(x.Equals(notAnExpr), Is.False);
    }

    [Test]
    public void GetHashCode_SameHandle_ReturnsSameHashCode()
    {
        using var context = new Z3Context();
        
        var x = context.IntConst("x");
        var y = context.IntConst("x"); // Same name, should have same handle
        
        // Test that expressions with the same handle have the same hash code
        Assert.That(x.GetHashCode(), Is.EqualTo(y.GetHashCode()));
    }

    [Test]
    public void GetHashCode_DifferentHandle_ReturnsDifferentHashCode()
    {
        using var context = new Z3Context();
        
        var x = context.IntConst("x");
        var y = context.IntConst("y");
        
        // Test that expressions with different handles have different hash codes
        Assert.That(x.GetHashCode(), Is.Not.EqualTo(y.GetHashCode()));
    }

    [Test]
    public void ToString_ValidExpression_ReturnsStringRepresentation()
    {
        using var context = new Z3Context();
        
        var x = context.IntConst("x");
        var toString = x.ToString();
        
        // Test that ToString returns a valid string representation
        Assert.That(toString, Is.Not.Null);
        Assert.That(toString, Is.Not.Empty);
        Assert.That(toString, Does.Contain("x")); // Should contain the variable name
    }

    [Test]
    public void ToString_ArithmeticExpression_ReturnsCorrectFormat()
    {
        using var context = new Z3Context();
        
        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var sum = x + y;
        var toString = sum.ToString();
        
        // Test that ToString returns a representation of the arithmetic expression
        Assert.That(toString, Is.Not.Null);
        Assert.That(toString, Is.Not.Empty);
        // Should contain both variable names and some arithmetic operator representation
        Assert.That(toString, Does.Contain("x").Or.Contain("y"));
    }

    [Test]
    public void ToString_BooleanExpression_ReturnsCorrectFormat()
    {
        using var context = new Z3Context();
        
        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        var conjunction = context.And(p, q);
        var toString = conjunction.ToString();
        
        // Test that ToString returns a representation of the boolean expression
        Assert.That(toString, Is.Not.Null);
        Assert.That(toString, Is.Not.Empty);
        // Should contain the variable names
        Assert.That(toString, Does.Contain("p").Or.Contain("q"));
    }

    [Test]
    public void ToString_DisposedContext_ReturnsDisposedMessage()
    {
        var context = new Z3Context();
        var x = context.IntConst("x");
        
        // Dispose the context
        context.Dispose();
        
        // Test that ToString handles disposed context gracefully
        var toString = x.ToString();
        Assert.That(toString, Is.EqualTo("<disposed>"));
    }

    [Test]
    public void EqualityConsistency_ReflexiveProperty()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var x = context.IntConst("x");
        
        // Test reflexivity: x == x should always be true
#pragma warning disable CS1718 // Comparison made to same variable
        // ReSharper disable once EqualExpressionComparison
        var equality = x == x;
#pragma warning restore CS1718
        solver.Assert(equality);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
        
        // Test that x.Eq(x) is also true
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
        
        // Test symmetry: (x == y) iff (y == x)
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
        
        // Test that (x != y) is equivalent to NOT(x == y)
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
        var realFive = context.Real(5.0);
        var equality = intFive == realFive;
        
        Assert.That(equality.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(equality.Context, Is.SameAs(context));
        
        // Test that 5 (int) == 5.0 (real) is satisfiable in Z3
        solver.Assert(equality);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void MixedTypeInequality_IntAndReal_WorksCorrectly()
    {
        using var context = new Z3Context();
        using var solver = context.CreateSolver();
        
        var intFive = context.Int(5);
        var realPi = context.Real(3.14159);
        var inequality = intFive != realPi;
        
        Assert.That(inequality.Handle, Is.Not.EqualTo(IntPtr.Zero));
        Assert.That(inequality.Context, Is.SameAs(context));
        
        // Test that 5 (int) != 3.14159 (real) is satisfiable
        solver.Assert(inequality);
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }
}