# Z3 C# Wrapper Implementation Plan

## Project Status: Core Complete with Unlimited Precision ✅

Z3Wrap is a modern C# wrapper for Microsoft's Z3 theorem prover with **complete core functionality and unlimited precision arithmetic**.

### Current State
- ✅ **Foundation Complete**: Context management, expression types, memory management
- ✅ **Solving Complete**: Z3Solver with full constraint solving capabilities
- ✅ **Model Extraction Complete**: Full value extraction from satisfying assignments with unlimited precision
- ✅ **Extended Operations Complete**: Boolean logic, arithmetic, comparisons, if-then-else
- ✅ **Type Conversions Complete**: Z3_mk_int2real and Z3_mk_real2int support with ToReal()/ToInt() methods
- ✅ **Architecture Mature**: Comprehensive test coverage, hierarchical disposal, modern C# patterns
- ✅ **Unlimited Precision**: BigInteger integration for integers, Real class for exact rationals
- ✅ **355 Tests Passing**: Comprehensive test suite covering all functionality including type conversions

## Current Architecture

### Project Structure
```
Z3Wrap/
├── Expressions/         # Expression hierarchy (Z3Wrap.Expressions)
│   ├── Z3Expr.cs       # Base abstract expression class
│   ├── Z3BoolExpr.cs   # Boolean expressions
│   ├── Z3IntExpr.cs    # Integer expressions  
│   └── Z3RealExpr.cs   # Real number expressions
├── Interop/            # Implementation details (Z3Wrap.Interop)
│   ├── NativeMethods.cs      # P/Invoke declarations
│   ├── AnsiStringPtr.cs      # String marshalling helper
│   └── Z3SortKind.cs         # Z3 sort enumeration
└── (root)              # Core API (Z3Wrap namespace)
    ├── Z3Context.cs            # Main context class
    ├── Z3Solver.cs             # Solver operations  
    ├── Z3Model.cs              # Model extraction
    ├── Z3Status.cs             # Status enumeration
    ├── Z3BoolValue.cs          # Boolean value enumeration
    └── Z3ContextExtensions.*.cs # Extension methods
```

### Current Usage (Fully Working)
```csharp
using var context = new Z3Context();

// Create variables and constraints
var x = context.IntConst("x");
var y = context.IntConst("y");
var r = context.RealConst("r");
var p = context.BoolConst("p");

using var solver = context.CreateSolver();
solver.Assert(x + y == context.Int(10));
solver.Assert(x > context.Int(0));
solver.Assert(p.Implies(x % 2 == context.Int(0))); // Extended operations work!

// Type conversions for mixed arithmetic
solver.Assert(x.ToReal() + r == context.Real(15.5m)); // int to real conversion
solver.Assert(r.ToInt() >= context.Int(5));           // real to int conversion

// Solve and extract model
if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"x = {model.GetIntValue(x)}");
    Console.WriteLine($"y = {model.GetIntValue(y)}");
    Console.WriteLine($"r = {model.GetRealValueAsString(r)}");
    Console.WriteLine($"p = {model.GetBoolValue(p)}");
}
```

## Real Class Implementation: COMPLETED ✅

### Problem Statement
Z3Wrap previously used `double` for real numbers, which was **fundamentally wrong** because:
- Z3's real sort represents **exact rational arithmetic** (fractions), not floating-point
- `double` introduces precision errors that don't exist in Z3's mathematical model  
- Users couldn't express exact fractions like 1/3 without corruption
- API misled users about Z3's exact arithmetic capabilities

### Solution: Real Class ✅ COMPLETED
Successfully implemented a proper `Real` class that provides exact rational arithmetic matching Z3's design.

#### Phase 1: Real Class ✅ COMPLETED
- ✅ **Created Z3Wrap/Real.cs** with full exact rational arithmetic using BigInteger
- ✅ **Constructors**: `Real(int num, int den)`, `Real(string "1/3")`, `Real(decimal)`, etc.
- ✅ **Arithmetic**: `+`, `-`, `*`, `/` with exact rational operations
- ✅ **Comparisons**: `==`, `!=`, `<`, `>`, `<=`, `>=`
- ✅ **Conversions**: Implicit from int/long/decimal/BigInteger, explicit to decimal/int/long/BigInteger
- ✅ **String formats**: Fraction ("1/3") and decimal approximation ("0.25")
- ✅ **Properties**: `Numerator`, `Denominator`, `IsInteger`, `IsZero`, `IsPositive`, `IsNegative`
- ✅ **Additional Methods**: `Abs()`, `Reciprocal()`, `Power()`, `Round()`, `Min()`, `Max()`

#### Phase 2: Core Integration ✅ COMPLETED  
Successfully replaced `double` usage throughout the codebase:
- ✅ **Z3RealExpr.cs** - All operator overloads now use `Real` instead of `double`
- ✅ **Z3ContextExtensions.Primitives.cs** - `Real(Real)` instead of `Real(double)`
- ✅ **Z3ContextExtensions.NumericOperators.cs** - All arithmetic operations use `Real`
- ✅ **Z3ContextExtensions.Comparison.cs** - All comparison operations use `Real`
- ✅ **Z3ContextExtensions.Equality.cs** - All equality operations use `Real`
- ✅ **Z3ContextExtensions.MinMax.cs** - Min/Max operations use `Real`

#### Phase 3: Test Integration ✅ COMPLETED
- ✅ **Updated all test files** to use decimal literals with `m` suffix (e.g., `3.14m`)
- ✅ **Preserved all existing method wrapping** (e.g., `context.Real()`, `context.Int()`)
- ✅ **Added comprehensive Real class tests** - 68 tests covering all functionality
- ✅ **Added explicit conversion tests** - Complete coverage of all conversion operators
- ✅ **All 341 tests passing** - No regressions, full backward compatibility

