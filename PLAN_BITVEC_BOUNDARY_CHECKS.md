# BitVector Boundary Check Enhancement Plan

## Overview

Add a simple fluent builder syntax for bitvector boundary checks.

## Current State

```csharp
var noOverflow = a.AddNoOverflow(b, signed: false);
solver.Assert(noOverflow);
```

## Proposed Solution

### Fluent Builder Syntax

```csharp
var overflowCheck = context.BitVecBoundaryCheck().Add(a, b).NoOverflow();
var underflowCheck = context.BitVecBoundaryCheck().Sub(a, b).NoUnderflow();
var divOverflowCheck = context.BitVecBoundaryCheck().Div(a, b).NoOverflow(signed: true);
var negationCheck = context.BitVecBoundaryCheck().Neg(a).NoOverflow();
```

## Implementation

### BitVecBoundaryCheckBuilder Class
```csharp
public class BitVecBoundaryCheckBuilder
{
    private readonly Z3Context context;

    internal BitVecBoundaryCheckBuilder(Z3Context context)
    {
        this.context = context;
    }

    public BitVecOperationBuilder Add(Z3BitVecExpr left, Z3BitVecExpr right) => new(context, BoundaryOperation.Add, left, right);
    public BitVecOperationBuilder Sub(Z3BitVecExpr left, Z3BitVecExpr right) => new(context, BoundaryOperation.Sub, left, right);
    public BitVecOperationBuilder Mul(Z3BitVecExpr left, Z3BitVecExpr right) => new(context, BoundaryOperation.Mul, left, right);
    public BitVecOperationBuilder Div(Z3BitVecExpr left, Z3BitVecExpr right) => new(context, BoundaryOperation.Div, left, right);
    public BitVecOperationBuilder Neg(Z3BitVecExpr operand) => new(context, BoundaryOperation.Neg, operand, null);
}

public class BitVecOperationBuilder
{
    private readonly Z3Context context;
    private readonly BoundaryOperation operation;
    private readonly Z3BitVecExpr left;
    private readonly Z3BitVecExpr? right;

    internal BitVecOperationBuilder(Z3Context context, BoundaryOperation operation, Z3BitVecExpr left, Z3BitVecExpr? right)
    {
        this.context = context;
        this.operation = operation;
        this.left = left;
        this.right = right;
    }

    public Z3BoolExpr NoOverflow(bool signed = false) { /* implementation */ }
    public Z3BoolExpr NoUnderflow(bool signed = false) { /* implementation */ }
}

internal enum BoundaryOperation
{
    Add,
    Sub,
    Mul,
    Div,
    Neg
}
```

### Context Extension Method
```csharp
public static BitVecBoundaryCheckBuilder BitVecBoundaryCheck(this Z3Context context)
{
    return new BitVecBoundaryCheckBuilder(context);
}
```

### Files to Create
- `Z3Wrap/BoundaryChecks/BitVecBoundaryCheckBuilder.cs` - Main builder classes
- `Z3Wrap/Z3ContextExtensions.BoundaryChecks.cs` - Context extension method

### Usage Example
```csharp
using var context = new Z3Context();
using var scope = context.SetUp();

var a = context.BitVecConst("a", 8);
var b = context.BitVecConst("b", 8);

var addCheck = context.BitVecBoundaryCheck().Add(a, b).NoOverflow();
var divCheck = context.BitVecBoundaryCheck().Div(a, b).NoOverflow(signed: true);

using var solver = context.CreateSolver();
solver.Assert(addCheck);
solver.Assert(divCheck);
```