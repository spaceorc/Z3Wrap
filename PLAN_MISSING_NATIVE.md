# Missing NativeLibrary Functions Implementation Plan

## Overview

This plan systematically implements the remaining missing Z3 C API functions in the NativeLibrary P/Invoke layer. These are low-level bindings that were intentionally skipped during initial development as they're not critical for core functionality.

**Current Status**: ~170 functions missing across 18 files

## Missing Functions Summary by File

| File | Missing | Priority | Category |
|------|---------|----------|----------|
| **Accessors.cs** | 67 | Medium | Many already in other files |
| **Solvers.cs** | 25 | High | Advanced solver features |
| **Tactics.cs** | 10 | Medium | Tactic introspection |
| **Models.cs** | 9 | Medium | Model evaluation helpers |
| **Numerals.cs** | 7 | Medium | Already in other files |
| **Sorts.cs** | 7 | Low | Sort introspection |
| **ConstantsAndApplications.cs** | 7 | Low | Rarely used functions |
| **Quantifiers.cs** | 6 | Medium | Old-style and generic quantifiers |
| **Modifiers.cs** | 5 | Low | AST modification |
| **Arrays.cs** | 4 | Medium | Multi-dimensional arrays |
| **SpecialTheories.cs** | 4+7 | Low | Order relations, recursion |
| **Configuration.cs** | 3 | Low | Config introspection |
| **Statistics.cs** | 3 | Low | Ref counting |
| **StringConversion.cs** | 3 | Low | toString variants |
| **Context.cs** | 4 | Low | Advanced context features |
| **Functions.cs** | 2 | Medium | Recursive functions |
| **BitVectors.cs** | 1 | Low | Minor bitv function |
| **Sets.cs** | 1 | Low | Complement function |
| **Symbols.cs** | 1 | Low | Symbol creation |
| **InteractionLogging.cs** | 1 | Low | Logging |
| **Miscellaneous.cs** | 1 | Low | Misc function |

**Total**: ~170 functions

## Implementation Phases

### Phase 1: High Priority - Advanced Solver Features (25 functions)

**File**: `NativeLibrary.Solvers.cs`

**Functions to Add**:
```
Z3_get_implied_equalities
Z3_solver_congruence_explain
Z3_solver_congruence_next
Z3_solver_congruence_root
Z3_solver_get_levels
Z3_solver_get_non_units
Z3_solver_get_trail
Z3_solver_get_units
Z3_solver_import_model_converter
Z3_solver_next_split
Z3_solver_propagate_consequence
Z3_solver_propagate_created
Z3_solver_propagate_decide
Z3_solver_propagate_declare
Z3_solver_propagate_diseq
Z3_solver_propagate_eq
Z3_solver_propagate_final
Z3_solver_propagate_fixed
Z3_solver_propagate_init
Z3_solver_propagate_on_binding
Z3_solver_propagate_register
Z3_solver_propagate_register_cb
Z3_solver_register_on_clause
Z3_solver_set_initial_value
Z3_solver_solve_for
```

**Value**: Theory propagation callbacks, incremental solving, congruence closure

**Complexity**: Medium (callbacks require special marshalling)

---

### Phase 2: Medium Priority - Tactics and Quantifiers (18 functions)

**File**: `NativeLibrary.Tactics.cs` (10 functions)

**Functions to Add**:
```
Z3_apply_result_get_num_subgoals
Z3_apply_result_get_subgoal
Z3_apply_result_to_string
Z3_get_num_probes
Z3_get_num_simplifiers
Z3_get_num_tactics
Z3_get_probe_name
Z3_get_simplifier_name
Z3_get_tactic_name
Z3_solver_add_simplifier
```

**File**: `NativeLibrary.Quantifiers.cs` (6 functions)

**Functions to Add**:
```
Z3_mk_forall_const (old-style quantifier API)
Z3_mk_exists_const (old-style quantifier API)
Z3_mk_quantifier_const (old-style quantifier API)
Z3_mk_quantifier_const_ex (old-style quantifier API)
Z3_mk_quantifier_const_generic (generic quantifier)
Z3_mk_lambda_const (already noted in file)
```

**File**: `NativeLibrary.Functions.cs` (2 functions)

**Functions to Add**:
```
Z3_mk_rec_func_decl
Z3_add_rec_def
```

**Value**: Tactic introspection, old-style quantifier API compatibility, recursive functions

**Complexity**: Low-Medium

---

### Phase 3: Medium Priority - Models and Arrays (13 functions)

**File**: `NativeLibrary.Models.cs` (9 functions)

**Functions to Add**:
```
Z3_eval
Z3_eval_get_bool
Z3_eval_get_bv
Z3_eval_get_int
Z3_eval_get_numeral_string
Z3_eval_get_real
Z3_eval_get_string
Z3_model_extrapolate
Z3_model_get_as_array_func_decl
```

**File**: `NativeLibrary.Arrays.cs` (4 functions)

**Functions to Add**:
```
Z3_mk_select_n
Z3_mk_store_n
Z3_mk_array_sort_n
Z3_get_array_sort_domain_n
```

**Value**: Convenient model evaluation helpers, multi-dimensional array support

**Complexity**: Low

---

### Phase 4: Low Priority - Sorts and Special Theories (18 functions)

