# Floating-Point Implementation Plan

## Overview

Implement IEEE 754 floating-point arithmetic support for Z3Wrap, providing type-safe constraint solving over floating-point numbers with natural C# integration.

## Current Status

✅ **IMPLEMENTATION COMPLETE**

- ✅ **Native API**: ~45+ Z3 floating-point functions available in `NativeZ3Library.FloatingPointArithmetic.generated.cs`
- ✅ **Public Wrappers**: All native functions wrapped with error checking in `Z3Library.FloatingPointArithmetic.generated.cs`
- ✅ **High-level API**: Full expression types and user-facing API implemented
- ✅ **Type System**: Float16, Float32, Float64 with generic `FpExpr<TFormat>`
- ✅ **Operations**: Arithmetic, comparisons, predicates, conversions all implemented
- ✅ **Test Coverage**: 80 tests, 96.1% line coverage, all passing
- ✅ **CI Pipeline**: Passing with ≥90% coverage requirement

## Z3 Floating-Point Theory

### What It Is

Floating-point is a **separate SMT theory** in Z3, not built on bit-vectors. It implements the full IEEE 754 standard with:

- **Sign-Exponent-Significand** representation
- **Special values**: NaN, Infinity, -0
- **Rounding modes**: RNE, RNA, RTZ, RTP, RTN
- **IEEE 754 semantics**: Proper handling of edge cases

### Sort Creation

```c
Z3_mk_fpa_sort(context, ebits, sbits)
```

- `ebits` = exponent bits (must be > 1)
- `sbits` = significand bits (must be > 2)
- Can create **custom formats** with arbitrary ebits/sbits
- Standard formats provided as convenience functions

### Standard IEEE 754 Formats

| Format | Total | Sign | Exponent (ebits) | Significand (sbits) | C# Type |
|--------|-------|------|------------------|---------------------|---------|
| Half | 16-bit | 1 | 5 | 11 | `System.Half` (.NET 5+) ✅ |
| Single | 32-bit | 1 | 8 | 24 | `float` ✅ |
| Double | 64-bit | 1 | 11 | 53 | `double` ✅ |
| Quadruple | 128-bit | 1 | 15 | 113 | ❌ No C# equivalent |

### Key Constraint

**Operations require same format**:
- `Z3_mk_fpa_add(rm, t1, t2)` - **t1 and t2 must have the same FloatingPoint sort**
- Cannot mix formats without explicit conversion
- Matches C# behavior and BitVector pattern

## Proposed API Design

### Type System

Follow the `BvExpr<TSize>` pattern:

```csharp
public class FpExpr<TFormat> : Z3Expr
    where TFormat : IFloatFormat
{
    // Expression class
}

public interface IFloatFormat
{
    static abstract uint ExponentBits { get; }
    static abstract uint SignificandBits { get; }
}

// Standard format markers
public struct Float16 : IFloatFormat
{
    public static uint ExponentBits => 5;
    public static uint SignificandBits => 11;
}

public struct Float32 : IFloatFormat
{
    public static uint ExponentBits => 8;
    public static uint SignificandBits => 24;
}

public struct Float64 : IFloatFormat
{
    public static uint ExponentBits => 11;
    public static uint SignificandBits => 53;
}

public struct Float128 : IFloatFormat
{
    public static uint ExponentBits => 15;
    public static uint SignificandBits => 113;
}
```

### Rounding Modes

```csharp
public class RoundingModeExpr : Z3Expr
{
    // Rounding mode expression
}

public enum RoundingMode
{
    NearestTiesToEven,  // Default IEEE 754 mode
    NearestTiesToAway,
    TowardPositive,
    TowardNegative,
    TowardZero
}

public static class RoundingModeContextExtensions
{
    public static RoundingModeExpr RoundingMode(this Z3Context context, RoundingMode mode);
}
```

### Value Creation

```csharp
public static class FpContextExtensions
{
    // Direct from C# types (overloads)
    public static FpExpr<Float16> Fp(this Z3Context context, Half value);
    public static FpExpr<Float32> Fp(this Z3Context context, float value);
    public static FpExpr<Float64> Fp(this Z3Context context, double value);

    // Explicit format
    public static FpExpr<TFormat> Fp<TFormat>(this Z3Context context, double value)
        where TFormat : IFloatFormat;

    // Constants
    public static FpExpr<TFormat> FpConst<TFormat>(this Z3Context context, string name)
        where TFormat : IFloatFormat;

    // Special values
    public static FpExpr<TFormat> FpNaN<TFormat>(this Z3Context context)
        where TFormat : IFloatFormat;
    public static FpExpr<TFormat> FpInfinity<TFormat>(this Z3Context context, bool negative = false)
        where TFormat : IFloatFormat;
    public static FpExpr<TFormat> FpZero<TFormat>(this Z3Context context, bool negative = false)
        where TFormat : IFloatFormat;
}
```

