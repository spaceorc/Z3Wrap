# Z3Wrap Optimization Feature - Implementation Plan

## Overview

Add optimization capabilities to Z3Wrap, enabling maximize/minimize objectives for constraint satisfaction problems. This extends beyond basic satisfiability to find optimal solutions.

## Current State

**What We Have**:
- ✅ Basic solver (`Z3Solver`) with assert/check/model extraction
- ✅ Reference-counted context management
- ✅ Strong typing and natural syntax patterns
- ✅ Comprehensive test coverage (93.3%)

**What's Missing**:
- ❌ Optimization context (`Z3_mk_optimize`)
- ❌ Maximize/minimize objectives
- ❌ Multi-objective optimization
- ❌ Objective value extraction from models

## Architecture Design

### Core Classes

```csharp
namespace Z3Wrap.Core
{
    /// <summary>
    /// Represents a Z3 optimization context for maximize/minimize problems.
    /// </summary>
    public sealed class Z3Optimizer : IDisposable
    {
        // Core properties
        public Z3Context Context { get; }
        internal IntPtr Handle { get; }

        // Constructor (internal - created via context)
        internal Z3Optimizer(Z3Context context, IntPtr handle);

        // Constraint management (same as Z3Solver)
        public void Assert(BoolExpr constraint);
        public void Push();
        public void Pop(uint count = 1);
        public void Reset();

        // Optimization objectives (NEW)
        public Z3OptimizeHandle Maximize(IntExpr objective);
        public Z3OptimizeHandle Maximize(RealExpr objective);
        public Z3OptimizeHandle Minimize(IntExpr objective);
        public Z3OptimizeHandle Minimize(RealExpr objective);

        // Solving
        public Z3Status Check();
        public Z3Model GetModel();

        // Parameter configuration
        public void SetParams(Z3Params parameters);

        // Resource management
        public void Dispose();
    }

    /// <summary>
    /// Handle for optimization objective to extract optimal value.
    /// </summary>
    public sealed class Z3OptimizeHandle
    {
        internal IntPtr Handle { get; }
        internal Z3Optimizer Optimizer { get; }

        // Extract optimal value from model
        public string GetValueAsString();
        public BigInteger GetIntValue();
        public Real GetRealValue();
    }
}
```

### Context Extension

```csharp
namespace Z3Wrap.Extensions.Optimization
{
    public static class OptimizerContextExtensions
    {
        /// <summary>
        /// Creates optimization context for maximize/minimize problems.
        /// </summary>
        public static Z3Optimizer CreateOptimizer(this Z3Context context)
        {
            var handle = NativeLibrary.Z3_mk_optimize(context.Handle);
            NativeLibrary.Z3_optimize_inc_ref(context.Handle, handle);
            return new Z3Optimizer(context, handle);
        }
    }
}
```

## Native Library Status

✅ **NativeLibrary.Optimization.cs already exists** with ALL required Z3 optimization functions:
- Creation/management: `MkOptimize`, `OptimizeIncRef`, `OptimizeDecRef`
- Assertions: `OptimizeAssert`, `OptimizeAssertSoft`, `OptimizeAssertAndTrack`
- Objectives: `OptimizeMaximize`, `OptimizeMinimize`
- Solving: `OptimizeCheck`, `OptimizeGetModel`
- Results: `OptimizeGetUpper`, `OptimizeGetLower`, `OptimizeGetReasonUnknown`
- Utilities: `OptimizeToString`, `OptimizeSetParams`, etc.

## Z3Library Wrapper Functions Needed

Add to `Z3Wrap/Core/Z3Library.cs` following the existing pattern:

