# Z3 Special Theories API Comparison Report

## Overview
**NativeLibrary.SpecialTheories.cs**: 5 functions
**Z3 C API (z3_api.h)**: 12 functions (Special Relations, Sorts, and Theories section)

## Complete Function Mapping

### ✅ Functions in Both (5/12 in NativeLibrary match Z3 API)

| Our Method | Z3 C API | Purpose |
|------------|----------|---------|
| `MkTransitiveClosure` | `Z3_mk_transitive_closure` | Creates transitive closure of binary relation |
| `MkFiniteDomainSort` | `Z3_mk_finite_domain_sort` | Creates finite domain sort with specified size |
| `MkEnumerationSort` | `Z3_mk_enumeration_sort` | Creates enumeration sort like C enum type |
| `MkFreshFuncDecl` | `Z3_mk_fresh_func_decl` | Creates fresh function declaration with unique name |
| `MkFreshConst` | `Z3_mk_fresh_const` | Creates fresh constant with unique name |

### ❌ Functions in Z3 but NOT in NativeLibrary (7 missing)

#### Special Order Relations (4 functions)
```c
Z3_func_decl Z3_API Z3_mk_partial_order(Z3_context c, Z3_sort a, unsigned id);
```
Creates a binary relation that is a partial order over the given sort. The `id` parameter distinguishes different partial orders over the same signature.

```c
Z3_func_decl Z3_API Z3_mk_linear_order(Z3_context c, Z3_sort a, unsigned id);
```
Creates a binary relation that is a linear (total) order over the given sort. Extends partial order with totality property.

```c
Z3_func_decl Z3_API Z3_mk_piecewise_linear_order(Z3_context c, Z3_sort a, unsigned id);
```
Creates a binary relation that is a piecewise linear order over the given sort.

```c
Z3_func_decl Z3_API Z3_mk_tree_order(Z3_context c, Z3_sort a, unsigned id);
```
Creates a binary relation that is a tree order over the given sort.

#### List Sort (1 function)
```c
Z3_sort Z3_API Z3_mk_list_sort(Z3_context c,
                               Z3_symbol name,
                               Z3_sort   elem_sort,
                               Z3_func_decl* nil_decl,
                               Z3_func_decl* is_nil_decl,
                               Z3_func_decl* cons_decl,
                               Z3_func_decl* is_cons_decl,
                               Z3_func_decl* head_decl,
                               Z3_func_decl* tail_decl);
```
Creates a list sort over an element sort. Returns the list sort and populates output parameters with nil/cons constructors and head/tail accessors.

#### Recursive Functions (2 functions)
```c
Z3_func_decl Z3_API Z3_mk_rec_func_decl(Z3_context c, Z3_symbol s,
                                        unsigned domain_size, Z3_sort const domain[],
                                        Z3_sort range);
```
Declares a recursive function. Must be followed by `Z3_add_rec_def` to provide the recursive definition.

```c
void Z3_API Z3_add_rec_def(Z3_context c, Z3_func_decl f, unsigned n,
                           Z3_ast args[], Z3_ast body);
```
Adds a recursive definition to a function declared with `Z3_mk_rec_func_decl`. The body can reference the function being defined.

### ⚠️ Functions in NativeLibrary but NOT in Z3
None - all our functions exist in Z3 C API.

## API Coverage Summary
| Metric | Count | Percentage |
|--------|-------|------------|
| Z3 C API Functions | 12 | 100% |
| Functions in NativeLibrary | 5 | 41.7% |
| Missing Functions | 7 | 58.3% |

## Function Categories

### Currently Implemented (5 functions)
1. **Transitive Closure** (1 function)
   - `Z3_mk_transitive_closure` - Transitive closure of binary relations

2. **Special Sorts** (2 functions)
   - `Z3_mk_finite_domain_sort` - Finite domain sorts
   - `Z3_mk_enumeration_sort` - Enumeration types

3. **Fresh Declarations** (2 functions)
   - `Z3_mk_fresh_func_decl` - Fresh function with unique name
   - `Z3_mk_fresh_const` - Fresh constant with unique name

### Missing Implementations (7 functions)
1. **Special Order Relations** (4 functions)
   - `Z3_mk_partial_order` - Partial order relations
   - `Z3_mk_linear_order` - Linear (total) order relations
   - `Z3_mk_piecewise_linear_order` - Piecewise linear orders
   - `Z3_mk_tree_order` - Tree order relations

2. **List Theory** (1 function)
   - `Z3_mk_list_sort` - List sorts with constructors and accessors

3. **Recursive Functions** (2 functions)
   - `Z3_mk_rec_func_decl` - Recursive function declarations
   - `Z3_add_rec_def` - Add recursive function definitions

