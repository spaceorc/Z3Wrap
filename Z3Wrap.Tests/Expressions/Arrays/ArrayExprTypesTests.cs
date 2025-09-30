using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Arrays;
using Spaceorc.Z3Wrap.Expressions.BitVectors;
using Spaceorc.Z3Wrap.Expressions.Common;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Expressions.Numerics;
using Spaceorc.Z3Wrap.Values.BitVectors;

namespace Z3Wrap.Tests.Expressions.Arrays;

[TestFixture]
public class ArrayExprTypesTests
{
    [Test]
    public void RealArrays_DecimalConstraints_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var prices = context.ArrayConst<IntExpr, RealExpr>("prices");

        solver.Assert(prices[1] == 19.99m);
        solver.Assert(prices[2] == 29.95m);
        solver.Assert(prices[1] < prices[2]);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var price1Str = model.GetNumericValueAsString(prices[1]);
        var price2Str = model.GetNumericValueAsString(prices[2]);

        Assert.Multiple(() =>
        {
            Assert.That(price1Str, Is.EqualTo("1999/100"));
            Assert.That(price2Str, Is.EqualTo("599/20"));
        });
    }

    [Test]
    public void BooleanArrays_LogicalConstraints_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var flags = context.ArrayConst<IntExpr, BoolExpr>("flags");

        solver.Assert(flags[0] == true);
        solver.Assert(flags[1] == false);
        solver.Assert(flags[0] & !flags[1]);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void BooleanArrays_NegatedConstraint_Unsatisfiable()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var flags = context.ArrayConst<IntExpr, BoolExpr>("flags");

        solver.Assert(flags[0] == true);
        solver.Assert(flags[1] == false);
        solver.Assert(!(flags[0] & !flags[1]));

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void IntegerArrays_ArithmeticConstraints_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var values = context.ArrayConst<IntExpr, IntExpr>("values");

        solver.Assert(values[0] == 10);
        solver.Assert(values[1] == 20);
        solver.Assert(values[0] + values[1] == 30);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetIntValue(values[0]), Is.EqualTo(new BigInteger(10)));
            Assert.That(model.GetIntValue(values[1]), Is.EqualTo(new BigInteger(20)));
        });
    }

    [Test]
    public void RealArrays_ArithmeticConstraints_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var measurements = context.ArrayConst<IntExpr, RealExpr>("measurements");

        solver.Assert(measurements[0] == 10.5m);
        solver.Assert(measurements[1] == 20.25m);
        solver.Assert(measurements[0] + measurements[1] == 30.75m);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [TestCase(TypeArgs = [typeof(BoolExpr), typeof(BoolExpr)])]
    [TestCase(TypeArgs = [typeof(BoolExpr), typeof(IntExpr)])]
    [TestCase(TypeArgs = [typeof(BoolExpr), typeof(RealExpr)])]
    [TestCase(TypeArgs = [typeof(IntExpr), typeof(IntExpr)])]
    [TestCase(TypeArgs = [typeof(IntExpr), typeof(RealExpr)])]
    [TestCase(TypeArgs = [typeof(IntExpr), typeof(BoolExpr)])]
    [TestCase(TypeArgs = [typeof(RealExpr), typeof(IntExpr)])]
    [TestCase(TypeArgs = [typeof(RealExpr), typeof(RealExpr)])]
    [TestCase(TypeArgs = [typeof(RealExpr), typeof(BoolExpr)])]
    [TestCase(TypeArgs = [typeof(BvExpr<Size32>), typeof(BvExpr<Size32>)])]
    [TestCase(TypeArgs = [typeof(IntExpr), typeof(BvExpr<Size32>)])]
    [TestCase(TypeArgs = [typeof(BvExpr<Size32>), typeof(IntExpr)])]
    public void ArrayTypes_VariousIndexAndValueTypes_Works<TIndex, TValue>()
        where TIndex : Z3Expr, IExprType<TIndex>
        where TValue : Z3Expr, IExprType<TValue>
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var array = context.ArrayConst<TIndex, TValue>("arr");

        Assert.Multiple(() =>
        {
            Assert.That(array, Is.Not.Null);
            Assert.That(array, Is.TypeOf<ArrayExpr<TIndex, TValue>>());
        });

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));
    }

    [Test]
    public void BitVectorArrays_IntToBitVector_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var memory = context.ArrayConst<IntExpr, BvExpr<Size32>>("memory");

        solver.Assert(memory[0] == 0xDEADBEEFu);
        solver.Assert(memory[1] == 0xCAFEBABEu);
        solver.Assert(memory[0] != memory[1]);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(memory[0]).Value, Is.EqualTo(new BigInteger(0xDEADBEEFu)));
            Assert.That(model.GetBitVec(memory[1]).Value, Is.EqualTo(new BigInteger(0xCAFEBABEu)));
        });
    }

    [Test]
    public void BitVectorArrays_BitVectorToBitVector_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var lookup = context.ArrayConst<BvExpr<Size8>, BvExpr<Size32>>("lookup");

        solver.Assert(lookup[0xFFu] == 0xDEADBEEFu);
        solver.Assert(lookup[0x00u] == 0xCAFEBABEu);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(lookup[0xFFu]).Value, Is.EqualTo(new BigInteger(0xDEADBEEFu)));
            Assert.That(model.GetBitVec(lookup[0x00u]).Value, Is.EqualTo(new BigInteger(0xCAFEBABEu)));
        });
    }

    [Test]
    public void BitVectorArrays_BitVectorToInt_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var counters = context.ArrayConst<BvExpr<Size32>, IntExpr>("counters");

        solver.Assert(counters[0x01u] == 100);
        solver.Assert(counters[0x02u] == 200);
        solver.Assert(counters[0x01u] + counters[0x02u] == 300);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetIntValue(counters[0x01u]), Is.EqualTo(new BigInteger(100)));
            Assert.That(model.GetIntValue(counters[0x02u]), Is.EqualTo(new BigInteger(200)));
        });
    }

    [Test]
    public void BitVectorArrays_ArithmeticConstraints_Works()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var registers = context.ArrayConst<IntExpr, BvExpr<Size32>>("registers");

        solver.Assert(registers[0] == 10u);
        solver.Assert(registers[1] == 20u);
        solver.Assert(registers[0] + registers[1] == 30u);

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBitVec(registers[0]).Value, Is.EqualTo(new BigInteger(10)));
            Assert.That(model.GetBitVec(registers[1]).Value, Is.EqualTo(new BigInteger(20)));
        });
    }
}