```csharp
// Optimization operations (add near line 1040 after solver operations)

/// <inheritdoc cref="NativeLibrary.MkOptimize" />
public IntPtr MkOptimize(IntPtr ctx)
{
    var result = nativeLibrary.MkOptimize(ctx);
    CheckError(ctx);
    return CheckHandle(result, nameof(MkOptimize));
}

/// <inheritdoc cref="NativeLibrary.OptimizeIncRef" />
public void OptimizeIncRef(IntPtr ctx, IntPtr optimize)
{
    nativeLibrary.OptimizeIncRef(ctx, optimize);
    CheckError(ctx);
}

/// <inheritdoc cref="NativeLibrary.OptimizeDecRef" />
public void OptimizeDecRef(IntPtr ctx, IntPtr optimize)
{
    nativeLibrary.OptimizeDecRef(ctx, optimize);
    CheckError(ctx);
}

/// <inheritdoc cref="NativeLibrary.OptimizeAssert" />
public void OptimizeAssert(IntPtr ctx, IntPtr optimize, IntPtr constraint)
{
    nativeLibrary.OptimizeAssert(ctx, optimize, constraint);
    CheckError(ctx);
}

/// <inheritdoc cref="NativeLibrary.OptimizeMaximize" />
public uint OptimizeMaximize(IntPtr ctx, IntPtr optimize, IntPtr objective)
{
    var result = nativeLibrary.OptimizeMaximize(ctx, optimize, objective);
    CheckError(ctx);
    return result;
}

/// <inheritdoc cref="NativeLibrary.OptimizeMinimize" />
public uint OptimizeMinimize(IntPtr ctx, IntPtr optimize, IntPtr objective)
{
    var result = nativeLibrary.OptimizeMinimize(ctx, optimize, objective);
    CheckError(ctx);
    return result;
}

/// <inheritdoc cref="NativeLibrary.OptimizeCheck" />
public int OptimizeCheck(IntPtr ctx, IntPtr optimize, uint numAssumptions, IntPtr[] assumptions)
{
    var result = nativeLibrary.OptimizeCheck(ctx, optimize, numAssumptions, assumptions);
    CheckError(ctx);
    return result;
}

/// <inheritdoc cref="NativeLibrary.OptimizeGetModel" />
public IntPtr OptimizeGetModel(IntPtr ctx, IntPtr optimize)
{
    var result = nativeLibrary.OptimizeGetModel(ctx, optimize);
    CheckError(ctx);
    return CheckHandle(result, nameof(OptimizeGetModel));
}

/// <inheritdoc cref="NativeLibrary.OptimizeGetUpper" />
public IntPtr OptimizeGetUpper(IntPtr ctx, IntPtr optimize, uint idx)
{
    var result = nativeLibrary.OptimizeGetUpper(ctx, optimize, idx);
    CheckError(ctx);
    return CheckHandle(result, nameof(OptimizeGetUpper));
}

/// <inheritdoc cref="NativeLibrary.OptimizeGetLower" />
public IntPtr OptimizeGetLower(IntPtr ctx, IntPtr optimize, uint idx)
{
    var result = nativeLibrary.OptimizeGetLower(ctx, optimize, idx);
    CheckError(ctx);
    return CheckHandle(result, nameof(OptimizeGetLower));
}

/// <inheritdoc cref="NativeLibrary.OptimizeSetParams" />
public void OptimizeSetParams(IntPtr ctx, IntPtr optimize, IntPtr paramsHandle)
{
    nativeLibrary.OptimizeSetParams(ctx, optimize, paramsHandle);
    CheckError(ctx);
}

/// <inheritdoc cref="NativeLibrary.OptimizeToString" />
public string? OptimizeToString(IntPtr ctx, IntPtr optimize)
{
    var result = nativeLibrary.OptimizeToString(ctx, optimize);
    CheckError(ctx);
    return Marshal.PtrToStringAnsi(CheckHandle(result, nameof(OptimizeToString)));
}

/// <inheritdoc cref="NativeLibrary.OptimizeGetReasonUnknown" />
public string? OptimizeGetReasonUnknown(IntPtr ctx, IntPtr optimize)
{
    var result = nativeLibrary.OptimizeGetReasonUnknown(ctx, optimize);
    CheckError(ctx);
    return Marshal.PtrToStringAnsi(CheckHandle(result, nameof(OptimizeGetReasonUnknown)));
}
```

**Pattern Notes**:
- Use `/// <inheritdoc cref="NativeLibrary.MethodName" />` for documentation
- Call `CheckError(ctx)` after every native call
- Use `CheckHandle(result, nameof(MethodName))` for IntPtr returns
- Use `Marshal.PtrToStringAnsi()` for string returns
- No custom XML documentation needed - inherits from NativeLibrary

## Implementation Phases

### Phase 1: Core Infrastructure (2-3 days)

**Files to Modify**:
- `Z3Wrap/Core/Z3Library.cs` - Add optimization wrapper methods

**Files to Create**:
- `Z3Wrap/Core/Z3Optimizer.cs` - Main optimizer class
- `Z3Wrap/Core/Z3OptimizeHandle.cs` - Objective handle class
- `Z3Wrap/Extensions/Optimization/OptimizerContextExtensions.cs` - Context creation

**Tasks**:
1. ✅ **Skip P/Invoke** - Already exists in `NativeLibrary.Optimization.cs`
2. Add optimization wrappers to `Z3Library.cs` (13 methods following existing pattern)
3. Implement `Z3Optimizer` class with reference counting
4. Implement `Z3OptimizeHandle` for objective tracking
5. Add optimizer to context's disposal tracking (see `Z3Context.TrackSolver()`)
6. Create `CreateOptimizer()` extension method

**Success Criteria**:
- `make build` succeeds with zero warnings
- Can create and dispose optimizer without crashes
- Reference counting prevents memory leaks

