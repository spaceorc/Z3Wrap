using Spaceorc.Z3Wrap;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Arrays;
using Spaceorc.Z3Wrap.Expressions.BitVectors;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Expressions.Numerics;
using Spaceorc.Z3Wrap.Extensions;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Z3Wrap.Tests.Unit.Core;

[TestFixture]
public class QuantifierTests
{
    [Test]
    public void ForAll_IntegerVariable_CreatesQuantifier()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.IntConst("x");
        var body = x > 0;

        var forall = context.ForAll(x, body);

        Assert.That(forall, Is.Not.Null);
        Assert.That(forall.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void Exists_IntegerVariable_CreatesQuantifier()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.IntConst("x");
        var body = x > 0;

        var exists = context.Exists(x, body);

        Assert.That(exists, Is.Not.Null);
        Assert.That(exists.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void ForAll_RealVariable_CreatesQuantifier()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var y = context.RealConst("y");
        var body = y > 0;

        var forall = context.ForAll(y, body);

        Assert.That(forall, Is.Not.Null);
        Assert.That(forall.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void Exists_RealVariable_CreatesQuantifier()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var y = context.RealConst("y");
        var body = y > 0;

        var exists = context.Exists(y, body);

        Assert.That(exists, Is.Not.Null);
        Assert.That(exists.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void ForAll_BooleanVariable_CreatesQuantifier()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var p = context.BoolConst("p");
        var body = p | !p; // always true (tautology)

        var forall = context.ForAll(p, body);

        Assert.That(forall, Is.Not.Null);
        Assert.That(forall.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void ForAll_TwoIntegerVariables_CreatesQuantifier()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var body = (x + y) == (y + x); // commutativity

        var forall = context.ForAll(x, y, body);

        Assert.That(forall, Is.Not.Null);
        Assert.That(forall.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void Exists_TwoVariables_IntegerAndReal_CreatesQuantifier()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.IntConst("x");
        var y = context.RealConst("y");
        var body = x.ToReal() + y == 10;

        var exists = context.Exists(x, y, body);

        Assert.That(exists, Is.Not.Null);
        Assert.That(exists.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void ForAll_ThreeVariables_CreatesQuantifier()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var z = context.IntConst("z");
        var body = (x + (y + z)) == ((x + y) + z); // associativity

        var forall = context.ForAll(x, y, z, body);

        Assert.That(forall, Is.Not.Null);
        Assert.That(forall.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void ForAll_FourVariables_CreatesQuantifier()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var w = context.IntConst("w");
        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var z = context.IntConst("z");
        var body = (w + x + y + z) == (z + y + x + w); // commutativity for four variables

        var forall = context.ForAll(w, x, y, z, body);

        Assert.That(forall, Is.Not.Null);
        Assert.That(forall.Handle, Is.Not.EqualTo(IntPtr.Zero));
    }

    [Test]
    public void ForAll_WithSolver_TautologyIsSatisfiable()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var tautology = context.ForAll(x, (x + 0) == x); // always true: x + 0 = x

        solver.Assert(tautology);

        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ForAll_WithSolver_ContradictionIsUnsatisfiable()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var contradiction = context.ForAll(x, x != (x + 0)); // always false: x ≠ x + 0

        solver.Assert(contradiction);

        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void Exists_WithSolver_SatisfiableFormula()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var existsPositive = context.Exists(x, x > 0); // there exists a positive integer

        solver.Assert(existsPositive);

        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ForAll_NestedWithExists_CreatesComplexFormula()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var y = context.IntConst("y");

        // ForAll x, Exists y, x + y = 0
        var innerExists = context.Exists(y, (x + y) == 0);
        var outerForall = context.ForAll(x, innerExists);

        solver.Assert(outerForall);

        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ForAll_WithImplication_LogicalStructure()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var premise = x > 0;
        var conclusion = x >= 1;
        var implication = premise.Implies(conclusion);
        var forall = context.ForAll(x, implication);

        solver.Assert(forall);

        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ForAll_IntegratesWithBitVectorOperations()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.BitVecConst<Size8>("x");
        var y = context.BitVecConst<Size8>("y");
        var body = (x & y) == (y & x); // bitwise AND commutativity

        var forall = context.ForAll(x, y, body);
        solver.Assert(forall);

        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Exists_WithArrays_QuantifiesOverArrayElements()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var arr = context.ArrayConst<IntExpr, IntExpr>("arr");
        var i = context.IntConst("i");

        // There exists an index i such that arr[i] = 10
        var existsIndex = context.Exists(i, arr[i] == 10);
        solver.Assert(existsIndex);

        // Set arr[5] = 10 to make it satisfiable
        solver.Assert(arr[context.Int(5)] == 10);

        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void ForAll_RealArithmetic_MathematicalProperty()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.RealConst("x");
        var y = context.RealConst("y");

        // ForAll x,y: x * y = y * x (multiplication commutativity)
        var body = (x * y) == (y * x);
        var forall = context.ForAll(x, y, body);

        solver.Assert(forall);

        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void QuantifierCombination_ExistsAndForAll_LogicalValidity()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var z = context.IntConst("z");

        // Exists x, ForAll y, Exists z: x + y = z
        var innerExists = context.Exists(z, (x + y) == z);
        var forallY = context.ForAll(y, innerExists);
        var outerExists = context.Exists(x, forallY);

        solver.Assert(outerExists);

        var result = solver.Check();
        Assert.That(result, Is.EqualTo(Z3Status.Satisfiable));
    }
}
