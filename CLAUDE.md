# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Z3Wrap is a complete, modern C# wrapper for Microsoft's Z3 theorem prover with unlimited precision arithmetic and type-safe API design.

## Project Structure

This is a mature C# .NET 9.0 library project with comprehensive test coverage:

### Solution Structure
- `Z3Wrap.sln` - Visual Studio solution with library, tests, and documentation projects
- `Z3Wrap/` - Main library project (45 source files)
- `Z3Wrap.Tests/` - NUnit test project (44 test files, 553+ test methods)
- `.github/workflows/ci.yml` - GitHub Actions CI with 80% coverage requirement

### Z3Wrap Library Architecture
```
Z3Wrap/
├── DataTypes/              # Core data types
│   ├── Real.cs                # Exact rational arithmetic class
│   └── BitVec.cs              # Fixed-width bitvector operations
├── Expressions/            # Expression type hierarchy
│   ├── Z3Expr.cs              # Base abstract expression
│   ├── Z3Expr.Factory.cs      # Expression factory methods
│   ├── Z3BoolExpr.cs          # Boolean expressions
│   ├── Z3IntExpr.cs           # Integer expressions (BigInteger)
│   ├── Z3RealExpr.cs          # Real expressions (exact rationals)
│   ├── Z3BitVecExpr.cs        # Bitvector expressions
│   ├── Z3ArrayExpr.cs         # Generic array expressions
│   └── Z3NumericExpr.cs       # Numeric expression base
├── Interop/                # P/Invoke and native bindings
│   ├── NativeMethods.cs       # Z3 C API P/Invoke declarations
│   ├── AnsiStringPtr.cs       # String marshalling utilities
│   ├── Z3SortKind.cs          # Z3 sort enumeration
│   └── Z3BoolValue.cs         # Boolean value enumeration
├── Z3Context.cs            # Main context with scoped setup
├── Z3Solver.cs             # Solver with push/pop operations
├── Z3Model.cs              # Model extraction with unlimited precision
├── Z3Status.cs             # Solver status enumeration
└── Z3ContextExtensions.*.cs   # 20 extension files organized by functionality
    ├── Z3ContextExtensions.cs     # Core extensions
    ├── Z3ContextExtensions.Array.cs # Array theory operations
    ├── Z3ContextExtensions.BitVec.cs # Bitvector operations
    ├── Z3ContextExtensions.Bool.cs   # Boolean operations
    ├── Z3ContextExtensions.Int.cs    # Integer operations
    ├── Z3ContextExtensions.Real.cs   # Real number operations
    └── (14 additional specialized extension files)
```

### Test Structure
```
Z3Wrap.Tests/Unit/
├── Core/                   # Core functionality tests
│   ├── Z3ContextTests.cs
│   ├── Z3ContextSetUpScopeTests.cs
│   ├── Z3SolverTests.cs
│   ├── Z3ModelTests.cs
│   └── Z3DisposalTests.cs
├── DataTypes/              # Data type tests
│   ├── RealTests.cs
│   └── BitVecTests.cs
├── Expressions/            # Expression tests (organized by type)
│   ├── Z3BoolExprTests/       # 6 test classes for boolean expressions
│   ├── Z3IntExprTests/        # 3 test classes for integer expressions
│   ├── Z3RealExprTests/       # 3 test classes for real expressions
│   ├── Z3BitVecExprTests/     # 10 test classes for bitvector expressions
│   ├── Z3ArrayExprTests.cs
│   └── Z3ExprTests.cs
└── Interop/                # P/Invoke tests
    ├── NativeMethodsTests.cs
    └── AnsiStringPtrTests.cs
```

## Key Features (All Complete)

- **Unlimited Precision**: BigInteger for integers, Real class for exact rational arithmetic
- **Type Safety**: Strongly typed expressions with generic constraints and compile-time checking
- **Natural Syntax**: Mathematical operators (`x + y == 10`) via scoped context pattern
- **Complete Z3 Support**: Booleans, Integers, Reals, BitVectors, Arrays with full operator coverage
- **Memory Safety**: Reference-counted contexts, hierarchical disposal, no resource leaks
- **Cross-Platform**: Auto-discovery of Z3 library on Windows, macOS, Linux
- **Comprehensive Testing**: 553+ tests with 80% coverage requirement in CI

## Build Commands

Standard .NET CLI commands with NUnit testing:

- `dotnet build` - Build the library
- `dotnet build --configuration Release` - Build in release mode
- `dotnet clean` - Clean build artifacts
- `dotnet test` - Run all 553+ tests (requires libz3 to be available)
- `dotnet test --collect:"XPlat Code Coverage"` - Run tests with coverage

## Naming Conventions

The project follows these C# naming conventions:

