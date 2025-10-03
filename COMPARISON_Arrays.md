# Z3 Arrays API Comparison Report

## Overview
**NativeLibrary.Arrays.cs**: 9 functions
**Z3 C API (z3_api.h)**: 14 functions

## Complete Function Mapping

### ✅ Functions in Both (9/14 in NativeLibrary match Z3 API - 64.3%)

| Our Method | Z3 C API | Parameters | Purpose |
|------------|----------|------------|---------|
| `MkArraySort` | `Z3_mk_array_sort` | `(ctx, domain, range)` | Creates array sort with single domain |
| `MkSelect` | `Z3_mk_select` | `(ctx, array, index)` | Array read operation (array[index]) |
| `MkStore` | `Z3_mk_store` | `(ctx, array, index, value)` | Array update operation (array[index := value]) |
| `MkConstArray` | `Z3_mk_const_array` | `(ctx, domain, value)` | Creates constant array with uniform value |
| `GetArraySortDomain` | `Z3_get_array_sort_domain` | `(ctx, arraySort)` | Retrieves domain sort of array |
| `GetArraySortRange` | `Z3_get_array_sort_range` | `(ctx, arraySort)` | Retrieves range sort of array |
| `MkArrayDefault` | `Z3_mk_array_default` | `(ctx, array)` | Accesses default value of array |
| `MkArrayExt` | `Z3_mk_array_ext` | `(ctx, arg1, arg2)` | Array extensionality constraint |
| `MkAsArray` | `Z3_mk_as_array` | `(ctx, f)` | Creates array from function interpretation |

### ❌ Functions in Z3 but NOT in NativeLibrary

**5 missing functions** - These support multi-dimensional arrays and advanced array operations:

1. **`Z3_mk_array_sort_n`** - `Z3_sort Z3_API Z3_mk_array_sort_n(Z3_context c, unsigned n, Z3_sort const *domain, Z3_sort range)`
   - Creates array sort with N domains (multi-dimensional arrays)
   - Example: 2D array with domains [Int, Bool] and range Real

2. **`Z3_mk_select_n`** - `Z3_ast Z3_API Z3_mk_select_n(Z3_context c, Z3_ast a, unsigned n, Z3_ast const *idxs)`
   - N-ary array read with multiple indices
   - Example: read from 2D array with indices [i, j]

3. **`Z3_mk_store_n`** - `Z3_ast Z3_API Z3_mk_store_n(Z3_context c, Z3_ast a, unsigned n, Z3_ast const *idxs, Z3_ast v)`
   - N-ary array update with multiple indices
   - Example: store value in 2D array at [i, j]

4. **`Z3_mk_map`** - `Z3_ast Z3_API Z3_mk_map(Z3_context c, Z3_func_decl f, unsigned n, Z3_ast const *args)`
   - Applies function to argument arrays element-wise
   - Example: map addition over two arrays to create result array

5. **`Z3_get_array_sort_domain_n`** - `Z3_sort Z3_API Z3_get_array_sort_domain_n(Z3_context c, Z3_sort t, unsigned idx)`
   - Returns i-th domain sort of N-dimensional array
   - Complements GetArraySortDomain for multi-dimensional arrays

### ⚠️ Functions in NativeLibrary but NOT in Z3

**None** - All 9 functions match Z3 C API exactly.

## API Coverage Summary

| Metric | Count | Percentage |
|--------|-------|------------|
| Z3 C API Functions | 14 | 100% |
| Functions in NativeLibrary | 9 | **64.3%** |
| Missing Functions | 5 | 35.7% |
| Extra Functions | 0 | 0% |

## Function Categories

### Array Sort Creation (2 functions - 1 missing)
- ✅ `Z3_mk_array_sort` - Single-dimensional array sort
- ❌ `Z3_mk_array_sort_n` - Multi-dimensional array sort (MISSING)

### Array Operations (6 functions - 4 missing)
- ✅ `Z3_mk_select` - Single-index array read
- ❌ `Z3_mk_select_n` - Multi-index array read (MISSING)
- ✅ `Z3_mk_store` - Single-index array update
- ❌ `Z3_mk_store_n` - Multi-index array update (MISSING)
- ✅ `Z3_mk_const_array` - Constant array creation
- ❌ `Z3_mk_map` - Function mapping over arrays (MISSING)

