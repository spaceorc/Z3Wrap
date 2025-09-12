# Z3Wrap

A modern C# wrapper for Microsoft's Z3 theorem prover with **unlimited precision arithmetic**, providing a clean and intuitive API for constraint solving and satisfiability checking.

## Features

- **Unlimited Precision Arithmetic** - BigInteger for integers, exact rational arithmetic for reals - no overflow or precision loss
- **Modern C# Design** - Built for .NET 9.0 with nullable reference types, using statements, and idiomatic patterns
- **Type-Safe Expression System** - Strongly typed expressions (`Z3BoolExpr`, `Z3IntExpr`, `Z3RealExpr`) with operator overloading
- **Natural Syntax** - Write constraints using familiar mathematical operators: `x + y == 10`, `p | q`, `x.Implies(y)`
- **Mathematical Correctness** - Exact arithmetic matching Z3's mathematical model (no double precision issues)
- **Comprehensive Operations** - Full support for arithmetic, boolean logic, comparisons, and conditional expressions
- **Memory Management** - Hierarchical disposal system with automatic resource cleanup
- **Model Extraction** - Extract satisfying assignments with unlimited precision value extraction
- **Cross-Platform** - Works on Windows, macOS, and Linux with dynamic library loading
- **Zero Configuration** - Automatically discovers and loads Z3 library on first use
- **Comprehensive Testing** - 343 test cases covering all functionality

## Quick Start

### Installation

```bash
# Clone the repository
git clone https://github.com/yourusername/z3wrap.git
cd z3wrap

# Build the library
dotnet build

# Run tests (requires libz3)
dotnet test
```

### Z3 Library Setup

Z3Wrap automatically discovers and loads the Z3 library on first use. No manual setup required if Z3 is installed in standard locations:

**Install Z3:**
```bash
# macOS (Homebrew)
brew install z3

# Ubuntu/Debian
sudo apt-get install libz3-4

# Windows (via releases)
# Download from https://github.com/Z3Prover/z3/releases
```

**Manual Loading (if needed):**
```csharp
// Explicit path (optional)
NativeMethods.LoadLibrary("/custom/path/libz3.so");

// Automatic discovery (default behavior)
NativeMethods.LoadLibraryAuto();
```

### Basic Usage

Z3Wrap works out of the box with zero configuration and unlimited precision:

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

## Unlimited Precision Arithmetic

### BigInteger Integration - No Integer Overflow

Z3Wrap uses `System.Numerics.BigInteger` throughout, providing unlimited precision integer arithmetic:

```csharp
using System.Numerics;

var x = context.IntConst("x");
var y = context.IntConst("y");

// Work with arbitrarily large integers - no overflow!
var huge = BigInteger.Parse("999999999999999999999999999999");
solver.Assert(x == huge);
solver.Assert(y >= 2147483648); // Beyond int.MaxValue - no problem!

// Natural syntax with implicit conversions
solver.Assert(x + y == 5); // int literals work via implicit conversion

// Unlimited precision extraction
if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    BigInteger value = model.GetIntValue(x); // No precision loss
    Console.WriteLine($"Huge value: {value}");
}
```

### Exact Rational Arithmetic - No Double Precision Issues

Z3Wrap uses a custom `Real` class for exact rational arithmetic, eliminating floating-point precision errors:

```csharp
var a = context.RealConst("a");
var b = context.RealConst("b");

// Exact fractions - no precision loss!
solver.Assert(a == new Real(1, 3));        // Exactly 1/3
solver.Assert(b == Real.Parse("2/7"));     // Exactly 2/7
solver.Assert(a + b == new Real(13, 21));  // Exact: 1/3 + 2/7 = 13/21

// Decimal literals converted to exact fractions
solver.Assert(a >= 0.25m);  // Converted to exact 1/4

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    // Exact extraction - no rounding errors
    string exactValue = model.GetRealValueAsString(a); // Returns "1/3"
    Real realValue = model.GetRealValue(a);           // Exact Real object
    Console.WriteLine($"Exact: {exactValue}");        // "1/3"
    Console.WriteLine($"Decimal approx: {realValue.ToDecimal()}"); // 0.333...
}
```

