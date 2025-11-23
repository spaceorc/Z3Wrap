# Logic and Arithmetic: Real-World Problem Solving

Boolean logic and basic arithmetic might seem simple, but they solve real problems. This guide shows **practical applications** of Z3's fundamental features.

## Boolean Logic Use Cases

### 1. **Meeting Scheduler - Find Available Time**

**Problem**: When can all participants meet?

```csharp
using var context = new Z3Context();
using var scope = context.SetUp();
using var solver = context.CreateSolver();

// Time slots
var monday9am = context.BoolConst("mon_9am");
var monday2pm = context.BoolConst("mon_2pm");
var tuesday10am = context.BoolConst("tue_10am");
var tuesday3pm = context.BoolConst("tue_3pm");

// Alice's availability
var aliceAvailable = monday9am | tuesday10am;

// Bob's availability
var bobAvailable = monday2pm | tuesday10am | tuesday3pm;

// Carol's availability
var carolAvailable = monday9am | monday2pm | tuesday10am;

// Find slot where ALL are available
solver.Assert(aliceAvailable);
solver.Assert(bobAvailable);
solver.Assert(carolAvailable);

// Must pick exactly one slot
solver.Assert(
    (monday9am & !monday2pm & !tuesday10am & !tuesday3pm) |
    (!monday9am & monday2pm & !tuesday10am & !tuesday3pm) |
    (!monday9am & !monday2pm & tuesday10am & !tuesday3pm) |
    (!monday9am & !monday2pm & !tuesday10am & tuesday3pm)
);

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    if (model.GetBoolValue(monday9am)) Console.WriteLine("Schedule: Monday 9am");
    if (model.GetBoolValue(monday2pm)) Console.WriteLine("Schedule: Monday 2pm");
    if (model.GetBoolValue(tuesday10am)) Console.WriteLine("Schedule: Tuesday 10am");
    if (model.GetBoolValue(tuesday3pm)) Console.WriteLine("Schedule: Tuesday 3pm");
    // Output: Tuesday 10am (only slot all three can make)
}
```

**What Z3 does**: Finds time that satisfies all scheduling constraints.

---

### 2. **Feature Flag Configuration - Valid Combinations**

**Problem**: Some features conflict - find valid configuration.

```csharp
var useCache = context.BoolConst("use_cache");
var useCDN = context.BoolConst("use_cdn");
var debugMode = context.BoolConst("debug_mode");
var enableMetrics = context.BoolConst("enable_metrics");

// Business rules:
// 1. If debug mode, must enable metrics
solver.Assert(debugMode.Implies(enableMetrics));

// 2. CDN and cache can't both be enabled (conflict)
solver.Assert(!(useCDN & useCache));

// 3. Either CDN or cache must be enabled (performance)
solver.Assert(useCDN | useCache);

// 4. Metrics requires cache (for local metrics)
solver.Assert(enableMetrics.Implies(useCache));

// Can we enable debug mode?
solver.Assert(debugMode);

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"Valid config:");
    Console.WriteLine($"  Cache: {model.GetBoolValue(useCache)}");
    Console.WriteLine($"  CDN: {model.GetBoolValue(useCDN)}");
    Console.WriteLine($"  Debug: {model.GetBoolValue(debugMode)}");
    Console.WriteLine($"  Metrics: {model.GetBoolValue(enableMetrics)}");
    // Output: Cache=true, CDN=false, Debug=true, Metrics=true
}
```

**What Z3 does**: Finds configuration that satisfies all business rules.

---

### 3. **Circuit Design - Boolean Gates**

**Problem**: Design circuit for specific truth table.

```csharp
var a = context.BoolConst("a");
var b = context.BoolConst("b");
var c = context.BoolConst("c");

// Design: output should be true when at least 2 of {a,b,c} are true
var atLeastTwo = (a & b) | (b & c) | (a & c);

// Test case 1: a=true, b=true, c=false → true
solver.Push();
solver.Assert(a & b & !c);
solver.Assert(atLeastTwo);
if (solver.Check() == Z3Status.Satisfiable)
{
    Console.WriteLine("✓ Test 1 passed");
}
solver.Pop();

// Test case 2: a=true, b=false, c=false → false
solver.Push();
solver.Assert(a & !b & !c);
solver.Assert(!atLeastTwo);
if (solver.Check() == Z3Status.Satisfiable)
{
    Console.WriteLine("✓ Test 2 passed");
}
solver.Pop();

// Test case 3: a=true, b=true, c=true → true
solver.Push();
solver.Assert(a & b & c);
solver.Assert(atLeastTwo);
if (solver.Check() == Z3Status.Satisfiable)
{
    Console.WriteLine("✓ Test 3 passed");
}
solver.Pop();
```

**What Z3 does**: Verifies circuit implements desired truth table.

---

### 4. **Puzzle Solving - Logic Grid Puzzles**

**Problem**: Solve "Who owns the zebra?" type puzzles.

