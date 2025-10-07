# String Theory Implementation Plan

## Overview

Implement **StringExpr** for Z3's built-in Unicode string sort with natural C# syntax and comprehensive test coverage.

**Scope**: Strings only (not sequences or regex) - focus on the most commonly used operations.

## Status: Initial Implementation Complete - Under Review

**Review Date**: 2025-10-08
**Current State**: Core implementation done, comprehensive review identified issues to address

## Design Decisions

### 1. Expression Class: `StringExpr`

**Type**: Sealed class inheriting from `Z3Expr`

**Why separate from sequences?**
- Z3 treats strings as a special built-in type (not just `Seq<Char>`)
- String-specific operations: `StrToInt`, `IntToStr`, lexicographic comparison
- Better ergonomics with implicit conversion from C# `string`

### 2. Character Expressions: `CharExpr`

**Type**: Sealed class for single Unicode characters

**Operations**:
- Character literals: `context.Char(0x41)` → 'A'
- Comparison: `ch1 <= ch2`
- Conversions: `ToInt()`, `ToBv()`, `FromBv()`
- Predicates: `IsDigit()`

### 3. Supported Operations

#### **Creation** (StringContextExtensions.cs)
```csharp
StringExpr String(string value)           // Literal: context.String("hello")
StringExpr StringConst(string name)       // Variable: context.StringConst("x")
```

#### **Concatenation** (Operator `+`)
```csharp
var result = str1 + str2 + str3;          // Natural syntax
var result = context.Concat(str1, str2);  // Extension method
var result = str1.Concat(str2);           // Instance method
```

#### **Comparison** (Lexicographic)
```csharp
str1 < str2    // MkStrLt
str1 <= str2   // MkStrLe
str1 > str2    // Derived from <
str1 >= str2   // Derived from <=
str1 == str2   // Equality (inherited)
str1 != str2   // Inequality (inherited)
```

#### **String Operations**
```csharp
str.Length()                // IntExpr
str.Contains(substr)        // BoolExpr
str.Substring(offset, len)  // StringExpr (Extract)
str.Replace(src, dst)       // StringExpr (first occurrence)
str.StartsWith(prefix)      // BoolExpr (Prefix)
str.EndsWith(suffix)        // BoolExpr (Suffix)
str.IndexOf(substr, offset) // IntExpr (returns -1 if not found)
str.LastIndexOf(substr)     // IntExpr

// Indexing (two operations, different return types)
str[index]                  // CharExpr - C# indexer, uses MkSeqNth
str.CharAt(index)           // CharExpr - explicit method, same as indexer
str.At(index)               // StringExpr - unit string, uses MkSeqAt
```

#### **Conversions**
```csharp
str.ToInt()               // Parse string to integer
intExpr.ToStr()           // Convert integer to string (extension on IntExpr)
```

#### **Character Operations** (CharContextExtensions.cs)
```csharp
CharExpr Char(uint codepoint)        // Literal
CharExpr CharConst(string name)      // Variable
ch.ToInt()                           // Codepoint as integer
ch.ToBv()                            // Codepoint as 18-bit bitvector (BvExpr<Size18>)
ch.ToBv<TSize>()                     // Generic version with runtime validation (TSize.Size must be 18)
bv18.ToChar()                        // BvExpr<Size18> to CharExpr
bv.ToChar<TSize>()                   // Generic version with runtime validation (TSize.Size must be 18)
ch.IsDigit()                         // Is digit predicate
ch1 <= ch2                           // Character comparison
```

### 4. Operations NOT Included (Future: Sequences/Regex)

**Sequences**: Map, Fold, Unit, Empty (requires `SeqExpr<T>`)
**Regex**: Pattern matching (requires `RegexExpr`)
**BitVector conversions**: `UbvToStr`, `SbvToStr` (requires bitvector integration)

### 5. Test Strategy