**Exact Arithmetic Examples:**
```csharp
// Exact arithmetic matching Z3's mathematical model:
solver.Assert(x == 0.1m + 0.2m);  // Exactly equals 0.3 - no precision errors!
solver.Assert(x == new Real(3, 10));  // Explicit exact fraction
solver.Assert(a == new Real(1, 3) + new Real(1, 6));  // Exactly 1/2
```

## Advanced Features

### Boolean Logic

```csharp
var p = context.BoolConst("p");
var q = context.BoolConst("q");
var r = context.BoolConst("r");

// Logical operators
solver.Assert(p & q);           // AND
solver.Assert(p | r);           // OR
solver.Assert(!p);              // NOT
solver.Assert(p ^ q);           // XOR
solver.Assert(p.Implies(q));    // Implication
solver.Assert(q.Iff(r));        // If-and-only-if
```

### Extended Arithmetic

```csharp
var x = context.IntConst("x");
var y = context.IntConst("y");

// Arithmetic with operators - all BigInteger based
solver.Assert(x % 2 == 0);      // x is even
solver.Assert(x.Abs() > 5);     // Absolute value
solver.Assert(-x < y);          // Unary minus

// Min/Max operations
solver.Assert(context.Min(x, y) > 0);
solver.Assert(context.Max(x, BigInteger.Parse("1000000")) < BigInteger.Parse("2000000"));
```

### Conditional Expressions

```csharp
// Type-safe if-then-else using the If method
var result = (x > 0).If(x * 2, x * -2);  // Returns Z3IntExpr
solver.Assert(result == 10);

// Using context Ite method
var conditional = context.Ite(x > y, x, y);  // Maximum of x and y
```

### Large Number Examples

```csharp
// Cryptographic-sized integers
var rsa2048 = BigInteger.Parse("25195908475657893494027183240048398571429282126204032027777137836043662020707595556264018525880784406918290641249515082189298559149176184502808489120072844992687392807287776735971418347270261896375014971824691165077613379859095700097330459748808428401797429100642458691817195118746121515172654632282216869987549182422433637259085141865462043576798423387184774447920739934236584823824281198163815010674810451660377306056201619676256133844143603833904414952634432190114657544454178424020924616515723350778707749817125772467962926386356373289912154831438167899885040445364023527381951378636564391212010397122822120720357");

var privateKey = context.IntConst("d");
var publicExponent = context.IntConst("e");

// Modular arithmetic with massive numbers - no overflow!
solver.Assert(privateKey * publicExponent % rsa2048 == 1);
solver.Assert(privateKey > 1);
solver.Assert(publicExponent == 65537);

// Z3 can find the private key!
if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    BigInteger d = model.GetIntValue(privateKey);
    Console.WriteLine($"Private key: {d}");
}
```

## Architecture

### Expression Hierarchy

```
Z3Expr (abstract base)
├── Z3BoolExpr - Boolean expressions with logical operators
├── Z3IntExpr  - BigInteger expressions with unlimited precision arithmetic
└── Z3RealExpr - Exact rational expressions with Real class
```

### Precision Features

| Type | Z3Wrap Approach | Benefits |
|------|----------------|----------|
| **Integers** | `BigInteger` - unlimited precision | No overflow, handles arbitrarily large numbers |
| **Reals** | `Real` class - exact fractions | No floating-point errors, exact rational arithmetic |
| **Example** | `1/3 + 1/6 = 1/2` exactly | Mathematically precise calculations |

### Memory Management

Z3Wrap uses a hierarchical disposal pattern where the `Z3Context` manages all object lifetimes:

- **Context** tracks all solvers and handles native resource cleanup
- **Solvers** are owned by their context and automatically disposed
- **Expressions** are managed by the context's reference counting
- **Models** are owned by their solver and invalidated on state changes

```csharp
using var context = new Z3Context();  // Will dispose all children
using var solver = context.CreateSolver();  // Optional - context will dispose anyway
// Expressions don't need disposal - managed by context
```

## API Reference

### Core Classes

- **`Z3Context`** - Main entry point for creating expressions and solvers
- **`Z3Solver`** - Constraint solver with push/pop stack operations  
- **`Z3Model`** - Satisfying assignment extraction with unlimited precision value methods
- **`Z3Expr`** - Base class for all expressions with ToString() support
- **`Real`** - Exact rational arithmetic class with BigInteger numerator/denominator

### Extension Methods

The library provides comprehensive extension methods organized by functionality:

