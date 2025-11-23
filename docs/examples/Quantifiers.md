# What Can You Do With Quantifiers?

Quantifiers in Z3 let you **express properties that must hold for all values or assert existence of values** without enumerating them. Think of it as "first-order logic for constraints" - perfect for mathematical properties, invariants, and specifications.

## Use Cases

### 1. **Mathematical Properties - Verify Axioms**

**Problem**: Prove addition is commutative

```csharp
using var context = new Z3Context();
using var scope = context.SetUp();
using var solver = context.CreateSolver();

var x = context.IntConst("x");
var y = context.IntConst("y");

// Universal property: ∀x,y. x + y = y + x
var commutativity = context.ForAll(x, y, x + y == y + x);

solver.Assert(commutativity);

if (solver.Check() == Z3Status.Satisfiable)
{
    Console.WriteLine("Addition is commutative!");
}
```

**What Z3 does**: Verifies mathematical properties hold universally.

---

### 2. **Existential Search - Find Witness**

**Problem**: Does there exist a solution to this equation?

```csharp
var x = context.IntConst("x");
var y = context.IntConst("y");
var z = context.IntConst("z");

// Does there exist integers where x² + y² = z²? (Pythagorean triple)
var existsTriple = context.Exists(
    x, y, z,
    x * x + y * y == z * z & x > 0 & y > 0 & z > 0
);

solver.Assert(existsTriple);

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine("Found Pythagorean triple!");
    // Note: Model won't show x,y,z values directly (they're quantified)
    // Need to extract witnesses using specific techniques
}
```

**What Z3 does**: Proves existence without finding specific values.

---

### 3. **Function Properties - Monotonicity**

**Problem**: Verify function is monotonically increasing

```csharp
var func = context.Func<IntExpr, IntExpr>("f");

var x = context.IntConst("x");
var y = context.IntConst("y");

// ∀x,y. x < y ⇒ f(x) < f(y)
var monotonic = context.ForAll(
    x, y,
    (x < y).Implies(func.Apply(x) < func.Apply(y))
);

solver.Assert(monotonic);

// Add a specific constraint
solver.Assert(func.Apply(10) == 20);

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"f(10) = {model.GetIntValue(func.Apply(10))}");
    Console.WriteLine($"f(5) must be < 20");
}
```

**What Z3 does**: Constraints function behavior universally.

---

### 4. **Array Initialization - All Elements**

**Problem**: Assert all array elements satisfy a property

```csharp
var arr = context.ArrayConst<IntExpr, IntExpr>("arr");
var idx = context.IntConst("idx");

// ∀ index. 0 ≤ index < 10 ⇒ arr[index] > 0
var allPositive = context.ForAll(
    idx,
    (idx >= 0 & idx < 10).Implies(arr[idx] > 0)
);

solver.Assert(allPositive);

// Query specific index
solver.Assert(arr[5] < 100);

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"arr[5] = {model.GetIntValue(arr[5])}");
    // Will be in range (0, 100)
}
```

**What Z3 does**: Expresses constraints over infinite/large domains.

---

### 5. **Invariant Verification - Loop Invariants**

**Problem**: Prove loop maintains an invariant

```csharp
// Model loop: for i in 0..n: sum += arr[i]
var arr = context.ArrayConst<IntExpr, IntExpr>("arr");
var n = context.IntConst("n");
var sum = context.IntConst("sum");
var i = context.IntConst("i");

// Invariant: ∀ index. 0 ≤ index < n ⇒ arr[index] ≥ 0
var allNonNegative = context.ForAll(
    i,
    (i >= 0 & i < n).Implies(arr[i] >= 0)
);

solver.Assert(allNonNegative);
solver.Assert(n > 0);

// Therefore: sum of all elements ≥ 0
// (This is simplified; real loop verification is more complex)
var sumResult = context.IntConst("sumResult");
solver.Assert(sumResult >= 0);

if (solver.Check() == Z3Status.Satisfiable)
{
    Console.WriteLine("Invariant maintained!");
}
```

**What Z3 does**: Verifies invariants hold across iterations.

---

### 6. **Bounded Quantifiers - Finite Range**

**Problem**: Property holds for specific range