**Test Files** (following existing patterns):
- `StringExprCreationTests.cs` - Creation methods (String, StringConst)
- `StringExprConcatTests.cs` - Concatenation (8 variants)
- `StringExprComparisonTests.cs` - Lexicographic comparisons (8 variants each)
- `StringExprOperationsTests.cs` - Contains, Substring, Replace, etc.
- `StringExprConversionTests.cs` - ToInt/FromInt
- `CharExprTests.cs` - Character operations

**Test Coverage**: 8-variant pattern for all operators (operator, context extension, instance method, with literal overloads)

**Validation**: All tests verify actual computed values via `model.GetStringValue()`, `model.GetIntValue()`, etc.

## Implementation Tasks

### Phase 1: Core Infrastructure
1. Create `StringExpr` class in `Z3Wrap/Expressions/Strings/StringExpr.cs`
2. Create `CharExpr` class in `Z3Wrap/Expressions/Strings/CharExpr.cs`
3. Add `GetStringValue()`, `GetCharValue()` to `Z3Model`

### Phase 2: Creation and Basic Operations
4. Create `StringContextExtensions.cs` - String/StringConst creation
5. Create `CharContextExtensions.cs` - Char/CharConst creation
6. Implement concatenation operator and extensions
7. Write creation and concatenation tests

### Phase 3: Comparison Operations
8. Create `StringExprExtensions.cs` - Comparison operators (<, <=, >, >=)
9. Write comparison tests (8 variants each)

### Phase 4: String Operations
10. Add string operation methods (Contains, Substring, Replace, etc.)
11. Write operation tests

### Phase 5: Conversions
12. Implement ToInt/ToStr conversions
13. Add character conversions (ToInt, ToBv, FromBv, IsDigit)
14. Write conversion tests

### Phase 6: Integration
15. Add README example demonstrating string constraints
16. Update `ReadmeExamplesTests.cs` with string examples
17. Run full test suite and verify ≥90% coverage
18. Format code (`make format`)

## Files to Create

```
Z3Wrap/
├── Expressions/
│   └── Strings/
│       ├── StringExpr.cs                    # String expression class
│       ├── CharExpr.cs                      # Character expression class
│       ├── StringContextExtensions.cs       # String creation (String, StringConst)
│       ├── StringExprExtensions.cs          # String operations
│       ├── CharContextExtensions.cs         # Char creation
│       └── CharExprExtensions.cs            # Char operations
└── Core/
    └── Z3Model.Strings.cs                   # GetStringValue, GetCharValue methods

Z3Wrap.Tests/
└── Expressions/
    └── Strings/
        ├── StringExprCreationTests.cs       # Creation tests
        ├── StringExprConcatTests.cs         # Concatenation tests
        ├── StringExprComparisonTests.cs     # Comparison tests
        ├── StringExprOperationsTests.cs     # String operations
        ├── StringExprConversionTests.cs     # ToInt/ToStr tests
        └── CharExprTests.cs                 # Character tests
```

## API Examples

### Basic String Constraints
```csharp
using var context = new Z3Context();
using var scope = context.SetUp();
using var solver = context.CreateSolver();

var greeting = context.String("Hello, ") + context.String("World!");
var expected = context.StringConst("result");

solver.Assert(expected == greeting);

var status = solver.Check();
var model = solver.GetModel();
Console.WriteLine(model.GetStringValue(expected)); // "Hello, World!"
```

### Palindrome Check
```csharp
var input = context.StringConst("s");
var reversed = context.StringConst("r");

// Constraint: reverse(input) == input (simplified - actual implementation may vary)
solver.Assert(input.Length() == 5);
solver.Assert(reversed == input); // Placeholder for reverse logic

var status = solver.Check();
```

### String Parsing
```csharp
var numStr = context.StringConst("num");
var value = numStr.ToInt();

solver.Assert(numStr.Length() <= 3);
solver.Assert(value >= 100);
solver.Assert(value <= 999);

var status = solver.Check();
var model = solver.GetModel();
Console.WriteLine(model.GetStringValue(numStr)); // e.g., "100"
```

## Success Criteria

