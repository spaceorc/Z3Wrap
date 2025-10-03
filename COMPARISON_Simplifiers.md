# Z3 Simplifiers API Comparison Report

## Overview
**NativeLibrary.Simplifiers.cs**: 8 functions
**Z3 C API (z3_api.h)**: 11 functions
**Completeness**: 72.7% (8/11 functions implemented)

## Complete Function Mapping

### ✅ Functions in Both (8/11 functions in NativeLibrary match Z3 API)

| Our Method | Z3 C API | Return Type | Purpose |
|------------|----------|-------------|---------|
| MkSimplifier | Z3_mk_simplifier | Z3_simplifier | Creates a simplifier by name |
| SimplifierIncRef | Z3_simplifier_inc_ref | void | Increments reference counter of simplifier |
| SimplifierDecRef | Z3_simplifier_dec_ref | void | Decrements reference counter of simplifier |
| SimplifierGetHelp | Z3_simplifier_get_help | Z3_string | Returns help string for simplifier |
| SimplifierGetParamDescrs | Z3_simplifier_get_param_descrs | Z3_param_descrs | Returns parameter descriptors for simplifier |
| SimplifierGetDescr | Z3_simplifier_get_descr | Z3_string | Returns description for named simplifier |
| SimplifierUsingParams | Z3_simplifier_using_params | Z3_simplifier | Creates simplifier configured with parameters |
| SimplifierAndThen | Z3_simplifier_and_then | Z3_simplifier | Creates sequential composition of two simplifiers |

### ❌ Functions in Z3 API but NOT in NativeLibrary (3 missing)

| Z3 C API | Signature | Purpose | Impact |
|----------|-----------|---------|--------|
| Z3_solver_add_simplifier | `Z3_solver Z3_API Z3_solver_add_simplifier(Z3_context c, Z3_solver solver, Z3_simplifier simplifier)` | Attaches simplifier to solver for incremental pre-processing | **MEDIUM** - Important for using simplifiers with solvers |
| Z3_get_num_simplifiers | `unsigned Z3_API Z3_get_num_simplifiers(Z3_context c)` | Returns number of builtin simplifiers available | **LOW** - Discovery/enumeration feature |
| Z3_get_simplifier_name | `Z3_string Z3_API Z3_get_simplifier_name(Z3_context c, unsigned i)` | Returns name of the i-th simplifier | **LOW** - Discovery/enumeration feature |

### ⚠️ Functions in NativeLibrary but NOT in Z3 API

None - all functions map correctly to Z3 C API.

## API Coverage Summary

| Metric | Count | Percentage |
|--------|-------|------------|
| Z3 C API Functions | 11 | 100% |
| Functions in NativeLibrary | 8 | 72.7% |
| Missing Functions | 3 | 27.3% |

## Function Categories

### Core Simplifier Operations (8/8 - 100%)
- ✅ Z3_mk_simplifier - Create simplifier by name
- ✅ Z3_simplifier_inc_ref - Reference counting
- ✅ Z3_simplifier_dec_ref - Reference counting
- ✅ Z3_simplifier_get_help - Get help string
- ✅ Z3_simplifier_get_param_descrs - Get parameter descriptors
- ✅ Z3_simplifier_get_descr - Get description for named simplifier
- ✅ Z3_simplifier_using_params - Apply parameters to simplifier
- ✅ Z3_simplifier_and_then - Compose simplifiers sequentially

### Solver Integration (0/1 - 0%)
- ❌ Z3_solver_add_simplifier - Attach simplifier to solver

### Simplifier Discovery (0/2 - 0%)
- ❌ Z3_get_num_simplifiers - Get count of builtin simplifiers
- ❌ Z3_get_simplifier_name - Get name of i-th simplifier

## Missing Functions Analysis

### Z3_solver_add_simplifier
**Impact**: MEDIUM
**Reason**: This function belongs to the Solver API category, not the Simplifiers API. It allows attaching a simplifier to a solver for incremental pre-processing. Should be added to `NativeLibrary.Solver.cs` instead.

**Recommendation**: Add to NativeLibrary.Solver.cs as it's part of solver configuration rather than simplifier creation.

### Z3_get_num_simplifiers & Z3_get_simplifier_name
**Impact**: LOW
**Reason**: These are discovery/enumeration functions that allow listing all available builtin simplifiers. Useful for tooling and introspection but not required for basic usage.

**Recommendation**: Add to NativeLibrary.Simplifiers.cs for completeness. These functions enable runtime discovery of available simplifiers, which can be useful for:
- Building dynamic UIs that list available simplifiers
- Documentation generation
- Testing and validation
- User-facing tools that need to discover capabilities

## Completeness Assessment

**Status**: ⚠️ MOSTLY COMPLETE (72.7%)

The NativeLibrary.Simplifiers.cs file provides comprehensive coverage of the core simplifier operations:
- ✅ All simplifier creation and composition functions present
- ✅ Complete reference counting support
- ✅ Full parameter and documentation query support
- ❌ Missing simplifier discovery/enumeration functions (2)
- ❌ Missing solver integration function (1, belongs in Solver.cs)

### Recommendations

1. **Add Discovery Functions** (Priority: LOW)
   - Implement Z3_get_num_simplifiers
   - Implement Z3_get_simplifier_name
   - These enable runtime enumeration of available simplifiers

2. **Move Solver Integration** (Priority: MEDIUM)
   - Z3_solver_add_simplifier should be added to NativeLibrary.Solver.cs
   - This function is part of the Solver API, not the Simplifiers API
   - Current Solver.cs has 5 functions, would become 6

3. **Update Documentation**
   - Add file header comment with Z3 source references
   - Document relationship between simplifiers and solvers
   - Note that Z3_solver_add_simplifier is in a different file

## Z3 Documentation References

- **Source**: z3_api.h (Simplifiers section)
- **URL**: https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h
- **Section**: Lines containing Z3_simplifier functions (search for "def_API.*SIMPLIFIER")
- **Online Guide**: https://microsoft.github.io/z3guide/docs/strategies/simplifiers/

## Implementation Notes

### Current File Organization
The file is well-organized with:
- Clear separation of loading, delegates, and methods
- Comprehensive XML documentation for all public methods
- Proper error handling and marshalling

### Delegate Signatures
All delegate signatures correctly match the Z3 C API:
- IntPtr used for Z3 opaque types (context, simplifier, string, param_descrs)
- void used for reference counting operations
- Proper parameter ordering

### Missing Function Signatures (for reference)

```csharp
// Should be added to NativeLibrary.Solver.cs
private delegate IntPtr SolverAddSimplifierDelegate(IntPtr ctx, IntPtr solver, IntPtr simplifier);

// Should be added to NativeLibrary.Simplifiers.cs
private delegate uint GetNumSimplifiersDelegate(IntPtr ctx);
private delegate IntPtr GetSimplifierNameDelegate(IntPtr ctx, uint idx);
```

## Verification
- ✅ Extracted all LoadFunctionOrNull calls from NativeLibrary.Simplifiers.cs
- ✅ Compared against Z3 C API z3_api.h source
- ✅ Identified all missing functions
- ✅ Categorized by functionality
- ✅ Assessed impact and priority
- ✅ Generated comprehensive comparison report

**Date**: 2025-10-03
**Z3 Version Reference**: master branch (latest)
