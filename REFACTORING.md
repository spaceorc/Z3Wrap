# Z3Wrap Test Refactoring: Complete Coverage Pattern

## Overview

This document describes a systematic refactoring pattern applied to Z3Wrap expression tests to ensure comprehensive coverage of all possible ways users can interact with the API.

## The Problem

Initially, many test methods in Z3BitVecExpr tests were incomplete, testing only some variations of operations while missing others. This created gaps in test coverage where certain API usage patterns were not validated.

### Example of Incomplete Coverage (Before):
```csharp
public void Add_AllVariations_ReturnsExpectedResult(int left, int right, int expectedResult)
{
    var x = context.BitVec(left, 8);
    var y = context.BitVec(right, 8);

    // Only testing some variations
    var resultOperatorBitVec = x + y;                    // BitVec + BitVec (operator)
    var resultOperatorRightBigInt = x + right;           // BitVec + BigInteger (operator) - using raw int!
    var resultOperatorLeftBigInt = left + y;             // BigInteger + BitVec (operator) - using raw int!
    var resultMethodBitVec = x.Add(y);                   // BitVec.Add(BitVec) (method)
    var resultMethodBigInt = x.Add(right);               // BitVec.Add(BigInteger) (method) - using raw int!

    // Missing: All context extension methods!
}
```

## The Complete Coverage Pattern

### Core Principle
**Every operation test must validate ALL possible ways a user can perform that operation:**

1. **Operators** (where applicable)
2. **Fluent Methods** on expressions
3. **Context Extension Methods**

### Required Combinations
Each operation must be tested with all type combinations:
- **BitVec + BitVec**
- **BitVec + BigInteger**
- **BigInteger + BitVec**

### Implementation Pattern

#### 1. Variable Setup
```csharp
var x = context.BitVec(left, 8);
var y = context.BitVec(right, 8);
var leftBigInt = new BigInteger(left);     // MUST use new BigInteger(), not raw int
var rightBigInt = new BigInteger(right);   // MUST use new BigInteger(), not raw int
```

#### 2. Operation Testing (8 variations total)
```csharp
// Test all variations of [operation]
var resultOperatorBitVec = x + y;                            // BitVec + BitVec (operator)
var resultOperatorRightBigInt = x + rightBigInt;             // BitVec + BigInteger (operator)
var resultOperatorLeftBigInt = leftBigInt + y;               // BigInteger + BitVec (operator)
var resultMethodBitVec = x.Add(y);                           // BitVec.Add(BitVec) (method)
var resultMethodBigInt = x.Add(rightBigInt);                 // BitVec.Add(BigInteger) (method)
var resultContextBitVec = context.Add(x, y);                 // Context.Add(BitVec, BitVec) (method)
var resultContextRightBigInt = context.Add(x, rightBigInt);  // Context.Add(BitVec, BigInteger) (method)
var resultContextLeftBigInt = context.Add(leftBigInt, y);    // Context.Add(BigInteger, BitVec) (method)
```

#### 3. Comprehensive Assertions
```csharp
Assert.Multiple(() =>
{
    Assert.That(model.GetBitVec(resultOperatorBitVec), Is.EqualTo(expected), "BitVec + BitVec operator failed");
    Assert.That(model.GetBitVec(resultOperatorRightBigInt), Is.EqualTo(expected), "BitVec + BigInteger operator failed");
    Assert.That(model.GetBitVec(resultOperatorLeftBigInt), Is.EqualTo(expected), "BigInteger + BitVec operator failed");
    Assert.That(model.GetBitVec(resultMethodBitVec), Is.EqualTo(expected), "BitVec.Add(BitVec) method failed");
    Assert.That(model.GetBitVec(resultMethodBigInt), Is.EqualTo(expected), "BitVec.Add(BigInteger) method failed");
    Assert.That(model.GetBitVec(resultContextBitVec), Is.EqualTo(expected), "Context.Add(BitVec, BitVec) method failed");
    Assert.That(model.GetBitVec(resultContextRightBigInt), Is.EqualTo(expected), "Context.Add(BitVec, BigInteger) method failed");
    Assert.That(model.GetBitVec(resultContextLeftBigInt), Is.EqualTo(expected), "Context.Add(BigInteger, BitVec) method failed");
});
```

## Special Cases

### Unary Operations (e.g., Negation)
For unary operations, test all available methods:
```csharp
var resultOperator = -x;               // -BitVec (unary operator)
var resultMethod = x.Neg();            // BitVec.Neg() (method)
var resultContext = context.Neg(x);    // Context.Neg(BitVec) (method)
```

