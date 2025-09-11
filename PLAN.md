# Z3 C# Wrapper Implementation Plan

## Project Structure
- **NativeMethods.cs** - P/Invoke declarations for Z3 C API functions ✅
- **Z3Context.cs** - Manages Z3 context lifecycle and configuration ✅
- **Z3Context.Factories.cs** - Factory methods for creating expressions ✅
- **Z3Expr.cs** - Base expression class with ToString() support ✅
- **Z3BoolExpr.cs** - Boolean expression wrapper ✅
- **Z3IntExpr.cs** - Integer expression wrapper ✅
- **Z3RealExpr.cs** - Real expression wrapper ✅
- **AnsiStringPtr.cs** - Disposable string marshalling helper ✅
- **Z3Solver.cs** - Wrapper for solver operations ✅
- **Z3Status.cs** - Solver result enumeration ✅
- **Z3Model.cs** - Model inspection wrapper ⏳
- **Z3Exception.cs** - Custom exception handling ⏳

## Current Status: Core Functionality Complete ✅

### Phase 1: Foundation (COMPLETED ✅)
- ✅ **NativeMethods.cs** - P/Invoke with dynamic library loading
- ✅ **Z3Context** - Context management with centralized expression tracking
- ✅ **Expression Types**: Z3BoolExpr, Z3IntExpr, Z3RealExpr with operator overloading
- ✅ **Factory Pattern** - Context-based expression creation (`context.MkInt(5)`, `context.MkBoolConst("p")`)
- ✅ **Memory Management** - Hierarchical disposal with context managing all object lifetimes
- ✅ **Modern C# Patterns** - Nullable reference types, `using var`, expression-bodied members

### Phase 2: Core Solving Capability (COMPLETED ✅)
1. ✅ **Z3Solver** - Full solver implementation with hierarchical disposal
   - ✅ Solver P/Invoke methods: `Z3_mk_solver`, `Z3_solver_assert`, `Z3_solver_check`
   - ✅ Solver reference counting: `Z3_solver_inc_ref`, `Z3_solver_dec_ref`
   - ✅ Constraint stack: `Z3_solver_push`, `Z3_solver_pop`
   - ✅ Simple solver support: `context.MkSimpleSolver()`
   - ✅ GetReasonUnknown() for diagnostic information
   - ✅ Hierarchical disposal: Context tracks and disposes all solvers

2. ✅ **Z3Status Enum** - Solver result enumeration
   ```csharp
   public enum Z3Status { Unknown = -1, Unsatisfiable = 0, Satisfiable = 1 }
   ```

3. ✅ **Z3Model** - Basic model extraction with rock-solid lifetime management
   - ✅ Model P/Invoke methods: `Z3_solver_get_model`, `Z3_model_to_string`
   - ✅ Solver-owned model lifecycle: Model owned and managed by solver
   - ✅ Safe disposal in any order: Context→Solver→Model hierarchical cleanup
   - ✅ Model invalidation: Automatic invalidation on solver state changes
   - ✅ Error handling: Throws on invalid states instead of returning null
   - ✅ Comprehensive testing: 13 lifetime tests covering all scenarios

### Phase 3: Complete Z3Model Value Extraction (COMPLETED ✅)
**Status**: Z3Model now has complete value extraction capabilities with rock-solid lifetime management.

**Implementation Plan:**
1. ✅ **Research Z3 C API model functions**
2. ✅ **Add required P/Invoke methods to NativeMethods.cs**
   - `Z3_model_eval` - Primary API for evaluating expressions in models
   - `Z3_get_numeral_string` - Extract numeric values as strings
   - `Z3_get_numeral_int` - Extract integer values directly  
   - `Z3_get_bool_value` - Extract boolean values
   - `Z3_is_numeral_ast` - Check if expression is a numeral
   - `Z3_get_sort`, `Z3_get_sort_kind` - For expression type detection
3. ✅ **Implement Z3Model.Evaluate() method**
   - Core evaluation using Z3_model_eval with model completion
   - Returns Z3Expr representing the evaluated result
   - Handles expression evaluation within model context
   - Automatic type detection and wrapper creation