### Arithmetic Operations

All arithmetic requires rounding mode:

```csharp
public static class FpArithmeticExprExtensions
{
    // Binary operations
    public static FpExpr<TFormat> Add<TFormat>(
        this FpExpr<TFormat> left,
        FpExpr<TFormat> right,
        RoundingModeExpr roundingMode
    ) where TFormat : IFloatFormat;

    public static FpExpr<TFormat> Sub<TFormat>(/*...*/);
    public static FpExpr<TFormat> Mul<TFormat>(/*...*/);
    public static FpExpr<TFormat> Div<TFormat>(/*...*/);

    // Unary operations
    public static FpExpr<TFormat> Neg<TFormat>(this FpExpr<TFormat> expr);
    public static FpExpr<TFormat> Abs<TFormat>(this FpExpr<TFormat> expr);
    public static FpExpr<TFormat> Sqrt<TFormat>(
        this FpExpr<TFormat> expr,
        RoundingModeExpr roundingMode
    );

    // Fused multiply-add
    public static FpExpr<TFormat> Fma<TFormat>(
        this FpExpr<TFormat> x,
        FpExpr<TFormat> y,
        FpExpr<TFormat> z,
        RoundingModeExpr roundingMode
    ); // x * y + z

    // Operators (require implicit rounding mode from context?)
    public static FpExpr<TFormat> operator +(FpExpr<TFormat> left, FpExpr<TFormat> right);
    public static FpExpr<TFormat> operator -(FpExpr<TFormat> left, FpExpr<TFormat> right);
    public static FpExpr<TFormat> operator *(FpExpr<TFormat> left, FpExpr<TFormat> right);
    public static FpExpr<TFormat> operator /(FpExpr<TFormat> left, FpExpr<TFormat> right);
    public static FpExpr<TFormat> operator -(FpExpr<TFormat> operand); // Negation
}
```

### Comparisons

```csharp
public static class FpComparisonExprExtensions
{
    public static BoolExpr operator <(FpExpr<TFormat> left, FpExpr<TFormat> right);
    public static BoolExpr operator <=(FpExpr<TFormat> left, FpExpr<TFormat> right);
    public static BoolExpr operator >(FpExpr<TFormat> left, FpExpr<TFormat> right);
    public static BoolExpr operator >=(FpExpr<TFormat> left, FpExpr<TFormat> right);
    public static BoolExpr operator ==(FpExpr<TFormat> left, FpExpr<TFormat> right);
    public static BoolExpr operator !=(FpExpr<TFormat> left, FpExpr<TFormat> right);
}
```

### Predicates

```csharp
public static class FpPredicateExprExtensions
{
    public static BoolExpr IsNaN<TFormat>(this FpExpr<TFormat> expr);
    public static BoolExpr IsInfinite<TFormat>(this FpExpr<TFormat> expr);
    public static BoolExpr IsZero<TFormat>(this FpExpr<TFormat> expr);
    public static BoolExpr IsNormal<TFormat>(this FpExpr<TFormat> expr);
    public static BoolExpr IsSubnormal<TFormat>(this FpExpr<TFormat> expr);
    public static BoolExpr IsNegative<TFormat>(this FpExpr<TFormat> expr);
    public static BoolExpr IsPositive<TFormat>(this FpExpr<TFormat> expr);
}
```

### Conversions Between Formats

