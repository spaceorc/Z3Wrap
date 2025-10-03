# Z3 Solver API Comparison Report

## Overview

**NativeLibrary.Solver.cs**: 13 functions
**Z3 C API (z3_api.h - Basic Solver Section)**: 14 functions

This file covers **basic solver operations** including solver creation, assertions, checking, and stack operations. Advanced solver features (assumptions, cores, proofs, etc.) are in `NativeLibrary.SolverExtensions.cs`.

## File Organization

The Z3 Solver API is split across multiple files in our implementation:
- **NativeLibrary.Solver.cs**: 13 functions - Basic solver creation, assertions, checking, stack operations
- **NativeLibrary.SolverExtensions.cs**: 17 functions - Advanced features (tracking, assumptions, cores, proofs, etc.)
- **Not Implemented**: 22 functions - User propagation API (experimental, callback-based)

## Complete Function Mapping

### ✅ Functions in NativeLibrary.Solver.cs (13/14 basic operations - 92.9%)

| Our Method | Z3 C API | Z3 C++ Method | Purpose |
|------------|----------|---------------|---------|
| MkSolver | Z3_mk_solver | solver::solver() | Create general solver with full Z3 capabilities |
| MkSimpleSolver | Z3_mk_simple_solver | - | Create simple solver (equivalent to "smt" tactic) |
| MkSolverFromTactic | Z3_mk_solver_from_tactic | solver(tactic) | Create solver from tactic |
| SolverIncRef | Z3_solver_inc_ref | - | Increment solver reference count for memory management |
| SolverDecRef | Z3_solver_dec_ref | - | Decrement solver reference count |
| SolverAssert | Z3_solver_assert | solver::add() | Assert Boolean constraint to solver |
| SolverCheck | Z3_solver_check | solver::check() | Check satisfiability of asserted constraints |
| SolverPush | Z3_solver_push | solver::push() | Push backtracking point onto assertion stack |
| SolverPop | Z3_solver_pop | solver::pop() | Pop backtracking points from assertion stack |
| SolverReset | Z3_solver_reset | solver::reset() | Reset solver by clearing all assertions |
| SolverGetModel | Z3_solver_get_model | solver::get_model() | Get satisfying model after successful check |
| SolverGetReasonUnknown | Z3_solver_get_reason_unknown | solver::reason_unknown() | Get reason why solver returned unknown result |
| SolverSetParams | Z3_solver_set_params | solver::set() | Set parameters on solver |

### ❌ Functions in Z3 C API but NOT in NativeLibrary.Solver.cs (1/14 - 7.1%)

| Z3 C API | Z3 C++ Method | Purpose | Priority |
|----------|---------------|---------|----------|
| Z3_mk_solver_for_logic | solver(logic) | Create solver customized for specific logic (QF_LIA, QF_BV, etc.) | Medium |

## API Coverage Summary

| Metric | Count | Percentage |
|--------|-------|------------|
| Z3 C API Functions (Basic Solver) | 14 | 100% |
| Functions in NativeLibrary.Solver.cs | 13 | 92.9% |
| Missing Functions | 1 | 7.1% |

## Function Categories

### Solver Creation (4 functions - 3/4 implemented - 75%)
- ✅ **Z3_mk_solver**: General solver with full capabilities
- ✅ **Z3_mk_simple_solver**: Simple incremental solver (equivalent to "smt" tactic)
- ✅ **Z3_mk_solver_from_tactic**: Solver from custom tactic
- ❌ **Z3_mk_solver_for_logic**: Solver customized for specific logic

### Reference Counting (2 functions - 2/2 implemented - 100%)
- ✅ **Z3_solver_inc_ref**: Increment reference count
- ✅ **Z3_solver_dec_ref**: Decrement reference count

### Assertions and Checking (2 functions - 2/2 implemented - 100%)
- ✅ **Z3_solver_assert**: Add Boolean constraint
- ✅ **Z3_solver_check**: Check satisfiability

### Backtracking Stack (2 functions - 2/2 implemented - 100%)
- ✅ **Z3_solver_push**: Create backtracking point
- ✅ **Z3_solver_pop**: Remove backtracking points

### Results and State (3 functions - 3/3 implemented - 100%)
- ✅ **Z3_solver_get_model**: Extract satisfying model
- ✅ **Z3_solver_get_reason_unknown**: Get reason for unknown result
- ✅ **Z3_solver_reset**: Clear all assertions

### Configuration (1 function - 1/1 implemented - 100%)
- ✅ **Z3_solver_set_params**: Set solver parameters

## Missing Function Details

### Z3_mk_solver_for_logic

**Signature**: `Z3_solver Z3_API Z3_mk_solver_for_logic(Z3_context c, Z3_symbol logic)`

**Purpose**: Creates a solver customized for a specific logic (e.g., QF_LIA, QF_BV, QF_UFLIA, etc.). This allows Z3 to use specialized tactics optimized for that logic.

**Benefits**:
- **Performance**: Significantly faster for specialized logics
- **Completeness**: Logic-specific decision procedures
- **Optimization**: Tailored preprocessing and strategies

