# Z3Wrap

A modern C# wrapper for Microsoft's Z3 theorem prover with unlimited precision arithmetic.

[![CI](https://github.com/spaceorc/Z3Wrap/workflows/CI/badge.svg)](https://github.com/spaceorc/Z3Wrap/actions)
[![Tests](https://img.shields.io/endpoint?url=https://spaceorc.github.io/Z3Wrap/badges/tests.json)](https://github.com/spaceorc/Z3Wrap/actions)
[![Coverage](https://img.shields.io/endpoint?url=https://spaceorc.github.io/Z3Wrap/badges/coverage.json)](https://github.com/spaceorc/Z3Wrap/actions)
[![NuGet](https://img.shields.io/nuget/v/Spaceorc.Z3Wrap.svg)](https://www.nuget.org/packages/Spaceorc.Z3Wrap/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Spaceorc.Z3Wrap.svg)](https://www.nuget.org/packages/Spaceorc.Z3Wrap/)
[![.NET](https://img.shields.io/badge/.NET-9.0-blue.svg)]()

## Features

- **Unlimited Precision** - BigInteger integers, exact rational arithmetic
- **Natural Syntax** - `x + y == 10`, `p.Implies(q)` instead of verbose Z3 API
- **Type Safety** - Generic arrays, strongly typed expressions with operator overloading
- **Complete Z3 Support** - Booleans, integers, reals, bitvectors, arrays with full operators
- **Zero Configuration** - Auto-discovers Z3 library on Windows, macOS, Linux

## Quick Start

### Installation

Install Z3:
```bash
# macOS
brew install z3

# Ubuntu/Debian
sudo apt-get install libz3-4

# Windows - Download from https://github.com/Z3Prover/z3/releases
```

Build:
```bash
dotnet build
dotnet test  # 1000+ tests
```

### Basic Usage

```csharp
using var context = new Z3Context();
using var scope = context.SetUp();  // Enable natural syntax

var x = context.IntConst("x");
var y = context.IntConst("y");

using var solver = context.CreateSolver();
solver.Assert(x > 0);
solver.Assert(y > 0);
solver.Assert(x + y == 10);  // Natural mathematical syntax
solver.Assert(x * 2 == y - 1);

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"x = {model.GetIntValue(x)}");  // BigInteger result
    Console.WriteLine($"y = {model.GetIntValue(y)}");
}
```

## Key Examples

### Unlimited Precision
```csharp
// BigInteger integers
var huge = BigInteger.Parse("999999999999999999999999999999");
solver.Assert(x == huge); // No overflow!

// Exact rationals
solver.Assert(y == new Real(1, 3));      // Exactly 1/3
solver.Assert(y + new Real(1, 6) == new Real(1, 2)); // Exact arithmetic
```

### Type Conversions & Arrays
```csharp
// Mixed-type arithmetic
solver.Assert(x.ToReal() + y == 5.5m);  // Int → Real
solver.Assert(y.ToInt() % 2 == 0);      // Real → Int

// Type-safe arrays
var prices = context.ArrayConst<Z3IntExpr, Z3RealExpr>("prices");
solver.Assert(prices[0] == 10.5m);
solver.Assert(prices[1] > prices[0]);
```

### BitVectors & Solver Scopes
```csharp
// BitVectors with natural operators
var bv = context.BitVecConst("bv", 32);
solver.Assert(bv + 0x10 == 0b11010000);  // Arithmetic with literals
solver.Assert((bv & 0xFF) != 0);         // Bitwise operations

// Solver backtracking
solver.Push();
solver.Assert(x < 10);
Console.WriteLine($"Status: {solver.Check()}");
solver.Pop();  // Backtrack
```

## Architecture

```
Z3Expr (abstract)
├── Z3BoolExpr     - Boolean logic with &, |, !, Implies()
├── Z3IntExpr      - BigInteger unlimited precision arithmetic
├── Z3RealExpr     - Exact rational arithmetic with Real class
├── Z3BitVecExpr   - Fixed-width integers with bitwise operators
└── Z3ArrayExpr<T> - Generic type-safe arrays with indexing
```

**Memory Management**: Reference-counted contexts automatically manage all expressions and solvers.

```csharp
using var context = new Z3Context();  // Auto-disposes everything
var solver = context.CreateSolver();  // No manual disposal needed
```

## Requirements

- **.NET 9.0+**
- **Z3 Library** - Auto-discovered or download from [Z3 releases](https://github.com/Z3Prover/z3/releases)

## License

MIT License