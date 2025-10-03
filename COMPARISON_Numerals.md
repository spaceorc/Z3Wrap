# Z3 Numerals API Comparison Report

## Overview
**NativeLibrary.Numerals.cs**: 9 functions
**Z3 C API (z3_api.h)**: ~11 functions (numeral extraction category)

## Complete Function Mapping

### ✅ Functions in Both (9/11 in NativeLibrary match Z3 API - 81.8%)

| Our Method | Z3 C API | Parameters | Purpose |
|------------|----------|------------|---------|
| `GetNumeralBinaryString` | `Z3_get_numeral_binary_string` | `(ctx, ast)` | Returns binary string representation (e.g., "1010" for 10) |
| `GetNumeralDecimalString` | `Z3_get_numeral_decimal_string` | `(ctx, ast, precision)` | Returns decimal string with specified precision |
| `GetNumeralInt` | `Z3_get_numeral_int` | `(ctx, ast, out value)` | Extracts 32-bit signed integer value |
| `GetNumeralUint` | `Z3_get_numeral_uint` | `(ctx, ast, out value)` | Extracts 32-bit unsigned integer value |
| `GetNumeralInt64` | `Z3_get_numeral_int64` | `(ctx, ast, out value)` | Extracts 64-bit signed integer value |
| `GetNumeralUint64` | `Z3_get_numeral_uint64` | `(ctx, ast, out value)` | Extracts 64-bit unsigned integer value |
| `GetNumeralRationalInt64` | `Z3_get_numeral_rational_int64` | `(ctx, ast, out num, out den)` | Extracts rational as 64-bit numerator/denominator |
| `GetNumeralSmall` | `Z3_get_numeral_small` | `(ctx, ast, out num, out den)` | Tests if numeral fits in small (64-bit) representation |
| `GetNumeralDouble` | `Z3_get_numeral_double` | `(ctx, ast, out value)` | Extracts double-precision floating-point approximation |

### ❌ Functions in Z3 but NOT in NativeLibrary

**2 functions missing** (intentionally split to other files for better organization):

1. **Z3_mk_numeral** - `Z3_ast Z3_mk_numeral(Z3_context c, Z3_string numeral, Z3_sort ty)`
   - **Reason**: Located in `NativeLibrary.Expressions.cs` (expression creation)
   - **Status**: ✅ Implemented elsewhere

2. **Z3_get_numeral_string** - `Z3_string Z3_get_numeral_string(Z3_context c, Z3_ast a)`
   - **Reason**: Likely missing, should be added for generic string representation
   - **Status**: ⚠️ **MISSING** - Generic string conversion (complement to binary/decimal)

**Additional related functions in other files:**

3. **Z3_get_numerator** - `Z3_ast Z3_get_numerator(Z3_context c, Z3_ast a)`
   - **Location**: `NativeLibrary.Queries.cs` (AST introspection)
   - **Status**: ✅ Implemented elsewhere

4. **Z3_get_denominator** - `Z3_ast Z3_get_denominator(Z3_context c, Z3_ast a)`
   - **Location**: `NativeLibrary.Queries.cs` (AST introspection)
   - **Status**: ✅ Implemented elsewhere

### ⚠️ Functions in NativeLibrary but NOT in Z3

**None** - All 9 functions match Z3 C API exactly.

## API Coverage Summary

| Metric | Count | Percentage |
|--------|-------|------------|
| Z3 Numeral Extraction Functions | 11 | 100% |
| Functions in NativeLibrary.Numerals.cs | 9 | 81.8% |
| Functions in other NativeLibrary files | 2 | 18.2% |
| Missing Functions | 1 | 9.1% |
| Extra Functions | 0 | 0% |

## Function Categories

### String Conversion (2 functions in NativeLibrary, 3 in Z3)
- `Z3_get_numeral_binary_string` - Binary representation (e.g., "1010")
- `Z3_get_numeral_decimal_string` - Decimal with precision (e.g., "3.14159")
- ⚠️ `Z3_get_numeral_string` - **MISSING** - Generic string (rational or decimal)

### Integer Extraction - 32-bit (2 functions)
- `Z3_get_numeral_int` - Signed 32-bit integer
- `Z3_get_numeral_uint` - Unsigned 32-bit integer

### Integer Extraction - 64-bit (2 functions)
- `Z3_get_numeral_int64` - Signed 64-bit integer
- `Z3_get_numeral_uint64` - Unsigned 64-bit integer

### Rational Extraction (2 functions)
- `Z3_get_numeral_rational_int64` - Extract rational as int64 num/den pair
- `Z3_get_numeral_small` - Test if numeral fits in small representation

### Floating-Point Approximation (1 function)
- `Z3_get_numeral_double` - Double-precision approximation

### Related Functions in Other Files (3 functions)
- `Z3_mk_numeral` - Create numeral from string (in `Expressions.cs`)
- `Z3_get_numerator` - Get numerator as AST (in `Queries.cs`)
- `Z3_get_denominator` - Get denominator as AST (in `Queries.cs`)

## Completeness Assessment

⚠️ **MOSTLY COMPLETE (90.9%)** - NativeLibrary.Numerals.cs covers 9/10 core numeral extraction functions.

