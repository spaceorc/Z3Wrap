# TODO - String/Char Tests Fixes

## Status: In Progress

### Completed ✅
1. ✅ Created CharExprExtensions.cs with comparison methods (Le, Ge, Lt, Gt)
2. ✅ Added context extension methods to CharContextExtensions.cs (ToInt, ToBv, IsDigit, Le, Ge, Lt, Gt)
3. ✅ Identified need to use `model.Evaluate()` for conversion operations
4. ✅ Started fixing CharExprOperationsTests.cs - ToInt and ToBv tests now use Evaluate

### Critical Issues Remaining ❌

#### 1. CharExprOperationsTests.cs - Complete Evaluate() fixes
**Files**: `/Users/spaceorc/work/temp/z3lib/Z3Wrap.Tests/Expressions/Strings/CharExprOperationsTests.cs`

**Need to add `model.Evaluate()` for**:
- `ToBv_Generic_WithSize18_Works` (line ~66-78)
- `IsDigit_WithDigits_ReturnsTrue` (line ~98-117)
- `IsDigit_WithNonDigits_ReturnsFalse` (line ~119-142)

**Pattern to apply**:
```csharp
var result = charExpr.SomeOperation();
var resultViaContext = context.SomeOperation(charExpr);

var status = solver.Check();
Assert.That(status, Is.EqualTo(Z3Status.Satisfiable));

var model = solver.GetModel();
var evaluatedResult = model.Evaluate(result);  // ADD THIS
var evaluatedResultViaContext = model.Evaluate(resultViaContext);  // ADD THIS

Assert.Multiple(() =>
{
    Assert.That(model.GetBoolValue(evaluatedResult), Is.True);  // Use evaluated
    Assert.That(model.GetBoolValue(evaluatedResultViaContext), Is.True);
});
```

#### 2. CharExprOperationsTests.cs - Comparison tests already have assertions
**Status**: Lines 144-294 comparison tests (Le, Ge, Lt, Gt) already use `solver.Assert()` pattern which is correct.
**Action**: Verify all 8 variants work correctly, they should already pass.

#### 3. StringExprConversionTests.cs - Add Assert.Multiple for variants
**File**: `/Users/spaceorc/work/temp/z3lib/Z3Wrap.Tests/Expressions/Strings/StringExprConversionTests.cs`

**Current**: Tests context methods separately (lines 196-227)
**Need**: Consolidate to test both instance and context methods together with Assert.Multiple

**Example** (lines 11-28 `ToInt_ValidNumericString_ReturnsCorrectInteger`):
```csharp
var str = context.String(strValue);
var intResult = str.ToInt();
var intResultViaContext = context.StrToInt(str);  // ADD THIS

var status = solver.Check();
var model = solver.GetModel();

Assert.Multiple(() =>  // ADD THIS
{
    Assert.That(model.GetIntValue(intResult), Is.EqualTo(new BigInteger(expected)));
    Assert.That(model.GetIntValue(intResultViaContext), Is.EqualTo(new BigInteger(expected)));  // ADD THIS
});
```

**Apply to all tests**:
- `ToInt_ValidNumericString_ReturnsCorrectInteger`
- `ToInt_WithVariable_SolvesCorrectly`
- `ToStr_Integer_ReturnsCorrectString`
- `ToStr_WithVariable_SolvesCorrectly`
- All round-trip tests
- Remove separate context method tests (lines 196-227) as they'll be covered

#### 4. StringExprOperationsTests.cs - Add variant testing
**File**: `/Users/spaceorc/work/temp/z3lib/Z3Wrap.Tests/Expressions/Strings/StringExprOperationsTests.cs`

**Current**: Only tests instance methods
**Need**: Test both instance methods and context extensions where they exist

**Check if context extensions exist for**:
- Contains, StartsWith, EndsWith
- Substring, Replace
- IndexOf, LastIndexOf
- CharAt, At

**If context extensions don't exist**: Tests are acceptable as-is, just verify they work.

#### 5. Verify 18-bit bitvector assumption
**Issue**: Documentation doesn't explicitly state char.to_bv returns 18-bit bitvector
**Current**: Code assumes 18 bits (based on existing BvExprCharExtensions.cs)
**Action**: Run tests to verify this works, or investigate Z3 source/behavior

**If wrong size**: Need to determine actual size and update:
- `CharExpr.ToBv()` return type
- `CharExpr.ToBv<TSize>()` validation
- `CharContextExtensions.ToBv()` return type
- `BvExprCharExtensions.ToChar()` parameter type
- All related tests

### Testing Strategy

**For conversions (ToInt, ToBv, IsDigit)**:
- Use `model.Evaluate()` to get concrete values
- Test both instance method and context extension
- Use Assert.Multiple

**For comparisons (<=, >=, <, >)**:
- Current `solver.Assert(result == expected)` pattern is correct
- Tests should already work with 8-variant pattern

**For string operations**:
- Check if context extensions exist
- Add them to tests if they do
- Use Assert.Multiple

### Next Steps

1. Fix remaining Evaluate() issues in CharExprOperationsTests
2. Run `make test` to see which tests pass
3. Fix StringExprConversionTests to use Assert.Multiple
4. Check/fix StringExprOperationsTests for context extension variants
5. Run full test suite
6. Verify ≥90% coverage with `make coverage`
7. Run `make format` before commit

### Files Modified So Far

- ✅ `Z3Wrap/Expressions/Strings/CharExprExtensions.cs` - CREATED
- ✅ `Z3Wrap/Expressions/Strings/CharContextExtensions.cs` - UPDATED (added ToInt, ToBv, IsDigit, comparisons)
- ✅ `Z3Wrap/Expressions/Strings/CharExpr.cs` - UPDATED (removed instance comparison methods, delegates to extensions)
- 🔄 `Z3Wrap.Tests/Expressions/Strings/CharExprOperationsTests.cs` - PARTIALLY FIXED (ToInt, ToBv use Evaluate)
- ❌ `Z3Wrap.Tests/Expressions/Strings/StringExprConversionTests.cs` - NOT STARTED
- ❌ `Z3Wrap.Tests/Expressions/Strings/StringExprOperationsTests.cs` - NOT STARTED

### Test Results (Last Run)

```
Failed!  - Failed:    22, Passed:  1034, Skipped:     1, Total:  1057
```

**Main failures**: CharExpr conversion tests not using Evaluate()

---

**Created**: 2025-01-08
**Last Updated**: 2025-01-08
