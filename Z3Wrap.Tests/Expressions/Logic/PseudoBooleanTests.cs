using Spaceorc.Z3Wrap.Core;
using Spaceorc.Z3Wrap.Expressions.Logic;

namespace Spaceorc.Z3Wrap.Tests.Expressions.Logic;

[TestFixture]
public class PseudoBooleanTests
{
    [Test]
    public void AtMost_ThreeBooleans_MaxTwo_AllowsTwoTrue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.BoolConst("a");
        var b = context.BoolConst("b");
        var c = context.BoolConst("c");

        solver.Assert(context.AtMost([a, b, c], 2));
        solver.Assert(a);
        solver.Assert(b);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(a), Is.True);
            Assert.That(model.GetBoolValue(b), Is.True);
            Assert.That(model.GetBoolValue(c), Is.False); // c must be false since a and b are true
        });
    }

    [Test]
    public void AtMost_ThreeBooleans_MaxTwo_ForbidsThreeTrue()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var a = context.BoolConst("a");
        var b = context.BoolConst("b");
        var c = context.BoolConst("c");

        solver.Assert(context.AtMost([a, b, c], 2));
        solver.Assert(a);
        solver.Assert(b);
        solver.Assert(c); // This makes it unsatisfiable

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void AtMost_IntOverload_WorksCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var bools = new[] { context.BoolConst("a"), context.BoolConst("b"), context.BoolConst("c") };

        solver.Assert(context.AtMost(bools, 1)); // Using int overload
        solver.Assert(bools[0]);

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        Assert.Multiple(() =>
        {
            Assert.That(model.GetBoolValue(bools[0]), Is.True);
            Assert.That(model.GetBoolValue(bools[1]), Is.False);
            Assert.That(model.GetBoolValue(bools[2]), Is.False);
        });
    }

    [Test]
    public void AtLeast_FiveBooleans_MinThree_RequiresAtLeastThree()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var workers = new[]
        {
            context.BoolConst("worker1"),
            context.BoolConst("worker2"),
            context.BoolConst("worker3"),
            context.BoolConst("worker4"),
            context.BoolConst("worker5"),
        };

        solver.Assert(context.AtLeast(workers, 3));

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var trueCount = workers.Count(w => model.GetBoolValue(w));
        Assert.That(trueCount, Is.GreaterThanOrEqualTo(3));
    }

    [Test]
    public void AtLeast_FiveBooleans_MinFour_WithOnlyThreeTrue_Unsatisfiable()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var workers = new[]
        {
            context.BoolConst("w1"),
            context.BoolConst("w2"),
            context.BoolConst("w3"),
            context.BoolConst("w4"),
            context.BoolConst("w5"),
        };

        solver.Assert(context.AtLeast(workers, 4));
        solver.Assert(!workers[0]);
        solver.Assert(!workers[1]); // Only 3 can be true, but we need at least 4

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void AtLeast_IntOverload_WorksCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var bools = new[] { context.BoolConst("a"), context.BoolConst("b"), context.BoolConst("c") };

        solver.Assert(context.AtLeast(bools, 2)); // Using int overload

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var trueCount = bools.Count(b => model.GetBoolValue(b));
        Assert.That(trueCount, Is.GreaterThanOrEqualTo(2));
    }

    [Test]
    public void Exactly_SevenBooleans_ExactlyThree_EnforcesExactCount()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var days = new[]
        {
            context.BoolConst("mon"),
            context.BoolConst("tue"),
            context.BoolConst("wed"),
            context.BoolConst("thu"),
            context.BoolConst("fri"),
            context.BoolConst("sat"),
            context.BoolConst("sun"),
        };

        solver.Assert(context.Exactly(days, 3));

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var trueCount = days.Count(d => model.GetBoolValue(d));
        Assert.That(trueCount, Is.EqualTo(3));
    }

    [Test]
    public void Exactly_WithFourTrue_Unsatisfiable()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var bools = new[]
        {
            context.BoolConst("a"),
            context.BoolConst("b"),
            context.BoolConst("c"),
            context.BoolConst("d"),
            context.BoolConst("e"),
        };

        solver.Assert(context.Exactly(bools, 3));
        solver.Assert(bools[0]);
        solver.Assert(bools[1]);
        solver.Assert(bools[2]);
        solver.Assert(bools[3]); // 4 true, but we need exactly 3

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void Exactly_IntOverload_WorksCorrectly()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var bools = new[] { context.BoolConst("a"), context.BoolConst("b"), context.BoolConst("c") };

        solver.Assert(context.Exactly(bools, 2)); // Using int overload

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var trueCount = bools.Count(b => model.GetBoolValue(b));
        Assert.That(trueCount, Is.EqualTo(2));
    }

    [Test]
    public void PbLe_WeightedBudget_EnforcesBudgetLimit()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var tasks = new[]
        {
            context.BoolConst("task1"),
            context.BoolConst("task2"),
            context.BoolConst("task3"),
            context.BoolConst("task4"),
        };
        var costs = new[] { 10, 20, 30, 40 };
        var budget = 50;

        solver.Assert(context.PbLe(costs, tasks, budget));
        solver.Assert(tasks[3]); // task4 costs 40

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var totalCost = tasks.Select((t, i) => model.GetBoolValue(t) ? costs[i] : 0).Sum();
        Assert.That(totalCost, Is.LessThanOrEqualTo(budget));
    }

    [Test]
    public void PbLe_ExceedsBudget_Unsatisfiable()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var tasks = new[] { context.BoolConst("t1"), context.BoolConst("t2"), context.BoolConst("t3") };
        var costs = new[] { 30, 30, 30 };
        var budget = 50;

        solver.Assert(context.PbLe(costs, tasks, budget));
        solver.Assert(tasks[0]);
        solver.Assert(tasks[1]); // 30 + 30 = 60 > 50

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void PbGe_MinimumRequirement_EnforcesMinimum()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var features = new[]
        {
            context.BoolConst("basic"),
            context.BoolConst("standard"),
            context.BoolConst("premium"),
        };
        var points = new[] { 10, 25, 50 };
        var minPoints = 35;

        solver.Assert(context.PbGe(points, features, minPoints));

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var totalPoints = features.Select((f, i) => model.GetBoolValue(f) ? points[i] : 0).Sum();
        Assert.That(totalPoints, Is.GreaterThanOrEqualTo(minPoints));
    }

    [Test]
    public void PbGe_BelowMinimum_Unsatisfiable()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var items = new[] { context.BoolConst("i1"), context.BoolConst("i2"), context.BoolConst("i3") };
        var values = new[] { 10, 10, 10 };
        var minRequired = 50;

        solver.Assert(context.PbGe(values, items, minRequired));

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Unsatisfiable)); // Can't reach 50 with max 30
    }

    [Test]
    public void PbEq_ExactBudget_EnforcesExactSum()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var coins = new[]
        {
            context.BoolConst("penny"),
            context.BoolConst("nickel"),
            context.BoolConst("dime"),
            context.BoolConst("quarter"),
        };
        var values = new[] { 1, 5, 10, 25 };
        var exactAmount = 31;

        solver.Assert(context.PbEq(values, coins, exactAmount));

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var total = coins.Select((c, i) => model.GetBoolValue(c) ? values[i] : 0).Sum();
        Assert.That(total, Is.EqualTo(exactAmount));

        // One solution: 1 penny + 1 nickel + 1 quarter = 31
        // Or: 1 penny + 3 dimes = 31
    }

    [Test]
    public void PbEq_ImpossibleSum_Unsatisfiable()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var items = new[] { context.BoolConst("a"), context.BoolConst("b"), context.BoolConst("c") };
        var weights = new[] { 10, 20, 30 };
        var target = 15; // Can't make 15 from {10, 20, 30}

        solver.Assert(context.PbEq(weights, items, target));

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Unsatisfiable));
    }

    [Test]
    public void PseudoBoolean_RealWorldScheduling_EmployeeShifts()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        // 5 employees, 7 days
        var employees = Enumerable.Range(0, 5).Select(i => context.BoolConst($"emp{i}")).ToArray();

        // Each employee works exactly 3 days per week
        foreach (var emp in employees)
        {
            var shifts = Enumerable.Range(0, 7).Select(d => context.BoolConst($"{emp}_day{d}")).ToArray();
            solver.Assert(context.Exactly(shifts, 3));
        }

        // Each day needs at least 2 employees
        for (int day = 0; day < 7; day++)
        {
            var dayShifts = employees.Select((emp, i) => context.BoolConst($"{emp}_day{day}")).ToArray();
            solver.Assert(context.AtLeast(dayShifts, 2));
        }

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        // Verify constraints
        for (int empIdx = 0; empIdx < 5; empIdx++)
        {
            var emp = employees[empIdx];
            var shiftCount = Enumerable.Range(0, 7).Count(d => model.GetBoolValue(context.BoolConst($"{emp}_day{d}")));
            Assert.That(shiftCount, Is.EqualTo(3), $"Employee {empIdx} should work exactly 3 days");
        }
    }

    [Test]
    public void PseudoBoolean_RealWorldResourceAllocation_TasksWithCosts()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var solver = context.CreateSolver();

        var taskNames = new[] { "Design", "Implement", "Test", "Deploy", "Document" };
        var tasks = taskNames.Select(name => context.BoolConst(name)).ToArray();
        var costs = new[] { 100, 200, 150, 50, 75 }; // Resource costs
        var priorities = new[] { 10, 20, 15, 25, 5 }; // Priority scores

        var totalBudget = 400;
        var minPriority = 50;

        // Budget constraint
        solver.Assert(context.PbLe(costs, tasks, totalBudget));

        // Minimum priority requirement
        solver.Assert(context.PbGe(priorities, tasks, minPriority));

        // Must select at least 3 tasks
        solver.Assert(context.AtLeast(tasks, 3));

        var status = solver.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = solver.GetModel();
        var selectedTasks = tasks
            .Select(
                (t, i) =>
                    new
                    {
                        Name = taskNames[i],
                        Selected = model.GetBoolValue(t),
                        Cost = costs[i],
                        Priority = priorities[i],
                    }
            )
            .Where(t => t.Selected)
            .ToList();

        var totalCost = selectedTasks.Sum(t => t.Cost);
        var totalPriority = selectedTasks.Sum(t => t.Priority);

        Assert.Multiple(() =>
        {
            Assert.That(totalCost, Is.LessThanOrEqualTo(totalBudget));
            Assert.That(totalPriority, Is.GreaterThanOrEqualTo(minPriority));
            Assert.That(selectedTasks.Count, Is.GreaterThanOrEqualTo(3));
        });
    }

    [Test]
    public void AtMost_EmptyArray_ThrowsArgumentException()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        Assert.Throws<ArgumentException>(() => context.AtMost([], 1));
    }

    [Test]
    public void AtMost_NegativeK_ThrowsArgumentOutOfRangeException()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var bools = new[] { context.BoolConst("a") };
        Assert.Throws<ArgumentOutOfRangeException>(() => context.AtMost(bools, -1));
    }

    [Test]
    public void AtLeast_EmptyArray_ThrowsArgumentException()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        Assert.Throws<ArgumentException>(() => context.AtLeast([], 1));
    }

    [Test]
    public void PbLe_MismatchedLengths_ThrowsArgumentException()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        var bools = new[] { context.BoolConst("a"), context.BoolConst("b") };
        var coeffs = new[] { 1, 2, 3 }; // Different length

        Assert.Throws<ArgumentException>(() => context.PbLe(coeffs, bools, 10));
    }

    [Test]
    public void PbLe_EmptyArray_ThrowsArgumentException()
    {
        using var context = new Z3Context();
        using var scope = context.SetUp();

        Assert.Throws<ArgumentException>(() => context.PbLe([], [], 10));
    }
}
