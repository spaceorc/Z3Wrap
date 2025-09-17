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
var prices = context.ArrayConst<Z3IntExpr, Z3RealExpr>("prices");
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

// BitVec class - proper .NET bitvector type with operations
var bv8 = new BitVec(0b10101010, 8);
var bv16 = bv8.Resize(16);       // Zero-extend to 16 bits
var extracted = bv8.Extract(7, 4); // Extract bits 7-4
Console.WriteLine(bv8.ToInt());     // 170
Console.WriteLine(bv8.ToBinaryString()); // "10101010"

// Direct arithmetic and bitwise operations
var result = bv8 + new BitVec(5, 8);   // BitVec arithmetic
var masked = bv8 & 0xFF;               // Bitwise operations
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
- **BitVectors**: Binary ops `&`, `|`, `^`, `~`, `<<`, `>>` with overflow checks
- **Arrays**: Generic `Z3ArrayExpr<TIndex, TValue>` with natural indexing
- **Solver**: Push/pop scopes for constraint backtracking

## Advanced Examples

### BitVector Operations
```csharp
var bv = context.BitVecConst("bv", 32);

solver.Assert((bv & 0xFF) == 0x42);      // Bitwise operations
solver.Assert(bv << 2 == bv * 4);        // Shift equivalence
solver.Assert((bv ^ 0xFFFFFFFF) == ~bv); // XOR/NOT relationship

// Overflow detection
solver.Assert(context.BitVecBoundaryCheck().Add(bv, 100).NoOverflow());
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
├── Z3BoolExpr     - Boolean logic
├── Z3IntExpr      - BigInteger arithmetic
├── Z3RealExpr     - Exact rational arithmetic
├── Z3BitVecExpr   - Fixed-width binary operations
└── Z3ArrayExpr<T> - Generic type-safe arrays
```

**Requirements**: .NET 9.0+, Z3 library (auto-discovered)

**License**: MIT