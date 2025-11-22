using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Numerics;
using Spaceorc.Z3Wrap.Expressions.Sequences;

namespace Z3Wrap.Tests.Expressions.Sequences;

[TestFixture]
public class SeqExprFactoryTests
{
    [Test]
    public void SeqConst_CreatesNamedConstant()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var seq = context.SeqConst<IntExpr>("mySeq");

        Assert.That(seq, Is.Not.Null);
        Assert.That(seq.ToString(), Does.Contain("mySeq"));
    }

    [Test]
    public void SeqEmpty_CreatesEmptySequence()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var empty = context.SeqEmpty<IntExpr>();
        var length = empty.Length();

        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetIntValue(length), Is.EqualTo(new BigInteger(0)));
    }

    [Test]
    public void SeqUnit_CreatesSingleElementSequence()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var seq = context.SeqUnit(context.Int(42));
        var length = seq.Length();
        var first = seq[0];

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetIntValue(length), Is.EqualTo(new BigInteger(1)));
            Assert.That(model.GetIntValue(first), Is.EqualTo(new BigInteger(42)));
        });
    }

    [Test]
    public void Seq_EmptyArray_CreatesEmptySequence()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var seq = context.Seq<IntExpr>();
        var length = seq.Length();

        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetIntValue(length), Is.EqualTo(new BigInteger(0)));
    }

    [Test]
    public void Seq_MultipleElements_CreatesCorrectSequence()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var seq = context.Seq<IntExpr>(1, 2, 3);
        var length = seq.Length();
        var first = seq[0];
        var second = seq[1];
        var third = seq[2];

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetIntValue(length), Is.EqualTo(new BigInteger(3)));
            Assert.That(model.GetIntValue(first), Is.EqualTo(new BigInteger(1)));
            Assert.That(model.GetIntValue(second), Is.EqualTo(new BigInteger(2)));
            Assert.That(model.GetIntValue(third), Is.EqualTo(new BigInteger(3)));
        });
    }

    [Test]
    public void SeqConcat_TwoSequences_ConcatenatesCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var seq1 = context.Seq<IntExpr>(1, 2);
        var seq2 = context.Seq<IntExpr>(3, 4);

        var resultViaContext = context.SeqConcat(seq1, seq2);

        var length = resultViaContext.Length();
        var elem0 = resultViaContext[0];
        var elem1 = resultViaContext[1];
        var elem2 = resultViaContext[2];
        var elem3 = resultViaContext[3];

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetIntValue(length), Is.EqualTo(new BigInteger(4)));
            Assert.That(model.GetIntValue(elem0), Is.EqualTo(new BigInteger(1)));
            Assert.That(model.GetIntValue(elem1), Is.EqualTo(new BigInteger(2)));
            Assert.That(model.GetIntValue(elem2), Is.EqualTo(new BigInteger(3)));
            Assert.That(model.GetIntValue(elem3), Is.EqualTo(new BigInteger(4)));
        });
    }

    [Test]
    public void SeqConcat_ThreeSequences_ConcatenatesCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var seq1 = context.Seq<IntExpr>(1);
        var seq2 = context.Seq<IntExpr>(2);
        var seq3 = context.Seq<IntExpr>(3);

        var result = context.SeqConcat(seq1, seq2, seq3);
        var length = result.Length();

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetIntValue(length), Is.EqualTo(new BigInteger(3)));
    }

    [Test]
    public void SeqConcat_EmptyArray_ThrowsException()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        Assert.Throws<ArgumentException>(() => context.SeqConcat<IntExpr>());
    }
}
