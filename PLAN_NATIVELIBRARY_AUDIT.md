# NativeLibrary Partial Classes Audit Plan

## Overview

Systematically audit all 35 NativeLibrary partial class files to:
1. Verify 100% completeness against Z3 C API
2. Add source header comments referencing Z3 header files
3. Document function counts and categorization
4. Identify any missing or extra functions
5. Generate comparison reports for each category

## Current Status

**Files Audited**: 26/35 (74.3%)
- ✅ NativeLibrary.Optimization.cs - 28/28 functions (100% complete)
- ✅ NativeLibrary.Goals.cs - 17/17 functions (100% complete)
- ✅ NativeLibrary.FloatingPoint.cs - 80/80 functions (100% complete)
- ✅ NativeLibrary.BitVectors.cs - 54/54 functions (100% complete)
- ✅ NativeLibrary.StringTheory.cs - 60/60 functions (100% complete)
- ✅ NativeLibrary.Queries.cs - 35/35 functions (100% complete)
- ✅ NativeLibrary.Expressions.cs - 30/30 functions (100% complete, absorbed CoreCreation)
- ✅ NativeLibrary.Tactics.cs - 21/21 functions (100% complete, Z3_mk_solver_from_tactic added to Solver.cs)
- ✅ NativeLibrary.AstCollections.cs - 21/21 functions (100% complete)
- ✅ NativeLibrary.Parameters.cs - 19/19 functions (100% complete)
- ✅ NativeLibrary.Datatypes.cs - 20/20 functions (100% complete)
- ✅ NativeLibrary.Predicates.cs - 18/18 functions (100% complete)
- ✅ NativeLibrary.Utilities.cs - 17/20 functions (85% complete, 3 string conversion functions missing)
- ✅ NativeLibrary.SolverExtensions.cs - 17/17 functions (100% complete)
- ✅ NativeLibrary.Probes.cs - 14/14 functions (100% complete)
- ✅ NativeLibrary.Solver.cs - 14/14 functions (100% complete, [report](COMPARISON_Solver.md))
- ✅ NativeLibrary.Sets.cs - 12/12 functions (100% complete)
- ✅ NativeLibrary.ReferenceCountingExtra.cs - 12/12 functions (100% complete)
- ✅ NativeLibrary.Parsing.cs - 10/10 functions (100% complete)
- ✅ NativeLibrary.Numerals.cs - 9/9 functions (100% complete, Z3_get_numeral_string in Model.cs, [report](COMPARISON_Numerals.md))
- ✅ NativeLibrary.Arrays.cs - 9/14 functions (64.3% complete, missing multi-dimensional array support)
- ✅ NativeLibrary.Statistics.cs - 9/9 functions (100% complete, 2 ref counting functions in ReferenceCountingExtra.cs, [report](COMPARISON_Statistics.md))
- ✅ NativeLibrary.Constraints.cs - 5/5 functions (100% complete)
- ✅ NativeLibrary.Simplify.cs - 4/4 functions (100% complete)
- ✅ NativeLibrary.Quantifiers.cs - 6/12 functions (50% complete, lambda expressions added, missing 2 generic const, 4 old-style)
- ✅ NativeLibrary.ErrorHandling.cs - 3/3 functions (100% complete for client use, Z3_set_error intentionally omitted, [report](COMPARISON_ErrorHandling.md))
- ✅ NativeLibrary.AlgebraicNumbers.cs - 2/22 functions (9.1% complete, ✅ FIXED return types, missing 20 z3_algebraic.h functions)

**Files Remaining**: 9/34 (26.5%) - CoreCreation eliminated

## File Audit Order

Files ordered by function count (largest first) to maximize impact:

### Priority 1: Large API Sections (10+ functions) - 18 files

