# Z3Wrap

A modern C# wrapper for Microsoft's Z3 theorem prover with unlimited precision arithmetic.

## Features

- **Unlimited Precision** - BigInteger for integers, exact rational arithmetic for reals
- **Type-Safe** - Strongly typed expressions with operator overloading
- **Natural Syntax** - Write constraints using familiar operators: `x + y == 10`, `p.Implies(q)`
- **Arrays** - Full Z3 array theory support with generic type safety
- **Solver Scopes** - Push/pop operations for backtracking constraint sets
- **Zero Configuration** - Automatically discovers and loads Z3 library
- **Cross-Platform** - Windows, macOS, and Linux support

## Quick Start

### Installation

Install Z3 using your platform's package manager:

```bash
# macOS
brew install z3

# Ubuntu/Debian  
sudo apt-get install libz3-4

# Windows
# Download from https://github.com/Z3Prover/z3/releases
```

Then build the project:

```bash
dotnet build
dotnet test
```

### Basic Usage

```csharp
using var context = new Z3Context(); // Automatically loads Z3 on first use
using var scope = context.SetUp();   // Enable implicit conversions (recommended)

// Create variables
var x = context.IntConst("x");
var y = context.IntConst("y");

// Create solver and add constraints
using var solver = context.CreateSolver();
solver.Assert(x > 0);
solver.Assert(y > 0);
solver.Assert(x + y == 10);  // Clean syntax with implicit conversions
solver.Assert(x * 2 == y - 1);

// Check satisfiability
if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    // Returns BigInteger - unlimited precision!
    Console.WriteLine($"x = {model.GetIntValue(x)}");  // e.g., x = 3
    Console.WriteLine($"y = {model.GetIntValue(y)}");  // e.g., y = 7
}
```

## Examples

### Integer Arithmetic with BigInteger

```csharp
using System.Numerics;

var x = context.IntConst("x");
var huge = BigInteger.Parse("999999999999999999999999999999");
solver.Assert(x == huge); // No overflow!
```

### Exact Rational Arithmetic

```csharp
var a = context.RealConst("a");
solver.Assert(a == new Real(1, 3));      // Exactly 1/3
solver.Assert(a + new Real(1, 6) == new Real(1, 2)); // Exact: 1/3 + 1/6 = 1/2
```

### Type Conversions

```csharp
var x = context.IntConst("x");
var y = context.RealConst("y");

// Convert integer to real for mixed arithmetic
solver.Assert(x.ToReal() + y == context.Real(5.5m));

// Convert real to integer (truncates)
solver.Assert(y.ToInt() % context.Int(2) == context.Int(0));

// Alternative extension method syntax
solver.Assert(context.ToReal(x) >= context.Real(10));
solver.Assert(context.ToInt(y) < context.Int(100));
```

### Boolean Logic

```csharp
var p = context.BoolConst("p");
var q = context.BoolConst("q");

solver.Assert(p & q);           // AND
solver.Assert(p.Implies(q));    // Implication
```

### Scoped Context (Recommended Pattern)

The `SetUp()` method creates a scoped context that enables implicit conversions from C# primitives to Z3 expressions. This dramatically improves code readability by eliminating repetitive explicit conversions:

```csharp
using var context = new Z3Context();
using var scope = context.SetUp();  // Enable implicit conversions

var x = context.IntConst("x");

// Without scope: verbose explicit conversions required
// solver.Assert(context.Eq(context.Add(x, context.Int(5)), context.Int(10)));

// With scope: natural mathematical syntax
solver.Assert(x + 5 == 10);  // int → Z3IntExpr automatically
solver.Assert(x > 0);        // 0 → Z3IntExpr automatically
solver.Assert(x <= 100);     // 100 → Z3IntExpr automatically

// Works with all types
var y = context.RealConst("y");
solver.Assert(y + 3.14m == 10.5m);  // decimal → Z3RealExpr
```

### Arrays

Z3Wrap provides full support for Z3's array theory with type-safe generics:

