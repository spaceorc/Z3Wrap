using System.Numerics;
using Spaceorc.Z3Wrap;
using Spaceorc.Z3Wrap.DataTypes;
using Spaceorc.Z3Wrap.Expressions;
using Spaceorc.Z3Wrap.Extensions;

namespace Z3Wrap.Tests.Unit;

/// <summary>
///     Tests that ensure all examples from README.md compile and work correctly.
///     These tests guarantee that users can copy examples directly from the README.
/// </summary>
[TestFixture]
public class ReadmeExamplesTests
{
    [Test]
    public void HeroSection_BasicUsage_WorksCorrectly()
    {
        #region Preparation

        using var console = new ConsoleCapture();

        #endregion

        #region Example from README.md (lines 9-26)

        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.IntConst("x");
        var y = context.IntConst("y");

        using var solver = context.CreateSolver();
        solver.Assert(x + y == 10);        // Natural syntax
        solver.Assert(x * 2 == y - 1);     // Mathematical operators

        if (solver.Check() == Z3Status.Satisfiable)
        {
            var model = solver.GetModel();
            Console.WriteLine($"x = {model.GetIntValue(x)}");  // BigInteger
            Console.WriteLine($"y = {model.GetIntValue(y)}");
        }

        #endregion

        #region Assertions

        Assert.That(console.Output, Is.EqualTo("""
                                               x = 3
                                               y = 7

                                               """));

        #endregion
    }

    [Test]
    public void NaturalSyntax_ComparisonWithZ3API_ShowsDifference()
    {
        #region Preparation

        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();
        var x = context.IntConst("x");
        var y = context.IntConst("y");
        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        var r = context.BoolConst("r");

        #endregion

        #region Example from README.md (lines 31-38)

        // Z3Wrap - readable
        solver.Assert(x + y == 10);
        solver.Assert(p.Implies(q & r));

        // Raw Z3 - verbose
        // solver.Assert(ctx.MkEq(ctx.MkAdd(x, y), ctx.MkInt(10)));
        // solver.Assert(ctx.MkImplies(p, ctx.MkAnd(q, r)));

        #endregion

        #region Assertions

        Assert.That(solver.Check(), Is.Not.EqualTo(Z3Status.Unknown));

        #endregion
    }

    [Test]
    public void UnlimitedPrecision_BigIntegerAndRational_WorksCorrectly()
    {
        #region Preparation

        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();
        var x = context.IntConst("x");
        var r = context.RealConst("r");

        #endregion

        #region Example from README.md (lines 41-52)

        // BigInteger - no integer overflow
        var huge = BigInteger.Parse("999999999999999999999999999999");
        solver.Assert(x == huge);

        // Exact rationals - no floating point errors
        solver.Assert(r == new Real(1, 3)); // Exactly 1/3
        solver.Assert(r * 3 == 1); // Perfect arithmetic

        #endregion

        #region Assertions

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetIntValue(x), Is.EqualTo(huge));
        Assert.That(model.GetNumericValueAsString(r), Is.EqualTo("1/3"));

