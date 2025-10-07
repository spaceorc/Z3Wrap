using System.Numerics;
using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Arrays;
using Spaceorc.Z3Wrap.Expressions.BitVectors;
using Spaceorc.Z3Wrap.Expressions.Functions;
using Spaceorc.Z3Wrap.Expressions.Logic;
using Spaceorc.Z3Wrap.Expressions.Numerics;
using Spaceorc.Z3Wrap.Expressions.Quantifiers;
using Spaceorc.Z3Wrap.Values.BitVectors;
using Spaceorc.Z3Wrap.Values.Numerics;

namespace Z3Wrap.Tests;

/// <summary>
/// Tests that ensure all examples from README.md compile and work correctly.
/// These tests guarantee that users can copy examples directly from the README.
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

        #region Example from README.md - Hero section (Basic Usage)

        using var context = new Z3Context();
        using var scope = context.SetUp();

        var x = context.IntConst("x");
        var y = context.IntConst("y");

        using var solver = context.CreateSolver();
        solver.Assert(x + y == 10); // Natural syntax
        solver.Assert(x * 2 == y - 1); // Mathematical operators

        if (solver.Check() == Z3Status.Satisfiable)
        {
            var model = solver.GetModel();
            Console.WriteLine($"x = {model.GetIntValue(x)}"); // BigInteger
            Console.WriteLine($"y = {model.GetIntValue(y)}");
        }

        #endregion

        #region Assertions

        Assert.That(
            console.Output,
            Is.EqualTo(
                """
                x = 3
                y = 7

                """
            )
        );

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

        #region Example from README.md - Natural Syntax section

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

        #region Example from README.md - Unlimited Precision section

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

        #region Example from README.md - Type Safety section

        // Compile-time type checking
        var prices = context.ArrayConst<IntExpr, RealExpr>("prices");
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
    public void CustomDataTypes_RealAndBv_WorkCorrectly()
    {
        #region Preparation

        using var console = new ConsoleCapture();

        #endregion

        #region Example from README.md - Custom .NET Data Types section

        // Real class - exact rational arithmetic (not decimal/double)
        var oneThird = new Real(1, 3);
        var twoThirds = new Real(2, 3);
        Console.WriteLine(oneThird + twoThirds); // "1" (exact)

        // Bv class - compile-time sized bitvectors with type safety
        var bv8 = new Bv<Size8>(0b10101010);
        var bv16 = bv8.Resize<Size16>(signed: false); // Resize to 16 bits
        Console.WriteLine(bv8.ToULong()); // 170
        Console.WriteLine(bv8.ToBinaryString()); // "10101010"

        // Direct arithmetic and bitwise operations
        var result = bv8 + new Bv<Size8>(5); // Type-safe arithmetic
        var masked = bv8 & new Bv<Size8>(0xFF); // Bitwise operations
        #endregion

        #region Assertions

        Assert.That(
            console.Output,
            Is.EqualTo(
                """
                1
                170
                10101010

                """
            )
        );

        Assert.That(Bv<Size16>.Size, Is.EqualTo(16U));
        Assert.That(result.ToULong(), Is.EqualTo(175UL));
        Assert.That(masked.ToULong(), Is.EqualTo(170UL));

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

        #region Example from README.md - BitVector Operations section

        var bv = context.BvConst<Size32>("bv");

        solver.Assert((bv & 0xFF) == 0x42); // Bitwise operations
        solver.Assert(bv << 2 == bv * 4); // Shift equivalence
        solver.Assert((bv ^ 0xFFFFFFFF) == ~bv); // XOR/NOT relationship

        // Overflow detection
        solver.Assert(context.AddNoOverflow(bv, 100U));

        #endregion

        #region Assertions

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var bvValue = model.GetBitVec(bv);
        Assert.That((bvValue.Value & 0xFF), Is.EqualTo(new BigInteger(0x42)));

        #endregion
    }

    [Test]
    public void Quantifiers_FirstOrderLogic_WorkCorrectly()
    {
        #region Preparation

        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        #endregion

        #region Example from README.md - Quantifiers section

        var x = context.IntConst("x");
        var y = context.IntConst("y");

        // Universal quantification: ∀x. x + 0 = x
        var identity = context.ForAll(x, x + 0 == x);
        solver.Assert(identity);

        // Existential quantification: ∃y. x + y = 10
        var existsSolution = context.Exists(y, x + y == 10);
        solver.Assert(existsSolution);

        // Multiple variables: ∀x,y. x * y = y * x
        var commutativity = context.ForAll(x, y, x * y == y * x);
        solver.Assert(commutativity);

        #endregion

        #region Assertions

        // All quantified formulas should be satisfiable (they express mathematical truths)
        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        // The solver should find a model that satisfies all quantifiers
        var model = solver.GetModel();
        Assert.That(model, Is.Not.Null);

        #endregion
    }

    [Test]
    public void UninterpretedFunctions_WorkCorrectly()
    {
        #region Preparation

        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();
        var x = context.IntConst("x");

        #endregion

        #region Example from README.md - Uninterpreted Functions section

        // Define function signature: Int → Int
        var func = context.Func<IntExpr, IntExpr>("f");

        // Apply function to arguments
        solver.Assert(func.Apply(5) == 10);
        solver.Assert(func.Apply(x) > 0);

        // Consistency: same inputs produce same outputs
        solver.Assert(func.Apply(5) == func.Apply(5));

        #endregion

        #region Assertions

        Assert.That(solver.Check(), Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.That(model.GetIntValue(func.Apply(5)), Is.EqualTo(new BigInteger(10)));

        #endregion
    }

    [Test]
    public void Optimization_MaximizeObjective_WorksCorrectly()
    {
        #region Preparation

        using var console = new ConsoleCapture();
        using var context = new Z3Context();
        using var scope = context.SetUp();

        #endregion

        #region Example from README.md - Optimization section

        using var optimizer = context.CreateOptimizer();

        var x = context.IntConst("x");
        var y = context.IntConst("y");

        // Constraints
        optimizer.Assert(x + y <= 100);
        optimizer.Assert(x >= 0);
        optimizer.Assert(y >= 0);

        // Objective: maximize 3x + 2y (returns typed handle)
        var objective = optimizer.Maximize(3 * x + 2 * y);

        if (optimizer.Check() == Z3Status.Satisfiable)
        {
            var model = optimizer.GetModel();
            var optimalValue = optimizer.GetUpper(objective); // Type-safe!
            Console.WriteLine($"Optimal: x={model.GetIntValue(x)}, y={model.GetIntValue(y)}");
            Console.WriteLine($"Max value: {model.GetIntValue(optimalValue)}");
        }

        #endregion

        #region Assertions

        Assert.That(
            console.Output,
            Is.EqualTo(
                """
                Optimal: x=100, y=0
                Max value: 300

                """
            )
        );

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

        #region Example from README.md - Solver Backtracking section

        solver.Push();
        solver.Assert(x < 10);
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
    public void CompleteFeatureSet_AllOperators_WorkCorrectly()
    {
        #region Preparation

        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        #endregion

        #region Example from README.md - Complete Feature Set section

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
        var bv1 = context.BvConst<Size8>("bv1");
        var bv2 = context.BvConst<Size8>("bv2");
        solver.Assert((bv1 & bv2) != 0);
        solver.Assert((bv1 | bv2) != 0);
        solver.Assert((bv1 ^ bv2) != (bv1 & bv2));
        solver.Assert(bv1 << 1 == bv1 * 2);

        // Arrays: Generic ArrayExpr<TIndex, TValue> with natural indexing
        var arr = context.ArrayConst<IntExpr, IntExpr>("arr");
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

    [Test]
    public void UnsatisfiableCores_ConflictDebugging_WorksCorrectly()
    {
        #region Preparation

        using var console = new ConsoleCapture();

        #endregion

        #region Example from README.md - Unsatisfiable Cores (Debugging Conflicts) section

        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var x = context.IntConst("x");

        // Create boolean trackers for each constraint
        var t1 = context.BoolConst("t1");
        var t2 = context.BoolConst("t2");
        var t3 = context.BoolConst("t3");

        // Link trackers to constraints
        solver.Assert(t1.Implies(x > 100));
        solver.Assert(t2.Implies(x < 50));
        solver.Assert(t3.Implies(x > 0));

        // Check with tracked assumptions
        if (solver.CheckAssumptions(t1, t2, t3) == Z3Status.Unsatisfiable)
        {
            // Get minimal conflicting subset
            var core = solver.GetUnsatCore();
            Console.WriteLine("Conflicting constraints:");
            foreach (var tracker in core)
            {
                Console.WriteLine($"  {tracker}");
            }
            // Output: t1, t2 (the actual conflict)
        }

        #endregion

        #region Assertions

        var output = console.Output;
        Assert.That(output, Does.Contain("Conflicting constraints:"));
        Assert.That(output, Does.Contain("t1"));
        Assert.That(output, Does.Contain("t2"));
        // t3 should not be in the core (it's not part of the conflict)

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
