# Z3Wrap Bitvector Implementation Plan

Based on comprehensive research of Z3's bitvector theory and the current Z3Wrap architecture, here's the implementation plan:

## Overview
Implement full Z3 bitvector theory support with type-safe generic bitvector expressions, following the established patterns in Z3Wrap for unlimited precision arithmetic, natural syntax, and comprehensive operator overloading.

## Phase 1: Foundation (Core Infrastructure) ✅ COMPLETED

### 1.1 Native Methods Extension ✅
- Added Z3 C API bitvector function delegates to `NativeMethods.cs`:
  - **Sort Creation**: `Z3MkBvSort(ctx, size)` ✅
  - **Creation**: `Z3MkBvNumeral(ctx, numStr, size)` ✅
  - **Arithmetic**: `Z3MkBvadd`, `Z3MkBvsub`, `Z3MkBvmul`, `Z3MkBvudiv`, `Z3MkBvsdiv`, `Z3MkBvurem`, `Z3MkBvsrem` ✅
  - **Bitwise**: `Z3MkBvand`, `Z3MkBvor`, `Z3MkBvxor`, `Z3MkBvnot`, `Z3MkBvneg` ✅
  - **Shifts**: `Z3MkBvshl`, `Z3MkBvlshr`, `Z3MkBvashr` ✅
  - **Comparisons**: `Z3MkBvult`, `Z3MkBvslt`, `Z3MkBvule`, `Z3MkBvsle`, `Z3MkBvugt`, `Z3MkBvsgt`, `Z3MkBvuge`, `Z3MkBvsge` ✅
  - **Extensions**: `Z3MkSignExt`, `Z3MkZeroExt`, `Z3MkExtract`, `Z3MkRepeat` ✅
  - **Conversions**: `Z3MkBv2int`, `Z3MkInt2bv` ✅
  - **Overflow Detection**: `Z3MkBvaddNoOverflow`, `Z3MkBvsubNoOverflow`, `Z3MkBvmulNoOverflow` ✅

### 1.2 Z3BitVecExpr Class ✅
Created `Z3Wrap/Expressions/Z3BitVecExpr.cs` with:
- Simple `Z3BitVecExpr` class inheriting from `Z3NumericExpr` ✅
- Size property for bit width tracking ✅
- Factory pattern consistent with other expression types ✅
- ToString() override showing size and content ✅

### 1.3 BitVec Creation and Context Extensions ✅
- Implemented `Z3ContextExtensions.BitVectors.cs` with BitVec creation from int values ✅
- Added `BitVecConst(name, size)` for creating BitVec variables ✅
- Added `BitVec(value, size)` for creating BitVec constants ✅

### 1.4 Model Value Extraction ✅
- Enhanced `Z3Model.cs` with comprehensive BitVec value extraction:
  - `GetBitVecValueAsBigInteger()` ✅
  - `GetBitVecValueAsString()` ✅
  - `GetBitVecValueAsBinaryString()` (manual conversion) ✅
  - `GetBitVecValueAsInt/UInt/Long/ULong()` with overflow handling ✅
- Created `Z3NumericExpr` base class for type hierarchy ✅
- Consolidated to single `ExtractNumeralString()` method using only existing Z3 functions ✅

### 1.5 Comprehensive Testing ✅
- 35 BitVec tests covering all functionality ✅
- Creation, extraction, edge cases, overflow handling ✅
- All 480 tests passing ✅

## Phase 1A: BitVec Value Type ✅ COMPLETED

### 1A.1 BitVec Readonly Struct ✅
Created `Z3Wrap/BitVec.cs` as `readonly struct` with:
- Value semantics and immutability ✅
- `BigInteger Value` and `uint Size` properties ✅
- All conversion methods (`ToInt()`, `ToBinaryString()`, etc.) ✅
- Meaningful binary operators (`+`, `-`, `*`, `/`, `&`, `|`, `^`, `<<`, `>>`) ✅
- Comparison operators with proper overflow handling ✅
- Comprehensive test coverage ✅

## Phase 1B: Z3BitVecExpr Operators ✅ COMPLETED

### 1B.1 Complete Operator Overloading ✅
Implemented all operators in `Z3BitVecExpr` with:
- **Arithmetic operators**: `+`, `-`, `*`, `/`, `%`, unary `-` (unsigned by default) ✅
- **Bitwise operators**: `&`, `|`, `^`, `~` ✅
- **Shift operators**: `<<` (logical left), `>>` (logical right) ✅
- **Comparison operators**: `<`, `<=`, `>`, `>=` (unsigned by default) ✅
- **Explicit signed methods**: `SignedLt()`, `SignedDiv()`, `ArithmeticShiftRight()` ✅

### 1B.2 Z3Context Extension Methods ✅
Added comprehensive BitVec operation methods:
- Arithmetic: `Add`, `Sub`, `Mul`, `UDiv`, `SDiv`, `URem`, `SRem`, `SMod`, `Neg` ✅
- Bitwise: `And`, `Or`, `Xor`, `Not` ✅
- Shift: `Shl`, `Lshr`, `Ashr` ✅
- Comparison: `Ult`, `Slt`, `Ule`, `Sle`, `Ugt`, `Sgt`, `Uge`, `Sge` ✅