| # | File | Functions | Z3 Header | Status | Notes |
|---|------|-----------|-----------|--------|-------|
| 1 | FloatingPoint | 80 | z3_fpa.h | ✅ **DONE** | IEEE 754 floating-point theory (100%, [report](COMPARISON_FloatingPoint.md)) |
| 2 | BitVectors | 54 | z3_api.h | ✅ **DONE** | Fixed-width binary arithmetic (100%, [report](COMPARISON_BitVectors.md)) |
| 3 | StringTheory | 60 | z3_api.h | ✅ **DONE** | String theory operations (100%, [report](COMPARISON_StringTheory.md)) |
| 4 | Queries | 35 | z3_api.h | ✅ **DONE** | AST introspection queries (100%, [report](COMPARISON_Queries.md)) |
| 5 | Optimization | 28 | z3_optimization.h | ✅ **DONE** | Optimization solver (100%) |
| 6 | Expressions | 30 | z3_api.h | ✅ **DONE** | Expression creation (100%, [report](COMPARISON_Expressions.md), absorbed CoreCreation) |
| 7 | Tactics | 21 | z3_api.h | ✅ **DONE** | Tactic-based solving (100%, [report](COMPARISON_Tactics.md), Z3_mk_solver_from_tactic added to Solver.cs) |
| 8 | AstCollections | 21 | z3_ast_containers.h | ✅ **DONE** | AST vectors and maps (100%, [report](COMPARISON_AstCollections.md)) |
| 9 | Parameters | 19 | z3_api.h | ✅ **DONE** | Parameter management (100%, [report](COMPARISON_Parameters.md)) |
| 10 | Datatypes | 20 | z3_api.h | ✅ **DONE** | Algebraic datatype definitions (100%, [report](COMPARISON_Datatypes.md)) |
| 11 | Predicates | 18 | z3_api.h | ✅ **DONE** | Type checking predicates (100%, [report](COMPARISON_Predicates.md)) |
| 12 | Utilities | 17 | z3_api.h | ✅ **DONE** | General utility functions (85%, [report](COMPARISON_Utilities.md), 3 string conversion functions missing) |
| 13 | SolverExtensions | 17 | z3_api.h | ✅ **DONE** | Advanced solver features (100%, [report](COMPARISON_SolverExtensions.md)) |
| 14 | Goals | 17 | z3_api.h | ✅ **DONE** | Goal-based solving (100%) |
| 15 | Probes | 14 | z3_api.h | ✅ **DONE** | Solver probes (100%, [report](COMPARISON_Probes.md)) |
| 16 | Sets | 12 | z3_api.h | ✅ **DONE** | Set theory operations (100%, [report](COMPARISON_Sets.md)) |
| 17 | ReferenceCountingExtra | 12 | z3_api.h | ✅ **DONE** | Extra ref counting utilities (100%, [report](COMPARISON_ReferenceCountingExtra.md)) |
| 18 | Model | 18 | z3_api.h | ✅ **DONE** | Model operations (100%, [report](COMPARISON_Model.md), 15 core + 4 helpers) |

### Priority 2: Medium API Sections (5-9 functions) - 8 files

| # | File | Functions | Z3 Header | Status | Notes |
|---|------|-----------|-----------|--------|-------|
| 19 | Parsing | 10 | z3_api.h | ✅ **DONE** | SMT-LIB parsing (100%, [report](COMPARISON_Parsing.md)) |
| 20 | Numerals | 9 | z3_api.h | ✅ **DONE** | Numeral extraction (90.9%, [report](COMPARISON_Numerals.md), missing Z3_get_numeral_string) |
| 21 | FunctionInterpretations | 9 | z3_api.h | ✅ **DONE** | Function interpretation extraction (100%, [report](COMPARISON_FunctionInterpretations.md)) |
| 22 | Arrays | 9 | z3_api.h | ✅ **DONE** | Array theory operations (64.3%, [report](COMPARISON_Arrays.md), missing 5 multi-dimensional functions) |
| 23 | Simplifiers | 8 | z3_api.h | ✅ **DONE** | Simplifier creation (72.7%, [report](COMPARISON_Simplifiers.md), missing 3 functions: 1 in Solver.cs, 2 discovery) |
| 24 | Statistics | 7 | z3_api.h | ✅ **DONE** | Solver statistics (77.8%, [report](COMPARISON_Statistics.md), missing 2 ref counting functions) |
| 25 | SpecialTheories | 5 | z3_api.h | ✅ **DONE** | Special relations and theories (41.7%, [report](COMPARISON_SpecialTheories.md), missing 7 functions: 4 order relations, 1 list sort, 2 recursive) |
| 26 | Solver | 13 | z3_api.h | ✅ **DONE** | Basic solver operations (92.9%, [report](COMPARISON_Solver.md), missing Z3_mk_solver_for_logic) |

