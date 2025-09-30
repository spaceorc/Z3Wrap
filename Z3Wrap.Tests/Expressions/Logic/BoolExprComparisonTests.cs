using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Expressions.Logic;

namespace Z3Wrap.Tests.Expressions.Logic;

/// <summary>
/// Tests for boolean equality comparison operations with comprehensive syntax variant coverage.
/// </summary>
[TestFixture]
public class BoolExprComparisonTests
{
    [TestCase(true, true, true)]
    [TestCase(true, false, false)]
    [TestCase(false, true, false)]
    [TestCase(false, false, true)]
    public void Equals_TwoValues_ComputesCorrectResult(bool aValue, bool bValue, bool expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Bool(aValue);
        var b = context.Bool(bValue);

        var result = a == b;
        var resultViaBoolLeft = aValue == b;
        var resultViaBoolRight = a == bValue;
        var resultViaContext = context.Eq(a, b);
        var resultViaContextBoolLeft = context.Eq(aValue, b);
        var resultViaContextBoolRight = context.Eq(a, bValue);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(result), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaBoolLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaBoolRight), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContext), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextBoolLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaContextBoolRight), Is.EqualTo(expected));
        });
    }

    [TestCase(true, true, false)]
    [TestCase(true, false, true)]
    [TestCase(false, true, true)]
    [TestCase(false, false, false)]
    public void NotEquals_TwoValues_ComputesCorrectResult(bool aValue, bool bValue, bool expected)
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.Bool(aValue);
        var b = context.Bool(bValue);

        var result = a != b;
        var resultViaBoolLeft = aValue != b;
        var resultViaBoolRight = a != bValue;

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(result), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaBoolLeft), Is.EqualTo(expected));
            Assert.That(model.GetBoolValue(resultViaBoolRight), Is.EqualTo(expected));
        });
    }
}
