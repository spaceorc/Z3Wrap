using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Functions;
using Spaceorc.Z3Wrap.Expressions.Numerics;
using Spaceorc.Z3Wrap.Expressions.Sequences;

namespace Z3Wrap.Tests.Expressions.Sequences;

[TestFixture]
public class SeqExprHigherOrderTests
{
    [Test]
    public void Map_DoubleElements_TransformsCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Create sequence [1, 2, 3]
        var seq = context.Seq(context.Int(1), context.Int(2), context.Int(3));

        // Create lambda: x => x * 2
        var x = context.IntConst("x");
        var doubler = context.Lambda(x, x * context.Int(2));

        // Map doubler over sequence
        var doubled = seq.Map(doubler);

        // Result should be [2, 4, 6]
        var expected = context.Seq(context.Int(2), context.Int(4), context.Int(6));

        solver.Assert(doubled == expected);
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Map_IncrementElements_ComputesCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Create sequence [10, 20, 30]
        var seq = context.Seq(context.Int(10), context.Int(20), context.Int(30));

        // Create lambda: x => x + 1
        var x = context.IntConst("x");
        var incrementer = context.Lambda(x, x + context.Int(1));

        // Map incrementer over sequence
        var incremented = context.Map(seq, incrementer);

        // Result should be [11, 21, 31]
        var expected = context.Seq(context.Int(11), context.Int(21), context.Int(31));

        solver.Assert(incremented == expected);
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Mapi_AddIndex_ComputesCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Create sequence [10, 20, 30]
        var seq = context.Seq(context.Int(10), context.Int(20), context.Int(30));

        // Create lambda: (i, x) => x + i
        var i = context.IntConst("i");
        var x = context.IntConst("x");
        var indexAdder = context.Lambda(i, x, x + i);

        // Map with index starting at 0
        var result = seq.Mapi(indexAdder, context.Int(0));

        // Result should be [10+0, 20+1, 30+2] = [10, 21, 32]
        var expected = context.Seq(context.Int(10), context.Int(21), context.Int(32));

        solver.Assert(result == expected);
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Mapi_CustomStartIndex_ComputesCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Create sequence [100, 200, 300]
        var seq = context.Seq(context.Int(100), context.Int(200), context.Int(300));

        // Create lambda: (i, x) => i
        var i = context.IntConst("i");
        var x = context.IntConst("x");
        var indexSelector = context.Lambda(i, x, i);

        // Map with index starting at 5
        var result = context.Mapi(seq, indexSelector, context.Int(5));

        // Result should be [5, 6, 7]
        var expected = context.Seq(context.Int(5), context.Int(6), context.Int(7));

        solver.Assert(result == expected);
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Foldl_Sum_ComputesTotal()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Create sequence [1, 2, 3, 4]
        var seq = context.Seq(context.Int(1), context.Int(2), context.Int(3), context.Int(4));

        // Create lambda: (acc, x) => acc + x
        var acc = context.IntConst("acc");
        var x = context.IntConst("x");
        var sum = context.Lambda(acc, x, acc + x);

        // Fold with initial accumulator 0
        var total = seq.Foldl(sum, context.Int(0));

        // Total should be 10
        solver.Assert(total == context.Int(10));
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Foldl_Product_ComputesResult()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Create sequence [2, 3, 4]
        var seq = context.Seq(context.Int(2), context.Int(3), context.Int(4));

        // Create lambda: (acc, x) => acc * x
        var acc = context.IntConst("acc");
        var x = context.IntConst("x");
        var product = context.Lambda(acc, x, acc * x);

        // Fold with initial accumulator 1
        var result = context.Foldl(seq, product, context.Int(1));

        // Result should be 24
        solver.Assert(result == context.Int(24));
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Foldl_WithNonZeroInitial_ComputesCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Create sequence [5, 10, 15]
        var seq = context.Seq(context.Int(5), context.Int(10), context.Int(15));

