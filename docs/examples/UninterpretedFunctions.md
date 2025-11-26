# What Can You Do With Uninterpreted Functions?

Uninterpreted functions in Z3 let you **reason about operations without defining their implementation**. Think of it as "algebra for black boxes" - perfect for modeling APIs, hash functions, oracles, and abstract operations.

## Use Cases

### 1. **Hash Function Properties - Collision Detection**

**Problem**: Can two different inputs produce the same hash?

```csharp
using var context = new Z3Context();
using var scope = context.SetUp();
using var solver = context.CreateSolver();

// Define hash function: String → Int (without implementing it)
var hash = context.Func<StringExpr, IntExpr>("hash");

var input1 = context.StringConst("input1");
var input2 = context.StringConst("input2");

// Check for collision: different inputs, same hash
solver.Assert(input1 != input2);
solver.Assert(hash.Apply(input1) == hash.Apply(input2));

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"Collision found!");
    Console.WriteLine($"  {model.GetStringValue(input1)} -> {model.GetIntValue(hash.Apply(input1))}");
    Console.WriteLine($"  {model.GetStringValue(input2)} -> {model.GetIntValue(hash.Apply(input2))}");
}
else
{
    Console.WriteLine("Hash is collision-free!");
}
```

**What Z3 does**: Proves properties about functions without knowing their implementation.

---

### 2. **API Consistency - Idempotence**

**Problem**: Verify API operation is idempotent

```csharp
var updateUser = context.Func<IntExpr, IntExpr>("updateUser");

var userId = context.IntConst("userId");

// Idempotence: f(f(x)) = f(x)
var once = updateUser.Apply(userId);
var twice = updateUser.Apply(once);

solver.Assert(once != twice);

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"API is NOT idempotent!");
    Console.WriteLine($"userId: {model.GetIntValue(userId)}");
}
else
{
    Console.WriteLine("API is idempotent!");
}
```

**What Z3 does**: Verifies API contracts without implementation details.

---

### 3. **Caching Correctness - Cache Behavior**

**Problem**: Does cache return same value as uncached call?

```csharp
var compute = context.Func<IntExpr, IntExpr>("compute");  // Expensive operation
var cache = context.ArrayConst<IntExpr, IntExpr>("cache");

var input = context.IntConst("input");
var cacheHit = context.BoolConst("cacheHit");

// If cache hit, use cached value; else compute
var cachedValue = cache[input];
var computedValue = compute.Apply(input);

var result = context.If(cacheHit, cachedValue, computedValue);

// After storing to cache
var updatedCache = cache.Store(input, computedValue);

// Next access must match
var nextResult = updatedCache[input];

solver.Assert(nextResult != computedValue);

if (solver.Check() == Z3Status.Satisfiable)
{
    Console.WriteLine("Cache inconsistency detected!");
}
else
{
    Console.WriteLine("Cache is consistent!");
}
```

**What Z3 does**: Models caching logic without implementing compute function.

---

### 4. **Encryption Properties - One-Way Function**

**Problem**: Can we reverse the encryption?

```csharp
var encrypt = context.Func<IntExpr, IntExpr>("encrypt");

var plaintext1 = context.IntConst("plaintext1");
var plaintext2 = context.IntConst("plaintext2");
var ciphertext = context.IntConst("ciphertext");

// We have ciphertext = encrypt(plaintext1)
solver.Assert(encrypt.Apply(plaintext1) == ciphertext);

// Can plaintext2 decrypt to same ciphertext?
solver.Assert(encrypt.Apply(plaintext2) == ciphertext);

// But plaintexts are different
solver.Assert(plaintext1 != plaintext2);

if (solver.Check() == Z3Status.Satisfiable)
{
    Console.WriteLine("Encryption is not injective - collision possible!");
}
else
{
    Console.WriteLine("Encryption is injective (one-to-one)!");
}
```

**What Z3 does**: Analyzes cryptographic properties symbolically.

---

### 5. **Function Composition - Pipeline Verification**

**Problem**: Verify data pipeline preserves properties

```csharp
var sanitize = context.Func<StringExpr, StringExpr>("sanitize");
var validate = context.Func<StringExpr, BoolExpr>("validate");

var userInput = context.StringConst("input");
var sanitized = sanitize.Apply(userInput);

// Property: sanitized input always validates
var alwaysValid = validate.Apply(sanitized);

solver.Assert(!alwaysValid);

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"Sanitization doesn't guarantee validation!");
    Console.WriteLine($"Input: {model.GetStringValue(userInput)}");
}
else
{
    Console.WriteLine("Sanitization guarantees validation!");
}
```

