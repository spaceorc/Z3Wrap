# Z3 API Groups vs NativeLibrary Files Mapping Analysis

## Summary
- **Z3 API Groups**: 29 groups
- **Our NativeLibrary Files**: 34 files
- **Match Level**: Partial - some groups align, many diverge

## Direct Matches (11)

| Z3 API Group | Our File | Status |
|--------------|----------|--------|
| arrays | Arrays | ✅ Match |
| bit_vectors | BitVectors | ✅ Match |
| error_handling | ErrorHandling | ✅ Match |
| goals | Goals | ✅ Match |
| models | Model | ✅ Match (singular vs plural) |
| numerals | Numerals | ✅ Match |
| parameters | Parameters | ✅ Match |
| quantifiers | Quantifiers | ✅ Match |
| sets | Sets | ✅ Match |
| statistics | Statistics | ✅ Match |
| sorts | ? | ❓ Missing in our files |

## Partial Matches / Merged (7)

| Z3 API Group | Our File(s) | Notes |
|--------------|-------------|-------|
| context_and_ast_reference_counting | Context + ReferenceCountingExtra | We split this into 2 files |
| integers_and_reals | Expressions? | Likely merged into Expressions |
| propositional_logic_and_equality | Expressions? | Likely merged into Expressions |
| constants_and_applications | Expressions + Functions | Split between creation and functions |
| sequences_and_regular_expressions | StringTheory | We use domain name instead |
| tactics_simplifiers_and_probes | Tactics + Simplifiers + Probes | We split into 3 separate files |
| solvers | Solver + SolverExtensions | We split basic/advanced |

## Z3 Groups Missing in Our Files (9)

| Z3 API Group | Functions | Possible Location |
|--------------|-----------|-------------------|
| accessors | 102 | Likely **Queries** - introspection functions |
| symbols | 2 | Likely in Utilities or Context |
| global_parameters | 3 | Likely in Context or Utilities |
| create_configuration | 3 | Likely in Context |
| parameter_descriptions | 7 | Likely in Parameters |
| modifiers | 5 | Likely in Expressions or Utilities |
| interaction_logging | 4 | Likely in Utilities |
| string_conversion | 7 | Likely in Utilities |
| miscellaneous | 6 | Likely in Utilities |

## Our Files Not in Z3 Groups (11)

| Our File | Reason |
|----------|--------|
| AlgebraicNumbers | From z3_algebraic.h (different header) |
| AstCollections | From z3_ast_containers.h (different header) |
| FloatingPoint | From z3_fpa.h (different header) |
| Optimization | From z3_optimization.h (different header) |
| Constraints | Subset of functions (pseudo-boolean, cardinality) |
| Datatypes | Subset of sorts/accessors? |
| Expressions | Core creation functions, merger of multiple groups |
| FunctionInterpretations | Subset of models |
| Functions | Subset of constants_and_applications |
| Predicates | Type checking predicates (is_X functions) |
| Simplify | Simplification operations |
| Substitution | Substitution operations |
| SpecialTheories | Special relations and recursive functions |

## Key Observations

### 1. **Accessors (102 functions) → Queries**
The largest Z3 group "Accessors" (102 functions) likely maps to our **Queries.cs** file which handles AST introspection. This is a naming difference - Z3 calls them "accessors", we call it "queries".

### 2. **Utilities is a Catch-All**
Our **Utilities.cs** likely contains functions from multiple Z3 groups:
- string_conversion (7 functions)
- symbols (2 functions)
- miscellaneous (6 functions)
- interaction_logging (4 functions)

### 3. **Context Aggregation**
Our **Context.cs** likely aggregates:
- context_and_ast_reference_counting (partial)
- global_parameters (3 functions)
- create_configuration (3 functions)

### 4. **We Split More Granularly**
Z3 has one group "tactics_simplifiers_and_probes" (58 functions).
We split into 3 files: Tactics, Simplifiers, Probes - more maintainable.

### 5. **Domain-Specific Naming**
- Z3: "sequences_and_regular_expressions"
- Us: "StringTheory" (domain terminology)

### 6. **Additional Headers**
We have 4 files from headers not in z3_api.h:
- AlgebraicNumbers (z3_algebraic.h)
- AstCollections (z3_ast_containers.h)
- FloatingPoint (z3_fpa.h)
- Optimization (z3_optimization.h)

## Recommendation

Our organization is **different but arguably better** than Z3's grouping:

**Pros of Our Approach:**
- More granular separation (Tactics/Simplifiers/Probes vs one big group)
- Domain terminology (StringTheory vs sequences_and_regular_expressions)
- Split complex areas (Solver/SolverExtensions for basic/advanced)
- Semantic grouping (Predicates for type checks, Queries for introspection)

**Cons of Our Approach:**
- Less direct 1:1 mapping to Z3 documentation
- Some arbitrary decisions (where do modifiers go?)
- "Utilities" becomes a catch-all

**Action Items:**
1. Verify Accessors → Queries mapping
2. Check if Utilities contains the expected 19+ functions from 4 Z3 groups
3. Consider renaming for clarity (Accessors vs Queries)
4. Document the mapping in each file's header