```csharp
public static class FpConversionExprExtensions
{
    // Generic conversion
    public static FpExpr<TTarget> ToFormat<TSource, TTarget>(
        this FpExpr<TSource> expr,
        RoundingModeExpr roundingMode
    )
        where TSource : IFloatFormat
        where TTarget : IFloatFormat;

    // Convenience methods
    public static FpExpr<Float16> ToFloat16<TFormat>(
        this FpExpr<TFormat> expr,
        RoundingModeExpr roundingMode
    ) where TFormat : IFloatFormat;

    public static FpExpr<Float32> ToFloat32<TFormat>(/*...*/);
    public static FpExpr<Float64> ToFloat64<TFormat>(/*...*/);
    public static FpExpr<Float128> ToFloat128<TFormat>(/*...*/);

    // To/from other Z3 types
    public static IntExpr ToInt<TFormat>(
        this FpExpr<TFormat> expr,
        RoundingModeExpr roundingMode
    );

    public static RealExpr ToReal<TFormat>(this FpExpr<TFormat> expr);

    public static FpExpr<TFormat> FromInt<TFormat>(
        this Z3Context context,
        IntExpr intExpr,
        RoundingModeExpr roundingMode
    ) where TFormat : IFloatFormat;

    public static FpExpr<TFormat> FromReal<TFormat>(
        this Z3Context context,
        RealExpr realExpr,
        RoundingModeExpr roundingMode
    ) where TFormat : IFloatFormat;
}
```

### Model Value Extraction

```csharp
public static class Z3ModelFpExtensions
{
    public static Half GetHalfValue(this Z3Model model, FpExpr<Float16> expr);
    public static float GetFloatValue(this Z3Model model, FpExpr<Float32> expr);
    public static double GetDoubleValue(this Z3Model model, FpExpr<Float64> expr);

    // Generic (returns as string for unsupported formats)
    public static string GetFpStringValue<TFormat>(this Z3Model model, FpExpr<TFormat> expr)
        where TFormat : IFloatFormat;
}
```

**Implementation**: Extract components using:
- `Z3_fpa_get_numeral_sign()` - Get sign bit
- `Z3_fpa_get_numeral_exponent_int64()` - Get exponent
- `Z3_fpa_get_numeral_significand_uint64()` - Get significand
- Reconstruct using `BitConverter.Int32BitsToSingle()` / `Int64BitsToDouble()`

## Implementation Files Structure

```
Z3Wrap/
├── Expressions/
│   └── FloatingPoint/
│       ├── FpExpr.cs                              # Main expression class
│       ├── RoundingModeExpr.cs                    # Rounding mode expression
│       ├── IFloatFormat.cs                        # Format interface
│       ├── FloatFormats.cs                        # Float16, Float32, Float64, Float128
│       ├── FpContextExtensions.cs                 # Creation methods
│       ├── FpArithmeticExprExtensions.cs          # Arithmetic operations
│       ├── FpComparisonExprExtensions.cs          # Comparisons
│       ├── FpPredicateExprExtensions.cs           # IsNaN, IsInf, etc.
│       ├── FpConversionExprExtensions.cs          # Format conversions
│       └── RoundingModeContextExtensions.cs       # Rounding mode creation

Z3Wrap.Tests/
└── Expressions/
    └── FloatingPoint/
        ├── FpExprCreationTests.cs                 # Value creation
        ├── FpExprArithmeticTests.cs               # Arithmetic (+, -, *, /)
        ├── FpExprComparisonTests.cs               # Comparisons (<, >, ==)
        ├── FpExprPredicateTests.cs                # IsNaN, IsInf tests
        ├── FpExprConversionTests.cs               # Format conversions
        ├── FpExprRoundingTests.cs                 # Rounding mode effects
        ├── FpExprSpecialValuesTests.cs            # NaN, Inf, -0 handling
        └── FpExprModelExtractionTests.cs          # Getting values from model

docs/examples/
└── FloatingPoint.md                               # IEEE 754 examples
```

## Testing Strategy

### Test Coverage Required (~300 tests)

1. **Creation Tests** (~30 tests)
   - From C# Half, float, double
   - Special values (NaN, Inf, -0)
   - Constants

2. **Arithmetic Tests** (~80 tests)
   - Each operation (+, -, *, /) with different rounding modes
   - Edge cases (overflow, underflow)
   - Special value arithmetic (NaN + x = NaN, etc.)

3. **Comparison Tests** (~40 tests)
   - All comparison operators
   - NaN comparisons (NaN != NaN)
   - Infinity comparisons

4. **Predicate Tests** (~30 tests)
   - IsNaN, IsInf, IsZero, IsNormal, IsSubnormal
   - For each format

5. **Conversion Tests** (~60 tests)
   - Between formats (Float32 → Float64, etc.)
   - To/from Int, Real
   - Rounding mode effects
   - Precision loss

6. **Special Values Tests** (~30 tests)
   - NaN propagation
   - Infinity arithmetic
   - Signed zero behavior

