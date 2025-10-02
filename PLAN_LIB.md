# Z3Library Refactoring Plan - Partial Classes

## Overview

Refactor the monolithic `Z3Library.cs` (1174 lines, 120 public methods) into organized partial classes following the `NativeLibrary` structure. This improves maintainability and makes it easier to add new Z3 API wrappers (like Optimization).

## Current Structure

**File**: `Z3Wrap/Core/Z3Library.cs`
- **Size**: 1174 lines
- **Methods**: 120 public methods
- **Pattern**: All methods follow `nativeLibrary.X() → CheckError() → return` pattern

**Sections** (by comment markers):
1. Configuration and context methods
2. Reference counting
3. Sort creation
4. Expression creation
5. Boolean operations
6. Arithmetic operations
7. Comparison operations
8. Type conversions
9. Quantifier operations
10. Function declaration and application operations
11. Solver operations
12. Model operations
13. Array operations
14. BitVector operations
15. BitVector functions
16. BitVector overflow checks
17. Parameters operations

## Target Structure

Create partial classes matching `NativeLibrary` organization:

```
Z3Wrap/Core/
├── Z3Library.cs                    # Main class (constructors, disposal, error handling)
├── Z3Library.Context.cs            # Context and configuration
├── Z3Library.Sorts.cs              # Sort creation (bool, int, real, bv, array)
├── Z3Library.Expressions.cs        # Expression creation (const, numeral, true/false)
├── Z3Library.Logic.cs              # Boolean operations (and, or, not, implies, iff, xor, ite)
├── Z3Library.Arithmetic.cs         # Arithmetic operations (add, sub, mul, div, mod, unary minus)
├── Z3Library.Comparisons.cs        # Comparison operations (eq, lt, le, gt, ge)
├── Z3Library.Conversions.cs        # Type conversions (int2real, real2int, int2bv, bv2int)
├── Z3Library.Quantifiers.cs        # Quantifier operations (forall, exists, pattern)
├── Z3Library.Functions.cs          # Function declarations and application
├── Z3Library.Solver.cs             # Solver operations (create, assert, check, push/pop, model)
├── Z3Library.Model.cs              # Model operations (eval, to string, get values)
├── Z3Library.Arrays.cs             # Array operations (select, store, const array)
├── Z3Library.BitVectors.cs         # BitVector operations (arithmetic, bitwise, shifts, comparisons)
├── Z3Library.Parameters.cs         # Parameter operations (create, set, ref counting)
└── Z3Library.Optimization.cs       # Optimization operations (NEW - to be added)
```

## Refactoring Approach

### Tool: `refactor_z3library.py`

Already created and tested. Usage:
```bash
python refactor_z3library.py --category CategoryName --methods Method1 Method2 Method3
```

**Features**:
- Extracts methods with XML documentation
- Removes from source file
- Creates properly formatted partial class
- Dry-run mode for testing
- Validates extraction success

### Execution Strategy

**Phase by Phase** to maintain working state:

1. Extract one category at a time
2. Run `make build` after each extraction
3. Run `make test` to verify
4. Commit if tests pass
5. If tests fail, `git checkout Z3Wrap/Core/Z3Library*.cs` and retry

## Detailed Refactoring Plan

### Phase 1: Core Infrastructure (Keep in Main File)

**File**: `Z3Library.cs` (main partial class)

**Keep**:
- Class declaration and fields (`nativeLibrary`, `disposed`)
- Constructors (private constructor)
- Static factory methods (`Load`, `LoadAuto`)
- Disposal (`Dispose`, finalizer)
- Error handling (`CheckHandle`, `CheckError`, `OnZ3ErrorSafe`)
- Properties (`LibraryPath`)

**Lines to keep**: ~85 lines

---

### Phase 2: Context and Configuration

**File**: `Z3Library.Context.cs`

**Methods to extract** (6 methods):
```
MkConfig
DelConfig
SetParamValue
MkContextRc
DelContext
UpdateParamValue
```

