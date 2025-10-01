# Z3Wrap

Write `x + y == 10` instead of verbose Z3 API calls. Natural C# syntax for Microsoft's Z3 theorem prover with unlimited precision arithmetic.

[![NuGet](https://img.shields.io/nuget/v/Spaceorc.Z3Wrap.svg)](https://www.nuget.org/packages/Spaceorc.Z3Wrap/)
[![Tests](https://img.shields.io/endpoint?url=https://spaceorc.github.io/Z3Wrap/badges/tests.json)](https://github.com/spaceorc/Z3Wrap/actions)
[![Coverage](https://img.shields.io/endpoint?url=https://spaceorc.github.io/Z3Wrap/badges/coverage.json)](https://github.com/spaceorc/Z3Wrap/actions)

```csharp
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
```

## Why Z3Wrap?

### Natural Syntax vs Z3 API
```csharp
// Z3Wrap - readable
solver.Assert(x + y == 10);
solver.Assert(p.Implies(q & r));

// Raw Z3 - verbose
solver.Assert(ctx.MkEq(ctx.MkAdd(x, y), ctx.MkInt(10)));
solver.Assert(ctx.MkImplies(p, ctx.MkAnd(q, r)));
```

### Unlimited Precision
```csharp
using System.Numerics;

// BigInteger - no integer overflow
var huge = BigInteger.Parse("999999999999999999999999999999");
solver.Assert(x == huge);

// Exact rationals - no floating point errors
var r = context.RealConst("r");
solver.Assert(r == new Real(1, 3));  // Exactly 1/3
solver.Assert(r * 3 == 1);           // Perfect arithmetic
```

### Type Safety
```csharp
// Compile-time type checking
var prices = context.ArrayConst<IntExpr, RealExpr>("prices");
solver.Assert(prices[0] == 10.5m);   // Index: Int, Value: Real
solver.Assert(prices[1] > prices[0]); // Type-safe comparisons

// Seamless conversions
solver.Assert(x.ToReal() + r == 5.5m);  // Int → Real
```

### Custom .NET Data Types
```csharp
// Real class - exact rational arithmetic (not decimal/double)
var oneThird = new Real(1, 3);
var twoThirds = new Real(2, 3);
Console.WriteLine(oneThird + twoThirds);  // "1" (exact)

// Bv class - compile-time sized bitvectors with type safety
var bv8 = new Bv<Size8>(0b10101010);
var bv16 = bv8.Resize<Size16>(signed: false);  // Resize to 16 bits
Console.WriteLine(bv8.ToULong());               // 170
Console.WriteLine(bv8.ToBinaryString());        // "10101010"

// Direct arithmetic and bitwise operations
var result = bv8 + new Bv<Size8>(5);   // Type-safe arithmetic
var masked = bv8 & new Bv<Size8>(0xFF); // Bitwise operations
```

## Installation

```bash
dotnet add package Spaceorc.Z3Wrap
brew install z3  # macOS (or apt-get install libz3-4 on Linux)
```

Z3 library is auto-discovered on Windows/macOS/Linux.

## Complete Feature Set

- **Booleans**: `p & q`, `p | q`, `!p`, `p.Implies(q)`
- **Integers**: BigInteger arithmetic with `+`, `-`, `*`, `/`, `%`
- **Reals**: Exact rational arithmetic with `Real(1,3)` fractions
- **BitVectors**: Type-safe compile-time sizes with `Bv<Size32>`, overflow detection
- **Arrays**: Generic `ArrayExpr<TIndex, TValue>` with natural indexing
- **Quantifiers**: First-order logic with `ForAll` and `Exists`
- **Functions**: Uninterpreted functions with type-safe signatures
- **Solver**: Push/pop scopes for constraint backtracking

## Advanced Examples

### BitVector Operations
```csharp
var bv = context.BvConst<Size32>("bv");

solver.Assert((bv & 0xFF) == 0x42);      // Bitwise operations
solver.Assert(bv << 2 == bv * 4);        // Shift equivalence
solver.Assert((bv ^ 0xFFFFFFFF) == ~bv); // XOR/NOT relationship

// Overflow detection
solver.Assert(context.AddNoOverflow(bv, 100U));
```

### Quantifiers (First-Order Logic)
```csharp
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
```

### Uninterpreted Functions
```csharp
// Define function signature: Int → Int
var func = context.Func<IntExpr, IntExpr>("f");

// Apply function to arguments
solver.Assert(func.Apply(5) == 10);
solver.Assert(func.Apply(x) > 0);

// Consistency: same inputs produce same outputs
solver.Assert(func.Apply(5) == func.Apply(5));
```

### Solver Backtracking
```csharp
solver.Push();
solver.Assert(x < 10);
if (solver.Check() == Z3Status.Unsatisfiable) {
    solver.Pop();  // Backtrack
    solver.Assert(x >= 10);
}
```

## Architecture

Reference-counted contexts with automatic memory management. No manual disposal needed for expressions.

```
Z3Expr (abstract)
├── BoolExpr       - Boolean logic
├── IntExpr        - BigInteger arithmetic
├── RealExpr       - Exact rational arithmetic
├── BvExpr<TSize>  - Compile-time sized bitvectors
└── ArrayExpr<TIndex, TValue> - Generic type-safe arrays
```

**Requirements**: .NET 9.0+, Z3 library (auto-discovered)

**License**: MIT