4. ✅ **Add convenient value extraction methods**
   - `GetIntValue(Z3IntExpr expr)` - Extract integer constant values (returns int)
   - `GetBoolValue(Z3BoolExpr expr)` - Extract boolean constant values (returns Z3BoolValue enum)
   - `GetRealValueAsString(Z3RealExpr expr)` - Extract real constant values (returns string)
   - Proper error handling for non-constant expressions
   - Consistent enum usage: Z3Status, Z3BoolValue, Z3SortKind (no magic constants)
5. ✅ **Create comprehensive tests**
   - 17 new tests for Z3Model value extraction functionality
   - Test value extraction for all expression types
   - Test error handling for invalid extractions
   - Test model completion behavior
   - Test model invalidation scenarios
6. ✅ **Update usage examples**

**Expected Usage Pattern:**
```csharp
using var context = new Z3Context();
var x = context.MkIntConst("x");
var y = context.MkBoolConst("y");
var z = context.MkRealConst("z");

using var solver = context.MkSolver();
solver.Assert(x > context.MkInt(0));
solver.Assert(x < context.MkInt(10));
solver.Assert(y);
solver.Assert(z == context.MkReal(3.14));

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();
    
    // Primary method: Evaluate returns Z3 expressions (consistent with factory pattern)
    var xExpr = model.Evaluate(x);        // Returns Z3IntExpr representing constant
    var yExpr = model.Evaluate(y);        // Returns Z3BoolExpr representing true/false
    var zExpr = model.Evaluate(z);        // Returns Z3RealExpr representing constant
    
    // Convenience methods: Extract primitive values for display/computation
    var xValue = model.GetIntValue(x);           // e.g., 5 (int)
    var yValue = model.GetBoolValue(y);          // e.g., Z3BoolValue.True (enum)  
    var zValue = model.GetRealValueAsString(z);  // e.g., "3.14" (string)
    
    Console.WriteLine($"x = {xValue}, y = {yValue}, z = {zValue}");
}
```

### Phase 4: Extended Boolean Operations (COMPLETED ✅)
Add missing logical operations to make Boolean expressions complete:
- ✅ **Implies** - `context.MkImplies(p, q)` and `p.Implies(q)`
- ✅ **Iff** - `context.MkIff(p, q)` and `p.Iff(q)` (biconditional)
- ✅ **Xor** - `context.MkXor(p, q)` and `p.Xor(q)` (exclusive or)

### Phase 5: Extended Arithmetic Operations (COMPLETED ✅)
Add common mathematical operations:
- ✅ **Modulo** - `context.MkMod(x, y)` and `x.Mod(y)` (with % operator!)
- ✅ **Absolute Value** - `x.Abs()` (instance methods for both integers and reals)
- ✅ **Unary Minus** - `context.MkUnaryMinus(x)` and `-x` (unary operator!)
- ✅ **Min/Max** - `context.Min(x, y)`, `context.Max(x, y)` (extension methods only) with comprehensive literal overloads
- 🔮 **Power** - `context.MkPower(x, y)` (limited Z3 support, can be added later)

### Phase 6: Generic If-Then-Else Operations (COMPLETED ✅)
Add type-safe conditional expressions:
- ✅ **Generic If Method** - `condition.If(thenExpr, elseExpr)` with compile-time type safety
- ✅ **Generic Factory Support** - `context.MkIte<T>(condition, thenExpr, elseExpr)` with compile-time type safety
- ✅ **Non-Generic Factory Support** - `context.MkIte(condition, thenExpr, elseExpr)` with runtime type detection
- ✅ **Z3 Sort Integration** - Uses Z3's sort system via `WrapExpr` for accurate type determination
- ✅ **Type Safety** - Returns correct expression type without runtime casting
- ✅ **IntelliSense Support** - Full IDE integration with method suggestions

**Usage Examples:**
```csharp
var condition = x > context.MkInt(0);

// Generic factory method - compile-time type safety
Z3IntExpr result1 = context.MkIte<Z3IntExpr>(condition, x, y);
Z3RealExpr result2 = context.MkIte<Z3RealExpr>(condition, realA, realB);

// Non-generic factory method - runtime type detection using Z3 sorts
var result3 = context.MkIte(condition, x, y); // Returns Z3IntExpr automatically
var result4 = context.MkIte(condition, realA, realB); // Returns Z3RealExpr automatically

// Instance method syntax (uses generic factory internally)
Z3IntExpr result5 = condition.If(x, y);
result5.Add(context.MkInt(5)); // IntelliSense works immediately

// Works with all expression types
var realResult = (a > 0.0).If(realA, realB);  // Z3RealExpr
var boolResult = p.If(q, r);                  // Z3BoolExpr
```