**What Z3 does**: Verifies pipeline correctness without implementation.

---

### 6. **Monotonicity - Ordering Preservation**

**Problem**: Does function preserve ordering?

```csharp
var process = context.Func<IntExpr, IntExpr>("process");

var x = context.IntConst("x");
var y = context.IntConst("y");

// Assume monotonicity: x < y implies process(x) < process(y)
var monotonic = (x < y).Implies(process.Apply(x) < process.Apply(y));

solver.Assert(monotonic);

// Check specific values
solver.Assert(x == 5);
solver.Assert(y == 10);

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"process(5) < process(10)");
    // Note: Can't get actual values since process is uninterpreted
    // But we know the relationship holds
}
```

**What Z3 does**: Reasons about functions under assumptions.

---

### 7. **Memoization - Same Input, Same Output**

**Problem**: Verify memoization preserves function semantics

```csharp
var compute = context.Func<IntExpr, IntExpr>("compute");
var memo = context.ArrayConst<IntExpr, IntExpr>("memo");

var input = context.IntConst("input");

// First call: compute and store
var result1 = compute.Apply(input);
var updatedMemo = memo.Store(input, result1);

// Second call: read from memo
var result2 = updatedMemo[input];

// Must be equal
solver.Assert(result1 != result2);

if (solver.Check() == Z3Status.Satisfiable)
{
    Console.WriteLine("Memoization broken!");
}
else
{
    Console.WriteLine("Memoization correct!");
}
```

**What Z3 does**: Verifies optimization correctness.

---

### 8. **Determinism - Same Inputs, Same Outputs**

**Problem**: Is function deterministic?

```csharp
var service = context.Func<IntExpr, IntExpr>("service");

var input = context.IntConst("input");
var call1 = service.Apply(input);
var call2 = service.Apply(input);

// Determinism: same input must produce same output
solver.Assert(call1 != call2);

if (solver.Check() == Z3Status.Satisfiable)
{
    Console.WriteLine("Function is non-deterministic!");
}
else
{
    Console.WriteLine("Function is deterministic!");
    // Z3 assumes functions are deterministic by default
}
```

**What Z3 does**: Leverages functional consistency (axiom of functions).

---

### 9. **Access Control - Permission Checking**

**Problem**: Model permission system

```csharp
var hasPermission = context.Func<IntExpr, StringExpr, BoolExpr>("hasPermission");

var userId = context.IntConst("userId");
var resource = context.StringConst("resource");

// Admin (user 1) has access to everything
var isAdmin = userId == 1;
solver.Assert(isAdmin.Implies(hasPermission.Apply(userId, resource)));

// User 2 has access to "public" resources
solver.Assert(
    (userId == 2 & resource == "public")
        .Implies(hasPermission.Apply(userId, resource))
);

// Check: Can user 2 access "secret"?
solver.Assert(hasPermission.Apply(2, context.String("secret")));

if (solver.Check() == Z3Status.Satisfiable)
{
    Console.WriteLine("User 2 can access secret resource!");
}
else
{
    Console.WriteLine("Access denied!");
}
```

**What Z3 does**: Models authorization logic abstractly.

---

### 10. **Commutativity - Order Independence**

**Problem**: Does operation order matter?

```csharp
var merge = context.Func<IntExpr, IntExpr, IntExpr>("merge");

var x = context.IntConst("x");
var y = context.IntConst("y");

// Assume commutativity: merge(x, y) = merge(y, x)
solver.Assert(context.ForAll(x, y, merge.Apply(x, y) == merge.Apply(y, x)));

// Verify specific case
solver.Assert(merge.Apply(5, 10) != merge.Apply(10, 5));

if (solver.Check() == Z3Status.Satisfiable)
{
    Console.WriteLine("Operation is not commutative!");
}
else
{
    Console.WriteLine("Operation is commutative!");
}
```

**What Z3 does**: Verifies algebraic properties.

---

## When to Use Uninterpreted Functions

### ✅ **Good Use Cases**

1. **Abstract operations**: You care about properties, not implementation
2. **Black-box systems**: External APIs, third-party services
3. **Optimization**: Hash functions, encryption, compression
4. **Specification**: Define contracts before implementation
5. **Equivalence**: Compare different implementations

### ❌ **Not Suitable For**

1. **Need concrete values**: If you need actual results, define the function
2. **Performance-critical**: Concrete functions are faster
3. **Simple operations**: Addition, comparison - use built-ins
4. **Testing specific inputs**: Use concrete values instead

---

## Uninterpreted vs Concrete Functions

### Concrete Function (Defined)

