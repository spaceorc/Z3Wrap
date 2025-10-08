# PLAN: Sort Validation for Expression Construction

## Status: In Progress

## Goal

Add runtime sort validation when constructing expressions to:
1. Catch sort mismatches early with clear error messages
2. Determine the actual bitvector size returned by `char.to_bv` operations
3. Ensure type safety matches Z3's actual behavior

## Background

Currently, expressions are created without validating that the Z3 AST handle has the expected sort. This can lead to:
- Silent bugs when wrong expression types are used
- Assumptions about bitvector sizes (e.g., char.to_bv returns ??-bit bitvector)
- Confusing errors deep in Z3 instead of at construction time

## Design

### 1. Z3 API Functions Available

```csharp
// Get sort from expression
IntPtr GetSort(IntPtr context, IntPtr ast)

// Get sort kind (INT, REAL, BV, BOOL, ARRAY, etc.)
SortKind GetSortKind(IntPtr context, IntPtr sort)

// Get bitvector size (only for BV_SORT)
uint GetBvSortSize(IntPtr context, IntPtr sort)

// Compare sorts for equality
// Can use GetSortId for numeric comparison or string comparison
```

### 2. Validation Strategy

**Option A: Validate in Z3Expr.Create<T> (RECOMMENDED)**
- Single point of validation for all expression creation
- Validates sort matches expected type T
- Can be conditionally compiled for performance

**Option B: Validate in each expression type's Create method**
- More granular control
- More code duplication

**Decision: Use Option A**

### 3. Implementation Plan

#### Step 1: Add Sort Validation Method to Z3Context

```csharp
// In Z3Context.cs
internal void ValidateExpressionSort<T>(IntPtr astHandle)
    where T : Z3Expr, IExprType<T>
{
    var actualSort = Library.GetSort(Handle, astHandle);
    var expectedSort = T.Sort(this);

    // Compare sorts (need to determine best comparison method)
    var actualSortKind = Library.GetSortKind(Handle, actualSort);
    var expectedSortKind = Library.GetSortKind(Handle, expectedSort);

    if (actualSortKind != expectedSortKind)
    {
        throw new InvalidOperationException(
            $"Sort mismatch: expected {expectedSortKind} but got {actualSortKind} " +
            $"when creating {typeof(T).Name}"
        );
    }

    // For bitvectors, also validate size
    if (actualSortKind == SortKind.Z3_BV_SORT)
    {
        var actualSize = Library.GetBvSortSize(Handle, actualSort);
        var expectedSize = Library.GetBvSortSize(Handle, expectedSort);

        if (actualSize != expectedSize)
        {
            throw new InvalidOperationException(
                $"Bitvector size mismatch: expected {expectedSize} bits " +
                $"but got {actualSize} bits when creating {typeof(T).Name}"
            );
        }
    }
}
```

#### Step 2: Modify Z3Expr.Create<T> to Validate

```csharp
// In Z3Expr.cs
internal static T Create<T>(Z3Context context, IntPtr handle)
    where T : Z3Expr, IExprType<T>
{
#if DEBUG
    context.ValidateExpressionSort<T>(handle);
#endif
    return T.Create(context, handle);
}
```

**Note**: Using `#if DEBUG` makes validation opt-in during development but doesn't impact production performance.

#### Step 3: Add Helper to Get Bitvector Size from Expression

```csharp
// In Z3Context.cs or new Z3Sort helper class
public uint GetBvSizeFromExpr(IntPtr astHandle)
{
    var sort = Library.GetSort(Handle, astHandle);
    var sortKind = Library.GetSortKind(Handle, sort);

    if (sortKind != SortKind.Z3_BV_SORT)
    {
        throw new InvalidOperationException(
            $"Expression is not a bitvector, got sort kind: {sortKind}"
        );
    }

    return Library.GetBvSortSize(Handle, sort);
}
```

#### Step 4: Discover Actual char.to_bv Size