### Priority 3: Small API Sections (2-4 functions) - 7 files

| # | File | Functions | Z3 Header | Status | Notes |
|---|------|-----------|-----------|--------|-------|
| ~~27~~ | ~~CoreCreation~~ | ~~3~~ | ~~z3_api.h~~ | ✅ **ELIMINATED** | Functions moved to Expressions.cs |
| 28 | Constraints | 5 | z3_api.h | ✅ **DONE** | Pseudo-Boolean and cardinality constraints (100%, [report](COMPARISON_Constraints.md)) |
| 29 | Simplify | 4 | z3_api.h | ✅ **DONE** | Simplification operations (100%, [report](COMPARISON_Simplify.md)) |
| 30 | Quantifiers | 6 | z3_api.h | ✅ **DONE** | Quantifier operations (50%, [report](COMPARISON_Quantifiers.md), ✅ lambda expressions added, missing 6: 2 generic const, 4 old-style) |
| 31 | Substitution | 3 | z3_api.h | ✅ **DONE** | Expression substitution (100%, [report](COMPARISON_Substitution.md)) |
| 32 | Functions | 3 | z3_api.h | ✅ **DONE** | Function declarations (71.4% overall, [report](COMPARISON_Functions.md), 3 in file + 3 in other files, missing 2 recursive functions) |
| 33 | Context | 8 | z3_api.h | ✅ **DONE** | Context management (80%, [report](COMPARISON_Context.md), missing Z3_interrupt and Z3_enable_concurrent_dec_ref) |

### Priority 4: Special Cases - 2 files

| # | File | Functions | Z3 Header | Status | Notes |
|---|------|-----------|-----------|--------|-------|
| 34 | AlgebraicNumbers | 2 | z3_api.h + z3_algebraic.h | ✅ **DONE** | Algebraic number bound approximations (9.1%, [report](COMPARISON_AlgebraicNumbers.md), ✅ return types FIXED, missing 20 z3_algebraic.h functions) |
| 35 | ErrorHandling | 3 | z3_api.h | ✅ **DONE** | Error handling (75%, [report](COMPARISON_ErrorHandling.md), missing Z3_set_error - internal use only) |

## Audit Process for Each File

For each NativeLibrary partial class file, perform the following steps:

### Step 1: Identify Z3 Header Source
- Determine which Z3 C API header file(s) the class maps to
- Primary candidates:
  - `z3_api.h` - Main API (most functions)
  - `z3_optimization.h` - Optimization solver
  - `z3_fpa.h` - Floating-point arithmetic
  - `z3_ast_containers.h` - AST collections
  - `z3_algebraic.h` - Algebraic numbers
  - `z3_fixedpoint.h` - Fixedpoint solver (if used)
  - `z3_polynomial.h` - Polynomial operations (if used)

### Step 2: Verify Function Completeness
- Extract all `LoadFunctionOrNull` calls from our file
- Search Z3 documentation/source for corresponding C API section
- Compare function lists:
  - ✅ Functions in both (mapped correctly)
  - ❌ Functions only in Z3 (missing from our implementation)
  - ⚠️ Functions only in ours (verify if deprecated or custom)
- Use Z3 C++ API docs as reference: https://z3prover.github.io/api/html/

### Step 3: Add Header Comment
Add standardized header to each file:

```csharp
// ReSharper disable IdentifierTypo
// ReSharper disable CommentTypo
// ReSharper disable StringLiteralTypo

// Z3 [Category] API - P/Invoke bindings for Z3 [description]
//
// Source: [header_file.h] from Z3 C API
// URL: https://github.com/Z3Prover/z3/blob/master/src/api/[header_file.h]
//
// This file provides bindings for Z3's [category] API ([N] functions):
// - [Major capability 1]
// - [Major capability 2]
// - [Major capability 3]
// - [Major capability N]

using System.Runtime.InteropServices;

namespace Spaceorc.Z3Wrap.Core.Interop;

internal sealed partial class NativeLibrary
{
    // ... existing code
}
```