## Future Phases: Advanced Types

### Phase 7: Bit Vectors (Later)
- **Z3BitVectorExpr** - Fixed-width integers
- Bit operations: `&`, `|`, `^`, `~`, `<<`, `>>`
- Arithmetic: `+`, `-`, `*`, `/`, `%` on bit vectors

### Phase 8: Arrays and Other Types (Later)
- **Z3ArrayExpr** - Array/map operations
- **Z3StringExpr** - String operations
- **Z3DataTypeExpr** - User-defined datatypes

### Phase 9: Quantifiers (Advanced)
- **ForAll/Exists** - Quantified expressions
- Variable binding and substitution

## Hierarchical Disposal System ✅

The library implements a sophisticated parent-child disposal pattern where the Z3Context manages all object lifetimes:

### Design Principles
- **Context is the parent** - Tracks all created solvers in `HashSet<Z3Solver> trackedSolvers`
- **Children delegate to parent** - Solver.Dispose() calls `context.DisposeSolver(this)`
- **Parent manages native resources** - Context handles Z3 reference counting (`Z3_solver_dec_ref`)
- **Automatic cleanup** - Context disposal automatically disposes all tracked children
- **Double disposal safety** - All objects can be safely disposed multiple times

### Implementation Details
```csharp
// Context tracks all solvers
private readonly HashSet<Z3Solver> trackedSolvers = [];

// Solver delegates disposal to context
public void Dispose()
{
    if (disposed) return;
    if (!isBeingDisposedByContext)
    {
        context.DisposeSolver(this); // Delegate to parent
    }
    disposed = true;
}

// Context handles all native resource cleanup
internal void DisposeSolver(Z3Solver solver)
{
    if (disposed) return; // Context already disposed
    UntrackSolver(solver);
    var solverHandle = solver.InternalHandle;
    if (solverHandle != IntPtr.Zero)
    {
        NativeMethods.Z3SolverDecRef(contextHandle, solverHandle);
    }
    solver.InternalDispose();
}
```

### Benefits
- **Memory safety** - No leaked Z3 objects, proper reference counting
- **Exception safety** - Context disposal works even if children throw
- **Natural usage** - Users can dispose objects in any order without issues
- **Comprehensive testing** - 29 disposal tests covering all scenarios (16 solver + 13 model)

## Z3Model Implementation ✅

The library now includes Z3Model for extracting satisfying assignments from solver results:

### Design Principles
- **Solver-owned lifecycle** - Models are created and owned by their solver
- **Automatic invalidation** - Models become invalid when solver state changes
- **Error on invalid access** - GetModel() throws exceptions instead of returning null
- **Safe disposal** - ToString() never throws, Handle throws after invalidation

### Model Lifecycle
```csharp
using var solver = context.MkSolver();
// ... add constraints ...

if (solver.Check() == Z3Status.Satisfiable)
{
    var model = solver.GetModel();  // ✅ Works - solver is satisfiable
    Console.WriteLine(model);       // ✅ Shows model contents
    
    solver.Assert(x == context.MkInt(1)); // Model invalidated here
    Console.WriteLine(model);       // ✅ Safe - returns "<invalidated>"
    model.Handle;                   // ❌ Throws ObjectDisposedException
}
```

### Error Handling
- **GetModel() throws** when solver status is not Satisfiable
- **GetModel() throws** when Check() hasn't been called first  
- **Clear error messages** describe exactly what went wrong
- **ToString() is always safe** - returns error strings instead of throwing