- [ ] All string operations implemented with natural C# syntax
- [ ] Character expressions fully functional
- [ ] Comprehensive test coverage (≥90%)
- [ ] All tests follow 8-variant pattern
- [ ] README examples validated by tests
- [ ] Zero build warnings
- [ ] `make ci` passes

## Notes

- **Regex/Sequences**: Deferred to future implementation
- **Encoding**: Unicode by default (configurable via Z3 parameters)
- **Performance**: Z3's string sort is optimized for string operations
- **Thread Safety**: StringExpr follows same patterns as other expression types

---

## Code Review Findings (2025-10-08)

### Critical Issues to Fix

#### 1. Test File Organization ❌ **CRITICAL**
**Problem**: Current implementation has `StringExprSmokeTests.cs` which violates project patterns.

**Required Structure** (following IntExpr/BoolExpr patterns):
```
Z3Wrap.Tests/Expressions/Strings/
├── StringExprFactoryTests.cs        # Creation: String(), StringConst()
├── StringExprConcatTests.cs         # Concatenation: all 8 variants
├── StringExprComparisonTests.cs     # Comparisons: <, <=, >, >= (8 variants each)
├── StringExprOperationsTests.cs     # Contains, Substring, Replace, StartsWith, etc.
├── StringExprConversionTests.cs     # ToInt() conversion
├── CharExprFactoryTests.cs          # Char(), CharConst() creation
└── CharExprOperationsTests.cs       # ToInt(), ToBv(), IsDigit(), comparisons
```

**Action**: Delete `StringExprSmokeTests.cs` and create proper test structure.

---

#### 2. Missing 8-Variant Test Coverage ❌ **CRITICAL**
**Problem**: Tests don't follow the mandatory 8-variant pattern for operators.

**Example from IntExprArithmeticTests.cs** (our standard):
```csharp
var sum = a + b;                              // 1. Operator
var sumViaIntLeft = 10 + b;                   // 2. Literal left (implicit conversion)
var sumViaIntRight = a + 32;                  // 3. Literal right (implicit conversion)
var sumViaContext = context.Add(a, b);        // 4. Context extension
var sumViaContextIntLeft = context.Add(10, b); // 5. Context + literal left (implicit conversion)
var sumViaContextIntRight = context.Add(a, 32); // 6. Context + literal right (implicit conversion)
var sumViaFunc = a.Add(b);                    // 7. Expression method
var sumViaFuncIntRight = a.Add(32);           // 8. Expression + literal right (implicit conversion)
```

**Required for String Theory**:
```csharp
// Concatenation test
var concat1 = str1 + str2;                       // 1. Operator
var concat2 = "hello" + str2;                    // 2. String literal left (implicit conversion)
var concat3 = str1 + "world";                    // 3. String literal right (implicit conversion)
var concat4 = context.Concat(str1, str2);        // 4. Context extension
var concat5 = context.Concat("hello", str2);     // 5. Context + literal left (implicit conversion)
var concat6 = context.Concat(str1, "world");     // 6. Context + literal right (implicit conversion)
var concat7 = str1.Concat(str2);                 // 7. Expression method
var concat8 = str1.Concat("world");              // 8. Expression + literal right (implicit conversion)

// Comparison test (e.g., <)
var cmp1 = str1 < str2;                          // 1. Operator
var cmp2 = "aaa" < str2;                         // 2. String literal left (implicit conversion)
var cmp3 = str1 < "zzz";                         // 3. String literal right (implicit conversion)
var cmp4 = context.Lt(str1, str2);               // 4. Context extension
var cmp5 = context.Lt("aaa", str2);              // 5. Context + literal left (implicit conversion)
var cmp6 = context.Lt(str1, "zzz");              // 6. Context + literal right (implicit conversion)
var cmp7 = str1.Lt(str2);                        // 7. Expression method
var cmp8 = str1.Lt("zzz");                       // 8. Expression + literal right (implicit conversion)
```

**Why test implicit conversions?**
- Verifies C# compiler correctly applies implicit conversion in all contexts
- Ensures Z3Context.Current works correctly when implicit conversion invoked
- Tests that the natural syntax users expect actually works
- Catches any issues with operator precedence or overload resolution

