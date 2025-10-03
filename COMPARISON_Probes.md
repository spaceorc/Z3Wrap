# Z3 Probes API Comparison Report

## Overview
**NativeLibrary.Probes.cs**: 14 functions
**Z3 C API (z3_api.h)**: 14 probe-specific functions
**Completeness**: 100%

## Complete Function Mapping

### ✅ Functions in Both (14/14 in NativeLibrary match Z3 API)

| Our Method | Z3 C API | Parameters | Return Type | Purpose |
|------------|----------|------------|-------------|---------|
| MkProbe | Z3_mk_probe | ctx, name | Z3_probe | Creates probe by name |
| ProbeIncRef | Z3_probe_inc_ref | ctx, probe | void | Increments reference counter |
| ProbeDecRef | Z3_probe_dec_ref | ctx, probe | void | Decrements reference counter |
| ProbeConst | Z3_probe_const | ctx, value | Z3_probe | Creates constant probe |
| ProbeLt | Z3_probe_lt | ctx, probe1, probe2 | Z3_probe | Less-than comparison |
| ProbeGt | Z3_probe_gt | ctx, probe1, probe2 | Z3_probe | Greater-than comparison |
| ProbeLe | Z3_probe_le | ctx, probe1, probe2 | Z3_probe | Less-than-or-equal comparison |
| ProbeGe | Z3_probe_ge | ctx, probe1, probe2 | Z3_probe | Greater-than-or-equal comparison |
| ProbeEq | Z3_probe_eq | ctx, probe1, probe2 | Z3_probe | Equality comparison |
| ProbeAnd | Z3_probe_and | ctx, probe1, probe2 | Z3_probe | Logical conjunction |
| ProbeOr | Z3_probe_or | ctx, probe1, probe2 | Z3_probe | Logical disjunction |
| ProbeNot | Z3_probe_not | ctx, probe | Z3_probe | Logical negation |
| ProbeGetDescr | Z3_probe_get_descr | ctx, name | Z3_string | Get probe description |
| ProbeApply | Z3_probe_apply | ctx, probe, goal | double | Apply probe to goal |

### ❌ Functions in Z3 but NOT in NativeLibrary
None - all probe-specific functions are implemented.

### ⚠️ Functions in NativeLibrary but NOT in Z3
None - no extra functions.

## API Coverage Summary
| Metric | Count | Percentage |
|--------|-------|------------|
| Z3 C API Functions | 14 | 100% |
| Functions in NativeLibrary | 14 | 100% |
| Missing Functions | 0 | 0% |

## Function Categories

### 1. Probe Creation (2 functions)
- **Z3_mk_probe**: Creates named probe for goal inspection
- **Z3_probe_const**: Creates constant-valued probe

### 2. Reference Counting (2 functions)
- **Z3_probe_inc_ref**: Increment reference counter
- **Z3_probe_dec_ref**: Decrement reference counter

### 3. Comparison Probes (5 functions)
- **Z3_probe_lt**: Less than (<)
- **Z3_probe_gt**: Greater than (>)
- **Z3_probe_le**: Less than or equal (≤)
- **Z3_probe_ge**: Greater than or equal (≥)
- **Z3_probe_eq**: Equal (=)

### 4. Logical Probes (3 functions)
- **Z3_probe_and**: Conjunction (AND)
- **Z3_probe_or**: Disjunction (OR)
- **Z3_probe_not**: Negation (NOT)

### 5. Probe Utilities (2 functions)
- **Z3_probe_get_descr**: Get probe description string
- **Z3_probe_apply**: Execute probe on goal and return result

## Completeness Assessment
✅ **100% Complete** - All probe-specific functions from Z3 C API are implemented in NativeLibrary.Probes.cs.

## What Are Probes?

Probes are functions/predicates used to inspect goals and collect information that may be used to decide which solver and/or preprocessing step will be used. They are essential for tactic selection and goal analysis in Z3's goal-oriented solving approach.

### Key Characteristics
- Probes always return a double value
- Boolean probes return 0.0 for false, non-zero for true
- Probes can be combined using comparison and logical operators
- Used with tactics (Z3_tactic_when, Z3_tactic_cond, Z3_tactic_fail_if)

### Common Named Probes
Examples include:
- "is-qfbv" - checks if goal is quantifier-free bit-vector
- "arith-max-deg" - returns maximum polynomial degree
- "size" - returns number of assertions in goal
- "num-exprs" - returns number of expressions in goal
- "num-consts" - returns number of constants in goal

## Related Functions in Other Files

Note: The following tactic-related functions that use probes are implemented in **NativeLibrary.Tactics.cs**, not this file:
- **Z3_tactic_when**: Apply tactic when probe is true
- **Z3_tactic_cond**: Conditional tactic based on probe
- **Z3_tactic_fail_if**: Fail if probe evaluates to true

This separation is correct as these are tactic constructors that consume probes, not probe operations themselves.

## Verification

### Source Reference
- **Z3 C API Header**: z3_api.h
- **GitHub URL**: https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h
- **API Documentation**: https://z3prover.github.io/api/html/group__capi.html

### Our Implementation
- **File**: Z3Wrap/Core/Interop/NativeLibrary.Probes.cs
- **Lines**: 292 (including documentation)
- **Structure**: LoadFunctionsProbes, delegates, and public methods with XML documentation

### Verification Method
```bash
# Extract all Z3 probe functions from z3_api.h
curl -s "https://raw.githubusercontent.com/Z3Prover/z3/master/src/api/z3_api.h" | \
  grep -E "Z3_mk_probe|Z3_probe_" | grep "Z3_API"
```

### Documentation Quality
All 14 functions have:
- ✅ Comprehensive XML documentation
- ✅ Parameter descriptions
- ✅ Return value documentation
- ✅ Detailed remarks explaining behavior
- ✅ See-also references to Z3 documentation

## Recommendations
✅ **No action required** - This file is complete and correctly implements the entire Z3 Probes API.

## Notes

1. **Delegate Naming**: Our delegate names follow Z3Wrap conventions (e.g., `MkProbeDelegate` for `Z3_mk_probe`)
2. **Method Naming**: Our method names use PascalCase without Z3_ prefix (e.g., `MkProbe` for `Z3_mk_probe`)
3. **Return Types**: Correctly mapped (IntPtr for Z3_probe/Z3_string, double for probe results, void for ref counting)
4. **Documentation**: Exceeds minimum requirements with comprehensive remarks and examples

## Summary

NativeLibrary.Probes.cs is a **complete and well-documented** implementation of the Z3 Probes API. All 14 probe-specific functions are correctly bound with proper signatures, comprehensive documentation, and appropriate error handling through the GetFunctionPointer mechanism.
