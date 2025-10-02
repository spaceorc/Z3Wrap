# Complete Z3 C API Coverage Plan - NativeLibrary Only

## Goal
Add ALL remaining Z3 C API functions to `NativeLibrary` (P/Invoke layer) to create complete coverage and serve as living documentation of the entire Z3 C API.

**IMPORTANT**: This plan is ONLY about the low-level `NativeLibrary` P/Invoke wrapper class (`Z3Wrap/Core/Interop/NativeLibrary*.cs` files). This is NOT about the high-level `Z3Library` class or any high-level C# API wrappers. The goal is to expose the complete raw Z3 C API through P/Invoke delegates, not to create high-level abstractions.

## Current Status
- **Currently implemented**: 149 functions (organized into 11 partial class files)
- **Total in Z3 C API (z3_api.h)**: 556 functions
- **Actually needed**: 552 functions (4 conversion functions are no-ops in C#)
- **Missing**: 403 functions (73% gap)
- **Progress**: 27% complete

### Completed Work
✅ **Phase 1 Setup: COMPLETE** (October 2, 2025)
- Created 10 partial class files for NativeLibrary
- Organized existing 120 functions into logical categories
- All tests passing, coverage 95.3%, CI passing

✅ **MVP: Predicates COMPLETE** (October 2, 2025)
- Created 11th partial class file: NativeLibrary.Predicates.cs
- Added 18 predicate/type-checking functions (is_* functions)
- Moved IsNumeralAst from Model.cs to Predicates.cs for better organization
- Excluded all NativeLibrary P/Invoke wrappers from code coverage (mechanical delegates)
- Current structure (11 partial class files, 149 functions):
  - NativeLibrary.Context.cs (8 functions)
  - NativeLibrary.Solver.cs (12 functions)
  - NativeLibrary.Model.cs (19 functions) ⭐ EXPANDED - was 8, added 11 model introspection functions
  - NativeLibrary.Parameters.cs (8 functions)
  - NativeLibrary.Quantifiers.cs (3 functions)
  - NativeLibrary.Expressions.cs (28 functions)
  - NativeLibrary.Arrays.cs (6 functions)
  - NativeLibrary.BitVectors.cs (40 functions)
  - NativeLibrary.Functions.cs (3 functions)
  - NativeLibrary.ErrorHandling.cs (3 functions)
  - NativeLibrary.Predicates.cs (18 functions)
- All 903 tests passing
- Coverage: **97.9%** (NativeLibrary excluded from coverage - it's mechanical P/Invoke)
- CI pipeline: passing
- **Ready to scale**: Can add hundreds more P/Invoke functions without affecting coverage

## Why Complete Coverage?

### 1. Living Documentation
- `NativeLibrary` code becomes authoritative reference for what's available in Z3 C API
- XML comments provide searchable documentation of raw Z3 functions
- IntelliSense shows all P/Invoke capabilities
- No need to constantly reference C headers (z3_api.h)

### 2. Foundation for Future High-Level API
- Complete P/Invoke layer enables gradual high-level API development in `Z3Library`
- Any missing low-level function won't block future high-level features
- Unlocks advanced features (tactics, goals, probes, optimization) when ready

### 3. Completeness at P/Invoke Layer
- Professional P/Invoke wrapper covers entire Z3 C API surface
- No "sorry, not implemented" at the native layer
- Power users can directly call any Z3 function if needed
- Research applications can access any Z3 capability

## Missing Functions by Category

### Creation Functions (140 functions)
**High Priority** - Core expression builders:
```
Z3_mk_abs                    - Absolute value
Z3_mk_array_default          - Array default value
Z3_mk_array_ext              - Array extensionality
Z3_mk_array_sort_n           - N-dimensional arrays
Z3_mk_as_array               - Array from function
Z3_mk_atleast                - At-least-K constraint
Z3_mk_atmost                 - At-most-K constraint
Z3_mk_bit2bool               - Bitvector bit to bool
Z3_mk_bound                  - Bound variable (quantifiers)
Z3_mk_bv_numeral             - Bitvector from numeral
Z3_mk_bvnand                 - Bitvector NAND
Z3_mk_bvnor                  - Bitvector NOR
Z3_mk_bvredand               - Bitvector reduction AND
Z3_mk_bvredor                - Bitvector reduction OR
Z3_mk_bvxnor                 - Bitvector XNOR
Z3_mk_char                   - Character literal
Z3_mk_char_from_bv           - Character from bitvector
Z3_mk_char_is_digit          - Character digit check
Z3_mk_char_le                - Character less-or-equal
Z3_mk_char_sort              - Character sort
Z3_mk_char_to_bv             - Character to bitvector
Z3_mk_char_to_int            - Character to integer
Z3_mk_concat                 - Sequence/string concatenation
Z3_mk_constructor            - Datatype constructor
Z3_mk_constructor_list       - Datatype constructor list
Z3_mk_context                - Context (non-RC version)
Z3_mk_datatype               - Single datatype
Z3_mk_datatype_sort          - Datatype sort
Z3_mk_datatypes              - Multiple datatypes
Z3_mk_distinct               - Distinct constraint
... and 110+ more mk_* functions
```

### Query Functions (85 functions)
**High Priority** - Introspection and inspection:
```
Z3_get_algebraic_number_lower    - Algebraic number approximation
Z3_get_algebraic_number_upper    - Algebraic number approximation
Z3_get_app_arg                   - Application argument
Z3_get_app_decl                  - Application declaration
Z3_get_app_num_args              - Application arity
Z3_get_arity                     - Function arity
Z3_get_array_arity               - Array arity (deprecated)
Z3_get_array_sort_domain_n       - N-dimensional array domain
Z3_get_as_array_func_decl        - Extract function from array
Z3_get_ast_hash                  - AST hash code
Z3_get_ast_id                    - AST unique ID
Z3_get_ast_kind                  - AST kind discriminator
Z3_get_datatype_sort_constructor - Datatype constructor accessor
Z3_get_datatype_sort_num_constructors - Datatype constructor count
Z3_get_decl_*                    - 15+ function declaration queries
Z3_get_denominator               - Rational denominator
Z3_get_depth                     - AST depth
Z3_get_domain                    - Function domain sort
... and 55+ more get_* functions
```

### Solver Functions (42 functions)
**Medium Priority** - Advanced solver capabilities:
```
Z3_solver_add_simplifier         - Add simplifier to solver
Z3_solver_assert_and_track       - Assert with tracking literal
Z3_solver_check_assumptions      - Check with assumptions
Z3_solver_congruence_*           - Congruence closure queries (3 functions)
Z3_solver_cube                   - Get cube for model
Z3_solver_from_file              - Parse solver from SMTLIB2 file
Z3_solver_from_string            - Parse solver from SMTLIB2 string
Z3_solver_get_assertions         - Get asserted formulas
Z3_solver_get_consequences       - Consequence finding
Z3_solver_get_help               - Solver help text
Z3_solver_get_levels             - Decision levels
Z3_solver_get_non_units          - Non-unit clauses
Z3_solver_get_num_scopes         - Scope depth
Z3_solver_get_param_descrs       - Parameter descriptors
Z3_solver_get_proof              - Proof object
Z3_solver_get_statistics         - Solver statistics
Z3_solver_get_trail              - Decision trail
Z3_solver_get_units              - Unit clauses
Z3_solver_get_unsat_core         - Unsatisfiable core
Z3_solver_import_model_converter - Import model converter
Z3_solver_interrupt              - Interrupt solving
Z3_solver_next_split             - Get next split variable
Z3_solver_propagate_*            - Theory propagation callbacks (13 functions)
Z3_solver_register_on_clause     - Register clause callback
Z3_solver_set_initial_value      - Set initial variable value
Z3_solver_solve_for              - Solve for variables (incremental)
Z3_solver_to_dimacs_string       - Export to DIMACS
Z3_solver_to_string              - Solver state as string
Z3_solver_translate              - Translate solver to another context
```

### Tactic/Goal/Probe Functions (43 functions)
**Medium Priority** - Advanced solving strategies:

**Tactics (20 functions)** - Transformation strategies:
```
Z3_tactic_and_then               - Sequential composition
Z3_tactic_apply                  - Apply tactic to goal
Z3_tactic_apply_ex               - Apply with parameters
Z3_tactic_cond                   - Conditional tactic
Z3_tactic_dec_ref / inc_ref      - Reference counting
Z3_tactic_fail                   - Always-fail tactic
Z3_tactic_fail_if                - Conditional failure
Z3_tactic_fail_if_not_decided    - Fail if not decided
Z3_tactic_get_descr              - Tactic description
Z3_tactic_get_help               - Tactic help
Z3_tactic_get_param_descrs       - Parameter descriptors
Z3_tactic_or_else                - Alternative tactic
Z3_tactic_par_and_then           - Parallel and-then
Z3_tactic_par_or                 - Parallel or
Z3_tactic_repeat                 - Repeat until fixpoint
Z3_tactic_skip                   - No-op tactic
Z3_tactic_try_for                - Timeout wrapper
Z3_tactic_using_params           - Apply with parameters
Z3_tactic_when                   - Conditional application
```

**Goals (16 functions)** - Intermediate proof states:
```
Z3_goal_assert                   - Add formula to goal
Z3_goal_convert_model            - Convert model for goal
Z3_goal_dec_ref / inc_ref        - Reference counting
Z3_goal_depth                    - Goal depth
Z3_goal_formula                  - Get formula at index
Z3_goal_inconsistent             - Check if inconsistent
Z3_goal_is_decided_sat           - Is SAT decided?
Z3_goal_is_decided_unsat         - Is UNSAT decided?
Z3_goal_num_exprs                - Number of formulas
Z3_goal_precision                - Precision level
Z3_goal_reset                    - Clear all formulas
Z3_goal_size                     - Goal size
Z3_goal_to_dimacs_string         - Export to DIMACS
Z3_goal_to_string                - String representation
Z3_goal_translate                - Translate to another context
```

**Probes (13 functions)** - Goal property tests:
```
Z3_probe_and                     - Conjunction of probes
Z3_probe_apply                   - Apply probe to goal
Z3_probe_const                   - Constant probe
Z3_probe_dec_ref / inc_ref       - Reference counting
Z3_probe_eq / ge / gt / le / lt  - Comparison probes
Z3_probe_get_descr               - Probe description
Z3_probe_not                     - Negation
Z3_probe_or                      - Disjunction
```

### Simplifier Functions (7 functions)
**Medium Priority** - Pre-processing:
```
Z3_simplifier_and_then           - Sequential simplifiers
Z3_simplifier_dec_ref / inc_ref  - Reference counting
Z3_simplifier_get_descr          - Simplifier description
Z3_simplifier_get_help           - Simplifier help
Z3_simplifier_get_param_descrs   - Parameter descriptors
Z3_simplifier_using_params       - Simplifier with parameters
```

### ~~Model Functions (11 functions)~~ - ✅ COMPLETE (October 2, 2025)
**DONE** - All 11 model introspection functions in NativeLibrary.Model.cs:
```
✅ Z3_model_get_num_consts        - Number of constant interpretations
✅ Z3_model_get_const_decl        - Get constant declaration at index
✅ Z3_model_get_const_interp      - Get constant interpretation
✅ Z3_model_get_num_funcs         - Number of function interpretations
✅ Z3_model_get_func_decl         - Get function declaration at index
✅ Z3_model_get_func_interp       - Get function interpretation
✅ Z3_model_has_interp            - Check if declaration has interpretation
✅ Z3_model_get_num_sorts         - Number of sort universes
✅ Z3_model_get_sort              - Get sort at index
✅ Z3_model_get_sort_universe     - Get finite sort universe
✅ Z3_model_translate             - Translate model to another context
```

### Reference Counting Functions (21 functions)
**Low Priority** - Already handled by wrapper patterns:
```
Z3_apply_result_dec_ref / inc_ref
Z3_ast_vector_dec_ref / inc_ref
Z3_ast_map_dec_ref / inc_ref
Z3_func_entry_dec_ref / inc_ref
Z3_func_interp_dec_ref / inc_ref
Z3_stats_dec_ref / inc_ref
... (most resource types need inc/dec ref)
```

### Function Interpretation Functions (9 functions)
**Medium Priority** - Advanced model queries:
```
Z3_func_entry_get_arg            - Get entry argument
Z3_func_entry_get_num_args       - Entry arity
Z3_func_entry_get_value          - Entry result value
Z3_func_interp_add_entry         - Add interpretation entry
Z3_func_interp_get_arity         - Function arity
Z3_func_interp_get_else          - Default value
Z3_func_interp_get_entry         - Get entry at index
Z3_func_interp_get_num_entries   - Number of entries
Z3_func_interp_set_else          - Set default value
```

### ~~Conversion Functions (4 functions)~~ - NOT NEEDED
**SKIPPED** - Unnecessary in C# P/Invoke wrapper:
```
Z3_app_to_ast                    - Application to AST (identity function)
Z3_func_decl_to_ast              - Declaration to AST (identity function)
Z3_sort_to_ast                   - Sort to AST (identity function)
Z3_pattern_to_ast                - Pattern to AST (identity function)
```
**Reason**: These functions exist in Z3 C API for type system reasons (converting
between `Z3_app*`, `Z3_func_decl*`, `Z3_sort*`, `Z3_pattern*` and `Z3_ast*`).
In C# P/Invoke, we use `IntPtr` for all handle types, so these conversions are
no-ops that just return the same pointer. No need to implement.

### String/Debugging Functions (17 functions)
**Low Priority** - Debugging and logging:
```
Z3_append_log                    - Append to log
Z3_close_log                     - Close log file
Z3_disable_trace / enable_trace  - Trace control
Z3_finalize_memory               - Memory cleanup
Z3_func_decl_to_string           - Declaration to string
Z3_benchmark_to_smtlib_string    - Benchmark to SMTLIB2
Z3_eval_smtlib2_string           - Evaluate SMTLIB2
Z3_solver_to_string              - Solver state
Z3_goal_to_string                - Goal state
Z3_stats_to_string               - Statistics
Z3_toggle_warning_messages       - Warning control
... and more
```

### ~~Predicates (18 functions)~~ - ✅ COMPLETE (October 2, 2025)
**DONE** - All 18 type-checking functions in NativeLibrary.Predicates.cs:
```
✅ Z3_is_eq_ast                   - AST equality
✅ Z3_is_eq_sort                  - Sort equality
✅ Z3_is_eq_func_decl             - Function declaration equality
✅ Z3_is_well_sorted              - Well-sorted check
✅ Z3_is_app                      - Is application?
✅ Z3_is_numeral_ast              - Is numeral?
✅ Z3_is_algebraic_number         - Is algebraic number?
✅ Z3_is_string                   - Is string literal?
✅ Z3_is_string_sort              - Is string sort?
✅ Z3_is_seq_sort                 - Is sequence sort?
✅ Z3_is_re_sort                  - Is regex sort?
✅ Z3_is_char_sort                - Is character sort?
✅ Z3_is_as_array                 - Is as-array expression?
✅ Z3_is_lambda                   - Is lambda expression?
✅ Z3_is_quantifier_forall        - Is universal quantifier?
✅ Z3_is_quantifier_exists        - Is existential quantifier?
✅ Z3_is_ground                   - Is ground (no free variables)?
✅ Z3_is_recursive_datatype_sort  - Is recursive datatype?
```

### Other Important Categories

**Parameters/Descriptors (15+ functions)**:
- `Z3_params_*` - Parameter set management
- `Z3_param_descrs_*` - Parameter descriptor queries
- `Z3_global_param_*` - Global parameter control

**AST Operations (20+ functions)**:
- `Z3_ast_vector_*` - AST vector operations
- `Z3_ast_map_*` - AST map operations
- `Z3_substitute*` - AST substitution
- `Z3_update_term` - Term updates

**String Theory (30+ functions)**:
- `Z3_mk_string` - String literals
- `Z3_mk_string_*` - String operations (concat, length, substring, etc.)
- `Z3_mk_seq_*` - Sequence operations
- `Z3_mk_re_*` - Regular expression operations

**Floating Point (40+ functions)**:
- `Z3_mk_fpa_*` - Floating-point operations
- `Z3_fpa_*` - FP-specific functions

**Special Theory (20+ functions)**:
- `Z3_mk_set_*` - Finite set theory
- `Z3_mk_pbeq` - Pseudo-boolean constraints
- `Z3_mk_transitive_closure` - Relation operations

## Implementation Strategy

### Phase 1: Foundation (High Priority - ~146 functions → 117 remaining)
Focus on functions needed for basic usage patterns:
- **Completed**: 29 functions (18 predicates + 11 model functions)
- **Remaining**: 117 functions

1. **Creation functions (mk_*)**: ~50 most common
   - Missing bitvector operations
   - String/sequence operations
   - Set operations
   - Array operations

2. **Query functions (get_*)**: ~40 most common
   - AST introspection
   - Datatype queries
   - Declaration queries

3. **~~Model functions~~**: ~~All 11~~ **✅ DONE** (October 2, 2025) - Complete model introspection

4. **~~Conversion functions~~**: ~~All 4~~ **SKIPPED** (no-ops in C# IntPtr-based wrapper)

5. **~~Predicates (is_*)~~**: ~~All 18~~ **✅ DONE** (October 2, 2025)

### Phase 2: Advanced Solving (Medium Priority - 100 functions)

1. **Solver extensions**: 42 functions
   - Proof/unsat core extraction
   - Assumptions/consequences
   - Theory propagation
   - Statistics

2. **Tactics/Goals/Probes**: 43 functions
   - Complete tactic API
   - Goal management
   - Probe system

3. **Simplifiers**: 7 functions
   - Pre-processing

4. **Parameters**: ~15 functions
   - Parameter management
   - Descriptors

### Phase 3: Specialized Theories (Medium Priority - 100 functions)

1. **String theory**: ~30 functions
   - String literals and operations
   - Sequence operations
   - Regular expressions

2. **Floating-point**: ~40 functions
   - FP sorts and operations
   - Rounding modes

3. **Special theories**: ~30 functions
   - Sets
   - Relations
   - Pseudo-boolean

### Phase 4: Utilities (Low Priority - 86 functions)

1. **Reference counting**: 21 functions
   - Complete resource management

2. **AST collections**: ~20 functions
   - Vector/map operations

3. **Debugging**: ~15 functions
   - Logging, tracing
   - String representations

4. **Algebraic numbers**: ~10 functions
   - RCF operations

5. **Miscellaneous**: ~20 functions
   - Version info
   - Global utilities

## Technical Implementation

### File Organization
**DECISION MADE**: Using partial classes (Option B)

Current structure (10 partial class files, 120 functions):
```
NativeLibrary.cs                    - Core infrastructure, loading
NativeLibrary.Context.cs            - Context lifecycle (8 functions)
NativeLibrary.Solver.cs             - Solver operations (12 functions)
NativeLibrary.Model.cs              - Model evaluation (9 functions)
NativeLibrary.Parameters.cs         - Parameters (8 functions)
NativeLibrary.Quantifiers.cs        - Quantifiers (3 functions)
NativeLibrary.Expressions.cs        - Expression creation (28 functions)
NativeLibrary.Arrays.cs             - Array theory (6 functions)
NativeLibrary.BitVectors.cs         - BitVector operations (40 functions)
NativeLibrary.Functions.cs          - Function declarations (3 functions)
NativeLibrary.ErrorHandling.cs      - Error handling (3 functions)
```

New files needed for Phase 1+:
```
NativeLibrary.Queries.cs            - get_* introspection functions
✅ NativeLibrary.Predicates.cs      - is_* type-checking functions (DONE - 18 functions)
NativeLibrary.Tactics.cs            - Tactic system
NativeLibrary.Goals.cs              - Goal management
NativeLibrary.Probes.cs             - Probe system
NativeLibrary.Simplifiers.cs        - Simplifier system
NativeLibrary.StringTheory.cs       - String/sequence operations
NativeLibrary.FloatingPoint.cs      - Floating-point operations
NativeLibrary.SpecialTheories.cs    - Sets, relations, special theories
NativeLibrary.AstCollections.cs     - AST vectors/maps
NativeLibrary.Utilities.cs          - Debug, logging, misc
```

This is P/Invoke only - no high-level abstractions.

### Delegate Definitions (P/Invoke Pattern)
Each Z3 C API function needs in `NativeLibrary`:
1. Private delegate type matching C signature
2. Internal wrapper method with XML docs (not public - internal P/Invoke layer)
3. Loading call in the partial file's `LoadFunctions{Name}()` method

**Template** (P/Invoke wrapper, not high-level API):
```csharp
// Delegate
private delegate IntPtr MkAbsDelegate(IntPtr ctx, IntPtr arg);

// Wrapper method
/// <summary>
/// Creates an absolute value expression.
/// </summary>
/// <param name="ctx">The Z3 context handle.</param>
/// <param name="arg">The argument expression.</param>
/// <returns>AST node representing absolute value of arg.</returns>
/// <seealso href="https://z3prover.github.io/api/html/group__capi.html">Z3 C API Documentation</seealso>
internal IntPtr MkAbs(IntPtr ctx, IntPtr arg)
{
    var funcPtr = GetFunctionPointer("Z3_mk_abs");
    var func = Marshal.GetDelegateForFunctionPointer<MkAbsDelegate>(funcPtr);
    return func(ctx, arg);
}

// In LoadLibraryInternal()
LoadFunctionOrNull(handle, functionPointers, "Z3_mk_abs");
```

### XML Documentation Strategy
- **Concise**: 1-2 sentence descriptions
- **Consistent**: Follow existing patterns
- **Link to official docs**: Include Z3 API reference link
- **Parameter docs**: Brief descriptions
- **Return docs**: What's returned

### Loading Strategy
All new functions use `LoadFunctionOrNull()`:
- Won't break existing code
- Fails at runtime if unsupported
- Clear error messages

## Testing Strategy

### Existing Tests
- All 903 tests must continue passing
- No changes to existing test code required

### New Tests
**Not required for NativeLibrary P/Invoke wrappers** because:
- P/Invoke wrapper functions are mechanical delegates
- No business logic to test at this layer
- Real testing happens at high-level `Z3Library` API layer (when built)
- Would need 436+ trivial test methods with no real value
- Functions mostly unused until high-level C# API wrappers exist

**Testing Strategy**:
- Existing 903 tests continue passing (no regressions)
- New functions tested when high-level API uses them
- Focus testing effort on high-level API, not P/Invoke layer

## Work Breakdown

### Phase 1: High Priority (1-2 weeks)
- Split NativeLibrary into partial classes
- Add 150 high-priority functions
- Generate delegates and wrappers
- XML documentation for all
- Run `make ci` to verify

### Phase 2: Medium Priority (1-2 weeks)
- Add 100 advanced solving functions
- Tactics/goals/probes complete
- Extended solver capabilities
- Run `make ci` to verify

### Phase 3: Specialized Theories (1 week)
- String theory (30 functions)
- Floating-point (40 functions)
- Sets/relations (30 functions)
- Run `make ci` to verify

### Phase 4: Utilities (3-5 days)
- Reference counting (21 functions)
- AST collections (20 functions)
- Debug/logging (15 functions)
- Miscellaneous (30 functions)
- Final `make ci` verification

## Success Criteria

✅ **Completeness**: All 556 Z3 C API functions present
✅ **Building**: `make build` succeeds with zero warnings
✅ **Tests**: All 903+ tests pass
✅ **Coverage**: Maintain ≥93% coverage
✅ **Documentation**: Every function has XML docs
✅ **Formatting**: `make format` and `make lint` pass
✅ **Organization**: Logical file structure with partial classes

## Benefits Delivered

### For P/Invoke Layer (NativeLibrary)
- Complete Z3 C API exposed through P/Invoke
- IntelliSense shows all raw Z3 functions
- Clear P/Invoke documentation in IDE
- No z3_api.h header lookups needed

### For Future High-Level API Development (Z3Library)
- High-level API can be built incrementally on solid foundation
- Clear categorization guides which features to wrap next
- Living reference of what's possible in Z3
- No missing low-level functions blocking high-level features

### For Power Users
- Direct access to any Z3 function if needed
- Advanced features available (tactics, probes, optimization)
- Theory propagation for custom theories
- Proof extraction capabilities

## Risk Mitigation

### Risk: Massive code size
**Mitigation**: Partial classes keep each file manageable

### Risk: Functions change between Z3 versions
**Mitigation**: Optional loading handles version differences gracefully

### Risk: Maintenance burden
**Mitigation**: Code is mechanical, easy to update, well-documented

### Risk: XML doc errors
**Mitigation**: Build warnings catch issues, systematic review

## Implementation Checklist

### Phase 1 Setup
- [x] Create partial class structure
- [x] Move existing functions to appropriate files
- [x] Verify all tests still pass
- [x] Run `make format`
- [x] Verify consistency of all partial files (delegates, loads, methods)

### Phase 1 Implementation (High Priority)
- [ ] Add 50 creation functions (mk_*)
- [ ] Add 40 query functions (get_*)
- [ ] Add 11 model functions
- [ ] Add 9 function interpretation functions
- [ ] Add 4 conversion functions
- [ ] Add 17 predicate functions
- [ ] All delegates defined
- [ ] All wrappers implemented
- [ ] All XML docs written
- [ ] Run `make ci`

### Phase 2 Implementation (Advanced Solving)
- [ ] Add 42 solver functions
- [ ] Add 20 tactic functions
- [ ] Add 16 goal functions
- [ ] Add 13 probe functions
- [ ] Add 7 simplifier functions
- [ ] Add 15 parameter functions
- [ ] All delegates defined
- [ ] All wrappers implemented
- [ ] All XML docs written
- [ ] Run `make ci`

### Phase 3 Implementation (Theories)
- [ ] Add 30 string theory functions
- [ ] Add 40 floating-point functions
- [ ] Add 30 special theory functions
- [ ] All delegates defined
- [ ] All wrappers implemented
- [ ] All XML docs written
- [ ] Run `make ci`

### Phase 4 Implementation (Utilities)
- [ ] Add 21 reference counting functions
- [ ] Add 20 AST collection functions
- [ ] Add 15 debug/logging functions
- [ ] Add 30 miscellaneous functions
- [ ] All delegates defined
- [ ] All wrappers implemented
- [ ] All XML docs written
- [ ] Run `make ci`

### Final Verification
- [ ] All 556 functions implemented
- [ ] Build with zero warnings
- [ ] All 903+ tests pass
- [ ] Coverage ≥93%
- [ ] `make format` applied
- [ ] `make lint` passes
- [ ] Documentation complete
- [ ] User approval for commit

## Timeline Estimate

**Total**: 4-6 weeks for complete implementation

- Week 1-2: Phase 1 (High Priority, 150 functions)
- Week 3-4: Phase 2 (Advanced Solving, 100 functions)
- Week 5: Phase 3 (Specialized Theories, 100 functions)
- Week 6: Phase 4 (Utilities, 86 functions) + Final review

**Incremental**: Can commit after each phase for progressive delivery

---

**Status**: Phase 1 In Progress - 29/146 functions complete (October 2, 2025)
**Next Step**: Continue with Function Interpretations (9 functions) or Query functions (~40 functions)

## Progress Log

### October 2, 2025 - Model Functions Complete
- ✅ Added 11 model introspection functions to NativeLibrary.Model.cs (now 19 total)
- ✅ Complete model API: iterate constants, functions, sorts; check interpretations; translate models
- ✅ All 903 tests passing, coverage 97.9%, CI passing
- ✅ Now at 149/552 functions (27% complete)
- 📊 Progress: 403 functions remaining
- 🎯 Phase 1: 29/146 functions complete (20%)

### October 2, 2025 - MVP: Predicates Complete
- ✅ Added NativeLibrary.Predicates.cs with 18 type-checking functions
- ✅ Moved IsNumeralAst from Model.cs to Predicates.cs
- ✅ Excluded NativeLibrary from coverage (mechanical P/Invoke delegates)
- ✅ Coverage jumped to 97.9% after exclusion
- ✅ All 903 tests passing, CI passing
- ✅ 138/552 functions (25% complete)

### October 2, 2025 - Phase 1 Setup Complete
- ✅ Created 10 partial class files organizing 120 existing functions
- ✅ Created Python extraction script (`extract_methods.py`) for safe refactoring
- ✅ All partial files verified for consistency (delegates, loads, methods match)
- ✅ All 903 tests passing, coverage 95.3%, CI passing
- 📦 Ready for Phase 1 Implementation