```csharp
var x = context.IntConst("x");

// ∀x ∈ [1, 10]. x² > x
var property = context.ForAll(
    x,
    (x >= 1 & x <= 10).Implies(x * x > x)
);

solver.Assert(property);

if (solver.Check() == Z3Status.Satisfiable)
{
    Console.WriteLine("Property holds for range [1, 10]");
}

// Can also check for counterexamples
var counterexample = context.ForAll(
    x,
    (x >= 1 & x <= 10).Implies(x * x > x + 10)
);

solver.Push();
solver.Assert(counterexample);

if (solver.Check() == Z3Status.Unsatisfiable)
{
    Console.WriteLine("Found counterexample to second property");
}
solver.Pop();
```

**What Z3 does**: Checks bounded universal properties efficiently.

---

### 7. **Alternating Quantifiers - Game Theory**

**Problem**: Does player 1 have a winning strategy?

```csharp
var x = context.IntConst("x");  // Player 1's move
var y = context.IntConst("y");  // Player 2's move

// ∃x. ∀y. player1_wins(x, y)
// Player 1 can choose x such that for ANY y, player 1 wins
var hasWinningStrategy = context.Exists(
    x,
    context.ForAll(
        y,
        // Win condition: x + y is even and x > y
        (x + y) % 2 == 0 & x > y
    )
);

solver.Assert(hasWinningStrategy);
solver.Assert(x >= 0 & x <= 10);
solver.Assert(y >= 0 & y <= 10);

if (solver.Check() == Z3Status.Satisfiable)
{
    Console.WriteLine("Player 1 has a winning strategy!");
}
else
{
    Console.WriteLine("No winning strategy exists");
}
```

**What Z3 does**: Handles nested quantifiers for strategic reasoning.

---

### 8. **Equivalence Checking - Function Implementations**

**Problem**: Are two implementations equivalent?

```csharp
var f = context.Func<IntExpr, IntExpr>("f");
var g = context.Func<IntExpr, IntExpr>("g");

var x = context.IntConst("x");

// Define f: f(x) = 2*x + 1
solver.Assert(context.ForAll(x, f.Apply(x) == 2 * x + 1));

// Define g differently: g(x) = x + x + 1
solver.Assert(context.ForAll(x, g.Apply(x) == x + x + 1));

// Are they equivalent? ∀x. f(x) = g(x)
var equivalent = context.ForAll(x, f.Apply(x) == g.Apply(x));

solver.Assert(!equivalent);  // Try to find counterexample

if (solver.Check() == Z3Status.Unsatisfiable)
{
    Console.WriteLine("Functions are equivalent!");
}
else
{
    Console.WriteLine("Functions differ!");
}
```

**What Z3 does**: Proves functional equivalence.

---

### 9. **Transitivity - Relation Properties**

**Problem**: Verify relation is transitive

```csharp
var lessThan = context.Func<IntExpr, IntExpr, BoolExpr>("lessThan");

var x = context.IntConst("x");
var y = context.IntConst("y");
var z = context.IntConst("z");

// ∀x,y,z. (x < y ∧ y < z) ⇒ x < z
var transitive = context.ForAll(
    x, y, z,
    (lessThan.Apply(x, y) & lessThan.Apply(y, z))
        .Implies(lessThan.Apply(x, z))
);

solver.Assert(transitive);

// Add specific facts
solver.Assert(lessThan.Apply(1, 5));
solver.Assert(lessThan.Apply(5, 10));

// Can we derive lessThan(1, 10)?
solver.Assert(!lessThan.Apply(1, 10));

if (solver.Check() == Z3Status.Unsatisfiable)
{
    Console.WriteLine("Transitivity correctly implies lessThan(1, 10)!");
}
```

**What Z3 does**: Verifies and uses relational properties.

---

### 10. **Uniqueness - Exists Exactly One**

**Problem**: Prove exactly one solution exists

```csharp
var x = context.IntConst("x");
var y = context.IntConst("y");

// ∃!x. x² = 4 ∧ x > 0
// (Exists unique: exactly one x where x² = 4 and x > 0)

// Express as: (∃x. P(x)) ∧ (∀x,y. P(x) ∧ P(y) ⇒ x = y)
var property = x * x == 4 & x > 0;

var existsSolution = context.Exists(x, property);

var unique = context.ForAll(
    x, y,
    (property & (y * y == 4 & y > 0)).Implies(x == y)
);

solver.Assert(existsSolution);
solver.Assert(unique);

if (solver.Check() == Z3Status.Satisfiable)
{
    Console.WriteLine("Exactly one positive square root of 4 exists!");
    // It's 2 (but model won't show quantified variables directly)
}
```

**What Z3 does**: Expresses uniqueness constraints.

