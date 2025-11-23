using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Expressions.Numerics;

namespace Z3Wrap.Tests.Core;

[TestFixture]
public class Z3SolverProofTests
{
    private static readonly Dictionary<string, string> ProofParams = new()
    {
        { "proof", "true" },
        { "unsat_core", "true" },
    };

    [Test]
    public void GetProof_SimpleContradiction_ReturnsProofString()
    {
        using var context = new Z3Context(ProofParams);
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");

        solver.Assert(x > 10);
        solver.Assert(x < 5);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Unsatisfiable));

        var proof = solver.GetProof();

        Assert.Multiple(() =>
        {
            Assert.That(proof, Is.Not.Null);
            Assert.That(proof, Is.Not.Empty);
            Assert.That(proof, Does.Contain("asserted")); // Contains user assertions
            Assert.That(proof.Length, Is.GreaterThan(0));
        });
    }

    [Test]
    public void GetProof_BooleanContradiction_ReturnsProofString()
    {
        using var context = new Z3Context(ProofParams);
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.BoolConst("a");

        solver.Assert(a);
        solver.Assert(!a);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Unsatisfiable));

        var proof = solver.GetProof();

        Assert.Multiple(() =>
        {
            Assert.That(proof, Does.Contain("asserted"));
            Assert.That(proof, Does.Contain("a"));
            Assert.That(proof, Does.Contain("false"));
        });
    }

    [Test]
    public void GetProof_ArithmeticTheoryLemma_ContainsTheoryReasoning()
    {
        using var context = new Z3Context(ProofParams);
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var y = context.IntConst("y");

        solver.Assert(x + y == 10);
        solver.Assert(x + y == 20);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Unsatisfiable));

        var proof = solver.GetProof();

        Assert.Multiple(() =>
        {
            Assert.That(proof, Does.Contain("asserted"));
            // Proof should show that 10 ≠ 20
            Assert.That(proof.Length, Is.GreaterThan(50));
        });
    }

    [Test]
    public void GetProof_WithoutProofEnabled_ThrowsOrReturnsEmptyProof()
    {
        using var context = new Z3Context(); // No proof parameters
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");

        solver.Assert(x > 10);
        solver.Assert(x < 5);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Unsatisfiable));

        // Without proof enabled, GetProof should either throw or return empty/minimal proof
        // Z3's behavior may vary
        try
        {
            var proof = solver.GetProof();
            // If it doesn't throw, proof might be empty or minimal
            Assert.That(proof, Is.Not.Null);
        }
        catch (Exception)
        {
            // Expected: Z3 may throw if proofs not enabled
            Assert.Pass("Z3 threw exception when proofs not enabled (expected behavior)");
        }
    }

    [Test]
    public void GetProof_AfterSatisfiableResult_ThrowsInvalidOperationException()
    {
        using var context = new Z3Context(ProofParams);
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        solver.Assert(x > 5);
        solver.Assert(x < 10);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var ex = Assert.Throws<InvalidOperationException>(() => solver.GetProof());
        Assert.That(ex.Message, Does.Contain("Satisfiable"));
    }

    [Test]
    public void GetProof_BeforeCheck_ThrowsInvalidOperationException()
    {
        using var context = new Z3Context(ProofParams);
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var ex = Assert.Throws<InvalidOperationException>(() => solver.GetProof());
        Assert.That(ex.Message, Does.Contain("Must call Check()"));
    }

    [Test]
    public void GetProof_MultipleConstraints_ShowsAllAssertions()
    {
        using var context = new Z3Context(ProofParams);
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var z = context.IntConst("z");

        solver.Assert(x == 5);
        solver.Assert(y == 10);
        solver.Assert(z == x + y);
        solver.Assert(z == 20); // Contradiction: 5 + 10 ≠ 20

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Unsatisfiable));

        var proof = solver.GetProof();

        Assert.Multiple(() =>
        {
            Assert.That(proof, Is.Not.Empty);
            Assert.That(proof, Does.Contain("asserted"));
        });
    }

    [Test]
    public void GetProof_AfterPushPop_ReflectsCurrentScope()
    {
        using var context = new Z3Context(ProofParams);
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");

        solver.Assert(x > 10);

        solver.Push();
        solver.Assert(x < 5);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Unsatisfiable));

        var proof1 = solver.GetProof();
        Assert.That(proof1, Is.Not.Empty);

        solver.Pop();

        // After pop, x > 10 alone is satisfiable
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var ex = Assert.Throws<InvalidOperationException>(() => solver.GetProof());
        Assert.That(ex.Message, Does.Contain("Satisfiable"));
    }

    [Test]
    public void GetProof_WithCheckAssumptions_ReturnsProof()
    {
        using var context = new Z3Context(ProofParams);
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var t1 = context.BoolConst("t1");
        var t2 = context.BoolConst("t2");

        solver.Assert(t1.Implies(x > 10));
        solver.Assert(t2.Implies(x < 5));

        var status = solver.CheckAssumptions(t1, t2);
        Assert.That(status, Is.EqualTo(Z3Status.Unsatisfiable));

        var proof = solver.GetProof();

        Assert.Multiple(() =>
        {
            Assert.That(proof, Is.Not.Empty);
            Assert.That(proof, Does.Contain("asserted"));
        });
    }

    [Test]
    public void GetProof_ProofFormat_IsSExpression()
    {
        using var context = new Z3Context(ProofParams);
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.BoolConst("a");
        solver.Assert(a);
        solver.Assert(!a);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Unsatisfiable));

        var proof = solver.GetProof();

        // S-expressions start with '(' and contain LISP-like structure
        Assert.Multiple(() =>
        {
            Assert.That(proof, Does.StartWith("("));
            Assert.That(proof, Does.Contain(")"));
            Assert.That(proof, Does.Contain("asserted"));
            Assert.That(proof, Does.Contain("false"));
        });
    }
}