#### Phase 4: Enhanced Model Extraction ✅ ALREADY EXISTS
- ✅ **Z3Model already has exact extraction** - `GetRealValueAsString()` provides exact rational representation
- ✅ **Backward compatibility maintained** - Existing string-based methods work perfectly

### API Achievement: Exact Rational Arithmetic ✅

#### Before (Wrong):
```csharp
solver.Assert(x == 0.333333); // Imprecise floating-point!
solver.Assert(x + 0.166667 == 0.5); // Wrong due to precision errors!
```

#### Now (Correct):
```csharp
solver.Assert(x == new Real(1, 3)); // Exact: 1/3
solver.Assert(x + new Real(1, 6) == new Real(1, 2)); // Exact arithmetic!

// Natural syntax with implicit conversions
solver.Assert(x == Real.Parse("1/3"));
solver.Assert(y >= 5); // Implicit int conversion
solver.Assert(z == 3.14m); // Implicit decimal conversion

// Exact model extraction (already working)
string exactValue = model.GetRealValueAsString(x); // Returns "1/3" exactly
Console.WriteLine(exactValue); // "1/3"
```

## Future Enhancements

### Type Conversions: COMPLETED ✅

**Goal**: Add support for Z3's `Z3_mk_int2real` and `Z3_mk_real2int` functions to enable seamless conversion between integer and real expressions at the Z3 level.

- ✅ **Native Methods**: Added `Z3_mk_int2real` and `Z3_mk_real2int` function loading and C# wrappers
- ✅ **Context Extensions**: Added `context.ToReal(Z3IntExpr)` and `context.ToInt(Z3RealExpr)` extension methods
- ✅ **Instance Methods**: Added `intExpr.ToReal()` and `realExpr.ToInt()` instance methods for convenient syntax
- ✅ **Test Coverage**: 12 comprehensive tests covering both APIs, mixed-type solving, and edge cases
- ✅ **Model Integration**: Full compatibility with existing model extraction methods

#### Achievement: Mixed-Type Arithmetic ✅

**API Usage:**
```csharp
var x = context.IntConst("x");
var y = context.RealConst("y");

// Instance methods (recommended)
solver.Assert(x.ToReal() + y == context.Real(5.5m));
solver.Assert(y.ToInt() % context.Int(2) == context.Int(0));

// Extension methods (alternative)
solver.Assert(context.ToReal(x) >= context.Real(10));
solver.Assert(context.ToInt(y) < context.Int(100));
```

**Benefits:**
- **Mixed Constraints**: Enable complex constraints requiring both integer and real variables
- **Type Safety**: Proper return types (`Z3IntExpr` vs `Z3RealExpr`) with compile-time checking  
- **Z3 Semantics**: Uses Z3's native type conversion for correct mathematical behavior
- **Dual APIs**: Both instance methods and extension methods for user preference

### BigInteger Integration: COMPLETED ✅

**Goal**: Replace `int` with `BigInteger` throughout Z3IntExpr system for unlimited precision integer arithmetic
- ✅ **Z3IntExpr Operations**: All operator overloads now use `BigInteger` instead of `int`
- ✅ **Z3ContextExtensions**: All integer operations (arithmetic, comparison, equality, min/max) support `BigInteger`
- ✅ **Model Extraction**: `GetIntValue()` now returns `BigInteger` using string-based extraction for unlimited precision
- ✅ **Test Integration**: All 343 tests passing with proper `BigInteger` assertions
- ✅ **Backward Compatibility**: Existing code with `int` literals continues to work via implicit conversion

#### Achievement: Unlimited Precision Integer Arithmetic ✅

**Before (Limited):**
```csharp
solver.Assert(x == 2147483647); // int.MaxValue - limited range
// Overflow with very large numbers!
```

**Now (Unlimited):**
```csharp
// Natural syntax with implicit conversions
solver.Assert(x == 5); // Implicit int → BigInteger
solver.Assert(y >= 2147483648); // Beyond int.MaxValue - no problem!

// Work with arbitrarily large integers
var huge = BigInteger.Parse("999999999999999999999999999999");
solver.Assert(z == huge); // No overflow!

// Exact model extraction
BigInteger exactValue = model.GetIntValue(x); // Unlimited precision
Console.WriteLine(exactValue); // Can represent any integer size
```

### Advanced Types (Later)
- **Bit Vectors** - Fixed-width integer operations
- **Arrays** - Array/map theory support  
- **Strings** - String constraint solving
- **Quantifiers** - ForAll/Exists expressions

### Architecture Benefits Achieved ✅
- **Mathematical Correctness**: Exact rational arithmetic (Real class) and unlimited precision integers (BigInteger) matching Z3's design
- **Type Conversions**: Seamless conversion between integer and real expressions using Z3's native functions
- **Unlimited Precision**: BigInteger for integers, Real class for exact rationals - no overflow or precision loss
- **Memory Safety**: Hierarchical disposal, no resource leaks
- **Type Safety**: Strongly typed expressions with compile-time checking
- **Natural Syntax**: Operator overloading with mixed-type support and implicit conversions
- **Modern C#**: Nullable types, using statements, seamless integration with .NET numeric types
- **Comprehensive Testing**: 355 test cases covering all functionality including type conversions and unlimited precision arithmetic
- **Cross-Platform**: Works on Windows, macOS, Linux with auto-discovery
- **Zero Configuration**: Automatically finds and loads Z3 library
- **Backward Compatibility**: Seamless migration from int/double-based APIs

Z3Wrap now provides **unlimited precision arithmetic** for both integers (BigInteger) and rationals (Real class) with **seamless type conversions** while maintaining its clean, intuitive API design. All core enhancements are complete and fully integrated.