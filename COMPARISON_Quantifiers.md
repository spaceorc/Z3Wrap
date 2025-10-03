# Z3 Quantifiers API Comparison Report

## Overview
**NativeLibrary.Quantifiers.cs**: 4 functions
**Z3 C API (z3_api.h)**: 12 quantifier creation functions + pattern/bound functions

## Complete Function Mapping

### ✅ Functions in Both (4/12 quantifier creation functions in NativeLibrary - 33.3%)

| Our Method | Z3 C API | Parameters | Purpose |
|------------|----------|------------|---------|
| `MkForallConst` | `Z3_mk_forall_const` | `(ctx, weight, num_bound, bound[], num_patterns, patterns[], body)` | Creates universal quantifier using constants |
| `MkExistsConst` | `Z3_mk_exists_const` | `(ctx, weight, num_bound, bound[], num_patterns, patterns[], body)` | Creates existential quantifier using constants |
| `MkPattern` | `Z3_mk_pattern` | `(ctx, num_patterns, terms[])` | Creates pattern for quantifier instantiation |
| `MkBound` | `Z3_mk_bound` | `(ctx, index, ty)` | Creates bound variable with de Bruijn index |

### ❌ Functions in Z3 but NOT in NativeLibrary (8 missing)

#### Old-Style Quantifier Constructors (4 functions)
These use sorts and symbol names instead of constants:

1. **Z3_mk_forall**
   - Signature: `Z3_ast Z3_mk_forall(Z3_context c, unsigned weight, unsigned num_patterns, Z3_pattern const patterns[], unsigned num_decls, Z3_sort const sorts[], Z3_symbol const decl_names[], Z3_ast body)`
   - Purpose: Creates universal quantifier using sorts/symbols (old-style API)

2. **Z3_mk_exists**
   - Signature: `Z3_ast Z3_mk_exists(Z3_context c, unsigned weight, unsigned num_patterns, Z3_pattern const patterns[], unsigned num_decls, Z3_sort const sorts[], Z3_symbol const decl_names[], Z3_ast body)`
   - Purpose: Creates existential quantifier using sorts/symbols (old-style API)

3. **Z3_mk_quantifier**
   - Signature: `Z3_ast Z3_mk_quantifier(Z3_context c, Z3_bool is_forall, unsigned weight, unsigned num_patterns, Z3_pattern const patterns[], unsigned num_decls, Z3_sort const sorts[], Z3_symbol const decl_names[], Z3_ast body)`
   - Purpose: Generic quantifier constructor using sorts/symbols (old-style API)

4. **Z3_mk_quantifier_ex**
   - Signature: `Z3_ast Z3_mk_quantifier_ex(Z3_context c, Z3_bool is_forall, unsigned weight, Z3_symbol quantifier_id, Z3_symbol skolem_id, unsigned num_patterns, Z3_pattern const patterns[], unsigned num_no_patterns, Z3_ast const no_patterns[], unsigned num_decls, Z3_sort const sorts[], Z3_symbol const decl_names[], Z3_ast body)`
   - Purpose: Extended quantifier constructor with quantifier_id and skolem_id support

#### Generic Const-Based Quantifier Constructors (2 functions)
These are generic versions of mk_forall_const/mk_exists_const:

5. **Z3_mk_quantifier_const**
   - Signature: `Z3_ast Z3_mk_quantifier_const(Z3_context c, Z3_bool is_forall, unsigned weight, unsigned num_bound, Z3_app const bound[], unsigned num_patterns, Z3_pattern const patterns[], Z3_ast body)`
   - Purpose: Generic quantifier constructor using constants

6. **Z3_mk_quantifier_const_ex**
   - Signature: `Z3_ast Z3_mk_quantifier_const_ex(Z3_context c, Z3_bool is_forall, unsigned weight, Z3_symbol quantifier_id, Z3_symbol skolem_id, unsigned num_bound, Z3_app const bound[], unsigned num_patterns, Z3_pattern const patterns[], unsigned num_no_patterns, Z3_ast const no_patterns[], Z3_ast body)`
   - Purpose: Extended generic quantifier constructor with quantifier_id, skolem_id, and no_patterns support

#### Lambda Expression Constructors (2 functions)

