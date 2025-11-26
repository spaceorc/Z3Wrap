# What Can You Do With Array Constraints?

Array theory in Z3 lets you **reason about mappings from indices to values** symbolically. Think of it as "algebra for lookup tables" - perfect for modeling memory, sparse data structures, and property mappings.

## Use Cases

### 1. **Memory Modeling - Find Buffer Overflow**

**Problem**: Can we write outside array bounds?

```csharp
using var context = new Z3Context();
using var scope = context.SetUp();
using var solver = context.CreateSolver();

// Model memory: address → byte value
var memory = context.ArrayConst<IntExpr, IntExpr>("memory");

var writeAddr = context.IntConst("writeAddr");
var readAddr = context.IntConst("readAddr");

// Write at some address
var updatedMemory = memory.Store(writeAddr, 0x42);

// Read from a different address
var value = updatedMemory[readAddr];

// Check: can we read the written value from outside [0, 100)?
solver.Assert(writeAddr >= 0 & writeAddr < 100);  // Valid write range
solver.Assert(readAddr < 0 | readAddr >= 100);    // Invalid read range
solver.Assert(value == 0x42);                     // But we read the written value!

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"Buffer overflow: write at {model.GetIntValue(writeAddr)}, " +
                     $"read at {model.GetIntValue(readAddr)}");
    // This means writeAddr == readAddr (contradiction!)
}
else
{
    Console.WriteLine("Memory access is safe!");
}
```

**What Z3 does**: Verifies memory safety properties symbolically.

---

### 2. **Permissions Matrix - Access Control**

**Problem**: Does user have permission to access resource?

```csharp
// Model permissions: (user_id, resource_id) → has_permission
var permissions = context.ArrayConst<IntExpr, IntExpr, BoolExpr>("permissions");

var userId = context.IntConst("user");
var resourceId = context.IntConst("resource");

// Set some permissions
var perms1 = permissions.Store(1, 100, true);   // User 1 can access resource 100
var perms2 = perms1.Store(1, 101, false);        // User 1 cannot access resource 101
var perms3 = perms2.Store(2, 100, false);        // User 2 cannot access resource 100

// Query: Can user 1 access resource 101?
var canAccess = perms3[1, 101];

solver.Assert(canAccess);

if (solver.Check() == Z3Status.Satisfiable)
{
    Console.WriteLine("Access granted!");
}
else
{
    Console.WriteLine("Access denied!");
    // Output: Access denied!
}
```

**What Z3 does**: Models and queries multi-dimensional permission tables.

---

### 3. **Sparse Data Structures**

**Problem**: Initialize array with default value, modify specific indices

```csharp
var data = context.ArrayConst<IntExpr, IntExpr>("data");

// Initialize all elements to default value 0
var defaultArray = context.Array<IntExpr, IntExpr>(0);

// Set specific values
var arr1 = defaultArray.Store(5, 100);
var arr2 = arr1.Store(10, 200);
var arr3 = arr2.Store(15, 300);

// Verify: index 5 has value 100, others have 0
solver.Assert(arr3[5] == 100);
solver.Assert(arr3[10] == 200);
solver.Assert(arr3[7] == 0);   // Unmodified index

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine("Sparse array initialized correctly!");
}
```

**What Z3 does**: Efficiently represents sparse arrays with default values.

---

### 4. **Matrix Operations - Find Solution**

**Problem**: Find matrix values satisfying constraints

```csharp
// 2D array: matrix[row][col] → value
var matrix = context.ArrayConst<IntExpr, IntExpr, IntExpr>("matrix");

// Set constraints on specific cells
solver.Assert(matrix[0, 0] + matrix[0, 1] == 10);
solver.Assert(matrix[1, 0] + matrix[1, 1] == 20);
solver.Assert(matrix[0, 0] + matrix[1, 0] == 15);

// All values positive
solver.Assert(matrix[0, 0] > 0);
solver.Assert(matrix[0, 1] > 0);
solver.Assert(matrix[1, 0] > 0);
solver.Assert(matrix[1, 1] > 0);

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"Matrix:");
    Console.WriteLine($"  [{model.GetIntValue(matrix[0, 0])}, {model.GetIntValue(matrix[0, 1])}]");
    Console.WriteLine($"  [{model.GetIntValue(matrix[1, 0])}, {model.GetIntValue(matrix[1, 1])}]");
    // Example output:
    // [7, 3]
    // [8, 12]
}
```

**What Z3 does**: Solves systems of constraints over multi-dimensional arrays.

---

### 5. **Property Maps - Configuration Validation**

**Problem**: Verify configuration consistency

```csharp
// Map service_name → port_number
var servicePorts = context.ArrayConst<StringExpr, IntExpr>("ports");

var webPort = servicePorts[context.String("web")];
var apiPort = servicePorts[context.String("api")];
var dbPort = servicePorts[context.String("db")];

// Constraints
solver.Assert(webPort == 80 | webPort == 443);  // Standard HTTP/HTTPS
solver.Assert(apiPort >= 3000 & apiPort < 4000); // Node.js range
solver.Assert(dbPort == 5432);                   // PostgreSQL default

// All ports must be different
solver.Assert(webPort != apiPort);
solver.Assert(apiPort != dbPort);
solver.Assert(webPort != dbPort);

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"web: {model.GetIntValue(webPort)}");
    Console.WriteLine($"api: {model.GetIntValue(apiPort)}");
    Console.WriteLine($"db: {model.GetIntValue(dbPort)}");
}
```

**What Z3 does**: Validates configuration mappings satisfy business rules.

---

### 6. **Cache Coherence - Find Race Condition**

**Problem**: Can two caches have different values for same key?