---

## When to Use Quantifiers

### ✅ **Good Use Cases**

1. **Mathematical properties**: Commutativity, associativity, distributivity
2. **Function specifications**: Monotonicity, injectivity, surjectivity
3. **Array constraints**: All elements satisfy property
4. **Invariants**: Properties that must always hold
5. **Equivalence checking**: Proving implementations match specs

### ⚠️ **Challenging Use Cases**

1. **Unbounded quantifiers**: May cause solver to return Unknown
2. **Complex nested quantifiers**: Can be very slow
3. **Arithmetic with quantifiers**: Mixing nonlinear arithmetic and quantifiers
4. **Large domains**: Better to use bounded quantifiers when possible

---

## Performance Tips

### 1. **Bound Your Quantifiers**

```csharp
// ❌ Slow: unbounded
var slow = context.ForAll(x, x + 0 == x);

// ✅ Better: bounded domain
var fast = context.ForAll(x, (x >= 0 & x < 100).Implies(x + 0 == x));
```

### 2. **Use Patterns (Triggers)**

Z3 uses patterns to instantiate quantifiers. Simple patterns work better:

```csharp
// ✅ Good: pattern is func.Apply(x)
var good = context.ForAll(x, func.Apply(x) > 0);

// ⚠️ Complex: harder for Z3 to instantiate
var complex = context.ForAll(x, y, func.Apply(x + y) > func.Apply(x - y));
```

### 3. **Prefer Existential Over Universal When Possible**

```csharp
// If you just need to find one solution
var exists = context.Exists(x, x * x == 16);  // Faster

// vs proving it holds for all
var forall = context.ForAll(x, (x * x == 16).Implies(x == 4 | x == -4));
```

### 4. **Minimize Nesting**

```csharp
// ❌ Deeply nested: very slow
var nested = context.ForAll(x,
    context.ForAll(y,
        context.Exists(z, x + y == z)));

// ✅ Flatten when possible
var flat = context.ForAll(x, y, x + y == x + y);  // Trivial, but demonstrates
```

---

## Common Patterns

### Pattern 1: **Implication in ForAll**

Always use implication for conditional properties:

```csharp
// ✅ Correct: property only needs to hold when condition is true
var correct = context.ForAll(x, (x > 0).Implies(x * x > 0));

// ❌ Wrong: forces condition to be true for all x
var wrong = context.ForAll(x, x > 0 & x * x > 0);
```

### Pattern 2: **De Morgan for Negation**

```csharp
// ∃x. P(x) is equivalent to ¬(∀x. ¬P(x))
var exists = context.Exists(x, x > 0);
var equivalent = !context.ForAll(x, !(x > 0));
```

### Pattern 3: **Skolemization for Exists**

If you need the witness value, use a constant instead:

```csharp
// Instead of quantifier
var existsX = context.Exists(x, x * x == 16);

// Use a constant and let solver find it
var witness = context.IntConst("witness");
solver.Assert(witness * witness == 16);
// Now you can get model.GetIntValue(witness)
```

---

## Quantifiers vs Alternatives

| Use Quantifier | Use Alternative |
|----------------|-----------------|
| Property must hold for all values | Can enumerate all cases |
| Proving mathematical properties | Testing specific inputs |
| Function specifications | Concrete implementations |
| Infinite domains | Finite, small domains |

**Example - Finite domain:**

```csharp
// ❌ Overkill for small domain
var withQuantifier = context.ForAll(x, (x >= 0 & x < 3).Implies(x * 2 >= 0));

// ✅ Better: enumerate
solver.Assert(0 * 2 >= 0);
solver.Assert(1 * 2 >= 0);
solver.Assert(2 * 2 >= 0);
```

---

## Debugging Quantifiers

### If solver returns Unknown:

1. **Simplify quantifiers**: Remove nesting, reduce scope
2. **Add bounds**: Restrict domains to finite ranges
3. **Check patterns**: Ensure Z3 can instantiate properly
4. **Try different solvers**: Some tactics handle quantifiers better

### If solver is slow:

1. **Limit quantifier scope**: Use bounded quantifiers
2. **Reduce alternations**: Minimize ∃∀∃ patterns
3. **Use explicit instantiation**: Sometimes manual instantiation is faster
4. **Check for redundancy**: Remove unnecessary quantifiers

---

**The power**: Quantifiers let you **express universal properties and existence claims** without enumerating all possibilities - essential for verification, specification, and mathematical reasoning.