**Command**:
```bash
python refactor_z3library.py --category Context \
    --methods MkConfig DelConfig SetParamValue MkContextRc DelContext UpdateParamValue
```

---

### Phase 3: Reference Counting and Sorts

**File**: `Z3Library.Sorts.cs`

**Methods to extract** (7 methods):
```
IncRef
DecRef
MkBoolSort
MkIntSort
MkRealSort
MkBvSort
MkArraySort
```

**Command**:
```bash
python refactor_z3library.py --category Sorts \
    --methods IncRef DecRef MkBoolSort MkIntSort MkRealSort MkBvSort MkArraySort
```

**Note**: IncRef/DecRef are general-purpose but used primarily with expressions/sorts

---

### Phase 4: Expression Creation

**File**: `Z3Library.Expressions.cs`

**Methods to extract** (3 methods):
```
MkConst
MkTrue
MkFalse
MkNumeral
```

**Command**:
```bash
python refactor_z3library.py --category Expressions \
    --methods MkConst MkTrue MkFalse MkNumeral
```

---

### Phase 5: Boolean Logic

**File**: `Z3Library.Logic.cs`

**Methods to extract** (7 methods):
```
MkAnd
MkOr
MkNot
MkImplies
MkIff
MkXor
MkIte
```

**Command**:
```bash
python refactor_z3library.py --category Logic \
    --methods MkAnd MkOr MkNot MkImplies MkIff MkXor MkIte
```

---

### Phase 6: Arithmetic Operations

**File**: `Z3Library.Arithmetic.cs`

**Methods to extract** (6 methods):
```
MkAdd
MkSub
MkMul
MkDiv
MkMod
MkUnaryMinus
```

**Command**:
```bash
python refactor_z3library.py --category Arithmetic \
    --methods MkAdd MkSub MkMul MkDiv MkMod MkUnaryMinus
```

---

### Phase 7: Comparison Operations

**File**: `Z3Library.Comparisons.cs`

**Methods to extract** (5 methods):
```
MkEq
MkLt
MkLe
MkGt
MkGe
```

**Command**:
```bash
python refactor_z3library.py --category Comparisons \
    --methods MkEq MkLt MkLe MkGt MkGe
```

---

### Phase 8: Type Conversions

**File**: `Z3Library.Conversions.cs`

**Methods to extract** (4 methods):
```
MkInt2Real
MkReal2Int
MkInt2Bv
MkBv2Int
```

**Command**:
```bash
python refactor_z3library.py --category Conversions \
    --methods MkInt2Real MkReal2Int MkInt2Bv MkBv2Int
```

---

### Phase 9: Quantifiers

**File**: `Z3Library.Quantifiers.cs`

**Methods to extract** (3 methods):
```
MkForallConst
MkExistsConst
MkPattern
```

**Command**:
```bash
python refactor_z3library.py --category Quantifiers \
    --methods MkForallConst MkExistsConst MkPattern
```

---

### Phase 10: Function Declarations

**File**: `Z3Library.Functions.cs`

**Methods to extract** (2 methods):
```
MkFuncDecl
MkApp
```

**Command**:
```bash
python refactor_z3library.py --category Functions \
    --methods MkFuncDecl MkApp
```

---

### Phase 11: Solver Operations

**File**: `Z3Library.Solver.cs`

**Methods to extract** (9 methods):
```
MkSolver
MkSimpleSolver
SolverIncRef
SolverDecRef
SolverAssert
SolverCheck
SolverPush
SolverPop
SolverGetModel
SolverGetReasonUnknown
SolverReset
SolverSetParams
```

**Command**:
```bash
python refactor_z3library.py --category Solver \
    --methods MkSolver MkSimpleSolver SolverIncRef SolverDecRef SolverAssert \
              SolverCheck SolverPush SolverPop SolverGetModel SolverGetReasonUnknown \
              SolverReset SolverSetParams
```

---

### Phase 12: Model Operations

