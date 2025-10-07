# Z3 Optimization Support Implementation Plan

## Overview

Add complete Z3 optimization support to Z3Wrap, mirroring the existing solver architecture. The low-level Z3Library methods are already generated in `Z3Library.OptimizationFacilities.generated.cs` (29 methods) - we need to create the high-level wrapper classes.

## Current Status

**✅ Complete:**
- All 29 Z3Library optimization methods generated in `Z3Library.OptimizationFacilities.generated.cs`
- Methods include: MkOptimize, OptimizeIncRef/DecRef, Assert/AssertSoft, Maximize/Minimize, Check, GetModel, Push/Pop, etc.

**❌ Missing:**
- High-level `Z3Optimize` class (user-facing API)
- Context integration (tracking optimizers like solvers)
- Extension methods for natural syntax
- Comprehensive tests

## Architecture

### Model: Copy Z3Solver Pattern

`Z3Optimize` should follow the **exact same architecture** as `Z3Solver`:

```
Z3Context
├── CreateSolver() → Z3Solver
└── CreateOptimizer() → Z3Optimize  ← NEW

Both managed via:
- Reference counting (IncRef/DecRef)
- Context tracking (HashSet<T>)
- Model invalidation
- IDisposable pattern
```

## Implementation Plan

### Phase 0: Typed Objective Handle

**File:** `Z3Wrap/Core/OptimizeObjective.cs`

Type-safe wrapper for optimization objective IDs that preserves expression type information:

```csharp
/// <summary>
/// Represents a typed optimization objective with compile-time type safety.
/// </summary>
/// <typeparam name="TExpr">The type of expression being optimized.</typeparam>
public sealed class OptimizeObjective<TExpr> where TExpr : Z3Expr
{
    internal OptimizeObjective(uint objectiveId)
    {
        ObjectiveId = objectiveId;
    }

    /// <summary>
    /// Gets the internal objective ID used by Z3.
    /// </summary>
    internal uint ObjectiveId { get; }
}
```

**Benefits:**
- Compile-time type safety for GetUpper/GetLower
- No casting required by users
- Clear API: `var obj = opt.Maximize(x); var upper = opt.GetUpper(obj);`
- Overload resolution works correctly (differs by return type)

### Phase 1: Core Z3Optimize Class

**File:** `Z3Wrap/Core/Z3Optimize.cs`

**Structure (mirror Z3Solver.cs):**
```csharp
public sealed class Z3Optimize : IDisposable
{
    private readonly Z3Context context;
    private Z3Model? cachedModel;
    private bool disposed;
    private bool isBeingDisposedByContext;
    private Z3Status? lastCheckResult;
    private readonly List<uint> objectiveIds = new();

    // Constructor (internal - created via context)
    internal Z3Optimize(Z3Context context)
    {
        this.context = context;
        InternalHandle = context.Library.MkOptimize(context.Handle);
        context.Library.OptimizeIncRef(context.Handle, InternalHandle);
    }

    // Properties
    public IntPtr Handle { get; }
    private IntPtr InternalHandle { get; }

    // Hard constraints
    public void Assert(BoolExpr constraint)
    public void AssertAndTrack(BoolExpr constraint, BoolExpr tracker)

    // Soft constraints (with weights/penalties)
    public uint AssertSoft(BoolExpr constraint, string weight, string id = "")

    // Objectives (returns typed objective handle for type-safe result retrieval)
    // Z3 accepts any "arithmetical term": Int, Real, BitVector
    public OptimizeObjective<IntExpr> Maximize(IntExpr expr)
    public OptimizeObjective<RealExpr> Maximize(RealExpr expr)
    public OptimizeObjective<BvExpr<TSize>> Maximize<TSize>(BvExpr<TSize> expr) where TSize : struct, IBvSize
    public OptimizeObjective<IntExpr> Minimize(IntExpr expr)
    public OptimizeObjective<RealExpr> Minimize(RealExpr expr)
    public OptimizeObjective<BvExpr<TSize>> Minimize<TSize>(BvExpr<TSize> expr) where TSize : struct, IBvSize

    // Solving
    public Z3Status Check()
    public Z3Status Check(params BoolExpr[] assumptions)
    public string GetReasonUnknown()

    // Model extraction
    public Z3Model GetModel()

    // Objective results (type-safe via OptimizeObjective<T>)
    public TExpr GetUpper<TExpr>(OptimizeObjective<TExpr> objective) where TExpr : Z3Expr
    public TExpr GetLower<TExpr>(OptimizeObjective<TExpr> objective) where TExpr : Z3Expr

    // Alternative: Get as string representation
    public string GetUpperAsString<TExpr>(OptimizeObjective<TExpr> objective) where TExpr : Z3Expr
    public string GetLowerAsString<TExpr>(OptimizeObjective<TExpr> objective) where TExpr : Z3Expr

    // Backtracking
    public void Push()
    public void Pop(uint numScopes = 1)

    // Parameters
    public void SetParams(Z3Params parameters)

    // Utilities
    public override string ToString()
    public void Reset()  // if supported by Z3

    // Disposal
    public void Dispose()
    internal void InternalDispose()
    private void InvalidateModel()
    private void ThrowIfDisposed()
}
```