```csharp
// Three houses: Red, Blue, Green
var redLeft = context.BoolConst("red_left");
var blueMiddle = context.BoolConst("blue_middle");
var greenRight = context.BoolConst("green_right");

// Each house has exactly one position
solver.Assert(redLeft | blueMiddle | greenRight);  // At least one
// Exactly one (mutual exclusion)
solver.Assert(!(redLeft & blueMiddle));
solver.Assert(!(blueMiddle & greenRight));
solver.Assert(!(redLeft & greenRight));

// Clue 1: Red house is not in the middle
solver.Assert(!blueMiddle | !redLeft);  // If middle chosen, it's not red

// Clue 2: Green house is to the right of red house
solver.Assert(redLeft.Implies(greenRight));

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine("Solution:");
    if (model.GetBoolValue(redLeft)) Console.WriteLine("  Left: Red");
    if (model.GetBoolValue(blueMiddle)) Console.WriteLine("  Middle: Blue");
    if (model.GetBoolValue(greenRight)) Console.WriteLine("  Right: Green");
}
```

**What Z3 does**: Solves constraint-based logic puzzles.

---

## Integer Arithmetic Use Cases

### 5. **Budget Allocation - Optimize Spending**

**Problem**: Allocate budget across departments with constraints.

```csharp
var engineering = context.IntConst("engineering");
var marketing = context.IntConst("marketing");
var sales = context.IntConst("sales");

var totalBudget = 100000;

// Constraints
solver.Assert(engineering + marketing + sales == totalBudget);
solver.Assert(engineering >= 0);
solver.Assert(marketing >= 0);
solver.Assert(sales >= 0);

// Business rules:
// 1. Engineering gets at least 40%
solver.Assert(engineering >= totalBudget * 4 / 10);

// 2. Marketing and sales combined get at least 30%
solver.Assert(marketing + sales >= totalBudget * 3 / 10);

// 3. No department gets more than 60%
solver.Assert(engineering <= totalBudget * 6 / 10);
solver.Assert(marketing <= totalBudget * 6 / 10);
solver.Assert(sales <= totalBudget * 6 / 10);

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"Engineering: ${model.GetIntValue(engineering)}");
    Console.WriteLine($"Marketing: ${model.GetIntValue(marketing)}");
    Console.WriteLine($"Sales: ${model.GetIntValue(sales)}");
}
```

**What Z3 does**: Finds budget allocation satisfying business rules.

---

### 6. **Work Scheduling - Minimize Overtime**

**Problem**: Assign workers to shifts without overtime.

```csharp
var worker1Hours = context.IntConst("worker1");
var worker2Hours = context.IntConst("worker2");
var worker3Hours = context.IntConst("worker3");

var totalWorkNeeded = 120;  // hours per week

// Distribute work
solver.Assert(worker1Hours + worker2Hours + worker3Hours == totalWorkNeeded);

// No overtime (max 40 hours/week)
solver.Assert(worker1Hours <= 40);
solver.Assert(worker2Hours <= 40);
solver.Assert(worker3Hours <= 40);

// Everyone works at least 20 hours (fairness)
solver.Assert(worker1Hours >= 20);
solver.Assert(worker2Hours >= 20);
solver.Assert(worker3Hours >= 20);

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"Worker 1: {model.GetIntValue(worker1Hours)} hours");
    Console.WriteLine($"Worker 2: {model.GetIntValue(worker2Hours)} hours");
    Console.WriteLine($"Worker 3: {model.GetIntValue(worker3Hours)} hours");
    // Output: Each gets 40 hours
}
```

**What Z3 does**: Balances workload across constraints.

---

### 7. **Inventory Management - Reorder Points**

**Problem**: When should we reorder stock?

```csharp
var currentStock = context.IntConst("current_stock");
var dailyUsage = context.IntConst("daily_usage");
var leadTimeDays = context.IntConst("lead_time");
var reorderPoint = context.IntConst("reorder_point");

// Known values
solver.Assert(dailyUsage == 50);   // Use 50 units per day
solver.Assert(leadTimeDays == 7);  // Takes 7 days to receive order

// Calculate reorder point: must have enough for lead time + safety stock (2 days)
solver.Assert(reorderPoint == dailyUsage * (leadTimeDays + 2));

// Current stock
solver.Assert(currentStock == 400);

// Should we reorder?
var shouldReorder = currentStock <= reorderPoint;

solver.Assert(shouldReorder);

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"Reorder point: {model.GetIntValue(reorderPoint)} units");
    Console.WriteLine($"Current stock: {model.GetIntValue(currentStock)} units");
    Console.WriteLine("Action: REORDER NOW");
    // Output: Reorder point is 450 (50 * 9), current is 400, so reorder!
}
```

**What Z3 does**: Automates inventory decision logic.

---

### 8. **Ticket Pricing - Break-Even Analysis**

**Problem**: What ticket price covers costs?

