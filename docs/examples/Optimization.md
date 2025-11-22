# Optimization with Z3: Beyond Satisfiability

Z3's optimizer doesn't just find *any* solution - it finds the **best** solution according to your objectives. Think of it as "constraint solving meets optimization".

## Use Cases

### 1. **Resource Allocation - Minimize Cost**

**Problem**: Assign tasks to workers minimizing total cost

```csharp
using var context = new Z3Context();
using var scope = context.SetUp();
using var optimizer = context.CreateOptimizer();

// 3 tasks, 3 workers, different costs
var task1 = context.IntConst("task1");  // Which worker gets task 1?
var task2 = context.IntConst("task2");
var task3 = context.IntConst("task3");

// Constraints: workers are 0, 1, or 2
optimizer.Assert(task1 >= 0 & task1 <= 2);
optimizer.Assert(task2 >= 0 & task2 <= 2);
optimizer.Assert(task3 >= 0 & task3 <= 2);

// Each worker can only do one task
optimizer.Assert(task1 != task2);
optimizer.Assert(task2 != task3);
optimizer.Assert(task1 != task3);

// Cost matrix: cost[task][worker]
var cost = task1 * 10 + task2 * 20 + task3 * 15;  // Simplified

// Minimize total cost
optimizer.Minimize(cost);

if (optimizer.Check() == Z3Status.Satisfiable)
{
    var model = optimizer.GetModel();
    Console.WriteLine($"Task 1 -> Worker {model.GetIntValue(task1)}");
    Console.WriteLine($"Task 2 -> Worker {model.GetIntValue(task2)}");
    Console.WriteLine($"Task 3 -> Worker {model.GetIntValue(task3)}");
    Console.WriteLine($"Total cost: {model.GetIntValue(cost)}");
}
```

**What Z3 does**: Finds optimal assignment minimizing total cost.

---

### 2. **Scheduling - Maximize Throughput**

**Problem**: Schedule jobs to maximize number of completed tasks

```csharp
var job1_start = context.IntConst("job1_start");
var job2_start = context.IntConst("job2_start");
var job3_start = context.IntConst("job3_start");

// Job durations
var duration1 = 5;
var duration2 = 3;
var duration3 = 4;

// Jobs can't overlap (on single machine)
optimizer.Assert(
    job1_start + duration1 <= job2_start |
    job2_start + duration2 <= job1_start
);

optimizer.Assert(
    job2_start + duration2 <= job3_start |
    job3_start + duration3 <= job2_start
);

optimizer.Assert(
    job1_start + duration1 <= job3_start |
    job3_start + duration3 <= job1_start
);

// All start times non-negative
optimizer.Assert(job1_start >= 0);
optimizer.Assert(job2_start >= 0);
optimizer.Assert(job3_start >= 0);

// Minimize makespan (latest job completion)
var makespan = context.IntConst("makespan");
optimizer.Assert(makespan >= job1_start + duration1);
optimizer.Assert(makespan >= job2_start + duration2);
optimizer.Assert(makespan >= job3_start + duration3);

optimizer.Minimize(makespan);

if (optimizer.Check() == Z3Status.Satisfiable)
{
    var model = optimizer.GetModel();
    Console.WriteLine($"Job 1: start at {model.GetIntValue(job1_start)}");
    Console.WriteLine($"Job 2: start at {model.GetIntValue(job2_start)}");
    Console.WriteLine($"Job 3: start at {model.GetIntValue(job3_start)}");
    Console.WriteLine($"Makespan: {model.GetIntValue(makespan)} time units");
}
```

**What Z3 does**: Finds optimal schedule minimizing completion time.

---

### 3. **Portfolio Selection - Maximize Return, Minimize Risk**

**Problem**: Allocate budget across investments

```csharp
var stock_a = context.IntConst("stock_a");  // Shares of stock A
var stock_b = context.IntConst("stock_b");
var stock_c = context.IntConst("stock_c");

var budget = 10000;
var price_a = 50;
var price_b = 30;
var price_c = 70;

// Budget constraint
optimizer.Assert(stock_a * price_a + stock_b * price_b + stock_c * price_c <= budget);

// Non-negative shares
optimizer.Assert(stock_a >= 0);
optimizer.Assert(stock_b >= 0);
optimizer.Assert(stock_c >= 0);

// Expected returns (cents per share)
var return_a = 5;
var return_b = 3;
var return_c = 8;

var totalReturn = stock_a * return_a + stock_b * return_b + stock_c * return_c;

// Maximize total return
optimizer.Maximize(totalReturn);

if (optimizer.Check() == Z3Status.Satisfiable)
{
    var model = optimizer.GetModel();
    Console.WriteLine($"Buy {model.GetIntValue(stock_a)} shares of A");
    Console.WriteLine($"Buy {model.GetIntValue(stock_b)} shares of B");
    Console.WriteLine($"Buy {model.GetIntValue(stock_c)} shares of C");
    Console.WriteLine($"Expected return: ${model.GetIntValue(totalReturn) / 100.0}");
}
```