---

### Phase 2: Basic Optimization API (2-3 days)

**Files to Create**:
- `Z3Wrap.Tests/Core/Z3OptimizerTests.cs` - Basic optimizer tests
- `Z3Wrap.Tests/Optimization/OptimizerSingleObjectiveTests.cs` - Single objective tests

**Tasks**:
1. Implement `Assert()`, `Push()`, `Pop()`, `Reset()` methods
2. Implement `Maximize()` and `Minimize()` for `IntExpr` and `RealExpr`
3. Implement `Check()` and `GetModel()` methods
4. Implement `Z3OptimizeHandle.GetValueAsString()`
5. Write comprehensive tests for single-objective optimization

**Test Coverage**:
- Create optimizer and assert constraints
- Maximize integer expression
- Minimize integer expression
- Maximize real expression
- Minimize real expression
- Extract optimal value from model
- Push/pop with objectives
- Reset optimizer state

**Example Test**:
```csharp
[Test]
public void Maximize_IntegerObjective_FindsOptimalSolution()
{
    using var context = new Z3Context();
    using var scope = context.SetUp();
    using var optimizer = context.CreateOptimizer();

    var x = context.IntConst("x");
    var y = context.IntConst("y");

    // Constraints: x + y <= 100, x >= 0, y >= 0
    optimizer.Assert(x + y <= 100);
    optimizer.Assert(x >= 0);
    optimizer.Assert(y >= 0);

    // Objective: maximize x + y
    var handle = optimizer.Maximize(x + y);

    var status = optimizer.Check();
    Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

    var model = optimizer.GetModel();
    var sum = model.GetIntValue(x) + model.GetIntValue(y);
    Assert.That(sum, Is.EqualTo(new BigInteger(100)));

    var optimalValue = handle.GetIntValue();
    Assert.That(optimalValue, Is.EqualTo(new BigInteger(100)));
}
```

**Success Criteria**:
- All basic optimization tests pass
- Can solve single-objective integer problems
- Can solve single-objective real problems
- Optimal values extracted correctly
- Test coverage remains ≥90%

---

### Phase 3: Multi-Objective Optimization (1-2 days)

**Files to Create**:
- `Z3Wrap.Tests/Optimization/OptimizerMultiObjectiveTests.cs` - Multi-objective tests

**Tasks**:
1. Test multiple `Maximize()`/`Minimize()` calls on same optimizer
2. Implement Pareto-optimal solution handling
3. Test objective priority and trade-offs
4. Add tests for conflicting objectives

**Test Coverage**:
- Multiple objectives on same optimizer
- Maximize + minimize combinations
- Lexicographic ordering of objectives
- Extract all objective values

**Example Test**:
```csharp
[Test]
public void MultipleObjectives_FindsLexicographicOptimum()
{
    using var context = new Z3Context();
    using var scope = context.SetUp();
    using var optimizer = context.CreateOptimizer();

    var x = context.IntConst("x");
    var y = context.IntConst("y");

    optimizer.Assert(x + y <= 100);
    optimizer.Assert(x >= 0 && y >= 0);

    // First priority: maximize x + y
    var h1 = optimizer.Maximize(x + y);
    // Second priority: maximize x
    var h2 = optimizer.Maximize(x);

    var status = optimizer.Check();
    Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

    var model = optimizer.GetModel();
    Assert.That(model.GetIntValue(x) + model.GetIntValue(y), Is.EqualTo(new BigInteger(100)));
    Assert.That(model.GetIntValue(x), Is.EqualTo(new BigInteger(100))); // Lexicographic optimum
}
```

**Success Criteria**:
- Multi-objective problems solve correctly
- Lexicographic ordering respected
- All objective values extractable
- Test coverage remains ≥90%

---

### Phase 4: Advanced Features & Polish (1-2 days)

**Files to Create**:
- `Z3Wrap.Tests/Optimization/OptimizerParameterTests.cs` - Parameter tests
- `Z3Wrap.Tests/Optimization/OptimizerEdgeCaseTests.cs` - Edge case tests

**Tasks**:
1. Implement `SetParams()` for optimizer configuration
2. Add parameter convenience extensions (timeout, etc.)
3. Test unbounded objectives (infinite solutions)
4. Test infeasible problems
5. Test edge cases (empty constraints, trivial objectives)
6. Add XML documentation to all public APIs
7. Update README with optimization examples

**Test Coverage**:
- Timeout configuration
- Unbounded objectives detection
- Infeasible constraint handling
- Empty optimizer behavior
- Resource cleanup edge cases

**Success Criteria**:
- All edge cases handled gracefully
- XML documentation complete (`make build` zero warnings)
- README updated with examples
- Test coverage ≥90%
- Full `make ci` passes

