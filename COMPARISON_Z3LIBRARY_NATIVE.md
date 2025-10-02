# Z3Library vs NativeLibrary Organization Comparison

## Overview

**NativeLibrary**: 35 partial classes covering **complete Z3 C API** (all native functions)
**Z3Library**: 14 partial classes covering **user-facing safe wrapper API** (subset of native functions)

## Category Comparison

### ✅ Categories in Both (9)

These categories exist in both libraries with similar purposes:

| Category | NativeLibrary | Z3Library | Notes |
|----------|---------------|-----------|-------|
| **Arrays** | ✅ | ✅ | Array theory operations |
| **BitVectors** | ✅ | ✅ | BitVector theory operations |
| **Context** | ✅ | ✅ | Context management |
| **Expressions** | ✅ | ✅ | Expression creation (but different scope) |
| **Functions** | ✅ | ✅ | Function declarations |
| **Model** | ✅ | ✅ | Model operations |
| **Parameters** | ✅ | ✅ | Parameter management |
| **Quantifiers** | ✅ | ✅ | Quantifier operations |
| **Solver** | ✅ | ✅ | Solver operations |

---

## 🔀 Categories Only in Z3Library (5)

These are **higher-level logical groupings** that don't exist in NativeLibrary:

| Category | Methods | Reason for Existence |
|----------|---------|---------------------|
| **Arithmetic** | 6 | Groups `MkAdd`, `MkSub`, `MkMul`, `MkDiv`, `MkMod`, `MkUnaryMinus` |
| **Comparisons** | 5 | Groups `MkEq`, `MkLt`, `MkLe`, `MkGt`, `MkGe` |
| **Conversions** | 4 | Groups `MkInt2Real`, `MkReal2Int`, `MkInt2Bv`, `MkBv2Int` |
| **Logic** | 7 | Groups `MkAnd`, `MkOr`, `MkNot`, `MkImplies`, `MkIff`, `MkXor`, `MkIte` |
| **Sorts** | 7 | Groups `MkBoolSort`, `MkIntSort`, `MkRealSort`, `MkBvSort`, `MkArraySort`, `IncRef`, `DecRef` |

**Rationale**: Z3Library splits NativeLibrary's large "Expressions" category into multiple focused categories (Arithmetic, Comparisons, Logic, etc.) for better organization.

---

## 🔥 Categories Only in NativeLibrary (27)

These exist in NativeLibrary but **NOT yet in Z3Library** (not exposed in public wrapper):

### Missing because Z3Library doesn't expose them yet:

| Category | Purpose | Impact |
|----------|---------|--------|
| **AlgebraicNumbers** | Algebraic number operations | Not exposed |
| **AstCollections** | AST vector/list operations | Internal use |
| **Constraints** | Constraint management | Not exposed |
| **CoreCreation** | Low-level Z3 core creation | Internal use |
| **Datatypes** | Algebraic datatype definitions | ⚠️ Missing (see ANALYSIS.md) |
| **ErrorHandling** | Error code management | Internal use |
| **FloatingPoint** | IEEE 754 floating-point | ⚠️ Missing (see ANALYSIS.md) |
| **FunctionInterpretations** | Function interpretation extraction | Not exposed |
| **Goals** | Goal management for tactics | Not exposed |
| **Numerals** | Numeral parsing/extraction | Partially exposed (GetNumeralString) |
| **Optimization** | Optimization solver | 🎯 **TO BE ADDED** (PLAN_OPT.md) |
| **Parsing** | SMTLIB parsing | Not exposed |
| **Predicates** | Predicate checking (is_int, is_numeral) | Partially exposed |
| **Probes** | Solver probes | Not exposed |
| **Queries** | AST introspection queries | Not exposed |
| **ReferenceCountingExtra** | Extra ref counting utilities | Internal use |
| **Sets** | Set theory operations | ⚠️ Missing (see ANALYSIS.md) |
| **Simplifiers** | Simplifier creation | Not exposed |
| **Simplify** | Simplification operations | Not exposed |
| **SolverExtensions** | Advanced solver features | Not exposed |
| **SpecialTheories** | Special theory operations | Not exposed |
| **Statistics** | Solver statistics | Not exposed |
| **StringTheory** | String theory operations | ⚠️ Missing (see ANALYSIS.md) |
| **Substitution** | Expression substitution | Not exposed |
| **Tactics** | Solving tactics | Not exposed |
| **Utilities** | Utility functions | Internal use |

---

## Key Organizational Differences

### 1. **Granularity**

**NativeLibrary**: Fine-grained, Z3 C API-centric organization
- Organized by Z3's internal API categories
- 35 partial classes for complete API coverage
- Example: All expression creation in one file (Expressions.cs)

**Z3Library**: Coarse-grained, user-centric organization
- Organized by logical operation types
- 14 partial classes for user-facing API
- Example: Expression creation split into Expressions, Arithmetic, Logic, Comparisons, etc.

### 2. **Scope**

| Aspect | NativeLibrary | Z3Library |
|--------|---------------|-----------|
| **Methods** | ~427 functions | ~116 functions (27% of native) |
| **Coverage** | 100% of Z3 C API | ~70% of commonly-used Z3 features |
| **Purpose** | Complete low-level API | Safe, user-friendly wrapper |
| **Error Handling** | Raw P/Invoke | CheckError/CheckHandle wrappers |

