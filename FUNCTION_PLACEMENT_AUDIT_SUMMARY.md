# Z3 Function Placement Audit - Executive Summary

**Date**: 2025-10-03
**Repository**: z3lib
**Auditor**: Comprehensive automated analysis comparing NativeLibrary*.cs against c_headers/*.txt

---

## Overview

This audit compares the placement of Z3 API functions in the NativeLibrary partial class files against their authoritative source locations defined in `c_headers/*.txt`. The goal is to ensure functions are organized logically and consistently with Z3's official API structure.

## Summary Statistics

| Metric | Count | Percentage |
|--------|-------|------------|
| **Total Functions in c_headers** | 556 | 100% |
| **Total Functions Implemented** | 600 | 107.9% |
| **Functions Correctly Placed** | 293 | 52.7% |
| **Functions Misplaced** | 263 | 47.3% |
| **Functions Missing** | 90 | 16.2% |
| **Duplicate Functions** | 2 | 0.4% |

**Note**: The implementation count exceeds the header count because some functions are duplicated (2) and others may be from newer Z3 versions.

---

## Critical Issues

### 1. Duplicate Functions (Immediate Attention Required)

Two functions are loaded in multiple files, which could cause conflicts:

- **Z3_mk_sbv_to_str**: In both `BitVectors.cs` AND `StringTheory.cs`
- **Z3_mk_ubv_to_str**: In both `BitVectors.cs` AND `StringTheory.cs`

**Impact**: Function pointer conflicts, potential runtime errors.

**Recommendation**: Remove from `BitVectors.cs`, keep only in `StringTheory.cs` (correct location per `z3_api_sequences_and_regular_expressions.txt`).

---

## Major Misplacement Categories

### 2. NativeLibrary.Expressions.cs - Massive Overload

**Problem**: This file contains 30 functions that should be distributed across multiple specialized files.

Functions that should move:

**To NativeLibrary.PropositionalLogicAndEquality.cs** (10 functions):
- Z3_mk_and, Z3_mk_or, Z3_mk_not
- Z3_mk_implies, Z3_mk_iff, Z3_mk_xor
- Z3_mk_eq, Z3_mk_distinct
- Z3_mk_ite

**To NativeLibrary.IntegersAndReals.cs** (16 functions):
- Z3_mk_add, Z3_mk_sub, Z3_mk_mul, Z3_mk_div
- Z3_mk_mod, Z3_mk_rem, Z3_mk_unary_minus
- Z3_mk_power, Z3_mk_abs
- Z3_mk_lt, Z3_mk_le, Z3_mk_gt, Z3_mk_ge
- Z3_mk_int2real, Z3_mk_real2int, Z3_mk_is_int, Z3_mk_divides

**To NativeLibrary.Sorts.cs** (3 functions):
- Z3_mk_bool_sort, Z3_mk_int_sort, Z3_mk_real_sort

**To NativeLibrary.Numerals.cs** (1 function):
- Z3_mk_numeral

### 3. NativeLibrary.Queries.cs - Should be NativeLibrary.Accessors.cs

**Problem**: 47 accessor/query functions are incorrectly placed in `Queries.cs`.

All these should move to `Accessors.cs`:
- AST accessors (Z3_get_ast_*, Z3_get_app_*)
- Declaration accessors (Z3_get_decl_*, Z3_get_arity, Z3_get_domain, Z3_get_range)
- Quantifier accessors (Z3_get_quantifier_*)
- Pattern accessors (Z3_get_pattern_*)
- Numeral accessors (Z3_get_numerator, Z3_get_denominator)
- Symbol accessors (Z3_get_symbol_string)
- Sort accessors (Z3_get_sort_name)

### 4. NativeLibrary.Model.cs vs NativeLibrary.Models.cs Confusion

**Problem**: Functions are split between two similarly-named files when they should all be in `Models.cs`.

Functions in `Model.cs` that should move to `Models.cs`:
- All Z3_model_* functions (14 functions)
- Z3_get_bool_value
- Z3_get_numeral_string
- Z3_get_sort
- Z3_get_sort_kind

### 5. NativeLibrary.FunctionInterpretations.cs - Should Merge into Models.cs

**Problem**: 9 function interpretation functions are in a separate file when they're part of the Models API.

All functions from `FunctionInterpretations.cs` should move to `Models.cs`:
- Z3_func_interp_* functions (7 functions)
- Z3_func_entry_* functions (3 functions)

### 6. Solver API Split Across Multiple Files

**Problem**: Solver functions are scattered across `Solver.cs`, `SolverExtensions.cs`, but should all be in `Solvers.cs`.

Functions that need consolidation (35 functions total):
- From `Solver.cs`: Z3_mk_solver, Z3_mk_simple_solver, Z3_solver_assert, Z3_solver_check, etc.
- From `SolverExtensions.cs`: Z3_solver_check_assumptions, Z3_solver_get_assertions, Z3_solver_from_file, etc.

### 7. Utilities.cs - Catch-all That Needs Refactoring

**Problem**: 14 functions in `Utilities.cs` belong to specific categories.

**To InteractionLogging.cs** (4 functions):
- Z3_open_log, Z3_close_log, Z3_append_log, Z3_toggle_warning_messages

**To Miscellaneous.cs** (5 functions):
- Z3_enable_trace, Z3_disable_trace
- Z3_finalize_memory, Z3_reset_memory
- Z3_get_version, Z3_get_full_version

**To StringConversion.cs** (4 functions):
- Z3_func_decl_to_string, Z3_pattern_to_string
- Z3_sort_to_string, Z3_set_ast_print_mode

**To ErrorHandling.cs** (1 function):
- Z3_set_error

### 8. Tactics/Simplifiers/Probes Scattered

**Problem**: 28 functions split across `Probes.cs` and `Simplifiers.cs` should all be in `Tactics.cs`.

**From Probes.cs** (15 functions):
- All Z3_probe_* functions
- Z3_mk_probe

**From Simplifiers.cs** (7 functions):
- All Z3_simplifier_* functions
- Z3_mk_simplifier

**From ReferenceCountingExtra.cs** (2 functions):
- Z3_apply_result_inc_ref, Z3_apply_result_dec_ref

### 9. Parameters vs Parameter Descriptions Confusion

**Problem**: 7 parameter description functions are in `Parameters.cs` instead of `ParameterDescriptions.cs`.

Functions to move:
- Z3_param_descrs_* (7 functions)

Also, global parameter functions should move to `GlobalParameters.cs`:
- Z3_global_param_set, Z3_global_param_get, Z3_global_param_reset_all

### 10. Datatypes vs Sorts Confusion

**Problem**: 20 sort-creation functions are in `Datatypes.cs` when they should be in `Sorts.cs`.

Functions to move:
- Z3_mk_constructor, Z3_mk_constructor_list, Z3_del_constructor, Z3_del_constructor_list
- Z3_constructor_num_fields
- Z3_mk_datatype, Z3_mk_datatype_sort, Z3_mk_datatypes
- Z3_mk_tuple_sort, Z3_mk_list_sort
- Z3_query_constructor
- Various datatype accessor functions (Z3_get_datatype_sort_*, Z3_get_tuple_sort_*)

---

## Missing Functions (90 total)

### High Priority Missing Functions

**Quantifiers** (6 missing) - Core functionality:
- Z3_mk_forall, Z3_mk_exists
- Z3_mk_quantifier, Z3_mk_quantifier_ex
- Z3_mk_quantifier_const, Z3_mk_quantifier_const_ex

**Numerals** (6 missing) - Convenient value creation:
- Z3_mk_int, Z3_mk_int64, Z3_mk_unsigned_int, Z3_mk_unsigned_int64
- Z3_mk_real, Z3_mk_real_int64

**Solvers** (25 missing) - Extended solver features:
- Z3_get_implied_equalities
- Z3_solver_congruence_* (explain, next, root)
- Z3_solver_get_levels, Z3_solver_get_trail, Z3_solver_get_units
- And 18 more solver functions

### Medium Priority Missing Functions

**Accessors** (19 missing):
- Z3_app_to_ast, Z3_func_decl_to_ast
- Z3_get_array_arity, Z3_get_array_sort_domain_n
- Z3_get_finite_domain_sort_size
- And 14 more accessor functions

**Tactics** (10 missing):
- Z3_apply_result_get_num_subgoals, Z3_apply_result_get_subgoal
- Z3_get_num_tactics, Z3_get_tactic_name
- Z3_solver_add_simplifier

### Lower Priority Missing Functions

**Models** (4 missing): Z3_add_const_interp, Z3_add_func_interp, Z3_mk_model, Z3_get_as_array_func_decl

**Arrays** (3 missing): Z3_mk_map, Z3_mk_select_n, Z3_mk_store_n

**Sorts** (3 missing): Z3_mk_array_sort_n, Z3_mk_type_variable, Z3_mk_uninterpreted_sort

**Special Relations** (4 missing): Z3_mk_linear_order, Z3_mk_partial_order, Z3_mk_piecewise_linear_order, Z3_mk_tree_order

---

## Recommendations

### Immediate Actions

1. **Fix Duplicates**: Remove Z3_mk_sbv_to_str and Z3_mk_ubv_to_str from `BitVectors.cs`

2. **Split NativeLibrary.Expressions.cs**:
   - Create/populate `PropositionalLogicAndEquality.cs` (10 functions)
   - Create/populate `IntegersAndReals.cs` (16 functions)
   - Move sort functions to `Sorts.cs` (3 functions)
   - Move numeral creation to `Numerals.cs` (1 function)

3. **Consolidate Model API**:
   - Merge `Model.cs` and `FunctionInterpretations.cs` into `Models.cs`
   - Total: ~40 functions in one logical group

4. **Consolidate Solver API**:
   - Merge `Solver.cs` and `SolverExtensions.cs` into `Solvers.cs`
   - Add 25 missing solver functions

5. **Rename/Consolidate Queries**:
   - Rename `Queries.cs` to merge with `Accessors.cs`
   - Total: ~100 accessor functions in one file

### Short-Term Actions

6. **Break Down Utilities.cs**: Distribute 14 functions to their proper homes
7. **Consolidate Tactics API**: Merge `Probes.cs`, `Simplifiers.cs` into `Tactics.cs`
8. **Fix Parameters Split**: Separate `ParameterDescriptions.cs` and `GlobalParameters.cs` properly
9. **Clarify Datatypes vs Sorts**: Move sort creation functions from `Datatypes.cs` to `Sorts.cs`

### Medium-Term Actions

10. **Add Missing Quantifier Functions**: Critical for many use cases (6 functions)
11. **Add Missing Numeral Constructors**: Convenience functions (6 functions)
12. **Add Missing Solver Features**: Extended solver capabilities (25 functions)
13. **Complete Accessors**: Add remaining 19 accessor functions

---

## File-by-File Impact Summary

| Current File | Functions | Status | Action Needed |
|--------------|-----------|--------|---------------|
| **Expressions.cs** | 30 | ❌ Major refactor | Split into 4 files |
| **Queries.cs** | 47 | ❌ Wrong name | Merge into Accessors.cs |
| **Model.cs** | 18 | ❌ Should be Models.cs | Rename and merge |
| **FunctionInterpretations.cs** | 9 | ❌ Redundant | Merge into Models.cs |
| **Solver.cs** | 10 | ❌ Split API | Merge into Solvers.cs |
| **SolverExtensions.cs** | 25 | ❌ Split API | Merge into Solvers.cs |
| **Utilities.cs** | 14 | ❌ Catch-all | Distribute to 4 files |
| **Probes.cs** | 15 | ❌ Should be in Tactics | Merge into Tactics.cs |
| **Simplifiers.cs** | 7 | ❌ Should be in Tactics | Merge into Tactics.cs |
| **Parameters.cs** | 10 | ⚠️ Mixed concerns | Split into 2 files |
| **Datatypes.cs** | 20 | ⚠️ Some misplaced | Move sorts to Sorts.cs |
| **BitVectors.cs** | 2 duplicates | ❌ Duplicates | Remove 2 functions |

**Total functions needing movement**: 263 (47.3% of implemented functions)

---

## Verification

The audit was performed using:
- **Source of truth**: `c_headers/*.txt` files containing official Z3 API groupings
- **Implementation**: All `NativeLibrary.*.cs` files in `Z3Wrap/Core/Interop/`
- **Method**: Automated parsing and cross-referencing of LoadFunction calls
- **Tool**: `audit_functions.py` Python script

Full detailed report available in: `FUNCTION_PLACEMENT_AUDIT.md`

---

## Conclusion

The current function placement shows **47.3% misplacement rate**, indicating significant organizational debt. The most impactful improvements would be:

1. Fixing duplicate functions (immediate bug risk)
2. Splitting the overloaded Expressions.cs file
3. Consolidating the fragmented Model and Solver APIs
4. Standardizing file naming (Model → Models, Queries → Accessors)

These changes would improve code maintainability, make the API structure match Z3's official organization, and reduce cognitive load for developers working with the codebase.
