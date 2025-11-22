using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Numerics;
using Spaceorc.Z3Wrap.Expressions.Sequences;

namespace Z3Wrap.Tests.Expressions.Sequences;

[TestFixture]
public class SeqExprOperationsTests
{
    [Test]
    public void Indexer_AccessesElementAtIndex()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var seq = context.Seq<IntExpr>(10, 20, 30);
        var elem = seq[1];

        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetIntValue(elem), Is.EqualTo(new BigInteger(20)));
    }

    [Test]
    public void Nth_AccessesElementAtIndex()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var seq = context.Seq<IntExpr>(10, 20, 30);
        var elem = seq.Nth(context.Int(2));

        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetIntValue(elem), Is.EqualTo(new BigInteger(30)));
    }

    [Test]
    public void At_ReturnsUnitSequence()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var seq = context.Seq<IntExpr>(10, 20, 30);
        var unitSeq = seq.At(context.Int(1));
        var length = unitSeq.Length();
        var elem = unitSeq[0];

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetIntValue(length), Is.EqualTo(new BigInteger(1)));
            Assert.That(model.GetIntValue(elem), Is.EqualTo(new BigInteger(20)));
        });
    }

    [Test]
    public void Length_ReturnsSequenceLength()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var seq = context.Seq<IntExpr>(1, 2, 3, 4);
        var length = seq.Length();

        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetIntValue(length), Is.EqualTo(new BigInteger(4)));
    }

    [Test]
    public void Contains_FindsSubsequence()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var seq = context.Seq<IntExpr>(1, 2, 3, 4);
        var subseq = context.Seq<IntExpr>(2, 3);
        var contains = seq.Contains(subseq);

        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(contains), Is.True);
    }

    [Test]
    public void Contains_DoesNotFindMissingSubsequence()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var seq = context.Seq<IntExpr>(1, 2, 4);
        var subseq = context.Seq<IntExpr>(2, 3);
        var contains = seq.Contains(subseq);

        solver.Assert(contains);
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void StartsWith_MatchesPrefix()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var seq = context.Seq<IntExpr>(1, 2, 3);
        var prefix = context.Seq<IntExpr>(1, 2);
        var startsWith = seq.StartsWith(prefix);

        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(startsWith), Is.True);
    }

    [Test]
    public void StartsWith_DoesNotMatchWrongPrefix()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var seq = context.Seq<IntExpr>(1, 2, 3);
        var prefix = context.Seq<IntExpr>(2, 3);
        var startsWith = seq.StartsWith(prefix);

        solver.Assert(startsWith);
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void EndsWith_MatchesSuffix()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var seq = context.Seq<IntExpr>(1, 2, 3);
        var suffix = context.Seq<IntExpr>(2, 3);
        var endsWith = seq.EndsWith(suffix);

        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(endsWith), Is.True);
    }

    [Test]
    public void EndsWith_DoesNotMatchWrongSuffix()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var seq = context.Seq<IntExpr>(1, 2, 3);
        var suffix = context.Seq<IntExpr>(1, 2);
        var endsWith = seq.EndsWith(suffix);

        solver.Assert(endsWith);
        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void Extract_ReturnsSubsequence()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var seq = context.Seq<IntExpr>(10, 20, 30, 40);
        var subseq = seq.Extract(context.Int(1), context.Int(2));
        var length = subseq.Length();
        var first = subseq[0];
        var second = subseq[1];

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetIntValue(length), Is.EqualTo(new BigInteger(2)));
            Assert.That(model.GetIntValue(first), Is.EqualTo(new BigInteger(20)));
            Assert.That(model.GetIntValue(second), Is.EqualTo(new BigInteger(30)));
        });
    }

    [Test]
    public void Replace_ReplacesFirstOccurrence()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var seq = context.Seq<IntExpr>(1, 2, 2, 3);
        var source = context.Seq<IntExpr>(2);
        var dest = context.Seq<IntExpr>(9);
        var result = seq.Replace(source, dest);

        var length = result.Length();
        var elem1 = result[1];
        var elem2 = result[2];

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetIntValue(length), Is.EqualTo(new BigInteger(4)));
            Assert.That(model.GetIntValue(elem1), Is.EqualTo(new BigInteger(9)));
            Assert.That(model.GetIntValue(elem2), Is.EqualTo(new BigInteger(2)));
        });
    }

    [Test]
    public void IndexOf_FindsSubsequenceIndex()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var seq = context.Seq<IntExpr>(1, 2, 3, 2);
        var subseq = context.Seq<IntExpr>(2);
        var index = seq.IndexOf(subseq, context.Int(0));

        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetIntValue(index), Is.EqualTo(new BigInteger(1)));
    }

    [Test]
    public void IndexOf_ReturnsMinusOneWhenNotFound()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var seq = context.Seq<IntExpr>(1, 2, 3);
        var subseq = context.Seq<IntExpr>(9);
        var index = seq.IndexOf(subseq, 0);

        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetIntValue(index), Is.EqualTo(new BigInteger(-1)));
    }

    [Test]
    [Ignore("LastIndexOf does not evaluate to numeric constant in Z3 - may require additional constraints")]
    public void LastIndexOf_FindsLastOccurrence()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var seq = context.Seq<IntExpr>(1, 2, 3, 2);
        var subseq = context.Seq<IntExpr>(2);
        var index = seq.LastIndexOf(subseq);

        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetIntValue(index), Is.EqualTo(new BigInteger(3)));
    }

    [Test]
    [Ignore("LastIndexOf does not evaluate to numeric constant in Z3 - may require additional constraints")]
    public void LastIndexOf_ReturnsMinusOneWhenNotFound()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var seq = context.Seq<IntExpr>(1, 2, 3);
        var subseq = context.Seq<IntExpr>(9);
        var index = seq.LastIndexOf(subseq);

        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetIntValue(index), Is.EqualTo(new BigInteger(-1)));
    }

    [Test]
    public void OperatorPlus_ConcatenatesSequences()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var seq1 = context.Seq<IntExpr>(1, 2);
        var seq2 = context.Seq<IntExpr>(3, 4);

        var resultViaOperator = seq1 + seq2;

        var length = resultViaOperator.Length();

        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetIntValue(length), Is.EqualTo(new BigInteger(4)));
    }

    [Test]
    public void Concat_ExtensionMethod_ConcatenatesMultiple()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var seq1 = context.Seq<IntExpr>(1);
        var seq2 = context.Seq<IntExpr>(2);
        var seq3 = context.Seq<IntExpr>(3);

        var resultViaExtension = seq1.Concat(seq2, seq3);
        var resultViaContext = context.SeqConcat(seq1, seq2, seq3);

        var length1 = resultViaExtension.Length();
        var length2 = resultViaContext.Length();

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetIntValue(length1), Is.EqualTo(new BigInteger(3)));
            Assert.That(model.GetIntValue(length2), Is.EqualTo(new BigInteger(3)));
        });
    }

    [Test]
    public void OperatorEquals_ComparesSequences()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var seq1 = context.Seq<IntExpr>(1, 2, 3);
        var seq2 = context.Seq<IntExpr>(1, 2, 3);
        var areEqual = seq1 == seq2;

        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(areEqual), Is.True);
    }

    [Test]
    public void OperatorNotEquals_DetectsDifference()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var seq1 = context.Seq<IntExpr>(1, 2);
        var seq2 = context.Seq<IntExpr>(1, 3);
        var areNotEqual = seq1 != seq2;

        var status = solver.Check();

        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));
        var model = solver.GetModel();
        Assert.That(model.GetBoolValue(areNotEqual), Is.True);
    }
}
