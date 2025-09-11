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

3. ⏳ **Z3Model** - Result extraction (next priority)
   - Add model P/Invoke methods: `Z3_solver_get_model`, `Z3_model_eval`
   - Model interpretation: `Z3_model_get_const_interp`

### Phase 3: Extended Boolean Operations
Add missing logical operations to make Boolean expressions complete:
- **Implies** - `context.MkImplies(p, q)` and `p.Implies(q)`
- **Iff** - `context.MkIff(p, q)` and `p.Iff(q)` (biconditional)
- **Xor** - `context.MkXor(p, q)` and `p.Xor(q)` (exclusive or)

### Phase 4: Extended Arithmetic Operations
Add common mathematical operations:
- **Modulo** - `context.MkMod(x, y)` and `x.Mod(y)`
- **Absolute Value** - `context.MkAbs(x)` and `x.Abs()`
- **Min/Max** - `context.MkMin(x, y)`, `context.MkMax(x, y)`
- **Power** - `context.MkPower(x, y)` (if supported by Z3)

## Future Phases: Advanced Types

### Phase 5: Bit Vectors (Later)
- **Z3BitVectorExpr** - Fixed-width integers
- Bit operations: `&`, `|`, `^`, `~`, `<<`, `>>`
- Arithmetic: `+`, `-`, `*`, `/`, `%` on bit vectors

### Phase 6: Arrays and Other Types (Later)
- **Z3ArrayExpr** - Array/map operations
- **Z3StringExpr** - String operations
- **Z3DataTypeExpr** - User-defined datatypes

### Phase 7: Quantifiers (Advanced)
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
- **Comprehensive testing** - 16 disposal tests covering all scenarios

## Current Usage Pattern (FULLY WORKING!)
```csharp
using var context = new Z3Context();

// Create variables using factory methods
var x = context.MkIntConst("x");
var y = context.MkIntConst("y");
var p = context.MkBoolConst("p");

// Create solver
using var solver = context.MkSolver();

// Add constraints using natural operators
solver.Assert(x > context.MkInt(0));
solver.Assert(y > context.MkInt(0));
solver.Assert(x + y == context.MkInt(10));
solver.Assert(p | (x != y)); // Boolean operators work!

// Check satisfiability - THIS WORKS NOW!
var result = solver.Check();
Console.WriteLine($"Result: {result}");
Console.WriteLine($"Variables: x={x}, y={y}, p={p}"); // ToString() works!

if (result == Z3Status.Unknown)
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
2. 🔥 **Z3Model** - Get actual variable values from satisfiable results
3. 📈 **Boolean operations** (Implies, Iff, Xor - commonly used)
4. 📈 **Arithmetic operations** (Mod, Abs, Min, Max - nice to have)
5. 🔮 **Bit vectors** (Very useful for verification, but can wait)
6. 🔮 **Arrays/Strings** (Specialized use cases)

## Architecture Decisions Made
- ✅ **Centralized factory pattern** - All expressions created through context
- ✅ **Hierarchical disposal system** - Context manages all object lifetimes, children delegate to parent
- ✅ **Modern C# patterns** - Nullable types, `using var`, expression-bodied members
- ✅ **Simplified dispose pattern** - No unused parameters, clean delegation
- ✅ **Automatic string marshalling** - `AnsiStringPtr` for clean P/Invoke
- ✅ **Operator overloading** - Natural mathematical syntax (`+`, `-`, `*`, `/`, `==`, `!=`, `<`, `>`, `<=`, `>=`)
- ✅ **Resilient ToString()** - Never throws exceptions, handles disposed contexts gracefully
- ✅ **Comprehensive test coverage** - 49 tests across 5 organized test files with global setup

## Test Suite Excellence ✅
- **GlobalSetup.cs** - One-time libz3 loading for all tests (eliminates redundant setup)
- **Z3ContextTests.cs** - Context lifecycle and parameter management
- **Z3ExpressionTests.cs** - Expression creation, operators, and resilient ToString() behavior  
- **Z3SolverTests.cs** - Solver functionality, push/pop, diagnostics
- **Z3DisposalTests.cs** - Comprehensive hierarchical disposal scenarios
- **Z3SimpleTest.cs** - Edge cases and specific constraint scenarios
- **Modern syntax** - Uses `using var` and clean patterns throughout
- **49 comprehensive tests** - Full coverage of library functionality and edge cases