        #endregion
    }

    [Test]
    public void TypeSafety_ArraysAndConversions_WorkCorrectly()
    {
        #region Preparation

        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();
        var x = context.IntConst("x");
        var r = context.RealConst("r");

        #endregion

        #region Example from README.md (lines 55-63)

        // Compile-time type checking
        var prices = context.ArrayConst<Z3IntExpr, Z3RealExpr>("prices");
        solver.Assert(prices[0] == 10.5m); // Index: Int, Value: Real
        solver.Assert(prices[1] > prices[0]); // Type-safe comparisons

        // Seamless conversions
        solver.Assert(x.ToReal() + r == 5.5m); // Int → Real

        #endregion

        #region Assertions

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        #endregion
    }

    [Test]
    public void CustomDataTypes_RealAndBitVec_WorkCorrectly()
    {
        #region Preparation

        using var console = new ConsoleCapture();

        #endregion

        #region Example from README.md (lines 65-82)

        // Real class - exact rational arithmetic (not decimal/double)
        var oneThird = new Real(1, 3);
        var twoThirds = new Real(2, 3);
        Console.WriteLine(oneThird + twoThirds);  // "1" (exact)

        // BitVec class - proper .NET bitvector type with operations
        var bv8 = new BitVec(0b10101010, 8);
        var bv16 = bv8.Resize(16);       // Zero-extend to 16 bits
        var extracted = bv8.Extract(7, 4); // Extract bits 7-4
        Console.WriteLine(bv8.ToInt());     // 170
        Console.WriteLine(bv8.ToBinaryString()); // "10101010"

        // Direct arithmetic and bitwise operations
        var result = bv8 + new BitVec(5, 8);   // BitVec arithmetic
        var masked = bv8 & 0xFF;               // Bitwise operations

        #endregion
        
        #region Assertions
        
        Assert.That(console.Output, Is.EqualTo("""
                                               1
                                               170
                                               10101010

                                               """));

        Assert.That(bv16.Size, Is.EqualTo(16U));
        Assert.That(extracted.Size, Is.EqualTo(4U));
        Assert.That(result.ToInt(), Is.EqualTo(175));
        Assert.That(masked.ToInt(), Is.EqualTo(170));

        #endregion
    }

    [Test]
    public void BitVectorOperations_BinaryOpsAndBoundaryChecks_WorkCorrectly()
    {
        #region Preparation

        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        #endregion

        #region Example from README.md (lines 104-114)

        var bv = context.BitVecConst("bv", 32);

        solver.Assert((bv & 0xFF) == 0x42); // Bitwise operations
        solver.Assert(bv << 2 == bv * 4); // Shift equivalence
        solver.Assert((bv ^ 0xFFFFFFFF) == ~bv); // XOR/NOT relationship

        // Overflow detection
        solver.Assert(context.BitVecBoundaryCheck().Add(bv, 100).NoOverflow());

        #endregion

        #region Assertions

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var bvValue = model.GetBitVec(bv);
        Assert.That(bvValue.Value & 0xFF, Is.EqualTo(new BigInteger(0x42)));

        #endregion
    }

    [Test]
    public void SolverBacktracking_PushPopOperations_WorkCorrectly()
    {
        #region Preparation

        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();
        var x = context.IntConst("x");
        solver.Assert(x > 15); // Make x >= 16 to ensure unsatisfiability
        solver.Assert(x < 20);

        #endregion

        #region Example from README.md (lines 116-124)

        solver.Push();
        solver.Assert(x < 10); // This will be unsatisfiable
        if (solver.Check() == Z3Status.Unsatisfiable)
        {
            solver.Pop(); // Backtrack
            solver.Assert(x >= 10);
        }

        #endregion

        #region Assertions

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var xValue = model.GetIntValue(x);
        Assert.That(xValue, Is.GreaterThanOrEqualTo(new BigInteger(16)));
        Assert.That(xValue, Is.LessThan(new BigInteger(20)));

        #endregion
    }

    [Test]
    public void FeatureSetExamples_AllOperators_WorkCorrectly()
    {
        #region Preparation

        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        #endregion

        #region Example from README.md Complete Feature Set (lines 93-100)

        // Booleans: p & q, p | q, !p, p.Implies(q)
        var p = context.BoolConst("p");
        var q = context.BoolConst("q");
        solver.Assert(p & q);
        solver.Assert(p | q);
        solver.Assert(!context.BoolConst("false_var"));
        solver.Assert(p.Implies(q));

        // Integers: BigInteger arithmetic with +, -, *, /, %
        var i1 = context.IntConst("i1");
        var i2 = context.IntConst("i2");
        solver.Assert(i1 + i2 == 10);
        solver.Assert(i1 - i2 == 2);
        solver.Assert(i1 * i2 == 24);
        solver.Assert(i1 % i2 == 2);

        // Reals: Exact rational arithmetic with Real(1,3) fractions
        var r1 = context.RealConst("r1");
        solver.Assert(r1 == new Real(1, 3));

        // BitVectors: Binary ops &, |, ^, ~, <<, >> with overflow checks
        var bv1 = context.BitVecConst("bv1", 8);
        var bv2 = context.BitVecConst("bv2", 8);
        solver.Assert((bv1 & bv2) != 0);
        solver.Assert((bv1 | bv2) != 0);
        solver.Assert((bv1 ^ bv2) != (bv1 & bv2));
        solver.Assert(bv1 << 1 == bv1 * 2);

        // Arrays: Generic Z3ArrayExpr<TIndex, TValue> with natural indexing
        var arr = context.ArrayConst<Z3IntExpr, Z3IntExpr>("arr");
        solver.Assert(arr[0] == 42);

        #endregion

        #region Assertions

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetIntValue(i1), Is.EqualTo(new BigInteger(6)));
        Assert.That(model.GetIntValue(i2), Is.EqualTo(new BigInteger(4)));
        Assert.That(model.GetNumericValueAsString(r1), Is.EqualTo("1/3"));
        Assert.That(model.GetBoolValue(p), Is.True);
        Assert.That(model.GetBoolValue(q), Is.True);

        #endregion
    }

    private sealed class ConsoleCapture : IDisposable
    {
        private readonly TextWriter originalOut;
        private readonly StringWriter stringWriter;

        public ConsoleCapture()
        {
            originalOut = Console.Out;
            stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
        }

        public string Output => stringWriter.ToString();

        public void Dispose()
        {
            Console.SetOut(originalOut);
            stringWriter.Dispose();
        }
    }
}