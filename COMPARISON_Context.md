# Z3 Context API Comparison Report

## Overview
**NativeLibrary.Context.cs**: 8 functions
**Z3 C API (z3_api.h Context section)**: 15 functions

## Complete Function Mapping

### ✅ Functions in Both (8/15 in NativeLibrary match Z3 API)

| Our Method | Z3 C API | Purpose |
|------------|----------|---------|
| `MkConfig()` | `Z3_mk_config` | Create configuration object |
| `DelConfig(cfg)` | `Z3_del_config` | Delete configuration object |
| `SetParamValue(cfg, param, value)` | `Z3_set_param_value` | Set configuration parameter before context creation |
| `MkContextRc(cfg)` | `Z3_mk_context_rc` | Create context with reference counting |
| `DelContext(ctx)` | `Z3_del_context` | Delete context |
| `UpdateParamValue(ctx, param, value)` | `Z3_update_param_value` | Update parameter in existing context |
| `IncRef(ctx, ast)` | `Z3_inc_ref` | Increment AST reference count |
| `DecRef(ctx, ast)` | `Z3_dec_ref` | Decrement AST reference count |

### ❌ Functions in Z3 but NOT in NativeLibrary (7 missing)

1. **`Z3_mk_context(Z3_config c)`**
   - Purpose: Create a context using the given configuration (automatic reference counting)
   - Note: We only have `Z3_mk_context_rc` which requires manual reference counting
   - Priority: Low (Z3_mk_context_rc is preferred for explicit memory management)

2. **`Z3_global_param_set(Z3_string param_id, Z3_string param_value)`**
   - Purpose: Set a global (or module) parameter
   - Note: Already in NativeLibrary.Parameters.cs
   - Priority: N/A (in different file)

3. **`Z3_global_param_reset_all()`**
   - Purpose: Reset all global parameters
   - Note: Already in NativeLibrary.Parameters.cs
   - Priority: N/A (in different file)

4. **`Z3_global_param_get(Z3_string param_id, Z3_string_ptr param_value)`**
   - Purpose: Get value of global parameter
   - Note: Already in NativeLibrary.Parameters.cs
   - Priority: N/A (in different file)

5. **`Z3_get_global_param_descrs()`**
   - Purpose: Retrieve descriptions of all global parameters
   - Note: Already in NativeLibrary.Parameters.cs
   - Priority: N/A (in different file)

6. **`Z3_interrupt(Z3_context c)`**
   - Purpose: Interrupt execution of a Z3 procedure
   - Priority: Medium (useful for cancellation support)
   - Use case: Long-running solver operations that need to be cancelled

7. **`Z3_enable_concurrent_dec_ref(Z3_context c)`**
   - Purpose: Enable thread-safe reference counting decrements
   - Priority: Medium (important for multi-threaded scenarios)
   - Use case: When multiple threads need to decrement reference counts safely

### ⚠️ Functions in NativeLibrary but NOT in Z3 (0)

None - all our functions map correctly to Z3 C API.

## API Coverage Summary

| Metric | Count | Percentage |
|--------|-------|------------|
| Z3 C API Context Functions | 10 | 100% |
| Functions in NativeLibrary | 8 | 80.0% |
| Missing Functions (actual) | 2 | 20.0% |
| Global param functions (in Parameters.cs) | 5 | N/A |

**Note**: Of the 15 total functions in Z3's context section:
- 8 are in NativeLibrary.Context.cs (53.3%)
- 5 are properly placed in NativeLibrary.Parameters.cs (33.3%)
- 2 are missing: `Z3_interrupt` and `Z3_enable_concurrent_dec_ref` (13.3%)
- 1 is intentionally not included: `Z3_mk_context` (we use `Z3_mk_context_rc` instead)

**Adjusted Coverage**: 8/10 core context functions = **80% complete**

## Function Categories

### Configuration Management (3/3 = 100%)
- ✅ `Z3_mk_config` - Create configuration
- ✅ `Z3_del_config` - Delete configuration
- ✅ `Z3_set_param_value` - Set config parameter

### Context Lifecycle (2/3 = 66.7%)
- ❌ `Z3_mk_context` - Not included (automatic ref counting)
- ✅ `Z3_mk_context_rc` - Create context with manual ref counting
- ✅ `Z3_del_context` - Delete context

### Parameter Management (1/1 = 100%)
- ✅ `Z3_update_param_value` - Update context parameter

### Reference Counting (2/3 = 66.7%)
- ✅ `Z3_inc_ref` - Increment reference
- ✅ `Z3_dec_ref` - Decrement reference
- ❌ `Z3_enable_concurrent_dec_ref` - Thread-safe decrements

### Control Flow (0/1 = 0%)
- ❌ `Z3_interrupt` - Interrupt execution

### Global Parameters (5/5 = 100% - in Parameters.cs)
- ✅ `Z3_global_param_set` (in NativeLibrary.Parameters.cs)
- ✅ `Z3_global_param_reset_all` (in NativeLibrary.Parameters.cs)
- ✅ `Z3_global_param_get` (in NativeLibrary.Parameters.cs)
- ✅ `Z3_get_global_param_descrs` (in NativeLibrary.Parameters.cs)
- ✅ `Z3_params_to_string` (parameter descriptions, in Parameters.cs)

## Completeness Assessment

### ✅ Status: **GOOD** (80% coverage for core context functions)

**Strengths**:
- All essential context lifecycle functions present
- Configuration management complete
- Basic reference counting complete
- Parameter management complete
- Global parameters properly categorized in Parameters.cs

**Gaps**:
1. **Missing `Z3_interrupt`**: Prevents graceful cancellation of long-running operations
2. **Missing `Z3_enable_concurrent_dec_ref`**: Limits thread-safety in multi-threaded scenarios

**Recommendations**:
1. **Priority Medium**: Add `Z3_interrupt(Z3_context c)` for cancellation support
   - Useful for interactive applications
   - Allows stopping solver operations that take too long
   - Implementation: Add to Context.cs with proper delegate

2. **Priority Medium**: Add `Z3_enable_concurrent_dec_ref(Z3_context c)` for thread safety
   - Critical for multi-threaded applications
   - Must be called after context creation
   - Implementation: Add to Context.cs with proper delegate

3. **Priority Low**: Consider adding `Z3_mk_context` for completeness
   - Currently only `Z3_mk_context_rc` is available
   - `Z3_mk_context` provides automatic reference counting
   - However, explicit reference counting (`Z3_mk_context_rc`) is preferred for .NET interop

## Design Notes

### Reference Counting Strategy
The library correctly uses `Z3_mk_context_rc` for explicit reference counting, which is the recommended approach for managed wrappers. The automatic reference counting variant (`Z3_mk_context`) is less suitable for .NET interop.

### Parameter Organization
Global parameter functions are correctly categorized in `NativeLibrary.Parameters.cs` rather than `Context.cs`, following logical API grouping.

### Missing Functionality Impact
- **Cancellation**: Without `Z3_interrupt`, users cannot cancel long-running solver operations
- **Thread Safety**: Without `Z3_enable_concurrent_dec_ref`, multi-threaded scenarios may require manual synchronization

## Verification
- **Source**: Z3 C API header z3_api.h (Context Management section)
- **Our implementation**: Z3Wrap/Core/Interop/NativeLibrary.Context.cs
- **Related files**: NativeLibrary.Parameters.cs (global parameter functions)
- **Date verified**: 2025-10-03