7. **Z3_mk_lambda**
   - Signature: `Z3_ast Z3_mk_lambda(Z3_context c, unsigned num_decls, Z3_sort const sorts[], Z3_symbol const decl_names[], Z3_ast body)`
   - Purpose: Creates lambda expression using sorts/symbols (old-style API)
   - Note: The resulting expression has sort (Array sorts range) where range is the sort of body

8. **Z3_mk_lambda_const**
   - Signature: `Z3_ast Z3_mk_lambda_const(Z3_context c, unsigned num_bound, Z3_app const bound[], Z3_ast body)`
   - Purpose: Creates lambda expression using constants (const-based API)
   - Note: Simpler const-based version, consistent with mk_forall_const/mk_exists_const

### ⚠️ Functions in NativeLibrary but NOT in Z3

**None** - All 4 functions match Z3 C API exactly.

## Related Functions in Other Files

### Predicates (NativeLibrary.Predicates.cs - 3 functions)
- `Z3_is_lambda` - Checks if AST is a lambda expression
- `Z3_is_quantifier_forall` - Checks if AST is a universal quantifier
- `Z3_is_quantifier_exists` - Checks if AST is an existential quantifier

### Queries (NativeLibrary.Queries.cs - 8 functions)
- `Z3_get_quantifier_num_bound` - Gets number of bound variables in quantifier
- `Z3_get_quantifier_bound_name` - Gets name of bound variable at index
- `Z3_get_quantifier_bound_sort` - Gets sort of bound variable at index
- `Z3_get_quantifier_body` - Gets the body expression of quantifier
- `Z3_get_quantifier_num_patterns` - Gets number of patterns in quantifier
- `Z3_get_quantifier_pattern_ast` - Gets pattern at index from quantifier
- `Z3_get_pattern_num_terms` - Gets number of terms in pattern
- `Z3_get_pattern` - Gets term at index from pattern

## API Coverage Summary

| Metric | Count | Percentage |
|--------|-------|------------|
| Z3 C API Quantifier Creation Functions | 12 | 100% |
| Functions in NativeLibrary.Quantifiers.cs | 4 | **33.3%** |
| Missing Quantifier Creation Functions | 8 | 66.7% |
| Related Functions in Other Files | 11 | N/A |
| **Total Quantifier-Related API Coverage** | **15/23** | **65.2%** |

## Function Categories

### Const-Based Quantifiers (4 functions - 2/4 implemented = 50%)
Modern API using Z3 constants as bound variables:
- ✅ `Z3_mk_forall_const` - Universal quantifier with constants
- ✅ `Z3_mk_exists_const` - Existential quantifier with constants
- ❌ `Z3_mk_quantifier_const` - Generic quantifier with constants
- ❌ `Z3_mk_quantifier_const_ex` - Extended generic quantifier with constants

### Old-Style Quantifiers (4 functions - 0/4 implemented = 0%)
Legacy API using sorts and symbol names:
- ❌ `Z3_mk_forall` - Universal quantifier with sorts/symbols
- ❌ `Z3_mk_exists` - Existential quantifier with sorts/symbols
- ❌ `Z3_mk_quantifier` - Generic quantifier with sorts/symbols
- ❌ `Z3_mk_quantifier_ex` - Extended quantifier with sorts/symbols

### Lambda Expressions (2 functions - 0/2 implemented = 0%)
Lambda expressions (anonymous functions):
- ❌ `Z3_mk_lambda` - Lambda with sorts/symbols (old-style)
- ❌ `Z3_mk_lambda_const` - Lambda with constants (modern)

### Pattern and Bound Variables (2 functions - 2/2 implemented = 100%)
Support functions for quantifiers:
- ✅ `Z3_mk_pattern` - Creates instantiation patterns
- ✅ `Z3_mk_bound` - Creates bound variables with de Bruijn indices

## Completeness Assessment

⚠️ **PARTIAL** - NativeLibrary.Quantifiers.cs provides 33.3% coverage of Z3's quantifier creation API.

### Current State
- ✅ Modern const-based API for forall/exists is supported
- ✅ Pattern and bound variable creation is complete
- ✅ Quantifier query functions are in separate file (Queries.cs)
- ✅ Quantifier predicate functions are in separate file (Predicates.cs)