---

## README Examples to Add

### Basic Optimization
```csharp
using var context = new Z3Context();
using var scope = context.SetUp();
using var optimizer = context.CreateOptimizer();

var x = context.IntConst("x");
var y = context.IntConst("y");

// Maximize profit: 3x + 5y subject to constraints
optimizer.Assert(x + 2*y <= 20);  // Resource constraint
optimizer.Assert(3*x + y <= 30);  // Time constraint
optimizer.Assert(x >= 0 && y >= 0);

var profitHandle = optimizer.Maximize(3*x + 5*y);

if (optimizer.Check() == Z3Status.Satisfiable)
{
    var model = optimizer.GetModel();
    Console.WriteLine($"x = {model.GetIntValue(x)}");
    Console.WriteLine($"y = {model.GetIntValue(y)}");
    Console.WriteLine($"Max profit = {profitHandle.GetIntValue()}");
}
```

### Multi-Objective Optimization
```csharp
// Lexicographic optimization: first maximize quality, then minimize cost
var qualityHandle = optimizer.Maximize(quality);
var costHandle = optimizer.Minimize(cost);

if (optimizer.Check() == Z3Status.Satisfiable)
{
    Console.WriteLine($"Optimal quality: {qualityHandle.GetIntValue()}");
    Console.WriteLine($"Minimal cost: {costHandle.GetIntValue()}");
}
```

## Testing Strategy

### Test Organization
```
Z3Wrap.Tests/
├── Core/
│   └── Z3OptimizerTests.cs              # Basic optimizer lifecycle
├── Optimization/
│   ├── OptimizerSingleObjectiveTests.cs # Single maximize/minimize
│   ├── OptimizerMultiObjectiveTests.cs  # Multiple objectives
│   ├── OptimizerParameterTests.cs       # Configuration
│   └── OptimizerEdgeCaseTests.cs        # Edge cases
└── ReadmeExamplesTests.cs               # Validate README examples
```

### Test Principles
- Follow existing patterns from `Z3SolverTests.cs`
- Test all syntax variants (context extensions, direct methods)
- Verify actual optimal values, not just satisfiability
- Test resource cleanup and disposal
- Maintain 90%+ coverage requirement

### Performance Tests
- Small problems (< 10 variables): < 1 second
- Medium problems (10-100 variables): < 10 seconds
- Large problems: Use timeout parameters

## Integration Checklist

- [ ] Phase 1: Core infrastructure implemented
- [ ] Phase 2: Basic optimization API working
- [ ] Phase 3: Multi-objective support complete
- [ ] Phase 4: Advanced features and polish
- [ ] All tests passing (`make test`)
- [ ] Coverage ≥90% (`make coverage`)
- [ ] XML documentation complete (`make build` zero warnings)
- [ ] Code formatted (`make format`)
- [ ] Linting passed (`make lint`)
- [ ] Full CI passed (`make ci`)
- [ ] README updated with examples
- [ ] `ReadmeExamplesTests.cs` updated and passing
- [ ] ANALYSIS.md updated with new coverage stats

## Risk Mitigation

### Memory Management
- **Risk**: Optimizer handle leaks
- **Mitigation**: Reference counting via `Z3_optimize_inc_ref/dec_ref`
- **Verification**: Dispose tests with resource tracking

### API Consistency
- **Risk**: Different patterns from Z3Solver
- **Mitigation**: Mirror Z3Solver API design exactly
- **Verification**: Code review for consistency

### Edge Cases
- **Risk**: Unbounded or infeasible problems crash
- **Mitigation**: Comprehensive edge case testing
- **Verification**: Dedicated edge case test suite

### Performance
- **Risk**: Optimization slower than expected
- **Mitigation**: Parameter tuning support, timeout configuration
- **Verification**: Performance benchmarks

## Success Metrics

### Functionality
- ✅ Single-objective optimization (maximize/minimize)
- ✅ Multi-objective optimization (lexicographic)
- ✅ Optimal value extraction
- ✅ Parameter configuration

### Quality
- ✅ Test coverage ≥90%
- ✅ All `make ci` checks pass
- ✅ XML documentation complete
- ✅ README examples validated

### Timeline
- **Total Estimate**: 6-8 days
- **Phase 1**: 2-3 days
- **Phase 2**: 2-3 days
- **Phase 3**: 1-2 days
- **Phase 4**: 1-2 days

## Next Steps After Completion

After optimization is complete, next priorities from ANALYSIS.md:
1. **Unsat Cores** - Debug impossible constraints (3-4 days)
2. **Tactics Configuration** - Performance tuning (1 week)
3. **String Theory** - Web security analysis (3-4 weeks)

This completes the Advanced Solver Features section, bringing coverage from 30% → 100%.