```csharp
// Concrete: Z3 knows what this does
var doubler = context.Lambda(x, x * 2);
solver.Assert(doubler.Apply(5) == 10);  // Z3 can evaluate

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"Result: {model.GetIntValue(doubler.Apply(5))}");
    // Output: 10
}
```

### Uninterpreted Function (Abstract)

```csharp
// Uninterpreted: Z3 doesn't know implementation
var process = context.Func<IntExpr, IntExpr>("process");
solver.Assert(process.Apply(5) == process.Apply(5));  // Only knows consistency

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    // Model assigns arbitrary value to process(5)
    Console.WriteLine($"Result: {model.GetIntValue(process.Apply(5))}");
    // Output: Some arbitrary value (e.g., 0, 42, ...)
}
```

---

## Function Axioms and Properties

### Axiom 1: **Consistency**

Z3 automatically enforces: same inputs produce same outputs

```csharp
var f = context.Func<IntExpr, IntExpr>("f");
var x = context.IntConst("x");

// This is ALWAYS true (Z3 axiom)
solver.Assert(f.Apply(x) == f.Apply(x));
```

### Axiom 2: **Extensionality**

If two functions produce same outputs for all inputs, they're equal

```csharp
var f = context.Func<IntExpr, IntExpr>("f");
var g = context.Func<IntExpr, IntExpr>("g");
var x = context.IntConst("x");

// If f(x) = g(x) for all x, then f = g
solver.Assert(context.ForAll(x, f.Apply(x) == g.Apply(x)));
// Implies: f and g are semantically equivalent
```

### Adding Custom Axioms

```csharp
var hash = context.Func<IntExpr, IntExpr>("hash");

// Axiom: Hash of zero is zero
solver.Assert(hash.Apply(0) == 0);

// Axiom: Hash is injective (no collisions)
var x = context.IntConst("x");
var y = context.IntConst("y");
solver.Assert(context.ForAll(x, y,
    (hash.Apply(x) == hash.Apply(y)).Implies(x == y)
));
```

---

## Performance Tips

### 1. **Minimize Function Applications**

```csharp
// ❌ Slow: repeated applications
solver.Assert(f.Apply(x) > 0);
solver.Assert(f.Apply(x) < 100);
solver.Assert(f.Apply(x) != 50);

// ✅ Better: cache application
var fx = f.Apply(x);
solver.Assert(fx > 0);
solver.Assert(fx < 100);
solver.Assert(fx != 50);
```

### 2. **Limit Function Arity**

```csharp
// ❌ High arity: slower
var f = context.Func<IntExpr, IntExpr, IntExpr, IntExpr, IntExpr>("f");

// ✅ Lower arity or nested functions
var g = context.Func<IntExpr, IntExpr, IntExpr>("g");
```

### 3. **Avoid Deep Nesting**

```csharp
// ❌ Deep nesting
var result = f.Apply(g.Apply(h.Apply(x)));

// ✅ Flatten with intermediate variables
var hx = h.Apply(x);
var ghx = g.Apply(hx);
var result = f.Apply(ghx);
```

---

## Common Patterns

### Pattern 1: **Injective Function**

No collisions - different inputs produce different outputs

```csharp
var f = context.Func<IntExpr, IntExpr>("f");
var x = context.IntConst("x");
var y = context.IntConst("y");

solver.Assert(context.ForAll(x, y,
    (f.Apply(x) == f.Apply(y)).Implies(x == y)
));
```

### Pattern 2: **Surjective Function**

Every output has at least one input

```csharp
var f = context.Func<IntExpr, IntExpr>("f");
var x = context.IntConst("x");
var y = context.IntConst("y");

solver.Assert(context.ForAll(y,
    context.Exists(x, f.Apply(x) == y)
));
```

### Pattern 3: **Fixed Points**

Function has value that maps to itself

```csharp
var f = context.Func<IntExpr, IntExpr>("f");
var fixedPoint = context.IntConst("fixedPoint");

solver.Assert(f.Apply(fixedPoint) == fixedPoint);
```

---

## Dynamic Function Builder

For runtime-defined signatures:

```csharp
// Build function with dynamic arity
var builder = context.FuncDeclBuilder("myFunc");
builder.WithArg<IntExpr>();
builder.WithArg<BoolExpr>();
builder.WithRange<IntExpr>();

var func = builder.Create();

// Apply dynamically
var result = func.Apply(context.Int(5), context.Bool(true));
```

---

**The power**: Uninterpreted functions let you **reason about operations abstractly** - verifying properties, consistency, and correctness without implementation details getting in the way.
