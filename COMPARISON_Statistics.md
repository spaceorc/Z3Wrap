# Z3 Statistics API Comparison Report

## Overview
**NativeLibrary.Statistics.cs**: 7 functions (data retrieval + type checking)
**NativeLibrary.ReferenceCountingExtra.cs**: 2 functions (reference counting)
**Total Coverage**: 9/9 functions (100%)
**Z3 C API (z3_api.h)**: 9 functions

## Complete Function Mapping

### ✅ Functions in Both (9/9 in NativeLibrary match Z3 API)

| Our Method | Z3 C API | Z3 C++ Method | File Location | Purpose |
|------------|----------|---------------|---------------|---------|
| `StatsIncRef` | `Z3_stats_inc_ref` | N/A (C++ uses RAII) | ReferenceCountingExtra.cs | Increments reference counter of statistics object |
| `StatsDecRef` | `Z3_stats_dec_ref` | N/A (C++ uses RAII) | ReferenceCountingExtra.cs | Decrements reference counter of statistics object |
| `StatsSize` | `Z3_stats_size` | `stats::size()` | Statistics.cs | Returns number of statistical entries |
| `StatsGetKey` | `Z3_stats_get_key` | `stats::key(unsigned)` | Statistics.cs | Gets key name for statistic entry at index |
| `StatsGetUintValue` | `Z3_stats_get_uint_value` | `stats::uint_value(unsigned)` | Statistics.cs | Gets unsigned integer value of statistic entry |
| `StatsGetDoubleValue` | `Z3_stats_get_double_value` | `stats::double_value(unsigned)` | Statistics.cs | Gets double-precision value of statistic entry |
| `StatsIsUint` | `Z3_stats_is_uint` | `stats::is_uint(unsigned)` | Statistics.cs | Checks if statistic entry is unsigned integer type |
| `StatsIsDouble` | `Z3_stats_is_double` | `stats::is_double(unsigned)` | Statistics.cs | Checks if statistic entry is double type |
| `StatsToString` | `Z3_stats_to_string` | `stats::to_string()` | Statistics.cs | Converts statistics to human-readable string |

### ❌ Functions in Z3 but NOT in NativeLibrary

None - All Z3 C API functions are implemented.

### ⚠️ Functions in NativeLibrary but NOT in Z3

None - All functions in NativeLibrary correctly map to Z3 C API.

## API Coverage Summary

| Metric | Count | Percentage |
|--------|-------|------------|
| Z3 C API Functions | 9 | 100% |
| Functions in Statistics.cs | 7 | 77.8% |
| Functions in ReferenceCountingExtra.cs | 2 | 22.2% |
| Total NativeLibrary Coverage | 9 | 100% |
| Missing Functions | 0 | 0% |

## Function Categories

### Data Retrieval (5 functions) - ✅ Complete (in Statistics.cs)
- `Z3_stats_size` / `StatsSize` - Get count of entries
- `Z3_stats_get_key` / `StatsGetKey` - Get entry key name
- `Z3_stats_get_uint_value` / `StatsGetUintValue` - Get uint value
- `Z3_stats_get_double_value` / `StatsGetDoubleValue` - Get double value
- `Z3_stats_to_string` / `StatsToString` - Format as string

### Type Checking (2 functions) - ✅ Complete (in Statistics.cs)
- `Z3_stats_is_uint` / `StatsIsUint` - Check if uint type
- `Z3_stats_is_double` / `StatsIsDouble` - Check if double type

### Memory Management (2 functions) - ✅ Complete (in ReferenceCountingExtra.cs)
- `Z3_stats_inc_ref` / `StatsIncRef` - Increment reference counter
- `Z3_stats_dec_ref` / `StatsDecRef` - Decrement reference counter

## Completeness Assessment

**Status**: ✅ **100% COMPLETE** - All Z3 Statistics API functions implemented

### Implementation Organization

The Statistics API implementation is split across two files for architectural clarity:

**NativeLibrary.Statistics.cs** (7 functions):
- Core statistics functionality (data retrieval, type checking, formatting)
- Functions specific to statistics introspection and querying

**NativeLibrary.ReferenceCountingExtra.cs** (2 functions):
- Reference counting for statistics objects
- Grouped with other non-AST reference counting functions (apply_result, func_entry, func_interp, pattern, fixedpoint)
- Provides consistent memory management pattern across Z3 object types

### Memory Management Pattern

Statistics objects support two usage patterns:

1. **Short-lived (typical)**: Retrieved from solver/fixedpoint, used immediately, disposed with context
2. **Extended lifetime (advanced)**: Explicitly reference counted using StatsIncRef/StatsDecRef for caching across solver calls

Both patterns are fully supported with proper memory management through the ReferenceCountingExtra.cs functions.

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
- ✅ Implementation: Z3Wrap/Core/Interop/NativeLibrary.Statistics.cs (7 functions)
- ✅ Implementation: Z3Wrap/Core/Interop/NativeLibrary.ReferenceCountingExtra.cs (2 functions)
- ✅ Function count verified: 9/9 (100%)
- ✅ Signatures verified against Z3 source
- ✅ All functions implemented across appropriate organizational files

---

**Audit Date**: 2025-10-03
**Z3 Version Reference**: master branch (latest)
**Auditor**: Claude Code (Automated Audit Process)