**Common Logics**:
- `QF_LIA` - Quantifier-free linear integer arithmetic
- `QF_LRA` - Quantifier-free linear real arithmetic
- `QF_BV` - Quantifier-free bit-vectors
- `QF_UFLIA` - Quantifier-free uninterpreted functions + linear integer arithmetic
- `QF_AUFLIA` - Quantifier-free arrays + uninterpreted functions + linear integer arithmetic
- `QF_NIA` - Quantifier-free nonlinear integer arithmetic
- `QF_NRA` - Quantifier-free nonlinear real arithmetic

**Workaround**: Currently users must use `Z3_mk_solver()` (general solver) which attempts to guess the logic.

**Implementation Notes**:
1. Add to LoadFunctionsSolver: `LoadFunctionOrNull(handle, functionPointers, "Z3_mk_solver_for_logic");`
2. Add delegate: `private delegate IntPtr MkSolverForLogicDelegate(IntPtr ctx, IntPtr logic);`
3. Add method:
```csharp
internal IntPtr MkSolverForLogic(IntPtr ctx, IntPtr logic)
{
    var funcPtr = GetFunctionPointer("Z3_mk_solver_for_logic");
    var func = Marshal.GetDelegateForFunctionPointer<MkSolverForLogicDelegate>(funcPtr);
    return func(ctx, logic);
}
```
4. Add high-level wrapper in Z3Context or Z3Solver

**Priority**: Medium - Performance optimization, but general solver works for all cases

## Completeness Assessment

### ✅ Core Solver Operations: 92.9% Complete

**NativeLibrary.Solver.cs** contains 13 of 14 basic solver functions from the Z3 C API. All essential operations are present:
- Solver creation (3/4 variants)
- Reference counting (2/2)
- Assertions and checking (2/2)
- Backtracking (2/2)
- Result extraction (2/2)
- Configuration (1/1)
- State management (1/1)

### ⚠️ Missing Logic-Specific Solver Creation

The only missing function is **Z3_mk_solver_for_logic**, which provides performance optimizations for known logics. The general solver (Z3_mk_solver) can handle all cases, so this is not a blocking issue.

### Recommendations

#### Priority 1: Add Z3_mk_solver_for_logic (Medium Priority)

**Benefits**:
- Significant performance improvements for specialized logics
- Better integration with SMT-LIB2 standards
- More predictable behavior for known problem classes

**Implementation Steps**:
1. Add P/Invoke binding in NativeLibrary.Solver.cs
2. Add high-level method to Z3Context: `CreateSolverForLogic(string logic)`
3. Add enum or constants for common logics (QF_LIA, QF_BV, etc.)
4. Add tests demonstrating performance benefits
5. Update documentation with logic selection guidance

**Example Usage** (after implementation):
```csharp
// Create solver optimized for bit-vector logic
using var solver = context.CreateSolverForLogic("QF_BV");

// Create solver optimized for linear integer arithmetic
using var solver = context.CreateSolverForLogic("QF_LIA");
```

#### Priority 2: Document Logic Selection (Low Priority)

Even without Z3_mk_solver_for_logic, we could:
1. Document how Z3 automatically detects logic
2. Provide guidance on problem formulation for best performance
3. Explain when logic-specific solvers matter most

## Verification

- **Source**: Z3 C API header (z3_api.h)
  - URL: https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h
  - Section: Solver API (basic solver creation and operations)
  - Functions: 14 basic solver functions

- **Our Implementation**:
  - File: Z3Wrap/Core/Interop/NativeLibrary.Solver.cs
  - Functions: 13 of 14 implemented
  - Loading: 7 LoadFunctionInternal (critical), 6 LoadFunctionOrNull (optional)

- **Verification Method**:
  - Cross-referenced with Z3 C++ API documentation
  - Compared with existing COMPARISON_SolverExtensions.md report
  - Verified function signatures and purposes

## Relationship to Other Files

### NativeLibrary.SolverExtensions.cs (17 functions - 100% complete)
Contains advanced solver features:
- Assumption-based checking
- Unsat cores and proof extraction
- SMT-LIB2 I/O
- Statistics and diagnostics
- Context translation
- Cubes and consequences
- Solver interruption

See: [COMPARISON_SolverExtensions.md](COMPARISON_SolverExtensions.md)

### Combined Coverage
| Category | Functions | Implemented | Percentage |
|----------|-----------|-------------|------------|
| Basic Solver (Solver.cs) | 14 | 13 | 92.9% |
| Advanced Extensions (SolverExtensions.cs) | 17 | 17 | 100% |
| **Total Core Solver API** | **31** | **30** | **96.8%** |

## Conclusion

**NativeLibrary.Solver.cs is 92.9% complete** for basic solver operations.

The file correctly implements 13 of 14 basic solver functions including:
- Three solver creation variants (general, simple, from-tactic)
- Complete reference counting
- All assertion and checking operations
- Full backtracking support
- Model extraction
- Parameter configuration
- State management

The only missing function is **Z3_mk_solver_for_logic**, which provides performance optimizations for specific logics. The general solver can handle all cases, making this a performance optimization rather than a functional gap.

**Combined with NativeLibrary.SolverExtensions.cs**, we achieve **96.8% coverage** (30/31 functions) of the core Z3 Solver API.

**Status**: ✅ **PRODUCTION READY** - All essential solver operations fully implemented and documented. Adding Z3_mk_solver_for_logic would provide performance benefits for specialized use cases.