## Current Usage Pattern (FULLY WORKING!)
```csharp
using var context = new Z3Context();

// Create variables using factory methods
var x = context.MkIntConst("x");
var y = context.MkIntConst("y");
var p = context.MkBoolConst("p");
var q = context.MkBoolConst("q");

// Create solver
using var solver = context.MkSolver();

// Add constraints using natural operators
solver.Assert(x > context.MkInt(0));
solver.Assert(y > context.MkInt(0));
solver.Assert(x + y == context.MkInt(10));
solver.Assert(p | (x != y)); // Boolean operators work!

// Extended boolean operations - NEW!
solver.Assert(p.Implies(q)); // If p then q
solver.Assert(q.Iff(x.Mod(y) == context.MkInt(0))); // q iff x is divisible by y
solver.Assert(p ^ q); // XOR operator: exactly one of p or q must be true

// Extended arithmetic operations - NEW!
solver.Assert(x.Abs() > context.MkInt(0)); // Absolute value
solver.Assert(y % 2 == context.MkInt(1)); // y is odd (using % operator!)
solver.Assert(-x < context.MkInt(0)); // Unary minus operator!

// Type-safe if-then-else operations - NEW!  
var conditional = (x > context.MkInt(5)).If(context.MkInt(100), context.MkInt(0)); // Returns Z3IntExpr
solver.Assert(conditional > context.MkInt(50)); // Can use immediately without casting

// Min/Max operations - NEW!
solver.Assert(context.Min(x, y) > context.MkInt(0)); // Minimum of x and y must be positive
solver.Assert(context.Max(x, y) < context.MkInt(20)); // Maximum of x and y must be less than 20
solver.Assert(context.Min(x, 5) > context.MkInt(0)); // Natural literal syntax
solver.Assert(context.Max(10, y) == context.MkInt(10)); // Works both ways

// Check satisfiability - THIS WORKS NOW!
var result = solver.Check();
Console.WriteLine($"Result: {result}");
Console.WriteLine($"Variables: x={x}, y={y}, p={p}, q={q}"); // ToString() works!

if (result == Z3Status.Satisfiable)
{
    // Extract the satisfying assignment - THIS WORKS NOW!
    var model = solver.GetModel();
    Console.WriteLine($"Model: {model}");
    
    // Extract values using convenient methods
    var xVal = model.GetIntValue(x);
    var yVal = model.GetIntValue(y);
    var pVal = model.GetBoolValue(p);
    var qVal = model.GetBoolValue(q);
    
    Console.WriteLine($"x = {xVal}, y = {yVal}");
    Console.WriteLine($"p = {pVal}, q = {qVal}");
    Console.WriteLine($"x % y = {model.GetIntValue(x % y)}"); // Using % operator!
    Console.WriteLine($"|x| = {model.GetIntValue(x.Abs())}");
    Console.WriteLine($"-x = {model.GetIntValue(-x)}"); // Using unary - operator!
}
else if (result == Z3Status.Unknown)
{
    Console.WriteLine($"Reason: {solver.GetReasonUnknown()}");
}

// Push/pop constraint contexts
solver.Push();
solver.Assert(x == context.MkInt(3));
Console.WriteLine($"With x=3: {solver.Check()}");
solver.Pop(); // Back to previous state
```

## Implementation Priority Queue  
1. ✅ **Z3Solver + Z3Status** - COMPLETED! Core solving works
2. ✅ **Z3Model** - COMPLETED! Get actual variable values from satisfiable results  
3. ✅ **Extended boolean operations** - COMPLETED! (Implies, Iff, Xor)
4. ✅ **Extended arithmetic operations** - COMPLETED! (Mod, Abs, UnaryMinus, Min, Max with operators and methods) 
5. ✅ **Generic If-Then-Else** - COMPLETED! Type-safe conditional expressions with compile-time checking
6. ✅ **Min/Max operations** - COMPLETED! Implemented using if-then-else with factory and instance methods
7. 🔮 **Bit vectors** (Very useful for verification, but can wait)
8. 🔮 **Arrays/Strings** (Specialized use cases)