### Signed vs Unsigned Operations
Some operations have signed/unsigned variants. These should be tested separately:
- **Unsigned division test**: Only test unsigned operators and `signed: false` methods
- **Signed division test**: Only test signed methods with `signed: true` parameter

```csharp
// Unsigned division test
var divResultOperator = x / y;                               // Always unsigned
var divResultMethod = x.Div(y, signed: false);              // Explicitly unsigned
var divResultContext = context.Div(x, y, signed: false);    // Explicitly unsigned

// Signed division test (separate test method)
var divResultMethod = x.Div(y, signed: true);               // Explicitly signed
var divResultContext = context.Div(x, y, signed: true);     // Explicitly signed
```

## Test Organization Structure

### Expression Type Directory Pattern
Each Z3 expression type should have its own dedicated test directory containing multiple focused test files. This follows the established pattern:

```
Z3Wrap.Tests/Unit/Expressions/
├── Z3BitVecExprTests/              ← Directory for BitVec expressions
│   ├── Z3BitVecExprArithmeticTests.cs
│   ├── Z3BitVecExprBitwiseTests.cs
│   ├── Z3BitVecExprComparisonTests.cs
│   ├── Z3BitVecExprShiftTests.cs
│   ├── Z3BitVecExprCreationTests.cs
│   └── ... (other BitVec-specific tests)
├── Z3BoolExprTests/                ← Directory for Bool expressions
│   ├── Z3BoolExprLogicalTests.cs
│   ├── Z3BoolExprImplicationTests.cs
│   ├── Z3BoolExprComparisonTests.cs
│   ├── Z3BoolExprConditionalTests.cs
│   ├── Z3BoolExprCreationTests.cs
│   └── ... (other Bool-specific tests)
├── Z3IntExprTests/                 ← Directory for Int expressions (to be created)
│   ├── Z3IntExprArithmeticTests.cs
│   ├── Z3IntExprComparisonTests.cs
│   ├── Z3IntExprCreationTests.cs
│   └── ... (other Int-specific tests)
├── Z3RealExprTests/                ← Directory for Real expressions (to be created)
│   ├── Z3RealExprArithmeticTests.cs
│   ├── Z3RealExprComparisonTests.cs
│   ├── Z3RealExprCreationTests.cs
│   └── ... (other Real-specific tests)
└── Z3ArrayExprTests/               ← Directory for Array expressions (to be created)
    ├── Z3ArrayExprOperationTests.cs
    ├── Z3ArrayExprCreationTests.cs
    └── ... (other Array-specific tests)
```

### Benefits of Directory-Per-Expression-Type:
1. **Clear Organization**: Each expression type's tests are grouped together
2. **Focused Test Files**: Each file covers a specific aspect (arithmetic, bitwise, comparison, etc.)
3. **Scalability**: Easy to add new test files for specific functionality
4. **Maintainability**: Related tests are co-located and easier to find/modify
5. **Parallel Development**: Different developers can work on different expression types independently

### Naming Conventions:
- **Directory**: `Z3{ExpressionType}ExprTests/` (e.g., `Z3BitVecExprTests/`)
- **Test Files**: `Z3{ExpressionType}Expr{Functionality}Tests.cs` (e.g., `Z3BitVecExprArithmeticTests.cs`)
- **Test Classes**: `Z3{ExpressionType}Expr{Functionality}Tests` (matching the file name)

## Files Refactored

### Completed Files (Z3BitVecExprTests/):
- **Z3BitVecExprArithmeticTests.cs**: Add, Sub, Mul, UnsignedDivision, SignedDivision, Neg operations
- **Z3BitVecExprBitwiseTests.cs**: And, Or, Xor, Not operations
- **Z3BitVecExprComparisonTests.cs**: All comparison operations (already complete)
- **Z3BitVecExprShiftTests.cs**: Shift operations (already complete)

### Excluded Files (Not operation-focused):
- Z3BitVecExprCreationTests.cs (creation/validation)
- Z3BitVecEdgeCasesTests.cs (edge cases)
- Z3BitVecExprOverflowTests.cs (overflow detection)
- Z3BitVecExprResizeTests.cs (size manipulation)
- Z3BitVecExprSizeValidationTests.cs (validation)

