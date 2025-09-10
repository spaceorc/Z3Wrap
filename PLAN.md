# Z3 C# Wrapper Implementation Plan

## Project Structure
- **NativeMethods.cs** - P/Invoke declarations for Z3 C API functions ✅
- **Z3Context.cs** - Manages Z3 context lifecycle and configuration ✅
- **Z3Context.Factories.cs** - Factory methods for creating expressions ✅
- **Z3Expr.cs** - Base expression class ✅
- **Z3BoolExpr.cs** - Boolean expression wrapper ✅
- **Z3IntExpr.cs** - Integer expression wrapper ✅
- **Z3RealExpr.cs** - Real expression wrapper ✅
- **AnsiStringPtr.cs** - Disposable string marshalling helper ✅
- **Z3Solver.cs** - Wrapper for solver operations ⏳
- **Z3Model.cs** - Model inspection wrapper ⏳
- **Z3Exception.cs** - Custom exception handling ⏳

## Current Status: Completed Foundation ✅

### Phase 1: Foundation (COMPLETED)
- ✅ **NativeMethods.cs** - P/Invoke with dynamic library loading
- ✅ **Z3Context** - Context management with centralized expression tracking
- ✅ **Expression Types**: Z3BoolExpr, Z3IntExpr, Z3RealExpr with operator overloading
- ✅ **Factory Pattern** - Context-based expression creation (`context.MkInt(5)`)
- ✅ **Memory Management** - Centralized reference counting and cleanup
- ✅ **Modern C# Patterns** - Nullable reference types, `using var`, expression-bodied members

## Next Phase: Core Solving Capability

### Phase 2: Solver Implementation (HIGH PRIORITY)
1. **Z3Solver** - The most critical missing component
   - Add solver P/Invoke methods: `Z3_mk_solver`, `Z3_solver_assert`, `Z3_solver_check`
   - Implement solver reference counting: `Z3_solver_inc_ref`, `Z3_solver_dec_ref`
   - Add constraint stack: `Z3_solver_push`, `Z3_solver_pop`

2. **Z3Model** - Result extraction
   - Add model P/Invoke methods: `Z3_solver_get_model`, `Z3_model_eval`
   - Model interpretation: `Z3_model_get_const_interp`

3. **Status Enum** - Solver results
   ```csharp
   public enum Z3Status { Unknown = -1, Unsatisfiable = 0, Satisfiable = 1 }
   ```

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

## Target Usage Pattern (Updated)
```csharp
using (var context = new Z3Context())
{
    // Create variables using factory methods
    var x = context.MkIntConst("x");
    var y = context.MkIntConst("y");
    
    // Create solver
    using (var solver = context.MkSolver())
    {
        // Add constraints using natural operators
        solver.Assert(x > 0);
        solver.Assert(y > 0);
        solver.Assert(x + y == 10);
        solver.Assert((x > 5).Implies(y < 5)); // Extended boolean ops
        
        // Check satisfiability
        if (solver.Check() == Z3Status.Satisfiable)
        {
            var model = solver.GetModel();
            Console.WriteLine($"x = {model.Eval(x)}");
            Console.WriteLine($"y = {model.Eval(y)}");
        }
    }
}
```

## Implementation Priority Queue
1. 🔥 **Z3Solver + Z3Model** (Can't solve anything without this!)
2. 🔥 **Boolean operations** (Implies, Iff, Xor - commonly used)
3. 📈 **Arithmetic operations** (Mod, Abs, Min, Max - nice to have)
4. 🔮 **Bit vectors** (Very useful for verification, but can wait)
5. 🔮 **Arrays/Strings** (Specialized use cases)

## Architecture Decisions Made
- ✅ **Centralized factory pattern** - All expressions created through context
- ✅ **Centralized memory management** - Context tracks all expressions
- ✅ **Modern C# patterns** - Nullable types, `using var`, expression-bodied members
- ✅ **Simplified dispose pattern** - No unused parameters
- ✅ **Automatic string marshalling** - `AnsiStringPtr` for clean P/Invoke
- ✅ **Operator overloading** - Natural mathematical syntax