## Architecture Decisions Made
- ✅ **Centralized factory pattern** - All expressions created through context
- ✅ **Hierarchical disposal system** - Context manages all object lifetimes, children delegate to parent
- ✅ **Modern C# patterns** - Nullable types, `using var`, expression-bodied members
- ✅ **Simplified dispose pattern** - No unused parameters, clean delegation
- ✅ **Automatic string marshalling** - `AnsiStringPtr` for clean P/Invoke
- ✅ **Operator overloading** - Natural mathematical syntax (`+`, `-`, `*`, `/`, `%`, `==`, `!=`, `<`, `>`, `<=`, `>=`, unary `-`) and logical operators (`&`, `|`, `^`, `!`)
- ✅ **Mixed-type operators** - Full bidirectional support for literals: `expr + 5`, `3.14 * expr`, `x >= 42`, etc.
- ✅ **Resilient ToString()** - Never throws exceptions, handles disposed contexts gracefully
- ✅ **Comprehensive test coverage** - 136 tests across 10 organized test files with global setup
- ✅ **Sealed classes** - All concrete classes properly sealed for performance and design clarity
- ✅ **Minimal codebase** - No unused methods, fields, or delegates - everything serves a purpose
- ✅ **Consistent patterns** - Operators call helper methods, helper methods call context functions
- ✅ **Generic type safety** - If-Then-Else operations with compile-time type checking
- ✅ **Clean architecture** - Modular extension methods organized by functionality (Primitives, Operators, Comparison, Equality, etc.)
- ✅ **Extension method pattern** - Proper `this Z3Context context` pattern with comprehensive mixed-type overloads
- ✅ **Mixed-type overloads** - Extension methods provide natural literal syntax (e.g., `context.Min(x, 5)`, `context.Max(3.14, y)`)
- ✅ **Warning suppression** - Clean builds with documented pragma directives for intentional design choices
- ✅ **InternalsVisibleTo** - Test assembly access to internal members while keeping API clean

## Extension Method Architecture ✅

The library uses a modular extension method pattern for clean separation of concerns:

- **Z3ContextExtensions.Primitives.cs** - Core factory methods (`Int`, `Real`, `BoolConst`, `True`, `False`)
- **Z3ContextExtensions.NumericOperators.cs** - Arithmetic operations (`Add`, `Sub`, `Mul`, `Div`, `Mod`, `UnaryMinus`, `Abs`)
- **Z3ContextExtensions.Comparison.cs** - Comparison operations (`Lt`, `Le`, `Gt`, `Ge`) with mixed-type support
- **Z3ContextExtensions.Equality.cs** - Equality operations (`Eq`, `Neq`) with comprehensive literal overloads
- **Z3ContextExtensions.BoolOperators.cs** - Boolean operations (`And`, `Or`, `Not`, `Implies`, `Iff`, `Xor`)
- **Z3ContextExtensions.MinMax.cs** - Min/Max operations using if-then-else pattern

**Benefits:**
- **Modular organization** - Each file has single responsibility
- **Mixed-type support** - `context.Add(expr, 5)`, `context.Lt(x, 3.14)` 
- **Bidirectional operators** - Both `expr + 5` and `5 + expr` work naturally
- **Consistent patterns** - All extensions follow same `this Z3Context context` pattern
- **IntelliSense friendly** - Grouped methods appear organized in IDE

### Recent Code Organization Improvements ✅

**Redundant Overload Cleanup (January 2025)**
- **Removed 63 redundant `int` overloads** from Z3RealExpr operations - C#'s implicit `int` to `double` conversion makes these unnecessary
- **Z3RealExpr.cs**: Removed 24 redundant operator overloads and 15 redundant method overloads for `int` parameters
- **Extension methods**: Removed 24 redundant `int` overloads across Z3ContextExtensions.* files
- **Verified with tests**: All 136 tests still pass - implicit conversion works perfectly
- **Benefits**: Cleaner API surface, less maintenance, leverages C# type system properly

**Logical Method Grouping Pattern ✅**
Applied consistent organizational pattern across all files grouping methods by parameter/return types:

1. **Same-type to same-type operations** (e.g., Z3IntExpr ↔ Z3IntExpr)
2. **Same-type to bool operations** (e.g., Z3IntExpr → Z3BoolExpr comparisons) 
3. **Mixed-type to same-type operations** (e.g., Z3RealExpr ↔ double)
4. **Mixed-type to bool operations** (e.g., Z3RealExpr + double → Z3BoolExpr comparisons)
5. **Unary operations** (UnaryMinus, Abs)

