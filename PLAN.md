# Next Step: Quantifier Foundation - Week 1 Research & Design

## Immediate Action (1-2 days): Research Z3 Quantifier API

### 1. **Understand Z3 Quantifier C API Functions**
Research these specific Z3 functions that need to be added to `NativeMethods.cs`:

```csharp
// Core quantifier functions to research:
Z3_mk_forall       // Create universal quantifier
Z3_mk_exists       // Create existential quantifier
Z3_mk_bound        // Create bound variable
Z3_mk_pattern      // Create quantifier patterns
Z3_mk_patterns     // Create pattern arrays
```

### 2. **Study Z3 Quantifier Examples**
Look at existing Z3 quantifier usage patterns from:
- Z3 Python API examples
- Z3 C++ API documentation
- SMT-LIB2 quantifier syntax
- Academic papers using Z3 quantifiers

### 3. **Analyze Current Extension Architecture**
Study how existing extensions are structured in Z3Wrap (like `Z3ContextExtensions.Bool.cs`).

## Day 1-2 Specific Tasks

### **Task 1A: Research Z3 Quantifier Signatures**
Create a research document answering:
- What are the exact C function signatures for `Z3_mk_forall` and `Z3_mk_exists`?
- What parameters do they take (bound variables, patterns, body)?
- How are bound variables created and managed?
- What are quantifier patterns and when are they needed?

### **Task 1B: Design API Surface**
Design the C# API that would look like:
```csharp
// Target API design to research:
public static Z3Bool ForAll<T>(this Z3Context context,
    Z3Expr<T> boundVar,
    Z3Bool body)

public static Z3Bool Exists<T>(this Z3Context context,
    Z3Expr<T> boundVar,
    Z3Bool body)

// Multi-variable version:
public static Z3Bool ForAll<T1, T2>(this Z3Context context,
    Z3Expr<T1> var1, Z3Expr<T2> var2,
    Z3Bool body)
```

### **Task 1C: Study Pattern Requirements**
Research questions:
- Are patterns required or optional for quantifiers?
- How do patterns affect solver performance?
- What patterns work best for different use cases?
- How should patterns be represented in the C# API?

## Day 3-5: Concrete Implementation Setup

### **Task 2A: Add Native Method Stubs**
Add to `Z3Wrap/Interop/NativeMethods.cs` around line 218:

```csharp
// Quantifier functions - ADD THESE
LoadFunctionInternal(handle, functionPointers, "Z3_mk_forall");
LoadFunctionInternal(handle, functionPointers, "Z3_mk_exists");
LoadFunctionInternal(handle, functionPointers, "Z3_mk_bound");
```

### **Task 2B: Create Quantifier Extension File**
Create new file: `Z3Wrap/Extensions/Z3ContextExtensions.Quantifiers.cs`

### **Task 2C: Write Failing Tests First**
Create: `Z3Wrap.Tests/Unit/Core/QuantifierTests.cs`

Start with simple failing test:
```csharp
[Test]
public void ForAll_IntegerVariable_CreatesQuantifier()
{
    using var context = new Z3Context();
    using var scope = context.SetUp();

    var x = context.IntConst("x");
    var body = x > context.Int(0);

    // This will fail initially - that's expected
    var forall = context.ForAll(x, body);

    Assert.That(forall, Is.Not.Null);
}
```

## Day 6-7: Implementation Planning

### **Task 3A: Bound Variable Strategy**
Decide how to handle bound variables:
- Should they be regular `Z3IntExpr`/`Z3RealExpr` or special `Z3BoundVar<T>`?
- How to prevent bound variables from being used outside quantifier scope?
- How to ensure type safety with bound variables?

### **Task 3B: Implementation Architecture**
Plan the implementation files:
- `Z3ContextExtensions.Quantifiers.cs` - Main quantifier methods
- `Z3BoundVariable.cs` - Bound variable wrapper (if needed)
- `QuantifierPattern.cs` - Pattern management (if needed)

## Exact Command Sequence

```bash
# 1. Research Z3 quantifier documentation
# (manual web research - no command)

# 2. Create test file first (TDD approach)
touch Z3Wrap.Tests/Unit/Core/QuantifierTests.cs

# 3. Create extension file
touch Z3Wrap/Extensions/Z3ContextExtensions.Quantifiers.cs

# 4. Run tests to see failing state
make test

# 5. Begin implementation iteratively
```

## Success Criteria for This Step

1. **Understanding**: Clear comprehension of Z3 quantifier API requirements
2. **Design**: Documented C# API design that maintains Z3Wrap's type safety patterns
3. **Infrastructure**: Test file and extension file created with basic structure
4. **Validation**: Failing tests that define expected behavior

This focused approach ensures you understand the quantifier requirements thoroughly before writing code, following Z3Wrap's existing patterns and maintaining the project's high quality standards.