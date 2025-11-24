# Pseudo-Boolean Constraints

Pseudo-Boolean constraints allow you to express cardinality and weighted constraints over boolean variables. These are extremely useful for scheduling, resource allocation, combinatorial optimization, and constraint satisfaction problems.

## Overview

Z3Wrap provides five pseudo-boolean constraint methods:

- **AtMost(exprs, k)** - At most k expressions can be true
- **AtLeast(exprs, k)** - At least k expressions must be true
- **Exactly(exprs, k)** - Exactly k expressions must be true (convenience method)
- **PbLe(coeffs, exprs, k)** - Weighted sum ≤ k
- **PbGe(coeffs, exprs, k)** - Weighted sum ≥ k
- **PbEq(coeffs, exprs, k)** - Weighted sum = k

## Cardinality Constraints

### Example: Employee Scheduling

You need to schedule employees for a week, ensuring each works exactly 3 days:

```csharp
using var context = new Z3Context();
using var scope = context.SetUp();
using var solver = context.CreateSolver();

// Create boolean variables for each employee's daily shifts
var employees = new[] { "Alice", "Bob", "Carol", "Dave", "Eve" };
var days = new[] { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };

foreach (var employee in employees)
{
    var shifts = days.Select(day => context.BoolConst($"{employee}_{day}")).ToArray();

    // Each employee works exactly 3 days per week
    solver.Assert(context.Exactly(shifts, 3));
}

// Each day needs at least 2 employees
for (int dayIdx = 0; dayIdx < days.Length; dayIdx++)
{
    var dayShifts = employees
        .Select(emp => context.BoolConst($"{emp}_{days[dayIdx]}"))
        .ToArray();
    solver.Assert(context.AtLeast(dayShifts, 2));
}

// Weekends need at least 3 employees
var saturdayShifts = employees.Select(e => context.BoolConst($"{e}_Sat")).ToArray();
var sundayShifts = employees.Select(e => context.BoolConst($"{e}_Sun")).ToArray();
solver.Assert(context.AtLeast(saturdayShifts, 3));
solver.Assert(context.AtLeast(sundayShifts, 3));

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();

    // Print the schedule
    foreach (var employee in employees)
    {
        var workDays = days
            .Where(day => model.GetBoolValue(context.BoolConst($"{employee}_{day}")))
            .ToList();
        Console.WriteLine($"{employee}: {string.Join(", ", workDays)}");
    }
}
```

Output example:
```
Alice: Mon, Wed, Sat
Bob: Tue, Thu, Sun
Carol: Mon, Fri, Sun
Dave: Wed, Sat, Sun
Eve: Tue, Fri, Sat
```

### Example: Meeting Room Allocation

You have 4 meeting rooms and 10 meetings that need to be scheduled:

```csharp
using var context = new Z3Context();
using var scope = context.SetUp();
using var solver = context.CreateSolver();

var meetings = Enumerable.Range(1, 10).ToArray();
var rooms = Enumerable.Range(1, 4).ToArray();

// Create variables: meeting_i_in_room_j
var assignments = meetings
    .SelectMany(m => rooms.Select(r => new
    {
        Meeting = m,
        Room = r,
        Var = context.BoolConst($"m{m}_r{r}")
    }))
    .ToList();

// Each meeting must be in exactly one room
foreach (var meeting in meetings)
{
    var roomVars = assignments
        .Where(a => a.Meeting == meeting)
        .Select(a => a.Var)
        .ToArray();
    solver.Assert(context.Exactly(roomVars, 1));
}

// Each room can have at most 3 meetings at this time slot
foreach (var room in rooms)
{
    var meetingVars = assignments
        .Where(a => a.Room == room)
        .Select(a => a.Var)
        .ToArray();
    solver.Assert(context.AtMost(meetingVars, 3));
}

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();

    foreach (var room in rooms)
    {
        var meetingsInRoom = assignments
            .Where(a => a.Room == room && model.GetBoolValue(a.Var))
            .Select(a => a.Meeting)
            .ToList();
        Console.WriteLine($"Room {room}: Meetings {string.Join(", ", meetingsInRoom)}");
    }
}
```

## Weighted Constraints

### Example: Project Task Selection with Budget

You need to select tasks for a project, considering costs and priority:

```csharp
using var context = new Z3Context();
using var scope = context.SetUp();
using var solver = context.CreateSolver();

var tasks = new[]
{
    new { Name = "Design UI", Cost = 100, Priority = 10 },
    new { Name = "Implement Backend", Cost = 200, Priority = 20 },
    new { Name = "Write Tests", Cost = 150, Priority = 15 },
    new { Name = "Deploy", Cost = 50, Priority = 25 },
    new { Name = "Documentation", Cost = 75, Priority = 5 },
    new { Name = "Performance Optimization", Cost = 180, Priority = 12 },
};

var taskVars = tasks.Select(t => context.BoolConst(t.Name)).ToArray();
var costs = tasks.Select(t => t.Cost).ToArray();
var priorities = tasks.Select(t => t.Priority).ToArray();

// Budget constraint: total cost <= 400
solver.Assert(context.PbLe(costs, taskVars, 400));

// Minimum priority requirement: total priority >= 50
solver.Assert(context.PbGe(priorities, taskVars, 50));

// Must select at least 3 tasks
solver.Assert(context.AtLeast(taskVars, 3));

// "Deploy" and "Write Tests" are mandatory
solver.Assert(taskVars[3]); // Deploy
solver.Assert(taskVars[2]); // Write Tests

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();

    var selectedTasks = tasks
        .Select((t, i) => new { Task = t, Selected = model.GetBoolValue(taskVars[i]) })
        .Where(x => x.Selected)
        .ToList();

    var totalCost = selectedTasks.Sum(x => x.Task.Cost);
    var totalPriority = selectedTasks.Sum(x => x.Task.Priority);

    Console.WriteLine($"Selected {selectedTasks.Count} tasks:");
    foreach (var item in selectedTasks)
    {
        Console.WriteLine($"  - {item.Task.Name} (Cost: {item.Task.Cost}, Priority: {item.Task.Priority})");
    }
    Console.WriteLine($"\nTotal Cost: {totalCost} / 400");
    Console.WriteLine($"Total Priority: {totalPriority}");
}
```