- **Primitives** - `Int(BigInteger)`, `Real(Real)`, `BoolConst()`, `True()`, `False()`
- **Arithmetic** - `Add()`, `Sub()`, `Mul()`, `Div()`, `Mod()`, `Abs()`, `UnaryMinus()`
- **Comparison** - `Lt()`, `Le()`, `Gt()`, `Ge()` with mixed-type BigInteger support
- **Equality** - `Eq()`, `Neq()` with comprehensive BigInteger literal overloads
- **Boolean** - `And()`, `Or()`, `Not()`, `Implies()`, `Iff()`, `Xor()`
- **Conditional** - `Ite()` method and `If()` extension for type-safe if-then-else
- **MinMax** - `Min()`, `Max()` with mixed-type BigInteger literal support

### Operators

All expression types support natural mathematical operators with unlimited precision:

```csharp
// Arithmetic: +, -, *, /, % (BigInteger), unary -
var expr1 = x + y * BigInteger.Parse("123456789") - z / 3;
var expr2 = x % 2 == 0;
var expr3 = -x + y.Abs();

// Comparison: ==, !=, <, <=, >, >= (BigInteger compatible)
var constraint1 = x >= y + BigInteger.Parse("999999999999");
var constraint2 = a != new Real(22, 7); // Not equal to 22/7

// Boolean: &, |, ^, !
var formula = p & (q | !r) ^ s;
```

## Development

### Building

```bash
# Restore dependencies and build
dotnet restore
dotnet build

# Build in release mode
dotnet build --configuration Release
```

### Testing

```bash
# Run all tests
dotnet test

# All 343 tests should pass with unlimited precision arithmetic
```

The project includes comprehensive tests with **343 test cases** covering:
- Expression creation and BigInteger operator overloads
- Solver functionality and constraint management
- Model extraction with unlimited precision value retrieval
- Memory management and disposal patterns
- Extended operations and large number edge cases
- Real class exact rational arithmetic
- Mixed-type operations and implicit conversions

### Test Coverage

Current testing: **343 tests passing** with comprehensive coverage of:
- **Core functionality** - Context, solver, expressions, models
- **Unlimited precision** - BigInteger integration, Real class arithmetic
- **Mathematical correctness** - Exact arithmetic, no precision loss
- **Edge cases** - Large numbers, exact fractions, operator combinations
- **Memory safety** - Disposal patterns, resource management

## Requirements

- **.NET 9.0** or later
- **Z3 Library** - Microsoft Z3 theorem prover
  - **Automatic discovery** - Z3Wrap automatically finds Z3 in standard locations:
    - Windows: `libz3.dll`, `z3.dll` in Program Files or current directory
    - macOS: `libz3.dylib` via Homebrew (`/opt/homebrew/opt/z3/lib/`, `/usr/local/opt/z3/lib/`)
    - Linux: `libz3.so` via package managers (`/usr/lib/`, `/usr/lib64/`, etc.)
  - **Manual loading** - Custom paths supported via `NativeMethods.LoadLibrary(path)`
  - **Zero configuration** - Works out of the box with standard Z3 installations

Install Z3 using your platform's package manager or download from [Z3 releases](https://github.com/Z3Prover/z3/releases).

## Design Principles

Z3Wrap is designed with **mathematical correctness** as a core principle:
- **Unlimited precision integers** - BigInteger handles arbitrarily large numbers without overflow
- **Exact rational arithmetic** - Real class uses exact fractions, eliminating floating-point errors
- **Type safety** - Strongly typed expressions prevent common mistakes
- **Natural syntax** - Mathematical operators work as expected: `x + y == 10`
- **Memory safety** - Automatic hierarchical disposal prevents resource leaks
- **Zero configuration** - Auto-discovery and loading of Z3 library

## License

This project is licensed under the MIT License. Z3 itself is licensed under the MIT License by Microsoft Corporation.

## Contributing

Contributions are welcome! Please ensure:

1. All tests pass: `dotnet test`
2. Code follows existing patterns and conventions
3. New features include comprehensive tests
4. Documentation is updated for public APIs
5. Maintain unlimited precision arithmetic principles

## Acknowledgments

This library wraps Microsoft's Z3 Theorem Prover. Special thanks to the Z3 team for creating such a powerful constraint solving engine.

**Z3Wrap provides unlimited precision arithmetic to match Z3's mathematical capabilities - no more integer overflow or floating-point precision issues!**