7. **Rounding Mode Tests** (~40 tests)
   - Each rounding mode behavior
   - Edge cases near rounding boundaries

8. **Real-world Scenarios** (~30 tests)
   - Numerical stability problems
   - IEEE 754 edge cases
   - Hardware verification examples

## Design Decisions

### 1. Rounding Mode Handling

**Challenge**: Most operations require a rounding mode, but writing it every time is verbose.

**Options**:

**Option A: Explicit everywhere** (strict)
```csharp
var result = a.Add(b, context.RoundingMode(RoundingMode.NearestTiesToEven));
```

**Option B: Default rounding mode in context** (convenient)
```csharp
context.SetDefaultRoundingMode(RoundingMode.NearestTiesToEven);
var result = a + b; // Uses default
```

**Option C: Operators use default, methods are explicit** (hybrid)
```csharp
var result1 = a + b;                              // Uses context default
var result2 = a.Add(b, customRoundingMode);       // Explicit control
```

**Recommendation**: Start with Option A (explicit), add Option C later if needed.

### 2. Float128 Support

**Challenge**: C# has no native 128-bit IEEE float type.

**Options**:

**Option A: Skip it initially**
- Implement Float16, Float32, Float64 only
- Add Float128 later if needed

**Option B: String-based construction**
```csharp
context.FpFromString<Float128>("3.14159265358979323846264338327950288");
```

**Option C: Bit-vector construction**
```csharp
context.FpFromComponents<Float128>(sign, exponent, significand);
```

**Recommendation**: Option A - skip Float128 initially. Most users won't need it.

### 3. Implicit Conversions

**Challenge**: Should we allow `float` → `FpExpr<Float32>` implicitly?

**Current pattern** (BitVectors):
```csharp
// NO implicit conversion
BvExpr<Size32> bv = 42; // ❌ Does not compile
BvExpr<Size32> bv = context.Bv(42); // ✅ Explicit
```

**Recommendation**: Follow existing pattern - NO implicit conversions. Explicit is better.

## Use Cases

### 1. Numerical Algorithm Verification

Verify that a numerical algorithm doesn't have precision issues:

```csharp
using var context = new Z3Context();
using var scope = context.SetUp();
using var solver = context.CreateSolver();

var x = context.FpConst<Float32>("x");
var y = context.FpConst<Float32>("y");

// Catastrophic cancellation: (x + y) - x should equal y
// But with floats, it might not!
var rm = context.RoundingMode(RoundingMode.NearestTiesToEven);
var result = (x.Add(y, rm)).Sub(x, rm);

solver.Assert(result != y); // Find cases where it's not equal

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    Console.WriteLine($"Found problematic values:");
    Console.WriteLine($"x = {model.GetFloatValue(x)}");
    Console.WriteLine($"y = {model.GetFloatValue(y)}");
}
```

### 2. Hardware FPU Verification

Verify that custom FPU hardware matches IEEE 754:

```csharp
// Verify FPU multiply-add
var a = context.FpConst<Float32>("a");
var b = context.FpConst<Float32>("b");
var c = context.FpConst<Float32>("c");

var rm = context.RoundingMode(RoundingMode.NearestTiesToEven);

// IEEE 754 fused multiply-add
var ieee = a.Fma(b, c, rm);

// Separate operations (might differ due to rounding)
var separated = a.Mul(b, rm).Add(c, rm);

solver.Assert(ieee != separated);
// Find cases where FMA differs from separate ops
```

### 3. Compiler Optimization Validation

Check if compiler optimizations preserve floating-point semantics:

```csharp
var x = context.FpConst<Float64>("x");

// Original: (x * 2.0) / 2.0
// Optimized: x
// Are they equivalent?

var rm = context.RoundingMode(RoundingMode.NearestTiesToEven);
var original = (x.Mul(context.Fp(2.0), rm)).Div(context.Fp(2.0), rm);
var optimized = x;

solver.Assert(original != optimized);
// Find cases where optimization changes result
```

## Complexity Assessment

**Compared to Pseudo-Boolean**:
- Pseudo-Boolean: 1 file, 9 methods, 21 tests (Simple) ✅
- Floating-Point: ~10 files, ~60+ methods, ~300 tests (Complex) ⚠️

**Effort Estimate**:
- **Minimum Viable** (Float32/Float64 only, basic ops): ~2-3 days
- **Standard Implementation** (All 4 formats, full ops): ~5-7 days
- **Complete with examples/docs**: ~10 days