**File**: `NativeLibrary.Sorts.cs` (7 functions)
**File**: `NativeLibrary.SpecialTheories.cs` (11 functions)
**File**: `NativeLibrary.ConstantsAndApplications.cs` (7 functions - check for duplicates)

**Value**: Advanced type system features, special relations, rarely used functions

**Complexity**: Low

---

### Phase 5: Low Priority - Utilities and Cleanup (40+ functions)

**Files**:
- `NativeLibrary.Accessors.cs` (67 - many duplicates in other files)
- `NativeLibrary.Numerals.cs` (7 - many duplicates in other files)
- `NativeLibrary.Modifiers.cs` (5)
- `NativeLibrary.Configuration.cs` (3)
- `NativeLibrary.Statistics.cs` (3)
- `NativeLibrary.StringConversion.cs` (3)
- `NativeLibrary.Context.cs` (4)
- Others (5 functions total)

**Value**: Completeness, edge cases, debugging utilities

**Complexity**: Low (mostly straightforward P/Invoke bindings)

---

## Implementation Process for Each Phase

### Step 1: Identify Exact Functions
1. Read the target NativeLibrary file header
2. Extract the "Missing Functions" list
3. Check Z3 API documentation for exact signatures
4. Verify function doesn't already exist in another file (many duplicates in Accessors.cs)

### Step 2: Add P/Invoke Bindings
For each function, add:

1. **Delegate definition** (in delegates section):
```csharp
private delegate IntPtr FunctionNameDelegate(IntPtr ctx, /* params */);
```

2. **Loading call** (in LoadFunctions method):
```csharp
LoadFunctionOrNull(handle, functionPointers, "Z3_function_name");
```

3. **Wrapper method** (in methods section):
```csharp
/// <summary>
/// Brief description of what the function does.
/// </summary>
/// <param name="ctx">The Z3 context.</param>
/// <returns>Description of return value.</returns>
internal IntPtr FunctionName(IntPtr ctx, /* params */)
{
    var funcPtr = GetFunctionPointer("Z3_function_name");
    var func = Marshal.GetDelegateForFunctionPointer<FunctionNameDelegate>(funcPtr);
    return func(ctx, /* params */);
}
```

### Step 3: Update Documentation
1. Update function count in file header
2. Remove function from "Missing Functions" list
3. Update coverage percentage if shown

### Step 4: Verify
1. Run `make build` (must produce zero warnings)
2. Run `make test` (all 903 tests must pass)
3. Run `make ci` (full pipeline must pass)

---

## Using Task Tool for Systematic Implementation

### Task Pattern for Each Phase:

```
"Implement Phase N: [Phase Name] - [X] missing NativeLibrary functions

Goal: Add [X] missing P/Invoke bindings to NativeLibrary.[FileName].cs

Steps:
1. Read NativeLibrary.[FileName].cs header to confirm missing functions list
2. For each missing function:
   - Look up exact signature in Z3 API documentation
   - Add private delegate definition
   - Add LoadFunctionOrNull call
   - Add internal wrapper method with XML docs
   - Follow exact structure in CLAUDE.md (delegates first, then methods)
3. Update file header:
   - Update function count
   - Remove functions from 'Missing Functions' list
   - Update coverage percentage
4. Run make build (verify zero warnings)
5. Run make test (verify all 903 tests pass)
6. Run make format
7. Report completion with summary of functions added

Requirements:
- Follow existing code style exactly
- Use LoadFunctionOrNull (not LoadFunctionInternal) for all new functions
- Group all delegates together before methods
- XML docs must be concise (see CLAUDE.md guidelines)
- No tests required (P/Invoke mechanical layer)

Return: Summary of functions added and verification results"
```

---

## Success Criteria

For each phase:
- ✅ All identified functions have P/Invoke bindings
- ✅ All delegates defined before methods (file structure correct)
- ✅ All XML documentation concise and accurate
- ✅ `make build` produces zero warnings
- ✅ All 903 tests continue passing
- ✅ `make format` applied
- ✅ File header updated with new counts

For overall completion:
- ✅ All ~170 missing functions implemented
- ✅ 100% Z3 C API coverage in NativeLibrary
- ✅ All file headers show accurate function counts
- ✅ No "Missing Functions" sections with non-zero counts

---

## Priority Rationale

**High Priority (Phase 1)**: Solver propagation and advanced solving features are most likely to be needed by power users doing custom theory integration.

**Medium Priority (Phases 2-3)**: Tactic introspection, quantifier variants, and model helpers provide valuable functionality for advanced use cases.

**Low Priority (Phases 4-5)**: Sort introspection, special theories, and utility functions are rarely used or have workarounds.

**Note on Accessors.cs**: Many of the 67 "missing" functions in Accessors.cs already exist in other files (like Numerals.cs, Models.cs, etc.). Need to audit for actual duplicates before implementing.

---

## Timeline Estimate

- **Phase 1**: 2-3 hours (complex callbacks)
- **Phase 2**: 2 hours (straightforward bindings)
- **Phase 3**: 1.5 hours (simple bindings)
- **Phase 4**: 2 hours (audit duplicates + implement)
- **Phase 5**: 3-4 hours (large cleanup, deduplication)

**Total**: 10-12 hours for complete implementation

**Incremental**: Can commit after each phase for progressive delivery

---

**Status**: Plan created - Ready to begin Phase 1
**Last Updated**: 2025-01-03