**File**: `Z3Library.Model.cs`

**Methods to extract** (8 methods):
```
ModelIncRef
ModelDecRef
ModelToString
AstToString
ModelEval
GetNumeralString
GetBoolValue
IsNumeralAst
GetSort
GetSortKind
```

**Command**:
```bash
python refactor_z3library.py --category Model \
    --methods ModelIncRef ModelDecRef ModelToString AstToString ModelEval \
              GetNumeralString GetBoolValue IsNumeralAst GetSort GetSortKind
```

---

### Phase 13: Array Operations

**File**: `Z3Library.Arrays.cs`

**Methods to extract** (5 methods):
```
MkConstArray
MkStore
MkSelect
GetArraySortDomain
GetArraySortRange
```

**Command**:
```bash
python refactor_z3library.py --category Arrays \
    --methods MkConstArray MkStore MkSelect GetArraySortDomain GetArraySortRange
```

---

### Phase 14: BitVector Operations

**File**: `Z3Library.BitVectors.cs`

**Methods to extract** (31 methods):
```
# Arithmetic
MkBvAdd MkBvSub MkBvMul MkBvSDiv MkBvUDiv MkBvSRem MkBvURem MkBvSMod MkBvNeg

# Bitwise
MkBvAnd MkBvOr MkBvXor MkBvNot

# Shifts
MkBvShl MkBvAShr MkBvLShr

# Comparisons
MkBvSLt MkBvULt MkBvSLe MkBvULe MkBvSGt MkBvUGt MkBvSGe MkBvUGe

# Functions
MkSignExt MkZeroExt MkExtract MkRepeat GetBvSortSize

# Overflow checks
MkBvAddNoOverflow MkBvSubNoOverflow MkBvSubNoUnderflow MkBvMulNoOverflow
MkBvMulNoUnderflow MkBvAddNoUnderflow MkBvSDivNoOverflow MkBvNegNoOverflow
```

**Command**:
```bash
python refactor_z3library.py --category BitVectors \
    --methods MkBvAdd MkBvSub MkBvMul MkBvSDiv MkBvUDiv MkBvSRem MkBvURem MkBvSMod MkBvNeg \
              MkBvAnd MkBvOr MkBvXor MkBvNot \
              MkBvShl MkBvAShr MkBvLShr \
              MkBvSLt MkBvULt MkBvSLe MkBvULe MkBvSGt MkBvUGt MkBvSGe MkBvUGe \
              MkSignExt MkZeroExt MkExtract MkRepeat GetBvSortSize \
              MkBvAddNoOverflow MkBvSubNoOverflow MkBvSubNoUnderflow MkBvMulNoOverflow \
              MkBvMulNoUnderflow MkBvAddNoUnderflow MkBvSDivNoOverflow MkBvNegNoOverflow
```

---

### Phase 15: Parameters

**File**: `Z3Library.Parameters.cs`

**Methods to extract** (6 methods):
```
MkParams
ParamsIncRef
ParamsDecRef
ParamsSetBool
ParamsSetUInt
ParamsSetDouble
ParamsSetSymbol
ParamsToString
```

**Command**:
```bash
python refactor_z3library.py --category Parameters \
    --methods MkParams ParamsIncRef ParamsDecRef ParamsSetBool ParamsSetUInt \
              ParamsSetDouble ParamsSetSymbol ParamsToString
```

---

### Phase 16: Optimization (NEW - To Be Added)

**File**: `Z3Library.Optimization.cs`

**Methods to add** (13 new methods from PLAN_OPT.md):
```
MkOptimize
OptimizeIncRef
OptimizeDecRef
OptimizeAssert
OptimizeMaximize
OptimizeMinimize
OptimizeCheck
OptimizeGetModel
OptimizeGetUpper
OptimizeGetLower
OptimizeSetParams
OptimizeToString
OptimizeGetReasonUnknown
```

**Note**: This will be added manually after refactoring is complete, following the same pattern.

---

## Verification Process

