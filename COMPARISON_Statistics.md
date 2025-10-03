# Z3 Statistics API Comparison Report

## Overview
**NativeLibrary.Statistics.cs**: 7 functions
**Z3 C API (z3_api.h)**: 9 functions

## Complete Function Mapping

### ✅ Functions in Both (7/9 in NativeLibrary match Z3 API)

| Our Method | Z3 C API | Z3 C++ Method | Purpose |
|------------|----------|---------------|---------|
| `StatsSize` | `Z3_stats_size` | `stats::size()` | Returns number of statistical entries |
| `StatsGetKey` | `Z3_stats_get_key` | `stats::key(unsigned)` | Gets key name for statistic entry at index |
| `StatsGetUintValue` | `Z3_stats_get_uint_value` | `stats::uint_value(unsigned)` | Gets unsigned integer value of statistic entry |
| `StatsGetDoubleValue` | `Z3_stats_get_double_value` | `stats::double_value(unsigned)` | Gets double-precision value of statistic entry |
| `StatsIsUint` | `Z3_stats_is_uint` | `stats::is_uint(unsigned)` | Checks if statistic entry is unsigned integer type |
| `StatsIsDouble` | `Z3_stats_is_double` | `stats::is_double(unsigned)` | Checks if statistic entry is double type |
| `StatsToString` | `Z3_stats_to_string` | `stats::to_string()` | Converts statistics to human-readable string |

### ❌ Functions in Z3 but NOT in NativeLibrary

| Z3 C API | Signature | Purpose | Priority |
|----------|-----------|---------|----------|
| `Z3_stats_inc_ref` | `void Z3_API Z3_stats_inc_ref(Z3_context c, Z3_stats s)` | Increments reference counter of statistics object | **HIGH** - Memory management |
| `Z3_stats_dec_ref` | `void Z3_API Z3_stats_dec_ref(Z3_context c, Z3_stats s)` | Decrements reference counter of statistics object | **HIGH** - Memory management |

### ⚠️ Functions in NativeLibrary but NOT in Z3

None - All functions in NativeLibrary correctly map to Z3 C API.

## API Coverage Summary

| Metric | Count | Percentage |
|--------|-------|------------|
| Z3 C API Functions | 9 | 100% |
| Functions in NativeLibrary | 7 | 77.8% |
| Missing Functions | 2 | 22.2% |

## Function Categories

### Data Retrieval (5 functions) - ✅ Complete
- `Z3_stats_size` / `StatsSize` - Get count of entries
- `Z3_stats_get_key` / `StatsGetKey` - Get entry key name
- `Z3_stats_get_uint_value` / `StatsGetUintValue` - Get uint value
- `Z3_stats_get_double_value` / `StatsGetDoubleValue` - Get double value
- `Z3_stats_to_string` / `StatsToString` - Format as string

### Type Checking (2 functions) - ✅ Complete
- `Z3_stats_is_uint` / `StatsIsUint` - Check if uint type
- `Z3_stats_is_double` / `StatsIsDouble` - Check if double type

### Memory Management (2 functions) - ❌ Missing
- `Z3_stats_inc_ref` - **MISSING** - Increment reference counter
- `Z3_stats_dec_ref` - **MISSING** - Decrement reference counter

## Completeness Assessment

**Status**: ⚠️ **77.8% Complete** - Missing critical reference counting functions

### Missing Functions Analysis

**Z3_stats_inc_ref / Z3_stats_dec_ref**
- **Impact**: HIGH - Required for proper memory management
- **Category**: Reference counting
- **Note**: These functions are critical for statistics objects that may need to outlive their creating context (e.g., caching statistics across solver calls)
- **Reason for omission**: Likely statistics are short-lived within solver/fixedpoint contexts and don't require explicit ref counting in typical usage patterns
- **Recommendation**: Add for completeness, especially if statistics objects are exposed in public API or need extended lifetime

### Current Implementation Pattern

The current implementation assumes statistics are:
1. Retrieved from solver/fixedpoint during query
2. Used immediately for introspection
3. Disposed when solver/fixedpoint context is disposed
4. Not stored or passed outside their originating scope

This pattern works correctly for typical usage but lacks the flexibility for advanced scenarios where statistics need independent lifetime management.

## Recommendations

### Priority 1: Document Current Limitation
Add XML documentation noting that statistics objects should not be stored beyond the lifetime of their creating solver/fixedpoint context.

### Priority 2 (Optional): Add Reference Counting
If statistics objects are exposed in public API or need extended lifetime:
1. Add `StatsIncRef` and `StatsDecRef` methods to NativeLibrary.Statistics.cs
2. Implement `IDisposable` pattern for statistics wrapper (if one exists)
3. Update documentation to reflect ref-counted lifetime semantics

### Priority 3: Verify Usage Patterns
Review all code that uses statistics functions to ensure:
- Statistics are not stored beyond their context lifetime
- No memory leaks occur with current pattern
- Public API (if any) properly documents statistics lifetime constraints

## Z3 C API Reference

Statistics functions are documented in:
- **Header**: `z3_api.h` (Statistics section)
- **URL**: https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h
- **C++ Wrapper**: `z3::stats` class in z3++.h
- **Documentation**: https://z3prover.github.io/api/html/classz3_1_1stats.html

### Typical Usage Pattern (Z3 C API)
```c
Z3_stats stats = Z3_solver_get_statistics(ctx, solver);
unsigned size = Z3_stats_size(ctx, stats);
for (unsigned i = 0; i < size; i++) {
    Z3_string key = Z3_stats_get_key(ctx, stats, i);
    if (Z3_stats_is_uint(ctx, stats, i)) {
        unsigned value = Z3_stats_get_uint_value(ctx, stats, i);
        printf("%s: %u\n", key, value);
    } else {
        double value = Z3_stats_get_double_value(ctx, stats, i);
        printf("%s: %f\n", key, value);
    }
}
// Note: Z3_stats objects are typically reference-counted
// Z3_stats_dec_ref(ctx, stats); // Only needed if inc_ref was called
```

## Verification

- ✅ Source: Z3 C API header (z3_api.h, Statistics section)
- ✅ Reference: Z3 C++ API docs (https://z3prover.github.io/api/html/classz3_1_1stats.html)
- ✅ Implementation: Z3Wrap/Core/Interop/NativeLibrary.Statistics.cs
- ✅ Function count verified: 7/9 (77.8%)
- ✅ Signatures verified against Z3 source
- ✅ Missing functions identified: 2 reference counting functions

---

**Audit Date**: 2025-10-03
**Z3 Version Reference**: master branch (latest)
**Auditor**: Claude Code (Automated Audit Process)