### 3. **Organization Philosophy**

**NativeLibrary**: Mirrors Z3's C API structure exactly
```
NativeLibrary.Expressions.cs:
  - Sorts (MkBoolSort, MkIntSort, MkRealSort)
  - Constants (MkConst, MkTrue, MkFalse)
  - Logic (MkAnd, MkOr, MkNot, MkImplies)
  - Arithmetic (MkAdd, MkSub, MkMul)
  - Comparisons (MkEq, MkLt, MkLe)
  (Everything in one 800+ line file)
```

**Z3Library**: Splits by functional area for usability
```
Z3Library.Sorts.cs: MkBoolSort, MkIntSort, MkRealSort, MkBvSort, MkArraySort
Z3Library.Expressions.cs: MkConst, MkTrue, MkFalse, MkNumeral
Z3Library.Logic.cs: MkAnd, MkOr, MkNot, MkImplies, MkIff, MkXor, MkIte
Z3Library.Arithmetic.cs: MkAdd, MkSub, MkMul, MkDiv, MkMod, MkUnaryMinus
Z3Library.Comparisons.cs: MkEq, MkLt, MkLe, MkGt, MkGe
(Split into 5 focused files)
```

---

## Specific Method Mapping Examples

### Example 1: Expression Creation Split

**NativeLibrary.Expressions.cs** contains:
- Sorts: `MkBoolSort`, `MkIntSort`, `MkRealSort`
- Constants: `MkConst`, `MkTrue`, `MkFalse`
- Logic: `MkAnd`, `MkOr`, `MkNot`, `MkImplies`, `MkIff`, `MkXor`
- Arithmetic: `MkAdd`, `MkSub`, `MkMul`, `MkDiv`, `MkMod`, `MkUnaryMinus`
- Comparisons: `MkEq`, `MkLt`, `MkLe`, `MkGt`, `MkGe`
- Conversions: `MkInt2Real`, `MkReal2Int`
- Quantifiers: `MkForall`, `MkExists`
- Arrays: `MkSelect`, `MkStore`

**Z3Library splits this into 8 files**:
1. `Z3Library.Sorts.cs` - Sorts
2. `Z3Library.Expressions.cs` - Constants only
3. `Z3Library.Logic.cs` - Logic operations
4. `Z3Library.Arithmetic.cs` - Arithmetic operations
5. `Z3Library.Comparisons.cs` - Comparison operations
6. `Z3Library.Conversions.cs` - Type conversions
7. `Z3Library.Quantifiers.cs` - Quantifiers
8. `Z3Library.Arrays.cs` - Array operations

### Example 2: Numerals Split

**NativeLibrary.Numerals.cs** (10 methods):
- `GetNumeralBinaryString`
- `GetNumeralDecimalString`
- `GetNumeralInt`
- `GetNumeralUint`
- `GetNumeralInt64`
- `GetNumeralUint64`
- `GetNumeralRationalInt64`
- `GetNumeralSmall`
- `GetNumeralDouble`
- `MkNumeral`

**Z3Library splits across 2 files**:
- `Z3Library.Expressions.cs`: `MkNumeral` (creation)
- `Z3Library.Model.cs`: `GetNumeralString` (single string method, others not exposed)

---

## Recommendations

### ✅ Current Z3Library Organization is Good

**Strengths**:
1. **User-focused**: Logical grouping by operation type (Arithmetic, Logic, Comparisons)
2. **Maintainable**: Smaller files (avg 92 lines vs NativeLibrary's avg 200+ lines)
3. **Discoverable**: Easy to find related operations
4. **Extensible**: Clear where to add new methods

### 🔧 Considerations for Future

1. **Add missing categories as needed**:
   - `Z3Library.Optimization.cs` (planned - PLAN_OPT.md)
   - `Z3Library.StringTheory.cs` (future - ANALYSIS.md Phase 2)
   - `Z3Library.Datatypes.cs` (future - ANALYSIS.md Phase 2)
   - `Z3Library.FloatingPoint.cs` (future - ANALYSIS.md Phase 3)

2. **Keep current split for Expressions**:
   - Don't merge Arithmetic/Logic/Comparisons back into Expressions
   - This split improves usability even though NativeLibrary doesn't have it

3. **Consider future splits**:
   - `Z3Library.Solver.cs` (113 lines) might benefit from:
     - `Z3Library.Solver.cs` (basic operations)
     - `Z3Library.SolverAdvanced.cs` (advanced features like unsat cores, tactics)

4. **Maintain consistency**:
   - Continue using NativeLibrary category names where they match
   - Use descriptive names for Z3Library-specific splits (Arithmetic, Logic, etc.)

---

## Summary

| Aspect | NativeLibrary | Z3Library |
|--------|---------------|-----------|
| **Partial Classes** | 35 | 14 |
| **Organization** | Z3 C API structure | User operation types |
| **Methods** | ~427 | ~116 (27%) |
| **Average File Size** | 200+ lines | 92 lines |
| **Philosophy** | Complete API mirror | Usable safe wrapper |
| **Granularity** | Fine (API-centric) | Coarse (user-centric) |

**Key Difference**: NativeLibrary is exhaustive and API-centric (35 files), while Z3Library is selective and user-centric (14 files). Z3Library's split of "Expressions" into Arithmetic/Logic/Comparisons/etc. is a **positive divergence** that improves usability.