**Key Implementation Details:**

1. **Reference Counting:**
   ```csharp
   internal Z3Optimize(Z3Context context)
   {
       this.context = context;
       InternalHandle = context.Library.MkOptimize(context.Handle);
       context.Library.OptimizeIncRef(context.Handle, InternalHandle);
   }

   internal void InternalDispose()
   {
       InvalidateModel();
       context.Library.OptimizeDecRef(context.Handle, InternalHandle);
       disposed = true;
   }
   ```

2. **Model Invalidation (copy from Z3Solver):**
   ```csharp
   private void InvalidateModel()
   {
       cachedModel?.Invalidate();
       cachedModel = null;
   }

   public void Assert(BoolExpr constraint)
   {
       ThrowIfDisposed();
       InvalidateModel();  // Model no longer valid
       context.Library.OptimizeAssert(context.Handle, InternalHandle, constraint.Handle);
   }
   ```

3. **Check with Status Tracking:**
   ```csharp
   public Z3Status Check()
   {
       ThrowIfDisposed();
       InvalidateModel();

       lastCheckResult = context.Library.OptimizeCheck(context.Handle, InternalHandle, 0, Array.Empty<IntPtr>()) switch
       {
           Z3Library.Lbool.Z3_L_FALSE => Z3Status.Unsatisfiable,
           Z3Library.Lbool.Z3_L_TRUE => Z3Status.Satisfiable,
           Z3Library.Lbool.Z3_L_UNDEF => Z3Status.Unknown,
           _ => throw new InvalidOperationException("Unexpected result"),
       };
       return lastCheckResult.Value;
   }
   ```

4. **Objective Result Extraction (Type-Safe):**
   ```csharp
   public TExpr GetUpper<TExpr>(OptimizeObjective<TExpr> objective) where TExpr : Z3Expr
   {
       ThrowIfDisposed();
       if (lastCheckResult != Z3Status.Satisfiable)
           throw new InvalidOperationException("Cannot get objective value when not satisfiable");

       var astHandle = context.Library.OptimizeGetUpper(
           context.Handle,
           InternalHandle,
           objective.ObjectiveId
       );

       // Use Z3Expr factory to create the right type
       return (TExpr)Z3Expr.Create(context, astHandle);
   }

   public TExpr GetLower<TExpr>(OptimizeObjective<TExpr> objective) where TExpr : Z3Expr
   {
       // Same pattern as GetUpper
   }
   ```

   **Note:** Requires adding a factory method to Z3Expr base class:
   ```csharp
   // In Z3Expr.cs
   internal static Z3Expr Create(Z3Context context, IntPtr handle)
   {
       // Determine sort kind and create appropriate typed expression
       var sortKind = context.Library.GetSortKind(context.Handle,
           context.Library.GetSort(context.Handle, handle));

       return sortKind switch
       {
           Z3Library.SortKind.IntSort => new IntExpr(context, handle),
           Z3Library.SortKind.RealSort => new RealExpr(context, handle),
           Z3Library.SortKind.BvSort => CreateBvExpr(context, handle),
           _ => throw new NotSupportedException($"Sort kind {sortKind} not supported")
       };
   }
   ```

### Phase 2: Context Integration

**File:** `Z3Wrap/Core/Z3Context.cs`

**Changes needed:**

1. **Add optimizer tracking field:**
   ```csharp
   private readonly HashSet<Z3Optimizer> trackedOptimizers = [];
   ```

2. **Add factory method:**
   ```csharp
   /// <summary>
   /// Creates a new optimizer instance for this context.
   /// </summary>
   /// <returns>A new optimizer instance.</returns>
   public Z3Optimize CreateOptimizer()
   {
       var optimizer = new Z3Optimize(this);
       TrackOptimizer(optimizer);
       return optimizer;
   }
   ```

