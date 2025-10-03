# Z3 Algebraic Numbers API Comparison Report

## Overview
**NativeLibrary.AlgebraicNumbers.cs**: 2 functions
**Z3 C API (z3_api.h + z3_algebraic.h)**: ~22 functions

## Complete Function Mapping

### ✅ Functions in Both (2/22 in NativeLibrary match Z3 API - 9.1%)

| Our Method | Z3 C API | Parameters | Purpose |
|------------|----------|------------|---------|
| `GetAlgebraicNumberLower` | `Z3_get_algebraic_number_lower` | `(ctx, algebraic, precision)` | Returns lower bound rational approximation of algebraic number |
| `GetAlgebraicNumberUpper` | `Z3_get_algebraic_number_upper` | `(ctx, algebraic, precision)` | Returns upper bound rational approximation of algebraic number |

**Note**: Both functions are from z3_api.h and return Z3_ast (in our implementation, we incorrectly use `int` return type - should be `IntPtr`).

### ❌ Functions in Z3 but NOT in NativeLibrary (20 functions from z3_algebraic.h)

#### Algebraic Predicates (4 functions)
```c
bool Z3_API Z3_algebraic_is_value(Z3_context c, Z3_ast a);
bool Z3_API Z3_algebraic_is_pos(Z3_context c, Z3_ast a);
bool Z3_API Z3_algebraic_is_neg(Z3_context c, Z3_ast a);
bool Z3_API Z3_algebraic_is_zero(Z3_context c, Z3_ast a);
```

#### Sign Operations (1 function)
```c
int Z3_API Z3_algebraic_sign(Z3_context c, Z3_ast a);  // Returns -1, 0, or 1
```

#### Arithmetic Operations (6 functions)
```c
Z3_ast Z3_API Z3_algebraic_add(Z3_context c, Z3_ast a, Z3_ast b);
Z3_ast Z3_API Z3_algebraic_sub(Z3_context c, Z3_ast a, Z3_ast b);
Z3_ast Z3_API Z3_algebraic_mul(Z3_context c, Z3_ast a, Z3_ast b);
Z3_ast Z3_API Z3_algebraic_div(Z3_context c, Z3_ast a, Z3_ast b);
Z3_ast Z3_API Z3_algebraic_root(Z3_context c, Z3_ast a, unsigned k);      // a^(1/k)
Z3_ast Z3_API Z3_algebraic_power(Z3_context c, Z3_ast a, unsigned k);     // a^k
```

#### Comparison Operations (6 functions)
```c
bool Z3_API Z3_algebraic_lt(Z3_context c, Z3_ast a, Z3_ast b);
bool Z3_API Z3_algebraic_gt(Z3_context c, Z3_ast a, Z3_ast b);
bool Z3_API Z3_algebraic_le(Z3_context c, Z3_ast a, Z3_ast b);
bool Z3_API Z3_algebraic_ge(Z3_context c, Z3_ast a, Z3_ast b);
bool Z3_API Z3_algebraic_eq(Z3_context c, Z3_ast a, Z3_ast b);
bool Z3_API Z3_algebraic_neq(Z3_context c, Z3_ast a, Z3_ast b);
```

#### Polynomial Operations (3 functions)
```c
Z3_ast_vector Z3_API Z3_algebraic_roots(Z3_context c, Z3_ast p, unsigned n, Z3_ast a[]);
int Z3_API Z3_algebraic_eval(Z3_context c, Z3_ast p, unsigned n, Z3_ast a[]);
Z3_ast_vector Z3_API Z3_algebraic_get_poly(Z3_context c, Z3_ast a);
```

### ⚠️ Functions in NativeLibrary but NOT in Z3

**None** - Both functions exist in Z3 C API.

## API Coverage Summary

| Metric | Count | Percentage |
|--------|-------|------------|
| Z3 C API Functions (z3_api.h + z3_algebraic.h) | ~22 | 100% |
| Functions in NativeLibrary | 2 | **9.1%** |
| Missing Functions | 20 | 90.9% |

## Function Categories

### Bound Approximation (2 functions - ✅ IMPLEMENTED)
- **Z3_get_algebraic_number_lower** - Get lower bound rational approximation
- **Z3_get_algebraic_number_upper** - Get upper bound rational approximation

### Predicates (4 functions - ❌ NOT IMPLEMENTED)
- **Z3_algebraic_is_value** - Check if AST is valid algebraic number
- **Z3_algebraic_is_pos/is_neg/is_zero** - Sign checking

### Arithmetic (7 functions - ❌ NOT IMPLEMENTED)
- **Basic operations**: add, sub, mul, div
- **Power operations**: root (a^(1/k)), power (a^k)
- **Sign**: algebraic_sign (-1, 0, 1)

### Comparison (6 functions - ❌ NOT IMPLEMENTED)
- **Ordering**: lt, gt, le, ge
- **Equality**: eq, neq

### Polynomial Operations (3 functions - ❌ NOT IMPLEMENTED)
- **Z3_algebraic_roots** - Find polynomial roots
- **Z3_algebraic_eval** - Evaluate polynomial sign at algebraic values
- **Z3_algebraic_get_poly** - Extract polynomial coefficients

## Implementation Notes

### Current Implementation Issues

