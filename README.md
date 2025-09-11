# Z3Wrap

A modern C# wrapper for Microsoft's Z3 theorem prover, providing a clean and intuitive API for constraint solving and satisfiability checking.

## Features

- **Modern C# Design** - Built for .NET 9.0 with nullable reference types, using statements, and idiomatic patterns
- **Type-Safe Expression System** - Strongly typed expressions (`Z3BoolExpr`, `Z3IntExpr`, `Z3RealExpr`) with operator overloading
- **Natural Syntax** - Write constraints using familiar mathematical operators: `x + y == 10`, `p | q`, `x.Implies(y)`
- **Comprehensive Operations** - Full support for arithmetic, boolean logic, comparisons, and conditional expressions
- **Memory Management** - Hierarchical disposal system with automatic resource cleanup
- **Model Extraction** - Extract satisfying assignments with convenient value extraction methods
- **Cross-Platform** - Works on Windows, macOS, and Linux with dynamic library loading

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

### Basic Usage

```csharp
using var context = new Z3Context();

// Create variables
var x = context.MkIntConst("x");
var y = context.MkIntConst("y");

// Create solver and add constraints
using var solver = context.MkSolver();
solver.Assert(x > 0);
solver.Assert(y > 0);
solver.Assert(x + y == 10);
solver.Assert(x * 2 == y + 1);

// Check satisfiability
if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"x = {model.GetIntValue(x)}");  // e.g., x = 3
    Console.WriteLine($"y = {model.GetIntValue(y)}");  // e.g., y = 7
}
```

## Advanced Features

### Boolean Logic

```csharp
var p = context.MkBoolConst("p");
var q = context.MkBoolConst("q");
var r = context.MkBoolConst("r");

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
var x = context.MkIntConst("x");
var y = context.MkIntConst("y");

// Arithmetic with operators
solver.Assert(x % 2 == 0);      // x is even
solver.Assert(x.Abs() > 5);     // Absolute value
solver.Assert(-x < y);          // Unary minus

// Min/Max operations
solver.Assert(context.Min(x, y) > 0);
solver.Assert(context.Max(x, 10) < 20);
```

### Conditional Expressions

```csharp
// Type-safe if-then-else
var result = (x > 0).If(x * 2, x * -2);  // Returns Z3IntExpr
solver.Assert(result == 10);

// Generic factory method
var conditional = context.MkIte<Z3IntExpr>(x > y, x, y);  // Maximum of x and y
```

### Real Numbers

```csharp
var a = context.MkRealConst("a");
var b = context.MkRealConst("b");

solver.Assert(a + b == 3.14);
solver.Assert(a * a + b * b <= 1.0);  // Inside unit circle

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"a = {model.GetRealValueAsString(a)}");
    Console.WriteLine($"b = {model.GetRealValueAsString(b)}");
}
```

## Architecture

### Expression Hierarchy

```
Z3Expr (abstract base)
├── Z3BoolExpr - Boolean expressions with logical operators
├── Z3IntExpr  - Integer expressions with arithmetic operators  
└── Z3RealExpr - Real number expressions with arithmetic operators
```

### Memory Management

Z3Wrap uses a hierarchical disposal pattern where the `Z3Context` manages all object lifetimes:

- **Context** tracks all solvers and handles native resource cleanup
- **Solvers** are owned by their context and automatically disposed
- **Expressions** are managed by the context's reference counting
- **Models** are owned by their solver and invalidated on state changes

```csharp
using var context = new Z3Context();  // Will dispose all children
using var solver = context.MkSolver();  // Optional - context will dispose anyway
// Expressions don't need disposal - managed by context
```

## API Reference

### Core Classes

- **`Z3Context`** - Main entry point for creating expressions and solvers
- **`Z3Solver`** - Constraint solver with push/pop stack operations  
- **`Z3Model`** - Satisfying assignment extraction with value methods
- **`Z3Expr`** - Base class for all expressions with ToString() support

### Extension Methods

The library provides comprehensive extension methods organized by functionality:

- **Primitives** - `Int()`, `Real()`, `BoolConst()`, `True()`, `False()`
- **Arithmetic** - `Add()`, `Sub()`, `Mul()`, `Div()`, `Mod()`, `Abs()`, `UnaryMinus()`
- **Comparison** - `Lt()`, `Le()`, `Gt()`, `Ge()` with mixed-type support
- **Equality** - `Eq()`, `Neq()` with comprehensive literal overloads
- **Boolean** - `And()`, `Or()`, `Not()`, `Implies()`, `Iff()`, `Xor()`
- **Conditional** - `MkIte()` with generic and non-generic overloads
- **MinMax** - `Min()`, `Max()` with mixed-type literal support

### Operators

All expression types support natural mathematical operators:

```csharp
// Arithmetic: +, -, *, /, % (integers), unary -
var expr1 = x + y * 2 - z / 3;
var expr2 = x % 2 == 0;
var expr3 = -x + y.Abs();

// Comparison: ==, !=, <, <=, >, >=
var constraint1 = x >= y + 1;
var constraint2 = a != 3.14;

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

# Run with coverage
make coverage

# Watch mode for TDD
make watch
```

The project includes comprehensive tests with 136+ test cases covering:
- Expression creation and operator overloads
- Solver functionality and constraint management
- Model extraction and value retrieval
- Memory management and disposal patterns
- Extended operations and edge cases

### Code Coverage

Generate detailed coverage reports:

```bash
make coverage-open  # Generate and open HTML coverage report
```

Current coverage: **86.9% line coverage** with comprehensive testing of all core functionality.

## Requirements

- **.NET 9.0** or later
- **libz3** - Z3 native library must be available at runtime
  - Windows: `libz3.dll` or `z3.dll`
  - macOS: `libz3.dylib` or `z3.dylib`  
  - Linux: `libz3.so` or `z3.so`

The library uses dynamic loading to locate Z3 at runtime, so you can specify custom paths if needed.

## License

This project is licensed under the MIT License. Z3 itself is licensed under the MIT License by Microsoft Corporation.

## Contributing

Contributions are welcome! Please ensure:

1. All tests pass: `dotnet test`
2. Code follows existing patterns and conventions
3. New features include comprehensive tests
4. Documentation is updated for public APIs

## Acknowledgments

This library wraps Microsoft's Z3 Theorem Prover. Special thanks to the Z3 team for creating such a powerful constraint solving engine.