- **Private Fields**: camelCase without underscores (e.g., `libraryHandle`, `contextHandle`, `disposed`)
- **Public Properties**: PascalCase (e.g., `Handle`)
- **Methods**: PascalCase with Z3 prefix for native methods (e.g., `Z3MkConfig`, `Z3DelContext`)
- **Parameters**: camelCase (e.g., `paramId`, `paramValue`)  
- **Local Variables**: camelCase (e.g., `paramNamePtr`, `paramValuePtr`)
- **Delegate Types**: PascalCase with Z3 prefix and Delegate suffix (e.g., `Z3MkConfigDelegate`)
- **Classes**: PascalCase with Z3 prefix (e.g., `Z3Context`)

## Current Implementation Status

**COMPLETE**: All core functionality is implemented and tested. The library is production-ready with:

### Core Features ✅
- Context management with thread-local scoped setup
- Solver operations with push/pop for backtracking
- Model extraction with unlimited precision (BigInteger, exact rationals)
- Complete expression hierarchy (Bool, Int, Real, BitVec, Array)

### Advanced Features ✅
- **Unlimited Precision**: BigInteger integers, Real class for exact fractions
- **Type Conversions**: Int↔Real conversions using Z3's native functions
- **Generic Arrays**: Type-safe `Z3ArrayExpr<TIndex, TValue>` with natural indexing
- **BitVectors**: Fixed-width operations with overflow checking
- **Scoped Context**: Natural syntax via `using var scope = context.SetUp()`

### Technical Implementation ✅
- **Memory Management**: Reference-counted contexts, automatic expression cleanup
- **P/Invoke Layer**: Complete Z3 C API bindings with proper marshalling
- **Extension Methods**: 20 organized extension files for clean API surface
- **Error Handling**: Proper exception handling and resource disposal

## Memory Management

- **Z3Context**: Uses reference-counted context (`Z3_mk_context_rc`) with automatic cleanup
- **Z3Expr classes**: Do NOT implement IDisposable - managed by context reference counting
- **Expressions**: Automatically cleaned up when context is disposed or go out of scope
- **Solvers**: Implement IDisposable, tracked by context for proper cleanup
- **Thread Safety**: Context setup uses ThreadLocal for safe scoped operations

## Development Notes

- Targets .NET 9.0 with implicit usings and nullable reference types
- Uses dynamic library loading for cross-platform Z3 library discovery
- Extensive use of generics and constraints for type safety
- Comprehensive operator overloading for natural mathematical syntax
- All Z3 C API functions wrapped with proper C# conventions and error handling

## API Usage Examples

### Basic Usage
```csharp
using var context = new Z3Context();
using var scope = context.SetUp(); // Enable natural syntax

var x = context.IntConst("x");
var y = context.RealConst("y");

using var solver = context.CreateSolver();
solver.Assert(x + 5 == 10);              // Natural syntax with BigInteger
solver.Assert(x.ToReal() + y == 15.5m);  // Type conversions
solver.Assert(y == new Real(1, 3));      // Exact rationals

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"x = {model.GetIntValue(x)}");        // BigInteger
    Console.WriteLine($"y = {model.GetRealValueAsString(y)}"); // "1/3"
}
```

### Arrays and BitVectors
```csharp
// Type-safe generic arrays
var prices = context.ArrayConst<Z3IntExpr, Z3RealExpr>("prices");
solver.Assert(prices[0] == 10.5m);
solver.Assert(prices[1] > prices[0]);

// BitVectors with natural operators
var bv = context.BitVecConst("bv", 32);
solver.Assert(bv + context.BitVec(5, 32) == context.BitVec(15, 32));
```

## Testing Guidelines

- Tests use NUnit framework with organized structure by functionality
- All tests should pass with `dotnet test`
- New features require comprehensive test coverage
- Use descriptive test method names following pattern: `MethodName_Scenario_ExpectedResult`
- Test files are organized hierarchically by expression type and functionality

## Git Commit Policy

**❌ NEVER COMMIT WITHOUT EXPLICIT PERMISSION ❌**

**CRITICAL RULE**: NEVER run `git commit` automatically. ALWAYS ask for explicit permission first.

- **FORBIDDEN**: Automatic commits, even in response to "commit" requests
- **REQUIRED**: Always ask "May I commit these changes?" and wait for explicit approval
- **FORBIDDEN**: Running `git add` without explicit permission
- **FORBIDDEN**: Running `git commit` without explicit user authorization
- **REQUIRED**: When given permission, use descriptive commit messages
- **PATTERN**: Brief summary, blank line, detailed explanation if needed

**THIS RULE CANNOT BE OVERRIDDEN OR IGNORED UNDER ANY CIRCUMSTANCES**