### 1B.3 Comprehensive Testing ✅
Created `Z3BitVecExprOperatorTests.cs` with 9 tests covering:
- All arithmetic operations with solver verification ✅
- Bitwise operations with binary value testing ✅
- Shift operations (logical and arithmetic) ✅
- Unsigned vs signed comparison differences ✅
- Complex chained expressions ✅
- All 607 tests passing ✅

## Phase 2: Type System (Size Safety) 🔄 PLANNED

### 2.1 Size Type System
- Create `IConstantSize` interface for compile-time size validation
- Implement common sizes: `Size8`, `Size16`, `Size32`, `Size64`, `Size128`
- Support arbitrary sizes via `CustomSize<N>` phantom type
- Size validation and conversion utilities

### 2.2 Literal Support
- Support all Z3 literal formats:
  - **Binary**: `#b010` (3 bits)
  - **Hexadecimal**: `#x0a0` (12 bits)
  - **Decimal**: `(_ bv10 32)` (32 bits, value 10)
- Implicit conversions from appropriate .NET types based on size
- BitVector utility class for cross-size operations

## Phase 4: Advanced Features 🔄 PLANNED

### 4.1 Size Conversions
- Safe upcasting (zero/sign extension)
- Safe downcasting with overflow detection
- Cross-size arithmetic with automatic promotion
- Extraction of bit ranges

### 4.2 Integration Features
- Conversion to/from integers with proper semantics
- Array support: `Z3ArrayExpr<Z3BitVecExpr<TSize>, TValue>`
- Model extraction with proper size handling
- String representation (binary, hex, decimal)

### 4.3 Scoped Context Integration
- Implicit conversions in SetUp() scope
- Natural literal syntax: `bitVec + 0x42 == 0b1010`
- Type inference for common sizes

## Phase 5: Testing & Documentation 🔄 PLANNED

### 5.1 Comprehensive Test Suite
- Size safety validation tests
- All arithmetic operations with overflow cases
- Bitwise operations and bit manipulation
- Signed vs unsigned semantics
- Cross-size operations and conversions
- Integration with existing Z3Wrap features

### 5.2 Documentation Updates
- README.md with bitvector examples
- PLAN.md completion status
- API reference documentation
- Migration guide for different use cases

## Expected API Usage

```csharp
using var context = new Z3Context();
using var scope = context.SetUp();

// Type-safe bitvector creation
var x = context.BitVecConst<Size32>("x");
var y = context.BitVecConst<Size32>("y");

// Natural syntax with implicit conversions
using var solver = context.CreateSolver();
solver.Assert(x + 0x10 == 0b11010000);  // 32-bit arithmetic
solver.Assert((x & y) != 0);            // Bitwise operations
solver.Assert(x < y);                   // Unsigned comparison
solver.Assert(x.SignedLt(y));           // Signed comparison

// Size conversions
var x64 = x.ZeroExtend<Size64>();       // Safe upcast
var low16 = x.Extract<Size16>(15, 0);   // Bit extraction

// Overflow detection
solver.Assert(!x.AddOverflows(y));      // Addition won't overflow

// Model extraction
if (solver.Check() == Z3Status.Satisfiable) {
    var model = solver.GetModel();
    uint xValue = model.GetBitVecValue(x);
    Console.WriteLine($"x = 0x{xValue:X8}");
}
```

## Benefits
- **Type Safety**: Compile-time size validation prevents runtime errors
- **Natural Syntax**: Familiar bitwise and arithmetic operators
- **Size Flexibility**: Support from 1-bit flags to 1024+ bit integers
- **Overflow Safety**: Optional overflow detection for safer arithmetic
- **Integration**: Seamless integration with existing Z3Wrap patterns
- **Performance**: Direct mapping to Z3's efficient bitvector operations

## Implementation Status

- ✅ **Research Complete**: Comprehensive Z3 bitvector API analysis
- ✅ **Phase 1 COMPLETED**: Core infrastructure with 35 tests, all functionality working
- ✅ **Phase 1A COMPLETED**: BitVec readonly struct value type with full operator support
- ✅ **Phase 1B COMPLETED**: Z3BitVecExpr operators with comprehensive testing (607 tests passing)
- 🔄 **Phase 2+ PLANNED**: Advanced features and type system

## Recent Achievements

### September 2024
- **Completed full operator implementation** for Z3BitVecExpr with natural C# syntax
- **All operators working**: arithmetic (+, -, *, /, %), bitwise (&, |, ^, ~), shift (<<, >>), comparison (<, <=, >, >=)
- **Signed operation support** with explicit methods for signed semantics
- **Comprehensive test suite** with 9 new operator tests covering all functionality
- **607 tests passing** including complete BitVec and Z3BitVecExpr operator coverage

### December 2024
- **Completed comprehensive BitVec foundation** with all core functionality
- **35 BitVec tests** covering creation, extraction, edge cases, overflow handling
- **Clean architecture** with Z3NumericExpr base class and consolidated extraction
- **Removed non-existent functionality** (hex extraction) and implemented manual conversions
- **Enhanced error handling** with detailed Z3 library loading diagnostics
- **BitVec readonly struct** with complete value semantics and operator overloading