### Step 4: Generate Comparison Report
Create a report file `COMPARISON_[Category].md` documenting:
- Total function count (ours vs Z3)
- Complete function mapping table
- Missing functions (if any)
- Extra functions (if any)
- Completeness percentage
- Recommendations for adding missing functions

### Step 5: Update Main Tracking
Update this plan file with:
- Status change (⏳ Pending → ✅ Done)
- Actual function count vs expected
- Link to comparison report
- Any issues or notes discovered

## Report Template

Each comparison report should follow this structure:

```markdown
# Z3 [Category] API Comparison Report

## Overview
**NativeLibrary.[Category].cs**: [N] functions
**Z3 C API ([header])**: [M] functions

## Complete Function Mapping

### ✅ Functions in Both ([X]/[M] in NativeLibrary match Z3 API)
[Table with columns: Our Method | Z3 C API | Z3 C++ Method | Purpose]

### ❌ Functions in Z3 but NOT in NativeLibrary
[List missing functions with Z3 signatures]

### ⚠️ Functions in NativeLibrary but NOT in Z3
[List extra functions - verify if deprecated]

## API Coverage Summary
| Metric | Count | Percentage |
|--------|-------|------------|
| Z3 C API Functions | [M] | 100% |
| Functions in NativeLibrary | [N] | [X/M%] |
| Missing Functions | [M-X] | [(M-X)/M%] |

## Function Categories
[Group functions by purpose]

## Completeness Assessment
[✅/⚠️/❌] Status with recommendations

## Verification
- Source: Z3 C API header
- Our implementation: Z3Wrap/Core/Interop/NativeLibrary.[Category].cs
```

## Key Questions to Answer for Each File

1. **Completeness**: Do we have all functions from the Z3 C API section?
2. **Correctness**: Are delegate signatures correct (parameter types, return types)?
3. **Documentation**: Are functions properly documented with XML comments?
4. **Organization**: Are functions logically grouped within the file?
5. **Source**: Which exact Z3 header file(s) does this map to?

## Special Considerations

### z3_api.h Sections
The main `z3_api.h` is massive (~4000+ lines) and contains multiple logical sections:
- Context management
- Sorts (types)
- Expressions
- Numerals
- Quantifiers
- Arrays
- BitVectors
- Goals
- Tactics
- Solvers
- Models
- etc.

We need to identify which section each of our files maps to within z3_api.h.

### Deprecated Functions
Some Z3 functions may be deprecated. We should:
- Note them in comparison reports
- Consider keeping them for backward compatibility
- Add `[Obsolete]` attributes if we expose them in Z3Library

### Version-Specific Functions
Some functions may be newer or removed in certain Z3 versions:
- Check Z3 version compatibility
- Note minimum Z3 version required
- Consider conditional loading for optional functions

## Success Criteria

Audit is complete when:
- ✅ All 35 files have header comments
- ✅ All 35 files have comparison reports
- ✅ Completeness percentage calculated for each
- ✅ Missing functions documented with recommendations
- ✅ All files verified to have correct source header references
- ✅ Summary report generated showing overall coverage

## Estimated Timeline

- **Priority 1** (18 files, large): 2-3 days (15-20 min per file)
- **Priority 2** (8 files, medium): 1 day (10-15 min per file)
- **Priority 3** (7 files, small): 0.5 days (5-10 min per file)
- **Priority 4** (2 files, special): 0.5 days (15-20 min per file)

**Total**: 4-5 days for complete audit

## Benefits

1. **Confidence**: Know exactly what Z3 features we support
2. **Documentation**: Clear mapping to Z3 C API for users
3. **Completeness**: Identify gaps in our coverage
4. **Maintenance**: Easy to verify when updating to new Z3 versions
5. **Quality**: Ensure all bindings are correct and complete

## Next Steps

1. Start with Priority 1, file #1 (FloatingPoint - 76 functions)
2. Complete audit following the process above
3. Generate comparison report
4. Update this tracking file
5. Move to next file
6. Repeat until all 35 files audited

---

**Status**: Plan created - Ready to begin systematic audit
**Started**: [Date to be filled when starting]
**Completed**: [Date to be filled when done]