## Recommendations

### Option 1: Defer (Recommended for now)
- Focus on other simpler theories first
- Most users can use `RealExpr` for exact arithmetic
- Floating-point is niche use case (hardware verification, numerical analysis)

### Option 2: Minimal Viable Implementation
- Only Float32 and Float64 (skip Float16/Float128)
- Basic arithmetic and comparisons
- No conversions initially
- ~100 tests instead of 300

### Option 3: Full Implementation
- Complete IEEE 754 support
- All 4 formats
- Full test coverage
- Production-ready
- Significant time investment

## Next Steps

1. Review this plan and decide on approach
2. If proceeding, start with minimal viable (Option 2)
3. Create tests first (TDD approach)
4. Implement core types and basic operations
5. Expand based on user feedback

## Implementation Summary

**Completed**: November 25, 2025

### What Was Implemented

**Formats Supported**:
- ✅ Float16 (Half) - 16-bit IEEE 754 half-precision
- ✅ Float32 (Single) - 32-bit IEEE 754 single-precision
- ✅ Float64 (Double) - 64-bit IEEE 754 double-precision
- ❌ Float128 (Quadruple) - Skipped (no C# native type)

**Core Features**:
- Generic type system: `FpExpr<TFormat>` with `IFloatFormat` interface
- Rounding modes: All 5 IEEE 754 modes (RNE, RNA, RTZ, RTP, RTN)
- Arithmetic: +, -, *, /, %, Neg, Abs, Sqrt, RoundToIntegral, Fma, Min, Max
- Comparisons: <, <=, >, >=, ==, !=
- Predicates: IsNaN, IsInfinite, IsZero, IsNormal, IsSubnormal, IsNegative, IsPositive
- Conversions: Between formats, to/from Real
- Special values: NaN, ±Infinity, ±Zero
- Model extraction: GetHalfValue, GetFloatValue, GetDoubleValue

**Files Created** (12 implementation + 4 test files):
```
Z3Wrap/Expressions/FloatingPoint/
├── IFloatFormat.cs                    # Format interface
├── FloatFormats.cs                    # Float16, Float32, Float64
├── RoundingMode.cs                    # Enum for modes
├── RoundingModeExpr.cs                # Rounding mode expression
├── FpExpr.cs                          # Main expression class with operators
├── FpContextExtensions.cs             # Value creation methods
├── RoundingModeContextExtensions.cs   # Rounding mode creation
├── FpArithmeticExprExtensions.cs      # Arithmetic operations
├── FpComparisonExprExtensions.cs      # (Empty - operators in FpExpr.cs)
├── FpPredicateExprExtensions.cs       # IsNaN, IsInf, etc.
├── FpConversionExprExtensions.cs      # Format conversions
└── Z3Model.FloatingPoint.cs           # Model value extraction

Z3Wrap.Tests/Expressions/FloatingPoint/
├── FpExprCreationTests.cs             # 30+ tests
├── FpExprArithmeticTests.cs           # 26 tests
├── FpExprComparisonTests.cs           # 29 tests
├── FpExprPredicateTests.cs            # 15 tests
└── FpExprConversionTests.cs           # 10 tests
```

**Test Results**:
- Total tests: 1311 (80 new FP tests)
- Pass rate: 100%
- Line coverage: 96.1%
- Branch coverage: 90.5%
- Method coverage: 98.5%

**Key Design Decisions Made**:

1. **Rounding Mode**: Operators (+, -, *, /) use default NearestTiesToEven; methods allow explicit control
2. **Float128**: Skipped due to no C# native type (can be added later if needed)
3. **Operators**: Defined in FpExpr<TFormat> class itself (C# limitation with generic constraints)
4. **Model Extraction**: Uses string parsing of IEEE bit format from Z3
5. **Half Creation**: Uses MkFpaNumeralFloat (converted from Half to float) instead of MkFpaNumeralInt
6. **Z3 Handle Reuse**: Z3 returns same handle for identical rounding mode constants (verified with tests)

**Known Behaviors**:
- IEEE 754 remainder differs from C# % operator (rounds to nearest, not truncates)
- Rounding mode expressions are cached by Z3 (same mode returns same handle)
- Model extraction parses Z3 format: `(fp #bS #bEEE... #bSSS...)` and `(_ +zero ebits sbits)`

**Lessons Learned**:
- Always verify Z3 API behavior with tests rather than assumptions
- String parsing is simpler than bit manipulation for model extraction
- Z3's floating-point representation uses IEEE bit format directly
- MkFpaNumeralFloat/Double are preferred over MkFpaNumeralInt for value creation

### Component-Based API

**Implemented**: `GetFpComponents<TFormat>()` for extracting raw IEEE 754 components

```csharp
var model = solver.GetModel();
var (sign, exponent, significand) = model.GetFpComponents(fpExpr);
```

This allows users working with custom precision formats to extract the raw bit-level representation as:
- `Sign`: bool (true = negative, false = positive)
- `Exponent`: ulong (biased exponent value)
- `Significand`: ulong (without implicit leading bit)

**Not Implemented**: `FpFromComponents<TFormat>()` for creating FpExpr from raw components

Attempted to implement using `Z3_mk_fpa_fp` (constructs FP from bit-vector components), but encountered issues with Z3 API:
- `MkNumeral` with BV sort produces AST that Z3_mk_fpa_fp doesn't recognize as bit-vector
- Error: "sort mismatch, expected three bit-vectors, the first one of size 1"
- Multiple approaches tried: MkBvNumeral, MkUnsignedInt64, string formats (#b, decimal)
- All failed with same error despite following existing BV patterns in codebase

**Future Work**: Investigate proper Z3 API usage for bit-vector construction or consider alternative approaches:
- Use Z3_mk_numeral with binary string for bit-vectors (may need different format)
- Look at Z3 Python bindings implementation
- Consider if this feature is actually needed (users can use standard constructors for common cases)

## Future Enhancements

### 1. Fp<TFormat> Value Type (Similar to Real and Bv<TSize>)

**Intention**: Create a C#-native value type for floating-point values that can be used independently of Z3 contexts.

**Proposed Design**:
```csharp
public readonly struct Fp<TFormat> : IComparable<Fp<TFormat>>
    where TFormat : IFloatFormat
{
    // Storage: IEEE 754 components
    private readonly bool sign;
    private readonly ulong exponent;
    private readonly ulong significand;

    // Factory methods for special values
    public static Fp<TFormat> Zero { get; }
    public static Fp<TFormat> NegativeZero { get; }
    public static Fp<TFormat> PositiveInfinity { get; }
    public static Fp<TFormat> NegativeInfinity { get; }
    public static Fp<TFormat> NaN { get; }

    // Conversions to/from C# types (for standard formats only)
    public static explicit operator Fp<Float16>(Half value);
    public static explicit operator Fp<Float32>(float value);
    public static explicit operator Fp<Float64>(double value);

    // Arithmetic operators (delegate to C# types for standard formats)
    public static Fp<TFormat> operator +(Fp<TFormat> left, Fp<TFormat> right);
    // ... other operators

    // ToString with format support
    public string ToString(string? format = null);
}

// Model extraction
public static Fp<TFormat> GetFpValue<TFormat>(this Z3Model model, FpExpr<TFormat> expr)
    where TFormat : IFloatFormat;

// Z3 expression creation from value
public static FpExpr<TFormat> Fp<TFormat>(this Z3Context context, Fp<TFormat> value)
    where TFormat : IFloatFormat;
```

**Benefits**:
- Consistent with existing `Real` and `Bv<TSize>` value types
- Allows working with FP values outside of Z3 context
- Type-safe representation of IEEE 754 values
- Enables custom precision formats not supported by C# natively

**Challenges**:
- Arithmetic operations tricky for custom formats (no native C# implementation)
- May need to delegate to Z3 for custom format operations
- Or implement IEEE 754 arithmetic in C# (significant complexity)

**Decision**: Defer until there's clear user demand for custom precision formats beyond Float16/32/64.

### 2. Additional Conversions

Currently supports:
- FpExpr to/from RealExpr

Future additions:
- FpExpr to/from IntExpr (with rounding modes)
- FpExpr to/from BvExpr (bit-level representation)
- Support for unsigned int conversions

### 3. Additional Operations

Could add:
- Remainder (fmod)
- Power functions
- Trigonometric functions (if Z3 supports them)
- Logarithmic functions

## References

- IEEE 754-2008 Standard for Floating-Point Arithmetic
- Z3 API Documentation: https://z3prover.github.io/api/html/group__capi.html
- SMT-LIB Floating-Point Theory
- Existing implementation: BvExpr<TSize> (similar pattern)