**Files Updated:**
- **Z3RealExpr.cs** - Reordered 16 methods into logical groups
- **Z3IntExpr.cs** - Reordered 24 methods into logical groups  
- **Z3ContextExtensions.NumericOperators.cs** - Complete reorganization by type groups
- **Z3ContextExtensions.Equality.cs** - Grouped by parameter types
- **Z3ContextExtensions.Comparison.cs** - Grouped by parameter types

**Benefits:**
- **Consistent organization** - Same pattern across all files
- **Better readability** - Related methods grouped together
- **Easier maintenance** - Logical structure makes changes easier
- **IntelliSense improvements** - Methods appear in logical order

## Test Suite Excellence ✅
- **GlobalSetup.cs** - One-time libz3 loading for all tests (eliminates redundant setup)
- **Z3ContextTests.cs** - Context lifecycle and parameter management
- **Z3ExpressionTests.cs** - Expression creation, operators, and resilient ToString() behavior  
- **Z3SolverTests.cs** - Solver functionality, push/pop, diagnostics
- **Z3DisposalTests.cs** - Comprehensive hierarchical disposal scenarios
- **Z3ModelLifetimeTests.cs** - Model ownership and lifetime management testing
- **Z3ModelValueExtractionTests.cs** - Model value extraction with comprehensive edge cases
- **Z3ExtendedOperationsTests.cs** - Extended boolean and arithmetic operations plus if-then-else and min/max (43 tests)
- **Clean test practices** - Silent tests with proper assertions, no console output, duplicate tests removed
- **Z3MixedTypeOperatorTests.cs** - Mixed-type arithmetic operations
- **Z3SimpleTest.cs** - Edge cases and specific constraint scenarios
- **Modern syntax** - Uses `using var` and clean patterns throughout
- **136 comprehensive tests** - Full coverage of library functionality and edge cases

### Test Coverage Analysis (January 2025) ✅

**Comprehensive Method Audit Complete**
Performed systematic analysis of all public methods in extension classes and expression classes to identify any untested functionality:

**Extension Methods Analyzed:**
- **Z3ContextExtensions.Primitives.cs**: Int, IntConst, Real, RealConst, True, False, Bool, BoolConst
- **Z3ContextExtensions.BoolOperators.cs**: And, Or, Not, Xor, Implies, Iff, Ite (generic and non-generic)
- **Z3ContextExtensions.Equality.cs**: Eq, Neq (with mixed-type overloads)
- **Z3ContextExtensions.Comparison.cs**: Lt, Le, Gt, Ge (with mixed-type overloads)
- **Z3ContextExtensions.NumericOperators.cs**: Add, Sub, Mul, Div, Mod, UnaryMinus, Abs
- **Z3ContextExtensions.MinMax.cs**: Min, Max (with mixed-type overloads)

**Expression Class Methods Analyzed:**
- **Z3BoolExpr**: And, Or, Not, Implies, Iff, Xor, If (operators: &, |, ^, !)
- **Z3IntExpr**: Add, Sub, Mul, Div, Mod, Lt, Le, Gt, Ge, UnaryMinus, Abs (operators: +, -, *, /, %, <, <=, >, >=, ==, !=, unary -)
- **Z3RealExpr**: Add, Sub, Mul, Div, Lt, Le, Gt, Ge, UnaryMinus, Abs (operators: +, -, *, /, <, <=, >, >=, ==, !=, unary -)

**Coverage Assessment:**
- **✅ All major functionality is comprehensively tested** across 10 test files
- **✅ All operators and overloads have test coverage**
- **✅ Mixed-type operations thoroughly tested**  
- **✅ Extended operations (Implies, Iff, Xor, Mod, Abs, UnaryMinus) fully covered**
- **✅ If-then-else and Min/Max operations extensively tested**
- **✅ Error handling and edge cases well covered**

**Minor Gap Identified:**
- **Bool(bool value) method** - Simple factory method not explicitly tested
  - Method: `public static Z3BoolExpr Bool(this Z3Context context, bool value)`
  - Implementation: `return value ? context.True() : context.False();`
  - Location: Z3ContextExtensions.Primitives.cs:53

**Conclusion:** Test coverage is excellent with only one minor untested method found. The Bool(bool) method is a trivial wrapper around True()/False() which are both well-tested, but should be added for completeness.