```csharp
// Two caches: key → value
var cache1 = context.ArrayConst<IntExpr, IntExpr>("cache1");
var cache2 = context.ArrayConst<IntExpr, IntExpr>("cache2");

var key = context.IntConst("key");

// After some updates
var updated1 = cache1.Store(key, 100);
var updated2 = cache2.Store(key, 200);

// Check: can they be inconsistent?
solver.Assert(updated1[key] != updated2[key]);

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"Race condition: key {model.GetIntValue(key)} has different values!");
    Console.WriteLine($"Cache1: {model.GetIntValue(updated1[key])}");
    Console.WriteLine($"Cache2: {model.GetIntValue(updated2[key])}");
}
```

**What Z3 does**: Detects cache coherence violations.

---

### 7. **Lookup Table Equivalence**

**Problem**: Are two lookup implementations equivalent?

```csharp
var table1 = context.ArrayConst<IntExpr, IntExpr>("table1");
var table2 = context.ArrayConst<IntExpr, IntExpr>("table2");

// Implementation 1: explicit stores
var impl1 = context.Array<IntExpr, IntExpr>(0)
    .Store(1, 10)
    .Store(2, 20)
    .Store(3, 30);

// Implementation 2: formula-based
var idx = context.IntConst("idx");
var impl2Value = context.If(
    idx >= 1 & idx <= 3,
    idx * 10,
    0
);

// Are they equivalent for all indices?
solver.Assert(impl1[idx] != impl2Value);

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"Difference at index {model.GetIntValue(idx)}");
}
else
{
    Console.WriteLine("Implementations are equivalent!");
}
```

**What Z3 does**: Proves or disproves functional equivalence.

---

### 8. **3D Tensor - Volume Data**

**Problem**: Model and query 3D grid data

```csharp
// 3D array: grid[x][y][z] → value
var grid = context.ArrayConst<IntExpr, IntExpr, IntExpr, IntExpr>("grid");

// Set voxel values
var g1 = grid.Store(0, 0, 0, 1);
var g2 = g1.Store(1, 1, 1, 2);
var g3 = g2.Store(2, 2, 2, 3);

// Query: sum of diagonal values
var sum = g3[0, 0, 0] + g3[1, 1, 1] + g3[2, 2, 2];

solver.Assert(sum == 6);  // 1 + 2 + 3

if (solver.Check() == Z3Status.Satisfiable)
{
    Console.WriteLine("3D diagonal sum verified!");
}
```

**What Z3 does**: Handles multi-dimensional array constraints.

---

### 9. **Symbolic Array Indices**

**Problem**: Find which index satisfies a property

```csharp
var arr = context.Array<IntExpr, IntExpr>(0)
    .Store(0, 5)
    .Store(1, 10)
    .Store(2, 15)
    .Store(3, 20);

var idx = context.IntConst("idx");

// Find index where value equals 15
solver.Assert(arr[idx] == 15);
solver.Assert(idx >= 0 & idx < 10);

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"Found at index: {model.GetIntValue(idx)}");
    // Output: 2
}
```

**What Z3 does**: Searches for indices satisfying value constraints.

---

### 10. **Default Array Patterns**

**Problem**: Initialize array with pattern, then override specific values

```csharp
// All elements default to their index * 2
var idx = context.IntConst("idx");
var defaultValue = idx * 2;

// Create default array (conceptually: arr[i] = i * 2)
// Then override specific indices
var arr = context.ArrayConst<IntExpr, IntExpr>("arr")
    .Store(5, 100)   // Override index 5
    .Store(10, 200); // Override index 10

// Query: What's the value at index 7?
var value7 = arr[7];

// For non-overridden indices, we need to establish the default
// This requires quantifiers or explicit modeling
solver.Assert(arr[7] != 14);  // If it's NOT the default (7*2=14)

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"Index 7 was overridden to: {model.GetIntValue(value7)}");
}
else
{
    Console.WriteLine("Index 7 has default value (14)");
}
```

**What Z3 does**: Distinguishes between default and explicitly set values.

---

## Key Concepts

### Array Theory Basics

**Arrays are:**
- ✅ **Immutable**: `Store` returns a new array
- ✅ **Infinite**: All indices have a value (default or stored)
- ✅ **Symbolic**: Index and value can be variables
- ✅ **Functional**: Arrays are mappings, not mutable data structures

**Operations:**
- `ArrayConst<TIndex, TValue>(name)` - Create symbolic array
- `Array<TIndex, TValue>(defaultValue)` - All elements have same value
- `array[index]` - Read (Select)
- `array.Store(index, value)` - Write (returns new array)
- Multi-dimensional: `array[i, j]`, `array.Store(i, j, val)`

### Arrays vs Sequences

| Arrays | Sequences |
|--------|-----------|
| Infinite domain | Finite length |
| Symbolic indices | Concrete length |
| Functional updates | Concatenation, substring |
| Memory modeling | String-like operations |

**Use arrays when:**
- Modeling memory or lookup tables
- Index is symbolic/unknown
- Need sparse representation
- Working with multi-dimensional data

**Use sequences when:**
- Working with strings
- Length matters
- Need sequence operations (concat, substring)
- Functional programming (map, fold)

---

## Performance Tips

1. **Avoid unnecessary stores**: Chain stores efficiently
2. **Use Array for defaults**: More efficient than many stores
3. **Minimize array equalities**: Expensive for solver
4. **Bounded indices**: Add range constraints when possible
5. **Multi-dimensional alternatives**: Sometimes nested structures are clearer

---

**The power**: Arrays let you reason about **mappings symbolically** - finding indices, values, or proving properties about lookup tables without enumerating all possibilities.