### Strengths
- Comprehensive coverage of integer extraction (32-bit and 64-bit, signed and unsigned)
- Proper rational number extraction with int64 support
- Binary and decimal string conversion with precision control
- Double-precision approximation for numeric analysis
- Excellent XML documentation for each function
- Proper delegate signatures with out parameters for extraction functions

### Missing Functions

1. **Z3_get_numeral_string** - Generic string representation
   - **Signature**: `Z3_string Z3_API Z3_get_numeral_string(Z3_context c, Z3_ast a)`
   - **Purpose**: Returns numeral as string (rationals as "num/den", decimals as-is)
   - **Impact**: Medium - Complements binary and decimal string functions
   - **Recommendation**: Add for completeness and convenience

### Architecture Notes

The Z3Wrap library intentionally splits numeral-related functions across multiple files:

1. **NativeLibrary.Numerals.cs** (this file)
   - Focus: Numeral **extraction** and **conversion**
   - Functions: Getting values out of existing numeral AST nodes

2. **NativeLibrary.Expressions.cs**
   - Focus: Expression **creation**
   - Functions: `Z3_mk_numeral` - creating new numeral AST nodes

3. **NativeLibrary.Queries.cs**
   - Focus: AST **introspection** and **decomposition**
   - Functions: `Z3_get_numerator`, `Z3_get_denominator` - returning AST nodes

This separation is logical and maintains clear boundaries between:
- Creation (Expressions)
- Inspection (Queries)
- Value extraction (Numerals)

## Verification

- **Source**: Z3 C API header `z3_api.h` from [Z3 GitHub repository](https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h)
- **Our implementation**: `Z3Wrap/Core/Interop/NativeLibrary.Numerals.cs`
- **Related files**:
  - `Z3Wrap/Core/Interop/NativeLibrary.Expressions.cs` (Z3_mk_numeral)
  - `Z3Wrap/Core/Interop/NativeLibrary.Queries.cs` (Z3_get_numerator/denominator)
- **Verification date**: 2025-10-03
- **Z3 version compatibility**: All versions (stable core API)

## Recommendations

### Priority 1: Add Missing Function

Add `Z3_get_numeral_string` to complete the string conversion API:

```csharp
// In LoadFunctionsNumerals method:
LoadFunctionOrNull(handle, functionPointers, "Z3_get_numeral_string");

// Delegate:
private delegate IntPtr GetNumeralStringDelegate(IntPtr ctx, IntPtr ast);

// Method:
/// <summary>
/// Gets string representation of numeral.
/// </summary>
/// <param name="ctx">The Z3 context handle.</param>
/// <param name="ast">The numeral AST.</param>
/// <returns>String representation (rationals as "num/den", integers as decimal).</returns>
/// <remarks>
/// Returns generic string representation. For rationals, format is "numerator/denominator".
/// For integers, returns decimal string. For best control, use GetNumeralBinaryString
/// or GetNumeralDecimalString instead.
/// </remarks>
/// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
internal IntPtr GetNumeralString(IntPtr ctx, IntPtr ast)
{
    var funcPtr = GetFunctionPointer("Z3_get_numeral_string");
    var func = Marshal.GetDelegateForFunctionPointer<GetNumeralStringDelegate>(funcPtr);
    return func(ctx, ast);
}
```

### Priority 2: Documentation Enhancement

Consider adding cross-references in XML documentation:
- Note that `Z3_mk_numeral` is in `NativeLibrary.Expressions.cs`
- Note that `Z3_get_numerator/denominator` are in `NativeLibrary.Queries.cs`
- Explain when to use each extraction method (string vs int vs double)

### Priority 3: Usage Guidance

Document the extraction strategy for users:
1. Try `GetNumeralInt64` or `GetNumeralUint64` first (fastest, no allocation)
2. For rationals, try `GetNumeralRationalInt64` (exact, fits in 64-bit)
3. For large values, use `GetNumeralString` (arbitrary precision)
4. For approximations, use `GetNumeralDouble` (floating-point only)

## Special Notes

### Z3_get_numeral_string Behavior

According to [Z3 Issue #7523](https://github.com/Z3Prover/z3/issues/7523):
- For floating-point numerals, may produce s-expression format for negative/non-whole values
- For rationals: returns "numerator/denominator" format
- For integers: returns decimal string
- Different from `GetNumeralDecimalString` which takes precision parameter

### Return Value Patterns

Z3 numeral extraction functions follow two patterns:

1. **IntPtr return** (string functions)
   - Returns Z3_string (IntPtr to null-terminated string)
   - Must be marshaled to C# string
   - No failure indication (always returns string)

2. **bool return with out parameter** (numeric extraction)
   - Returns `true` if extraction succeeded
   - Returns `false` if value doesn't fit in target type
   - Allows graceful handling of overflow

## Conclusion

NativeLibrary.Numerals.cs provides **90.9% coverage** of Z3's numeral extraction API. The missing `Z3_get_numeral_string` function should be added for completeness, though the existing binary and decimal string functions provide good coverage. The architectural split across Numerals, Expressions, and Queries files is logical and maintains clear separation of concerns.

**Status**: ⚠️ Mostly Complete - Add `Z3_get_numeral_string` for 100% coverage.