**Action**: Rewrite all operator tests with full 8-variant coverage testing implicit conversions.

---

#### 3. String Literal Overloads - NOT NEEDED ✅ **CLARIFICATION**
**Status**: Implementation is CORRECT - no changes needed.

**Why**: C# implicit conversion handles this automatically!

**Pattern from IntExpr**:
```csharp
// IntExpr has implicit conversion
public static implicit operator IntExpr(int value) => Z3Context.Current.Int(value);

// Operators only defined for IntExpr + IntExpr
public static IntExpr operator +(IntExpr left, IntExpr right) => left.Add(right);

// C# automatically handles:
var result1 = intExpr + 10;    // 10 implicitly converts to IntExpr
var result2 = 10 + intExpr;    // 10 implicitly converts to IntExpr
var result3 = intExpr < 10;    // 10 implicitly converts to IntExpr
var result4 = 10 < intExpr;    // 10 implicitly converts to IntExpr
```

**StringExpr already correct**:
```csharp
// Has implicit conversion ✅
public static implicit operator StringExpr(string value) => Z3Context.Current.String(value);

// Operators only for StringExpr + StringExpr ✅
public static StringExpr operator +(StringExpr left, StringExpr right) => ...;

// C# automatically handles:
var result1 = strExpr + "hello";    // "hello" implicitly converts
var result2 = "hello" + strExpr;    // "hello" implicitly converts
var result3 = strExpr < "world";    // "world" implicitly converts
var result4 = "world" < strExpr;    // "world" implicitly converts
```

**Action**: ~~Add literal overloads~~ → **No action needed - already correct!**

---

#### 4. Method Placement - IntExprStringExtensions.cs ❌ **CRITICAL**
**Problem**: File is in wrong location violating organizational principles.

**Current Location**: `Z3Wrap/Expressions/Strings/IntExprStringExtensions.cs`
**Issue**: Extensions for IntExpr should live with IntExpr, not with StringExpr.

**Correct Organization**:
```
Z3Wrap/Expressions/Numerics/IntExprExtensions.cs
  → Add ToStr() method here (extends IntExpr)

Z3Wrap/Expressions/Strings/StringExprExtensions.cs
  → Keep ToInt() here (extends StringExpr)
```

**Principle**: Cross-type conversions live with the **source type**, not the target type.

**Action**: Move `ToStr()` extension to `IntExprExtensions.cs` in Numerics folder.

---

#### 5. Missing Extension Files and Methods ❌ **CRITICAL**
**Problem**: Missing extension files and methods following project organizational patterns.

**Current State**:
- ✅ `StringExpr.cs` has operators and `Concat(StringExpr other)` instance method
- ❌ **Missing**: `StringExprExtensions.cs` file
- ❌ **Missing**: Comparison instance methods (`Lt`, `Le`, `Gt`, `Ge`)
- ❌ **Missing**: Context-level operations in `StringContextExtensions.cs`

**Organizational Pattern** (from IntExpr):

1. **Expression Class** (`StringExpr.cs`) - ✅ Already correct:
   - Operators: `+`, `<`, `<=`, `>`, `>=` → delegate to extension methods
   - Type-specific instance methods: `Length()`, `Contains()`, `Substring()`, etc.
   - Conversions: `ToInt()`

2. **Type-Specific Extensions** (`StringExprExtensions.cs`) - ❌ **MUST CREATE**:
   ```csharp
   // Instance method extensions for comparisons (match ArithmeticComparisonExprExtensions pattern)
   public static BoolExpr Lt(this StringExpr left, StringExpr right)
       => left.Context.Lt(left, right);
   public static BoolExpr Le(this StringExpr left, StringExpr right)
       => left.Context.Le(left, right);
   public static BoolExpr Gt(this StringExpr left, StringExpr right)
       => left.Context.Gt(left, right);
   public static BoolExpr Ge(this StringExpr left, StringExpr right)
       => left.Context.Ge(left, right);

   // Variadic concat instance method (match ArithmeticOperationsExprExtensions pattern)
   // CRITICAL: Prepends 'left' to 'others' array before calling context
   public static StringExpr Concat(this StringExpr left, params ReadOnlySpan<StringExpr> others)
       => left.Context.Concat([left, .. others]);
   ```

   **Note**: This will REPLACE the existing `Concat(StringExpr other)` method in `StringExpr.cs`