## Completeness Assessment

⚠️ **PARTIAL COVERAGE (41.7%)**

### Impact of Missing Functions

#### Special Order Relations
The missing order relation functions (`Z3_mk_partial_order`, `Z3_mk_linear_order`, `Z3_mk_piecewise_linear_order`, `Z3_mk_tree_order`) are important for:
- Efficient reasoning about ordering constraints
- Reachability problems in graphs
- Scheduling and planning applications
- Built-in decision procedures using Bellman-Ford variants
- Avoiding expensive first-order axiomatization

These functions provide specialized, efficient decision procedures for their respective order types.

#### List Sort
The `Z3_mk_list_sort` function enables:
- List theory with built-in nil/cons constructors
- Head/tail accessors
- List recognizer predicates
- Similar to ML-style lists or Lisp

#### Recursive Functions
The recursive function support (`Z3_mk_rec_func_decl`, `Z3_add_rec_def`) enables:
- Defining functions recursively in terms of themselves
- Essential for modeling recursive data structures
- Required for advanced use cases like program verification

## Recommendations

### Priority 1: Special Order Relations (High Value)
Add the four special order relation functions as they provide efficient specialized solvers:
```csharp
internal IntPtr MkPartialOrder(IntPtr ctx, IntPtr sort, uint id)
internal IntPtr MkLinearOrder(IntPtr ctx, IntPtr sort, uint id)
internal IntPtr MkPiecewiseLinearOrder(IntPtr ctx, IntPtr sort, uint id)
internal IntPtr MkTreeOrder(IntPtr ctx, IntPtr sort, uint id)
```

These provide significant performance benefits over manual axiomatization.

### Priority 2: List Sort (Medium Value)
Add `Z3_mk_list_sort` for list theory support:
```csharp
internal IntPtr MkListSort(
    IntPtr ctx,
    IntPtr name,
    IntPtr elemSort,
    IntPtr[] nilDecl,
    IntPtr[] isNilDecl,
    IntPtr[] consDecl,
    IntPtr[] isConsDecl,
    IntPtr[] headDecl,
    IntPtr[] tailDecl)
```

This enables built-in list reasoning similar to functional programming languages.

### Priority 3: Recursive Functions (Medium Value)
Add recursive function support:
```csharp
internal IntPtr MkRecFuncDecl(
    IntPtr ctx,
    IntPtr name,
    uint numDomains,
    IntPtr[] domains,
    IntPtr range)

internal void AddRecDef(
    IntPtr ctx,
    IntPtr funcDecl,
    uint numArgs,
    IntPtr[] args,
    IntPtr body)
```

Required for advanced use cases involving recursive definitions.

## File Organization

The current file is well-organized with clear sections:
- Relation Theory (transitive closure)
- Special Sorts (finite domain, enumeration)
- Miscellaneous (fresh functions)

Consider reorganizing to:
- **Special Order Relations** (partial, linear, piecewise, tree orders + transitive closure)
- **Special Sorts** (finite domain, enumeration, lists)
- **Fresh Declarations** (fresh functions and constants)
- **Recursive Functions** (recursive declarations and definitions)

## Verification
- **Source**: Z3 C API header file `z3_api.h`
  - URL: https://github.com/Z3Prover/z3/blob/master/src/api/z3_api.h
  - Special Relations section (lines ~1100-1200)
  - Fresh declarations section (lines ~1050-1100)
- **Our Implementation**: Z3Wrap/Core/Interop/NativeLibrary.SpecialTheories.cs
- **Documentation**:
  - https://microsoft.github.io/z3guide/docs/theories/Special%20Relations/
  - https://z3prover.github.io/api/html/group__capi.html

## Notes

1. **Special Order Relations**: These functions provide efficient built-in decision procedures using "variants of Bellman-Ford push relabeling graphs" - much faster than first-order axiomatization.

2. **ID Parameter**: The `id` parameter in order relation functions allows distinguishing multiple order relations over the same sort signature.

3. **List Sort**: Returns multiple output parameters (constructors, testers, accessors) via pointer arrays.

4. **Recursive Functions**: Two-step process: declare with `Z3_mk_rec_func_decl`, then define with `Z3_add_rec_def`.

5. **File Naming**: The current name "SpecialTheories" is somewhat generic. Consider renaming to "SpecialRelations" or "OrdersAndSorts" to better reflect the focused functionality.

---

**Status**: 41.7% complete (5 of 12 functions)
**Recommendation**: Add the 7 missing functions to achieve complete coverage of Z3's special theories API
**Priority**: HIGH for special order relations, MEDIUM for lists and recursive functions