### Established Pattern (Z3BoolExprTests/):
The Z3BoolExprTests directory already follows this pattern with files like:
- Z3BoolExprLogicalTests.cs
- Z3BoolExprImplicationTests.cs
- Z3BoolExprComparisonTests.cs
- Z3BoolExprConditionalTests.cs
- Z3BoolExprCreationTests.cs
- Z3BoolExprEdgeCasesTests.cs

## Benefits

### 1. Complete API Coverage
Every possible way a user can write an operation is tested:
```csharp
// All these work and are tested:
var result1 = x + y;                    // Natural operator syntax
var result2 = x + bigIntValue;          // Mixed types via operator
var result3 = bigIntValue + y;          // Mixed types via operator
var result4 = x.Add(y);                 // Fluent method syntax
var result5 = x.Add(bigIntValue);       // Fluent method with mixed types
var result6 = context.Add(x, y);        // Context extension syntax
var result7 = context.Add(x, bigIntValue);      // Context extension with mixed types
var result8 = context.Add(bigIntValue, y);      // Context extension with mixed types
```

### 2. Consistency Validation
Ensures all API approaches produce identical results for the same logical operation.

### 3. Regression Prevention
Any change that breaks one approach to performing an operation will be caught immediately.

### 4. Documentation Through Tests
Tests serve as comprehensive examples of every way the API can be used.

## Key Requirements for Future Tests

1. **Always use `new BigInteger(value)`** instead of raw integers for BigInteger parameters
2. **Test all type combinations** (BitVec/BitVec, BitVec/BigInteger, BigInteger/BitVec)
3. **Test all API approaches** (operators, methods, context extensions)
4. **Use descriptive assertion messages** that clearly identify which approach failed
5. **Separate signed/unsigned operations** into different test methods when applicable
6. **Only apply this pattern to operation tests**, not creation/validation/edge-case tests

## Type-Specific Variations

### BitVec Operations (Covered Above)
The 8-variation pattern with BigInteger combinations as described.

### Bool Operations
For Z3BoolExpr operations, the pattern is slightly different due to C# bool integration:

```csharp
var x = context.Bool("x");
var y = context.Bool("y");
var csharpBool1 = true;
var csharpBool2 = false;

// Test all variations of boolean operations
var resultExprExpr = x & y;                          // BoolExpr & BoolExpr (operator)
var resultExprBool = x & csharpBool1;                // BoolExpr & bool (operator)
var resultBoolExpr = csharpBool1 & y;                // bool & BoolExpr (operator)
var resultMethodExpr = x.And(y);                     // BoolExpr.And(BoolExpr) (method)
var resultMethodBool = x.And(csharpBool1);           // BoolExpr.And(bool) (method)
var resultContextExprExpr = context.And(x, y);       // Context.And(BoolExpr, BoolExpr) (method)
var resultContextExprBool = context.And(x, csharpBool1); // Context.And(BoolExpr, bool) (method)
var resultContextBoolExpr = context.And(csharpBool1, y); // Context.And(bool, BoolExpr) (method)
```

**Key Points for Bool Testing:**
- Some operations have explicit **bool overloads** in methods and operators
- Some operations work through **implicit conversion** within `context.SetUp()` scope
- **Both approaches must be tested** to ensure the syntax works for users
- Test with actual C# `bool` values (`true`/`false`), not just expressions

### Int Operations
For Z3IntExpr operations with C# integers:

```csharp
var x = context.Int("x");
var y = context.Int("y");
var csharpInt1 = 42;
var csharpInt2 = 17;
var bigInt = new BigInteger(100);

// Test all variations of integer operations
var resultExprExpr = x + y;                          // IntExpr + IntExpr (operator)
var resultExprInt = x + csharpInt1;                  // IntExpr + int (operator)
var resultIntExpr = csharpInt1 + y;                  // int + IntExpr (operator)
var resultExprBigInt = x + bigInt;                   // IntExpr + BigInteger (operator)
var resultBigIntExpr = bigInt + y;                   // BigInteger + IntExpr (operator)
var resultMethodExpr = x.Add(y);                     // IntExpr.Add(IntExpr) (method)
var resultMethodInt = x.Add(csharpInt1);             // IntExpr.Add(int) (method)
var resultMethodBigInt = x.Add(bigInt);              // IntExpr.Add(BigInteger) (method)
var resultContextExprExpr = context.Add(x, y);       // Context.Add(IntExpr, IntExpr) (method)
var resultContextExprInt = context.Add(x, csharpInt1); // Context.Add(IntExpr, int) (method)
var resultContextIntExpr = context.Add(csharpInt1, y); // Context.Add(int, IntExpr) (method)
var resultContextExprBigInt = context.Add(x, bigInt);  // Context.Add(IntExpr, BigInteger) (method)
var resultContextBigIntExpr = context.Add(bigInt, y);  // Context.Add(BigInteger, IntExpr) (method)
```