3. **Add tracking methods:**
   ```csharp
   private void TrackOptimizer(Z3Optimize optimizer)
   {
       ThrowIfDisposed();
       trackedOptimizers.Add(optimizer);
   }

   private void UntrackOptimizer(Z3Optimize optimizer)
   {
       trackedOptimizers.Remove(optimizer);
   }

   internal void DisposeOptimizer(Z3Optimize optimizer)
   {
       if (disposed)
           return;

       UntrackOptimizer(optimizer);
       optimizer.InternalDispose();
   }
   ```

4. **Update DisposeCore to cleanup optimizers:**
   ```csharp
   private void DisposeCore()
   {
       if (disposed)
           return;

       // Dispose all tracked optimizers
       foreach (var optimizer in trackedOptimizers.ToList())
           optimizer.InternalDispose();
       trackedOptimizers.Clear();

       // ... existing solver disposal code ...
   }
   ```

### Phase 3: Extension Methods (Optional)

**File:** `Z3Wrap/Core/Z3OptimizeExtensions.cs`

Natural syntax helpers (similar to solver parameter extensions):

```csharp
public static class Z3OptimizeExtensions
{
    // Type-safe maximize/minimize based on expression type
    public static uint Maximize(this Z3Optimize opt, NumericExpr expr) { ... }
    public static uint Minimize(this Z3Optimize opt, NumericExpr expr) { ... }

    // Soft constraint overloads
    public static uint AssertSoft(this Z3Optimize opt, BoolExpr constraint, int weight)
        => opt.AssertSoft(constraint, weight.ToString(), "");

    public static uint AssertSoft(this Z3Optimize opt, BoolExpr constraint, double weight)
        => opt.AssertSoft(constraint, weight.ToString(), "");
}
```

### Phase 4: Comprehensive Tests

**File:** `Z3Wrap.Tests/Core/Z3OptimizeTests.cs`

**Test categories (mirror Z3SolverTests structure):**

1. **Creation and Disposal:**
   - CreateOptimizer returns valid instance
   - Dispose releases resources
   - Context disposes all optimizers
   - Disposed optimizer throws ObjectDisposedException

2. **Hard Constraints:**
   - Assert adds constraints
   - Multiple assertions
   - AssertAndTrack with tracker

3. **Soft Constraints:**
   - AssertSoft with positive weight
   - AssertSoft with negative weight (reward)
   - Multiple soft constraints
   - Soft constraint groups (via id parameter)

4. **Optimization Objectives:**
   - Maximize integer expression
   - Minimize integer expression
   - Maximize real expression
   - Minimize real expression
   - Maximize bitvector expression
   - Minimize bitvector expression
   - Multiple objectives
   - Retrieve objective IDs

5. **Solving:**
   - Check returns Satisfiable/Unsatisfiable/Unknown
   - GetReasonUnknown for Unknown status
   - Check with assumptions
   - Multiple Check calls

6. **Model Extraction:**
   - GetModel after Satisfiable
   - GetModel throws before Check
   - GetModel throws when Unsatisfiable
   - Model values match constraints

7. **Objective Results:**
   - GetUpper/GetLower for integer objectives
   - GetUpper/GetLower for real objectives
   - GetUpper/GetLower for bitvector objectives
   - GetUpperAsVector/GetLowerAsVector (infinity + epsilon encoding)
   - Results match expected optimization

8. **Model Invalidation:**
   - Model invalidated after Assert
   - Model invalidated after AssertSoft
   - Model invalidated after Maximize/Minimize
   - Model invalidated after Push/Pop
   - Model invalidated after Check

9. **Backtracking:**
   - Push/Pop work correctly
   - Constraints removed after Pop
   - Multiple Push/Pop levels
   - Pop throws with too many scopes

10. **Parameters:**
    - SetParams applies parameters
    - Parameters affect behavior

11. **String Representation:**
    - ToString returns valid SMT-LIB2

12. **Real-World Example:**
    ```csharp
    [Test]
    public void LinearProgramming_Example()
    {
        // Maximize 3x + 2y
        // Subject to: x + y <= 10
        //             x >= 0, y >= 0

        using var context = new Z3Context();
        using var scope = context.SetUp();
        using var opt = context.CreateOptimizer();

        var x = context.IntConst("x");
        var y = context.IntConst("y");

        opt.Assert(x + y <= 10);
        opt.Assert(x >= 0);
        opt.Assert(y >= 0);

        // Returns OptimizeObjective<IntExpr> for type-safe result access
        var objective = opt.Maximize(3 * x + 2 * y);

        var status = opt.Check();
        Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

        var model = opt.GetModel();

        // GetUpper returns IntExpr automatically (no cast needed!)
        var optimalValue = opt.GetUpper(objective);

        // Optimal: x=10, y=0, objective=30
        Assert.That(model.GetIntValue(x), Is.EqualTo(10));
        Assert.That(model.GetIntValue(y), Is.EqualTo(0));
        Assert.That(model.GetIntValue(optimalValue), Is.EqualTo(30));
    }
    ```

