# Z3Wrap

A modern C# wrapper for Microsoft's Z3 theorem prover with unlimited precision arithmetic.

## Features

- **Unlimited Precision** - BigInteger for integers, exact rational arithmetic for reals
- **Type-Safe** - Strongly typed expressions with operator overloading  
- **Natural Syntax** - Write constraints using familiar operators: `x + y == 10`, `p.Implies(q)`
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

// Create variables
var x = context.IntConst("x");
var y = context.IntConst("y");

// Create solver and add constraints
using var solver = context.CreateSolver();
solver.Assert(x > 0);
solver.Assert(y > 0);
solver.Assert(x + y == 10);
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

### Boolean Logic

```csharp
var p = context.BoolConst("p");
var q = context.BoolConst("q");

solver.Assert(p & q);           // AND
solver.Assert(p.Implies(q));    // Implication
```

## Architecture

### Expression Hierarchy

```
Z3Expr (abstract base)
├── Z3BoolExpr - Boolean expressions with logical operators
├── Z3IntExpr  - BigInteger expressions with unlimited precision arithmetic
└── Z3RealExpr - Exact rational expressions with Real class
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
- **`Z3Solver`** - Constraint solver with push/pop stack operations  
- **`Z3Model`** - Model extraction with unlimited precision value methods
- **`Z3Expr`** - Base class for all expressions
- **`Real`** - Exact rational arithmetic class

### Operators

```csharp
// Arithmetic: +, -, *, /, % (BigInteger), unary -
var expr = x + y * BigInteger.Parse("123456789");

// Comparison: ==, !=, <, <=, >, >= (BigInteger compatible)  
var constraint = x >= BigInteger.Parse("999999999999");

// Boolean: &, |, ^, !
var formula = p & (q | !r);
```

## Requirements

- **.NET 9.0** or later
- **Z3 Library** - Automatically discovered in standard locations or download from [Z3 releases](https://github.com/Z3Prover/z3/releases)

## License

MIT License