        // Create lambda: (acc, x) => acc + x
        var acc = context.IntConst("acc");
        var x = context.IntConst("x");
        var sum = context.Lambda(acc, x, acc + x);

        // Fold with initial accumulator 100
        var total = seq.Foldl(sum, context.Int(100));

        // Total should be 100 + 5 + 10 + 15 = 130
        solver.Assert(total == context.Int(130));
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Foldli_WithIndex_TracksIndexCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Create sequence [10, 20, 30]
        var seq = context.Seq(context.Int(10), context.Int(20), context.Int(30));

        // Create lambda: (i, acc, x) => acc + i
        var i = context.IntConst("i");
        var acc = context.IntConst("acc");
        var x = context.IntConst("x");
        var indexSum = context.Lambda(i, acc, x, acc + i);

        // Fold with index starting at 0, accumulator 0
        var result = seq.Foldli(indexSum, context.Int(0), context.Int(0));

        // Result should be 0 + 0 + 1 + 2 = 3
        solver.Assert(result == context.Int(3));
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Foldli_CustomStartIndex_ComputesCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Create sequence [100, 200]
        var seq = context.Seq(context.Int(100), context.Int(200));

        // Create lambda: (i, acc, x) => acc + x + i
        var i = context.IntConst("i");
        var acc = context.IntConst("acc");
        var x = context.IntConst("x");
        var combined = context.Lambda(i, acc, x, acc + x + i);

        // Fold with index starting at 10, accumulator 0
        var result = context.Foldli(seq, combined, context.Int(10), context.Int(0));

        // Result should be 0 + (100 + 10) + (200 + 11) = 321
        solver.Assert(result == context.Int(321));
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Map_EmptySequence_ReturnsEmpty()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Create empty sequence
        var seq = context.SeqEmpty<IntExpr>();

        // Create lambda: x => x * 2
        var x = context.IntConst("x");
        var doubler = context.Lambda(x, x * context.Int(2));

        // Map over empty sequence
        var result = seq.Map(doubler);

        // Result should be empty
        solver.Assert(result == context.SeqEmpty<IntExpr>());
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Foldl_EmptySequence_ReturnsInitialAccumulator()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Create empty sequence
        var seq = context.SeqEmpty<IntExpr>();

        // Create lambda: (acc, x) => acc + x
        var acc = context.IntConst("acc");
        var x = context.IntConst("x");
        var sum = context.Lambda(acc, x, acc + x);

        // Fold over empty sequence with initial accumulator 42
        var result = seq.Foldl(sum, context.Int(42));

        // Result should be 42 (unchanged)
        solver.Assert(result == context.Int(42));
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void Map_ChainedOperations_ComputesCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Create sequence [1, 2, 3]
        var seq = context.Seq(context.Int(1), context.Int(2), context.Int(3));

        // Create lambda: x => x * 2
        var x = context.IntConst("x");
        var doubler = context.Lambda(x, x * context.Int(2));

        // Create lambda: x => x + 10
        var y = context.IntConst("y");
        var addTen = context.Lambda(y, y + context.Int(10));

        // Chain map operations: double then add 10
        var result = seq.Map(doubler).Map(addTen);

        // Result should be [12, 14, 16]
        var expected = context.Seq(context.Int(12), context.Int(14), context.Int(16));

        solver.Assert(result == expected);
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void MapThenFold_CombinedOperations_ComputesCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // Create sequence [1, 2, 3]
        var seq = context.Seq(context.Int(1), context.Int(2), context.Int(3));

        // Map: x => x * x (square elements)
        var x = context.IntConst("x");
        var square = context.Lambda(x, x * x);
        var squared = seq.Map(square); // [1, 4, 9]

        // Fold: (acc, x) => acc + x (sum)
        var acc = context.IntConst("acc");
        var elem = context.IntConst("elem");
        var sum = context.Lambda(acc, elem, acc + elem);
        var total = squared.Foldl(sum, context.Int(0)); // 1 + 4 + 9 = 14

        solver.Assert(total == context.Int(14));
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
    }
}