```csharp
// Test to discover the size
[Test]
public void CharToBv_DiscoverBitvectorSize()
{
    using var context = new Z3Context();
    using var scope = context.SetUp();

    var ch = context.Char('A');
    var bvHandle = context.Library.SeqToRe(context.Handle, ch.Handle); // or whatever char.to_bv is

    var size = context.GetBvSizeFromExpr(bvHandle);
    Console.WriteLine($"char.to_bv returns {size}-bit bitvector");

    // Then we can update our code accordingly
}
```

#### Step 5: Update CharExpr ToBv Methods

Based on discovered size, update:
- `CharExpr.ToBv()` return type
- `CharExpr.ToBv<TSize>()` validation
- `CharContextExtensions.ToBv()` return type
- `BvExprCharExtensions.ToChar()` parameter type
- All related tests

### 4. Alternative: Make char.to_bv Size Dynamic

If the size varies based on Z3 configuration, we could:

```csharp
// In CharExpr
public BvExpr<TSize> ToBv<TSize>() where TSize : ISize
{
    var bvHandle = Context.Library.SeqCharToBv(...);
    var actualSize = Context.GetBvSizeFromExpr(bvHandle);

    if (actualSize != TSize.Size)
    {
        throw new InvalidOperationException(
            $"char.to_bv returns {actualSize}-bit bitvector, " +
            $"but ToBv<{typeof(TSize).Name}> expects {TSize.Size} bits"
        );
    }

    return Z3Expr.Create<BvExpr<TSize>>(Context, bvHandle);
}
```

### 5. Testing Strategy

#### Tests to Add

1. **Sort Validation Tests** (`Z3Wrap.Tests/Core/Z3ExprSortValidationTests.cs`)
   - Test that creating IntExpr with BoolExpr handle throws
   - Test that creating BvExpr<Size8> with Size16 handle throws
   - Test that creating BoolExpr with IntExpr handle throws
   - Test that correct sorts don't throw

2. **Char ToBv Size Discovery Test** (`Z3Wrap.Tests/Expressions/Strings/CharExprOperationsTests.cs`)
   - Discover actual bitvector size returned by char.to_bv
   - Document the size in test output

3. **Update Existing Tests**
   - Update all CharExpr ToBv tests with correct size
   - Verify BvExprCharExtensions.ToChar uses correct size

### 6. Open Questions

1. **Performance**: Should validation be DEBUG-only or always on?
   - **Recommendation**: DEBUG-only to avoid production overhead
   - Users can test with DEBUG builds to catch issues

2. **Sort Comparison**: Best way to compare sorts?
   - Use `GetSortKind` for kind comparison
   - Use `GetBvSortSize` for bitvector size comparison
   - May need `GetSortId` for other complex sorts (arrays, etc.)

3. **Error Messages**: What level of detail?
   - Include: expected sort, actual sort, expression type
   - Include: full Z3 AST string for debugging?

4. **Char Bitvector Size**: Is it configurable?
   - Need to check Z3 documentation
   - If configurable, need to handle dynamically
   - If fixed, can hardcode the size type

### 7. Implementation Order

1. ✅ Create this plan
2. ⏳ Implement `ValidateExpressionSort` in Z3Context
3. ⏳ Modify `Z3Expr.Create<T>` to call validation
4. ⏳ Write sort validation tests
5. ⏳ Add `GetBvSizeFromExpr` helper
6. ⏳ Write char.to_bv size discovery test and run it
7. ⏳ Update CharExpr ToBv methods based on discovered size
8. ⏳ Update all related tests
9. ⏳ Run `make test` and `make coverage`
10. ⏳ Update TODO.md with progress

### 8. Benefits

- **Early Error Detection**: Catch type mismatches at expression creation time
- **Clear Error Messages**: Know exactly what went wrong and where
- **Size Discovery**: Automatically determine bitvector sizes from Z3
- **Type Safety**: Ensure C# types match Z3 sorts
- **Better DX**: Easier to debug issues

### 9. Risks

- **Performance**: Validation adds overhead (mitigated by DEBUG-only)
- **Complexity**: More code to maintain
- **Breaking Changes**: If char.to_bv size is different than assumed

## Next Steps

1. Implement sort validation infrastructure
2. Discover actual char.to_bv bitvector size
3. Update CharExpr accordingly
4. Continue with TODO.md items

---

**Created**: 2025-01-08
**Status**: Implementation starting