1. **Incorrect Return Types**: The current implementation returns `int` (success/failure) instead of `IntPtr` (Z3_ast):
   ```csharp
   // Current (INCORRECT)
   internal int GetAlgebraicNumberLower(IntPtr ctx, IntPtr algebraic, uint precision)

   // Should be
   internal IntPtr GetAlgebraicNumberLower(IntPtr ctx, IntPtr algebraic, uint precision)
   ```

2. **Missing Extended API**: The 20 functions from z3_algebraic.h are completely missing.

3. **File Organization**: Functions from two different headers (z3_api.h and z3_algebraic.h) should ideally be in this file since both relate to algebraic numbers.

### Usage Context

Algebraic numbers in Z3 represent exact values like √2, ∛5, or roots of polynomials. The bound functions provide rational approximations for these irrational values, which is useful for:
- Numerical computation with controlled precision
- Displaying approximate decimal values to users
- Integration with numerical libraries

The missing extended API from z3_algebraic.h provides direct algebraic arithmetic without converting to/from SMT expressions, which can be more efficient for certain algebraic number computations.

## Completeness Assessment

❌ **INCOMPLETE** - Only 9.1% of Z3's algebraic number API is implemented.

### Critical Issues
1. **Wrong Return Types** - Both bound functions return `int` instead of `IntPtr`
2. **Missing Extended API** - 20 functions from z3_algebraic.h not bound
3. **Limited Functionality** - Only approximation bounds available, no algebraic arithmetic

### Recommendations

#### Priority 1: Fix Existing Functions
```csharp
// Fix return types from int to IntPtr
private delegate IntPtr GetAlgebraicNumberLowerDelegate(IntPtr ctx, IntPtr algebraic, uint precision);
private delegate IntPtr GetAlgebraicNumberUpperDelegate(IntPtr ctx, IntPtr algebraic, uint precision);

internal IntPtr GetAlgebraicNumberLower(IntPtr ctx, IntPtr algebraic, uint precision)
{
    var funcPtr = GetFunctionPointer("Z3_get_algebraic_number_lower");
    var func = Marshal.GetDelegateForFunctionPointer<GetAlgebraicNumberLowerDelegate>(funcPtr);
    return func(ctx, algebraic, precision);
}

internal IntPtr GetAlgebraicNumberUpper(IntPtr ctx, IntPtr algebraic, uint precision)
{
    var funcPtr = GetFunctionPointer("Z3_get_algebraic_number_upper");
    var func = Marshal.GetDelegateForFunctionPointer<GetAlgebraicNumberUpperDelegate>(funcPtr);
    return func(ctx, algebraic, precision);
}
```

#### Priority 2: Add Extended Algebraic API (if needed)

If algebraic number operations are required, consider adding the z3_algebraic.h functions. However, these are specialized and may not be needed for typical SMT solving use cases. The z3_algebraic.h API is more for:
- Pure algebraic number computation
- Polynomial root finding
- Efficient algebraic arithmetic outside SMT context

Most users will work with algebraic numbers through normal Z3 Real expressions rather than the specialized algebraic API.

#### Priority 3: Documentation

Add examples showing when to use:
- **Bound functions** (z3_api.h): Get decimal approximations of algebraic numbers from models
- **Algebraic API** (z3_algebraic.h): Direct algebraic computation (if implemented)

## Z3 C API Reference

### Functions from z3_api.h (2 functions)

```c
/**
   \brief Return a lower bound for the given real algebraic number.
   The interval isolating the number is smaller than 1/10^precision.
   The result is a numeral AST of sort Real.

   \pre Z3_is_algebraic_number(c, a)
*/
Z3_ast Z3_API Z3_get_algebraic_number_lower(Z3_context c, Z3_ast a, unsigned precision);

/**
   \brief Return an upper bound for the given real algebraic number.
   The interval isolating the number is smaller than 1/10^precision.
   The result is a numeral AST of sort Real.

   \pre Z3_is_algebraic_number(c, a)
*/
Z3_ast Z3_API Z3_get_algebraic_number_upper(Z3_context c, Z3_ast a, unsigned precision);
```

### Functions from z3_algebraic.h (20 functions - NOT BOUND)

See "Missing Functions" section above for complete list.

## Verification

- **Source**:
  - Z3 C API header (z3_api.h) - bound approximation functions
  - Z3 Algebraic API header (z3_algebraic.h) - extended algebraic operations
- **URLs**:
  - https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h
  - https://github.com/Z3Prover/z3/blob/master/src/api/z3_algebraic.h
  - https://z3prover.github.io/api/html/group__capi.html
- **Our implementation**: Z3Wrap/Core/Interop/NativeLibrary.AlgebraicNumbers.cs
- **Lines of code**: 62 lines (minimal - only 2 function bindings)

## Action Items

1. **Fix Critical Bug**: Change return type from `int` to `IntPtr` for both functions
2. **Verify Usage**: Check if incorrect return type is causing issues in high-level API
3. **Evaluate Need**: Determine if z3_algebraic.h functions are needed for project goals
4. **Document Scope**: Clarify that only bound approximation is supported, not full algebraic API

---

**Report Generated**: 2025-10-03
**Audit Status**: ✅ Complete (audit done, implementation incomplete)
**Completeness**: 2/22 functions (9.1%)
**Critical Issue**: ❌ Incorrect return types - needs immediate fix
