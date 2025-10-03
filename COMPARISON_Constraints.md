# Z3 Constraints API Comparison Report

## Overview
**NativeLibrary.Constraints.cs**: 5 functions
**Z3 C API (z3_api.h)**: 5 functions
**Completeness**: 100% ✅

## Complete Function Mapping

### ✅ Functions in Both (5/5 in NativeLibrary match Z3 API)

| Our Method | Z3 C API | Z3 C++ Method | Purpose |
|------------|----------|---------------|---------|
| `MkAtLeast` | `Z3_mk_atleast` | `z3::atleast` | Creates cardinality constraint: at least k of args are true |
| `MkAtMost` | `Z3_mk_atmost` | `z3::atmost` | Creates cardinality constraint: at most k of args are true |
| `MkPbEq` | `Z3_mk_pbeq` | `z3::pbeq` | Creates pseudo-Boolean equality constraint: weighted sum = k |
| `MkPbGe` | `Z3_mk_pbge` | `z3::pbge` | Creates pseudo-Boolean greater-or-equal constraint: weighted sum >= k |
| `MkPbLe` | `Z3_mk_pble` | `z3::pble` | Creates pseudo-Boolean less-or-equal constraint: weighted sum <= k |

### ❌ Functions in Z3 but NOT in NativeLibrary
None - all Z3 constraint functions are present.

### ⚠️ Functions in NativeLibrary but NOT in Z3
None - all functions match Z3 C API.

## API Coverage Summary
| Metric | Count | Percentage |
|--------|-------|------------|
| Z3 C API Functions | 5 | 100% |
| Functions in NativeLibrary | 5 | 100% |
| Missing Functions | 0 | 0% |

## Function Categories

### Cardinality Constraints (2 functions)
Simple counting constraints over Boolean variables:
- **MkAtLeast**: At least k of the Boolean variables must be true
- **MkAtMost**: At most k of the Boolean variables can be true

**Use Case**: Express "K-out-of-N" constraints, e.g., "exactly 2 out of 5 sensors must be active"

### Pseudo-Boolean Constraints (3 functions)
Weighted linear constraints over Boolean variables (generalize cardinality):
- **MkPbEq**: `coeffs[0]*args[0] + ... + coeffs[n-1]*args[n-1] = k`
- **MkPbGe**: `coeffs[0]*args[0] + ... + coeffs[n-1]*args[n-1] >= k`
- **MkPbLe**: `coeffs[0]*args[0] + ... + coeffs[n-1]*args[n-1] <= k`

**Use Case**: Express weighted constraints, e.g., "total cost of selected items must be at most budget"

## Function Signatures

### Z3 C API
```c
Z3_ast Z3_API Z3_mk_atleast(Z3_context c, unsigned num_args, Z3_ast const args[], unsigned k);
Z3_ast Z3_API Z3_mk_atmost(Z3_context c, unsigned num_args, Z3_ast const args[], unsigned k);
Z3_ast Z3_API Z3_mk_pbeq(Z3_context c, unsigned num_args, Z3_ast const args[], int const coeffs[], int k);
Z3_ast Z3_API Z3_mk_pbge(Z3_context c, unsigned num_args, Z3_ast const args[], int const coeffs[], int k);
Z3_ast Z3_API Z3_mk_pble(Z3_context c, unsigned num_args, Z3_ast const args[], int const coeffs[], int k);
```

### Our P/Invoke Bindings
```csharp
internal IntPtr MkAtLeast(IntPtr ctx, uint numArgs, IntPtr[] args, uint k);
internal IntPtr MkAtMost(IntPtr ctx, uint numArgs, IntPtr[] args, uint k);
internal IntPtr MkPbEq(IntPtr ctx, uint numArgs, IntPtr[] args, int[] coeffs, int k);
internal IntPtr MkPbGe(IntPtr ctx, uint numArgs, IntPtr[] args, int[] coeffs, int k);
internal IntPtr MkPbLe(IntPtr ctx, uint numArgs, IntPtr[] args, int[] coeffs, int k);
```

## SMT-LIB Syntax
These constraints are represented in SMT-LIB format as:
- `((_ at-least k) x y z ...)` - At least k of the arguments are true
- `((_ at-most k) x y z ...)` - At most k of the arguments are true
- `((_ pbge c1 c2 c3 k) x y z)` - Weighted sum >= k
- `((_ pble c1 c2 c3 k) x y z)` - Weighted sum <= k
- `((_ pbeq c1 c2 c3 k) x y z)` - Weighted sum = k

## Completeness Assessment

✅ **COMPLETE** - 100% API Coverage

All 5 constraint functions from Z3 C API are present in NativeLibrary.Constraints.cs:
- All cardinality constraint functions (2/2)
- All pseudo-Boolean constraint functions (3/3)
- Correct delegate signatures matching Z3 C API
- Comprehensive XML documentation

## Notes

### Design Observations
1. **Small, Focused API**: This is one of the smallest Z3 API sections with only 5 functions
2. **Clear Separation**: Cardinality (unweighted) vs pseudo-Boolean (weighted) constraints
3. **Symmetry**: Three comparison operators (=, >=, <=) for pseudo-Boolean constraints
4. **Documentation Quality**: All functions have detailed XML comments explaining:
   - Purpose and use case
   - Parameter meanings
   - Return values
   - Mathematical formulas in remarks

### Usage Patterns
These functions are typically used for:
- **Resource allocation**: "Select exactly K out of N options"
- **Scheduling**: "At least K workers must be available"
- **Configuration**: "At most K features can be enabled simultaneously"
- **Optimization**: "Maximize weighted sum of selected items subject to budget"

### Relationship to Other Theories
While these are Boolean constraints, they bridge to arithmetic:
- Internally, Z3 may translate these to integer arithmetic constraints
- Can be more efficient than manual encoding as integer sums
- Z3 can use specialized pseudo-Boolean solving techniques

## Verification
- **Source**: z3_api.h from Z3 C API
- **Reference**: Z3 C++ API documentation at https://z3prover.github.io/api/html/namespacez3.html
- **Python API**: Z3Py provides `AtLeast`, `AtMost`, `PbEq`, `PbGe`, `PbLe` functions
- **Our Implementation**: Z3Wrap/Core/Interop/NativeLibrary.Constraints.cs
- **Audit Date**: 2025-10-03

## Recommendations
None - implementation is complete and correct.