**What Z3 does**: Finds optimal portfolio within constraints.

---

### 4. **Multi-Objective Optimization - Pareto Frontier**

**Problem**: Minimize both time AND cost (competing objectives)

```csharp
var use_fast_server = context.BoolConst("use_fast_server");
var num_workers = context.IntConst("num_workers");

optimizer.Assert(num_workers >= 1 & num_workers <= 10);

// Time decreases with more workers, but fast server is quicker
var time = context.If(
    use_fast_server,
    100 / num_workers,      // Fast server
    200 / num_workers       // Slow server
);

// Cost increases with workers, fast server costs more
var cost = context.If(
    use_fast_server,
    50 + num_workers * 10,  // Fast server: $50 + $10/worker
    20 + num_workers * 10   // Slow server: $20 + $10/worker
);

// Minimize both (Z3 finds Pareto-optimal solutions)
var timeHandle = optimizer.Minimize(time);
var costHandle = optimizer.Minimize(cost);

if (optimizer.Check() == Z3Status.Satisfiable)
{
    var model = optimizer.GetModel();
    Console.WriteLine($"Use fast server: {model.GetBoolValue(use_fast_server)}");
    Console.WriteLine($"Workers: {model.GetIntValue(num_workers)}");
    Console.WriteLine($"Time: {model.GetIntValue(time)} minutes");
    Console.WriteLine($"Cost: ${model.GetIntValue(cost)}");

    // Can query lower/upper bounds on objectives
    Console.WriteLine($"Time bounds: {optimizer.GetLower(timeHandle)} - {optimizer.GetUpper(timeHandle)}");
}
```

**What Z3 does**: Balances conflicting objectives, explores trade-offs.

---

### 5. **Soft Constraints - Preferences**

**Problem**: Satisfy hard constraints, optimize soft preferences

```csharp
var red = context.IntConst("red");
var green = context.IntConst("green");
var blue = context.IntConst("blue");

// Hard constraints: valid RGB values
optimizer.Assert(red >= 0 & red <= 255);
optimizer.Assert(green >= 0 & green <= 255);
optimizer.Assert(blue >= 0 & blue <= 255);

// Hard: must be visible (not black)
optimizer.Assert(red + green + blue > 0);

// Soft: prefer red-ish colors (weight = 10)
optimizer.AssertSoft(red > 200, 10);

// Soft: avoid too much blue (weight = 5)
optimizer.AssertSoft(blue < 100, 5);

// Soft: prefer bright colors (weight = 3)
optimizer.AssertSoft(red + green + blue > 400, 3);

if (optimizer.Check() == Z3Status.Satisfiable)
{
    var model = optimizer.GetModel();
    var r = model.GetIntValue(red);
    var g = model.GetIntValue(green);
    var b = model.GetIntValue(blue);
    Console.WriteLine($"RGB: ({r}, {g}, {b})");
    Console.WriteLine($"Color: #{r:X2}{g:X2}{b:X2}");
    // Likely: high red, low blue, bright overall
}
```

**What Z3 does**: Maximizes satisfaction of weighted soft constraints.

---

## Key Differences from Regular Solving

| Solver | Optimizer |
|--------|-----------|
| Find *any* solution | Find *best* solution |
| `Check()` → SAT/UNSAT | `Check()` → optimal model |
| Boolean result | Objective values + bounds |
| Fast | Slower (explores space) |

## Optimization Strategies

1. **Minimize/Maximize**: Single objective optimization
2. **Multi-objective**: Balance competing goals (Pareto-optimal)
3. **Soft Constraints**: Maximize satisfaction of preferences
4. **Lexicographic**: Prioritize objectives in order

## Common Patterns

### Minimize Distance
```csharp
var x = context.IntConst("x");
var target = 100;
optimizer.Minimize((x - target).Abs());  // Get closest to 100
```

### Maximize Utilization
```csharp
var used = context.IntConst("used");
var capacity = 1000;
optimizer.Maximize(used);  // Use as much capacity as possible
optimizer.Assert(used <= capacity);
```

### Balance Load
```csharp
var server1_load = context.IntConst("s1");
var server2_load = context.IntConst("s2");
var imbalance = (server1_load - server2_load).Abs();
optimizer.Minimize(imbalance);  // Balance loads evenly
```

---

**The power**: Z3 doesn't just check if a solution exists - it finds the **optimal** solution according to your business logic, preferences, and constraints.