### Array Properties (6 functions - 1 missing)
- ✅ `Z3_get_array_sort_domain` - Domain sort for single-dimensional arrays
- ❌ `Z3_get_array_sort_domain_n` - Domain sort for specific dimension (MISSING)
- ✅ `Z3_get_array_sort_range` - Range sort
- ✅ `Z3_mk_array_default` - Default value accessor
- ✅ `Z3_mk_array_ext` - Array extensionality
- ✅ `Z3_mk_as_array` - Function-to-array conversion

## Completeness Assessment

⚠️ **INCOMPLETE** - NativeLibrary.Arrays.cs provides 64.3% coverage of Z3's array theory API.

### Strengths
- Complete support for single-dimensional arrays (select/store/sort creation)
- All array property queries implemented
- Array extensionality and function conversion supported
- Excellent XML documentation for all 9 functions

### Gaps
- **No multi-dimensional array support** (Z3_mk_array_sort_n, Z3_mk_select_n, Z3_mk_store_n)
- **No array mapping** (Z3_mk_map) for element-wise function application
- **No N-dimensional domain query** (Z3_get_array_sort_domain_n)

### Impact Analysis
The missing functions limit support for:
1. **Multi-dimensional arrays**: Cannot create or manipulate 2D+ arrays efficiently
2. **Functional array operations**: Cannot map functions over array elements
3. **N-dimensional type queries**: Cannot inspect individual dimensions of multi-dimensional arrays

### Recommendations

#### Priority 1: Add Multi-Dimensional Array Support
Add the three N-ary array functions to enable full multi-dimensional array theory:

```csharp
// Add to LoadFunctionsArrays
LoadFunctionOrNull(handle, functionPointers, "Z3_mk_array_sort_n");
LoadFunctionOrNull(handle, functionPointers, "Z3_mk_select_n");
LoadFunctionOrNull(handle, functionPointers, "Z3_mk_store_n");
LoadFunctionOrNull(handle, functionPointers, "Z3_get_array_sort_domain_n");

// Delegates
private delegate IntPtr MkArraySortNDelegate(IntPtr ctx, uint n, IntPtr[] domains, IntPtr range);
private delegate IntPtr MkSelectNDelegate(IntPtr ctx, IntPtr array, uint n, IntPtr[] indices);
private delegate IntPtr MkStoreNDelegate(IntPtr ctx, IntPtr array, uint n, IntPtr[] indices, IntPtr value);
private delegate IntPtr GetArraySortDomainNDelegate(IntPtr ctx, IntPtr arraySort, uint idx);
```

#### Priority 2: Add Array Map Operation
Add Z3_mk_map for functional programming patterns with arrays:

```csharp
// Add to LoadFunctionsArrays
LoadFunctionOrNull(handle, functionPointers, "Z3_mk_map");

// Delegate
private delegate IntPtr MkMapDelegate(IntPtr ctx, IntPtr f, uint n, IntPtr[] args);
```

#### Usage Impact
- **Current**: Single-dimensional arrays fully supported (sufficient for most use cases)
- **With additions**: Full multi-dimensional array theory support
- **User impact**: Advanced users requiring 2D+ arrays cannot currently use this library

## Verification

- **Source**: Z3 C API header `z3_api.h` from [Z3 GitHub repository](https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h)
- **Documentation**: [Z3 C API Reference](https://z3prover.github.io/api/html/group__capi.html)
- **Our implementation**: `Z3Wrap/Core/Interop/NativeLibrary.Arrays.cs`
- **Verification date**: 2025-01-03
- **Z3 version compatibility**: All versions (array theory is stable core feature)

## Related API Sections

Note: The Sets API (which builds on arrays) is tracked separately:
- `Z3_mk_set_sort` - Set type creation
- `Z3_mk_set_add`, `Z3_mk_set_del` - Set operations
- `Z3_mk_set_union`, `Z3_mk_set_intersect`, etc. - Set theory operations

These are expected to be in `NativeLibrary.Sets.cs` (not yet audited).