3. **Context Extensions** (`StringContextExtensions.cs`) - ❌ **MUST ADD**:
   ```csharp
   // Variadic concat - Z3 native support via Z3_mk_seq_concat(context, n, args[])
   public static StringExpr Concat(this Z3Context context, params ReadOnlySpan<StringExpr> strings)
   {
       if (strings.Length == 0)
           throw new ArgumentException("Concat requires at least one operand.", nameof(strings));

       var args = new IntPtr[strings.Length];
       for (int i = 0; i < strings.Length; i++)
           args[i] = strings[i].Handle;

       var resultHandle = context.Library.MkSeqConcat(context.Handle, (uint)args.Length, args);
       return Z3Expr.Create<StringExpr>(context, resultHandle);
   }

   // Comparison context methods (for 8-variant pattern)
   public static BoolExpr Lt(this Z3Context context, StringExpr left, StringExpr right)
   {
       var handle = context.Library.MkStrLt(context.Handle, left.Handle, right.Handle);
       return Z3Expr.Create<BoolExpr>(context, handle);
   }
   // ... same for Le, Gt, Ge
   ```

**Z3 Native Support Verified**:
- `Z3_mk_seq_concat(context, n, args[])` - Works with n ≥ 1 (tested!)
- Uses same pattern as `Z3_mk_add` for IntExpr
- Current `Concat(StringExpr other)` uses `MkSeqConcat` with n=2 successfully

**Action**:
1. Create `StringExprExtensions.cs` with comparison and variadic concat instance methods
2. Add context-level methods to `StringContextExtensions.cs`

---

#### 6. CharExpr Missing Implicit Conversion ❌ **HIGH**
**Problem**: CharExpr doesn't follow pattern established by IntExpr/BoolExpr.

**Current**: Only explicit creation via `context.Char('A')`
**Need**:
```csharp
// In CharExpr.cs
public static implicit operator CharExpr(char value)
    => Z3Context.Current.Char(value);
```

**Justification**: Enables natural syntax: `CharExpr ch = 'A';` like `IntExpr i = 42;`

**Action**: Add implicit conversion from char to CharExpr.

---

#### 7. CharExpr Creation - Dual Overloads ⚠️ **MEDIUM**
**Problem**: Plan specifies `Char(uint codepoint)` but implementation also has `Char(char value)`.

**Current Implementation**:
```csharp
public static CharExpr Char(this Z3Context context, char value);    // C#-friendly
public static CharExpr Char(this Z3Context context, uint codepoint); // Unicode codepoint
```

**Recommendation**: Keep both, document clearly:
- `Char(char)` for ASCII/BMP characters (most common)
- `Char(uint)` for full Unicode range (0x0000-0x10FFFF)

**Action**: Ensure both overloads tested, document in XML comments.

---

#### 8. Comparison Extension Methods - Missing ❌ **HIGH**
**Problem**: StringExpr operators delegate to `Lt()`, `Le()`, `Gt()`, `Ge()` methods that don't exist yet.

**Current**: Operators in `StringExpr.cs` directly inline Z3 calls:
```csharp
public static BoolExpr operator <(StringExpr left, StringExpr right)
{
    var handle = left.Context.Library.MkStrLt(left.Context.Handle, left.Handle, right.Handle);
    return Z3Expr.Create<BoolExpr>(left.Context, handle);
}
```

**Should be** (following IntExpr pattern):
```csharp
// In StringExpr.cs - operators delegate
public static BoolExpr operator <(StringExpr left, StringExpr right) => left.Lt(right);
public static BoolExpr operator <=(StringExpr left, StringExpr right) => left.Le(right);
public static BoolExpr operator >(StringExpr left, StringExpr right) => left.Gt(right);
public static BoolExpr operator >=(StringExpr left, StringExpr right) => left.Ge(right);
```