```csharp
using var context = new Z3Context();
using var scope = context.SetUp();
using var solver = context.CreateSolver();

// Create arrays with explicit types
var prices = context.ArrayConst<Z3IntExpr, Z3RealExpr>("prices");
var flags = context.ArrayConst<Z3IntExpr, Z3BoolExpr>("flags");

// Array access with indexer syntax
solver.Assert(prices[0] == 10.5m);
solver.Assert(prices[1] > prices[0]);
solver.Assert(flags[5] == true);

// Store operation (creates new array)
var updatedPrices = prices.Store(context.Int(2), context.Real(15.99m));
solver.Assert(updatedPrices[2] == 15.99m);

// Constant arrays (all elements same value)
var defaultPrices = context.Array<Z3IntExpr, Z3RealExpr>(context.Real(0));
solver.Assert(defaultPrices[100] == 0);  // Every index has value 0
```

### Solver Scopes (Push/Pop)

Use solver scopes for backtracking and exploring multiple constraint sets:

```csharp
using var context = new Z3Context();
using var scope = context.SetUp();
using var solver = context.CreateSolver();

var x = context.IntConst("x");
solver.Assert(x > 0);

solver.Push();  // Save current constraint set
{
    solver.Assert(x < 10);
    Console.WriteLine($"x < 10: {solver.Check()}");  // Check constraints

    solver.Push();  // Nested scope
    {
        solver.Assert(x == 5);
        Console.WriteLine($"x == 5: {solver.Check()}");
    }
    solver.Pop();   // Back to x > 0 && x < 10
}
solver.Pop();   // Back to just x > 0

// Now add different constraints
solver.Assert(x > 100);
Console.WriteLine($"x > 100: {solver.Check()}");
```

## Architecture

### Expression Hierarchy

```
Z3Expr (abstract base)
├── Z3BoolExpr - Boolean expressions with logical operators
├── Z3IntExpr  - BigInteger expressions with unlimited precision arithmetic
├── Z3RealExpr - Exact rational expressions with Real class
└── Z3ArrayExpr<TIndex, TValue> - Generic arrays with type-safe indexing
```

### Key Features

- **Integers**: `BigInteger` provides unlimited precision - no overflow
- **Reals**: `Real` class uses exact fractions - no floating-point errors  
- **Memory**: Hierarchical disposal - context manages all object lifetimes

```csharp
using var context = new Z3Context();  // Will dispose all children
using var solver = context.CreateSolver();
// Expressions don't need disposal - managed by context
```

## API Reference

### Core Classes

- **`Z3Context`** - Main entry point for creating expressions and solvers
- **`Z3Context.SetUpScope`** - Scoped context for implicit conversions (`using var scope = context.SetUp()`)
- **`Z3Solver`** - Constraint solver with push/pop stack operations
- **`Z3Model`** - Model extraction with unlimited precision value methods
- **`Z3Expr`** - Base class for all expressions
- **`Z3ArrayExpr<TIndex, TValue>`** - Generic arrays with type-safe indexing
- **`Real`** - Exact rational arithmetic class

### Operators

```csharp
// Arithmetic: +, -, *, /, % (BigInteger), unary -
var expr = x + y * BigInteger.Parse("123456789");

// Comparison: ==, !=, <, <=, >, >= (BigInteger compatible)
var constraint = x >= BigInteger.Parse("999999999999");

// Boolean: &, |, ^, !
var formula = p & (q | !r);

// Arrays: indexer access and store operations
var element = array[index];                    // Select operation
var newArray = array.Store(index, value);     // Store operation

// Min/Max operations
var minimum = context.Min(x, y);
var maximum = context.Max(a, b);

// Type Conversions: ToReal(), ToInt()
var mixed = x.ToReal() + y;  // Convert int to real for mixed arithmetic
var truncated = z.ToInt();   // Convert real to int (truncates)
```

## Requirements

- **.NET 9.0** or later
- **Z3 Library** - Automatically discovered in standard locations or download from [Z3 releases](https://github.com/Z3Prover/z3/releases)

## License

MIT License