```csharp
var ticketPrice = context.IntConst("ticket_price");
var attendees = context.IntConst("attendees");
var fixedCosts = context.IntConst("fixed_costs");
var variableCostPerPerson = context.IntConst("variable_cost");

// Known values
solver.Assert(fixedCosts == 10000);       // Venue rental, etc.
solver.Assert(variableCostPerPerson == 5); // Food per person
solver.Assert(attendees == 200);           // Expected attendance

// Revenue
var revenue = ticketPrice * attendees;

// Total costs
var totalCosts = fixedCosts + (variableCostPerPerson * attendees);

// Must break even or profit
solver.Assert(revenue >= totalCosts);

// Ticket price constraints
solver.Assert(ticketPrice >= 20);  // Minimum to seem valuable
solver.Assert(ticketPrice <= 100); // Maximum market will bear

// Find minimum viable price
using var optimizer = context.CreateOptimizer();
optimizer.Assert(revenue >= totalCosts);
optimizer.Assert(ticketPrice >= 20);
optimizer.Assert(ticketPrice <= 100);
optimizer.Assert(attendees == 200);
optimizer.Assert(fixedCosts == 10000);
optimizer.Assert(variableCostPerPerson == 5);

optimizer.Minimize(ticketPrice);

if (optimizer.Check() == Z3Status.Satisfiable)
{
    var model = optimizer.GetModel();
    Console.WriteLine($"Minimum ticket price: ${model.GetIntValue(ticketPrice)}");
    // Output: $55 (to cover $11,000 total costs with 200 attendees)
}
```

**What Z3 does**: Calculates pricing to meet financial goals.

---

### 9. **Game Score - Achievement Unlocking**

**Problem**: Can player unlock achievement?

```csharp
var kills = context.IntConst("kills");
var deaths = context.IntConst("deaths");
var assists = context.IntConst("assists");
var score = context.IntConst("score");

// Score formula: kills * 100 + assists * 50 - deaths * 25
solver.Assert(score == kills * 100 + assists * 50 - deaths * 25);

// Achievement requirements:
// - Score >= 1000
// - K/D ratio >= 2 (kills >= deaths * 2)
// - At least 5 kills

solver.Assert(score >= 1000);
solver.Assert(kills >= deaths * 2);
solver.Assert(kills >= 5);

// Reasonable bounds
solver.Assert(kills >= 0 & kills <= 50);
solver.Assert(deaths >= 0 & deaths <= 50);
solver.Assert(assists >= 0 & assists <= 100);

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine("Achievement unlocked!");
    Console.WriteLine($"  Kills: {model.GetIntValue(kills)}");
    Console.WriteLine($"  Deaths: {model.GetIntValue(deaths)}");
    Console.WriteLine($"  Assists: {model.GetIntValue(assists)}");
    Console.WriteLine($"  Score: {model.GetIntValue(score)}");
}
```

**What Z3 does**: Verifies achievement requirements are satisfiable.

---

### 10. **Loan Calculation - Affordability Check**

**Problem**: Can I afford this loan?

```csharp
var loanAmount = context.IntConst("loan_amount");
var monthlyPayment = context.IntConst("monthly_payment");
var interestRatePercent = context.IntConst("interest_rate");
var termMonths = context.IntConst("term_months");
var monthlyIncome = context.IntConst("monthly_income");

// Known values
solver.Assert(loanAmount == 300000);      // House price
solver.Assert(interestRatePercent == 5);  // 5% annual
solver.Assert(termMonths == 360);         // 30 years
solver.Assert(monthlyIncome == 8000);

// Simplified payment calculation (approximate)
// Real formula is complex, this is illustrative
var monthlyRate = interestRatePercent;  // Simplified
solver.Assert(monthlyPayment == (loanAmount * monthlyRate) / 1000 + loanAmount / termMonths);

// Affordability: payment should be < 30% of income
var maxPayment = monthlyIncome * 3 / 10;

solver.Assert(monthlyPayment <= maxPayment);

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine("Loan is affordable!");
    Console.WriteLine($"Monthly payment: ${model.GetIntValue(monthlyPayment)}");
    Console.WriteLine($"Max affordable: ${model.GetIntValue(maxPayment)}");
}
else
{
    Console.WriteLine("Loan is NOT affordable!");
}
```

**What Z3 does**: Validates financial decisions against rules.

---

## Key Takeaways

### Boolean Logic Solves:
- **Scheduling**: Finding time slots, resource allocation
- **Configuration**: Valid feature combinations
- **Logic puzzles**: Sudoku, grid puzzles, constraint satisfaction
- **Circuit design**: Hardware verification

### Integer Arithmetic Solves:
- **Finance**: Budget allocation, pricing, affordability
- **Planning**: Work scheduling, inventory management
- **Gaming**: Score calculation, achievement systems
- **Optimization**: Minimize costs, maximize efficiency

---

**The power**: Even "simple" boolean and integer constraints can **solve complex real-world problems** - scheduling, budgeting, configuration, and decision-making - without writing complex algorithms.