**Action**:
1. Refactor operators to delegate to extension methods (covered in issue #5)
2. This is a minor refactoring - operators work but violate the pattern

---

#### 9. Solution File Not Updated ❌ **MEDIUM**
**Problem**: New markdown files not added to Z3Wrap.sln per CLAUDE.md rules.

**Files to Add**:
- `PLAN_STRING_THEORY.md`
- `CHAR_TO_BV_EXPLANATION.md`
- `STRING_EXAMPLES.md`

**Required Action** (per CLAUDE.md § Solution File Management):
```
Project("{2150E333-8FDC-42A3-9474-1A3956D46DE8}") = "misc", "misc", "{...}"
    ProjectSection(SolutionItems) = preProject
        ...existing files...
        PLAN_STRING_THEORY.md = PLAN_STRING_THEORY.md
        CHAR_TO_BV_EXPLANATION.md = CHAR_TO_BV_EXPLANATION.md
        STRING_EXAMPLES.md = STRING_EXAMPLES.md
    EndProjectSection
```

**Action**: Update Z3Wrap.sln to include all new markdown files.

---

#### 10. Instance Method Overloads - Covered in Issue #5 ✅
**Status**: Addressed by creating `StringExprExtensions.cs` (see issue #5)

**Summary**:
- ✅ `str.Concat(StringExpr other)` exists in `StringExpr.cs`
- ✅ Variadic `str.Concat(params ReadOnlySpan<StringExpr> others)` will be in `StringExprExtensions.cs`
- ✅ Comparison methods `Lt()`, `Le()`, `Gt()`, `Ge()` will be in `StringExprExtensions.cs`
- ✅ String literal overloads NOT needed - implicit conversion handles it

**Action**: No separate action - covered by issue #5.

---

### Summary Checklist

**Code Organization**:
- [ ] Delete `StringExprSmokeTests.cs`
- [ ] Create 7 proper test files (FactoryTests, ConcatTests, ComparisonTests, etc.)
- [ ] Move `IntExprStringExtensions.cs` → `IntExprExtensions.cs` in Numerics

**API Completeness**:
- [ ] ~~Add string literal overloads~~ → NOT NEEDED (implicit conversion handles it) ✅
- [ ] Add context-level extension methods (Concat, Lt, Le, Gt, Ge - without string overloads)
- [ ] Verify instance methods exist (Lt, Le, Gt, Ge)
- [ ] Add `CharExpr` implicit conversion from `char`
- [ ] Verify comparison extension methods exist

**Test Coverage**:
- [ ] Implement 8-variant pattern for concatenation
- [ ] Implement 8-variant pattern for all comparisons
- [ ] Test both `Char(char)` and `Char(uint)` overloads

**Project Hygiene**:
- [ ] Update Z3Wrap.sln with new markdown files
- [ ] Run `make format` before any commit
- [ ] Verify `make build` produces zero warnings
- [ ] Verify ≥90% coverage with `make coverage`

---

## Future Work (Discovered During String Theory Implementation)

### Audit Arithmetic Operations for Native Z3 Support

**Problem**: Some operations currently implemented in C# wrapper code may have native Z3 implementations that are more efficient.

**Example**: `MkAbs` - native Z3 function for absolute value that might be better than wrapper implementation.

**Action Required**:
1. Audit all arithmetic operations in:
   - `ArithmeticOperationsContextExtensions.cs`
   - `ArithmeticFunctionsContextExtensions.cs`
   - `IntContextExtensions.cs`
   - `RealContextExtensions.cs`
2. Check Z3 C API headers for native equivalents
3. Replace C# implementations with native Z3 calls where available
4. Document any operations that intentionally use C# implementations

**Priority**: Medium - Not blocking string theory implementation, but should be done for performance and correctness.

---

**Created**: 2025-10-07
**Reviewed**: 2025-10-08
**Status**: Awaiting fixes before proceeding to comprehensive testing