**Estimated test count:** ~40-50 tests (similar to Z3SolverTests coverage)

### Phase 5: Documentation Updates

1. **README.md** - Add optimization example:
   ```markdown
   ### Optimization
   ```csharp
   using var optimizer = context.CreateOptimizer();

   // Constraints
   optimizer.Assert(x + y <= 100);
   optimizer.Assert(x >= 0);

   // Objective (returns typed handle)
   var objective = optimizer.Maximize(3 * x + 2 * y);

   if (optimizer.Check() == Z3Status.Satisfiable)
   {
       var model = optimizer.GetModel();
       var optimalValue = optimizer.GetUpper(objective);  // Type-safe!
       Console.WriteLine($"Optimal value: {model.GetIntValue(optimalValue)}");
   }
   ```
   ```

2. **CHANGELOG.md** - Update [Unreleased]:
   ```markdown
   ### Added
   - **Z3 Optimization support**
     - `Z3Optimize` class for minimize/maximize objectives
     - Hard constraints via `Assert()` and soft constraints via `AssertSoft()`
     - `Maximize()` and `Minimize()` for optimization objectives
     - `GetUpper()` and `GetLower()` for optimal value bounds
     - Push/Pop backtracking support
   ```

3. **PLAN.md** - Update status:
   ```markdown
   - ✅ **Optimization** (`Z3Optimize` API for min/max objectives)
   ```

4. **ANALYSIS.md** - Update Priority 1C:
   ```markdown
   - ✅ Optimization (complete implementation)
   ```

5. **XML Documentation** - All public methods must have complete XML docs (zero warnings enforced)

## Testing Strategy

1. **Pattern Matching:** Mirror Z3SolverTests structure exactly
2. **Coverage Goal:** Maintain ≥90% coverage requirement
3. **Test Execution:** `make test` must pass all tests
4. **Format Checking:** `make lint` must pass (zero warnings)
5. **README Validation:** Add optimization example to ReadmeExamplesTests.cs

## Success Criteria

- [ ] `OptimizeObjective<TExpr>` class created
- [ ] `Z3Optimize` class implemented with all 15+ public methods
- [ ] `Z3Expr.Create()` factory method added for dynamic type creation
- [ ] Context integration complete (tracking, disposal)
- [ ] All tests pass (`make test`)
- [ ] Coverage ≥90% maintained (`make coverage`)
- [ ] Zero XML documentation warnings (`make build`)
- [ ] Code formatted (`make lint` passes)
- [ ] CI pipeline passes (`make ci`)
- [ ] README examples updated and validated
- [ ] CHANGELOG.md updated

## Development Workflow

1. Create `OptimizeObjective.cs` (typed objective handle)
2. Add `Z3Expr.Create()` factory method to Z3Expr base class
3. Create `Z3Optimize.cs` (copy structure from Z3Solver.cs)
4. Update `Z3Context.cs` (add optimizer tracking)
5. Create `Z3OptimizeTests.cs` (mirror Z3SolverTests structure)
6. Run `make test` frequently during development
7. Run `make format` before commit
8. Verify `make ci` passes before PR

## Estimated Effort

- **Phase 0 (OptimizeObjective + Z3Expr.Create):** 1 hour
- **Phase 1 (Z3Optimize):** 2-3 hours
- **Phase 2 (Context integration):** 30 minutes
- **Phase 3 (Extensions - optional):** 30 minutes
- **Phase 4 (Tests):** 3-4 hours
- **Phase 5 (Documentation):** 30 minutes

**Total:** ~8-10 hours for complete implementation

## Notes

- Z3Library methods already handle error checking (CheckError)
- Follow exact same patterns as Z3Solver (proven architecture)
- Model invalidation is critical for correctness
- Reference counting prevents memory leaks
- All existing infrastructure (Z3Status, Z3Model, Z3Params) can be reused
- No changes needed to expression types (IntExpr, RealExpr, etc.)
- Optimization is an **advanced solver feature**, not a separate theory