### Missing Functionality
- ❌ Generic const-based quantifiers (`Z3_mk_quantifier_const`, `Z3_mk_quantifier_const_ex`)
- ❌ Old-style quantifiers using sorts/symbols (4 functions)
- ❌ Lambda expressions (both old-style and const-based)

### Strengths
- Clean separation: creation (Quantifiers.cs), queries (Queries.cs), predicates (Predicates.cs)
- Modern const-based API is the preferred approach (properly implemented)
- Excellent XML documentation for implemented functions
- Proper delegate signatures matching Z3 C API

### Impact Analysis

**Low Priority Missing Functions** (Old-Style API):
- `Z3_mk_forall`, `Z3_mk_exists`, `Z3_mk_quantifier`, `Z3_mk_quantifier_ex`
- These are superseded by const-based versions
- Only needed for legacy code or specific use cases
- Modern Z3 usage should prefer const-based API

**Medium Priority Missing Functions** (Generic Const-Based):
- `Z3_mk_quantifier_const` - Generic quantifier constructor
- `Z3_mk_quantifier_const_ex` - Extended version with quantifier_id/skolem_id
- Useful for programmatic quantifier generation
- Can be worked around using `mk_forall_const`/`mk_exists_const`

**High Priority Missing Functions** (Lambda Expressions):
- `Z3_mk_lambda` and `Z3_mk_lambda_const`
- Lambda expressions are important for higher-order logic
- Used in array theory and functional programming scenarios
- No workaround available - distinct from quantifiers

## Verification

- **Source**: Z3 C API header `z3_api.h` from [Z3 GitHub repository](https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h)
- **Reference**: [Z3 Rust bindings documentation](https://docs.rs/z3-sys/latest/z3_sys/) for comprehensive function list
- **Our implementation**: `Z3Wrap/Core/Interop/NativeLibrary.Quantifiers.cs`
- **Verification date**: 2025-01-03
- **Z3 version compatibility**: All versions (quantifier API is stable)

## Recommendations

### Priority 1: Add Lambda Expression Support (High Priority)
```csharp
// Add to NativeLibrary.Quantifiers.cs:
internal IntPtr MkLambdaConst(IntPtr ctx, uint numBound, IntPtr[] bound, IntPtr body)
internal IntPtr MkLambda(IntPtr ctx, uint numDecls, IntPtr[] sorts, IntPtr[] declNames, IntPtr body)
```
**Rationale**: Lambda expressions are essential for higher-order logic and cannot be emulated.

### Priority 2: Add Generic Const-Based Quantifiers (Medium Priority)
```csharp
// Add to NativeLibrary.Quantifiers.cs:
internal IntPtr MkQuantifierConst(IntPtr ctx, bool isForall, uint weight, uint numBound, IntPtr[] bound, uint numPatterns, IntPtr[] patterns, IntPtr body)
internal IntPtr MkQuantifierConstEx(IntPtr ctx, bool isForall, uint weight, IntPtr quantifierId, IntPtr skolemId, uint numBound, IntPtr[] bound, uint numPatterns, IntPtr[] patterns, uint numNoPatterns, IntPtr[] noPatterns, IntPtr body)
```
**Rationale**: Provides programmatic flexibility for quantifier generation.

### Priority 3: Old-Style Quantifiers (Low Priority)
These can be added if needed for completeness or legacy compatibility, but modern code should use const-based API.

### Organization Note
The current file organization is excellent:
- **Creation** (Quantifiers.cs) - Building quantifiers
- **Queries** (Queries.cs) - Introspecting quantifier structure
- **Predicates** (Predicates.cs) - Type checking quantifiers

This separation should be maintained when adding new functions.

## Summary

NativeLibrary.Quantifiers.cs provides solid support for the core modern quantifier API (forall/exists with constants) but is missing:
1. **Lambda expressions** (high priority, no workaround)
2. **Generic quantifier constructors** (medium priority, can work around)
3. **Old-style API** (low priority, superseded by const-based)

The 33.3% coverage percentage understates the actual usability - the most important modern API is fully covered. Adding lambda expression support would bring the practical coverage to a solid level for most use cases.