### Real Operations
For Z3RealExpr operations with C# numeric types:

```csharp
var x = context.Real("x");
var y = context.Real("y");
var csharpInt = 42;
var csharpDouble = 3.14;
var csharpDecimal = 2.71m;
var bigInt = new BigInteger(100);

// Test all variations of real operations
var resultExprExpr = x + y;                          // RealExpr + RealExpr (operator)
var resultExprInt = x + csharpInt;                   // RealExpr + int (operator)
var resultIntExpr = csharpInt + y;                   // int + RealExpr (operator)
var resultExprDouble = x + csharpDouble;             // RealExpr + double (operator)
var resultDoubleExpr = csharpDouble + y;             // double + RealExpr (operator)
var resultExprDecimal = x + csharpDecimal;           // RealExpr + decimal (operator)
var resultDecimalExpr = csharpDecimal + y;           // decimal + RealExpr (operator)
var resultExprBigInt = x + bigInt;                   // RealExpr + BigInteger (operator)
var resultBigIntExpr = bigInt + y;                   // BigInteger + RealExpr (operator)
var resultMethodExpr = x.Add(y);                     // RealExpr.Add(RealExpr) (method)
var resultMethodInt = x.Add(csharpInt);              // RealExpr.Add(int) (method)
var resultMethodDouble = x.Add(csharpDouble);        // RealExpr.Add(double) (method)
var resultMethodDecimal = x.Add(csharpDecimal);      // RealExpr.Add(decimal) (method)
var resultMethodBigInt = x.Add(bigInt);              // RealExpr.Add(BigInteger) (method)
// ... plus all context extension variations
```

## Why Test C# Native Types

### 1. Explicit Overloads vs Implicit Conversion
Some operations have **explicit overloads** for C# types:
```csharp
public static Z3BoolExpr operator &(Z3BoolExpr left, bool right) // Explicit overload
public Z3IntExpr Add(int value)                                  // Explicit overload
```

Others work through **implicit conversion** within `context.SetUp()` scope:
```csharp
using var scope = context.SetUp();
var result = expr + 42; // Works via implicit conversion of 42 to Z3IntExpr
```

### 2. User Experience Validation
Users expect these syntaxes to work naturally:
```csharp
var condition = boolExpr && true;           // Should work with C# bool
var sum = intExpr + 42;                     // Should work with C# int
var product = realExpr * 3.14;              // Should work with C# double
```

### 3. API Completeness
Testing both approaches ensures:
- **Explicit overloads** work correctly when provided
- **Implicit conversions** work correctly within setup scope
- **All user-expected syntaxes** are validated and supported

### 4. Critical Principle: Test Implicit Conversions Explicitly
Even when operations work through implicit conversion (rather than explicit overloads), **we must test these syntaxes explicitly** to ensure they work reliably for users.

**Example:**
```csharp
// Even if context.Eq(BoolExpr, bool) works through implicit conversion:
var result1 = context.Eq(boolExpr, true);          // MUST be tested
var result2 = context.Eq(true, boolExpr);          // MUST be tested

// This ensures users can write natural syntax confidently:
var equality1 = context.Eq(someExpression, userBoolValue);    // Works
var equality2 = context.Eq(userBoolValue, someExpression);    // Works
```

**Why This Matters:**
- **User confidence**: Users expect natural syntax to work
- **Regression prevention**: Changes to implicit conversion behavior get caught
- **Documentation**: Tests serve as examples of supported syntax
- **Completeness**: Every way a user might write the operation is validated

## Extended Pattern Recognition

**Apply the complete pattern when:**
- Testing BitVec operations (with BigInteger combinations)
- Testing Bool operations (with C# bool combinations)
- Testing Int operations (with int and BigInteger combinations)
- Testing Real operations (with int, double, decimal, and BigInteger combinations)
- Testing any expression operation that supports mixed types

**The key principle:** Test every syntax a user might naturally try, whether it works through explicit overloads or implicit conversions within the context scope.

This comprehensive approach ensures that Z3Wrap provides a consistent, well-tested API where users can choose their preferred syntax (operators, fluent methods, or context extensions) and mix expression types with C# native types naturally and confidently.