After **each phase**:

```bash
# 1. Build
make build

# Expected: 0 warnings, 0 errors

# 2. Test
make test

# Expected: Passed: 903, Failed: 0

# 3. If successful, commit
git add Z3Wrap/Core/Z3Library*.cs
git commit -m "refactor: extract [Category] methods into Z3Library.[Category].cs"

# 4. If failed, revert and investigate
git checkout Z3Wrap/Core/Z3Library*.cs
```

## Final Structure Summary

After refactoring:

| File | Methods | Lines | Category |
|------|---------|-------|----------|
| Z3Library.cs | Core (5) | ~85 | Infrastructure |
| Z3Library.Context.cs | 6 | ~120 | Configuration |
| Z3Library.Sorts.cs | 7 | ~140 | Sort creation |
| Z3Library.Expressions.cs | 4 | ~80 | Expression creation |
| Z3Library.Logic.cs | 7 | ~140 | Boolean logic |
| Z3Library.Arithmetic.cs | 6 | ~120 | Arithmetic ops |
| Z3Library.Comparisons.cs | 5 | ~100 | Comparisons |
| Z3Library.Conversions.cs | 4 | ~80 | Type conversions |
| Z3Library.Quantifiers.cs | 3 | ~60 | Quantifiers |
| Z3Library.Functions.cs | 2 | ~40 | Function decls |
| Z3Library.Solver.cs | 12 | ~240 | Solver ops |
| Z3Library.Model.cs | 10 | ~200 | Model ops |
| Z3Library.Arrays.cs | 5 | ~100 | Array theory |
| Z3Library.BitVectors.cs | 31 | ~620 | BitVector theory |
| Z3Library.Parameters.cs | 8 | ~160 | Parameters |
| **Total** | **120** | **~2285** | **(15 files)** |

**After adding Optimization** (Phase 16):
| Z3Library.Optimization.cs | 13 | ~260 | Optimization |
| **Total** | **133** | **~2545** | **(16 files)** |

## Benefits

1. **Maintainability**: Each file focused on single concern
2. **Discoverability**: Easy to find related methods
3. **Extensibility**: Clear where to add new Z3 API wrappers
4. **Consistency**: Mirrors NativeLibrary structure
5. **Readability**: Smaller files easier to review
6. **Organization**: Logical grouping of related functionality

## Risks & Mitigation

| Risk | Impact | Mitigation |
|------|--------|------------|
| Build breaks | High | Test after each phase, easy rollback |
| Tests fail | High | Incremental approach, verify each step |
| Method extraction errors | Medium | Dry-run mode, review diffs |
| Missing methods | Low | Script reports missing methods |
| Merge conflicts | Low | Single-developer, linear process |

## Timeline

**Estimated**: 1-2 hours (automated + verification)

- Phase 1 (Core): Manual review, 5 minutes
- Phases 2-15 (Extraction): 5 minutes each × 14 = 70 minutes
- Phase 16 (Optimization): Add manually after, 15 minutes
- Final verification: 10 minutes

**Total**: ~90-100 minutes

## Next Steps

1. ✅ Review this plan
2. Execute Phase 1 (identify core infrastructure to keep)
3. Execute Phases 2-15 (automated extraction)
4. Add Phase 16 (Optimization) manually using established pattern
5. Update PLAN_OPT.md with correct file location
6. Final `make ci` verification
7. Commit refactored structure

---

## Script Usage Reference

```bash
# Test extraction (dry-run)
python refactor_z3library.py --category CategoryName --methods Method1 Method2 --dry-run

# Actual extraction
python refactor_z3library.py --category CategoryName --methods Method1 Method2

# Verify
make build
make test

# Commit or revert
git add Z3Wrap/Core/Z3Library*.cs
git commit -m "refactor: extract CategoryName into partial class"
# OR
git checkout Z3Wrap/Core/Z3Library*.cs  # if failed
```

This plan provides systematic, safe, incremental refactoring with verification at each step.