Output example:
```
Selected 4 tasks:
  - Implement Backend (Cost: 200, Priority: 20)
  - Write Tests (Cost: 150, Priority: 15)
  - Deploy (Cost: 50, Priority: 25)

Total Cost: 400 / 400
Total Priority: 60
```

### Example: Exact Change Problem

Find coins that sum to an exact amount:

```csharp
using var context = new Z3Context();
using var scope = context.SetUp();
using var solver = context.CreateSolver();

// Available coins
var coins = new[]
{
    new { Name = "Penny", Value = 1, Available = 10 },
    new { Name = "Nickel", Value = 5, Available = 5 },
    new { Name = "Dime", Value = 10, Available = 5 },
    new { Name = "Quarter", Value = 25, Available = 4 },
};

// Create variables for how many of each coin to use
var coinVars = coins.Select(c => context.BoolConst(c.Name)).ToArray();
var values = coins.Select(c => c.Value).ToArray();

// Need exactly 31 cents
var targetAmount = 31;
solver.Assert(context.PbEq(values, coinVars, targetAmount));

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();

    var usedCoins = coins
        .Select((c, i) => new { Coin = c, Used = model.GetBoolValue(coinVars[i]) })
        .Where(x => x.Used)
        .ToList();

    Console.WriteLine($"Make {targetAmount} cents with:");
    foreach (var item in usedCoins)
    {
        Console.WriteLine($"  - {item.Coin.Name} ({item.Coin.Value}¢)");
    }

    var total = usedCoins.Sum(x => x.Coin.Value);
    Console.WriteLine($"\nTotal: {total}¢");
}
```

## Combinatorial Optimization

### Example: Feature Selection for Product Tiers

Select features for different product tiers:

```csharp
using var context = new Z3Context();
using var scope = context.SetUp();
using var solver = context.CreateSolver();

var features = new[]
{
    "BasicSupport", "EmailSupport", "PhoneSupport",
    "CloudStorage", "Analytics", "API",
    "CustomBranding", "SSO", "Audit Logs"
};

var featureVars = features.Select(f => context.BoolConst(f)).ToArray();

// Basic tier: exactly 3 features
var basicTier = featureVars.Take(3).ToArray();
solver.Assert(context.Exactly(basicTier, 2));

// Professional tier: at least 5 features
var proTier = featureVars.Take(6).ToArray();
solver.Assert(context.AtLeast(proTier, 5));

// Enterprise tier: at least 7 features
solver.Assert(context.AtLeast(featureVars, 7));

// SSO requires API
var ssoIdx = Array.IndexOf(features, "SSO");
var apiIdx = Array.IndexOf(features, "API");
solver.Assert(featureVars[ssoIdx].Implies(featureVars[apiIdx]));

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();

    var enabledFeatures = features
        .Select((f, i) => new { Feature = f, Enabled = model.GetBoolValue(featureVars[i]) })
        .Where(x => x.Enabled)
        .Select(x => x.Feature)
        .ToList();

    Console.WriteLine($"Enterprise tier features ({enabledFeatures.Count}):");
    foreach (var feature in enabledFeatures)
    {
        Console.WriteLine($"  - {feature}");
    }
}
```

## Performance Tips

1. **Use AtMost/AtLeast instead of manual encoding**: Z3 has optimized solvers for cardinality constraints
2. **Prefer Exactly() over manual And(AtLeast, AtMost)**: It's clearer and may be optimized by Z3
3. **For large problems**: Consider breaking constraints into smaller groups
4. **Integer coefficients only**: Pseudo-boolean constraints require integer weights

## Common Patterns

### At least M out of N constraint
```csharp
solver.Assert(context.AtLeast(options, minimumRequired));
```

### At most M out of N constraint
```csharp
solver.Assert(context.AtMost(options, maximumAllowed));
```

### Exactly M out of N constraint
```csharp
solver.Assert(context.Exactly(options, exactCount));
```

### Budget constraint (cost ≤ budget)
```csharp
solver.Assert(context.PbLe(costs, selections, budget));
```

### Minimum value constraint (value ≥ minimum)
```csharp
solver.Assert(context.PbGe(values, selections, minimumValue));
```

### Exact sum constraint (sum = target)
```csharp
solver.Assert(context.PbEq(weights, selections, target));
```

## See Also

- [Boolean Logic](BooleanLogic.md) - Basic boolean operations
- [Optimization](Optimization.md) - Finding optimal solutions
- [Quantifiers](Quantifiers.md) - Universal and existential quantification
